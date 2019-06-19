using System.Collections.Generic;
using System.Web.Mvc;
using ScheduleBuilder.Model;


namespace ScheduleBuilder.Controllers
{
    public class MainController : Controller
    {
        private DAL.ShiftDAL dal;
        public MainController()
        {
            ViewBag.User = Session["user"];
            dal = new DAL.ShiftDAL();
        }
        public ActionResult Index()
        {
            return View();
        }
    }
}