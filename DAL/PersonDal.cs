using ScheduleBuilder.Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Dapper;
using System.Data;

namespace ScheduleBuilder.DAL
{
    /// <summary>
    /// This class provides access to the database 
    /// It is conserned with Person 
    /// </summary>
    public class PersonDAL
    {
        string selectedPersons = "Select id" +
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
                ", email"      +    
                " From dbo.person ";



        public List<model> LoadData<model>(string sql)
        {
            using (IDbConnection cnn = ScheduleBuilder_DB_Connection.GetConnection())
            {
                return cnn.Query<model>(sql).AsList();
            }
        }

        public int SaveData<model>(string sql, model data)
        {
            using (IDbConnection cnn = ScheduleBuilder_DB_Connection.GetConnection())
            {
                return cnn.Execute(sql, data);
            }
        }



        /// <summary>
        /// this method returns all employees
        /// </summary>
        /// <returns></returns>
        public List<Person> GetDesiredPersons(string whereClause) {
            List<Person> persons = new List<Person>();
            string desiredEmployees = this.selectedPersons + whereClause; 
                
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
            HashingService hash = new HashingService();
            int addedPersonId = -1;

            try
            {
                using (SqlConnection connection = ScheduleBuilder_DB_Connection.GetConnection())
                {
                    connection.Open();
                    using (SqlTransaction transaction = connection.BeginTransaction())
                    {
                     string insertPerson = "INSERT person(" +
                            "[last_name]" +
                            ", [first_name]" +
                            ", [date_of_birth]" +
                            ", [ssn]" +
                            ", [gender]" +
                            ", [street_address]" +
                            ", [phone]" +
                            ", [zipcode]" +
                            ", [username]" +
                            ", [password]" +
                            ", [roleId]" +
                            ", [statusId]" +
                            ", [email])" +

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
                            ", @statusId" +
                            ", @email)";

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
                            command.Parameters.Add(new SqlParameter("@password", hash.PasswordHashing(addPerson.Password)));
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
