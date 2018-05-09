using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTransferObjects
{
    public class PurchaseDetails
    {
        public User User { get; set; }

        public MarketEntryDetails MarketEntryDetails { get; set; }
    }
}
