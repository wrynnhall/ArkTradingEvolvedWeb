using DataAccessLayer;
using DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace LogicLayer
{
    public class UserManager
    {


        public bool CreateUser(User user)
        {
            user.PasswordHash = HashSha256(user.PasswordHash);

            try
            {
                int result = UserAccessor.CreateUser(user);
                if (result == 0)
                {
                    throw new ApplicationException("User was not created");
                }
            }
            catch (Exception)
            {

                return false;
            }

            return true;
        }


        public UserDetails AuthenticateUser(string username, string password)
        {
            UserDetails userDetails = null; // user token to build

            //can test for password complexity, but wont for now

            // we need a function to hash the password first
            var passwordHash = HashSha256(password);


            try
            {
                //we want to try to get a 1 as a result of calling the access method
                if (1 == UserAccessor.VerifyUsernameAndPassword(username, passwordHash))
                {
                    //get the employee object
                    var user = UserAccessor.RetrieveUserByUsername(username);

                    //get the roles list
                    var roles = UserAccessor.RetrieveUserRoles(user.UserID);

                    //check to see if the password needs changing
                    bool passwordNeedsChanging = false;
                    if (password == "password")
                    {
                        passwordNeedsChanging = true;
                        roles.Clear();
                        roles.Add(new Role() { RoleID = "New User" });
                    }
                    // we might want to include code to invalidate the user by clearing
                    // the roles list if the user's password is expired, such as with user.Roles.Clear()

                    userDetails = new UserDetails(user, roles, passwordNeedsChanging);
                }
            }
            catch (Exception ex) //other exceptions are possible: (SqlException)
            {
                // wrap the exception in one with a friendlier message
                throw new ApplicationException("Login Failure!", ex);
            }

            return userDetails;
        }

        //function to apply a SHA256 (secure hash algorithm) to a password to store
        //or compare with the user's passwordHash in the database
        public string HashSha256(string source)
        {
            string result = null;

            //create a byte array
            byte[] data;

            //create a .NET Hash provider object
            //this using statement is commonly used to create 
            //block level code when no block statement is needed
            //that allows us to use computationally expensive
            //classes, and be sure they are disposed immediately
            using (SHA256 sha256hash = SHA256.Create())
            {
                // hash the input
                data = sha256hash.ComputeHash(Encoding.UTF8.GetBytes(source));

            }

            //now build the result string
            var s = new StringBuilder();
            //loop through the byte array creating letters
            //to add to the StringBuilder
            for (int i = 0; i < data.Length; i++)
            {
                //read each byte as a number, convert it to a string 
                //format it as hex digits, not as decimal digits
                s.Append(data[i].ToString("x2"));//to hex digits
            }

            //call a string from the stringbuilder
            result = s.ToString();

            return result;
        }

        public UserDetails UpdatePassword(UserDetails user, string oldPassword, string newPassword)
        {
            UserDetails newUser = null;
            int rowsAffected = 0;

            string oldPasswordHash = HashSha256(oldPassword);
            string newPasswordHash = HashSha256(newPassword);

            // try to invoke the data access method
            try
            {
                rowsAffected = UserAccessor.UpdatePasswordHash(user.User.UserID, oldPasswordHash, newPasswordHash);
                if (rowsAffected == 1)// update succeeded
                {
                    if (user.Roles[0].RoleID == "New User")
                    {
                        var roles = UserAccessor.RetrieveUserRoles(user.User.UserID);
                        newUser = new UserDetails(user.User, roles);
                    }
                    else
                    {
                        newUser = user; // existing user, nothing to change.
                    }
                }
                else
                {
                    throw new ApplicationException("Update returned 0 rows affected");
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Password changed failed", ex);
                throw;
            }

            return newUser;
        }

        public User RetreiveUserByMarketEntryPurchaseMarketEntryID(int id)
        {
            User user = null;

            try
            {
                user = UserAccessor.RetreiveUserByMarketEntryPurchaseMarketEntryID(id);
            }
            catch (Exception)
            {
                
                throw;
            }


            return user;
        }

        public User RetreiveUserByUsername(string username)
        {
            User user = null;

            try
            {
                user = UserAccessor.RetrieveUserByUsername(username);
            }
            catch (Exception)
            {

                throw;
            }


            return user;
        }


        public User RetreiveUserByID(int id)
        {
            User user = null;

            try
            {
                user = UserAccessor.RetrieveUserById(id);
            }
            catch (Exception)
            {

                throw;
            }


            return user;
        }
    }
}
