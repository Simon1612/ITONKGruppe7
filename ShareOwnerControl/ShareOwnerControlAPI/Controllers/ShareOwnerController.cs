using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ShareOwnerControlAPI.Controllers
{
    [Route("api/[controller]")]
    public class ShareOwnerController : Controller
    {
        // GET api/values/5
        [HttpGet("GetStockInfo/{stockId}")]
        public string GetStockInfo(string stockId)
        {
            using (ShareOwnerContext context = new ShareOwnerContext(options))
            {
                return "stuff";
            }
        }

        [HttpGet("VerifyShareOwnership/{stockId}")]
        public string VerifyShareOwnership(string stockId, [FromBody]Guid userId, int sharesAmount)
        {
            using (ShareOwnerContext context = new ShareOwnerContext(options))
            {
                return "stuff";
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
