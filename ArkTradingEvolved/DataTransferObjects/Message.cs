using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTransferObjects
{
    public class Message
    {
        public int MessageID { get; set; }
        public int UserID { get; set; }
        public int MarketEntryID { get; set; }
        public string  Text { get; set; }
        public DateTime SentTime { get; set; }
    }
}
