
using System.ComponentModel.DataAnnotations;

namespace ScheduleBuilder.Model
{
    public class Position
    {
        [Display(Name = "Position ID")]
        public int positionID { get; set; }
        [Display(Name = "Position Title")]
        public string positionTitle { get; set; }
        [Display(Name = "Active")]
        public bool isActive { get; set; }
        [Display(Name = "Position Description")]
        public string positionDescription { get; set; }
    }
}