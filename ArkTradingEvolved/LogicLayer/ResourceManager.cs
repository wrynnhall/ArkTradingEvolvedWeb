using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataTransferObjects;
using DataAccessLayer;

namespace LogicLayer
{
    public class ResourceManager
    {
        public List<Resource> RetrieveResources(bool active = true)
        {
            var resources = new List<Resource>();

            try
            {
                resources = ResourceAccessor.RetrieveResources(active);
            }
            catch (Exception)
            {

                throw;
            }
            
            return resources;
        }

        public List<Resource> RetrieveResourcesList()
        {
            var resources = new List<Resource>();

            try
            {
                resources = ResourceAccessor.RetrieveResourceList();
            }
            catch (Exception)
            {

                throw;
            }

            return resources;
        } 

        public int UpdateResource(Resource oldResource, Resource newResource)
        {
            int result = 0;


            try
            {
                result = ResourceAccessor.UpdateResource(oldResource, newResource);
            }
            catch (Exception)
            {

                throw;
            }

            return result;
        }

        public Resource RetreiveResourceByID(string id)
        {
            Resource resource = null;

            try
            {
                resource = ResourceAccessor.RetreiveResourceByID(id);
            }
            catch (Exception)
            {

                throw;
            }
            return resource;
        }

        public int UpdateResourceActive(string resourceId, bool active)
        {
            int result = 0;

            try
            {
                result = ResourceAccessor.UpdateResourceActive(resourceId, active);
            }
            catch (Exception)
            {

                throw;
            }

            return result;
        }

        public int AddResource(Resource resource)
        {
            int result = 0;

            try
            {
                result = ResourceAccessor.AddResource(resource);
            }
            catch (Exception)
            {

                throw;
            }

            return result;
        }

    }
}
