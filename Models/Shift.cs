using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScheduleManager.Model
{
    public class Shift
    {
        public int shiftID { get; set; }
        public int scheduledShiftID { get; set; }
        public int personID { get; set;}
        public string personFirstName { get; set; }
        public string personLastName { get; set; }
        public int positionID { get; set; }
        public string positionName { get; set; }
        public DateTime scheduledStartTime { get; set; }
        public DateTime scheduledEndTime { get; set; }

    }
}
