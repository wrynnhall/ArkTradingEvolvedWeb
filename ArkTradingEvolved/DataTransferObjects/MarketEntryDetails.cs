using System;
using System.Collections.Generic;

using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTransferObjects
{
    public class MarketEntryDetails 
    {
		public User User { get; set; }
        public MarketEntry MarketEntry { get; set; }
        public CollectionEntry CollectionEntry { get; set; }
		
    }
}
