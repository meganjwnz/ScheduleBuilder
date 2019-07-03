using System.Data.SqlClient;


namespace ScheduleBuilder.DAL
{
    public static class ScheduleBuilder_DB_Connection
    {
        /// <summary>
        /// Gets the database connection
        /// </summary>
        /// <returns></returns>
        public static SqlConnection GetConnection()
        {
#if DEBUG
            string connectionString = "Data Source =localhost;Initial Catalog=ScheduleManager;" +
            "Integrated Security=True;";
#else
            string connectionString = "Server=tcp:scheduledbcs6920.database.windows.net,1433;Initial Catalog = ScheduleDB;" +
                " Persist Security Info = False; User ID = cs6920;Password = Yoder1245!@; MultipleActiveResultSets = False;" +
                " Encrypt = True; TrustServerCertificate = False;Connection Timeout = 30; ";
#endif


            SqlConnection connection = new SqlConnection(connectionString);
            return connection;
        }


    }
}
