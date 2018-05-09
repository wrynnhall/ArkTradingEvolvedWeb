using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTransferObjects
{
    public class MarketEntry
    {
        public int MarketEntryID { get; set; }
        public int CollectionEntryID { get; set; }
        public string MarketEntryStatusID { get; set; }
        public string ResourceID { get; set; }
        public int ResourceAmount { get; set; }
        public bool Active { get; set; }

        /*[MarketEntryID]	[int] IDENTITY(400000,1)	NOT NULL,
	        [CollectionEntryID]		[int]			NOT NULL,
	        [MarketEntryStatusID]	[nvarchar](30)	NOT NULL,
	        [ResourceID]			[nvarchar](30)	NOT NULL,
	        [Units]					[int]			NOT NULL,
	        [Active]				[bit]			NOT NULL DEFAULT 1,*/
    }
}
