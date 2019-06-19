using ScheduleBuilder.DAL;
using System.Web.Mvc;

namespace ScheduleBuilder.Controllers
{
    public class ShiftController : Controller
    {

        ShiftDAL shiftDAL = new ShiftDAL();

        /// <summary>
        /// gets all shifts from the database
        /// </summary>
        public ActionResult ViewAllShifts()
        {
            ViewBag.Message = "View All Shifts";
            return View();
        }
    }
}
