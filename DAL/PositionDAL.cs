using ScheduleBuilder.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace ScheduleBuilder.DAL
{
    /// <summary>
    /// This class provides access to the database 
    /// It is conserned with Person 
    /// </summary>
    public class PositionDAL : IPositionDAL
    {
        /// <summary>
        /// Returns all ActivePositions
        /// </summary>
        /// <returns></returns>
        public List<Position> GetAllActivePositions()
        {
            SqlConnection connection = ScheduleBuilder_DB_Connection.GetConnection();
            List<Position> positionList = new List<Position>();

            //May need to add in the position tasks to this later
            string selectStatement = "SELECT p.id, p.position_title, p.isActive, p.position_description " +
                "FROM position as p " +
                "WHERE p.isActive = 1";
                
            using (connection)
            {
                connection.Open();

                using (SqlCommand selectCommand = new SqlCommand(selectStatement, connection))
                {
                    using (SqlDataReader reader = selectCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Position position = new Position();
                            position.positionID = int.Parse(reader["id"].ToString());
                            position.positionTitle = reader["position_title"].ToString();
                            position.positionDescription = reader["position_description"].ToString();
                            positionList.Add(position);
                        }
                    }
                }
            }
            return positionList;
        }

        public List<Position> GetAllPositions()
        {
            SqlConnection connection = ScheduleBuilder_DB_Connection.GetConnection();
            List<Position> positionList = new List<Position>();

            string selectStatement = "SELECT p.id, p.position_title, p.isActive, p.position_description " +
                "FROM position p";

            using (connection)
            {
                connection.Open();

                using (SqlCommand selectCommand = new SqlCommand(selectStatement, connection))
                {
                    using (SqlDataReader reader = selectCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Position position = new Position();
                            position.positionID = int.Parse(reader["id"].ToString());
                            position.positionTitle = reader["position_title"].ToString();
                            position.isActive = (bool)reader["isActive"];
                            position.positionDescription = reader["position_description"].ToString();
                            positionList.Add(position);
                        }
                    }
                }
            }
            return positionList;
        }
    }
}
