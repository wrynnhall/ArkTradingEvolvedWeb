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
    public class CreatureTypeAccessor
    {
        public static List<CreatureType> RetrieveCreatureTypeList()
        {
            var creatureTypes = new List<CreatureType>();

            //connect
            var conn = DBConnection.GetDBConnection();

            // command text
            var cmdText = @"sp_retrieve_creatures_types";

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
                        var type = new CreatureType()
                        {
                            CreatureTypeID = reader.GetString(0),
                            Active = reader.GetBoolean(1)
                        };
                        creatureTypes.Add(type);
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

            return creatureTypes;
        }

        public static List<CreatureType> RetrieveCreatureTypeListActive()
        {
            var creatureTypes = new List<CreatureType>();

            //connect
            var conn = DBConnection.GetDBConnection();

            // command text
            var cmdText = @"sp_retrieve_creatures_types_active";

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
                        var type = new CreatureType()
                        {
                            CreatureTypeID = reader.GetString(0),
                            Active = reader.GetBoolean(1)
                        };
                        creatureTypes.Add(type);
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

            return creatureTypes;
        }

        public static int UpdateCreatureType(CreatureType oldCreatureType, CreatureType newCreatureType)
        {
            int result = 0;


            var conn = DBConnection.GetDBConnection();
            var cmdText = @"sp_update_creature_type";

            var cmd = new SqlCommand(cmdText, conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@OldCreatureTypeID", oldCreatureType.CreatureTypeID);
            cmd.Parameters.AddWithValue("@NewCreatureTypeID", newCreatureType.CreatureTypeID);

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

		public static CreatureType RetreiveCreatureTypeByID(string id)
		{
			CreatureType type = null;

			//connect
			var conn = DBConnection.GetDBConnection();

			// command text
			var cmdText = @"sp_retrieve_creatures_type_by_id";

			//command
			var cmd = new SqlCommand(cmdText, conn);
			cmd.CommandType = CommandType.StoredProcedure;
			cmd.Parameters.AddWithValue("@CreatureTypeID", id);
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
					type = new CreatureType()
					{
						CreatureTypeID = reader.GetString(0),
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
			
			return type;
		}

        public static int UpdateCreatureTypeActive(string creatureTypeId, bool active)
        {
            int result = 0;

            var conn = DBConnection.GetDBConnection();
            var cmdText = @"sp_update_creature_type_active";

            var cmd = new SqlCommand(cmdText, conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@CreatureTypeID", creatureTypeId);
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

        public static int AddCreatureType(CreatureType creatureType)
        {
            int result = 0;

            var conn = DBConnection.GetDBConnection();
            var cmdText = @"sp_add_creature_type";

            var cmd = new SqlCommand(cmdText, conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@CreatureTypeID", creatureType.CreatureTypeID);

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
