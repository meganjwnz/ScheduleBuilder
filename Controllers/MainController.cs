using System.Collections.Generic;
using System.Web.Mvc;
using ScheduleManager.Model;


namespace ScheduleManager.Controllers
{
    public class MainController : Controller
    {
        private DAL.ShiftDAL dal;
        public MainController()
        {
            dal = new DAL.ShiftDAL();
        }
        public ActionResult Index()
        {
            return View();
        }

        [Route("Main/Test")]
        public ActionResult Test()
        {
            List<Shift> shifts = dal.GetShifts();
            return Json(shifts);
        }
    }
}