using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using ServiceStack.OrmLite;
using TradeBrokerAPI.Clients;

namespace TradeBrokerAPI.Controllers
{
    [Route("api/[controller]")]
    public class TradeBrokerController : Controller
    {
        private const string ConnectionString = "Data Source=(localdb)\\.\\SharedDB;Initial Catalog=TradeBrokerDb;Integrated Security=SSPI;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

        [HttpPost("InitiateTrade/{stockId}/{sharesAmount}")]
        public void InitiateTrade(string stockId, int sharesAmount, [FromBody]Guid requesterId)
        {
            var stockShareProviderClient = new StockShareProviderClient("http://localhost:8748");
            var paymentControlClient = new PaymentControlClient("http://localhost:8965");
            var shareOwnerControlClient = new ShareOwnerControlClient("http://localhost:8758");
            var tobinTaxClient = new TobinTaxClient("http://localhost:8132");

            var numberOfAvailableShares = 0;
            var chosenShareProviders = new List<Guid>();
            var paymentsToProcess = new List<PaymentDataModel>();

            var availableShares = stockShareProviderClient.ApiStockShareProviderGetSharesForSaleByStockIdGetAsync(stockId).Result;
            var stockInfo = shareOwnerControlClient.ApiShareOwnerGetStockInfoByStockIdGetAsync(stockId).Result;

            foreach (var share in availableShares)
            {
                if (share.SharesAmount != null)
                    numberOfAvailableShares += share.SharesAmount.Value;
                else
                {
                    BadRequest();
                }
            }

            if (sharesAmount > numberOfAvailableShares) return;
            var reservedSharesAmount = 0;
            var index = 0;

            while (reservedSharesAmount < sharesAmount)
            {
                chosenShareProviders.Add(availableShares[index].StockOwner.Value);

                if (availableShares[index].SharesAmount <= (sharesAmount - reservedSharesAmount))
                {
                    reservedSharesAmount += (int)availableShares[index].SharesAmount;

                    stockShareProviderClient.ApiStockShareProviderDecreaseSharesAmountForSaleByStockIdPutAsync(stockId,
                        availableShares[index].StockOwner,
                        availableShares[index].SharesAmount);

                    tobinTaxClient.ApiTobinTaxPostAsync(stockInfo.SharePrice * availableShares[index].SharesAmount,
                        availableShares[index].StockOwner);

                    var afterTax = ((stockInfo.SharePrice * availableShares[index].SharesAmount) /100)*99;

                    paymentsToProcess.Add(new PaymentDataModel { CounterParty = availableShares[index].StockOwner, PaymentType = "Payment", PaymentAmount = afterTax });
                    paymentsToProcess.Add(new PaymentDataModel { CounterParty = requesterId, PaymentType = "Invoice", PaymentAmount = (stockInfo.SharePrice * availableShares[index].SharesAmount) });
                }
                else
                {
                    stockShareProviderClient.ApiStockShareProviderDecreaseSharesAmountForSaleByStockIdPutAsync(stockId,
                        availableShares[index].StockOwner,
                        (sharesAmount - reservedSharesAmount));

                    tobinTaxClient.ApiTobinTaxPostAsync((sharesAmount - reservedSharesAmount) * stockInfo.SharePrice,
                        availableShares[index].StockOwner);

                    var afterTax = ((sharesAmount - reservedSharesAmount) * stockInfo.SharePrice / 100) * 99;

                    paymentsToProcess.Add(new PaymentDataModel { CounterParty = availableShares[index].StockOwner, PaymentType = "Payment", PaymentAmount = afterTax });
                    paymentsToProcess.Add(new PaymentDataModel { CounterParty = requesterId, PaymentType = "Invoice", PaymentAmount = (sharesAmount - reservedSharesAmount) * stockInfo.SharePrice });

                    reservedSharesAmount = sharesAmount;
                }

                index++;
            }

            foreach (var payment in paymentsToProcess)
            {
                paymentControlClient.ApiPaymentCreatePaymentInfoByPaymentTypeByPaymentAmountPostAsync(payment.PaymentType, payment.PaymentAmount.Value, payment.CounterParty);
            }

            var dbFactory = new OrmLiteConnectionFactory(
                ConnectionString,
                SqlServerDialect.Provider);


            using (var db = dbFactory.Open())
            {
                db.CreateTableIfNotExists<TradeDataModel>();

                var value = new TradeDataModel
                {
                    StockId = stockId,
                    SharesAmount = sharesAmount,
                    StockRequester = requesterId,
                    StockProviders = chosenShareProviders
                };

                db.Insert(value);
            }

            foreach (var payment in paymentsToProcess)
            {
                if (payment.PaymentType.Equals("Payment"))
                {
                    shareOwnerControlClient.ApiShareOwnerUpdateShareOwnershipByStockIdPutAsync(stockId, requesterId,
                        payment.CounterParty, payment.PaymentAmount / (stockInfo.SharePrice));
                }
            }
        }

        [HttpGet("GetTradeData/{tradeDataId}")]
        public TradeDataModel GetTradeData(int tradeDataId)
        {
            var dbFactory = new OrmLiteConnectionFactory(
                ConnectionString,
                SqlServerDialect.Provider);

            using (var db = dbFactory.Open())
            {
                return db.Single<TradeDataModel>(x => x.Id == tradeDataId);
            }
        }
    }
}
