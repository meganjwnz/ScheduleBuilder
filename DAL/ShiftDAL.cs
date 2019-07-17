using ScheduleBuilder.Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace ScheduleBuilder.DAL
{
    /// <summary>
    /// This class provides access to the database 
    /// It is conserned with Person 
    /// </summary>
    public class ShiftDAL : IShiftDAL
    {
        PersonDAL personDAL = new PersonDAL();
        /// <summary>
        /// Get all shifts in the database
        /// </summary>
        /// <returns>A list of shift objects</returns>
        public List<Shift> GetAllShifts()
        {
            SqlConnection connection = ScheduleBuilder_DB_Connection.GetConnection();
            List<Shift> shiftList = new List<Shift>();

            string selectStatement = "SELECT s.id, s.scheduleShiftId, s.personId, s.positionId, s.notes, sh.scheduledStartTime, sh.scheduledEndTime, " +
                "sh.scheduledLunchBreakStartTime, sh.scheduledLunchBreakEndTime, sh.actualStartTime, sh.actualEndTime, sh.actualLunchBreakStart, " +
                "sh.acutalLunchBreakEnd, p.first_name, p.last_name, ps.position_title, STUFF((SELECT '; ' + CONVERT(varchar, a.taskId) " +
                "FROM assignedTask as a " +
                "WHERE a.shiftId = s.id " +
                "FOR XML PATH('')), 1, 1, '') [taskList] " +
                "FROM shift AS s " +
                "JOIN shiftHours AS sh ON s.scheduleShiftId = sh.id " +
                "JOIN person AS p ON s.personId = p.id " +
                "JOIN position AS ps ON s.positionId = ps.id";

            using (connection)
            {
                connection.Open();

                using (SqlCommand selectCommand = new SqlCommand(selectStatement, connection))
                {
                    using (SqlDataReader reader = selectCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Shift shift = new Shift();
                            shift.shiftID = int.Parse(reader["id"].ToString());
                            shift.scheduleShiftID = int.Parse(reader["scheduleShiftId"].ToString());
                            shift.personID = int.Parse(reader["personId"].ToString());
                            shift.positionID = int.Parse(reader["positionId"].ToString());
                            shift.Notes = reader["notes"].ToString();
                            shift.scheduledStartTime = (DateTime)reader["scheduledStartTime"];
                            shift.scheduledEndTime = (DateTime)reader["scheduledEndTime"];
                            shift.scheduledLunchBreakStart = reader["scheduledLunchBreakStartTime"] as DateTime?;
                            shift.scheduledLunchBreakEnd = reader["scheduledLunchBreakEndTime"] as DateTime?;
                            shift.actualStartTime = reader["actualStartTime"] as DateTime?;
                            shift.actualEndTime = reader["actualEndTime"] as DateTime?;
                            shift.actualLunchBreakStart = reader["actualLunchBreakStart"] as DateTime?;
                            shift.actualLunchBreakEnd = reader["acutalLunchBreakEnd"] as DateTime?;
                            shift.positionName = reader["position_title"].ToString();
                            shift.personLastName = reader["last_name"].ToString();
                            shift.personFirstName = reader["first_name"].ToString();
                            string taskIdList = reader["taskList"].ToString();
                            shift.TaskIdList = taskIdList == string.Empty ? new List<int>() : taskIdList.Split(';').Select(int.Parse).ToList();
                            shiftList.Add(shift);
                        }
                    }
                }
            }
            return shiftList;

        }

        /// <summary>
        /// Returns list equal to the accepted where statement
        /// </summary>
        /// <returns></returns>
        public List<Shift> GetAllShifts(string whereClause)
        {
            SqlConnection connection = ScheduleBuilder_DB_Connection.GetConnection();
            List<Shift> shiftList = new List<Shift>();

            string selectStatement = "SELECT s.id, s.scheduleShiftId, s.personId, s.positionId, s.notes, sh.scheduledStartTime, sh.scheduledEndTime, " +
                "sh.scheduledLunchBreakStartTime, sh.scheduledLunchBreakEndTime, sh.actualStartTime, sh.actualEndTime, sh.actualLunchBreakStart, " +
                "sh.acutalLunchBreakEnd, p.first_name, p.last_name, ps.position_title " +
                "FROM shift AS s " +
                "JOIN shiftHours AS sh ON s.scheduleShiftId = sh.id " +
                "JOIN person AS p ON s.personId = p.id " +
                "JOIN position AS ps ON s.positionId = ps.id " + whereClause;

            using (connection)
            {
                connection.Open();

                using (SqlCommand selectCommand = new SqlCommand(selectStatement, connection))
                {
                    using (SqlDataReader reader = selectCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Shift shift = new Shift();
                            shift.shiftID = int.Parse(reader["id"].ToString());
                            shift.scheduleShiftID = int.Parse(reader["scheduleShiftId"].ToString());
                            shift.personID = int.Parse(reader["personId"].ToString());
                            shift.positionID = int.Parse(reader["positionId"].ToString());
                            shift.Notes = reader["notes"].ToString();
                            shift.scheduledStartTime = (DateTime)reader["scheduledStartTime"];
                            shift.scheduledEndTime = (DateTime)reader["scheduledEndTime"];
                            shift.scheduledLunchBreakStart = reader["scheduledLunchBreakStartTime"] as DateTime?;
                            shift.scheduledLunchBreakEnd = reader["scheduledLunchBreakEndTime"] as DateTime?;
                            shift.actualStartTime = reader["actualStartTime"] as DateTime?;
                            shift.actualEndTime = reader["actualEndTime"] as DateTime?;
                            shift.actualLunchBreakStart = reader["actualLunchBreakStart"] as DateTime?;
                            shift.actualLunchBreakEnd = reader["acutalLunchBreakEnd"] as DateTime?;
                            shift.positionName = reader["position_title"].ToString();
                            shift.personLastName = reader["last_name"].ToString();
                            shift.personFirstName = reader["first_name"].ToString();
                            shiftList.Add(shift);
                        }
                    }
                }
            }
            return shiftList;

        }

        public Shift GetNearestShift(string whereClause)
        {
            SqlConnection connection = ScheduleBuilder_DB_Connection.GetConnection();
            Shift shift = new Shift();
            string query = $"SELECT TOP 1 s.id" +
                $", s.scheduleShiftId" +
                $", s.personId" +
                $", s.positionId" +
                $", sh.scheduledStartTime" +
                $", sh.scheduledEndTime" +
                $", sh.scheduledLunchBreakStartTime" +
                $", sh.scheduledLunchBreakEndTime" +
                $", sh.actualStartTime" +
                $", sh.actualEndTime" +
                $", sh.actualLunchBreakStart" +
                $", sh.acutalLunchBreakEnd" +
                $", p.first_name" +
                $", p.last_name" +
                $", ps.position_title " +
                $" FROM shift AS s " +
                $" JOIN shiftHours AS sh ON s.scheduleShiftId = sh.id " +
                $" JOIN person AS p ON s.personId = p.id " +
                $" JOIN position AS ps ON s.positionId = ps.id "
                +  whereClause +
                $"  ORDER BY ABS(DATEDIFF(MINUTE, scheduledStartTime, GETDATE()))";

            using (connection)
            {
                connection.Open();

                using (SqlCommand selectCommand = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = selectCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            shift.shiftID = int.Parse(reader["id"].ToString());
                            shift.scheduleShiftID = int.Parse(reader["scheduleShiftId"].ToString());
                        
                            shift.positionID = int.Parse(reader["positionId"].ToString());
                            shift.scheduledStartTime = (DateTime)reader["scheduledStartTime"];
                            shift.scheduledEndTime = (DateTime)reader["scheduledEndTime"];
                            shift.scheduledLunchBreakStart = reader["scheduledLunchBreakStartTime"] as DateTime?;
                            shift.scheduledLunchBreakEnd = reader["scheduledLunchBreakEndTime"] as DateTime?;
                            shift.actualStartTime = reader["actualStartTime"] as DateTime?;
                            shift.actualEndTime = reader["actualEndTime"] as DateTime?;
                            shift.actualLunchBreakStart = reader["actualLunchBreakStart"] as DateTime?;
                            shift.actualLunchBreakEnd = reader["acutalLunchBreakEnd"] as DateTime?;
                            shift.positionName = reader["position_title"].ToString();
                            shift.personLastName = reader["last_name"].ToString();
                            shift.personFirstName = reader["first_name"].ToString();
                            shift.personID = int.Parse(reader["personId"].ToString());
                        }
                    }
                }
            }
            return shift;

        }

        /// <summary>
        /// Add a shift
        /// </summary>
        /// <param name="shift">A shift object to be sent to db</param>
        /// <returns>A boolean value of success or not</returns>
        public bool AddShift(Shift shift, Dictionary<int, bool> taskList)
        {
            int shiftHoursResult = 0;
            int shiftResult = 0;

            string insertShiftHoursStatement =
            "INSERT INTO shiftHours([scheduledStartTime], [scheduledEndTime], [scheduledLunchBreakStartTime], [ScheduledLunchBreakEndTime])" +
            "VALUES(@scheduledStartTime,@scheduledEndTime,@scheduledLunchBreakStartTime,@scheduledLunchBreakEndTime); SELECT SCOPE_IDENTITY()";

            string insertShiftStatement =
            "INSERT INTO shift([scheduleShiftID],[personId],[positionId],[notes])" +
            "VALUES(@scheduleShiftID,@personId,@positionId, @notes); SELECT SCOPE_IDENTITY()";

            string insertShiftTaskStatement =
                "INSERT INTO assignedTask([shiftId],[taskId]) " +
                "VALUES(@shiftID, @taskID)";

            //create new shift and shift hours entries
            using (SqlConnection connection = ScheduleBuilder_DB_Connection.GetConnection())
            {
                int pk = -1;
                int shiftpk = -1;

                connection.Open();
                SqlTransaction transaction = connection.BeginTransaction();
                try
                {
                    using (SqlCommand insertCommand = new SqlCommand(insertShiftHoursStatement, connection))
                    {
                        insertCommand.Transaction = transaction;
                        insertCommand.Parameters.AddWithValue("@scheduledStartTime", shift.scheduledStartTime);
                        insertCommand.Parameters.AddWithValue("@scheduledEndTime", shift.scheduledEndTime);
                        insertCommand.Parameters.AddWithValue("@scheduledLunchBreakStartTime", ((object)shift.scheduledLunchBreakStart) ?? DBNull.Value);
                        insertCommand.Parameters.AddWithValue("@scheduledLunchBreakEndTime", ((object)shift.scheduledLunchBreakEnd) ?? DBNull.Value);
                        
                        pk = Convert.ToInt32(insertCommand.ExecuteScalar());
                        shiftHoursResult = 1;
                    }
                    using (SqlCommand insertShiftCommand = new SqlCommand(insertShiftStatement, connection))
                    {
                        insertShiftCommand.Transaction = transaction;
                        insertShiftCommand.Parameters.AddWithValue("@scheduleShiftId", pk);
                        insertShiftCommand.Parameters.AddWithValue("@personId", shift.personID);
                        insertShiftCommand.Parameters.AddWithValue("@positionId", shift.positionID);
                        insertShiftCommand.Parameters.AddWithValue("@notes", ((object)shift.Notes) ?? DBNull.Value);

                        shiftpk = Convert.ToInt32(insertShiftCommand.ExecuteScalar());
                        shiftResult = 1;
                    }
                    foreach(KeyValuePair<int,bool> taskActive in taskList)
                    {
                        if (taskActive.Value)
                        {
                            using (SqlCommand insertShiftTaskCommand = new SqlCommand(insertShiftTaskStatement, connection))
                            {
                                insertShiftTaskCommand.Transaction = transaction;
                                insertShiftTaskCommand.Parameters.AddWithValue("@shiftID", shiftpk);
                                insertShiftTaskCommand.Parameters.AddWithValue("@taskID", taskActive.Key);
                                insertShiftTaskCommand.ExecuteNonQuery();
                            }
                        }
                    }
                    
                    transaction.Commit();
                }
                catch
                {
                    
                    transaction.Rollback();
                }
            }
            bool successful = (shiftHoursResult == 1 && shiftResult >= 1 ? true : false);
            if (successful)
            {
                this.ContactPersonShift(shift);
            }
            return successful;
        }

        private void ContactPersonShift(Shift shift)
        {
            Person person = this.personDAL.GetDesiredPersons($"Where Id = {shift.personID}").FirstOrDefault();
            Email email = new Email(person);

            string subject = $"You have a new Shift ";

            string body = $"Hello { person.GetFullName()}, \n" +
                $"\nYou are recieving this email to let you know you have a shift on {shift.scheduledStartTime} \n" +
                $"\n Please log in to your account to see all schedule changes\n" +

                $"\n If this has been done in error please contact your Admin as soon as possible " +
                $"\n Hope you have a wonderful day";


            email.SendMessage(subject, body);

        }

        /// <summary>
        /// Update an existing shift/shift hours
        /// </summary>
        /// <param name="shift">The shift object to be updated</param>
        /// <returns>A boolean value based on the success or failure of the update</returns>
        public bool UpdateShift(Shift shift, Dictionary<int, bool> taskList)
        {
            int shiftHoursResult = 0;
            int shiftResult = 0;

            string updateShiftHoursStatement =
            "UPDATE shiftHours SET [scheduledStartTime] = @scheduledStartTime, [scheduledEndTime] = @scheduledEndTime, [scheduledLunchBreakStartTime] = @scheduledLunchBreakStartTime, " +
            "[scheduledLunchBreakEndTime] = @scheduledLunchBreakEndTime " +
            "WHERE id = @id";

            string updateShiftStatement =
            "UPDATE shift SET [personId] = @personId, [positionId] = @positionId, [notes] = @notes " +
            "WHERE id = @id";

            string deleteAssignTask =
            "DELETE FROM assignedTask " +
            "WHERE shiftId = @shiftId";

            string insertShiftTaskStatement =
                "INSERT INTO assignedTask([shiftId],[taskId]) " +
                "VALUES(@shiftID, @taskID)";

            using (SqlConnection connection = ScheduleBuilder_DB_Connection.GetConnection())
            {
                connection.Open();
                SqlTransaction transaction = connection.BeginTransaction();
                try
                {
                    using (SqlCommand updateShiftHoursCommand = new SqlCommand(updateShiftHoursStatement, connection))
                    {
                        updateShiftHoursCommand.Transaction = transaction;
                        updateShiftHoursCommand.Parameters.AddWithValue("@scheduledStartTime", shift.scheduledStartTime);
                        updateShiftHoursCommand.Parameters.AddWithValue("@scheduledEndTime", shift.scheduledEndTime);
                        updateShiftHoursCommand.Parameters.AddWithValue("@scheduledLunchBreakStartTime", ((object)shift.scheduledLunchBreakStart) ?? DBNull.Value);
                        updateShiftHoursCommand.Parameters.AddWithValue("@scheduledLunchBreakEndTime", ((object)shift.scheduledLunchBreakEnd) ?? DBNull.Value);
                        updateShiftHoursCommand.Parameters.AddWithValue("@id", shift.scheduleShiftID);

                        shiftHoursResult = updateShiftHoursCommand.ExecuteNonQuery();
                    }
                    using (SqlCommand updateShiftCommand = new SqlCommand(updateShiftStatement, connection))
                    {
                        updateShiftCommand.Transaction = transaction;
                        updateShiftCommand.Parameters.AddWithValue("@id", shift.shiftID);
                        updateShiftCommand.Parameters.AddWithValue("@personId", shift.personID);
                        updateShiftCommand.Parameters.AddWithValue("@positionId", shift.positionID);
                        updateShiftCommand.Parameters.AddWithValue("@notes", ((object)shift.Notes) ?? DBNull.Value);

                        shiftResult = updateShiftCommand.ExecuteNonQuery();
                    }

                    using (SqlCommand deleteAssignTaskCommand = new SqlCommand(deleteAssignTask, connection))
                    {
                        deleteAssignTaskCommand.Transaction = transaction;
                        deleteAssignTaskCommand.Parameters.AddWithValue("@shiftID", shift.shiftID);
                        deleteAssignTaskCommand.ExecuteNonQuery();
                    }

                    foreach (KeyValuePair<int, bool> taskActive in taskList)
                    {
                        if (taskActive.Value)
                        {
                            using (SqlCommand insertShiftTaskCommand = new SqlCommand(insertShiftTaskStatement, connection))
                            {
                                insertShiftTaskCommand.Transaction = transaction;
                                insertShiftTaskCommand.Parameters.AddWithValue("@shiftID", shift.shiftID);
                                insertShiftTaskCommand.Parameters.AddWithValue("@taskID", taskActive.Key);
                                insertShiftTaskCommand.ExecuteNonQuery();
                            }
                        }
                    }

                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                }
            }

            return (shiftHoursResult == 1 && shiftResult >= 1 ? true : false);
        }

        /// <summary>
        /// Delete a shift
        /// </summary>
        /// <param name="shift"></param>
        /// <returns></returns>
        public bool DeleteShift(Shift shift)
        {
            int shiftHoursResult = 0;
            int shiftResult = 0;
            int taskResult = 0;

            string deleteShiftHoursStatement =
            "DELETE FROM shiftHours " +
            "WHERE id = @id";

            string deleteShiftStatement =
            "DELETE FROM shift " +
            "WHERE id = @id";

            string deleteTaskStatement =
            "DELETE FROM assignedTask " +
            "WHERE shiftId = @shiftId";

            //delete shift and shift hour entries
            using (SqlConnection connection = ScheduleBuilder_DB_Connection.GetConnection())
            {
                connection.Open();
                SqlTransaction transaction = connection.BeginTransaction();
                try
                {
                    using (SqlCommand deleteTaskCommand = new SqlCommand(deleteTaskStatement, connection))
                    {
                        deleteTaskCommand.Transaction = transaction;
                        deleteTaskCommand.Parameters.AddWithValue("@shiftId", shift.shiftID);

                        taskResult = deleteTaskCommand.ExecuteNonQuery();
                    }
                    using (SqlCommand deleteShiftCommand = new SqlCommand(deleteShiftStatement, connection))
                    {
                        deleteShiftCommand.Transaction = transaction;
                        deleteShiftCommand.Parameters.AddWithValue("@id", shift.shiftID);

                        shiftResult = deleteShiftCommand.ExecuteNonQuery();
                    }
                    using (SqlCommand deleteShiftHoursCommand = new SqlCommand(deleteShiftHoursStatement, connection))
                    {
                        deleteShiftHoursCommand.Transaction = transaction;
                        deleteShiftHoursCommand.Parameters.AddWithValue("@id", shift.scheduleShiftID);

                        shiftHoursResult = deleteShiftHoursCommand.ExecuteNonQuery();
                    }
                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                }
            }

            return (shiftHoursResult == 1 && shiftResult >= 1 ? true : false);
        }

        #region TimeCard 
        public void ClockUserIn(int shiftHoursId, DateTime clockIn)
        {
            string insert = $" UPDATE ShiftHours" +
                            $" SET actualStartTime = @clockInTime" +
                            $" WHERE id = @shiftId";
            using (SqlConnection connection = ScheduleBuilder_DB_Connection.GetConnection())
            {
                connection.Open();
                try
                {
                    using (SqlCommand command = new SqlCommand(insert, connection))
                    {

                        command.Parameters.AddWithValue("@shiftId", shiftHoursId);
                        command.Parameters.AddWithValue("@clockInTime", clockIn);

                        command.ExecuteNonQuery();   
                    }
                    connection.Close();
                    Shift shift = this.GetAllShifts(" WHERE sh.id = " + shiftHoursId.ToString()).FirstOrDefault<Shift>();
                    if (IsEmployeeLate(shift))
                    {
                        this.AlertManagmentOfLateEmployee(shift);
                        this.AlertUserOfLateClockin(shift);
                    }
                }
                catch (SqlException ex)
                {
                    throw ex;
                }
            }
        }

        private bool IsEmployeeLate(Shift shift)
        {
            bool lateEmployee = false;
            DateTime clockIn = shift.actualStartTime ?? DateTime.MinValue;
            if (shift.scheduledStartTime.AddMinutes(5) < clockIn)
            {
                lateEmployee = true;
            }

            return lateEmployee;
        }

        private void AlertManagmentOfLateEmployee(Shift shift)
        {
            Person person = this.personDAL.GetDesiredPersons($"Where Id = {shift.personID}").FirstOrDefault();
            Email email = new Email("Admin", "schedulebuilder2019@gmail.com");

            string subject = $"Late Shift Alert";

            string body = $"\n" +
                $"\n Please note that { person.GetFullName()} was supposed to arrive at {shift.scheduledStartTime} \n" +
                $"\n But failed to arrive until {shift.actualStartTime}\n" +

                $"\n This is unacceptable behavoir \n" +
                $"Please take appropiate corrective action " +
                $"\n ";


            email.SendMessage(subject, body);
        }

        private void AlertUserOfLateClockin(Shift shift)
        {
            Person person = this.personDAL.GetDesiredPersons($"Where Id = {shift.personID}").FirstOrDefault();
            Email email = new Email(person);

            string subject = $"Late Shift Alert";

            string body = $"\n" +
                $"\n { person.GetFullName()} \n you where supposed to arrive at {shift.scheduledStartTime} \n" +
                $"\n But failed to arrive until {shift.actualStartTime}\n" +

                $"\n This is unacceptable behavoir \n" +
                $" Please insure that you arrive on time" +
                $"\n \n" +
                $"If you believe this is an error please contact Management";


            email.SendMessage(subject, body);
        }


        public void ClockUserOut(int scheduleShiftID, DateTime now)
        {
            string insert = $" UPDATE ShiftHours" +
                           $" SET actualEndTime = @clockOutTime" +
                           $" WHERE id = @shiftId";
            using (SqlConnection connection = ScheduleBuilder_DB_Connection.GetConnection())
            {
                connection.Open();
                try
                {
                    using (SqlCommand command = new SqlCommand(insert, connection))
                    {
                        command.Parameters.AddWithValue("@shiftId", scheduleShiftID);
                     
                        command.Parameters.AddWithValue("@clockOutTime", now);
                        
                        command.ExecuteNonQuery();
                    }
                    connection.Close();
                }
                catch (SqlException ex)
                {
                    throw ex;
                }
            }
        }
  
        public void ClockLunchStart(int scheduleShiftID, DateTime now)
        {
            string insert = $" UPDATE ShiftHours" +
                          $" SET actualLunchBreakStart = @lunchStartTime" +
                          $" WHERE id = @shiftId";
            using (SqlConnection connection = ScheduleBuilder_DB_Connection.GetConnection())
            {
                connection.Open();
                try
                {
                    using (SqlCommand command = new SqlCommand(insert, connection))
                    {
                        command.Parameters.AddWithValue("@shiftId", scheduleShiftID);

                        command.Parameters.AddWithValue("@lunchStartTime", now);

                        command.ExecuteNonQuery();
                    }
                    connection.Close();
                }
                catch (SqlException ex)
                {
                    throw ex;
                }
            }
        }

        public void ClockLunchEnd(int scheduleShiftID, DateTime now)
        {
            string insert = $" UPDATE ShiftHours" +
                          $" SET acutalLunchBreakEnd = @lunchEndTime" +
                          $" WHERE id = @shiftId";
            using (SqlConnection connection = ScheduleBuilder_DB_Connection.GetConnection())
            {
                connection.Open();
                try
                {
                    using (SqlCommand command = new SqlCommand(insert, connection))
                    {
                        command.Parameters.AddWithValue("@shiftId", scheduleShiftID);

                        command.Parameters.AddWithValue("@lunchEndTime", now);

                        command.ExecuteNonQuery();
                    }
                    connection.Close();
                }
                catch (SqlException ex)
                {
                    throw ex;
                }
            }
        }
        #endregion

        /// <summary>
        /// Checks to see if a person is already scheduled for a specific time
        /// </summary>
        /// <param name="personId"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        public bool CheckIfPersonIsScheduled(int personId, DateTime startTime, DateTime endTime)
        {
            Person person = new Person();
            Shift shift = new Shift();
            string selectStatement =
                "SELECT person.id, shiftHours.scheduledStartTime, shiftHours.scheduledEndTime " +
                "FROM person " +
                "INNER JOIN shift ON shift.personId = person.id " +
                "INNER JOIN shiftHours ON shiftHours.id = shift.scheduleShiftId " +
                "WHERE person.id = @personId AND shiftHours.scheduledStartTime = @startTime";
            using (SqlConnection connection = ScheduleBuilder_DB_Connection.GetConnection())
            {
                connection.Open();
                using (SqlCommand selectCommand = new SqlCommand(selectStatement, connection))
                {
                    selectCommand.Parameters.AddWithValue("@personId", personId);
                    selectCommand.Parameters.AddWithValue("@startTime", startTime);
                    selectCommand.Parameters.AddWithValue("@endTime", endTime);
                    using (SqlDataReader reader = selectCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            shift.personID = (int)reader["id"];
                            shift.scheduledStartTime = (DateTime)reader["scheduledStartTime"];
                            shift.scheduledEndTime = (DateTime)reader["scheduledEndTime"];
                            if (startTime >= shift.scheduledStartTime.AddHours(4) && startTime <= shift.scheduledEndTime.AddHours(4))
                            {
                                return true;
                            }
                        }
                    }
                }
                if (shift.personID == personId && shift.scheduledStartTime == startTime)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        
    }
}
