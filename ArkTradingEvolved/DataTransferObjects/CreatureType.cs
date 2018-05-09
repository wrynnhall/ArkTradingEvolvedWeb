using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace DataTransferObjects
{
    public class CreatureType
    {
		[DisplayName("Creature Type")]
		public string CreatureTypeID { get; set; }
        public bool Active { get; set; }
    }
}
