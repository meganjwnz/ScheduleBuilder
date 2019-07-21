
namespace ScheduleBuilder.Model
{
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