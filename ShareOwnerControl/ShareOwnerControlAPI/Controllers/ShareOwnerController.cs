﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ShareOwnerControlAPI.Controllers
{
    [Route("api/[controller]")]
    public class ShareOwnerController : Controller
    {
        // GET api/values/5
        [HttpGet("{stockId}")]
        public string GetStockInfo(string stockId)
        {
            using (ShareOwnerContext context = new ShareOwnerContext(options))
            {
                return "stuff";
            }
        }

        [HttpGet("{stockId}")]
        public string VerifyShareOwnership(string stockId, [FromBody]Guid userId, [FromBody]int sharesAmount)
        {
            using (ShareOwnerContext context = new ShareOwnerContext(options))
            {
                return "stuff";
            }
        }

        // PUT api/values/5
        [HttpPut("{stockId}")]
        public void UpdateShareOwnership(string stockId, [FromBody]Guid requester, [FromBody]Guid provider, [FromBody]int sharesAmount)
        {
            //Check if requester have relation to stock. If yes´, update amount with old + new. If no, create new relation.
            //Subtract sharesAmount from providers relation to stock. If 0, delete relation.
            using (ShareOwnerContext context = new ShareOwnerContext(options))
            {
            }
        }

        [HttpPost]
        public void CreateShareOwnership(string stockId, [FromBody]Guid userId, [FromBody]int sharesAmount)
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
