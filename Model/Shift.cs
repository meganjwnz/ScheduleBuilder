using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScheduleBuilder.Model
{
    /// <summary>
    /// Values of shift class
    /// </summary>
    public class Shift
    {
        //Shift Id
        public int shiftID { get; set; }
        //Scheduled shift id
        public int scheduleShiftID { get; set; }
        //Scheduled person Id
        public int personID { get; set;}
        //Scheduled persons first name
        public string personFirstName { get; set; }
        //Scheduled persons last name
        public string personLastName { get; set; }
        //Shift postion ID
        public int positionID { get; set; }
        //Shift postition name
        public string positionName { get; set; }
        //Shift start time
        public DateTime scheduledStartTime { get; set; }
        //shift end time
        public DateTime scheduledEndTime { get; set; }
        //shift lunch start time
        public DateTime? scheduledLunchBreakStart { get; set; }
        //shift lunch end time
        public DateTime? scheduledLunchBreakEnd { get; set; }
        //shift actual start time
        public DateTime? actualStartTime { get; set; }
        //shift actual end time
        public DateTime? actualEndTime { get; set; }
        //shift actual lunch start time
        public DateTime? actualLunchBreakStart { get; set; }
        //shift actual lunch end time
        public DateTime? actualLunchBreakEnd { get; set; }

    }
}
