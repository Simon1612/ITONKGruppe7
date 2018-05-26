using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using ShareOwnerControlAPI.Models;
using System.Collections.Generic;

namespace ShareOwnerControlAPI.Controllers
{
    [Route("api/[controller]")]
    public class ShareOwnerController : Controller
    {
        [HttpGet("GetStockInfo/{stockId}")]
        public ShareDataModel GetStockInfo(string stockId)
        {
            using (var context = new ShareOwnerContext(Options))
            {
                return context.ShareDataModel
                              .SingleOrDefault(x => x.StockId.Equals(stockId));
            }
        }

        [HttpGet("GetAllSharesForUser/{userId}")]
        public List<ShareOwnerDataModel> GetAllSharesForUser(Guid userId)
        {
            using (var context = new ShareOwnerContext(Options))
            {
                return context.ShareOwnerDataModel
                    .Where(shareOwnerModel => shareOwnerModel.ShareOwner.ShareHolderId.Equals(userId))
                    .Include(share => share.Stock)
                    .Include(user => user.ShareOwner)
                    .ToListAsync().Result;
            }
        }

        [HttpGet("VerifyShareOwnership/{stockId}")]
        public bool VerifyShareOwnership(string stockId, Guid userId, int sharesAmount)
        {
            using (var context = new ShareOwnerContext(Options))
            {
                var shareOwnerData = context.ShareOwnerDataModel
                                        .SingleOrDefault(x => x.ShareOwner.ShareHolderId.Equals(userId) && x.Stock.StockId.Equals(stockId));

                if (shareOwnerData == null) return false;
                return shareOwnerData.SharesAmount >= sharesAmount;
            }
        }

        [HttpPut("UpdateShareOwnership/{stockId}")]
        public void UpdateShareOwnership(string stockId, [FromBody]Guid requester, Guid provider, int sharesAmount)
        {
            using (var context = new ShareOwnerContext(Options))
            {
                if (!VerifyShareOwnership(stockId, provider, sharesAmount)) return;
                var newShareOwnerData = context.ShareOwnerDataModel
                    .SingleOrDefault(x => x.ShareOwner.ShareHolderId.Equals(requester) && x.Stock.StockId.Equals(stockId));

                var oldShareOwnerData = context.ShareOwnerDataModel
                    .SingleOrDefault(x => x.ShareOwner.ShareHolderId.Equals(provider) && x.Stock.StockId.Equals(stockId));

                if (oldShareOwnerData == null) return;
                if (oldShareOwnerData.SharesAmount < sharesAmount) return;

                if (newShareOwnerData == null)
                {
                    var shareOwner = context.OwnerDataModel
                        .Single(x => x.ShareHolderId.Equals(requester));

                    var stock = context.ShareDataModel
                        .Single(x => x.StockId.Equals(stockId));

                    context.ShareOwnerDataModel.Add(new ShareOwnerDataModel { ShareOwner = shareOwner, SharesAmount = sharesAmount, Stock = stock });
                }
                else
                {
                    newShareOwnerData.SharesAmount += sharesAmount;
                    context.ShareOwnerDataModel.Update(newShareOwnerData);
                }

                oldShareOwnerData.SharesAmount -= sharesAmount;

                context.ShareOwnerDataModel.Update(oldShareOwnerData);
                context.SaveChanges();
            }
        }

        [HttpPost("CreateShareOwnership/{stockId}/{sharesAmount}")]
        public void CreateShareOwnership(string stockId, [FromBody]Guid userId, int sharesAmount)
        {
            using (var context = new ShareOwnerContext(Options))
            {
                var user = context.OwnerDataModel
                                  .SingleOrDefault(x => x.ShareHolderId.Equals(userId));

                var stock = context.ShareDataModel
                                   .SingleOrDefault(x => x.StockId.Equals(stockId));

                if (user == null)
                {
                    user = new OwnerDataModel() { ShareHolderId = userId };
                    context.OwnerDataModel.Add(user);
                }

                if (stock == null)
                {
                    stock = new ShareDataModel()
                    {
                        StockId = stockId,
                    };
                    context.ShareDataModel.Add(stock);
                }

                context.ShareOwnerDataModel.Add(new ShareOwnerDataModel()
                {
                    Stock = stock,
                    SharesAmount = sharesAmount,
                    ShareOwner = user
                });
                context.SaveChanges();
            }
        }

        public DbContextOptions<ShareOwnerContext> Options = new DbContextOptionsBuilder<ShareOwnerContext>()
                .UseInMemoryDatabase(databaseName: "ShareOwnerDb")
                .Options;
    }
}
