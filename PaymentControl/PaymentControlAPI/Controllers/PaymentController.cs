using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace PaymentControlAPI.Controllers
{
    [Route("api/[controller]")]
    public class PaymentController : Controller
    {
        // GET api/values/5
        [HttpGet("{paymentId}")]
        public string GetPaymentInfo(Guid paymentId)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void CreatePaymentInfo([FromBody]string value)
        {
            //Think about this one
        }

        [HttpPut]
        public void UpdatePaymentInfo([FromBody]string value)
        {
            //Think about this one
        }
    }
}
