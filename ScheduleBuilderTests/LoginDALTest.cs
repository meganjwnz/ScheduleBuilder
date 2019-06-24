using Autofac.Extras.Moq;
using Moq;
using ScheduleBuilder.DAL;
using ScheduleBuilder.Model;
using System;
using System.Data;
using Xunit;

namespace ScheduleBuilderTests
{
    public class LoginDALTest
    {
        //Test to confirm data table returns appropriate information of user that is logging in
        [Fact]
        public void GetLoginTest()
        {
            using (var mock = AutoMock.GetLoose())
            {
                mock.Mock<ILoginDAL>()
                    .Setup(x => x.GetLogin(It.IsAny<string>(), It.IsAny<string>())).Returns(GetSampleUserDataTable());

                var loginDAL = mock.Create<ILoginDAL>();

                var dt = GetSampleUserDataTable();
                var expectedName = dt.Rows[0]["name"];
                var expectedID = dt.Rows[0]["id"];
                var expectedUsername = dt.Rows[0]["username"];
                var expectedPassword = dt.Rows[0]["password"];


                var actualName = loginDAL.GetLogin("mbrown","mbrownpass");
                var actualTitle = loginDAL.GetLogin("mbrown", "mbrownpass");
                var actualID = loginDAL.GetLogin("mbrown", "mbrownpass");
                var actualUsername = loginDAL.GetLogin("mbrown", "mbrownpass");
                var actualPassword = loginDAL.GetLogin("mbrown", "mbrownpass");

                Assert.Equal(expectedID, actualID.Rows[0]["id"]);
                Assert.Equal(expectedName, actualName.Rows[0]["name"]);
                Assert.Equal(expectedUsername, actualName.Rows[0]["username"]);
                Assert.Equal(expectedPassword, actualTitle.Rows[0]["password"]);
            }
        }

        //Provides a dummy list of data
        private DataTable GetSampleUserDataTable()
        {
            Person output = new Person {
                    LastName = "Brown",
                    FirstName = "Megan",
                    Ssn = "123456789",
                    Gender = "Female",
                    DateOfBirth = DateTime.Now,
                    RoleId = 1,
                    StatusId = 1,
                    Email = "megan@email.com",
                    StreetAddress = "605 Buddy West",
                    Zipcode = "30263",
                    Phone = "6788775774",
                    Password = "mbrownpass",
                    Username = "mbrown"
            };
            DataTable dt = new DataTable();
            dt.Columns.Add("id", typeof(int));
            dt.Columns.Add("username", typeof(string));
            dt.Columns.Add("password", typeof(string));
            dt.Columns.Add("name", typeof(string));

            DataRow dtRow = dt.NewRow();
            dtRow["id"] = output.Id;
            dtRow["username"] = output.Username;
            dtRow["password"] = output.Password;
            dtRow["name"] = output.FirstName + " " + output.LastName;

            dt.Rows.Add(dtRow);
            return dt;
        }
    }
}
