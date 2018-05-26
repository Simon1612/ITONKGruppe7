using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TradeBrokerAPI.Clients;

namespace TradeBrokerAPI.Controllers
{
    [Route("api/[controller]")]
    public class TradeBrokerController : Controller
    {
        [HttpPost("InitiateTrade/{stockId}{sharesAmount}")]
        public void InitiateTrade(string stockId, int sharesAmount, [FromBody]Guid requesterId)
        {
            var stockShareProviderClient = new StockShareProviderClient("http://localhost:8748");
            var paymentControlClient = new PaymentControlClient("http://localhost:8965");

            int numberOfAvailableShares = 0;
            List<Guid> chosenShareProviders = new List<Guid>();
            List<PaymentDataModel> paymentsToProcess = new List<PaymentDataModel>();

            var availableShares = stockShareProviderClient.ApiStockShareProviderGetSharesForSaleByStockIdGetAsync(stockId).Result;

            foreach (var share in availableShares)
            {
                numberOfAvailableShares += (int)share.SharesAmount;
            }

            if (sharesAmount <= numberOfAvailableShares)
            {
                int reservedShares = 0;
                int index = 0;

                while (reservedShares < sharesAmount)
                {
                    chosenShareProviders.Add((Guid)availableShares[index].StockOwner);

                    if (availableShares[index].SharesAmount <= (sharesAmount - reservedShares))
                    {
                        reservedShares += (int)availableShares[index].SharesAmount;

                        stockShareProviderClient.ApiStockShareProviderDecreaseSharesAmountForSaleByStockIdPutAsync(stockId,
                            availableShares[index].StockOwner,
                            availableShares[index].SharesAmount);

                        //TODO: Get prices from shareownercontrol * shares reserved instead of 0
                        paymentsToProcess.Add(new PaymentDataModel { CounterParty = availableShares[index].StockOwner, PaymentType = "Payment", PaymentAmount = 0 });
                        paymentsToProcess.Add(new PaymentDataModel { CounterParty = requesterId, PaymentType = "Invoice", PaymentAmount = 0 });
                    }
                    else
                    {
                        stockShareProviderClient.ApiStockShareProviderDecreaseSharesAmountForSaleByStockIdPutAsync(stockId,
                            availableShares[index].StockOwner,
                            (sharesAmount - reservedShares));

                        //TODO: Get prices from shareownercontrol * shares reserved instead of 0
                        paymentsToProcess.Add(new PaymentDataModel { CounterParty = availableShares[index].StockOwner, PaymentType = "Payment", PaymentAmount = 0 });
                        paymentsToProcess.Add(new PaymentDataModel { CounterParty = requesterId, PaymentType = "Invoice", PaymentAmount = 0 });

                        reservedShares = sharesAmount;
                    }

                    index++;
                }

                foreach (var payment in paymentsToProcess)
                {
                    paymentControlClient.ApiPaymentCreatePaymentInfoByPaymentTypeByPaymentAmountPostAsync(payment.PaymentType, (int)payment.PaymentAmount, payment.CounterParty);
                }

                using (TradeLogContext context = new TradeLogContext(options))
                {
                    context.Add(new TradeDataModel { StockId = stockId, SharesAmount = sharesAmount, StockRequester = requesterId, StockProviders = chosenShareProviders });
                    context.SaveChanges();
                }

                //TODO: Kald ShareOwnerControl og tildel sharesAmount af stockId til requesterId
            }
        }

        [HttpGet("GetTradeData/{tradeDataId}")]
        public TradeDataModel GetTradeData(int tradeDataId)
        {
            var options = new DbContextOptionsBuilder<TradeLogContext>()
                .UseInMemoryDatabase(databaseName: "TradeLogDb")
                .Options;
            using (TradeLogContext context = new TradeLogContext(options))
            {
                TradeDataModel model;
                model = context.Find<TradeDataModel>(tradeDataId);
                return model;
            }
        }

        public DbContextOptions<TradeLogContext> options = new DbContextOptionsBuilder<TradeLogContext>()
                .UseInMemoryDatabase(databaseName: "TradeLogDb")
                .Options;
    }
}
