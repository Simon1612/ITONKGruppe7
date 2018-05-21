using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace TradeBrokerAPI.Controllers
{
    [Route("api/[controller]")]
    public class TradeBrokerController : Controller
    {
        [HttpPost]
        public void InitiateTrade([FromBody]string stockName, Guid requester)
        {
            //Do something
        }
    }
}
