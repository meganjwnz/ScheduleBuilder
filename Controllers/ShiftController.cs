using ScheduleBuilder.DAL;
using System;
using System.Web.Mvc;

namespace ScheduleBuilder.Controllers
{
    public class ShiftController : Controller
    {

        ShiftDAL shiftDAL = new ShiftDAL();

        /// <summary>
        /// gets all shifts from the database
        /// </summary>
        [HttpPost]
        public ActionResult ViewAllShifts()
        {
            try
            {
                return Json(shiftDAL.GetAllShifts());
            }catch(Exception e)
            {
                //what is e?
            }
            return null;

        }
    }
}
