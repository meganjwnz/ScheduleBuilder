using ScheduleBuilder.DAL;
using ScheduleBuilder.Model;
using ScheduleBuilder.ModelViews;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;

namespace ScheduleBuilder.Controllers
{
    public class HomeController : Controller
    {
        private bool success;
        RoleDAL roleDAL = new RoleDAL();

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
            string whereClause = " Where roleId < 3 ";
            List<ManagerViewModel> managers = this.personDAL.GetManagersView(whereClause);
            return View(managers);
        }


        /// <summary>
        /// Returns the peopleDirectory page
        /// </summary>
        /// <returns></returns>
        public ActionResult PeopleDirectory()
        {
            return View();
        }

        #region Login/Logout & New User functionality
        /// <summary>
        /// Allows users to login
        /// </summary>
        /// <returns></returns>
        public ActionResult Login()
        {
            if (this.success == true)
            {
                ViewBag.loginAgain = "You've successfully changed your password! Please login again.";
            }
            return View();
        }

        /// <summary>
        /// Recives the posted information and either logs user in or informs them of incorrect username/pass
        /// </summary>
        /// <param name="person"></param>
        /// <returns></returns>
        PersonDAL personDAL = new PersonDAL();
        [HttpPost]
        public ActionResult Login(Person person)
        {
            ViewBag.Error = "Invalid Username or Password";
            if (person.Username == null || person.Username == " " || person.Password == null || person.Password == " ")
            {
                ViewBag.Error = "Username and/or password must not be blank";
                return View();
            }
            LoginDAL loginDAL = new LoginDAL();
            DataTable dataTable = loginDAL.GetLogin(person.Username, person.Password);

            if (dataTable.Rows.Count > 0)
            {
                if (person.Password == "newHire")
                {
                    this.success = false;
                    Session["user"] = dataTable.Rows[0]["name"];
                    Session["roleTitle"] = dataTable.Rows[0]["roleTitle"];
                    Session["id"] = dataTable.Rows[0]["id"];
                    FormsAuthentication.SetAuthCookie(person.Username, true);
                    return RedirectToAction("UpdatePassword", new { id = dataTable.Rows[0]["id"] });
                }
                else
                {
                    Session["user"] = dataTable.Rows[0]["name"];
                    Session["roleTitle"] = dataTable.Rows[0]["roleTitle"];
                    Session["id"] = dataTable.Rows[0]["id"];
                    FormsAuthentication.SetAuthCookie(person.Username, true);
                    return View("Index");
                }
            }
            return View();
        }

        /// <summary>
        /// Logs a user out and clears the session and cookie data
        /// </summary>
        /// <returns></returns>
        public ActionResult LogOut()
        {
            Session.Clear();
            Session.Abandon();
            return RedirectToAction("Login");
        }

        /// <summary>
        /// Allows users to update thier password
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult UpdatePassword(int id)
        {
            string whereClause = "";
            Person person = this.personDAL.GetDesiredPersons(whereClause).Where(p => p.Id == id).FirstOrDefault();
            this.success = true;
            return View("UpdatePassword", person);
        }

        /// <summary>
        /// Recieves the new user's password and encrypts/updates it
        /// </summary>
        /// <param name="person"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult UpdatePassword(Person person)
        {
            personDAL.UpdatePassword(person);
            this.success = true;
            return RedirectToAction("Login");
        }

        /// <summary>
        /// Populates the view when the user has forgotten thier password
        /// </summary>
        /// <returns></returns>
        public ActionResult ForgotPassword()
        {
            return View();
        }

        /// <summary>
        /// Resets the user's password
        /// </summary>
        /// <returns></returns>
        public ActionResult ForgotPasswordSubmission()
        {
            try
            {
                int personID = personDAL.GetIDByEmail(Request.Form["confirmEmail"]);
                if (personID == 0)
                {
                    ViewBag.noEmail = "No user with that email.";
                    return View("ForgotPassword");
                }
                Person person = personDAL.GetPersonByID(personID);
                personDAL.UpdatePasswordOnly(person, Request.Form["newPassword"]);
                return RedirectToAction("Login");

            }
            catch
            {
                ViewBag.noEmail = "No user with that email.";
                return View("ForgetPassword");
            }
        }
    }
    #endregion
}