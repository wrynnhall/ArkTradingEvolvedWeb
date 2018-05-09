using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTransferObjects
{
    public class User
    {
        public int UserID { get; set; }
        public string Gamertag { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string UserTypeID { get; set; }
        public bool Active { get; set; }

        public String FullName { 
            get {
                return FirstName + " " + LastName;

            } 
        }

        public string PasswordHash { get; set; }
    }
}
