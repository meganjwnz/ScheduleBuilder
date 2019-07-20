using System;
using System.ComponentModel.DataAnnotations;

namespace ScheduleBuilder.ModelViews
{
    public class TimeCardEditViewModel
    {
        //Scheduled persons first name
        [Display(Name = "First Name")]
        public string personFirstName { get; set; }

        //Scheduled persons last name
        [Display(Name = "Last Name")]
        public string personLastName { get; set; }

        //Shift start time
        [Display(Name = "Scheduled Start Time")]
        public DateTime scheduledStartTime { get; set; }

        //shift end time
        [Display(Name = "Scheduled End Time")]
        public DateTime scheduledEndTime { get; set; }

        //shift lunch start time
        [Display(Name = "Scheduled Break Start")]
        public DateTime? scheduledLunchBreakStart { get; set; }

        //shift lunch end time
        [Display(Name = "Scheduled Break End")]
        public DateTime? scheduledLunchBreakEnd { get; set; }

        //shift actual start time
        [Display(Name = "Actual Clock In")]
        public DateTime? actualStartTime { get; set; }

        //shift actual end time
        [Display(Name = "Actual Clock Out")]
        public DateTime? actualEndTime { get; set; }

        //shift actual lunch start time
        [Display(Name = "Actual Break Start")]
        public DateTime? actualLunchBreakStart { get; set; }

        //shift actual lunch end time
        [Display(Name = "Actual Break End")]
        public DateTime? actualLunchBreakEnd { get; set; }

    }
}