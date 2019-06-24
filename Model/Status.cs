

namespace ScheduleBuilder.Model
{
    /// <summary>
    /// Values of the Status class
    /// </summary>
    public class Status
    {
        //Status ID
        public int Id { get; set; }
        //Status StatusDescription
        public string StatusDescription { get; set; }
        //Status If employee is able to work
        public bool IsAbleToWork { get; set; }
        //Status Title
        public string StatusTitle { get; set; }
    }
}