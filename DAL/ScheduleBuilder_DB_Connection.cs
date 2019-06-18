
using System.Data.SqlClient;


namespace ScheduleBuilder.DAL
{
    public static class ScheduleBuilder_DB_Connection
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
