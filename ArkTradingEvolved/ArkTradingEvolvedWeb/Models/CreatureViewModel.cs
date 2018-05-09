using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DataTransferObjects;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ArkTradingEvolvedWeb.Models
{
    public class CreatureViewModel : Creature
    {
        
        [Required]
        [MinLength(1, ErrorMessage = "Name cannot be empty")]
        [MaxLength(50, ErrorMessage = "Name cannot be greater than 50")]
        [DisplayName("Creature Name")]
        public string DisplayCreatureID { get { return this.CreatureID; } set { this.CreatureID = value; } }

        [Required]
        [MinLength(1, ErrorMessage = "Name cannot be empty")]
        [MaxLength(30, ErrorMessage = "Name cannot be greater than 30")]
        [DisplayName("Creature Type")]
        public string DisplayCreatureTypeID { get { return this.CreatureTypeID; } set { this.CreatureTypeID = value; } }

        [Required]
        [MinLength(1, ErrorMessage = "Name cannot be empty")]
        [MaxLength(30, ErrorMessage = "Name cannot be greater than 30")]
        [DisplayName("Creature Diet")]
        public string DisplayCreatureDietID { get { return this.CreatureDietID; } set { this.CreatureDietID = value; } }
    }
}