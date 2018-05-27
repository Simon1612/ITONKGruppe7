using System;
using System.Globalization;
using System.IO;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace TobinTaxControlAPI.Controllers
{
    [Route("api/[controller]")]
    public class TobinTaxController : Controller
    {
        // POST api/values
        [HttpPost]
        public void PayTobinTax(float transactionValue, [FromBody]Guid shareProviderId)
        {
            var tobinTaxValue = transactionValue / 100;

            if (!Directory.Exists("C:\\TSEIS\\"))
            {
                Directory.CreateDirectory("C:\\TSEIS\\");
            }

            using (var log = new LoggerConfiguration().WriteTo.File("C:\\TSEIS\\tobinTaxLog.txt").CreateLogger())
            {
                log.Information($"Transaction: {shareProviderId} has been taxed {tobinTaxValue.ToString(CultureInfo.InvariantCulture)}(1%) of the total value.");
            }
        }
    }
}
