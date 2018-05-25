using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using ShareOwnerControlAPI.Models;

namespace ShareOwnerControlAPI.Controllers
{
    [Route("api/[controller]")]
    public class ShareOwnerController : Controller
    {
        // GET api/values/5
        [HttpGet("GetStockInfo/{stockId}")]
        public ShareDataModel GetStockInfo(string stockId)
        {
            using (ShareOwnerContext context = new ShareOwnerContext(options))
            {
                var stockInfo = context.ShareDataModel
                                       .Where(x => x.StockId.Equals(stockId))
                                       .SingleOrDefault();

                if (!stockInfo.Equals(null))
                {
                    return stockInfo;
                } else
                {
                    return new ShareDataModel(); //Ikke sikker på hvad default retur-værdi ellers skal være
                }
            }
        }

        [HttpGet("VerifyShareOwnership/{stockId}")]
        public bool VerifyShareOwnership(string stockId, [FromBody]Guid userId, int sharesAmount)
        {
            using (ShareOwnerContext context = new ShareOwnerContext(options))
            {
                var shareOwnerData = context.ShareOwnerDataModel
                                        .Where(x => x.ShareOwner.Equals(userId))
                                        .Where(y => y.StockId.Equals(stockId))
                                        .SingleOrDefault();
                //Validerer på om useren ejer de assigned stocks og om antallet passer og returnere en bool baseret på dette
                if (!shareOwnerData.Equals(null))
                {
                    if(shareOwnerData.SharesAmount >= sharesAmount)
                    {
                        return true;
                    }
                    return false;
                }
                return false;
            }
        }

        // PUT api/values/5
        [HttpPut("UpdateShareOwnership/{stockId}")]
        public void UpdateShareOwnership(string stockId, [FromBody]Guid requester, Guid provider, int sharesAmount)
        {
            //Check if requester have relation to stock. If yes´, update amount with old + new. If no, create new relation.
            //Subtract sharesAmount from providers relation to stock. If 0, delete relation.
            using (ShareOwnerContext context = new ShareOwnerContext(options))
            {
                if(VerifyShareOwnership(stockId, provider, sharesAmount))
                {
                    var newShareOwnerData = context.ShareOwnerDataModel
                                            .Where(x => x.ShareOwner.Equals(requester))
                                            .Where(y => y.StockId.Equals(stockId))
                                            .SingleOrDefault();

                    var oldShareOwnerData = context.ShareOwnerDataModel
                                            .Where(x => x.ShareOwner.Equals(provider))
                                            .Where(y => y.StockId.Equals(stockId))
                                            .SingleOrDefault();
                    if(!oldShareOwnerData.Equals(null))
                    { 
                        if(!(oldShareOwnerData.SharesAmount < sharesAmount))
                        { 
                            if (newShareOwnerData.Equals(null))
                            {
                                context.Add(new ShareOwnerDataModel { ShareOwner = requester, SharesAmount = sharesAmount, StockId = stockId });
                            }
                            else {
                                newShareOwnerData.SharesAmount += sharesAmount;
                                context.Update(newShareOwnerData);
                            }
                            oldShareOwnerData.SharesAmount -= sharesAmount;
                            context.Update(oldShareOwnerData);
                            context.SaveChanges();
                        }
                    }
                    return;
                }
            }
        }

        [HttpPost("CreateShareOwnership/{stockId}{sharesAmount}")]
        public void CreateShareOwnership(string stockId, [FromBody]Guid userId, int sharesAmount)
        {
            //Create new user if not present 
            //Create new stock if not present
            //Create relation between user and stock
            using (ShareOwnerContext context = new ShareOwnerContext(options))
            {
            }
        }

        public DbContextOptions<ShareOwnerContext> options = new DbContextOptionsBuilder<ShareOwnerContext>()
                .UseInMemoryDatabase(databaseName: "ShareOwnerDb")
                .Options;
    }
}
