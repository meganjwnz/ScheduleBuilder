using ScheduleBuilder.DAL;
using ScheduleBuilder.Model;
using System;
using System.Linq;
using System.Web.Mvc;

namespace ScheduleBuilder.Controllers
{
    public class ShiftController : Controller
    {

        ShiftDAL shiftDAL = new ShiftDAL();
        PositionDAL positionDAL = new PositionDAL();
        PersonDAL personDAL = new PersonDAL();
        RoleDAL roleDAL = new RoleDAL();
        StatusDAL statusDAL = new StatusDAL();
        private int shiftHoursId;

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

        public ActionResult AddTimePunchPage()
        {
            string loggedInUserId = (Session["id"].ToString());
            string whereClause = "WHERE p.id = " + loggedInUserId;
            //this.shiftHoursId = shiftDAL.GetAllShifts(whereClause)[0].scheduleShiftID;
            //int herefortesting = this.shiftHoursId;
            return View(shiftDAL.GetAllShifts(whereClause)[0]);
        }

        public ActionResult ClockUserIn()
        {
            //This needs to be cleaned up THREE LINES TO GET one ID not cool
            string loggedInUserId = (Session["id"].ToString());
            string whereClause = "WHERE p.id = " + loggedInUserId;
            this.shiftDAL.ClockUserIn(shiftDAL.GetAllShifts(whereClause)[0].scheduleShiftID, DateTime.Now);
            return Redirect(Request.UrlReferrer.ToString()); 
        }

        public ActionResult ClockUserOut()
        {
            //This needs to be cleaned up THREE LINES TO GET one ID not cool
            string loggedInUserId = (Session["id"].ToString());
            string whereClause = "WHERE p.id = " + loggedInUserId;
            this.shiftDAL.ClockUserOut(shiftDAL.GetAllShifts(whereClause)[0].scheduleShiftID, DateTime.Now); 
            return Redirect(Request.UrlReferrer.ToString());
        }

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
            var date = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return date.AddMilliseconds(jsDate);
        }
    }
}
