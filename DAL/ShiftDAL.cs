using ScheduleManager.Model;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace ScheduleManager.DAL
{
    /// <summary>
    /// This class provides access to the database 
    /// It is conserned with Person 
    /// </summary>
    public class ShiftDAL
    {
        public List<Shift> GetShifts()
        {
            SqlConnection connection = ScheduleManager_DB_Connection.GetConnection();
            List<Shift> shiftList = new List<Shift>();

            string selectStatement = "SELECT s.id, s.scheduleShiftId, s.personId, s.positionId, sh.scheduledStartTime, sh.scheduledEndTime, " +
                "sh.scheduledLunchBreakStartTime, sh.scheduledLunchBreakEndTime, sh.actualStartTime, sh.actualEndTime, sh.actualLunchBreakStart, " +
                "sh.acutalLunchBreakEnd, p.first_name, p.last_name, ps.position_title " +
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
                            shift.personLastName = reader["lastName"].ToString();
                            shift.personFirstName = reader["firstName"].ToString();


                            shiftList.Add(shift);
                        }
                    }
                }
            }
            return shiftList;
            
        }
    }
}
