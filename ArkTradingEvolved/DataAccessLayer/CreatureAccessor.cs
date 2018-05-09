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
    public class CreatureAccessor
    {
        public static List<Creature> RetrieveCreatureList()
        {
            var creatures = new List<Creature>();

            //connect
            var conn = DBConnection.GetDBConnection();

            // command text
            var cmdText = @"sp_retrieve_creatures";

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
                        var creature = new Creature()
                        {
                            CreatureID = reader.GetString(0),
                            CreatureTypeID = reader.GetString(1),
                            CreatureDietID = reader.GetString(2),
                            Active = reader.GetBoolean(3)
                        };
                        creatures.Add(creature);
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

            return creatures;
        }

        public static Creature RetreiveCreatureByID(string id)
        {
            Creature creature = null;

            //connect
            var conn = DBConnection.GetDBConnection();

            // command text
            var cmdText = @"sp_select_creature_by_id";

            //command
            var cmd = new SqlCommand(cmdText, conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@CreatureID", id);



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
                    creature = new Creature()
                    {
                        CreatureID = reader.GetString(0),
                        CreatureTypeID = reader.GetString(1),
                        CreatureDietID = reader.GetString(2),
                        Active = reader.GetBoolean(3)
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

            return creature;
        }

        public static int UpdateCreature(Creature oldCreature, Creature newCreature)
        {
            int result = 0;

            var conn = DBConnection.GetDBConnection();
            var cmdText = @"sp_update_creature";

            var cmd = new SqlCommand(cmdText, conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@OldCreatureID", oldCreature.CreatureID);
            cmd.Parameters.AddWithValue("@NewCreatureID", newCreature.CreatureID);
            cmd.Parameters.AddWithValue("@OldCreatureTypeID", oldCreature.CreatureTypeID);
            cmd.Parameters.AddWithValue("@OldCreatureDietID", oldCreature.CreatureDietID);
            cmd.Parameters.AddWithValue("@NewCreatureTypeID", newCreature.CreatureTypeID);
            cmd.Parameters.AddWithValue("@NewCreatureDietID", newCreature.CreatureDietID);

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

        public static int UpdateCreatureActive(string creatureId, bool active)
        {
            int result = 0;
            
            var conn = DBConnection.GetDBConnection();
            var cmdText = @"sp_update_creature_active";

            var cmd = new SqlCommand(cmdText, conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@CreatureID", creatureId);
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

        public static int AddCreature(Creature creature)
        {
            int result = 0;

            var conn = DBConnection.GetDBConnection();
            var cmdText = @"sp_add_creature";

            var cmd = new SqlCommand(cmdText, conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@CreatureID", creature.CreatureID);
            cmd.Parameters.AddWithValue("@CreatureTypeID", creature.CreatureTypeID);
            cmd.Parameters.AddWithValue("@CreatureDietID", creature.CreatureDietID);

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
