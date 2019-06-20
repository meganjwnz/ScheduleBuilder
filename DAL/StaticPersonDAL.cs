using ScheduleBuilder.Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace ScheduleBuilder.DAL
{
    public static class StaticPersonDAL
    {
        public static List<Person> GetDesiredPersons()
        {
            List<Person> persons = new List<Person>();
            string desiredEmployees = "Select id" +
                ", last_name" +
                ", first_name" +
                ", date_of_birth" +
                ", ssn" +
                ", gender" +
                ", street_address" +
                ", phone" +
                ", zipcode" +
                ", username" +
                ", password" +
                ", roleId" +
                ", statusId" +
                ", email" +
                " From dbo.person ";

            using (SqlConnection connection = ScheduleBuilder_DB_Connection.GetConnection())
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(desiredEmployees, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Person person = new Person();
                            person.Id = (int)reader["id"];
                            person.LastName = reader["last_name"].ToString();
                            person.FirstName = reader["first_name"].ToString();
                            person.DateOfBirth = (DateTime)reader["date_of_birth"];
                            person.Ssn = reader["ssn"].ToString();
                            person.Gender = reader["gender"].ToString();
                            person.StreetAddress = reader["street_address"].ToString();
                            person.Phone = reader["phone"].ToString();
                            person.Zipcode = reader["zipcode"].ToString();
                            person.Username = reader["username"].ToString();
                            person.Password = reader["password"].ToString();
                            person.RoleId = (int)reader["roleId"];
                            person.StatusId = (int)reader["statusId"];
                            person.Email = reader["email"].ToString();

                            persons.Add(person);
                        }

                    }
                    return persons;
                }
            }

        }
    }
}