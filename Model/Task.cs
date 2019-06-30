
namespace ScheduleBuilder.Model
{
    public class Task
    {
        public int TaskId { get; set; }

        public string Task_title { get; set; }

        public string Task_description { get; set; }

        public bool IsActive { get; set; }

        public int? PositionID { get; set; }
    }
}