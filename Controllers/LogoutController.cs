using System.Web.Mvc;
using System.Web.Security;

namespace ScheduleBuilder.Controllers
{
    public class LogoutController : Controller
    {
        // GET: Logout
        public ActionResult Index()
        {
            FormsAuthentication.SignOut();
            return View("Index", "Home");
        }
    }
}