
using ScheduleBuilder.Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace ScheduleBuilder.DAL
{
    public class TaskDAL
    {
        public List<Task> GetAllTasks()
        {
            SqlConnection connection = ScheduleBuilder_DB_Connection.GetConnection();
            List<Task> taskList = new List<Task>();

            string selectStatement = "SELECT t.id, t.task_title, t.isActive, t.task_description, pt.roleId, p.position_title " +
                "FROM task t " +
                "RIGHT JOIN position_tasks AS pt ON t.id = pt.taskId " +
                "JOIN position AS p ON pt.roleId = p.id";

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
                            task.PositionID = int.Parse(reader["roleId"].ToString());
                            task.PositionName = reader["position_title"].ToString();
                            taskList.Add(task);
                        }
                    }
                }
            }
            return taskList;
        }

        public List<Task> GetPositionTasks(int positionID)
        {
            SqlConnection connection = ScheduleBuilder_DB_Connection.GetConnection();
            List<Task> taskList = new List<Task>();

            string selectStatement = "SELECT t.id, t.task_title, t.isActive, t.task_description, pt.roleId, p.position_title " +
                "FROM task t " +
                "RIGHT JOIN position_tasks AS pt ON t.id = pt.taskId " +
                "JOIN position AS p ON pt.roleId = p.id " +
                "WHERE pt.roleId = @positionID AND t.isActive = 1";

            using (connection)
            {
                connection.Open();

                using (SqlCommand selectCommand = new SqlCommand(selectStatement, connection))
                {
                    selectCommand.Parameters.AddWithValue("@positionID", positionID);
                    using (SqlDataReader reader = selectCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Task task = new Task();
                            task.TaskId = int.Parse(reader["id"].ToString());
                            task.Task_title = reader["task_title"].ToString();
                            task.IsActive = (bool)reader["isActive"];
                            task.Task_description = reader["task_description"].ToString();
                            task.PositionID = int.Parse(reader["roleId"].ToString());
                            task.PositionName = reader["position_title"].ToString();
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
        /// <param name="task">A task object</param>
        /// <returns>True if successful, false otherwise</returns>
        public bool AddShiftTask(Task task)
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
                        insertCommand.Parameters.AddWithValue("@task_description", task.Task_description);
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
        /// Adds a new task with a position
        /// </summary>
        /// <param name="task">A task object</param>
        /// <returns>true if successful, false if failure</returns>
        public bool AddPositionTask(Task task)
        {
            int taskresult = 0;
            int positiontaskresult = 0;

            string insertStatement =
                "INSERT INTO task([task_title],[isActive], [task_description]) " +
                "VALUES(@task_title, @isActive, @task_description);SELECT SCOPE_IDENTITY();";

            string insertptStatement =
                "INSERT INTO position_tasks([taskId],[roleId]) " +
                "VALUES(@taskId, @roleId)";

            using (SqlConnection connection = ScheduleBuilder_DB_Connection.GetConnection())
            {
                int pk = -1;
                connection.Open();
                SqlTransaction transaction = connection.BeginTransaction();
                try
                {
                    using (SqlCommand insertCommand = new SqlCommand(insertStatement, connection))
                    {
                        insertCommand.Transaction = transaction;
                        insertCommand.Parameters.AddWithValue("@task_title", task.Task_title);
                        insertCommand.Parameters.AddWithValue("@isActive", task.IsActive);
                        insertCommand.Parameters.AddWithValue("@task_description", task.Task_description);
                        pk = Convert.ToInt32(insertCommand.ExecuteScalar());
                        taskresult = 1;
                    }
                    using (SqlCommand insertCommand = new SqlCommand(insertptStatement, connection))
                    {
                        insertCommand.Transaction = transaction;
                        insertCommand.Parameters.AddWithValue("@taskId", pk);
                        insertCommand.Parameters.AddWithValue("@roleId", task.PositionID);
                        positiontaskresult = insertCommand.ExecuteNonQuery();
                    }
                    transaction.Commit();
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.ToString());
                    transaction.Rollback();
                }
            }
            return (taskresult == 1 && positiontaskresult >=1 ? true : false);
        }

        /// <summary>
        /// Updates a shift task 
        /// </summary>
        /// <param name="task">A task object</param>
        /// <returns>True if successful, false otherwise</returns>
        public bool UpdateShiftTask(Task task)
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
        /// Update a position task
        /// </summary>
        /// <param name="task">A task object</param>
        /// <returns>True if successful, false otherwise</returns>
        public bool UpdatePositionTask(Task task)
        {
            string updateStatement =
                "UPDATE task " +
                "SET [task_title] = @task_title, " +
                "[isActive] = @isActive, " +
                "[task_description] = @task_description " +
                "WHERE id = @id";

            string updateptStatement =
                "UPDATE position_tasks " +
                "SET [roleId] = @roleId " +
                "WHERE taskId = @taskId ";

            int taskResult = 0;
            int positionTaskResult = 0;

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
                    using (SqlCommand updateCommand = new SqlCommand(updateptStatement, connection))
                    {
                        updateCommand.Transaction = transaction;
                        updateCommand.Parameters.AddWithValue("@taskId", task.TaskId);
                        updateCommand.Parameters.AddWithValue("@roleId", task.PositionID);

                        positionTaskResult = updateCommand.ExecuteNonQuery();
                    }
                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                }
            }
            return (taskResult >= 1 && positionTaskResult >= 1 ? true : false);
        }


    }
}