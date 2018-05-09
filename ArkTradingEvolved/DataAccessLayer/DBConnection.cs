using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class DBConnection
    {

        /// <summary>
        /// Where all the data access methods get their connection objects from
        /// </summary>
        /// <returns></returns>
        public static SqlConnection GetDBConnection()
        {
            var connString = @"Data Source=localhost;Initial Catalog=tradingevolved;Integrated Security=True";
            var conn = new SqlConnection(connString);
            return conn;
        }
       
    }
}
