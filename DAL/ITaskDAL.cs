using ScheduleBuilder.Model;
using System;
using System.Collections.Generic;


namespace ScheduleBuilder.DAL
{
    public interface ITaskDAL
    {
        List<Task> GetAllTasks();
        List<Task> GetPositionTasks(int positionID);
        bool AddShiftTask(Task task);
        bool AddPositionTask(Task task);
        bool UpdateShiftTask(Task task);
        bool UpdatePositionTask(Task task);
    }
}
