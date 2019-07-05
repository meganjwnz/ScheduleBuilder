using ScheduleBuilder.Model;
using System.Collections.Generic;
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

        /// <summary>
        /// Retrieves all the positions
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Get only the positions assigned to a specific person
        /// </summary>
        /// <param name="personID">The id of the person</param>
        /// <returns>A list of positions</returns>
        public List<Position> GetPersonPositions(int personID)
        {
            SqlConnection connection = ScheduleBuilder_DB_Connection.GetConnection();
            List<Position> personPepositionList = new List<Position>();

            string selectStatement = "SELECT p.id, p.position_title, p.isActive, p.position_description " +
                "FROM position p " +
                "JOIN assignedPosition AS ap ON p.id = ap.positionId " +
                "WHERE ap.personId = @personID";

            using (connection)
            {
                connection.Open();

                SqlCommand selectCommand = new SqlCommand(selectStatement, connection);
                selectCommand.Parameters.AddWithValue("@personID", personID);
                SqlDataReader reader = selectCommand.ExecuteReader();
                
                while (reader.Read())
                {
                    Position position = new Position();
                    position.positionID = int.Parse(reader["id"].ToString());
                    position.positionTitle = reader["position_title"].ToString();
                    position.isActive = (bool)reader["isActive"];
                    position.positionDescription = reader["position_description"].ToString();
                    personPepositionList.Add(position);
                }

            }
            return personPepositionList;
        }


        /// <summary>
        /// Adds a new position
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public bool AddPosition(Position position)
        {
            int positionResult = 0;

            string insertStatement =
                "INSERT INTO position([position_title],[isActive], [position_description]) " +
                "VALUES(@position_title, @isActive, @position_description)";

            using (SqlConnection connection = ScheduleBuilder_DB_Connection.GetConnection())
            {
                connection.Open();
                SqlTransaction transaction = connection.BeginTransaction();
                try
                {
                    using (SqlCommand insertCommand = new SqlCommand(insertStatement, connection))
                    {
                        insertCommand.Transaction = transaction;
                        insertCommand.Parameters.AddWithValue("@position_title", position.positionTitle);
                        insertCommand.Parameters.AddWithValue("@isActive", position.isActive);
                        insertCommand.Parameters.AddWithValue("@position_description", position.positionDescription);
                        positionResult = insertCommand.ExecuteNonQuery();
                    }
                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                }
            }
            return (positionResult >= 1 ? true : false);
        }

        /// <summary>
        /// Adds a position to a person
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public bool AddPositionToPerson(int person, int position)
        {
            int positionResult = 0;

            string insertStatement =
                "INSERT INTO assignedPosition([personId],[positionId]) " +
                "VALUES(@personId, @positionId)";

            using (SqlConnection connection = ScheduleBuilder_DB_Connection.GetConnection())
            {
                connection.Open();
                SqlTransaction transaction = connection.BeginTransaction();
                try
                {
                    using (SqlCommand insertCommand = new SqlCommand(insertStatement, connection))
                    {
                        insertCommand.Transaction = transaction;
                        insertCommand.Parameters.AddWithValue("@personId", person);
                        insertCommand.Parameters.AddWithValue("@positionId", position);
                        positionResult = insertCommand.ExecuteNonQuery();
                    }
                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                }
            }
            return (positionResult >= 1 ? true : false);
        }

        /// <summary>
        /// Updates a selected position 
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public bool UpdatePosition(Position position)
        {
            string updateStatement =
                "UPDATE position " +
                "SET [position_title] = @position_title, " +
                "[isActive] = @isActive, " +
                "[position_description] = @position_description " +
                "WHERE id = @id";

            int positionResult = 0;
            using (SqlConnection connection = ScheduleBuilder_DB_Connection.GetConnection())
            {
                connection.Open();
                SqlTransaction transaction = connection.BeginTransaction();
                try
                {
                    using (SqlCommand updateCommand = new SqlCommand(updateStatement, connection))
                    {
                        updateCommand.Transaction = transaction;
                        updateCommand.Parameters.AddWithValue("@id", position.positionID);
                        updateCommand.Parameters.AddWithValue("@position_title", position.positionTitle);
                        updateCommand.Parameters.AddWithValue("@isActive", position.isActive);
                        updateCommand.Parameters.AddWithValue("@position_description", position.positionDescription);

                        positionResult = updateCommand.ExecuteNonQuery();
                    }
                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                }
            }
            return (positionResult >= 1 ? true : false);
        }


        public List<Role> GetRoles()
        {
            List<Role> roles = new List<Role>();
            string selectStatement =
                "SELECT id, roleTitle, roleDescription " +
                "FROM role";
            using (SqlConnection connection = ScheduleBuilder_DB_Connection.GetConnection())
            {
                connection.Open();
                using (SqlCommand sqlCommand = new SqlCommand(selectStatement, connection))
                {
                    using (SqlDataReader reader = sqlCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Role role = new Role
                            {
                                Id = (int)reader["id"],
                                RoleTitle = (string)reader["roleTitle"],
                                RoleDescription = (string)reader["roleDescription"]
                            };
                            roles.Add(role);
                        }
                    }
                    return roles;
                }
            }
        }
    }
}
