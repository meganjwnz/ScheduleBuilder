namespace ScheduleBuilder.Model
{
    /// <summary>
    /// The Task Model Class represents an individual task associated with a position that can be assigned
    /// to a shift.
    /// </summary>
    public class Task
    {
        //id
        public int TaskId { get; set; }

        //Title
        public string Task_title { get; set; }

        //description
        public string Task_description { get; set; }

        //Is taskActive
        public bool IsActive { get; set; }

        //Positon task is attached too
        public int? PositionID { get; set; }

        //Position Name
        public string PositionName { get; set; }
    }
}