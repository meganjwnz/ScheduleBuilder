using Autofac.Extras.Moq;
using ScheduleBuilder.DAL;
using ScheduleBuilder.Model;
using System;
using System.Collections.Generic;
using Xunit;

namespace ScheduleBuilderTests
{
    public class PositionDALTest
    {
        //Tests getting all the positions
        [Fact]
        public void TestGetAllActivePositions()
        {
            using (var mock = AutoMock.GetLoose())
            {
                mock.Mock<IPositionDAL>()
                    .Setup(x => x.GetAllActivePositions()).Returns(GetSamplePositions());

                var positionDAL = mock.Create<IPositionDAL>();

                var expected = GetSamplePositions();
                var actual = positionDAL.GetAllActivePositions();


                Assert.True(actual != null);
                Assert.Equal(expected.Count, actual.Count);

                for (int count = 0; count < expected.Count; count++)
                {
                    Assert.Equal(expected[count].positionID, actual[count].positionID);
                    Assert.Equal(expected[count].positionTitle, actual[count].positionTitle);
                    Assert.Equal(expected[count].positionDescription, actual[count].positionDescription);
                    Assert.Equal(expected[count].isActive, actual[count].isActive);
                }
            }
        }

        //Tests getting all the positions
        [Fact]
        public void TestGetAllPositions()
        {
            using (var mock = AutoMock.GetLoose())
            {
                mock.Mock<IPositionDAL>()
                    .Setup(x => x.GetAllActivePositions()).Returns(GetSamplePositions());

                var positionDAL = mock.Create<IPositionDAL>();

                var expected = GetSamplePositions();
                var actual = positionDAL.GetAllPositions();


                Assert.True(actual != null);
                Assert.Equal(expected.Count, actual.Count);

                for (int count = 0; count < expected.Count; count++)
                {
                    Assert.Equal(expected[count].positionID, actual[count].positionID);
                    Assert.Equal(expected[count].positionTitle, actual[count].positionTitle);
                    Assert.Equal(expected[count].positionDescription, actual[count].positionDescription);
                    Assert.Equal(expected[count].isActive, actual[count].isActive);
                }
            }
        }

        [Fact]
        public void TestGetPersonPosition()
        {
            using (var mock = AutoMock.GetLoose())
            {
                mock.Mock<IPositionDAL>()
                    .Setup(x => x.GetAllActivePositions()).Returns(GetSamplePositions());

                var positionDAL = mock.Create<IPositionDAL>();

                var expected = GetSamplePositions();

                Person person = new Person
                        {
                            LastName = "Doe",
                            FirstName = "Jane",
                            Ssn = "123456789",
                            Gender = "Female",
                            DateOfBirth = DateTime.Now,
                            RoleId = 1,
                            StatusId = 1,
                            Email = "jdoe@email.com",
                            StreetAddress = "605 Dance Street",
                            Zipcode = "30101",
                            Phone = "1234443234",
                            Password = "pass",
                            Username = "test"
                        };
                var actual = positionDAL.GetPersonPositions(1);

                Assert.True(actual != null);
                Assert.Equal(expected.Count, actual.Count);

                for (int count = 0; count < expected.Count; count++)
                {
                    Assert.Equal(expected[count].positionID, actual[count].positionID);
                    Assert.Equal(expected[count].positionTitle, actual[count].positionTitle);
                    Assert.Equal(expected[count].positionDescription, actual[count].positionDescription);
                    Assert.Equal(expected[count].isActive, actual[count].isActive);
                }
            }
        }

        //populates a list of roles
        private List<Position> GetSamplePositions()
        {
            List<Position> output = new List<Position> {
                new Position
                {
                    positionID = 1,
                    positionTitle = "Owner",
                    isActive = true,
                    positionDescription = "Highest Authority",
                },

                new Position
                {
                    positionID = 2,
                    positionTitle = "Cook",
                    isActive = true,
                    positionDescription = "Cooks orders to recipe",
                },

                new Position
                {
                    positionID = 2,
                    positionTitle = "Waitress",
                    isActive = true,
                    positionDescription = "Serves customers",
                }
            };
            return output;
        }
    }
}
