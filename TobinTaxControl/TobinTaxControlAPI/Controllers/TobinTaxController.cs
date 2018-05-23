using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace TobinTaxControlAPI.Controllers
{
    [Route("api/[controller]")]
    public class TobinTaxController : Controller
    {
        // POST api/values
        [HttpPost]
        public void PayTobinTax(float transactionValue, [FromBody]int transactionId)
        {
            float tobinTaxValue = transactionValue / 100;

            if (!Directory.Exists("C:\\TSEIS\\"))
            {
                Directory.CreateDirectory("C:\\TSEIS\\");
            }

            using (var log = new LoggerConfiguration().WriteTo.File("C:\\TSEIS\\tobinTaxLog.txt").CreateLogger())
            {
                log.Information($"Transaction: {transactionId} has been taxed {tobinTaxValue.ToString()}(1%) of the total value.");
            }
        }
    }
}
