using DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public static class UserAccessor
    {
        public static int VerifyUsernameAndPassword(string username, string passwordHash)
        {
            var result = 0; //return the number of rows found (should be 1)

            //need to start with a connection
            var conn = DBConnection.GetDBConnection();

            //next, we need command text - a procedure name or query string
            var cmdTxt = @"sp_authenticate_user";

            //next, we use the connection and command text to create a Command
            var cmd = new SqlCommand(cmdTxt, conn);

            //for a stored procedure, we need to set the command type
            cmd.CommandType = CommandType.StoredProcedure;

            // for a stored procedure, add any needed parameters
            cmd.Parameters.Add("@Email", SqlDbType.NVarChar, 100);
            cmd.Parameters.Add("@PasswordHash", SqlDbType.NVarChar, 100);

            //set the parameter values 
            cmd.Parameters["@Email"].Value = username;
            cmd.Parameters["@PasswordHash"].Value = passwordHash;

            // now that the connection command and parameters are set up we can execute the command.

            //database code is always and everywhere unsafe code, so...
            try
            {
                // first open the connection
                conn.Open();

                //Execute the command
                result = (int)cmd.ExecuteScalar();
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                // good housekeeping requires connections to be closed 
                conn.Close();
            }

            return result;
        }

        public static User RetrieveUserByUsername(string username)
        {
            User user = null;

            var conn = DBConnection.GetDBConnection();

            var cmdText = @"sp_retrieve_user_by_email";
            var cmd = new SqlCommand(cmdText, conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@Email", username);

            try
            {
                conn.Open();
                var reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    reader.Read();

                    user = new User()
                    {
                        UserID = reader.GetInt32(0),
                        Gamertag = reader.GetString(1),
                        FirstName = reader.GetString(2),
                        LastName = reader.GetString(3),
                        PhoneNumber = reader.IsDBNull(4) ? null : reader.GetString(4),
                        Email = reader.GetString(5),
                        Active = reader.GetBoolean(6)
                    };

                    if(user.Active != true)
                    {
                        throw new ApplicationException("Not an active user");
                    }
                }
                else
                {
                    throw new ApplicationException("User not found");
                }


            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                conn.Close();
            }
            
            return user;
        }

        public static User RetrieveUserById(int id)
        {
            User user = null;

            var conn = DBConnection.GetDBConnection();

            var cmdText = @"sp_retrieve_user_by_id";
            var cmd = new SqlCommand(cmdText, conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@UserID", id);

            try
            {
                conn.Open();
                var reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    reader.Read();

                    user = new User()
                    {
                        UserID = reader.GetInt32(0),
                        Gamertag = reader.GetString(1),
                        FirstName = reader.GetString(2),
                        LastName = reader.GetString(3),
                        PhoneNumber = reader.IsDBNull(4) ? "" : reader.GetString(4),
                        Email = reader.GetString(5),
                        Active = reader.GetBoolean(6)
                    };

                    if (user.Active != true)
                    {
                        throw new ApplicationException("Not an active user");
                    }
                }
                else
                {
                    throw new ApplicationException("User not found");
                }


            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                conn.Close();
            }

            return user;
        }

        public static User RetreiveUserByMarketEntryPurchaseMarketEntryID(int id)
        {
            User user = null;

            var conn = DBConnection.GetDBConnection();

            var cmdText = @"retrieve_user_by_market_entry_purchase_market_entry_id";
            var cmd = new SqlCommand(cmdText, conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@MarketEntryID", id);

            try
            {
                conn.Open();
                var reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    reader.Read();

                    user = new User()
                    {
                        UserID = reader.GetInt32(0),
                        Gamertag = reader.GetString(1),
                        FirstName = reader.GetString(2),
                        LastName = reader.GetString(3),
                        PhoneNumber = reader.GetString(4),
                        Email = reader.GetString(5),
                        Active = reader.GetBoolean(6)
                    };

                    if (user.Active != true)
                    {
                        throw new ApplicationException("Not an active user");
                    }
                }
                else
                {
                    throw new ApplicationException("User not found");
                }


            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                conn.Close();
            }

            return user;
        }

        public static User RetrieveUserByMarketEntryID(int id)
        {
            User user = null;

            var conn = DBConnection.GetDBConnection();

            var cmdText = @"sp_select_user_by_market_entry_id";
            var cmd = new SqlCommand(cmdText, conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@MarketEntryID", id);

            try
            {
                conn.Open();
                var reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    reader.Read();

                    user = new User()
                    {
                        UserID = reader.GetInt32(0),
                        Gamertag = reader.GetString(1),
                        FirstName = reader.GetString(2),
                        LastName = reader.GetString(3),
                        PhoneNumber = reader.GetString(4),
                        Email = reader.GetString(5),
                        Active = reader.GetBoolean(6)
                    };

                    if (user.Active != true)
                    {
                        throw new ApplicationException("Not an active user");
                    }
                }
                else
                {
                    throw new ApplicationException("User not found");
                }
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                conn.Close();
            }

            return user;
        }

        public static int UpdatePasswordHash(int userID, string oldPasswordHash, string newPasswordHash)
        {
            int result = 0;

            //connection
            var conn = DBConnection.GetDBConnection();

            //command text
            var cmdText = @"sp_update_passwordHash";

            //command
            var cmd = new SqlCommand(cmdText, conn);
            cmd.CommandType = CommandType.StoredProcedure;

            // parameters
            cmd.Parameters.Add("@UserID", SqlDbType.Int);
            cmd.Parameters.Add("@OldPasswordHash", SqlDbType.NVarChar, 100);
            cmd.Parameters.Add("@NewPasswordHash", SqlDbType.NVarChar, 100);

            //parameter values
            cmd.Parameters["@UserID"].Value = userID;
            cmd.Parameters["@OldPasswordHash"].Value = oldPasswordHash;
            cmd.Parameters["@NewPasswordHash"].Value = newPasswordHash;

            try
            {
                conn.Open();

                //execute the command
                result = cmd.ExecuteNonQuery();
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                conn.Close();
            }
            return result;
        }

        public static List<Role> RetrieveUserRoles(int userID)
        {
            List<Role> roles = new List<Role>();

            //connect
            var conn = DBConnection.GetDBConnection();

            // command text
            var cmdText = @"sp_retrieve_user_roles";

            //command
            var cmd = new SqlCommand(cmdText, conn);
            cmd.CommandType = CommandType.StoredProcedure;
            // parameters
            cmd.Parameters.Add("@UserID", SqlDbType.NVarChar, 20);

            //parameter values
            cmd.Parameters["@UserID"].Value = userID;

            try
            {
                //open the connection
                conn.Open();
                //execute the command
                var reader = cmd.ExecuteReader();

                // check for return rows
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        var role = new Role()
                        {
                            RoleID = reader.GetString(0)
                        };
                        roles.Add(role);
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                conn.Close();
            }

            return roles;
        }

        public static int CreateUser(User user)
        {
            int result = 0;

            var conn = DBConnection.GetDBConnection();
            var cmdText = @"sp_create_user";

            var cmd = new SqlCommand(cmdText, conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@Gamertag", user.Gamertag);
            cmd.Parameters.AddWithValue("@FirstName", user.FirstName);
            cmd.Parameters.AddWithValue("@LastName", user.LastName);

            if (user.PhoneNumber == null)
            {
                cmd.Parameters.AddWithValue("@PhoneNumber", user.PhoneNumber);
            }
            
           
            cmd.Parameters.AddWithValue("@Email", user.Email);
           
            cmd.Parameters.AddWithValue("@PasswordHash", user.PasswordHash);


            try
            {
                conn.Open();
                result = cmd.ExecuteNonQuery();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                conn.Close();
            }


            return result;
        }

        
    }
}
