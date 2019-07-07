using Autofac.Extras.Moq;
using Moq;
using System;
using System.Collections.Generic;
using Xunit;
using ScheduleBuilder.DAL;
using ScheduleBuilder.Model;

namespace ScheduleBuilderTests
{
    public class ShiftDALTest
    {

        /// <summary>
        /// Insures that GetAllShifts returns all the shifts within the database
        /// </summary>
        [Fact]
        public void TestGetAllShifts()
        {
            using (var mock = AutoMock.GetLoose())
            {
                mock.Mock<IShiftDAL>()
                    .Setup(x => x.GetAllShifts()).Returns(GetSampleShifts());

                var shiftDAL = mock.Create<IShiftDAL>();

                var expected = GetSampleShifts();
                var actual = shiftDAL.GetAllShifts();


                Assert.True(actual != null);
                Assert.Equal(expected.Count, actual.Count);

                for (int count = 0; count < expected.Count; count++)
                {
                    Assert.Equal(expected[count].shiftID, actual[count].shiftID);
                    Assert.Equal(expected[count].scheduleShiftID, actual[count].scheduleShiftID);
                    Assert.Equal(expected[count].personID, actual[count].personID);
                    Assert.Equal(expected[count].personFirstName, actual[count].personFirstName);
                    Assert.Equal(expected[count].personLastName, actual[count].personLastName);
                    Assert.Equal(expected[count].positionID, actual[count].positionID);
                    Assert.Equal(expected[count].positionName, actual[count].positionName);
                    Assert.Equal(expected[count].scheduledStartTime, actual[count].scheduledStartTime);
                    Assert.Equal(expected[count].scheduledEndTime, actual[count].scheduledEndTime);
                    Assert.Equal(expected[count].scheduledLunchBreakStart, actual[count].scheduledLunchBreakStart);
                    Assert.Equal(expected[count].scheduledLunchBreakEnd, actual[count].scheduledLunchBreakEnd);
                    Assert.Equal(expected[count].actualStartTime, actual[count].actualStartTime);
                    Assert.Equal(expected[count].actualEndTime, actual[count].actualEndTime);
                    Assert.Equal(expected[count].actualLunchBreakStart, actual[count].actualLunchBreakStart);
                    Assert.Equal(expected[count].actualLunchBreakEnd, actual[count].actualLunchBreakEnd);
                }
            }
        }

        /// <summary>
        /// Return the nearest shift object based on where input statement
        /// </summary>
        [Fact]
        public void TestGetNearestShift()
        {
            using (var mock = AutoMock.GetLoose())
            {
                mock.Mock<IShiftDAL>()
                    .Setup(x => x.GetNearestShift(" WHERE s.personId = 1"));

                var shiftDAL = mock.Create<IShiftDAL>();

                var expected = new Shift
                {
                    shiftID = 1,
                    scheduleShiftID = 1,
                    personID = 1,
                    personFirstName = "Melissa",
                    personLastName = "Osborne",
                    positionID = 1,
                    positionName = "International Spy",
                    scheduledStartTime = DateTime.Today.AddHours(4),
                    scheduledEndTime = DateTime.Today,
                    scheduledLunchBreakStart = DateTime.Today,
                    scheduledLunchBreakEnd = DateTime.Today,
                    actualStartTime = DateTime.Today,
                    actualEndTime = DateTime.Today,
                    actualLunchBreakStart = DateTime.Today,
                    actualLunchBreakEnd = DateTime.Today
                };

                shiftDAL.GetNearestShift(" WHERE s.personId = 1");

                mock.Mock<IShiftDAL>().Verify(x => x.GetNearestShift(" WHERE s.personId = 1"), Times.Exactly(1));
            }
            }
        
        /// <summary>
        /// Test that a shift can be added succecssfully
        /// </summary>
        [Fact]
        public void TestAddShift()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var shift = new Shift
                {
                    shiftID = 7,
                    scheduleShiftID = 15,
                    personID = 43,
                    personFirstName = "Cooper",
                    personLastName = "Speer",
                    positionID = 45,
                    positionName = "Boss",
                    scheduledStartTime = DateTime.Today,
                    scheduledEndTime = DateTime.Today,
                    scheduledLunchBreakStart = DateTime.Today,
                    scheduledLunchBreakEnd = DateTime.Today,
                    actualStartTime = DateTime.Today,
                    actualEndTime = DateTime.Today,
                    actualLunchBreakStart = DateTime.Today,
                    actualLunchBreakEnd = DateTime.Today
                };
                var taskList = new Dictionary<int, bool>();

                mock.Mock<IShiftDAL>().Setup(x => x.AddShift(shift, taskList));

                var cls = mock.Create<IShiftDAL>();

                cls.AddShift(shift, taskList);

                mock.Mock<IShiftDAL>().Verify(x => x.AddShift(shift, taskList), Times.Exactly(1));
            }
        }

        /// <summary>
        /// Test that a shift can be updated successfully
        /// </summary>
        [Fact]
        public void TestUpdateShift()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var updatedshift = new Shift
                {
                    shiftID = 1,
                    scheduleShiftID = 15,
                    personID = 43,
                    personFirstName = "Cooper",
                    personLastName = "Speer",
                    positionID = 45,
                    positionName = "Boss",
                    scheduledStartTime = DateTime.Today,
                    scheduledEndTime = DateTime.Today,
                    scheduledLunchBreakStart = DateTime.Today,
                    scheduledLunchBreakEnd = DateTime.Today,
                    actualStartTime = DateTime.Today,
                    actualEndTime = DateTime.Today,
                    actualLunchBreakStart = DateTime.Today,
                    actualLunchBreakEnd = DateTime.Today
                };
                var taskList = new Dictionary<int, bool>();

                mock.Mock<IShiftDAL>().Setup(x => x.GetAllShifts()).Returns(GetSampleShifts());

                var shiftDAL = mock.Create<IShiftDAL>();
                shiftDAL.UpdateShift(updatedshift, taskList);

                mock.Mock<IShiftDAL>().Verify(x => x.UpdateShift(updatedshift, taskList), Times.Exactly(1));
            }
        }

        /// <summary>
        /// Tests that a shift and associations can be removed
        /// </summary>
        [Fact]
        public void TestDeleteShift()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var deleteshift = new Shift
                {
                    shiftID = 1,
                    scheduleShiftID = 15,
                    personID = 43,
                    personFirstName = "Cooper",
                    personLastName = "Speer",
                    positionID = 45,
                    positionName = "Boss",
                    scheduledStartTime = DateTime.Today,
                    scheduledEndTime = DateTime.Today,
                    scheduledLunchBreakStart = DateTime.Today,
                    scheduledLunchBreakEnd = DateTime.Today,
                    actualStartTime = DateTime.Today,
                    actualEndTime = DateTime.Today,
                    actualLunchBreakStart = DateTime.Today,
                    actualLunchBreakEnd = DateTime.Today
                };

                mock.Mock<IShiftDAL>().Setup(x => x.GetAllShifts()).Returns(GetSampleShifts());

                var shiftDAL = mock.Create<IShiftDAL>();
                shiftDAL.DeleteShift(deleteshift);

                mock.Mock<IShiftDAL>().Verify(x => x.DeleteShift(deleteshift));
            }

        }

        /// <summary>
        /// A dummy list of shifts (mock uses this as a return from the db)
        /// </summary>
        /// <returns>A list of shift objects</returns>
        private List<Shift> GetSampleShifts()
        {
            List<Shift> output = new List<Shift> {
                
                new Shift
                {
                    shiftID = 1,
                    scheduleShiftID = 1,
                    personID = 1,
                    personFirstName = "Melissa",
                    personLastName = "Osborne",
                    positionID = 1,
                    positionName = "International Spy",
                    scheduledStartTime = DateTime.Today.AddHours(4),
                    scheduledEndTime = DateTime.Today,
                    scheduledLunchBreakStart = DateTime.Today,
                    scheduledLunchBreakEnd = DateTime.Today,
                    actualStartTime = DateTime.Today,
                    actualEndTime = DateTime.Today,
                    actualLunchBreakStart = DateTime.Today,
                    actualLunchBreakEnd = DateTime.Today
                },
                //no clocked hours
                  new Shift
                {
                    shiftID = 2,
                    scheduleShiftID = 2,
                    personID = 2,
                    personFirstName = "Maggie Rose",
                    personLastName = "Osborne",
                    positionID = 2,
                    positionName = "Engineer",
                    scheduledStartTime = DateTime.Today,
                    scheduledEndTime = DateTime.Today,
                    scheduledLunchBreakStart = DateTime.Today,
                    scheduledLunchBreakEnd = DateTime.Today,
                    actualStartTime = null,
                    actualEndTime = null,
                    actualLunchBreakStart = null,
                    actualLunchBreakEnd = null
                },
                  //no lunch break
                  new Shift
                {
                    shiftID = 3,
                    scheduleShiftID = 3,
                    personID = 3,
                    personFirstName = "Molly",
                    personLastName = "Osborne",
                    positionID = 3,
                    positionName = "Guardian",
                    scheduledStartTime = DateTime.Today,
                    scheduledEndTime = DateTime.Today,
                    scheduledLunchBreakStart = null,
                    scheduledLunchBreakEnd = null,
                    actualStartTime = null,
                    actualEndTime = null,
                    actualLunchBreakStart = null,
                    actualLunchBreakEnd = null
                },
                 new Shift
                {
                    shiftID = 4,
                    scheduleShiftID = 4,
                    personID = 4,
                    personFirstName = "Frogger",
                    personLastName = "Osborne",
                    positionID = 4,
                    positionName = "Comic Relief",
                    scheduledStartTime = DateTime.Today,
                    scheduledEndTime = DateTime.Today,
                    scheduledLunchBreakStart = DateTime.Today,
                    scheduledLunchBreakEnd = DateTime.Today,
                    actualStartTime = DateTime.Today,
                    actualEndTime = DateTime.Today,
                    actualLunchBreakStart = DateTime.Today,
                    actualLunchBreakEnd = DateTime.Today
                }
            };
            return output;


        }
    }
}
