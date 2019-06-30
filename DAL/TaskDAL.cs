
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


        /// <summary>
        /// Adds a new task
        /// </summary>
        /// <param name="task"></param>
        /// <returns></returns>
        public bool AddTask(Task task)
        {
            int taskresult = 0;

            string insertStatement =
                "INSERT INTO task([task_title],[isActive], [task_description]) " +
                "VALUES(@task_title, @isActive, @task_description)";

            using (SqlConnection connection = ScheduleBuilder_DB_Connection.GetConnection())
            {
                connection.Open();
                SqlTransaction transaction = connection.BeginTransaction();
                try
                {
                    using (SqlCommand insertCommand = new SqlCommand(insertStatement, connection))
                    {
                        insertCommand.Transaction = transaction;
                        insertCommand.Parameters.AddWithValue("@task_title", task.Task_title);
                        insertCommand.Parameters.AddWithValue("@isActive", task.IsActive);
                        insertCommand.Parameters.AddWithValue("@position_description", task.Task_description);
                        taskresult = insertCommand.ExecuteNonQuery();
                    }
                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                }
            }
            return (taskresult >= 1 ? true : false);
        }

        /// <summary>
        /// Updates a selected task 
        /// </summary>
        /// <param name="task"></param>
        /// <returns></returns>
        public bool UpdateTask(Task task)
        {
            string updateStatement =
                "UPDATE task " +
                "SET [task_title] = @task_title, " +
                "[isActive] = @isActive, " +
                "[task_description] = @task_description " +
                "WHERE id = @id";

            int taskResult = 0;
            using (SqlConnection connection = ScheduleBuilder_DB_Connection.GetConnection())
            {
                connection.Open();
                SqlTransaction transaction = connection.BeginTransaction();
                try
                {
                    using (SqlCommand updateCommand = new SqlCommand(updateStatement, connection))
                    {
                        updateCommand.Transaction = transaction;
                        updateCommand.Parameters.AddWithValue("@id", task.TaskId);
                        updateCommand.Parameters.AddWithValue("@task_title", task.Task_title);
                        updateCommand.Parameters.AddWithValue("@isActive", task.IsActive);
                        updateCommand.Parameters.AddWithValue("@task_description", task.Task_description);

                        taskResult = updateCommand.ExecuteNonQuery();
                    }
                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                }
            }
            return (taskResult >= 1 ? true : false);
        }

        /// <summary>
        /// Deactivates a task
        /// </summary>
        /// <param name="task"></param>
        /// <returns></returns>
        public Task DeactivateTask(Task task)
        {
            string update =
                "UPDATE task " +
                "SET [isActive] = @isActive " +
                "WHERE id = @id";
            try
            {
                using (SqlConnection connection = ScheduleBuilder_DB_Connection.GetConnection())
                {
                    connection.Open();
                    using (SqlCommand updateCommand = new SqlCommand(update, connection))
                    {

                        updateCommand.Parameters.AddWithValue("@isActive", 0);
                        updateCommand.Parameters.AddWithValue("@id", task.TaskId);
                        updateCommand.ExecuteNonQuery();
                    }
                    connection.Close();
                }
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            return task;
        }
    }
}