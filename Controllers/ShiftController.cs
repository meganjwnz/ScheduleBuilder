using ScheduleBuilder.DAL;
using ScheduleBuilder.Model;
using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace ScheduleBuilder.Controllers
{
    public class ShiftController : Controller
    {

        ShiftDAL shiftDAL = new ShiftDAL();
        PositionDAL positionDAL = new PositionDAL();
        PersonDAL personDAL = new PersonDAL();
        RoleDAL roleDAL = new RoleDAL();
        StatusDAL statusDAL = new StatusDAL();

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

                this.Messagebox(e.ToString());
                return null;
            }
        }

        #region TimeCard

        /// <summary>
        /// Adds Timecard page
        /// </summary>
        /// <returns></returns>
        public ActionResult AddTimePunchPage()
        {
            string loggedInUserId = (Session["id"].ToString());
            string whereClause = " WHERE s.personId = " + loggedInUserId;
            if (shiftDAL.GetNearestShift(whereClause).scheduledStartTime == DateTime.MinValue)
            {
                TempData["notice"] = "You have no scheduled shifts\n\n See Mangement";
                return RedirectToAction("Index", "Home");
            }
            if (!(shiftDAL.GetNearestShift(whereClause).scheduledStartTime.ToUniversalTime() >= DateTime.Now.AddDays(-1).ToUniversalTime() && shiftDAL.GetNearestShift(whereClause).scheduledStartTime < DateTime.Now.AddDays(1).ToUniversalTime()))
            {
                TempData["notice"] = "You have no shifts scheduled today\n\n See Mangement";
                return RedirectToAction("Index", "Home");
            }
            //If Users clock in under 4 hours late
            else if (shiftDAL.GetNearestShift(whereClause).scheduledStartTime.AddHours(4).ToUniversalTime() < DateTime.Now.ToUniversalTime())
            {
                TempData["notice"] = "You are too late to clock in\n See Mangement";
                return RedirectToAction("Index", "Home");

            }
            else if ((shiftDAL.GetNearestShift(whereClause).scheduledStartTime.AddHours(-4).ToUniversalTime() > DateTime.Now.ToUniversalTime()))
            {
                TempData["notice"] = "You are too early to clock in\n See Mangement";
                return RedirectToAction("Index", "Home");
            }
            Shift whatever = shiftDAL.GetNearestShift(whereClause);
            return View(this.timeHack(whatever));
        }

        /// <summary>
        /// Clocks user in retruns them to the time card page
        /// </summary>
        /// <returns></returns>
        public ActionResult ClockUserIn()
        {
            //This needs to be cleaned up THREE LINES TO GET one ID not cool
            string loggedInUserId = (Session["id"].ToString());
            string whereClause = "WHERE p.id = " + loggedInUserId;
            this.shiftDAL.ClockUserIn(shiftDAL.GetNearestShift(whereClause).scheduleShiftID, DateTime.Now.ToUniversalTime());
            return Redirect(Request.UrlReferrer.ToString());
        }

        /// <summary>
        /// Clocks the user out returns them to the time card page
        /// </summary>
        /// <returns></returns>
        public ActionResult ClockUserOut()
        {
            //This needs to be cleaned up THREE LINES TO GET one ID not cool
            string loggedInUserId = (Session["id"].ToString());
            string whereClause = "WHERE p.id = " + loggedInUserId;
            this.shiftDAL.ClockUserOut(shiftDAL.GetNearestShift(whereClause).scheduleShiftID, DateTime.Now.ToUniversalTime());
            return Redirect(Request.UrlReferrer.ToString());
        }

        //Starts the users lunch break
        public ActionResult ClockLunchStart()
        {
            string loggedInUserId = (Session["id"].ToString());
            string whereClause = "WHERE p.id = " + loggedInUserId;
            this.shiftDAL.ClockLunchStart(shiftDAL.GetNearestShift(whereClause).scheduleShiftID, DateTime.Now.ToUniversalTime());
            return Redirect(Request.UrlReferrer.ToString());
        }

        //ends user's lunch break
        public ActionResult ClockLunchEnd()
        {
            string loggedInUserId = (Session["id"].ToString());
            string whereClause = "WHERE p.id = " + loggedInUserId;
            this.shiftDAL.ClockLunchEnd(shiftDAL.GetNearestShift(whereClause).scheduleShiftID, DateTime.Now.ToUniversalTime());
            return Redirect(Request.UrlReferrer.ToString());
        }
        #endregion
        /// <summary>
        /// Returns accepted SQL errors 
        /// </summary>
        /// <param name="xMessage"></param>
        public void Messagebox(string xMessage)
        {
            Response.Write("<script>alert('" + xMessage + "')</script>");
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
                this.Messagebox(e.ToString());
                return null;
            }
        }

        /// <summary>
        /// gets all positions from the database
        /// </summary>
        [HttpPost]
        public ActionResult AddShift(string personID, string positionID, string startdt, string enddt, string startlunchdt, string endlunchdt, string taskList)
        {
            JavaScriptSerializer thing = new JavaScriptSerializer();
            dynamic otherThing = thing.Deserialize<object>(taskList);
            Shift shift = new Shift();
            shift.personID = int.Parse(personID);
            shift.positionID = int.Parse(positionID);
            shift.scheduledStartTime = ConvertDateToC(long.Parse(startdt));
            shift.scheduledEndTime = ConvertDateToC(long.Parse(enddt));
            if (!string.IsNullOrEmpty(startlunchdt))
            {
                shift.scheduledLunchBreakStart = ConvertDateToC(long.Parse(startlunchdt));
            }
            else
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

        /// <summary>
        /// gets all positions from the database
        /// </summary>
        [HttpPost]
        public ActionResult UpdateShift(string personID, string positionID, string startdt, string enddt, string startlunchdt, string endlunchdt, string shiftID, string scheduleshiftID, string isDelete)
        {
            //Create shift object from values passed from view
            Shift shift = new Shift();
            shift.shiftID = int.Parse(shiftID);
            shift.scheduleShiftID = int.Parse(scheduleshiftID);
            shift.personID = int.Parse(personID);
            shift.positionID = int.Parse(positionID);
            shift.scheduledStartTime = ConvertDateToC(long.Parse(startdt));
            shift.scheduledEndTime = ConvertDateToC(long.Parse(enddt));
            if (!string.IsNullOrEmpty(startlunchdt))
            {
                shift.scheduledLunchBreakStart = ConvertDateToC(long.Parse(startlunchdt));
            }
            else
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

            //Delete or update shift accordingly
            if (string.Equals(isDelete, "delete"))
            {
                this.ContactPersonShiftChange("delete", shift);
                return Json(shiftDAL.DeleteShift(shift));
            }
            else
            {
                this.ContactPersonShiftChange("update", shift);
                return Json(shiftDAL.UpdateShift(shift));

            }

        }
        private void ContactPersonShiftChange(string type, Shift shift)
        {
            string loggedInUserId = (Session["id"].ToString());
            Person loggedInUser = this.personDAL.GetDesiredPersons($"Where Id = {loggedInUserId}").FirstOrDefault();
            Person shiftChangePerson = this.personDAL.GetDesiredPersons($"Where Id = {shift.personID}").FirstOrDefault();
            Email email = new Email(shiftChangePerson);

            string subject = $"Your Shift has been {type}d";

            string body = $"Hello { shiftChangePerson.GetFullName()}, \n" +
                $"\nYou are recieving this email to let you know that { loggedInUser.GetFullName() } Has {type}d your shift on {shift.scheduledStartTime} \n" +
                $"\n Please log in to your account to see all schedule changes\n" +

                $"\n If this has been done in error please contact your Admin as soon as possible " +
                $"\n Hope you have a wonderful day";


            email.SendMessage(subject, body);

        }

        private DateTime ConvertDateToC(long jsDate)
        {
            var date = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            return date.AddMilliseconds(jsDate);
        }

        private Shift timeHack(Shift shift)
        {
            shift.scheduledStartTime = shift.scheduledStartTime.AddHours(-4);
            shift.scheduledEndTime = shift.scheduledEndTime.AddHours(-4);
            if (shift.scheduledLunchBreakStart != null)
            {
                DateTime temp = shift.scheduledLunchBreakStart ?? DateTime.MinValue; shift.scheduledLunchBreakStart = temp.AddHours(-4);
            }
            if (shift.scheduledLunchBreakEnd != null)
            {
                DateTime temp = shift.scheduledLunchBreakEnd ?? DateTime.MinValue;
                shift.scheduledLunchBreakEnd = temp.AddHours(-4);
            }
            if (shift.actualStartTime != null)
            {
                DateTime temp = shift.actualStartTime ?? DateTime.MinValue;
                shift.actualStartTime = temp.AddHours(-4);
            }
            if (shift.actualEndTime != null)
            {
                DateTime temp = shift.actualEndTime ?? DateTime.MinValue;
                shift.actualEndTime = temp.AddHours(-4);
            }
            if (shift.actualLunchBreakStart != null) {
                DateTime temp = shift.actualLunchBreakStart ?? DateTime.MinValue;
                shift.actualLunchBreakStart = temp.AddHours(-4); }
            if (shift.actualLunchBreakEnd != null) {
                DateTime temp = shift.actualLunchBreakEnd ?? DateTime.MinValue;
                shift.actualLunchBreakEnd = temp.AddHours(-4);
            }
            return shift;
        }

    }
}
