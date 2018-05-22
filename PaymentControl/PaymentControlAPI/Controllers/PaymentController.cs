using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace PaymentControlAPI.Controllers
{
    [Route("api/[controller]")]
    public class PaymentController : Controller
    {
        // GET api/values/5
        [HttpGet("{paymentId}")]
        public string GetPaymentInfo(Guid paymentId)
        {
            using (PaymentContext context = new PaymentContext(options))
            {
                return "value";
            }
        }

        // POST api/values
        [HttpPost]
        public void CreatePaymentInfo([FromBody]string value)
        {
            using (PaymentContext context = new PaymentContext(options))
            {
            }
                //Think about this one
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