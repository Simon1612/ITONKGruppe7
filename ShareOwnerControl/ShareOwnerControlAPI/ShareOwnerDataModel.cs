using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShareOwnerControlAPI
{
    public class ShareOwnerDataModel
    {
        public ShareOwnerDataModel()
        {
        }

        public int Id { get; set; }
        public string StockId { get; set; }
        public int SharesAmount { get; set; }
        public Guid ShareOwner { get; set; }
    }
}
