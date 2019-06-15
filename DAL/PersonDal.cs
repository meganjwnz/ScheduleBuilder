using ScheduleManager.Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace ScheduleManager.DAL
{
    /// <summary>
    /// This class provides access to the database 
    /// It is conserned with Person 
    /// </summary>
    public static class PersonDAL
    {
        //Sad about this
        //string selectedPersons = "Select id" +
        //        ", last_name" +
        //        ", first_name" +
        //        ", date_of_birth" +
        //        ", ssn" +
        //        ", gender" +
        //        ", street_address" +
        //        ", phone" +
        //        ", zipcode" +
        //        ", username" +
        //        ", password" +
        //        ", roleId" +
        //        ", statusId" +
        //        " From dbo.person ";
        ///// <summary>
        /// this method returns all employees
        /// </summary>
        /// <returns></returns>
        public static List<Person> GetDesiredPersons(string whereClause) {
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
                " From dbo.person " + whereClause;
                
            using (SqlConnection connection = ScheduleManager_DB_Connection.GetConnection())
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
                          //  person.Ssn = (char)reader["ssn"];
                            person.Gender = reader["gender"].ToString();
                            person.StreetAddress = reader["street_address"].ToString();
                            person.Phone = reader["phone"].ToString();
                            person.Zipcode = reader["zipcode"].ToString();
                            person.Username = reader["username"].ToString();
                            person.Password = (byte[])reader["password"];
                            person.RoleId = (int)reader["roleId"];
                            person.StatusId = (int)reader["statusId"];


                            persons.Add(person);
                        }

                    }
                    return persons;
                }
            }
        }
        /// <summary>
        /// This method adds an accepted person to the database
        /// 
        /// 
        /// Need to set in the View an appropiate auto generated-- password - roleId - StatusId -
        /// 
        /// </summary>
        /// <param name="addPerson"></param>
        public static void AddPerson(Person addPerson)
        {
            int addedPersonId = -1;

            try
            {
                using (SqlConnection connection = ScheduleManager_DB_Connection.GetConnection())
                {
                    connection.Open();
                    using (SqlTransaction transaction = connection.BeginTransaction())
                    {
                     string insertPerson = "INSERT person(" +
                            "[last_name]" +
                            " ,[first_name]" +
                            " ,[date_of_birth]" +
                            " ,[ssn]" +
                            " ,[gender]" +
                            " ,[street_address]" +
                            " ,[phone]" +
                            " ,[zipcode]" +
                            " ,[username]" +
                            " ,[password]" +
                            " ,[roleId]" +
                            " ,[statusId])" +
                            " VALUES(" +
                            " @last_name" +
                            ", @first_name" +
                            ", @date_of_birth" +
                            ", @ssn" +
                            ", @gender" +
                            ", @street_address" +
                            ", @phone" +
                            ", @zipcode" +
                            ", @username" +
                            ", HASHBYTES('SHA2_256', @password)" +
                            ", @roleId" +
                            ", @statusId)";


                        using (SqlCommand command = new SqlCommand(insertPerson, connection))
                        {
                            command.Parameters.Add(new SqlParameter("@last_name", addPerson.LastName));
                            command.Parameters.Add(new SqlParameter("@first_name", addPerson.FirstName));
                            command.Parameters.Add(new SqlParameter("@date_of_birth", addPerson.DateOfBirth));
                            command.Parameters.Add(new SqlParameter("@ssn", addPerson.Ssn));
                            command.Parameters.Add(new SqlParameter("@gender", addPerson.Gender));
                            command.Parameters.Add(new SqlParameter("@street_address", addPerson.StreetAddress));
                            command.Parameters.Add(new SqlParameter("@phone", addPerson.Phone));
                            command.Parameters.Add(new SqlParameter("@zipcode", addPerson.Zipcode));
                            command.Parameters.Add(new SqlParameter("@username", addPerson.Username));
                            command.Parameters.Add(new SqlParameter("@password", addPerson.Password));
                            command.Parameters.Add(new SqlParameter("@roleId", addPerson.Password));
                            command.Parameters.Add(new SqlParameter("@statusID", addPerson.Password));

                            string selectStatement = "SELECT IDENT_CURRENT('Person') FROM Person";

                            using (SqlCommand selectCommand = new SqlCommand(selectStatement, connection))
                            {
                                selectCommand.Transaction = transaction;
                                addedPersonId = Convert.ToInt32(selectCommand.ExecuteScalar());
                            }
                            transaction.Commit();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //Message Box won't work - thats wierd
               // MessageBox.Show(ex.Message, "Error");
            }
        }
    }
}
