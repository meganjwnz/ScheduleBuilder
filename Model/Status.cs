

namespace ScheduleBuilder.Model
{
    public class Status
    {
        public int Id { get; set; }
        public string StatusDescription { get; set; }
        public bool IsAbleToWork { get; set; }
        public string StatusTitle { get; set; }
    }
}