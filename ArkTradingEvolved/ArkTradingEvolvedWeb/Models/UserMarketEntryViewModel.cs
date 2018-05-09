using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DataTransferObjects;
using System.ComponentModel.DataAnnotations;

namespace ArkTradingEvolvedWeb.Models
{
	public class UserMarketEntryViewModel
	{
		public int MarketEntryID { get; set; }

		[Required(ErrorMessage ="The Collection Entry is required")]
		public CollectionEntry CollectionEntry { get; set; }
		[Required(ErrorMessage = "The Resource is required")]
		public Resource Resource { get; set; }

		
		
		public int? Units { get; set; }

		public List<CollectionEntry> CollectionEntries { get; set; }
		public List<Resource> Resources { get; set; }
	}
}