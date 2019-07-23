using ScheduleBuilder.Model;
using System;
using System.Collections.Generic;

namespace ScheduleBuilder.DAL
{
    /// <summary>
    /// This interface is for the shift DAL
    /// </summary>
    public interface IShiftDAL
    {
        /// <summary>
        /// Get all shifts in the system
        /// </summary>
        /// <returns>A list of shift objects</returns>
        List<Shift> GetAllShifts();

        Shift GetNearestShift(string whereClause);

        /// <summary>
        /// Add a shift to the shift and shift hours table
        /// </summary>
        /// <param name="shift">The shift to be added</param>
        /// <returns>true if success</returns>
        bool AddShift(Shift shift, Dictionary<int, bool> taskList);

        /// <summary>
        /// Update an existing shift via shift and shifthours table
        /// </summary>
        /// <param name="shift">The shift to be updated</param>
        /// <returns>true if success, false if failure</returns>
        bool UpdateShift(Shift shift, Dictionary<int, bool> taskList);

        /// <summary>
        /// Delete a shift from the db
        /// </summary>
        /// <param name="shift">The shift to be deleted</param>
        /// <returns>true if success, false if failure</returns>
        bool DeleteShift(Shift shift);

        /// <summary>
        /// Clocks user by the id IN by the clockin time
        /// </summary>
        /// <param name="shiftHoursId"></param>
        /// <param name="clockIn"></param>
        void ClockUserIn(int shiftHoursId, DateTime clockIn);

        /// <summary>
        /// Clocks user by Id OUT by the clockout time
        /// </summary>
        /// <param name="shiftHoursId"></param>
        /// <param name="clockOut"></param>
        void ClockUserOut(int shiftHoursId, DateTime clockOut);
    }
}
