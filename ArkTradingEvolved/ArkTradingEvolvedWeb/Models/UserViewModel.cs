using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DataTransferObjects;

namespace ArkTradingEvolvedWeb.Models
{
    public class UserViewModel
    {
		public User User { get; set; }
        public List<Collection> Collections { get; set; }
        public List<CollectionEntryDetails> CollectionEntries { get; set; }
		public List<MarketEntryDetails> Purchases { get; set; }
		public List<MarketEntryDetails> MarketEntries { get; set; }
		public int SelectedCollection { get; set; }
    }
}