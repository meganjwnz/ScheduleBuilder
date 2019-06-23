using Autofac.Extras.Moq;
using Moq;
using ScheduleBuilder.BusinessLogic;
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
    //https://www.youtube.com/watch?v=DwbYxP-etMY
    public class PersonDALTest 
    {
        //GetLoose test to see if a method was called - if other methods also called thats acceptable
        //AutoMock is a framework for creating mock items
        //.Setup(x => x.GetDesiredPersons(It.IsAny<string>())).Returns(GetSamplePeople()); - the It.isAny<String> IS REQUIRED DO NOT TOUCH
        [Fact]
        public void testGetAllPeople()
        {
            using (var mock = AutoMock.GetLoose())
            {
                mock.Mock<IPersonDAL>()
                    .Setup(x => x.GetDesiredPersons(It.IsAny<string>())).Returns(GetSamplePeople());

                var cls = mock.Create<IPersonDAL>();

                var expected = GetSamplePeople();
                var actual = cls.GetDesiredPersons("");


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
