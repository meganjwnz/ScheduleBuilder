using System.ComponentModel.DataAnnotations;

namespace ScheduleBuilder.Model
{
    /// <summary>
    /// The Position Model Class used to represent a position at a place of employment
    /// </summary>
    public class Position
    {
        /// <summary>
        /// Id
        /// </summary>
        [Display(Name = "Position ID")]
        public int positionID { get; set; }

        //Title
        [Display(Name = "Position Title")]
        public string positionTitle { get; set; }

        //IsActive - 
        [Display(Name = "Active")]
        public bool isActive { get; set; }

        //Description
        [Display(Name = "Position Description")]
        public string positionDescription { get; set; }

    }
}