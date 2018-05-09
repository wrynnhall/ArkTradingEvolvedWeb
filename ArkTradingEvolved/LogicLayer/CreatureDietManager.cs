using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer;
using DataTransferObjects;
namespace LogicLayer
{
    public class CreatureDietManager
    {

		public CreatureDiet RetreiveCreatureDietByID(string id)
		{
			CreatureDiet diet = null;

			try
			{
				diet = CreatureDietAccessor.RetreiveCreatureDietByID(id);
			}
			catch (Exception)
			{

				throw;
			}


			return diet;
		}



		public List<CreatureDiet> RetrieveCreatureDietList()
        {
            List<CreatureDiet> diets = null;

            try
            {
                diets = CreatureDietAccessor.RetrieveCreatureDietList();
            }
            catch (Exception)
            {

                throw;
            }

            return diets;
        }


        public int UpdateCreatureDiet(CreatureDiet oldCreatureDiet, CreatureDiet newCreatureDiet)
        {
            int result = 0;

            try
            {
                result = CreatureDietAccessor.UpdateCreatureDiet(oldCreatureDiet, newCreatureDiet);
            }
            catch (Exception)
            {

                throw;
            }

            return result;
        }

        public int UpdateCreatureDietActive(string creatureDietId, bool active)
        {
            int result = 0;

            try
            {
                result = CreatureDietAccessor.UpdateCreatureDietActive(creatureDietId, active);
            }
            catch (Exception)
            {

                throw;
            }

            return result;
        }

        public int AddCreatureDiet(CreatureDiet creatureDiet)
        {
            int result = 0;

            try
            {
                result = CreatureDietAccessor.AddCreatureDiet(creatureDiet);
            }
            catch (Exception)
            {

                throw;
            }

            return result;
        }

        public List<CreatureDiet> RetrieveCreatureDietListActive()
        {
            List<CreatureDiet> diets = null;

            try
            {
                diets = CreatureDietAccessor.RetrieveCreatureDietListActive();
            }
            catch (Exception)
            {

                throw;
            }

            return diets;
        }
    }
}
