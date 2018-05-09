using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataTransferObjects;
using DataAccessLayer;
namespace LogicLayer
{
    public class MessageManager
    {
        public List<Message> RetrieveMessagesByMarketEntryID(int id)
        {
            var messageList = new List<Message>();

            try
            {
                messageList = MessageAccessor.ReRetrieveMessagesByMarketEntryID(id);
            }
            catch (Exception)
            {
                
                throw;
            }

            return messageList;
        }

        public int AddMessage(Message message)
        {
            int result = 0;

            try
            {
                result = MessageAccessor.CreateMessage(message);
            }
            catch (Exception)
            {
                
                throw;
            }

            return result;
        }
    }
}
