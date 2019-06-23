using ScheduleBuilder.DAL;
using ScheduleBuilder.Model;
using System;
using System.Diagnostics;
using System.Web.Mvc;

namespace ScheduleBuilder.Controllers
{
    public class ShiftController : Controller
    {

        ShiftDAL shiftDAL = new ShiftDAL();
        PositionDAL positionDAL = new PositionDAL();

        /// <summary>
        /// gets all shifts from the database
        /// </summary>
        [HttpPost]
        public ActionResult ViewAllShifts()
        {
            try
            {
                return Json(shiftDAL.GetAllShifts());
            }
            catch (Exception e)
            {
                //what is e?
            }
            return null;

        }

        /// <summary>
        /// gets all positions from the database
        /// </summary>
        [HttpPost]
        public ActionResult ViewAllActivePositions()
        {
            try
            {
                return Json(positionDAL.GetAllActivePositions());
            }
            catch (Exception e)
            {
                
            }
            return null;

        }

        /// <summary>
        /// gets all positions from the database
        /// </summary>
        [HttpPost]
        public ActionResult AddShift(string personID, string positionID, string startdt, string enddt, string startlunchdt, string endlunchdt)
        {
            Shift shift = new Shift();
            shift.personID = int.Parse(personID);
            shift.positionID = int.Parse(positionID);
            shift.scheduledStartTime = ConvertDateToC(long.Parse(startdt));
            shift.scheduledEndTime = ConvertDateToC(long.Parse(enddt));
            if (!string.IsNullOrEmpty(startlunchdt))
            {
                shift.scheduledLunchBreakStart = ConvertDateToC(long.Parse(startlunchdt));
            } else
            {
                shift.scheduledLunchBreakStart = null;
            }
            if (!string.IsNullOrEmpty(endlunchdt))
            {
                shift.scheduledLunchBreakEnd = ConvertDateToC(long.Parse(endlunchdt));
            }
            else
            {
                shift.scheduledLunchBreakEnd = null;
            }

            return Json(shiftDAL.AddShift(shift));
            
            

        }

        private DateTime ConvertDateToC (long jsDate)
        {
            var date = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return date.AddMilliseconds(jsDate - 14400000);
        }
    }
}
