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
        public static int addPerson(string lastName
            , string firstName
            , DateTime dateOfBirth
            , char ssn
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
                Gender = gender,
                Phone = phone,
                StreetAddress = streetAddress,
                Zipcode = zipcode,
                Username = username,
                Email = email,
                Password = "newHire",
                StatusId = 1,
                RoleId = 1

            };
            return 1;//NOT FINISHED
        }

    }
}