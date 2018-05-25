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
            using (ShareOwnerContext context = new ShareOwnerContext(options))
            {
                return context.ShareDataModel
                              .Where(x => x.StockId.Equals(stockId))
                              .SingleOrDefault();
            }
        }

        [HttpGet("GetAllSharesForUser/{userId}")]
        public List<ShareOwnerDataModel> GetAllSharesForUser(Guid userId)
        {
            using (ShareOwnerContext context = new ShareOwnerContext(options))
            {
                var user = context.OwnerDataModel.Where(x => x.ShareHolderId.Equals(userId)).Single();
                var shareOwner = context.ShareOwnerDataModel
                              .Where(x => x.ShareOwner.Equals(user))
                              .ToList();

                return shareOwner;
            }
        }

        [HttpGet("VerifyShareOwnership/{stockId}")]
        public bool VerifyShareOwnership(string stockId, Guid userId, int sharesAmount)
        {
            using (ShareOwnerContext context = new ShareOwnerContext(options))
            {
                var shareOwnerData = context.ShareOwnerDataModel
                                        .Where(x => x.ShareOwner.ShareHolderId.Equals(userId) && x.Stock.StockId.Equals(stockId))
                                        .SingleOrDefault();

                if (shareOwnerData != null)
                {
                    if(shareOwnerData.SharesAmount >= sharesAmount)
                    {
                        return true;
                    }
                }
                return false;
            }
        }
        
        [HttpPut("UpdateShareOwnership/{stockId}")]
        public void UpdateShareOwnership(string stockId, [FromBody]Guid requester, Guid provider, int sharesAmount)
        {
            using (ShareOwnerContext context = new ShareOwnerContext(options))
            {
                if(VerifyShareOwnership(stockId, provider, sharesAmount))
                {
                    var newShareOwnerData = context.ShareOwnerDataModel
                                                   .Where(x => x.ShareOwner.ShareHolderId.Equals(requester) && x.Stock.StockId.Equals(stockId))
                                                   .SingleOrDefault();

                    var oldShareOwnerData = context.ShareOwnerDataModel
                                                   .Where(x => x.ShareOwner.ShareHolderId.Equals(provider) && x.Stock.StockId.Equals(stockId))
                                                   .SingleOrDefault();

                    if(oldShareOwnerData != null)
                    { 
                        if(!(oldShareOwnerData.SharesAmount < sharesAmount))
                        { 
                            if (newShareOwnerData == null)
                            {
                                var shareOwner = context.OwnerDataModel
                                                        .Where(x => x.ShareHolderId.Equals(requester))
                                                        .Single();

                                var stock = context.ShareDataModel
                                                   .Where(x => x.StockId.Equals(stockId))
                                                   .Single();

                                context.ShareOwnerDataModel.Add(new ShareOwnerDataModel { ShareOwner = shareOwner, SharesAmount = sharesAmount, Stock = stock });
                            }
                            else {
                                newShareOwnerData.SharesAmount += sharesAmount;
                                context.ShareOwnerDataModel.Update(newShareOwnerData);
                            }

                            oldShareOwnerData.SharesAmount -= sharesAmount;

                            context.ShareOwnerDataModel.Update(oldShareOwnerData);
                            context.SaveChanges();
                        }
                    }
                }
            }
        }

        [HttpPost("CreateShareOwnership/{stockId}/{sharesAmount}")]
        public void CreateShareOwnership(string stockId, [FromBody]Guid userId, int sharesAmount)
        {
            using (ShareOwnerContext context = new ShareOwnerContext(options))
            {
                var user = context.OwnerDataModel
                                  .Where(x => x.ShareHolderId.Equals(userId))
                                  .SingleOrDefault();

                var stock = context.ShareDataModel
                                   .Where(x => x.StockId.Equals(stockId))
                                   .SingleOrDefault();
                
                if (user == null)
                {
                    user = new OwnerDataModel() { ShareHolderId = userId };
                    context.OwnerDataModel.Add(user);
                }
                
                if(stock == null)
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

        public DbContextOptions<ShareOwnerContext> options = new DbContextOptionsBuilder<ShareOwnerContext>()
                .UseInMemoryDatabase(databaseName: "ShareOwnerDb")
                .Options;
    }
}
