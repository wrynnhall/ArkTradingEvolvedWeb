using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer;
using DataTransferObjects;

namespace LogicLayer
{
    public class MarketEntryManager
    {
        public List<MarketEntryDetails> RetrieveMarketEntryDetailsByStatus(string status)
        {
            var detailList = new List<MarketEntryDetails>();

            try
            {
                var entries = MarketEntryAccessor.RetrieveMarketEntriesByStatus(status);
                foreach(MarketEntry e in entries)
                {
                    var detail = new MarketEntryDetails() {
                        MarketEntry = e,
                        User = UserAccessor.RetrieveUserByMarketEntryID(e.MarketEntryID),
                        CollectionEntry = CollectionAccessor.RetrieveCollectionEntryByID(e.CollectionEntryID)
                    };
                    detailList.Add(detail);
                }
            }
            catch (Exception)
            {
                throw;
            }


            return detailList;
        }

        public List<MarketEntryDetails> RetrieveMarketEntryDetailsByUser(int id)
        {
            var detailList = new List<MarketEntryDetails>();

            try
            {
                var entries = MarketEntryAccessor.RetrieveMarketEntriesByUser(id);
                foreach (MarketEntry e in entries)
                {
                    var detail = new MarketEntryDetails()
                    {
                        MarketEntry = e,
                        User = UserAccessor.RetrieveUserByMarketEntryID(e.MarketEntryID),
                        CollectionEntry = CollectionAccessor.RetrieveCollectionEntryByID(e.CollectionEntryID)
                    };
                    detailList.Add(detail);
                }
            }
            catch (Exception)
            {
                throw;
            }


            return detailList;
        }

        public List<PurchaseDetails> RetrievePurchaseDetailsByUser(int id)
        {
            var purchaseList = new List<PurchaseDetails>();

            try
            {
                var purchases = MarketEntryAccessor.RetrievePurchasesByUser(id);
                foreach (Purchase e in purchases)
                {
                    var marketEntry = MarketEntryAccessor.RetrieveMarketEntryById(e.MarketEntryID);
                    var marketEntryDetail = new MarketEntryDetails()
                    {
                        MarketEntry = marketEntry,
                        User = UserAccessor.RetrieveUserByMarketEntryID(marketEntry.MarketEntryID),
                        CollectionEntry = CollectionAccessor.RetrieveCollectionEntryByID(marketEntry.CollectionEntryID)
                    };
                    var purhcase = new PurchaseDetails()
                    {
                        MarketEntryDetails = marketEntryDetail,
                        User = UserAccessor.RetrieveUserById(e.UserID)
                    };
                    purchaseList.Add(purhcase);
                }
            }
            catch (Exception)
            {
                throw;
            }


            return purchaseList;
        }

        public int UpdateMarketEntryStatus(MarketEntry marketEntry, string status)
        {
            int result = 0;

            try
            {
                result = MarketEntryAccessor.EditMarketEntryStatus(marketEntry, status);
            }
            catch (Exception)
            {

                throw;
            }


            return result;
        }

        public int AddMarketEntryPurchase(User user, MarketEntry marketEntry)
        {
            int result = 0;

            try
            {
                result = MarketEntryAccessor.CreateMarketEntryPurchase(user, marketEntry);
            }
            catch (Exception)
            {

                throw;
            }


            return result;
        }

        public int VerifyMarketEntryCollectionEntryPresence(int collectionEntryId)
        {
            int result = 0;


            try
            {
                result = MarketEntryAccessor.VerifyMarketEntryCollectionEntryPresence(collectionEntryId);
            }
            catch (Exception)
            {

                throw;
            }

            return result;
        }

        public int AddMarketEntry(MarketEntry entry)
        {
            int result = 0;

            try
            {
                result = MarketEntryAccessor.CreateMarketEntry(entry);
            }
            catch (Exception)
            {

                throw;
            }


            return result;
        }

        public int UpdateMarketEntry(MarketEntry updateEntry, MarketEntry oldEntry)
        {
            int result = 0;

            try
            {
                result = MarketEntryAccessor.EditMarketEntry(updateEntry, oldEntry);
            }
            catch (Exception)
            {

                throw;
            }


            return result;
        }

        public int PerformMarketEntryPurchaseComplete(int collectionID, PurchaseDetails purchaseDetails)
        {
            int result = 0;

            try
            {
                
                result = MarketEntryAccessor.PerformMarketEntryPurchaseComplete(collectionID, purchaseDetails);
            }
            catch (Exception)
            {
                
                throw;
            }

            return result;
        }

		public List<MarketEntryDetails> RetreiveMarketEntryPurchaseDetailsByUserID(int id)
		{
			List<MarketEntryDetails> details = null;

			try
			{
				details = MarketEntryAccessor.RetreiveMarketEntryPurchaseDetailsByUserID(id);
			}
			catch (Exception)
			{

				throw;
			}
			return details;
		}

		public PurchaseDetails RetreivePurchaseDetailByID(int id)
		{
			PurchaseDetails details = null;

			try
			{
				details = MarketEntryAccessor.RetreivePurchaseDetailByID(id);
			}
			catch (Exception)
			{

				throw;
			}

			return details;
		}

		public List<MarketEntryDetails> RetreiveMarketEntryDetailsByUserID(int id)
		{
			List<MarketEntryDetails> details = null;

			try
			{
				details = MarketEntryAccessor.RetreiveMarketEntryDetailsByUserID(id);
			}
			catch (Exception)
			{

				throw;
			}



			return details;
		}

		public MarketEntryDetails RetreiveMarketEntryDetailByID(int id)
		{
			MarketEntryDetails details = null;

			try
			{
				details = MarketEntryAccessor.RetreiveMarketEntryDetailByID(id);
			}
			catch (Exception)
			{

				throw;
			}

			return details;
		}

		public List<MarketEntryDetails> RetreiveMarketEntryDetailsByAvailable()
		{
			List<MarketEntryDetails> details = null;

			try
			{
				details = MarketEntryAccessor.RetreiveMarketEntryDetailsByAvailable();
			}
			catch (Exception)
			{

				throw;
			}



			return details;
		}
	}
}
