using System;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using ShareOwnerControlAPI.Models;
using System.Collections.Generic;
using ServiceStack.OrmLite;

namespace ShareOwnerControlAPI.Controllers
{
    [Route("api/[controller]")]
    public class ShareOwnerController : Controller
    {
        private const string ConnectionString = "Data Source=(localdb)\\.\\SharedDB;Initial Catalog=ShareOwnerControlDb;Integrated Security=SSPI;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

        [HttpGet("GetAllSharesForUser/{userId}")]
        public List<ShareOwnerDataModel> GetAllSharesForUser(Guid userId)
        {
            var dbFactory = new OrmLiteConnectionFactory(
                ConnectionString,
                SqlServerDialect.Provider);

            using (var db = dbFactory.Open())
            {
                return db.Select<ShareOwnerDataModel>()
                    .Where(shareOwnerModel => shareOwnerModel.ShareOwner.ShareHolderId.Equals(userId)).ToList();
            }
        }

        [HttpGet("VerifyShareOwnership/{stockId}")]
        public bool VerifyShareOwnership(string stockId, Guid userId, int sharesAmount)
        {
            var dbFactory = new OrmLiteConnectionFactory(
                ConnectionString,
                SqlServerDialect.Provider);

            using (var db = dbFactory.Open())
            {
                var shareOwnerData = db.Select<ShareOwnerDataModel>().FirstOrDefault(x => x.ShareOwner.ShareHolderId == userId && x.Stock.StockId == stockId);

                if (shareOwnerData == null) return false;
                return shareOwnerData.SharesAmount >= sharesAmount;
            }
        }

        [HttpPut("UpdateShareOwnership/{stockId}")]
        public void UpdateShareOwnership(string stockId, [FromBody]Guid requester, Guid provider, int sharesAmount)
        {
            var dbFactory = new OrmLiteConnectionFactory(
                ConnectionString,
                SqlServerDialect.Provider);

            using (var db = dbFactory.Open())
            {
                db.CreateTableIfNotExists<ShareOwnerDataModel>();
                db.CreateTableIfNotExists<StockDataModel>();
                db.CreateTableIfNotExists<OwnerDataModel>();

                if (!VerifyShareOwnership(stockId, provider, sharesAmount)) return;

                var newShareOwnerData = db.Select<ShareOwnerDataModel>().FirstOrDefault(x => x.ShareOwner.ShareHolderId == requester && x.Stock.StockId == stockId);

                var oldShareOwnerData = db.Select<ShareOwnerDataModel>().FirstOrDefault(x => x.ShareOwner.ShareHolderId == provider && x.Stock.StockId == stockId);

                if (oldShareOwnerData == null) return;
                if (oldShareOwnerData.SharesAmount < sharesAmount) return;

                if (newShareOwnerData == null)
                {
                    var shareOwner = db.Single<OwnerDataModel>(x => x.ShareHolderId == requester);

                    if (shareOwner == null) BadRequest("Share Requester does not exist.");

                    var stock = db.Single<StockDataModel>(x => x.StockId == stockId);

                    db.Insert(new ShareOwnerDataModel { ShareOwner = shareOwner, SharesAmount = sharesAmount, Stock = stock });
                }
                else
                {
                    newShareOwnerData.SharesAmount += sharesAmount;
                    db.Update(newShareOwnerData);
                }

                oldShareOwnerData.SharesAmount -= sharesAmount;

                db.Update(oldShareOwnerData);
            }
        }

        [HttpPost("CreateShareOwnership/{stockId}/{sharesAmount}")]
        public void CreateShareOwnership(string stockId, [FromBody]Guid userId, int sharesAmount)
        {
            var dbFactory = new OrmLiteConnectionFactory(
                ConnectionString,
                SqlServerDialect.Provider);

            using (var db = dbFactory.Open())
            {
                db.CreateTableIfNotExists<ShareOwnerDataModel>();
                db.CreateTableIfNotExists<StockDataModel>();
                db.CreateTableIfNotExists<OwnerDataModel>();

                var user = db.Single<OwnerDataModel>(x => x.ShareHolderId == userId);

                var stock = db.Single<StockDataModel>(x => x.StockId == stockId);

                if (user == null)
                {
                    user = new OwnerDataModel() { ShareHolderId = userId };
                    db.Insert(user);
                }

                if (stock == null)
                {
                    stock = new StockDataModel()
                    {
                        StockId = stockId,
                        SharePrice = 5
                    };

                   db.Insert(stock);
                }

                db.Insert(new ShareOwnerDataModel()
                {
                    Stock = stock,
                    SharesAmount = sharesAmount,
                    ShareOwner = user
                });
            }
        }

        [HttpGet("GetStockInfo/{stockId}")]
        public StockDataModel GetStockInfo(string stockId)
        {
            var dbFactory = new OrmLiteConnectionFactory(
                ConnectionString,
                SqlServerDialect.Provider);

            using (var db = dbFactory.Open())
            {
                return db.Single<StockDataModel>(x => x.StockId == stockId);
            }
        }

        [HttpGet("GetAllStocks")]
        public List<StockDataModel> GetAllStocks()
        {
            var dbFactory = new OrmLiteConnectionFactory(
                ConnectionString,
                SqlServerDialect.Provider);

            using (var db = dbFactory.Open())
            {
                return db.Select<StockDataModel>().ToList();
            }
        }

        [HttpPost("CreateStock/{stockId}")]
        public void CreateStock(string stockId, int sharePrice)
        {
            var dbFactory = new OrmLiteConnectionFactory(
                ConnectionString,
                SqlServerDialect.Provider);

            using (var db = dbFactory.Open())
            {
                db.CreateTableIfNotExists<StockDataModel>();

                db.Insert(new StockDataModel
                {
                    StockId = stockId,
                    SharePrice = sharePrice
                });
            }
        }

        [HttpGet("GetAllUsers")]
        public List<OwnerDataModel> GetAllUsers()
        {
            var dbFactory = new OrmLiteConnectionFactory(
                ConnectionString,
                SqlServerDialect.Provider);

            using (var db = dbFactory.Open())
            {
                return db.Select<OwnerDataModel>().ToList();
            }
        }

        [HttpPost("CreateOwner")]
        public void CreateOwner([FromBody]Guid shareHolderId)
        {
            var dbFactory = new OrmLiteConnectionFactory(
                ConnectionString,
                SqlServerDialect.Provider);

            using (var db = dbFactory.Open())
            {
                db.CreateTableIfNotExists<OwnerDataModel>();

                db.Insert(new OwnerDataModel
                {
                    ShareHolderId = shareHolderId
                });
            }
        }


    }
}
