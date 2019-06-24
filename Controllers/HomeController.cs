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
        /// <summary>
        /// Returns home page
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Retuns the about page
        /// </summary>
        /// <returns></returns>
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }


        /// <summary>
        /// Returns the contact page
        /// </summary>
        /// <returns></returns>
        public ActionResult Contact()
        {
            return View();
        }

        /// <summary>
        /// Returns the peopleDirectory page
        /// </summary>
        /// <returns></returns>
        public ActionResult PeopleDirectory()
        {
            return View();
        }

        /// <summary>
        /// Allows users to login
        /// </summary>
        /// <returns></returns>
        public ActionResult Login()
        {
            return View();
        }

        /// <summary>
        /// Recives the posted information and either logs user in or informs them of incorrect username/pass
        /// </summary>
        /// <param name="person"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Login(Person person)
        {
            ViewBag.Error = "Invalid Username or Password";
            if (person.Username == null || person.Username == " " || person.Password == null || person.Password == " ")
            {
                return View();
            }
            LoginDAL loginDAL = new LoginDAL();
            DataTable dataTable = loginDAL.GetLogin(person.Username, person.Password);

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