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
    public class CreatureDietAccessor
    {
        public static List<CreatureDiet> RetrieveCreatureDietList()
        {
            var creatureDiets = new List<CreatureDiet>();

            //connect
            var conn = DBConnection.GetDBConnection();

            // command text
            var cmdText = @"sp_retrieve_creatures_diet";

            //command
            var cmd = new SqlCommand(cmdText, conn);
            cmd.CommandType = CommandType.StoredProcedure;



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
                        var diet = new CreatureDiet()
                        {
                            CreatureDietID = reader.GetString(0),
                            Active = reader.GetBoolean(1)
                        };
                        creatureDiets.Add(diet);
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

            return creatureDiets;
        }


        public static int UpdateCreatureDiet(CreatureDiet oldCreatureDiet, CreatureDiet newCreatureDiet)
        {
            int result = 0;

            var conn = DBConnection.GetDBConnection();
            var cmdText = @"sp_update_creature_diet";

            var cmd = new SqlCommand(cmdText, conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@OldCreatureDietID", oldCreatureDiet.CreatureDietID);
            cmd.Parameters.AddWithValue("@NewCreatureDietID", newCreatureDiet.CreatureDietID);

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

        public static List<CreatureDiet> RetrieveCreatureDietListActive()
        {
            var creatureDiets = new List<CreatureDiet>();

            //connect
            var conn = DBConnection.GetDBConnection();

            // command text
            var cmdText = @"sp_retrieve_creatures_diet_active";

            //command
            var cmd = new SqlCommand(cmdText, conn);
            cmd.CommandType = CommandType.StoredProcedure;



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
                        var diet = new CreatureDiet()
                        {
                            CreatureDietID = reader.GetString(0),
                            Active = reader.GetBoolean(1)
                        };
                        creatureDiets.Add(diet);
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

            return creatureDiets;
        }

        public static int UpdateCreatureDietActive(string creatureDietId, bool active)
        {
            int result = 0;

            var conn = DBConnection.GetDBConnection();
            var cmdText = @"sp_update_creature_diet_active";

            var cmd = new SqlCommand(cmdText, conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@CreatureDietID", creatureDietId);
            cmd.Parameters.AddWithValue("@Active", active);

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

        public static int AddCreatureDiet(CreatureDiet creatureDiet)
        {
            int result = 0;

            var conn = DBConnection.GetDBConnection();
            var cmdText = @"sp_add_creature_diet";

            var cmd = new SqlCommand(cmdText, conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@CreatureDietID", creatureDiet.CreatureDietID);

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

		public static CreatureDiet RetreiveCreatureDietByID(string id)
		{
			CreatureDiet creatureDiet = null;

			//connect
			var conn = DBConnection.GetDBConnection();

			// command text
			var cmdText = @"sp_retrieve_creatures_diet_by_id";

			//command
			var cmd = new SqlCommand(cmdText, conn);
			cmd.CommandType = CommandType.StoredProcedure;
			cmd.Parameters.AddWithValue("@CreatureDietID", id);

			try
			{
				//open the connection
				conn.Open();
				//execute the command
				var reader = cmd.ExecuteReader();

				// check for return rows
				if (reader.HasRows)
				{
					reader.Read();
					creatureDiet = new CreatureDiet()
					{
						CreatureDietID = reader.GetString(0),
						Active = reader.GetBoolean(1)
					};
						
					
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

			return creatureDiet;
		}
    }
}
