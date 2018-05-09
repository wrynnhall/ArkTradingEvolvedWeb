using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataTransferObjects;
using System.Data;
using System.Data.SqlClient;

namespace DataAccessLayer
{
    public static class MarketEntryAccessor
    {
        public static List<MarketEntry> RetrieveMarketEntriesByStatus(string status)
        {
            var entries = new List<MarketEntry>();

            //connect
            var conn = DBConnection.GetDBConnection();

            // command text
            var cmdText = @"sp_select_market_entries_by_status";

            //command
            var cmd = new SqlCommand(cmdText, conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@MarketEntryStatusID", status);

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
                        var entry = new MarketEntry()
                        {
                            MarketEntryID = reader.GetInt32(0),
                            CollectionEntryID = reader.GetInt32(1),
                            MarketEntryStatusID = reader.GetString(2),
                            ResourceID = reader.GetString(3),
                            ResourceAmount = reader.GetInt32(4),
                            Active = reader.GetBoolean(5)
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

        public static List<MarketEntry> RetrieveMarketEntriesByUser(int id)
        {
            
            var entries = new List<MarketEntry>();

            //connect
            var conn = DBConnection.GetDBConnection();

            // command text
            var cmdText = @"sp_select_market_entries_by_user";

            //command
            var cmd = new SqlCommand(cmdText, conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@UserID", id);

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
                        var entry = new MarketEntry()
                        {
                            MarketEntryID = reader.GetInt32(0),
                            CollectionEntryID = reader.GetInt32(1),
                            MarketEntryStatusID = reader.GetString(2),
                            ResourceID = reader.GetString(3),
                            ResourceAmount = reader.GetInt32(4),
                            Active = reader.GetBoolean(5)
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

        public static MarketEntry RetrieveMarketEntryById(int id)
        {
            MarketEntry entry = null;

            //connect
            var conn = DBConnection.GetDBConnection();

            // command text
            var cmdText = @"sp_select_market_entry_by_id";

            //command
            var cmd = new SqlCommand(cmdText, conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@MarketEntryID", id);

            try
            {
                //open the connection
                conn.Open();
                //execute the command
                var reader = cmd.ExecuteReader();
                
                reader.Read();
                
                entry = new MarketEntry()
                {
                    MarketEntryID = reader.GetInt32(0),
                    CollectionEntryID = reader.GetInt32(1),
                    MarketEntryStatusID = reader.GetString(2),
                    ResourceID = reader.GetString(3),
                    ResourceAmount = reader.GetInt32(4),
                    Active = reader.GetBoolean(5)
                };
                
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

        public static int EditMarketEntry(MarketEntry updateEntry, MarketEntry oldEntry)
        {
            int result = 0;

            var conn = DBConnection.GetDBConnection();
            var cmdText = @"sp_update_market_entry";

            var cmd = new SqlCommand(cmdText, conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@MarketEntryID", oldEntry.MarketEntryID);
            cmd.Parameters.AddWithValue("@OldResourceID", oldEntry.ResourceID);
            cmd.Parameters.AddWithValue("@NewResourceID", updateEntry.ResourceID);
            cmd.Parameters.AddWithValue("@OldUnits", oldEntry.ResourceAmount);
            cmd.Parameters.AddWithValue("@NewUnits", updateEntry.ResourceAmount);
            
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

        public static List<Purchase> RetrievePurchasesByUser(int id)
        {
            var purchases = new List<Purchase>();

            //connect
            var conn = DBConnection.GetDBConnection();

            // command text
            var cmdText = @"sp_select_market_entry_purchases_by_user";

            //command
            var cmd = new SqlCommand(cmdText, conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@UserID", id);

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
                        var purchase = new Purchase()
                        {
                            UserID = reader.GetInt32(0),
                            MarketEntryID = reader.GetInt32(1)
                        };
                        purchases.Add(purchase);
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

            return purchases;
        }

        public static int CreateMarketEntryPurchase(User user, MarketEntry marketEntry)
        {
            int result = 0;

            var conn = DBConnection.GetDBConnection();
            var cmdText = @"sp_perform_market_entry_purchase";

            var cmd = new SqlCommand(cmdText, conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@UserID", user.UserID);
            cmd.Parameters.AddWithValue("@MarketEntryID", marketEntry.MarketEntryID);

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

        public static int EditMarketEntryStatus(MarketEntry marketEntry, string status)
        {
            int result = 0;

            var conn = DBConnection.GetDBConnection();
            var cmdText = @"sp_update_market_entry_status";

            var cmd = new SqlCommand(cmdText, conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@MarketEntryID", marketEntry.MarketEntryID);
            cmd.Parameters.AddWithValue("@NewMarketEntryStatusID", status);
            cmd.Parameters.AddWithValue("@OldMarketEntryStatusID", marketEntry.MarketEntryStatusID);

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

        public static int VerifyMarketEntryCollectionEntryPresence(int collectionEntryId)
        {
            //sp_retrieve_market_entry_count_by_collection_entry_id
            var result = 0; //return the number of rows found (should be 0)

            //need to start with a connection
            var conn = DBConnection.GetDBConnection();

            //next, we need command text - a procedure name or query string
            var cmdTxt = @"sp_retrieve_market_entry_count_by_collection_entry_id";

            //next, we use the connection and command text to create a Command
            var cmd = new SqlCommand(cmdTxt, conn);

            //for a stored procedure, we need to set the command type
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@CollectionEntryID", collectionEntryId);
            //set the parameter values 
            

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

        public static int CreateMarketEntry(MarketEntry entry)
        {
            int result = 0;
            
            var conn = DBConnection.GetDBConnection();
            var cmdText = @"sp_create_market_entry";

            var cmd = new SqlCommand(cmdText, conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@CollectionEntryID", entry.CollectionEntryID);
            cmd.Parameters.AddWithValue("@ResourceID", entry.ResourceID);
            cmd.Parameters.AddWithValue("@Units", entry.ResourceAmount);

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

        public static int PerformMarketEntryPurchaseComplete(int collectionID, PurchaseDetails purchaseDetails)
        {
            int result = 0;

            var conn = DBConnection.GetDBConnection();
            var cmdText = @"sp_perform_market_entry_complete";

            var cmd = new SqlCommand(cmdText, conn);
            cmd.CommandType = CommandType.StoredProcedure;
            
            cmd.Parameters.AddWithValue("@CollectionID", collectionID);
            cmd.Parameters.AddWithValue("@CreatureID", purchaseDetails.MarketEntryDetails.CollectionEntry.CreatureID);
            cmd.Parameters.AddWithValue("@Name", purchaseDetails.MarketEntryDetails.CollectionEntry.Name);
            cmd.Parameters.AddWithValue("@Level", purchaseDetails.MarketEntryDetails.CollectionEntry.Level);
            cmd.Parameters.AddWithValue("@Health", purchaseDetails.MarketEntryDetails.CollectionEntry.Health);
            cmd.Parameters.AddWithValue("@Stamina", purchaseDetails.MarketEntryDetails.CollectionEntry.Stamina);
            cmd.Parameters.AddWithValue("@Oxygen", purchaseDetails.MarketEntryDetails.CollectionEntry.Oxygen);
            cmd.Parameters.AddWithValue("@Food", purchaseDetails.MarketEntryDetails.CollectionEntry.Food);
            cmd.Parameters.AddWithValue("@Weight", purchaseDetails.MarketEntryDetails.CollectionEntry.Weight);
            cmd.Parameters.AddWithValue("@BaseDamage", purchaseDetails.MarketEntryDetails.CollectionEntry.BaseDamage);
            cmd.Parameters.AddWithValue("@MovementSpeed", purchaseDetails.MarketEntryDetails.CollectionEntry.MovementSpeed);
            cmd.Parameters.AddWithValue("@Torpor", purchaseDetails.MarketEntryDetails.CollectionEntry.Torpor);
            cmd.Parameters.AddWithValue("@Imprint", purchaseDetails.MarketEntryDetails.CollectionEntry.Imprint);
            cmd.Parameters.AddWithValue("@Active", purchaseDetails.MarketEntryDetails.CollectionEntry.Active);
            cmd.Parameters.AddWithValue("@MarketEntryID", purchaseDetails.MarketEntryDetails.MarketEntry.MarketEntryID);
            cmd.Parameters.AddWithValue("@CollectionEntryID", purchaseDetails.MarketEntryDetails.CollectionEntry.CollectionEntryID);
            

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

		public static List<MarketEntryDetails> RetreiveMarketEntryPurchaseDetailsByUserID(int id)
		{
			List<MarketEntryDetails> details = new List<MarketEntryDetails>();

			//connect
			var conn = DBConnection.GetDBConnection();

			// command text
			var cmdText = @"sp_select_market_entry_purchases_by_user_2";

			//command
			var cmd = new SqlCommand(cmdText, conn);
			cmd.CommandType = CommandType.StoredProcedure;

			cmd.Parameters.AddWithValue("@UserID", id);

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
						var detail = new MarketEntryDetails()
						{
							CollectionEntry = new CollectionEntry
							{
								CreatureID = reader.GetString(0),
								Level = reader.GetInt32(1),
								Name = reader.GetString(2)
							},
							User = new User
							{
								Gamertag = reader.GetString(3)
							},
							MarketEntry = new MarketEntry
							{
								MarketEntryID = reader.GetInt32(4),
								ResourceID = reader.GetString(5),
								ResourceAmount = reader.GetInt32(6)
							}
						};
						details.Add(detail);
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



			return details;
		}

		public static PurchaseDetails RetreivePurchaseDetailByID(int id)
		{
			PurchaseDetails detail = null;

			//connect
			var conn = DBConnection.GetDBConnection();

			// command text
			var cmdText = @"sp_select_market_entry_details_by_id";

			//command
			var cmd = new SqlCommand(cmdText, conn);
			cmd.CommandType = CommandType.StoredProcedure;

			cmd.Parameters.AddWithValue("@MarketEntryID", id);

			try
			{
				//open the connection
				conn.Open();
				//execute the command
				var reader = cmd.ExecuteReader();

				reader.Read();

				detail = new PurchaseDetails()
				{
					MarketEntryDetails = new MarketEntryDetails
					{
						MarketEntry = new MarketEntry
						{
							MarketEntryID = reader.GetInt32(0),
							CollectionEntryID = reader.GetInt32(1),
							ResourceID = reader.GetString(15),
							ResourceAmount = reader.GetInt32(16),
							MarketEntryStatusID = reader.GetString(17)
						},
						CollectionEntry = new CollectionEntry
						{
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
						}
					},
					User = new User()

				};//[MarketEntry].[MarketEntryID], [MarketEntry].[CollectionEntryID], [CollectonEntry].[CreatureID], 
				//[CollectonEntry].[Name], [CollectonEntry].[Level], [CollectonEntry].[Health], [CollectonEntry].[Stamina], 
				//[CollectonEntry].[Oxygen], [CollectonEntry].[Food], [CollectonEntry].[Weight], [CollectonEntry].[BaseDamage], 
				//[CollectonEntry].[MovementSpeed], [CollectonEntry].[Torpor], [CollectonEntry].[Imprint], [CollectonEntry].[Active]

			}
			catch (Exception)
			{
				throw;
			}
			finally
			{
				conn.Close();
			}


			return detail;
		}

		public static List<MarketEntryDetails> RetreiveMarketEntryDetailsByUserID(int id)
		{
			List<MarketEntryDetails> details = new List<MarketEntryDetails>();

			//connect
			var conn = DBConnection.GetDBConnection();

			// command text
			var cmdText = @"sp_select_market_entries_by_user_2";

			//command
			var cmd = new SqlCommand(cmdText, conn);
			cmd.CommandType = CommandType.StoredProcedure;

			cmd.Parameters.AddWithValue("@UserID", id);

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
						var detail = new MarketEntryDetails()
						{
							MarketEntry = new MarketEntry
							{
								MarketEntryID = reader.GetInt32(0),
								CollectionEntryID = reader.GetInt32(1),
								ResourceID = reader.GetString(15),
								ResourceAmount = reader.GetInt32(16),
								MarketEntryStatusID = reader.GetString(17)
							},
							CollectionEntry = new CollectionEntry
							{
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
							}
						};
						details.Add(detail);
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

			return details;
		}

		public static MarketEntryDetails RetreiveMarketEntryDetailByID(int id)
		{
			MarketEntryDetails detail = null;

			//connect
			var conn = DBConnection.GetDBConnection();

			// command text
			var cmdText = @"sp_select_market_entry_details_by_id";

			//command
			var cmd = new SqlCommand(cmdText, conn);
			cmd.CommandType = CommandType.StoredProcedure;

			cmd.Parameters.AddWithValue("@MarketEntryID", id);

			try
			{
				//open the connection
				conn.Open();
				//execute the command
				var reader = cmd.ExecuteReader();

				reader.Read();

				detail = new MarketEntryDetails()
				{
					
					MarketEntry = new MarketEntry
					{
						MarketEntryID = reader.GetInt32(0),
						CollectionEntryID = reader.GetInt32(1),
						ResourceID = reader.GetString(15),
						ResourceAmount = reader.GetInt32(16),
						MarketEntryStatusID = reader.GetString(17)
					},
					CollectionEntry = new CollectionEntry
					{
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
					}
					

				};//[MarketEntry].[MarketEntryID], [MarketEntry].[CollectionEntryID], [CollectonEntry].[CreatureID], 
				  //[CollectonEntry].[Name], [CollectonEntry].[Level], [CollectonEntry].[Health], [CollectonEntry].[Stamina], 
				  //[CollectonEntry].[Oxygen], [CollectonEntry].[Food], [CollectonEntry].[Weight], [CollectonEntry].[BaseDamage], 
				  //[CollectonEntry].[MovementSpeed], [CollectonEntry].[Torpor], [CollectonEntry].[Imprint], [CollectonEntry].[Active]

			}
			catch (Exception)
			{
				throw;
			}
			finally
			{
				conn.Close();
			}


			return detail;
		}

		public static List<MarketEntryDetails> RetreiveMarketEntryDetailsByAvailable()
		{//sp_select_market_entry_details_by_available
			List<MarketEntryDetails> details = new List<MarketEntryDetails>();

			//connect
			var conn = DBConnection.GetDBConnection();

			// command text
			var cmdText = @"sp_select_market_entry_details_by_available";

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
						var detail = new MarketEntryDetails()
						{
							MarketEntry = new MarketEntry
							{
								MarketEntryID = reader.GetInt32(0),
								CollectionEntryID = reader.GetInt32(1),
								ResourceID = reader.GetString(15),
								ResourceAmount = reader.GetInt32(16),
								MarketEntryStatusID = reader.GetString(17)
							},
							CollectionEntry = new CollectionEntry
							{
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
							},
							User = new User
							{
								Gamertag = reader.GetString(18),
								UserID = reader.GetInt32(19)
							}
						};
						details.Add(detail);
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

			return details;
		}
	}
}
