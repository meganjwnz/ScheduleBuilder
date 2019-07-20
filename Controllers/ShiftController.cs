using Newtonsoft.Json;
using ScheduleBuilder.DAL;
using ScheduleBuilder.Model;
using ScheduleBuilder.ModelViews;
using System;
using System.Collections.Generic;
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
            Shift nearestShift = shiftDAL.GetNearestShift(whereClause);
            return View(this.TimeUpdate(nearestShift));
        }

        public bool ValidPunch(string whereClause)
        {
            bool validShift = true;

            if (shiftDAL.GetNearestShift(whereClause).scheduledStartTime == DateTime.MinValue)
            {
                validShift = false;
                TempData["notice"] = "You have no scheduled shifts\n\n See Mangement";
            }
            if (!(shiftDAL.GetNearestShift(whereClause).scheduledStartTime.ToUniversalTime() >= DateTime.Now.AddDays(-1).ToUniversalTime() && shiftDAL.GetNearestShift(whereClause).scheduledStartTime < DateTime.Now.AddDays(1).ToUniversalTime()))
            {
                validShift = false;
                TempData["notice"] = "You have no shifts scheduled today\n\n See Mangement";
            }
            //If Users clock in under 4 hours late
            else if (shiftDAL.GetNearestShift(whereClause).scheduledStartTime.AddHours(4).ToUniversalTime() < DateTime.Now.ToUniversalTime())
            {
                validShift = false;
                TempData["notice"] = "You are too late to clock in\n See Mangement";
            }
            else if ((shiftDAL.GetNearestShift(whereClause).scheduledStartTime.AddHours(-4).ToUniversalTime() > DateTime.Now.ToUniversalTime()))
            {
                validShift = false;
                TempData["notice"] = "You are too early to clock in\n See Mangement";
            }
            return validShift;
        }

        public ActionResult RefreshPageDisplayError()
        {
            return RedirectToAction("AddTimePunchPage", "Shift");
        }
        /// <summary>
        /// Clocks user in retruns them to the time card page
        /// </summary>
        /// <returns></returns>
        public ActionResult ClockUserIn()
        {

            string loggedInUserId = (Session["id"].ToString());
            string whereClause = "WHERE p.id = " + loggedInUserId;
            if (this.ValidPunch(whereClause))
            {
                this.shiftDAL.ClockUserIn(shiftDAL.GetNearestShift(whereClause).scheduleShiftID, DateTime.Now.ToUniversalTime());
                return Redirect(Request.UrlReferrer.ToString());

            }
            else
            {
                return this.RefreshPageDisplayError();
            }
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

        #region TimeCardEdit

        public ActionResult GetLastTwoWeeksOfShiftsForEdit(int personid)
        {
            string whereClause = $"Where p.id =  {personid}  and sh.scheduledStartTime > (SELECT DATEADD(day,- 14, GETDATE()))";
            List<Shift> shifts = this.shiftDAL.GetAllShifts(whereClause);
            List<TimeCardEditViewModel> timeCardEdits = this.CovertShiftToTimeCardView(shifts);


            return View(timeCardEdits);
        }

        private List<TimeCardEditViewModel> CovertShiftToTimeCardView(List<Shift> shifts)
        {
            List<TimeCardEditViewModel> timeCardEdits = new List<TimeCardEditViewModel>();
            foreach (Shift shift in shifts)
            {
                TimeCardEditViewModel timeCard = new TimeCardEditViewModel();
                timeCard.personFirstName = shift.personFirstName;
                timeCard.personLastName = shift.personLastName;
                timeCard.scheduledStartTime = shift.scheduledStartTime;
                timeCard.scheduledEndTime = shift.scheduledEndTime;
                timeCard.scheduledLunchBreakStart = shift.scheduledLunchBreakStart;
                timeCard.scheduledLunchBreakEnd = shift.scheduledLunchBreakEnd;
                timeCard.actualStartTime = shift.actualStartTime;
                timeCard.actualEndTime = shift.actualEndTime;
                timeCard.actualLunchBreakStart = shift.actualLunchBreakStart;
                timeCard.actualLunchBreakEnd = shift.actualLunchBreakEnd;
                timeCardEdits.Add(timeCard);
            }

            return timeCardEdits;
        }

        #endregion
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

        [HttpPost]
        public ActionResult AddShift(string personID, string positionID, string startdt, string enddt, string startlunchdt, string endlunchdt, string taskList, string notes)
        {

            JavaScriptSerializer thing = new JavaScriptSerializer();
            Dictionary<int, bool> otherThing = taskList == null ? new Dictionary<int, bool>() : JsonConvert.DeserializeObject<Dictionary<int, bool>>(taskList);
            Shift shift = new Shift();
            shift.personID = int.Parse(personID);
            shift.positionID = int.Parse(positionID);
            shift.scheduledStartTime = ConvertDateToC(long.Parse(startdt));
            shift.scheduledEndTime = ConvertDateToC(long.Parse(enddt));

            bool checkIfAlreadyScheduled = this.shiftDAL.CheckIfPersonIsScheduled(shift.personID, shift.scheduledStartTime, shift.scheduledEndTime);
            if (checkIfAlreadyScheduled == true)
            {
                return View();
            }
            else
            {
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
                shift.Notes = notes;

                return Json(shiftDAL.AddShift(shift, otherThing));
            }
        }

        /// <summary>
        /// gets all positions from the database
        /// allows updating shifts
        /// </summary>
        [HttpPost]
        public ActionResult UpdateShift(string personID, string positionID, string startdt, string enddt, string startlunchdt, string endlunchdt, string shiftID, string scheduleshiftID, string isDelete, string taskList, string notes)
        {
            JavaScriptSerializer thing = new JavaScriptSerializer();
            Dictionary<int, bool> otherThing = taskList == null ? new Dictionary<int, bool>() : JsonConvert.DeserializeObject<Dictionary<int, bool>>(taskList);
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
            shift.Notes = notes;

            //Delete or update shift accordingly
            if (string.Equals(isDelete, "delete"))
            {
                this.ContactPersonShiftChange("delete", shift);
                return Json(shiftDAL.DeleteShift(shift));
            }
            else
            {
                this.ContactPersonShiftChange("update", shift);
                return Json(shiftDAL.UpdateShift(shift, otherThing));

            }

        }

        public ActionResult RequestTimeOff()
        {
            ViewBag.failedRequest = "";
            ViewBag.successfulRequest = "";
            return View();
        }

        [HttpPost]
        public ActionResult RequestTimeOffFunction(string personID, string positionID, string startDate, string endDate, string taskList, string notes)
        {
            startDate = Request.Form["startDate"];
            endDate = Request.Form["endDate"];


            Shift shift = new Shift();
            shift.personID = int.Parse(Session["id"].ToString());
            shift.positionID = this.positionDAL.FindPositionIDByUnavailable();
            shift.scheduledStartTime = DateTime.Parse(startDate);
            shift.scheduledEndTime = DateTime.Parse(endDate);
            Dictionary<int, bool> otherThing = taskList == null ? new Dictionary<int, bool>() : JsonConvert.DeserializeObject<Dictionary<int, bool>>(taskList);
            bool checkIfAlreadyScheduled = this.shiftDAL.CheckIfPersonIsScheduled(shift.personID, shift.scheduledStartTime, shift.scheduledEndTime);
            if (checkIfAlreadyScheduled == true)
            {
                ViewBag.failedRequest = "You are already scheduled between " + startDate + " and " +
                    endDate + " or have requested this time off already. Please check your schedule.";
                return View("RequestTimeOff");
            }
            else
            {
                ViewBag.successfulRequest = "Your request beginning " + startDate + " and ending " + endDate + " has been submitted.";
                this.shiftDAL.AddShift(shift, otherThing);
                return View("RequestTimeOff");
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

        private Shift TimeUpdate(Shift shift)
        {
            int timezone = -4;
            shift.scheduledStartTime = shift.scheduledStartTime.AddHours(timezone);
            shift.scheduledEndTime = shift.scheduledEndTime.AddHours(timezone);
            if (shift.scheduledLunchBreakStart != null)
            {
                DateTime temp = shift.scheduledLunchBreakStart ?? DateTime.MinValue; shift.scheduledLunchBreakStart = temp.AddHours(timezone);
            }
            if (shift.scheduledLunchBreakEnd != null)
            {
                DateTime temp = shift.scheduledLunchBreakEnd ?? DateTime.MinValue;
                shift.scheduledLunchBreakEnd = temp.AddHours(timezone);
            }
            if (shift.actualStartTime != null)
            {
                DateTime temp = shift.actualStartTime ?? DateTime.MinValue;
                shift.actualStartTime = temp.AddHours(timezone);
            }
            if (shift.actualEndTime != null)
            {
                DateTime temp = shift.actualEndTime ?? DateTime.MinValue;
                shift.actualEndTime = temp.AddHours(timezone);
            }
            if (shift.actualLunchBreakStart != null)
            {
                DateTime temp = shift.actualLunchBreakStart ?? DateTime.MinValue;
                shift.actualLunchBreakStart = temp.AddHours(timezone);
            }
            if (shift.actualLunchBreakEnd != null)
            {
                DateTime temp = shift.actualLunchBreakEnd ?? DateTime.MinValue;
                shift.actualLunchBreakEnd = temp.AddHours(timezone);
            }
            return shift;
        }


    }
}
