using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DataTransferObjects;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ArkTradingEvolvedWeb.Models
{
	public class PurchaseCompleteViewModel
	{
		public List<Collection> Collections { get; set; }
		public PurchaseDetails PurchaseDetails { get; set; }

		[Required(ErrorMessage = "The Collection is required")]
		public int CollectionToAddTo { get; set; }
	}
}