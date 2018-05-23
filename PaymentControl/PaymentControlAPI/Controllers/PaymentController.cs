using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace PaymentControlAPI.Controllers
{
    [Route("api/[controller]")]
    public class PaymentController : Controller
    {
        // GET api/values/5
        [HttpGet("GetPaymentInfo/{paymentId}")]
        public string GetPaymentInfo([FromBody]Guid paymentId)
        {
            using (PaymentContext context = new PaymentContext(options))
            {
                return "value";
            }
        }

        // POST api/values
        [HttpPost("CreatePaymentInfo/{value]")]
        public void CreatePaymentInfo(string value)
        {
            using (PaymentContext context = new PaymentContext(options))
            {
            }
                //Think about this one
        }

        [HttpPut("UpdatePaymentInfo/{value}")]
        public void UpdatePaymentInfo(string value)
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