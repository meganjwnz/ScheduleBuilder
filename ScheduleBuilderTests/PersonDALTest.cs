using Autofac.Extras.Moq;
using Moq;
using ScheduleBuilder.Controllers;
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
    //https://www.youtube.com/watch?v=DwbYxP-etMY and information from https://github.com/Moq/moq4/wiki/Quickstart 
    public class PersonDALTest 
    {
        //GetLoose test to see if a method was called - if other methods also called thats acceptable
        //AutoMock is a framework for creating mock items
        //.Setup(x => x.GetDesiredPersons(It.IsAny<string>())).Returns(GetSamplePeople()); - the It.isAny<String> IS REQUIRED DO NOT TOUCH


        /// <summary>
        /// Insures that GetAllPeople returns all the people within the database
        /// </summary>
        [Fact]
        public void TestGetAllPeople()
        {
            using (var mock = AutoMock.GetLoose())
            {
                mock.Mock<IPersonDAL>() 
                    .Setup(x => x.GetDesiredPersons(It.IsAny<string>())).Returns(GetSamplePeople());

                var personDAL = mock.Create<IPersonDAL>();

                var expected = GetSamplePeople();
                var actual = personDAL.GetDesiredPersons("");


                Assert.True(actual != null);
                Assert.Equal(expected.Count, actual.Count);

                for (int count = 0; count < expected.Count; count++)
                {
                    Assert.Equal(expected[count].FirstName, actual[count].FirstName);
                    Assert.Equal(expected[count].LastName, actual[count].LastName);
                    Assert.Equal(expected[count].RoleId, actual[count].RoleId);
                    Assert.Equal(expected[count].StatusId, actual[count].StatusId);
                    Assert.Equal(expected[count].Email, actual[count].Email);
                }
            }
        }


        [Fact]
        public void TestSeperatePerson()
        {
            using (var mock = AutoMock.GetLoose())
            {
                Mock<Person> mockPerson = new Mock<Person>();
                mock.Mock<IPersonDAL>().Setup(x => x.SeperateEmployee(It.IsAny<Person>())).Returns(SerperatedPerson());
                var personDAL = mock.Create<IPersonDAL>();
                var controlPerson = SerperatedPerson();
                var actualPerson = personDAL.SeperateEmployee(GetSamplePeople()[0]);
                Assert.True(controlPerson  != null);
                Assert.Equal(actualPerson.StatusId, controlPerson.StatusId);
                Assert.Equal(actualPerson.FirstName, controlPerson.FirstName);
                Assert.Equal(actualPerson.Gender, controlPerson.Gender);
                Assert.Equal(actualPerson.Email, controlPerson.Email);
            }

        } 

        [Fact]
        public void TestEditPerson()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var editPerson = new Person
                {
                    Id = 1,
                    LastName = "EditedDrew",
                    FirstName = "Drew",
                    Ssn = "123456789",
                    Gender = "Male",
                    DateOfBirth = DateTime.Now,
                    RoleId = 1,
                    StatusId = 1,
                    Email = "Drew@email.com",
                    StreetAddress = "1149 Grove",
                    Zipcode = "30145",
                    Phone = "7777777777",
                    Password = "pass",
                    Username = "test"
                };

                mock.Mock<IPersonDAL>().Setup(x => x.GetDesiredPersons(It.IsAny<string>())).Returns(GetSamplePeople());

                var personDAL = mock.Create<IPersonDAL>();
                personDAL.EditPerson(editPerson);

                mock.Mock<IPersonDAL>().Verify(x => x.EditPerson(editPerson), Times.Exactly(1));
            }
        }


        [Fact]
        public void TestGetAll_Active_People()
        {
            using (var mock = AutoMock.GetLoose())
            {
                mock.Mock<IPersonDAL>()
                    .Setup(x => x.GetDesiredPersons(It.IsAny<string>())).Returns(GetActivePeople());

                var personDAL = mock.Create<IPersonDAL>();

                var expected = GetActivePeople();
                var actual = personDAL.GetDesiredPersons("WHERE statusId = 1 OR statusId = 5");


                Assert.True(actual != null);
                Assert.Equal(expected.Count, actual.Count);

                for (int count = 0; count < expected.Count; count++)
                {
                    Assert.Equal(expected[count].FirstName, actual[count].FirstName);
                    Assert.Equal(expected[count].LastName, actual[count].LastName);
                    Assert.Equal(expected[count].RoleId, actual[count].RoleId);
                    Assert.Equal(expected[count].StatusId, actual[count].StatusId);
                    Assert.Equal(expected[count].Email, actual[count].Email);
                }
            }
        }


        private Person SerperatedPerson() {
            Person seperated = new Person
                {
                    LastName = "Coleman",
                    FirstName = "Drew",
                    Ssn = "123456789",
                    Gender = "Male",
                    DateOfBirth = DateTime.Now,
                    RoleId = 1,
                    StatusId = 4,
                    Email = "Drew@email.com",
                    StreetAddress = "1149 Grove",
                    Zipcode = "30145",
                    Phone = "4543631011",
                    Password = "pass",
                    Username = "test"
                };
            return seperated;
        }

        private List<Person> GetActivePeople()
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
                 }
            };
                 return output;
        }

        [Fact]
        public void Test_AddPerson()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var person = new Person
                {
                    LastName = "OneNew",
                    FirstName = "Boy",
                    Ssn = "123456219",
                    Gender = "Male",
                    DateOfBirth = DateTime.Now,
                    RoleId = 1,
                    StatusId = 1,
                    Email = "newGuy@email.com",
                    StreetAddress = "1149 Grove",
                    Zipcode = "30145",
                    Phone = "4543631011",
                    Password = "newHire",
                    Username = "testNewGuy"
                };

                mock.Mock<IPersonDAL>()
                    .Setup(x => x.AddPerson(person.LastName
                    , person.FirstName
                    , person.DateOfBirth
                    , person.Ssn
                    , person.Gender
                    , person.Phone
                    , person.StreetAddress
                    , person.Zipcode
                    , person.Email));

                var cls = mock.Create<IPersonDAL>();

                cls.AddPerson(person.LastName
                    , person.FirstName
                    , person.DateOfBirth
                    , person.Ssn
                    , person.Gender
                    , person.Phone
                    , person.StreetAddress
                    , person.Zipcode
                    , person.Email);

                mock.Mock<IPersonDAL>().Verify(x => x.AddPerson(person.LastName
                    , person.FirstName
                    , person.DateOfBirth
                    , person.Ssn
                    , person.Gender
                    , person.Phone
                    , person.StreetAddress
                    , person.Zipcode
                    , person.Email), Times.Exactly(1));
            }
        }

        //Provides a dummy list of data
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
                    StatusId = 2,
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
                     StatusId = 3,
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
                     StatusId = 3,
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
