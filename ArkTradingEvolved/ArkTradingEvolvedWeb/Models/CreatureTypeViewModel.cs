using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DataTransferObjects;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ArkTradingEvolvedWeb.Models
{
	public class CreatureTypeViewModel : CreatureType
	{

		[Required]
		[MinLength(1, ErrorMessage = "Creature Type cannot be empty")]
		[MaxLength(30, ErrorMessage = "Creature Type cannot be greater than 30")]
		[DisplayName("Creature Type")]
		public string DisplayCreatureTypeID { get { return this.CreatureTypeID; } set { this.CreatureTypeID = value; } }


		
	}
}