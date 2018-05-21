using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace StockShareProviderAPI.Controllers
{
    [Route("api/[controller]")]
    public class StockShareProviderController : Controller
    {
        [HttpPut("{stockId}")]
        public void MarkSharesForSale(string stockId, [FromBody] Guid userId, [FromBody] int sharesAmount)
        {
            //Validate ownership of stock with stockId
            //Validate stockId has sharesAmount of shares available for sale.
            //Make stock with stockId put sharesAmount of shares for sale.
            //return response
        }

        [HttpGet("{stockId}")]
        public string GetSharesForSale(string stockId)
        {
            //Return list of shares for sale, for stock with stockId
            return "stuff";
        }

        /*
        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }

        // GET api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }
        */
    }
}
