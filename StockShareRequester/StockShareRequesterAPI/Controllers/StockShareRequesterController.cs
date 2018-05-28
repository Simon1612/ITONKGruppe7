using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StockShareRequesterAPI.Models;

namespace StockShareRequesterAPI.Controllers
{
    [Route("api/[controller]")]
    public class StockShareRequesterController : Controller
    {
        [HttpGet("GetSharesForUser/{userId}")]
        public List<ShareDataModel> GetAllSharesByUserId([FromBody] Guid userId)
        {
            //GetAllShares from OwnerControl by userId
        }

        //Gets shares marked for sale
        [HttpGet("GetAvailableSharesForUser/{userId}")]
        public List<ShareDataModel> GetSharesForSaleByUserId([FromBody] Guid userId)
        {
            //Get shares marked for sale from provider by userId
        }

        [HttpGet]
        public List<ShareDataModel> GetAllSharesForSale()
        {
            //Gets all available shares from provider
        }

        // POST api/values
        [HttpPost("CreateAvailableShares/{stockId}")]
        public void CreateAvailableShares(string stockId, [FromBody] Guid userId, int sharesAmount)
        {
            //Creates shares for sale in provider
        }

        [HttpPost("InitiateTrade/{stockId}/{sharesAmount}")]
        public void InitiateTrade(string stockId, int sharesAmount, [FromBody] Guid requesterId)
        {
            //From buy in GUI. Call initiate in broker
        }
    }
}
