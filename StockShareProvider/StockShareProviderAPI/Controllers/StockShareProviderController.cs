using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace StockShareProviderAPI.Controllers
{
    public class StockShareProviderController : Controller
    {
        [HttpPost("CreateAvailableShares/{stockId}")]
        public void CreateAvailableShares(string stockId, [FromBody] Guid userId, int sharesAmount)
        {
            using (AvailableSharesContext context = new AvailableSharesContext(options))
            {
                context.Add(new AvailableSharesDataModel { StockId = stockId, StockOwner = userId, SharesAmount = sharesAmount});
                context.SaveChanges();
            }
        }

        [HttpPut("IncreaseSharesAmountForSale/{stockId}")]
        public void IncreaseSharesAmountForSale(string stockId, [FromBody] Guid userId, int sharesAmount)
        {
            using (AvailableSharesContext context = new AvailableSharesContext(options))
            {
                var selectedStock = context.AvailableSharesDataModel.Where(x => x.StockId.Equals(stockId)).Single();

                if (selectedStock.StockOwner.Equals(userId))
                {   
                        selectedStock.SharesAmount += sharesAmount;
                }
                context.Update(selectedStock);
                context.SaveChanges();
            }
        }

        [HttpPut("DescreaseSharesAmountForSale/{stockId}")]
        public void DecreaseSharesAmountForSale(string stockId, [FromBody] Guid userId, int sharesAmount)
        {
            using (AvailableSharesContext context = new AvailableSharesContext(options))
            {

                var selectedStock = context.AvailableSharesDataModel.Where(x => x.StockId.Equals(stockId)).Single();

                if (selectedStock.StockOwner.Equals(userId))
                {
                    selectedStock.SharesAmount -= sharesAmount;
                    if(selectedStock.SharesAmount < 0) { return; }
                }
                context.Update(selectedStock);
                context.SaveChanges();
            }
        }

        [HttpGet("GetSharesForSale/{stockId}")]
        public AvailableSharesDataModel GetSharesForSale(string stockId)
        {
            using (AvailableSharesContext context = new AvailableSharesContext(options))
            {
                AvailableSharesDataModel sharesForSale = new AvailableSharesDataModel();
                var selectedStock = context.AvailableSharesDataModel.Where(x => x.StockId.Equals(stockId)).SingleOrDefault();

                if (selectedStock != null)
                {
                    sharesForSale = selectedStock;
                }

                return sharesForSale; 
            }
        }

        private DbContextOptions<AvailableSharesContext> options = new DbContextOptionsBuilder<AvailableSharesContext>()
           .UseInMemoryDatabase(databaseName: "AvailableSharesDb")
           .Options;
    }
}
