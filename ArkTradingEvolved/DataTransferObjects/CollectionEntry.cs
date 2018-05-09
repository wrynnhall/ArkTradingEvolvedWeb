using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTransferObjects
{
    public class CollectionEntry
    {
        public int CollectionEntryID { get; set; }
        public int CollectionID { get; set; }
        public string CreatureID { get; set; }
        public string Name { get; set; }
        public int Level { get; set; }
        public int Health { get; set; }
        public int Stamina { get; set; }
        public int Oxygen { get; set; }
        public int Food { get; set; }
        public int Weight { get; set; }
        public int BaseDamage { get; set; }
        public int MovementSpeed { get; set; }
        public int Torpor { get; set; }
        public int Imprint { get; set; }
        public bool Active { get; set; }
    }
}
