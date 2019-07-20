using Autofac.Extras.Moq;
using ScheduleBuilder.DAL;
using ScheduleBuilder.Model;
using System.Collections.Generic;
using Xunit;

namespace ScheduleBuilderTests
{
    public class TaskDALTests
    {
        [Fact]
        public void TestGetAllTasks()
        {
            using (var mock = AutoMock.GetLoose())
            {
                mock.Mock<ITaskDAL>()
                    .Setup(x => x.GetAllTasks()).Returns(GetSampleTasks());

                var taskDAL = mock.Create<ITaskDAL>();

                var expected = GetSampleTasks();
                var actual = taskDAL.GetAllTasks();


                Assert.True(actual != null);
                Assert.Equal(expected.Count, actual.Count);

                for (int count = 0; count < expected.Count; count++)
                {
                    Assert.Equal(expected[count].TaskId, actual[count].TaskId);
                    Assert.Equal(expected[count].Task_title, actual[count].Task_title);
                    Assert.Equal(expected[count].Task_description, actual[count].Task_description);
                }
            }
        }

        private List<Task> GetSampleTasks()
        {
            List<Task> output = new List<Task> {
                new Task
                {
                    Task_title = "Dancer",
                    Task_description = "Dances"
                },

                new Task
                {
                    Task_title = "Singer",
                    Task_description = "Sings"
                },

                new Task
                {
                    Task_title = "Musician",
                    Task_description = "Makes music"
                }
            };
            return output;
        }
    }
}
