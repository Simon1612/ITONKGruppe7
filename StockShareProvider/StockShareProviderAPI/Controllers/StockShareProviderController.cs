using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Collections.Generic;

namespace StockShareProviderAPI.Controllers
{
    [Route("api/[controller]")]
    public class StockShareProviderController : Controller
    {
        [HttpPost("CreateAvailableShares/{stockId}")]
        public void CreateAvailableShares(string stockId, [FromBody] Guid userId, int sharesAmount)
        {
            if (stockId != null && userId != null)
            {
                using (AvailableSharesContext context = new AvailableSharesContext(options))
                {
                    context.Add(new AvailableSharesDataModel { StockId = stockId, StockOwner = userId, SharesAmount = sharesAmount });
                    context.SaveChanges();
                }
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

        [HttpPut("DecreaseSharesAmountForSale/{stockId}")]
        public void DecreaseSharesAmountForSale(string stockId, [FromBody] Guid userId, int sharesAmount)
        {
            using (AvailableSharesContext context = new AvailableSharesContext(options))
            {
                var selectedStock = context.AvailableSharesDataModel.Where(x => x.StockId.Equals(stockId)).Single();

                if (selectedStock.StockOwner.Equals(userId))
                {
                    selectedStock.SharesAmount -= sharesAmount;

                    if (selectedStock.SharesAmount < 0)
                        return;

                    if (selectedStock.SharesAmount == 0)
                        context.Remove(selectedStock);
                    else
                        context.Update(selectedStock);
                }
                
                context.SaveChanges();
            }
        }

        [HttpGet("GetSharesForSale/{stockId}")]
        public List<AvailableSharesDataModel> GetSharesForSale(string stockId)
        {
            using (AvailableSharesContext context = new AvailableSharesContext(options))
            {
                return context.AvailableSharesDataModel.Where(x => x.StockId.Equals(stockId)).ToList();
            }
        }

        [HttpGet]
        public List<AvailableSharesDataModel> GetAllSharesForSale()
        {
            using (AvailableSharesContext context = new AvailableSharesContext(options))
            {
                return context.AvailableSharesDataModel.ToList();
            }
        }

        private DbContextOptions<AvailableSharesContext> options = new DbContextOptionsBuilder<AvailableSharesContext>()
           .UseInMemoryDatabase(databaseName: "AvailableSharesDb")
           .Options;
    }
}
