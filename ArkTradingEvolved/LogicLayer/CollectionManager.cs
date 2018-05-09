using DataAccessLayer;
using DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicLayer
{
    public class CollectionManager
    {
        public List<Collection> RetrieveCollectionList(int userId)
        {
            var collections = new List<Collection>();

            try
            {
                collections = CollectionAccessor.RetrieveCollectionsByUserId(userId);
            }
            catch (Exception)
            {

                throw;
            }
            
            return collections;
        }

        public List<CollectionEntry> RetrieveCollectionEntries(int collectionId)
        {
            var entries = new List<CollectionEntry>();
            try
            {
                entries = CollectionAccessor.RetrieveCollectionEntriesByCollectionId(collectionId);
            }
            catch (Exception)
            {

                throw;
            }

            return entries;
        }

        public List<Creature> RetrieveCreatures()
        {
            List<Creature> creatures = new List<Creature>();

            try
            {
               creatures = CollectionAccessor.RetrieveCreatures();
            }
            catch (Exception)
            {
                throw;
            }
            
            return creatures;
        }

        public Collection RetreiveCollectionByID(int? id)
        {
            Collection collection = null;

            try
            {
                collection = CollectionAccessor.RetreiveCollectionByID(id);
            }
            catch (Exception)
            {

                throw;
            }

            return collection;
        }

        public CollectionEntry RetreiveCollectionEntryByID(int? id)
        {
            CollectionEntry entry = null;

            try
            {
                entry = CollectionAccessor.RetreiveCollectionEntryByID(id);
            }
            catch (Exception)
            {

                throw;
            }

            return entry;
        }

        public List<CollectionEntryDetails> RetrieveCollectionEntryDetails(int collectionId)
        {
            var entryDetails = new List<CollectionEntryDetails>();
            try
            {
                var entries = CollectionAccessor.RetrieveCollectionEntriesByCollectionId(collectionId);
                foreach (var item in entries)
                {
                    var entryDetail = new CollectionEntryDetails()
                    {
                        CollectionEntry = item,
                        Creature = CollectionAccessor.RetrieveCreature(item.CreatureID)
                    };
                    entryDetails.Add(entryDetail);
                }
            }
            catch (Exception)
            {

                throw;
            }

            return entryDetails;
        }

        public int AddCollection(Collection collection)
        {
            int result = 0;

            try
            {
                result = CollectionAccessor.CreateCollection(collection);
            }
            catch(Exception)
            {
                throw;
            }
            
            return result;
        }

        public int EditCollection(Collection oldCollection, Collection newCollection)
        {
            int result = 0;

            try
            {
                result = CollectionAccessor.UpdateCollection(oldCollection, newCollection);
            }
            catch (Exception)
            {

                throw;
            }
            
            return result;
        }

        public int DeactivateCollection(int collectionID)
        {
            int result = 0;

            try
            {
                result = CollectionAccessor.DeactivateCollection(collectionID);
            }
            catch (Exception)
            {

                throw;
            }

            return result;
        }

        public int AddCollectionEntry(CollectionEntry entry)
        {
            int result = 0;

            try
            {
                result = CollectionAccessor.CreateCollectionEntry(entry);
            }
            catch (Exception)
            {

                throw;
            }


            return result;
        }

        public int EditCollectionEntry(CollectionEntry updateEntry, CollectionEntry collectionEntry)
        {
            int result = 0;

            try
            {
                result = CollectionAccessor.UpdateCollectionEntry(updateEntry, collectionEntry);
            }
            catch (Exception)
            {

                throw;
            }
            
            return result;
        }

        public int DeactivateCollectionEntry(int collectionEntryID)
        {
            int result = 0;

            try
            {
                result = CollectionAccessor.DeactivateCollectionEntry(collectionEntryID);
            }
            catch (Exception)
            {

                throw;
            }

            return result;
        }

		public List<CollectionEntry> RetreiveCollectionEntryListByUserID(int id)
		{
			var entries = new List<CollectionEntry>();
			try
			{
				entries = CollectionAccessor.RetreiveCollectionEntryListByUserID(id);
			}
			catch (Exception)
			{

				throw;
			}

			return entries;
		}

	}
}
