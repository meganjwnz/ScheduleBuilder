using ScheduleBuilder.Model;
using System.Collections.Generic;

namespace ScheduleBuilder.DAL
{
    public interface IPositionDAL
    {
        List<Position> GetAllActivePositions();
    }
}
