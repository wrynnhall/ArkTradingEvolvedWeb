using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer;
using DataTransferObjects;
namespace LogicLayer
{
    public class CreatureTypeManager
    {
        public List<CreatureType> RetrieveCreatureTypeList()
        {
            List<CreatureType> types = null;

            try
            {
                types = CreatureTypeAccessor.RetrieveCreatureTypeList();
            }
            catch (Exception)
            {

                throw;
            }

            return types;
        }

        public List<CreatureType> RetrieveCreatureTypeListActive()
        {
            List<CreatureType> types = null;

            try
            {
                types = CreatureTypeAccessor.RetrieveCreatureTypeListActive();
            }
            catch (Exception)
            {

                throw;
            }

            return types;
        }

        public int UpdateCreatureType(CreatureType oldCreatureType, CreatureType newCreatureType)
        {
            int result = 0;

            try
            {
                result = CreatureTypeAccessor.UpdateCreatureType(oldCreatureType, newCreatureType);
            }
            catch (Exception)
            {

                throw;
            }

            return result;
        }

        public int UpdateCreatureTypeActive(string creatureTypeId, bool active)
        {
            int result = 0;

            try
            {
                result = CreatureTypeAccessor.UpdateCreatureTypeActive(creatureTypeId, active);
            }
            catch (Exception)
            {

                throw;
            }

            return result;
        }

        public int AddCreatureType(CreatureType creatureType)
        {
            int result = 0;

            try
            {
                result = CreatureTypeAccessor.AddCreatureType(creatureType);
            }
            catch (Exception)
            {

                throw;
            }

            return result;
        }

		public CreatureType RetreiveCreatureTypeByID(string id)
		{
			CreatureType type = null;

			try
			{
				type = CreatureTypeAccessor.RetreiveCreatureTypeByID(id);
			}
			catch (Exception)
			{

				throw;
			}

			return type;
		}

    }
}
