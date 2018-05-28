using System;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Collections.Generic;
using ServiceStack.OrmLite;

namespace StockShareProviderAPI.Controllers
{
    [Route("api/[controller]")]
    public class StockShareProviderController : Controller
    {
        private const string ConnectionString = "Data Source=(localdb)\\.\\SharedDB;Initial Catalog=StockShareProviderDb;Integrated Security=SSPI;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

        [HttpPost("CreateAvailableShares/{stockId}")]
        public void CreateAvailableShares(string stockId, [FromBody] Guid userId, int sharesAmount)
        {
            if (stockId == null) return;

            var dbFactory = new OrmLiteConnectionFactory(
                ConnectionString,
                SqlServerDialect.Provider);

            using (var db = dbFactory.Open())
            {
                db.CreateTableIfNotExists<AvailableSharesDataModel>();

                db.Insert(new AvailableSharesDataModel { StockId = stockId, StockOwner = userId, SharesAmount = sharesAmount });
            }
        }

        [HttpPut("IncreaseSharesAmountForSale/{stockId}")]
        public void IncreaseSharesAmountForSale(string stockId, [FromBody] Guid userId, int sharesAmount)
        {
            var dbFactory = new OrmLiteConnectionFactory(
                ConnectionString,
                SqlServerDialect.Provider);

            using (var db = dbFactory.Open())
            {
                db.CreateTableIfNotExists<AvailableSharesDataModel>();

                var selectedStock = db.Single<AvailableSharesDataModel>(x => x.StockId == stockId);

                if (selectedStock.StockOwner.Equals(userId))
                {
                    selectedStock.SharesAmount += sharesAmount;
                }

                db.Update(selectedStock);
            }
        }

        [HttpPut("DecreaseSharesAmountForSale/{stockId}")]
        public void DecreaseSharesAmountForSale(string stockId, [FromBody] Guid userId, int sharesAmount)
        {
            var dbFactory = new OrmLiteConnectionFactory(
                ConnectionString,
                SqlServerDialect.Provider);

            using (var db = dbFactory.Open())
            {
                db.CreateTableIfNotExists<AvailableSharesDataModel>();

                var selectedStock = db.Single<AvailableSharesDataModel>(x => x.StockId == stockId);

                if (!selectedStock.StockOwner.Equals(userId)) return;

                selectedStock.SharesAmount -= sharesAmount;

                if (selectedStock.SharesAmount < 0)
                    return;

                if (selectedStock.SharesAmount == 0)
                    db.Delete(selectedStock);
                else
                    db.Update(selectedStock);
            }
        }

        [HttpGet("GetSharesForSale/{stockId}")]
        public List<AvailableSharesDataModel> GetSharesForSaleByStock(string stockId)
        {
            var dbFactory = new OrmLiteConnectionFactory(
                ConnectionString,
                SqlServerDialect.Provider);

            using (var db = dbFactory.Open())
            {
                return db.Select<AvailableSharesDataModel>().Where(x => x.StockId.Equals(stockId)).ToList();
            }
        }

        [HttpGet("GetSharesForSale")]
        public List<AvailableSharesDataModel> GetSharesForSaleByUser([FromBody]Guid userId)
        {
            var dbFactory = new OrmLiteConnectionFactory(
                ConnectionString,
                SqlServerDialect.Provider);

            using (var db = dbFactory.Open())
            {
                return db.Select<AvailableSharesDataModel>().Where(x => x.StockOwner == userId).ToList();
            }
        }

        [HttpGet]
        public List<AvailableSharesDataModel> GetAllSharesForSale()
        {
            var dbFactory = new OrmLiteConnectionFactory(
                ConnectionString,
                SqlServerDialect.Provider);

            using (var db = dbFactory.Open())
            {
                return db.Select<AvailableSharesDataModel>().ToList();
            }
        }
    }
}
