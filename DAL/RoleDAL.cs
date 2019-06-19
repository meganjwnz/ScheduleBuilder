using ScheduleBuilder.Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;


namespace ScheduleBuilder.DAL
{
    public class RoleDAL
    {
        /// <summary>
        /// Retrieves all roles that a user can be
        /// </summary>
        /// <returns>List of roles</returns>
        public List<Role> GetRoles()
        {
            List<Role> roles = new List<Role>();
            string selectStatement =
                "SELECT id, roleTile, roleDescription " +
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