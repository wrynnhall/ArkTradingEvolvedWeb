using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTransferObjects
{
    public class UserServer
    {
        public int UserID { get; set; }
        public int ServerID { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}
