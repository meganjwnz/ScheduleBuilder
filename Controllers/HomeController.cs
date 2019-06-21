using ScheduleBuilder.DAL;
using ScheduleBuilder.Model;
using System.Collections.Generic;
using System.Data;
using System.Web.Mvc;
using System.Web.Security;

namespace ScheduleBuilder.Controllers
{
    public class HomeController : Controller
    {
 
        [HttpPost]
        public ActionResult SearchPeople(string searchParam)
        {
            string param = Request.Form["SearchString"];
            List <Person> people = StaticPersonDAL.GetDesiredPersons();
            List<Person> searchedPeople = people.FindAll(x => x.FirstName.Contains(param) || x.LastName.Contains(param));
            return View(searchedPeople);
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            return View();
        }

        public ActionResult PeopleDirectory()
        {
            return View();
        }
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(Person person)
        {
            ViewBag.Error = "Invalid Username or Password";
            if (person.Username == null || person.Username == " " || person.Password == null || person.Password == " ")
            {
                return View();
            }
            DataTable dataTable = LoginDAL.GetLogin(person.Username, person.Password);

            if (dataTable.Rows.Count > 0)
            {
                Session["user"] = dataTable.Rows[0]["name"];
                Session["roleTitle"] = dataTable.Rows[0]["roleTitle"];
                Session["id"] = dataTable.Rows[0]["id"];
                FormsAuthentication.SetAuthCookie(person.Username, true);
                return View("Index");
            }
            return View();
        }

        public ActionResult LogOut()
        {
            Session.Clear();
            Session.Abandon();
            FormsAuthentication.SignOut();
            return View("Login");
        }
    }
}