using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using DataTransferObjects;

namespace ArkTradingEvolvedWeb.Models
{
	public class CreatureDietViewModel : CreatureDiet
	{
	
		[Required]
		[MinLength(1, ErrorMessage = "Creature Diet cannot be empty")]
		[MaxLength(30, ErrorMessage = "Creature Diet cannot be greater than 30")]
		[DisplayName("Creature Diet")]
		public string DisplayCreatureDietID { get { return this.CreatureDietID; } set { this.CreatureDietID = value; } }
	}
}