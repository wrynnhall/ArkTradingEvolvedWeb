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
    public static class ResourceAccessor
    {
        public static List<Resource> RetrieveResources(bool active)
        {
            //sp_retrieve_resources_by_active
            var resources = new List<Resource>();

            var conn = DBConnection.GetDBConnection();

            var cmdText = @"sp_retrieve_resources_by_active";
            var cmd = new SqlCommand(cmdText, conn);
            cmd.CommandType = CommandType.StoredProcedure;
            
            cmd.Parameters.AddWithValue("@Active", active);
            try
            {
                conn.Open();
                var reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        var resource = new Resource()
                        {
                            ResourceID = reader.GetString(0),
                            Active = reader.GetBoolean(1)
                        };

                        resources.Add(resource);
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


            return resources;
        }

        public static List<Resource> RetrieveResourceList()
        {
            var resources = new List<Resource>();

            //connect
            var conn = DBConnection.GetDBConnection();

            // command text
            var cmdText = @"sp_retrieve_resources";

            //command
            var cmd = new SqlCommand(cmdText, conn);
            cmd.CommandType = CommandType.StoredProcedure;



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
                        var resource = new Resource()
                        {
                            ResourceID = reader.GetString(0),
                            Active = reader.GetBoolean(1)
                        };
                        resources.Add(resource);
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

            return resources;
        }

        public static Resource RetreiveResourceByID(string id)
        {
            Resource resource = null;

            var conn = DBConnection.GetDBConnection();

            var cmdText = @"sp_retrieve_resource_by_id";
            var cmd = new SqlCommand(cmdText, conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@ResourceID", id);

            try
            {
                conn.Open();
                var reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    reader.Read();

                    resource = new Resource()
                    {
                        ResourceID = reader.GetString(0),
                        Active = reader.GetBoolean(1)
                    };

                   
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

            return resource;
        }

        public static int UpdateResource(Resource oldResource, Resource newResource)
        {
            int result = 0;

            var conn = DBConnection.GetDBConnection();
            var cmdText = @"sp_update_resource";

            var cmd = new SqlCommand(cmdText, conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@OldResourceID", oldResource.ResourceID);
            cmd.Parameters.AddWithValue("@NewResourceID", newResource.ResourceID);

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


        public static int UpdateResourceActive(string resourceId, bool active)
        {
            int result = 0;

            var conn = DBConnection.GetDBConnection();
            var cmdText = @"sp_update_resource_active";

            var cmd = new SqlCommand(cmdText, conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@ResourceID", resourceId);
            cmd.Parameters.AddWithValue("@Active", active);

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

        public static int AddResource(Resource resource)
        {
            int result = 0;

            var conn = DBConnection.GetDBConnection();
            var cmdText = @"sp_add_resource";

            var cmd = new SqlCommand(cmdText, conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@ResourceID", resource.ResourceID);

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
