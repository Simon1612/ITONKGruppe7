using System;
using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace PaymentControlAPI.Controllers
{
    [Route("api/[controller]")]
    public class PaymentController : Controller
    {
        // GET api/values/5
        [HttpGet("{paymentId}")]
        public PaymentDataModel GetPaymentInfo(int paymentId)
        {
            using (PaymentContext context = new PaymentContext(options))
            {
                return context.Find<PaymentDataModel>(paymentId);
            }
        }

        // POST api/values
        [HttpPost]
        public void CreatePaymentInfo(string paymentType, int paymentAmount, [FromBody]Guid counterParty)
        {
            using (PaymentContext context = new PaymentContext(options))
            {
                context.Add(new PaymentDataModel { PaymentType = paymentType, PaymentAmount = paymentAmount, CounterParty=counterParty });
                context.SaveChanges();
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

        [HttpPut]
        public void UpdatePaymentInfo([FromBody]string value)
        {
            using (PaymentContext context = new PaymentContext(options))
            {
            }
            //Think about this one
        }

        public DbContextOptions<PaymentContext> options = new DbContextOptionsBuilder<PaymentContext>()
            .UseInMemoryDatabase(databaseName: "PaymentDb")
            .Options;
    }
}