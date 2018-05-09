using DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer;

namespace LogicLayer
{
    public class CreatureManager
    {
        public List<Creature> RetrieveCreatureList()
        {
            List<Creature> creatures = null;

            try
            {
                creatures = CreatureAccessor.RetrieveCreatureList();
            }
            catch (Exception)
            {

                throw;
            }

            return creatures;
        }


        public int UpdateCreature(Creature oldCreature, Creature newCreature)
        {
            int result = 0;


            try
            {
                result = CreatureAccessor.UpdateCreature(oldCreature, newCreature);
            }
            catch (Exception)
            {

                throw;
            }

            return result;
        }


        public int UpdateCreatureActive(string creatureId, bool active)
        {
            int result = 0;


            try
            {
                result = CreatureAccessor.UpdateCreatureActive(creatureId, active);
            }
            catch (Exception)
            {

                throw;
            }

            return result;
        }

        public Creature RetreiveCreatureByID(string id)
        {
            Creature creature = null;

            try
            {
                    creature = CreatureAccessor.RetreiveCreatureByID(id);
            }
            catch (Exception)
            {

                throw;
            }
            return creature;
        }

        public int AddCreature(Creature creature)
        {
            int result = 0;


            try
            {
                result = CreatureAccessor.AddCreature(creature);
            }
            catch (Exception)
            {

                throw;
            }

            return result;
        }

    }
}
