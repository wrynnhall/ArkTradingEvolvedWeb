using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DataTransferObjects;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace ArkTradingEvolvedWeb.Models
{
    public class CollectionEntryModel : CollectionEntry
    {
        
        public List<Creature> AvailableCreatures { get; set; }

        [Required(ErrorMessage = "The Creature is required")]
        public string DisplayCreatureID { get { return this.CreatureID; } set { this.CreatureID = value; } }

        

        [Required]
        [MinLength(1, ErrorMessage = "Name cannot be emty")]
        [MaxLength(50, ErrorMessage = "Name cannot be greater than 50")]
        [DisplayName("Name")]
        public string DisplayName { get { return this.Name; } set { this.Name = value; } }

        [Required]
        [DisplayName("Level")]
        public int DisplayLevel { get { return this.Level; } set { this.Level = value; } }

        [Required]
        [DisplayName("Health")]
        public int DisplayHealth { get { return this.Health; } set { this.Health = value; } }

        [Required]
        [DisplayName("Stamina")]
        public int DisplayStamina { get { return this.Stamina; } set { this.Stamina = value; } }

        [Required]
        [DisplayName("Oxygen")]
        public int DisplayOxygen { get { return this.Oxygen; } set { this.Oxygen = value; } }

        [Required]
        [DisplayName("Food")]
        public int DisplayFood { get { return this.Food; } set { this.Food = value; } }

        [Required]
        [DisplayName("Weight")]
        public int DisplayWeight { get { return this.Weight; } set { this.Weight = value; } }

        [Required]
        [DisplayName("Base Damage")]
        public int DisplayBaseDamage { get { return this.BaseDamage; } set { this.BaseDamage = value; } }

        [Required]
        [DisplayName("Movement Speed")]
        public int DisplayMovementSpeed { get { return this.MovementSpeed; } set { this.MovementSpeed = value; } }

        [Required]
        [DisplayName("Torpor")]
        public int DisplayTorpor { get { return this.Torpor; } set { this.Torpor = value; } }

        [Required]
        [DisplayName("Imprint")]
        public int DisplayImprint { get { return this.Imprint; } set { this.Imprint = value; } }

        

       

        /*[CreatureID]		[nvarchar](50)	NOT NULL,
	[Name]				[nvarchar](50)	NOT NULL,*/
    }
}