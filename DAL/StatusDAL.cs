using ScheduleBuilder.Model;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace ScheduleBuilder.DAL
{
    public class StatusDAL : IStatusDAL
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

        public List<Status> GetStatuses()
        {
            
            List<Status> statuses = new List<Status>();
            string selectStatement =
                               "SELECT id, status_description, isAbleToWork, status_title " +
                               "FROM status";
            using (SqlConnection connection = ScheduleBuilder_DB_Connection.GetConnection())
            {
                connection.Open();
                using (SqlCommand sqlCommand = new SqlCommand(selectStatement, connection))
                {
                    using (SqlDataReader reader = sqlCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Status status = new Status
                            {
                                Id = (int)reader["id"],
                                StatusDescription = reader["status_description"].ToString(),
                                IsAbleToWork = (bool)reader["isAbleToWork"],
                                StatusTitle = reader["status_title"].ToString()
                            };
                            statuses.Add(status);
                        }
                       
                    }
                }
                
            }
            return statuses;
        }

    }
}