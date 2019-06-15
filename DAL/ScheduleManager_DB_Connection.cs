using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ScheduleManager.DAL
{
    public static class ScheduleManager_DB_Connection
    {
        public static SqlConnection GetConnection()
        {
            string connectionString = "Data Source =localhost;Initial Catalog=ScheduleManager;" +
            "Integrated Security=True;";

            SqlConnection connection = new SqlConnection(connectionString);
            return connection;
        }


    }
}
