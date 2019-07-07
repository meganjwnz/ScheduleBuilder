using ScheduleBuilder.Model;
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
        bool UpdateShift(Shift shift);

        /// <summary>
        /// Delete a shift from the db
        /// </summary>
        /// <param name="shift">The shift to be deleted</param>
        /// <returns>true if success, false if failure</returns>
        bool DeleteShift(Shift shift);
    }
}
