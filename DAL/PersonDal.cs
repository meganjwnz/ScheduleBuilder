using Dapper;
using ScheduleBuilder.Model;
using ScheduleBuilder.ModelViews;
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
    public class PersonDAL : IPersonDAL
    {
        HashingService hashingService = new HashingService();
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
                ", email" +
                " From dbo.person ";

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="model"></typeparam>
        /// <param name="sql"></param>
        /// <returns></returns>
        public List<model> LoadData<model>(string sql)
        {
            using (IDbConnection cnn = ScheduleBuilder_DB_Connection.GetConnection())
            {
                return cnn.Query<model>(sql).AsList();
            }
        }

        /// <summary>
        /// returns true if zipcode exists
        /// </summary>
        /// <param name="zipcode"></param>
        /// <returns></returns>
        public bool ZipCodeExisits(string zipcode)
        {
            bool doesExist = false;
            int count = 0;
            string sqlStatement = $" SELECT *" +
                                    $" FROM zipcode" +
                                    $" WHERE zipcode = @zipcode";
            using (SqlConnection connection = ScheduleBuilder_DB_Connection.GetConnection())
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(sqlStatement, connection))
                {
                    command.Parameters.AddWithValue("@zipcode", zipcode);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            count++;
                        }
                    }
                    if (count > 0)
                    {
                        doesExist = true;
                    }
                }
                connection.Close();
            }
            return doesExist;
        }

        /// <summary>
        /// Adds a person to the database 
        /// all new persons have roleId = 3s
        /// statusId = 1
        /// password = 'newHire'
        /// </summary>
        /// <param name="lastName"></param>
        /// <param name="firstName"></param>
        /// <param name="dateOfBirth"></param>
        /// <param name="ssn"></param>
        /// <param name="gender"></param>
        /// <param name="phone"></param>
        /// <param name="streetAddress"></param>
        /// <param name="zipcode"></param>
        /// <param name="username"></param>
        /// <param name="email"></param>
        public int AddPerson(string lastName
            , string firstName
            , DateTime dateOfBirth
            , string ssn
            , string gender
            , string phone
            , string streetAddress
            , string zipcode
            , string email)
        {
            string username = $"{firstName.Substring(0, 1)}{lastName}";
            username = this.UsernameCheck(username, 0);
            Person addedPerson = new Person
            {
                LastName = lastName,
                FirstName = firstName,
                DateOfBirth = dateOfBirth,
                Ssn = ssn,
                Gender = gender,
                Phone = phone,
                StreetAddress = streetAddress,
                Zipcode = zipcode,
                Username = username,
                Email = email,
                Password = this.hashingService.PasswordHashing("newHire"),
                StatusId = 1,
                RoleId = 3
            };

            string sql = @"INSERT INTO dbo.person( 
                            [last_name] 
                            , [first_name] 
                            , [date_of_birth] 
                            , [ssn] 
                            , [gender] 
                            , [street_address] 
                            , [phone] 
                            , [zipcode] 
                            , [username] 
                            , [password] 
                            , [roleId] 
                            , [statusId] 
                            , [email]) 

                             VALUES( 
                             @LastName 
                            , @FirstName 
                            , @DateOfBirth 
                            , @Ssn 
                            , @Gender 
                            , @StreetAddress 
                            , @Phone 
                            , @Zipcode 
                            , @Username 
                            , @Password 
                            , @RoleId 
                            , @StatusId 
                            , @Email); SELECT SCOPE_IDENTITY() ";


            int id = this.SaveData(sql, addedPerson);
            return id;
        }

        private string UsernameCheck(string defaultName, int num)
        {
            string username = defaultName + num.ToString();
            // string newUsername = username;

            if (DoesUserNameExists(username))
            {
                return this.UsernameCheck(defaultName, num + 1);
            }
            //else
            //{
            ////    newUsername = username;
            //}
            return username;
        }

        private bool DoesUserNameExists(string username)
        {
            bool doesExist = false;
            string sqlStatement = "Select count(*) FROM person Where username = @username";
            try
            {
                using (SqlConnection connection = ScheduleBuilder_DB_Connection.GetConnection())
                {
                    connection.Open();
                    using (SqlCommand checkUsername = new SqlCommand(sqlStatement, connection))
                    {
                        checkUsername.Parameters.AddWithValue("@username", username);
                        Int32 count = 0;
                        count = (Int32)checkUsername.ExecuteScalar();
                        if (count > 0)
                        {
                            doesExist = true;
                        }
                    }
                    connection.Close();
                }
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            return doesExist;
        }

        /// <summary>
        /// Saves the accepted data with the accepted sql statement
        /// </summary>
        /// <typeparam name="model"></typeparam>
        /// <param name="sql"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        private int SaveData<model>(string sql, model data)
        {
            using (IDbConnection cnn = ScheduleBuilder_DB_Connection.GetConnection())
            {
                return Convert.ToInt32(cnn.ExecuteScalar(sql, data));
            }
        }

        /// <summary>
        /// this method returns all employees
        /// </summary>
        /// <returns></returns>
        public List<Person> GetDesiredPersons(string whereClause)
        {
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
        /// Allows users to edit a previously created person
        /// </summary>
        /// <param name="editPerson"></param>
        public void EditPerson(Person editPerson)
        {
            string update = @"UPDATE dbo.person
                            SET last_name = @lastName
                            , first_name = @firstName
                            , ssn = @ssn
                            , gender = @gender
                            , street_address = @streetAddress
                            , phone = @phone
                            , zipcode = @zipcode
                            , username = @username
                            , email = @email
                            , statusId = @statusId
                            , roleId = @roledId
                            WHERE id = @id 
                            AND  password = @password                            
                            AND date_of_birth = @dateOfBirth";
            int count = 0;
            try
            {
                using (SqlConnection connection = ScheduleBuilder_DB_Connection.GetConnection())
                {
                    connection.Open();
                    using (SqlCommand updateCommand = new SqlCommand(update, connection))
                    {
                        updateCommand.Parameters.AddWithValue("@lastName", editPerson.LastName);
                        updateCommand.Parameters.AddWithValue("@firstName", editPerson.FirstName);
                        updateCommand.Parameters.AddWithValue("@dateOfBirth", editPerson.DateOfBirth);
                        updateCommand.Parameters.AddWithValue("@roleId", editPerson.RoleId);

                        if (String.IsNullOrEmpty(editPerson.Ssn))
                        {
                            updateCommand.Parameters.AddWithValue("@ssn", DBNull.Value);
                        }
                        else
                        {
                            updateCommand.Parameters.AddWithValue("@ssn", editPerson.Ssn);
                        }

                        updateCommand.Parameters.AddWithValue("@gender", editPerson.Gender);
                        updateCommand.Parameters.AddWithValue("@streetAddress", editPerson.StreetAddress);
                        updateCommand.Parameters.AddWithValue("@phone", editPerson.Phone);
                        updateCommand.Parameters.AddWithValue("@zipcode", editPerson.Zipcode);
                        updateCommand.Parameters.AddWithValue("@username", editPerson.Username);
                        updateCommand.Parameters.AddWithValue("@email", editPerson.Email);
                        updateCommand.Parameters.AddWithValue("@id", editPerson.Id);
                        updateCommand.Parameters.AddWithValue("@statusId", editPerson.StatusId);
                        updateCommand.Parameters.AddWithValue("@password", editPerson.Password);

                        count = updateCommand.ExecuteNonQuery();
                    }
                    connection.Close();
                }
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Sets the accepted person's status as seperated
        /// </summary>
        /// <param name="seperatePerson"></param>
        /// <returns></returns>
        public Person SeperateEmployee(Person seperatePerson)
        {
            string update = @"UPDATE dbo.person 
                            Set statusId = 4 
                            WHERE id = @id";
            try
            {
                using (SqlConnection connection = ScheduleBuilder_DB_Connection.GetConnection())
                {
                    connection.Open();
                    using (SqlCommand updateCommand = new SqlCommand(update, connection))
                    {
                        updateCommand.Parameters.AddWithValue("@lastName", seperatePerson.LastName);
                        updateCommand.Parameters.AddWithValue("@firstName", seperatePerson.FirstName);
                        updateCommand.Parameters.AddWithValue("@dateOfBirth", seperatePerson.DateOfBirth);

                        if (seperatePerson.Ssn == "")
                        {
                            updateCommand.Parameters.AddWithValue("@ssn", DBNull.Value);
                        }
                        else
                        {
                            updateCommand.Parameters.AddWithValue("@ssn", seperatePerson.Ssn);
                        }

                        updateCommand.Parameters.AddWithValue("@gender", seperatePerson.Gender);
                        updateCommand.Parameters.AddWithValue("@streetAddress", seperatePerson.StreetAddress);
                        updateCommand.Parameters.AddWithValue("@phone", seperatePerson.Phone);
                        updateCommand.Parameters.AddWithValue("@zipcode", seperatePerson.Zipcode);
                        updateCommand.Parameters.AddWithValue("@username", seperatePerson.Username);
                        updateCommand.Parameters.AddWithValue("@email", seperatePerson.Email);
                        updateCommand.Parameters.AddWithValue("@id", seperatePerson.Id);
                        updateCommand.Parameters.AddWithValue("@roleId", seperatePerson.RoleId);
                        updateCommand.Parameters.AddWithValue("@statusId", 4);
                        updateCommand.Parameters.AddWithValue("@password", seperatePerson.Password);

                        updateCommand.ExecuteNonQuery();
                    }
                    connection.Close();
                }
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            return seperatePerson;
        }

        /// <summary>
        /// Updates a password of a new person
        /// </summary>
        /// <param name="passwordUpdated"></param>
        public void UpdatePassword(Person passwordUpdated)
        {
            HashingService hashed = new HashingService();
            string update = @"UPDATE dbo.person 
                            SET password = @password 
                            WHERE id = @id";
            int count = 0;
            try
            {
                using (SqlConnection connection = ScheduleBuilder_DB_Connection.GetConnection())
                {
                    connection.Open();
                    using (SqlCommand updateCommand = new SqlCommand(update, connection))
                    {
                        updateCommand.Parameters.AddWithValue("@password", hashed.PasswordHashing(passwordUpdated.Password));
                        updateCommand.Parameters.AddWithValue("@id", passwordUpdated.Id);
                        count = updateCommand.ExecuteNonQuery();
                    }
                    connection.Close();
                }
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Gets a persons ID by their email
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public int GetIDByEmail(string email)
        {
            Person person = new Person();
            string selectStatement =
                         "SELECT id, email " +
                         "FROM person " +
                         "WHERE email = @email";

            using (SqlConnection connection = ScheduleBuilder_DB_Connection.GetConnection())
            {
                connection.Open();
                using (SqlCommand sqlCommand = new SqlCommand(selectStatement, connection))
                {
                    sqlCommand.Parameters.AddWithValue("@email", email);
                    using (SqlDataReader reader = sqlCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            person.Id = (int)reader["id"];
                            person.Email = reader["email"].ToString();
                        }
                    }
                }
                return person.Id;
            }
        }

        /// <summary>
        /// Gets a person by their ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Person GetPersonByID(int id)
        {
            Person person = new Person();
            string selectStatement =
                         "SELECT id" +
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
                         ", email " +
                         "FROM person " +
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
                        }
                    }
                }
                return person;
            }
        }

        /// <summary>
        /// Updates a persons password with a new password based on email input in controller
        /// </summary>
        /// <param name="person"></param>
        /// <param name="newPassword"></param>
        public void UpdatePasswordOnly(Person person, string newPassword)
        {
            HashingService hashed = new HashingService();
            string update = @"UPDATE dbo.person 
                            SET password = @password 
                            WHERE id = @id";
            int count = 0;
            try
            {
                using (SqlConnection connection = ScheduleBuilder_DB_Connection.GetConnection())
                {
                    connection.Open();
                    using (SqlCommand updateCommand = new SqlCommand(update, connection))
                    {
                        updateCommand.Parameters.AddWithValue("@password", hashed.PasswordHashing(newPassword));
                        updateCommand.Parameters.AddWithValue("@id", person.Id);
                        count = updateCommand.ExecuteNonQuery();
                    }
                    connection.Close();
                }
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Returns a list of ManagerViewModels
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public List<ManagerViewModel> GetManagersView(string where)
        {
            RoleDAL role = new RoleDAL();
            List<ManagerViewModel> managers = new List<ManagerViewModel>();
            List<Person> people = this.GetDesiredPersons(where);
            foreach (Person person in people)
            {
                ManagerViewModel manager = new ManagerViewModel();
                manager.Email = person.Email;
                manager.FirstName = person.FirstName;
                manager.LastName = person.LastName;
                manager.Phone = ("(" + person.Phone.Substring(0, 3) + ") " + person.Phone.Substring(3, 3) + "-" + person.Phone.Substring(6));
                manager.Role = role.GetRoleByID(person.RoleId);
                managers.Add(manager);
            }
            return managers;
        }
    }
}
