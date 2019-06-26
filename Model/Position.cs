using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ScheduleBuilder.Model
{
    public class Position
    {
        [Display(Name = "Position ID")]
        public int positionID { get; set; }
        [Display(Name = "Position Title")]
        public string positionTitle { get; set; }
        [Display(Name = "Position Status")]
        public int? isActive { get; set; }
        [Display(Name = "Position Description")]
        public string positionDescription { get; set; }
    }
}