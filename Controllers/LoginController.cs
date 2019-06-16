using ScheduleBuilder.DAL;

using System.Data;

using System.Web.Mvc;

namespace ScheduleBuilder.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }

        public DataTable GetLogin(string username, string password)
        {
            return LoginDAL.GetLogin(username, password);
        }
    }
}