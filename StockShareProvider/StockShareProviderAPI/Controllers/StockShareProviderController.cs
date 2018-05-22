using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace StockShareProviderAPI.Controllers
{
    [Route("api/[controller]")]
    public class StockShareProviderController : Controller
    {
        [HttpPut("{stockId}")]
        public List<AvailableSharesDataModel> MarkSharesForSale(string stockId, [FromBody] Guid userId, [FromBody] int sharesAmount)
        {
            //Validate ownership of stock with stockId
            //Validate stockId has sharesAmount of shares available for sale.
            //Make stock with stockId put sharesAmount of shares for sale.
            
            using (AvailableSharesContext context = new AvailableSharesContext(options))
            {
                //Return list of shares for sale, for stock with stockId
                return new List<AvailableSharesDataModel>();
            }
            //return response
        }

        [HttpGet("{stockId}")]
        public List<AvailableSharesDataModel> GetSharesForSale(string stockId)
        {
            using (AvailableSharesContext context = new AvailableSharesContext(options)) {
                //Return list of shares for sale, for stock with stockId
                return new List<AvailableSharesDataModel>();
            }
        }

        public DbContextOptions<AvailableSharesContext> options = new DbContextOptionsBuilder<AvailableSharesContext>()
                .UseInMemoryDatabase(databaseName: "AvailableSharesDb")
                .Options;

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
