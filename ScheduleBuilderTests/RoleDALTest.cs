using Autofac.Extras.Moq;
using Moq;
using ScheduleBuilder.DAL;
using ScheduleBuilder.Model;
using System.Collections.Generic;
using Xunit;

namespace ScheduleBuilderTests
{
    public class RoleDALTest
    {
        //Tests getting all the roles
        [Fact]
        public void TestGetRoles ()
        {
            using (var mock = AutoMock.GetLoose())
            {
                mock.Mock<IRoleDAL>()
                    .Setup(x => x.GetRoles()).Returns(GetSampleRoles());

                var roleDAL = mock.Create<IRoleDAL>();

                var expected = GetSampleRoles();
                var actual = roleDAL.GetRoles();


                Assert.True(actual != null);
                Assert.Equal(expected.Count, actual.Count);

                for (int count = 0; count < expected.Count; count++)
                {
                    Assert.Equal(expected[count].Id, actual[count].Id);
                    Assert.Equal(expected[count].RoleTitle, actual[count].RoleTitle);
                    Assert.Equal(expected[count].RoleDescription, actual[count].RoleDescription);
                }
            }
        }
        
        //Tests getting a role by its id
        [Fact]
        public void TestGetRoleById()
        {
            using (var mock = AutoMock.GetLoose())
            {
                mock.Mock<IRoleDAL>()
                    .Setup(x => x.GetRoleByID(It.IsAny<int>())).Returns(GetSingleRole().RoleTitle);

                var roleDAL = mock.Create<IRoleDAL>();

                var expected = GetSingleRole().RoleTitle;
                var actual = roleDAL.GetRoleByID(1);

                Assert.Equal(expected, actual);      
            }
        }

        //Tests getting a roles ID by its title
        [Fact]
        public void TestGetRoleByRoleTitle()
        {
            using (var mock = AutoMock.GetLoose())
            {
                mock.Mock<IRoleDAL>()
                    .Setup(x => x.GetRoleIdByTitle(It.IsAny<string>())).Returns(GetSingleRole().Id);

                var roleDAL = mock.Create<IRoleDAL>();

                var expected = GetSingleRole().Id;
                var actual = roleDAL.GetRoleIdByTitle("Employee");

                Assert.Equal(expected, actual);
            }
        }

        //populates a list of roles
        private List<Role> GetSampleRoles()
        {
            List<Role> output = new List<Role> {
                new Role
                {
                    Id = 1,
                    RoleTitle = "Employee",
                    RoleDescription = "In charge of customer orders and experience",
                },

                new Role
                {
                    Id = 2,
                    RoleTitle = "Manager",
                    RoleDescription = "In charge of day to day operations",
                },

                new Role
                {
                    Id = 3,
                    RoleTitle = "Administrator",
                    RoleDescription = "In charge of administration"
                }
            };
            return output;
        }

        //creates a single role
        private Role GetSingleRole()
        {
            Role output = new Role
            {
                Id = 1,
                RoleTitle = "Employee",
                RoleDescription = "In charge of customer orders and experience"
            };
            return output;
        }
    }
}
