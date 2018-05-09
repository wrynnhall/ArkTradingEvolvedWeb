using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTransferObjects
{
    public class UserDetails
    {
        public User User { get; set; }
        public List<Role> Roles { get; set; }
        public bool PasswordMustBeChanged { get; set; }

        public UserDetails(User user, List<Role> roles, bool passwordMustBeChanged = false)
        {
            this.User = user;
            this.Roles = roles;
            this.PasswordMustBeChanged = passwordMustBeChanged;
        }

    }
}
