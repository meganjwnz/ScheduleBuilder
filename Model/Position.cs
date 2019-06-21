using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ScheduleBuilder.Model
{
    public class Position
    {
        public int positionID { get; set; }
        public string positionTitle { get; set; }
        public int? isActive { get; set; }
        public string positionDescription { get; set; }
    }
}