using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace TradeBrokerAPI.Controllers
{
    [Route("api/[controller]")]
    public class TradeBrokerController : Controller
    {
        [HttpPost]
        public void InitiateTrade(string stockId, int sharesAmount, [FromBody]string requester)
        {

            //Modtag request fra StockShareRequester -> InitiateTrade kaldes fra StockShareRequesteren
            //Request AvailableShares for stockName fra StockShareProvider
            //If(requestedShares <= AvaiableShares) notify ShareOwnerControl
            //Modtag svar fra ShareOwnerControl
            //If(trade == succesful)
            var options = new DbContextOptionsBuilder<TradeLogContext>()
                .UseInMemoryDatabase(databaseName: "TradeLogDb")
                .Options;
            using (TradeLogContext context = new TradeLogContext(options))
            {
                context.Add(new TradeDataModel { StockId = stockId, SharesAmount = sharesAmount, StockRequester = new Guid(requester), StockProvider = new Guid()});
                context.SaveChanges();
            }
        }

        [HttpGet]
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
    }
}
