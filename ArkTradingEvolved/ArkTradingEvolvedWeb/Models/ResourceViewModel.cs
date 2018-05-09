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
    public class ResourceViewModel : Resource
    {
        [Required]
        [MinLength(1, ErrorMessage = "Name cannot be emty")]
        [MaxLength(30, ErrorMessage = "Name cannot be greater than 30")]
        [DisplayName("Resource Name")]
        public string DisplayResourceID { get { return this.ResourceID; } set { this.ResourceID = value; } }
    }
}