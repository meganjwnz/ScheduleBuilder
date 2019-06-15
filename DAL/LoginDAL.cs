using System.Data;
using System.Data.SqlClient;


namespace ScheduleManager.DAL
{
    public class LoginDAL
    {
        /// <summary>
        /// Verifies a person's login information and retrieves full name and role
        /// </summary>
        /// <param name="username">As a string</param>
        /// <param name="password">As a string</param>
        /// <returns></returns>
        public DataTable GetLogin(string username, string password)
        {
            DataTable dt = new DataTable();
            string selectStatement =
                         "SELECT l.personID, l.userName, l.password, (p.first_name + ' ' + p.last_name) AS 'name', p.role" +
                         "FROM login l JOIN person p ON p.id = l.personID" +
                         "WHERE username = @username AND password = @password";

            using (SqlConnection connection = ScheduleManager_DB_Connection.GetConnection())
            {
                connection.Open();
                SqlCommand sqlCommand = new SqlCommand(selectStatement, connection);
                sqlCommand.Parameters.AddWithValue("@username", username);
                sqlCommand.Parameters.AddWithValue("@password", password);
                SqlDataReader reader = sqlCommand.ExecuteReader();

                dt.Columns.Add("personID", typeof(int));
                dt.Columns.Add("userName", typeof(string));
                dt.Columns.Add("password", typeof(string));
                dt.Columns.Add("name", typeof(string));
                dt.Columns.Add("role", typeof(string));

                dt.Load(reader);
            }
            return dt;
        }   
    }
}
