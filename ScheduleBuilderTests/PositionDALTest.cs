using Autofac.Extras.Moq;
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

        //populates a list of roles
        private List<Position> GetSamplePositions()
        {
            List<Position> output = new List<Position> {
                new Position
                {
                    positionID = 1,
                    positionTitle = "Owner",
                    isActive = 1,
                    positionDescription = "Highest Authority",
                },

                new Position
                {
                    positionID = 2,
                    positionTitle = "Cook",
                    isActive = 1,
                    positionDescription = "Cooks orders to recipe",
                },

                new Position
                {
                    positionID = 2,
                    positionTitle = "Waitress",
                    isActive = 1,
                    positionDescription = "Serves customers",
                }
            };
            return output;
        }
    }
}
