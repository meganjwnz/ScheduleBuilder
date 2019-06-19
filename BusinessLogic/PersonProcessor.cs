using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ScheduleBuilder.Model;
using ScheduleBuilder.DAL;


namespace ScheduleBuilder.BusinessLogic
{
    public class PersonProcessor
    {
        PersonDAL personDAL = new PersonDAL();
        HashingService hashingService = new HashingService();
        public int addPerson(string lastName
            , string firstName
            , DateTime dateOfBirth
            , string ssn
            , string gender
            , string phone
            , string streetAddress
            , string zipcode
            , string username
            , string email)
        {
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
                            , @Email); ";


            return personDAL.SaveData(sql, addedPerson);
        }

        public List<Person> LoadPeople()
        {
            string sql = @"Select * From dbo.person;";
            return this.personDAL.LoadData<Person>(sql);
        }

    }
}