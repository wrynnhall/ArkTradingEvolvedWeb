using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace DataTransferObjects
{
    public class CreatureDiet
    {
		[DisplayName("Creature Diet")]
		public string CreatureDietID { get; set; }
        public bool Active { get; set; }
    }
}
