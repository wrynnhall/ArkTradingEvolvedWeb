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
    public static class CollectionAccessor
    {
        

        public static List<Collection> RetrieveCollectionsByUserId(int id)
        {
             var collections = new List<Collection>();

            var conn = DBConnection.GetDBConnection();

            var cmdText = @"sp_select_collections_by_active";
            var cmd = new SqlCommand(cmdText, conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@UserID", id);
            cmd.Parameters.AddWithValue("@Active", true);

            try
            {
                conn.Open();
                var reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        var collection = new Collection()
                        {
                            CollectionID = reader.GetInt32(0),
                            UserID = reader.GetInt32(1),
                            Name = reader.GetString(2),
                            Active = reader.GetBoolean(3)
                        };

                        collections.Add(collection);
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

            return collections;
        }

        public static Collection RetreiveCollectionByID(int? id)
        {
            Collection collection = null;

           
            var conn = DBConnection.GetDBConnection();

            var cmdText = @"sp_select_collection_by_id";
            var cmd = new SqlCommand(cmdText, conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@CollectionID", id);

            try
            {
                conn.Open();
                var reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    reader.Read();
                    collection = new Collection()
                    {
                        CollectionID = reader.GetInt32(0),
                        UserID = reader.GetInt32(1),
                        Name = reader.GetString(2),
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


            return collection;
        }

        public static CollectionEntry RetreiveCollectionEntryByID(int? id)
        {
            CollectionEntry entry = null;

            var conn = DBConnection.GetDBConnection();

            var cmdText = @"sp_select_collection_entry_by_id";
            var cmd = new SqlCommand(cmdText, conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@CollectionEntryID", id);
            try
            {
                conn.Open();
                var reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    reader.Read();
                    entry = new CollectionEntry()
                    {
                        CollectionEntryID = reader.GetInt32(0),
                        CollectionID = reader.GetInt32(1),
                        CreatureID = reader.GetString(2),
                        Name = reader.GetString(3),
                        Level = reader.GetInt32(4),
                        Health = reader.GetInt32(5),
                        Stamina = reader.GetInt32(6),
                        Oxygen = reader.GetInt32(7),
                        Food = reader.GetInt32(8),
                        Weight = reader.GetInt32(9),
                        BaseDamage = reader.GetInt32(10),
                        MovementSpeed = reader.GetInt32(11),
                        Torpor = reader.GetInt32(12),
                        Imprint = reader.GetInt32(13),
                        Active = reader.GetBoolean(14)
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


            return entry;
        }

        public static Creature RetrieveCreature(string creatureID)
        {
            Creature creature = null;

            /**
             * [CreatureID]	[nvarchar](50)	NOT NULL,
     [Oxygen]		[bit]			NOT NULL DEFAULT 1,
     [CreatureTypeID]	[nvarchar](30)	NOT NULL,
     [CreatureDietID]	[nvarchar](30)	NOT NULL,
             */
            var conn = DBConnection.GetDBConnection();

            var cmdText = @"sp_select_creature_by_id";
            var cmd = new SqlCommand(cmdText, conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@CreatureID", creatureID);

            try
            {
                conn.Open();
                var reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    reader.Read();
                    creature = new Creature() {
                        CreatureID = reader.GetString(0),
                        CreatureTypeID = reader.GetString(1),
                        CreatureDietID = reader.GetString(2)
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

        public static List<Creature> RetrieveCreatures()
        {
            List<Creature> creatures = new List<Creature>();

            /**
             * [CreatureID]	[nvarchar](50)	NOT NULL,
     [Oxygen]		[bit]			NOT NULL DEFAULT 1,
     [CreatureTypeID]	[nvarchar](30)	NOT NULL,
     [CreatureDietID]	[nvarchar](30)	NOT NULL,
             */
            var conn = DBConnection.GetDBConnection();

            var cmdText = @"sp_select_creatures";
            var cmd = new SqlCommand(cmdText, conn);
            cmd.CommandType = CommandType.StoredProcedure;
            

            try
            {
                conn.Open();
                var reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        var creature = new Creature()
                        {
                            CreatureID = reader.GetString(0),
                            CreatureTypeID = reader.GetString(1),
                            CreatureDietID = reader.GetString(2)
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

        public static int DeactivateCollectionEntry(int collectionEntryID)
        {
            int result = 0;

            var conn = DBConnection.GetDBConnection();
            var cmdText = @"sp_update_collection_entry_active";
            var cmd = new SqlCommand(cmdText, conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@CollectionEntryID", collectionEntryID);
            cmd.Parameters.AddWithValue("@Active", false);

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

        public static int UpdateCollectionEntry(CollectionEntry updateEntry, CollectionEntry collectionEntry)
        {

            int result = 0;

            var conn = DBConnection.GetDBConnection();
            var cmdText = @"sp_update_collection_entry";
            var cmd = new SqlCommand(cmdText, conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@CollectionEntryID", collectionEntry.CollectionEntryID);
            cmd.Parameters.AddWithValue("@OldCreatureID", collectionEntry.CreatureID);
            cmd.Parameters.AddWithValue("@NewCreatureID", updateEntry.CreatureID);
            cmd.Parameters.AddWithValue("@OldName", collectionEntry.Name);
            cmd.Parameters.AddWithValue("@NewName", updateEntry.Name);
            cmd.Parameters.AddWithValue("@OldLevel", collectionEntry.Level);
            cmd.Parameters.AddWithValue("@NewLevel", updateEntry.Level);
            cmd.Parameters.AddWithValue("@OldHealth", collectionEntry.Health);
            cmd.Parameters.AddWithValue("@NewHealth", updateEntry.Health);
            cmd.Parameters.AddWithValue("@OldStamina", collectionEntry.Stamina);
            cmd.Parameters.AddWithValue("@NewStamina", updateEntry.Stamina);
            cmd.Parameters.AddWithValue("@OldOxygen", collectionEntry.Oxygen);
            cmd.Parameters.AddWithValue("@NewOxygen", updateEntry.Oxygen);
            cmd.Parameters.AddWithValue("@OldFood", collectionEntry.Food);
            cmd.Parameters.AddWithValue("@NewFood", updateEntry.Food);
            cmd.Parameters.AddWithValue("@OldWeight", collectionEntry.Weight);
            cmd.Parameters.AddWithValue("@NewWeight", updateEntry.Weight);
            cmd.Parameters.AddWithValue("@OldBaseDamage", collectionEntry.BaseDamage);
            cmd.Parameters.AddWithValue("@NewBaseDamage", updateEntry.BaseDamage);
            cmd.Parameters.AddWithValue("@OldMovementSpeed", collectionEntry.MovementSpeed);
            cmd.Parameters.AddWithValue("@NewMovementSpeed", updateEntry.MovementSpeed);
            cmd.Parameters.AddWithValue("@OldTorpor", collectionEntry.Torpor);
            cmd.Parameters.AddWithValue("@NewTorpor", updateEntry.Torpor);
            cmd.Parameters.AddWithValue("@OldImprint", collectionEntry.Imprint);
            cmd.Parameters.AddWithValue("@NewImprint", updateEntry.Imprint);


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

        public static List<CollectionEntry> RetrieveCollectionEntriesByCollectionId(int id)
        {
            var entries = new List<CollectionEntry>();

            var conn = DBConnection.GetDBConnection();

            var cmdText = @"sp_select_collection_entries_by_active";
            var cmd = new SqlCommand(cmdText, conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@CollectionID", id);
            cmd.Parameters.AddWithValue("@Active", true);
            try
            {
                conn.Open();
                var reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        var entry = new CollectionEntry() {
                            CollectionEntryID = reader.GetInt32(0),
                            CollectionID = reader.GetInt32(1),
                            CreatureID = reader.GetString(2),
                            Name = reader.GetString(3),
                            Level = reader.GetInt32(4),
                            Health = reader.GetInt32(5),
                            Stamina = reader.GetInt32(6),
                            Oxygen = reader.GetInt32(7),
                            Food = reader.GetInt32(8),
                            Weight = reader.GetInt32(9),
                            BaseDamage = reader.GetInt32(10),
                            MovementSpeed = reader.GetInt32(11),
                            Torpor = reader.GetInt32(12),
                            Imprint = reader.GetInt32(13),
                            Active = reader.GetBoolean(14)
                        };

                        entries.Add(entry);
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


            return entries;
        }

        public static int DeactivateCollection(int collectionID)
        {
            int result = 0;

            var conn = DBConnection.GetDBConnection();
            var cmdText = @"sp_update_collection_active";
            var cmd = new SqlCommand(cmdText, conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@CollectionID", collectionID);
            cmd.Parameters.AddWithValue("@Active", false);

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

        public static int CreateCollection(Collection collection)
        {

            int result = 0;
            var conn = DBConnection.GetDBConnection();

            var cmdText = @"sp_create_collection";
            var cmd = new SqlCommand(cmdText, conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@UserID", collection.UserID);
            cmd.Parameters.AddWithValue("@Name", collection.Name);

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

        public static int CreateCollectionEntry(CollectionEntry entry)
        {
            int result = 0;
            var conn = DBConnection.GetDBConnection();

            var cmdText = @"sp_create_collection_entry";
            var cmd = new SqlCommand(cmdText, conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@CollectionID", entry.CollectionID);
            cmd.Parameters.AddWithValue("@CreatureID", entry.CreatureID);
            cmd.Parameters.AddWithValue("@Name", entry.Name);
            cmd.Parameters.AddWithValue("@Level", entry.Level);
            cmd.Parameters.AddWithValue("@Health", entry.Health);
            cmd.Parameters.AddWithValue("@Stamina", entry.Stamina);
            cmd.Parameters.AddWithValue("@Oxygen", entry.Oxygen);
            cmd.Parameters.AddWithValue("@Food", entry.Food);
            cmd.Parameters.AddWithValue("@Weight", entry.Weight);
            cmd.Parameters.AddWithValue("@BaseDamage", entry.BaseDamage);
            cmd.Parameters.AddWithValue("@MovementSpeed", entry.MovementSpeed);
            cmd.Parameters.AddWithValue("@Torpor", entry.Torpor);
            cmd.Parameters.AddWithValue("@Imprint", entry.Imprint);
            cmd.Parameters.AddWithValue("@Active", entry.Active);

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

        public static int UpdateCollection(Collection oldCollection, Collection newCollection)
        {
            int result = 0;

            var conn = DBConnection.GetDBConnection();
            var cmdText = @"sp_update_collection";
            var cmd = new SqlCommand(cmdText, conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@CollectionID", oldCollection.CollectionID);
            cmd.Parameters.AddWithValue("@OldName", oldCollection.Name);
            cmd.Parameters.AddWithValue("@NewName", newCollection.Name);

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

        public static CollectionEntry RetrieveCollectionEntryByID(int id)
        {
            CollectionEntry entry = null;

            var conn = DBConnection.GetDBConnection();

            var cmdText = @"sp_select_collection_entry_by_id";
            var cmd = new SqlCommand(cmdText, conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@CollectionEntryID", id);
            try
            {
                conn.Open();
                var reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    reader.Read();
                        entry = new CollectionEntry()
                        {
                            CollectionEntryID = reader.GetInt32(0),
                            CollectionID = reader.GetInt32(1),
                            CreatureID = reader.GetString(2),
                            Name = reader.GetString(3),
                            Level = reader.GetInt32(4),
                            Health = reader.GetInt32(5),
                            Stamina = reader.GetInt32(6),
                            Oxygen = reader.GetInt32(7),
                            Food = reader.GetInt32(8),
                            Weight = reader.GetInt32(9),
                            BaseDamage = reader.GetInt32(10),
                            MovementSpeed = reader.GetInt32(11),
                            Torpor = reader.GetInt32(12),
                            Imprint = reader.GetInt32(13),
                            Active = reader.GetBoolean(14)
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


            return entry;
        }

		public static List<CollectionEntry> RetreiveCollectionEntryListByUserID(int id)
		{
			var entries = new List<CollectionEntry>();

			var conn = DBConnection.GetDBConnection();

			var cmdText = @"sp_select_collection_entries_by_user_id";
			var cmd = new SqlCommand(cmdText, conn);
			cmd.CommandType = CommandType.StoredProcedure;

			cmd.Parameters.AddWithValue("@UserID", id);
			try
			{
				conn.Open();
				var reader = cmd.ExecuteReader();

				if (reader.HasRows)
				{
					while (reader.Read())
					{
						var entry = new CollectionEntry()
						{
							CollectionEntryID = reader.GetInt32(0),
							CollectionID = reader.GetInt32(1),
							CreatureID = reader.GetString(2),
							Name = reader.GetString(3),
							Level = reader.GetInt32(4),
							Health = reader.GetInt32(5),
							Stamina = reader.GetInt32(6),
							Oxygen = reader.GetInt32(7),
							Food = reader.GetInt32(8),
							Weight = reader.GetInt32(9),
							BaseDamage = reader.GetInt32(10),
							MovementSpeed = reader.GetInt32(11),
							Torpor = reader.GetInt32(12),
							Imprint = reader.GetInt32(13),
							Active = reader.GetBoolean(14)
						};

						entries.Add(entry);
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


			return entries;
		}

    }
    
}
