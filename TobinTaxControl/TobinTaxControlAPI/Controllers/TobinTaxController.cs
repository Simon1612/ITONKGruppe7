using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace TobinTaxControlAPI.Controllers
{
    [Route("api/[controller]")]
    public class TobinTaxController : Controller
    {
        // POST api/values
        [HttpPost]
        public void PayTobinTax([FromBody]int transactionValue, Guid transactionId)
        {
            var tobinTaxValue = transactionValue / 100;

            //Log tobinTaxValue and transactionId in file.
        }
    }
}
