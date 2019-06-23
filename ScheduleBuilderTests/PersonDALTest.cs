using Autofac.Extras.Moq;
using ScheduleBuilder.BusinessLogic;
using ScheduleBuilder.DAL;
using ScheduleBuilder.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ScheduleBuilderTests
{
    //This testing framework was develeped using methods from Tim Corey Course found
    //https://www.youtube.com/watch?v=DwbYxP-etMY
    public class PersonDALTest //: PersonProcessor

    {
        //    ISqliteDataAccess _database;


        //GetLoose test to see if a method was called - if other methods also called thats acceptable
        //AutoMock is a framework for creating mock items
        [Fact]
        public void testGetAllPeople()
        {
            using (var mock = AutoMock.GetLoose())
            {
                mock.Mock<PersonDAL>()
                    .Setup(x => x.GetDesiredPersons(""))
                    .Returns(GetSamplePeople());
            }
        }

        private List<Person> GetSamplePeople()
        {
            List<Person> output = new List<Person> {
                new Person
                {
                    LastName = "Coleman",
                    FirstName = "Drew",
                    Ssn = "123456789",
                    Gender = "Male",
                    DateOfBirth = DateTime.Now,
                    RoleId = 1,
                    StatusId = 1,
                    Email = "Drew@email.com",
                    StreetAddress = "1149 Grove",
                    Zipcode = "30145",
                    Phone = "4543631011",
                    Password = "pass",
                    Username = "test"
                },
                 new Person
                 {
                     LastName = "Coleman",
                     FirstName = "Keeleigh",
                     Ssn = "987654321",
                     Gender = "Female",
                     DateOfBirth = DateTime.Now,
                     RoleId = 1,
                     StatusId = 1,
                     Email = "K@email.com",
                     StreetAddress = "1149 Grove",
                     Zipcode = "30145",
                     Phone = "4543631011",
                     Password = "pass",
                     Username = "test"
                 },
                 new Person
                 {
                     LastName = "Coleman",
                     FirstName = "Wayne",
                     Ssn = "987651321",
                     Gender = "Male",
                     DateOfBirth = DateTime.Now,
                     RoleId = 1,
                     StatusId = 1,
                     Email = "Wayne@email.com",
                     StreetAddress = "1149 Grove",
                     Zipcode = "30145",
                     Phone = "4543631011",
                     Password = "pass",
                     Username = "test"
                 },
                 new Person
                 {
                     LastName = "Coleman",
                     FirstName = "Witten",
                     Ssn = "987654351",
                     Gender = "Male",
                     DateOfBirth = DateTime.Now,
                     RoleId = 1,
                     StatusId = 1,
                     Email = "Witten@email.com",
                     StreetAddress = "1149 Grove",
                     Zipcode = "30145",
                     Phone = "4543631011",
                     Password = "pass",
                     Username = "test"
                 }
            };
            return output;
        }
    }
}
