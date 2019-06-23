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

        /// <summary>
        /// Gets a role by its id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string GetRoleByID(int id)
        {
            Role role = new Role();
            string selectStatement =
                         "SELECT id, roleTitle " +
                         "FROM role " +
                         "WHERE id = @id";

            using (SqlConnection connection = ScheduleBuilder_DB_Connection.GetConnection())
            {
                connection.Open();
                using (SqlCommand sqlCommand = new SqlCommand(selectStatement, connection))
                {
                    sqlCommand.Parameters.AddWithValue("@id", id);
                    using (SqlDataReader reader = sqlCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            role.Id = (int)reader["id"];
                            role.RoleTitle = reader["roleTitle"].ToString();
                        }
                    }
                }
                return role.RoleTitle;
            }
        }

        /// <summary>
        /// Gets a role's id by its title
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public int GetRoleIdByTitle(string roleTitle)
        {
            Role role = new Role();
            string selectStatement =
                         "SELECT id, roleTitle " +
                         "FROM role " +
                         "WHERE roleTitle = @roleTitle";

            using (SqlConnection connection = ScheduleBuilder_DB_Connection.GetConnection())
            {
                connection.Open();
                using (SqlCommand sqlCommand = new SqlCommand(selectStatement, connection))
                {
                    sqlCommand.Parameters.AddWithValue("@roleTitle", roleTitle);
                    using (SqlDataReader reader = sqlCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            role.Id = (int)reader["id"];
                            role.RoleTitle = reader["roleTitle"].ToString();
                        }
                    }
                }
                return role.Id;
            }
        }
    }
}