using ScheduleBuilder.Model;
using System.Collections.Generic;

namespace ScheduleBuilder.DAL
{
    public interface IPositionDAL
    {
        List<Position> GetAllActivePositions();

        List<Position> GetAllPositions();

        List<Position> GetPersonPositions(int personId);

        bool AddPosition(Position position);

        bool UpdatePosition(Position position);

        bool AddPositionToPerson(int personID, int positionID);
    }
}
