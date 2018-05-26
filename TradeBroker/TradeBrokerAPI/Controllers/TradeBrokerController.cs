using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using TradeBrokerAPI.Clients;

namespace TradeBrokerAPI.Controllers
{
    [Route("api/[controller]")]
    public class TradeBrokerController : Controller
    {
        [HttpPost("InitiateTrade/{stockId}/{sharesAmount}")]
        public void InitiateTrade(string stockId, int sharesAmount, [FromBody]Guid requesterId)
        {
            var stockShareProviderClient = new StockShareProviderClient("http://localhost:8748");
            var paymentControlClient = new PaymentControlClient("http://localhost:8965");
            var shareOwnerControlClient = new ShareOwnerControlClient("http://localhost:8758");

            var numberOfAvailableShares = 0;
            var chosenShareProviders = new List<string>();
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
            var reservedShares = 0;
            var index = 0;

            while (reservedShares < sharesAmount)
            {
                chosenShareProviders.Add(availableShares[index].StockOwner.Value.ToString());

                if (availableShares[index].SharesAmount <= (sharesAmount - reservedShares))
                {
                    reservedShares += (int)availableShares[index].SharesAmount;

                    stockShareProviderClient.ApiStockShareProviderDecreaseSharesAmountForSaleByStockIdPutAsync(stockId,
                        availableShares[index].StockOwner,
                        availableShares[index].SharesAmount);

                    paymentsToProcess.Add(new PaymentDataModel { CounterParty = availableShares[index].StockOwner, PaymentType = "Payment", PaymentAmount = (stockInfo.SharePrice * availableShares[index].SharesAmount) });
                    paymentsToProcess.Add(new PaymentDataModel { CounterParty = requesterId, PaymentType = "Invoice", PaymentAmount = (stockInfo.SharePrice * availableShares[index].SharesAmount) });
                }
                else
                {
                    stockShareProviderClient.ApiStockShareProviderDecreaseSharesAmountForSaleByStockIdPutAsync(stockId,
                        availableShares[index].StockOwner,
                        (sharesAmount - reservedShares));

                    paymentsToProcess.Add(new PaymentDataModel { CounterParty = availableShares[index].StockOwner, PaymentType = "Payment", PaymentAmount = (sharesAmount - reservedShares) });
                    paymentsToProcess.Add(new PaymentDataModel { CounterParty = requesterId, PaymentType = "Invoice", PaymentAmount = (sharesAmount - reservedShares) * stockInfo.SharePrice });

                    reservedShares = sharesAmount;
                }

                index++;
            }

            foreach (var payment in paymentsToProcess)
            {
                paymentControlClient.ApiPaymentCreatePaymentInfoByPaymentTypeByPaymentAmountPostAsync(payment.PaymentType, payment.PaymentAmount.Value, payment.CounterParty);
            }

            using (var context = new TradeLogContext(Options))
            {
                var value = new TradeDataModel
                {
                    StockId = stockId,
                    SharesAmount = sharesAmount,
                    StockRequester = requesterId,
                    StockProviders = JsonConvert.SerializeObject(chosenShareProviders)
                };
                var tradeData = JsonConvert.SerializeObject(value);
                context.Add(tradeData);
                context.SaveChanges();
                //TODO: UDSKIFT TIL NORMAL DATABASE UDEN ENTITYFRAMEWORKS. UNDERLIG STRING FEJL HER
            }

            foreach (var payment in paymentsToProcess)
            {
                if (payment.PaymentType.Equals("Payment"))
                {
                    shareOwnerControlClient.ApiShareOwnerUpdateShareOwnershipByStockIdPutAsync(stockId, requesterId,
                        payment.CounterParty, payment.PaymentAmount / stockInfo.SharePrice);
                }
            }
        }

        [HttpGet("GetTradeData/{tradeDataId}")]
        public TradeDataModel GetTradeData(int tradeDataId)
        {
            using (var context = new TradeLogContext(Options))
            {
                return JsonConvert.DeserializeObject<TradeDataModel>(context.TradeData.SingleOrDefault(x => x.Id == tradeDataId)?.ToString());
            }
        }

        public DbContextOptions<TradeLogContext> Options = new DbContextOptionsBuilder<TradeLogContext>()
                .UseInMemoryDatabase(databaseName: "TradeLogDb")
                .Options;
    }
}
