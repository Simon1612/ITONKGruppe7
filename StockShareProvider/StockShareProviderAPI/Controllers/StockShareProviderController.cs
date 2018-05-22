using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace StockShareProviderAPI.Controllers
{
    public class StockShareProviderController : Controller
    {
        [HttpPost("{stockId}")]
        public void CreateAvailableShares(string stockId, [FromBody] Guid userId, int sharesAmount)
        {
            using (AvailableSharesContext context = new AvailableSharesContext(options))
            {
                context.Add(new AvailableSharesDataModel { StockId = stockId, StockOwner = userId, SharesAmount = sharesAmount});
                context.SaveChanges();
            }
        }

        [HttpPut("{stockId}")]
        public void IncreaseSharesAmountForSale(string stockId, [FromBody] Guid userId, int sharesAmount)
        {
            using (AvailableSharesContext context = new AvailableSharesContext(options))
            {
                var selectedStock = from x in context.AvailableSharesDataModel
                                                         where x.StockId.Equals(stockId)
                                                         select x;

                AvailableSharesDataModel stock = selectedStock.SingleOrDefault();

                if (selectedStock.Select(x => x.StockOwner).Equals(userId))
                {   
                        stock.SharesAmount += sharesAmount;
                }
                context.Update(stock);
                context.SaveChanges();
            }
        }

        [HttpPut("{stockId}")]
        public void DecreaseSharesAmountForSale(string stockId, [FromBody] Guid userId, int sharesAmount)
        {
            using (AvailableSharesContext context = new AvailableSharesContext(options))
            {
                var selectedStock = from x in context.AvailableSharesDataModel
                                    where x.StockId.Equals(stockId)
                                    select x;

                AvailableSharesDataModel stock = selectedStock.SingleOrDefault();

                if (selectedStock.Select(x => x.StockOwner).Equals(userId))
                {
                    stock.SharesAmount -= sharesAmount;
                }
                context.Update(stock);
                context.SaveChanges();
            }
        }

        [HttpGet("{stockId}")]
        public AvailableSharesDataModel GetSharesForSale(string stockId)
        {
            using (AvailableSharesContext context = new AvailableSharesContext(options))
            {
                var selectedStock = from x in context.AvailableSharesDataModel
                                    where x.StockId.Equals(stockId)
                                    select x;

                AvailableSharesDataModel sharesForSale = new AvailableSharesDataModel();

                if (selectedStock.Single() != null)
                {
                    sharesForSale = selectedStock.Single();
                }

                return sharesForSale; 
            }
        }

        public DbContextOptions<AvailableSharesContext> options = new DbContextOptionsBuilder<AvailableSharesContext>()
                .UseInMemoryDatabase(databaseName: "AvailableSharesDb")
                .Options;
    }
}
