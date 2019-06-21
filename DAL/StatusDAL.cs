using ScheduleBuilder.Model;
using System.Data.SqlClient;

namespace ScheduleBuilder.DAL
{
    public class StatusDAL
    {
        /// <summary>
        /// Gets a status by its id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Status GetStatusByID(int id)
        {
            Status status = new Status();
            string selectStatement =
                         "SELECT id, status_description, isAbleToWork, status_title " +
                         "FROM status " +
                         "WHERE id = @id";

            using (SqlConnection connection = ScheduleBuilder_DB_Connection.GetConnection())
            {
                connection.Open();
                using (SqlCommand sqlCommand = new SqlCommand(selectStatement, connection))
                {
                    sqlCommand.Parameters.AddWithValue("@id", id);
                    using (SqlDataReader reader = sqlCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            status.Id = (int)reader["id"];
                            status.StatusDescription = reader["status_description"].ToString();
                            status.IsAbleToWork = (bool)reader["isAbleToWork"];
                            status.StatusTitle = reader["status_title"].ToString();
                        }
                    }
                }
                return status;
            }
        }
    }
}