using System;
using System.IO;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using ServiceStack.OrmLite;

namespace PaymentControlAPI.Controllers
{
    [Route("api/[controller]")]
    public class PaymentController : Controller
    {
        private const string ConnectionString = "Data Source=(localdb)\\.\\SharedDB;Initial Catalog=PaymentControlDb;Integrated Security=SSPI;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

        // GET api/values/5
        [HttpGet("GetPaymentInfo")]
        public PaymentDataModel GetPaymentInfoForUser([FromBody]Guid userId)
        {
            var dbFactory = new OrmLiteConnectionFactory(
                ConnectionString,
                SqlServerDialect.Provider);

            using (var db = dbFactory.Open())
            {
                return db.Single<PaymentDataModel>(x => x.CounterParty == userId);
            }
        }

        // POST api/values
        [HttpPost("CreatePaymentInfo/{paymentType}/{paymentAmount}")]
        public void CreatePaymentInfo(string paymentType, int paymentAmount, [FromBody]Guid counterParty)
        {
            var dbFactory = new OrmLiteConnectionFactory(
                ConnectionString,
                SqlServerDialect.Provider);

            using (var db = dbFactory.Open())
            {
                db.CreateTableIfNotExists<PaymentDataModel>();

                db.Insert(new PaymentDataModel { PaymentType = paymentType, PaymentAmount = paymentAmount, CounterParty = counterParty });
            }

            if (!Directory.Exists("C:\\TSEIS\\"))
            {
                Directory.CreateDirectory("C:\\TSEIS\\");
            }

            using (var log = new LoggerConfiguration().WriteTo.File("C:\\TSEIS\\paymentLog.txt").CreateLogger())
            {
                log.Information($"{paymentType} of amount: {paymentAmount} has been send to {counterParty}");
            }
        }
    }
}