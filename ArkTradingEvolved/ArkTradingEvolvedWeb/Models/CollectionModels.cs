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
    public class CollectionModels : Collection
    {
        [Required]
        [MinLength(1, ErrorMessage = "Name cannot be emty")]
        [MaxLength(100, ErrorMessage = "Name cannot be greater than 100")]
        [DisplayName("Name")]
        public string DisplayName { get { return this.Name; } set { this.Name = value; } }

        
    }
}