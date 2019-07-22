using ScheduleBuilder.Model;
using System.Collections.Generic;


namespace ScheduleBuilder.DAL
{
    public interface ITaskDAL
    {
        List<Task> GetAllTasks();
        List<Task> GetPositionTasks(int positionID);
        bool AddPositionTask(Task task);
        bool UpdatePositionTask(Task task);
    }
}
