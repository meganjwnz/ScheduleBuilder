using Autofac.Extras.Moq;
using Moq;
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
                    .Setup(x => x.GetAllPositions()).Returns(GetSamplePositions());

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
                    .Setup(x => x.GetPersonPositions(It.IsAny<int>())).Returns(GetSamplePositions());

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

                var actual = positionDAL.GetPersonPositions(person.Id);

                Assert.True(actual != null);
                Assert.Equal(expected.Count, actual.Count);
                Assert.Equal(expected[0].positionID, actual[0].positionID);
                Assert.Equal(expected[0].positionTitle, actual[0].positionTitle);
                Assert.Equal(expected[0].positionDescription, actual[0].positionDescription);
                Assert.Equal(expected[0].isActive, actual[0].isActive);
            }
        }

        [Fact]
        public void TestAddPosition()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var position = new Position
                {
                    positionTitle = "New Position",
                    positionDescription = "Test Position",
                    isActive = true
                };

                mock.Mock<IPositionDAL>()
                    .Setup(x => x.AddPosition(position));

                var newPosition = mock.Create<IPositionDAL>();

                newPosition.AddPosition(position);

                mock.Mock<IPositionDAL>().Verify(x => x.AddPosition(position), Times.Exactly(1));
            }
        }

        [Fact]
        public void TestUpdatePosition()
        {
            
            using (var mock = AutoMock.GetLoose())
            {
                var position = new Position
                {
                    positionTitle = "New Position",
                    positionDescription = "Test Position",
                    isActive = true
                };
                mock.Mock<IPositionDAL>().Setup(x => x.GetAllActivePositions()).Returns(GetSamplePositions());

                var positionDAL = mock.Create<IPositionDAL>();
                positionDAL.UpdatePosition(position);

                mock.Mock<IPositionDAL>().Verify(x => x.UpdatePosition(position), Times.Exactly(1));
            }
        }

        [Fact]
        public void TestAddPositionToPerson()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var position = new Position
                {
                    positionTitle = "New Position",
                    positionDescription = "Test Position",
                    isActive = true
                };

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

                mock.Mock<IPositionDAL>()
                    .Setup(x => x.AddPositionToPerson(It.IsAny<int>(), It.IsAny<int>()));

                var newPosition = mock.Create<IPositionDAL>();

                newPosition.AddPositionToPerson(person.Id, position.positionID);

                mock.Mock<IPositionDAL>().Verify(x => x.AddPositionToPerson(person.Id, position.positionID), Times.Exactly(1));
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
