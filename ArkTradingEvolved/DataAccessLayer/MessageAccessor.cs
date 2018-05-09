using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataTransferObjects;
using System.Data.SqlClient;
using System.Data;

namespace DataAccessLayer
{
    public static class MessageAccessor
    {

        public static List<Message> ReRetrieveMessagesByMarketEntryID(int id)
        {
            var messages = new List<Message>();

            //connect
            var conn = DBConnection.GetDBConnection();

            // command text
            var cmdText = @"sp_retrieve_message_by_market_entry_id";

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

                // check for return rows
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        var message = new Message()
                        {
                            /*[MessageID], [UserID], [MarketEntryID], [Text], [SentTime]*/ 
                            MessageID = reader.GetInt32(0),
                            UserID = reader.GetInt32(1),
                            MarketEntryID = reader.GetInt32(2),
                            Text = reader.GetString(3),
                            SentTime = reader.GetDateTime(4)
                        };
                        messages.Add(message);
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

            return messages;
        }

        public static int CreateMessage(Message message)
        {
            int result = 0;

            var conn = DBConnection.GetDBConnection();
            var cmdText = @"sp_create_message";

            var cmd = new SqlCommand(cmdText, conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@UserID", message.UserID);
            cmd.Parameters.AddWithValue("@MarketEntryID", message.MarketEntryID);
            cmd.Parameters.AddWithValue("@Text", message.Text);
            cmd.Parameters.AddWithValue("@SentTime", message.SentTime);

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
