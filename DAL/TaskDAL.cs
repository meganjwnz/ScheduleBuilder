
using ScheduleBuilder.Model;
using System.Collections.Generic;
using System.Data.SqlClient;


namespace ScheduleBuilder.DAL
{
    public class TaskDAL
    {
        public List<Task> GetAllTasks()
        {
            SqlConnection connection = ScheduleBuilder_DB_Connection.GetConnection();
            List<Task> taskList = new List<Task>();

            string selectStatement = "SELECT t.id, t.task_title, t.isActive, t.task_description " +
                "FROM task t";

            using (connection)
            {
                connection.Open();

                using (SqlCommand selectCommand = new SqlCommand(selectStatement, connection))
                {
                    using (SqlDataReader reader = selectCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Task task = new Task();
                            task.TaskId = int.Parse(reader["id"].ToString());
                            task.Task_title = reader["task_title"].ToString();
                            task.IsActive = (bool)reader["isActive"];
                            task.Task_description = reader["task_description"].ToString();
                            taskList.Add(task);
                        }
                    }
                }
            }
            return taskList;
        }
    }
}