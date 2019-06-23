using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using ScheduleBuilder.Controllers;
using ScheduleBuilder.Model;
using System;
using System.Web.Mvc;
using Xunit;

namespace ScheduleBuilderTests
{
    public class LoginTest
    {

        public Person person = new Person
        {
            LastName = "Doe",
            FirstName = "Jane",
            Ssn = "123456789",
            Gender = "Female",
            DateOfBirth = DateTime.Now,
            RoleId = 1,
            StatusId = 1,
            Email = "jdoe@email.com",
            StreetAddress = "3163 Boogieland",
            Zipcode = "30263",
            Phone = "6785443445",
            Password = "pass",
            Username = "test"
        };

        [Fact]
        public void ShouldLogUserIn()
        {
            HomeController home = new HomeController();
            ActionResult result = home.Login(person);
            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsNotInstanceOfType(result, typeof(RedirectToRouteResult));
        }
    }
}
