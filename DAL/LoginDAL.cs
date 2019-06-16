using ScheduleManager.DAL;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace ScheduleBuilder.DAL
{
    public class LoginDAL
    {
        /// <summary>
        /// Verifies a person's login information and retrieves full name and role
        /// </summary>
        /// <param name="username">As a string</param>
        /// <param name="password">As a string</param>
        /// <returns></returns>
        public static DataTable GetLogin(string username, string password)
        {
            DataTable dt = new DataTable();
            string selectStatement =
                         "SELECT p.id, p.username, p.password, (p.first_name + ' ' + p.last_name) AS 'name', r.roleTitle " +
                         "FROM person p " +
                         "JOIN role r ON p.roleID = r.id " +
                         "WHERE username = @username AND password = @password";

            using (SqlConnection connection = ScheduleManager_DB_Connection.GetConnection())
            {
                connection.Open();
                SqlCommand sqlCommand = new SqlCommand(selectStatement, connection);
                sqlCommand.Parameters.AddWithValue("@username", username);
                sqlCommand.Parameters.AddWithValue("@password", password);
                SqlDataReader reader = sqlCommand.ExecuteReader();

                dt.Columns.Add("id", typeof(int));
                dt.Columns.Add("username", typeof(string));
                dt.Columns.Add("password", typeof(string));
                dt.Columns.Add("name", typeof(string));
                dt.Columns.Add("roleTitle", typeof(string));

                dt.Load(reader);  
            }
            return dt;
        }   
    }
}
