using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using StockShareRequesterAPI.Clients;

namespace StockShareRequesterAPI.Controllers
{
    [Route("api/[controller]")]
    public class StockShareRequesterController : Controller
    {
        [HttpGet("GetSharesForUser")]
        public List<ShareOwnerDataModel> GetAllSharesByUserId([FromBody] Guid userId)
        {
            var shareOwnerControlClient = new ShareOwnerControlClient("http://localhost:8758");
            var sharesList = shareOwnerControlClient.ApiShareOwnerGetAllSharesForUserByUserIdGetAsync(userId).Result;

            return sharesList.ToList();
        }

 
        [HttpGet("GetAvailableSharesForUser")]
        public List<AvailableSharesDataModel> GetSharesForSaleByUserId([FromBody] Guid userId)
        {
            var stockShareProviderClient = new StockShareProviderClient("http://localhost:8748");
            var sharesForSale = stockShareProviderClient.ApiStockShareProviderGetSharesForSaleGetAsync(userId).Result;

            return sharesForSale.ToList();
        }

        [HttpGet("GetAllSharesForSale")]
        public List<AvailableSharesDataModel> GetAllSharesForSale()
        {
            var stockShareProviderClient = new StockShareProviderClient("http://localhost:8748");
            var sharesList = stockShareProviderClient.ApiStockShareProviderGetAsync().Result;

            return sharesList.ToList();
        }

        [HttpPost("CreateAvailableShares/{stockId}")]
        public void CreateAvailableShares(string stockId, [FromBody] Guid userId, int sharesAmount)
        {
            var stockShareProviderClient = new StockShareProviderClient("http://localhost:8748");
            stockShareProviderClient.ApiStockShareProviderCreateAvailableSharesByStockIdPostAsync(stockId, userId,
                sharesAmount);
        }

        [HttpPost("InitiateTrade/{stockId}/{sharesAmount}")]
        public void InitiateTrade(string stockId, int sharesAmount, [FromBody] Guid requesterId)
        {
            var tradeBrokerClient = new TradeBrokerClient("http://localhost:8761");
            tradeBrokerClient.ApiTradeBrokerInitiateTradeByStockIdBySharesAmountPostAsync(stockId, sharesAmount,
                requesterId);
        }

        [HttpPost("CreateStock/{stockId}")]
        public void CreateStock(string stockId, int sharePrice)
        {
            var shareOwnerControlClient = new ShareOwnerControlClient("http://localhost:8758");
            shareOwnerControlClient.ApiShareOwnerCreateStockByStockIdPostAsync(stockId, sharePrice);
        }



        [HttpPost("CreateOwner")]
        public void CreateOwner([FromBody] Guid shareHolderId)
        {
            var shareOwnerControlClient = new ShareOwnerControlClient("http://localhost:8758");
            shareOwnerControlClient.ApiShareOwnerCreateOwnerPostAsync(shareHolderId);
        }


        [HttpGet("GetAllUsers")]
        public List<OwnerDataModel> GetAllUsers()
        {
            var shareOwnerControlClient = new ShareOwnerControlClient("http://localhost:8758");
            var usersList = shareOwnerControlClient.ApiShareOwnerGetAllUsersGetAsync().Result;

            return usersList.ToList();
        }
    }
}
