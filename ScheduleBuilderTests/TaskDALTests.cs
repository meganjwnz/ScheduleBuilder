using Autofac.Extras.Moq;
using Moq;
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

        [Fact]
        public void TestAddPositionTask()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var task = new Task
                {
                    Task_title = "Waitress",
                    Task_description = "Serve customers",
                    IsActive = true
                };

                mock.Mock<ITaskDAL>().Setup(x => x.AddPositionTask(task));

                var cls = mock.Create<ITaskDAL>();

                cls.AddPositionTask(task);

                mock.Mock<ITaskDAL>().Verify(x => x.AddPositionTask(task), Times.Exactly(1));
            }
        }

        [Fact]
        public void TestUpdatePositionTask()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var updatedtask = new Task
                {
                    Task_title = "Cook",
                    Task_description = "Cooks the food",
                    IsActive = true
                };

                mock.Mock<ITaskDAL>().Setup(x => x.GetAllTasks()).Returns(GetSampleTasks());

                var taskDAL = mock.Create<ITaskDAL>();
                taskDAL.UpdatePositionTask(updatedtask);

                mock.Mock<ITaskDAL>().Verify(x => x.UpdatePositionTask(updatedtask), Times.Exactly(1));
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
