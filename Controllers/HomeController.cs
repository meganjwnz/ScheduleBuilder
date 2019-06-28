using ScheduleBuilder.DAL;
using ScheduleBuilder.Model;
using System.Collections.Generic;
using System.Data;
using System.Web.Mvc;
using System.Web.Security;
using MailKit;
using MailKit.Net.Smtp;
using MimeKit;
using System.Linq;

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
            //Email steps
            //Instantiate message
            var message = new MimeMessage();
            //From
            message.From.Add(new MailboxAddress("Drew", "ScheduleBuilder2019@gmail.com"));
            //To
            message.To.Add(new MailboxAddress("Self", "dkcoleman12@gmail.com"));
            //Subject
            message.Subject = " Look and Email it is exciting";
            // Body
            message.Body = new TextPart("plain")
            {
                Text = "Testing this will suck"
            };

            // Configure and send mail
            using (var client = new SmtpClient())
            {
                client.Connect("smtp.gmail.com", 587, false);

                client.Authenticate("ScheduleBuilder2019@gmail.com", "!Yoder19");

                client.Send(message);
                client.Disconnect(true);
                // client.ServerCertificateValidationCallback = (s,char,)
            }
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

        #region Login/Logout & New User functionality
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
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }

        public ActionResult UpdatePassword(int id)
        {
            string whereClause = "";
            Person person = this.personDAL.GetDesiredPersons(whereClause).Where(p => p.Id == id).FirstOrDefault();

            return View("UpdatePassword", person);
        }

        [HttpPost]
        public ActionResult UpdatePassword(Person person)
        {
            personDAL.UpdatePassword(person);
            ViewBag.loginAgain = "You've successfully changed your password! Please login again.";

            return View("Login");
        }
    }
    #endregion
}