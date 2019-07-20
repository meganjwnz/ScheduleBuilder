

using ScheduleBuilder.Model;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace ScheduleBuilder.DAL
{
    public class PositionTaskDAL
    {
        /// <summary>
        /// Retrieves a list of all tasks that have a position assigned to it
        /// </summary>
        /// <returns></returns>
        public List<PositionTask> GetAllPositionTasks()
        {
            SqlConnection connection = ScheduleBuilder_DB_Connection.GetConnection();
            List<PositionTask> positionTaskList = new List<PositionTask>();

            string selectStatement = "SELECT pt.taskId, pt.roleId " +
                "FROM position_tasks pt";

            using (connection)
            {
                connection.Open();

                using (SqlCommand selectCommand = new SqlCommand(selectStatement, connection))
                {
                    using (SqlDataReader reader = selectCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            PositionTask positionTask = new PositionTask();
                            positionTask.TaskId = int.Parse(reader["taskId"].ToString());
                            positionTask.PositionId = int.Parse(reader["roleId"].ToString());
                            positionTaskList.Add(positionTask);
                        }
                    }
                }
            }
            return positionTaskList;
        }

        /// <summary>
        /// Creates a new connection between a position and a task
        /// </summary>
        /// <param name="task"></param>
        /// <returns></returns>
        public bool CreatePostionTasks(PositionTask positionTask)
        {
            int positionTaskResult = 0;

            string insertStatement =
                "INSERT INTO position_tasks([taskId],[roleId]) " +
                "VALUES(@taskId, @roleId)";

            using (SqlConnection connection = ScheduleBuilder_DB_Connection.GetConnection())
            {
                connection.Open();
                SqlTransaction transaction = connection.BeginTransaction();
                try
                {
                    using (SqlCommand insertCommand = new SqlCommand(insertStatement, connection))
                    {
                        insertCommand.Transaction = transaction;
                        insertCommand.Parameters.AddWithValue("@taskId", positionTask.TaskId);
                        insertCommand.Parameters.AddWithValue("@roleId", positionTask.PositionId);
                        positionTaskResult = insertCommand.ExecuteNonQuery();
                    }
                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                }
            }
            return (positionTaskResult >= 1 ? true : false);
        }


        /// <summary>
        /// Updates the relationship between a position and a task
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public bool UpdatePositionTask(PositionTask positionTask)
        {
            string updateStatement =
                "UPDATE position_tasks " +
                "SET [taskId] = @taskId, " +
                "[roleId] = @roleId " +
                "WHERE taskId = @taskId AND roleId = @roleId";

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
                        updateCommand.Parameters.AddWithValue("@taskId", positionTask.TaskId);
                        updateCommand.Parameters.AddWithValue("@roleId", positionTask.PositionId);

                        positionTaskResult = updateCommand.ExecuteNonQuery();
                    }
                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                }
            }
            return (positionTaskResult >= 1 ? true : false);
        }
    }
}