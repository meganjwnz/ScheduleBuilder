using Autofac.Extras.Moq;
using Moq;
using ScheduleBuilder.DAL;
using ScheduleBuilder.Model;
using Xunit;

namespace ScheduleBuilderTests
{
    public class StatusDALTest
    {
        //Tests getting a status by its id
        [Fact]
        public void TestGetStatusByID()
        {
            using (var mock = AutoMock.GetLoose())
            {
                mock.Mock<IStatusDAL>()
                    .Setup(x => x.GetStatusByID(It.IsAny<int>())).Returns(GetStatusData());

                var roleDAL = mock.Create<IStatusDAL>();

                var expected = GetStatusData().StatusDescription;
                var actual = roleDAL.GetStatusByID(3);

                Assert.Equal(expected, actual.StatusDescription);
            }
        }

        //creates a single role
        private Status GetStatusData()
        {
            Status output = new Status
            {
                Id = 1,
                StatusDescription = "Fully able to work",
                IsAbleToWork = true,
                StatusTitle = "No restrictions"

            };
            return output;
        }
    }
}
