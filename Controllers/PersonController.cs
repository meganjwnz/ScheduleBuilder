using ScheduleBuilder.DAL;
using ScheduleBuilder.Model;
using ScheduleBuilder.ModelViews;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;

namespace ScheduleBuilder.Controllers
{
    /// <summary>
    /// This class insures proper access to the DAL 
    /// and controls all manipulation of the person object(s)
    /// </summary>
    public class PersonController : Controller
    {
        PersonDAL personDAL = new PersonDAL();
        RoleDAL roleDAL = new RoleDAL();
        StatusDAL statusDAL = new StatusDAL();
        PositionDAL positionDAL = new PositionDAL();

        private void InitializeViewBag()
        {
            List<Role> roles = this.roleDAL.GetRoles();
            List<Status> statuses = this.statusDAL.GetStatuses();
            ViewBag.Role = new SelectList(roles, "id", "roleTitle");
            ViewBag.Status = new SelectList(statuses, "id", "StatusTitle");
        }
        #region Add Person
        /// <summary>
        /// Adds a person to the database
        /// </summary>
        /// <param name="addedPerson"></param>
        public ActionResult AddPerson()
        {
            ViewBag.Message = "Add Employee";
            ViewBag.allPositions = this.positionDAL.GetAllActivePositions();
            return View();
        }

        /// <summary>
        /// Catches the newly added person and sends the information to the DAL
        /// </summary>
        /// <param name="personViewModel"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddPerson(PersonViewModel personViewModel)
        {
            ViewBag.allPositions = this.positionDAL.GetAllActivePositions();
            if (Request.Form["assignedPosition"] == "")
            {
                ViewBag.positionNull = "Position required";
                return View();
            }
            else
            {
                if (ModelState.IsValid)
                {
                    this.personDAL.AddPerson(personViewModel.LastName
                            , personViewModel.FirstName
                            , personViewModel.DateOfBirth
                            , personViewModel.Ssn
                            , personViewModel.Gender
                            , personViewModel.Phone
                            , personViewModel.StreetAddress
                            , personViewModel.Zipcode
                            , personViewModel.Email);
                    this.ContactAddedPerson(personViewModel.LastName
                        , personViewModel.FirstName
                        , personViewModel.Email
                        , personViewModel.Gender
                        , personViewModel.StreetAddress
                        , personViewModel.Phone
                        , personViewModel.Zipcode
                        , personViewModel.DateOfBirth
                        , personViewModel.Ssn);
                    this.positionDAL.AddPositionToPerson(this.personDAL.GetIDByEmail(personViewModel.Email), Convert.ToInt32(Request.Form["assignedPosition"]));
                    return RedirectToAction("GetAllPeoples");
                }
            }
            return View();
        }


        #endregion

        #region Search People

        /// <summary>
        /// Returns all persons with a first or last name containing the accepted searchstring
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SearchPeople()
        {
            string param = Request.Form["SearchString"];
            List<Person> searchedPeople = new List<Person>();
            if (param != null)
            {
                List<Person> people = this.personDAL.GetDesiredPersons("");
                searchedPeople = people.FindAll(x => x.FirstName.IndexOf(param, StringComparison.OrdinalIgnoreCase) >= 0);
                searchedPeople.AddRange(people.FindAll(x => x.LastName.IndexOf(param, StringComparison.OrdinalIgnoreCase) >= 0));
            }
            return View(searchedPeople);
        }
        #endregion

        #region Return specified Persons
        /// <summary>
        /// returns a list of all persons - no where clause specified
        /// </summary>
        /// <returns></returns>
        public ActionResult GetAllPeoples()
        {
            return View(this.personDAL.GetDesiredPersons(""));
        }

        //Returns all active employees
        public ActionResult GetAllActivePeoplePage()
        {

            string whereClause = "WHERE statusId = 1 OR statusId = 5";
            return View(this.personDAL.GetDesiredPersons(whereClause));

        }

        //Returns the Admin
        public ActionResult GetAdminPeople()
        {
            string whereClause = "Where roleId = 1";
            return Json(this.personDAL.GetDesiredPersons(whereClause));
        }

        /// <summary>
        /// Return a json list of all active employees that can be scheduled to work.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetAllActivePeople()
        {
            try
            {
                string whereClause = "WHERE statusId = 1 OR statusId = 5";
                return Json(this.personDAL.GetDesiredPersons(whereClause));
            }
            catch (Exception e)
            {
                this.Messagebox(e.ToString());
                return null;
            }

        }

        #endregion

        #region CRUD actions
        /// <summary>
        /// Allows user to edit the person equal to the accepted id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Edit(int id)
        {
            this.InitializeViewBag();
            string whereClause = "";
            Person person = this.personDAL.GetDesiredPersons(whereClause).Where(p => p.Id == id).FirstOrDefault();
            return View(person);
        }


        /// <summary>
        /// Sends the accepted person to the personDAL for update
        /// redirects to the getALLPeoples page
        /// </summary>
        /// <param name="person"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Person person)
        {
            this.InitializeViewBag();
            //try
            //{
                this.personDAL.EditPerson(person);
                this.ContactEditedPerson(person);
                return RedirectToAction("GetAllPeoples");
          //  }
           // catch
            //{
             //   return View(person);
            //}
        }

        #region Contact 
        //This method was created using methods derived from https://www.youtube.com/watch?v=lnRBShlB9hA
        //By Raja Raman of Dot Net
        private void ContactEditedPerson(Person editPerson)
        {
            string loggedInUserId = (Session["id"].ToString());
            Person loggedInUser = this.personDAL.GetDesiredPersons($"Where Id = {loggedInUserId}").FirstOrDefault();
            Email email = new Email(editPerson);

            string subject = "Your personal information has been edited";

            string body = $"Hello { editPerson.GetFullName()}, \n" +
                $"\nYou are recieving this email to let you know that { loggedInUser.GetFullName() } Has altered your personal information\n" +
                $"\n Your new personal details are as follows \n" +
                $"\n Full name:        {editPerson.GetFullName()}" +
                $"\n Gender:           {editPerson.Gender}" +
                $"\n Address:          {editPerson.StreetAddress}" +
                $"\n Zipcode:          {editPerson.Zipcode}" +
                $"\n Date of Birth:    {editPerson.DateOfBirth.Date}" +
                $"\n Phone:            {editPerson.Phone}" +
                $"\n SSn:              {editPerson.Ssn}" +
                $"\n Role:             {this.roleDAL.GetRoleByID(editPerson.RoleId)}" +
                $"\n Status:           {this.statusDAL.GetStatusByID(editPerson.StatusId).StatusDescription}" +
                $"\n Username:         {editPerson.Username}" +
                $"\n If this has been done in error please contact your Admin as soon as possible " +
                $"\n Hope you have a wonderful day";


            email.SendMessage(subject, body);

        }

        private void ContactAddedPerson(string lastname
            , string firstname
            , string emailAddress
            , string gender
            , string address
            , string phone
            , string zipcode
            , DateTime dateOfBirth
            , string ssn)
        {
            string fullname = firstname + " " + lastname;
            string loggedInUserId = (Session["id"].ToString());
            Person loggedInUser = this.personDAL.GetDesiredPersons($"Where Id = {loggedInUserId}").FirstOrDefault();
            Email email = new Email(fullname, emailAddress);

            string subject = "Your personal information has been added";

            string body = $"Hello { firstname}, \n" +
                $"\n Welcome to ScheduleBuilder \n" +
                $"\n In this webased application you will discover that you can see your shifts," +
                $"\n request time off, change your availbility, and even swap shifts \n" +
                $"\n Your personal details are as follows \n" +
                $"\n Full name:        {fullname}" +
                $"\n Gender:           {gender}" +
                $"\n Address:          {address}" +
                $"\n Zipcode:          {zipcode}" +
                $"\n Date of Birth:    {dateOfBirth}" +
                $"\n Phone:            {phone}" +
                $"\n SSn:              {ssn}" +
                $"\n Username:         {firstname.Substring(0, 1).ToLower()}{lastname.ToLower()}" +

                $"\n If you notice any errors please contact your Admin as soon as possible \n" +
                $"\n Your initial password is 'newHire' " +
                $"\n Please log into http://schedulebuildercs6920.azurewebsites.net/Home/Login to update your password" +
                $"\n Hope you have a wonderful day" +
                $"";


            email.SendMessage(subject, body);

        }
        #endregion
        /// <summary>
        /// Gets the details of the accepted person id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Details(int id)
        {
            string whereClause = "";
            Person person = this.personDAL.GetDesiredPersons(whereClause).Where(p => p.Id == id).FirstOrDefault();
            this.SetRole(person);
            this.SetStatus(person);
            this.ConvertSSN(person);
            this.FormatPhone(person);
            return View(person);
        }

        /// <summary>
        /// populates view to allow a new position
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult AddPosition(int id)
        {
            ViewBag.allPositions = this.positionDAL.GetAllActivePositions();
            string whereClause = "";
            Person person = this.personDAL.GetDesiredPersons(whereClause).Where(p => p.Id == id).FirstOrDefault();
            return View(person);

        }
        
        /// <summary>
        /// Catches the infromation about the add positon and sends it to the DAL
        /// </summary>
        /// <param name="person"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AddPosition(Person person)
        {
            this.InitializeViewBag();
            try
            {
                this.positionDAL.AddPositionToPerson(person.Id, Convert.ToInt32(Request.Form["assignedPosition"]));
                //this.ContactEditedPerson(person);
                return RedirectToAction("GetAllPeoples");
            }
            catch
            {
                return View(person);
            }
        }

        /// <summary>
        /// DEACTIVATES - not deletes - the accepted person id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Delete(int id)
        {
            string whereClause = "";
            Person person = this.personDAL.GetDesiredPersons(whereClause).Where(p => p.Id == id).FirstOrDefault();
            return View(person);
        }

        /// <summary>
        /// Marks the accepted person as seperated from company
        /// </summary>
        /// <param name="person"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Delete(Person person)
        {
            person = this.personDAL.SeperateEmployee(person);
            return RedirectToAction("GetAllActivePeoplePage");
        }
        #endregion

        #region person related model methods

        /// <summary>
        /// Sets the person's role
        /// </summary>
        /// <param name="person"></param>
        private void SetRole(Person person)
        {
            string roleTitle = this.roleDAL.GetRoleByID(person.RoleId);
            ViewBag.userRoleTitle = roleTitle;
        }

        /// <summary>
        /// Sets the person's status
        /// </summary>
        /// <param name="person"></param>
        private void SetStatus(Person person)
        {
            string statusDescription = this.statusDAL.GetStatusByID(person.StatusId).StatusDescription;
            if(person.StatusId == 1 || person.StatusId == 5)
            {
                ViewBag.activeStatus = "Active";
            }
            else
            {
                ViewBag.activeStatus = "Inactive";
            }
            ViewBag.userStatusDescription = statusDescription;
        }

        /// <summary>
        /// Converts the SSN to a view which hids the majority of the characters
        /// </summary>
        /// <param name="person"></param>
        private void ConvertSSN(Person person)
        {
            string formattedSSN = "";
            if (person.Ssn.Length == 9)
            {
                formattedSSN = person.Ssn.Replace(person.Ssn.Substring(0, 3), "###").Substring(0, person.Ssn.Length - 6) + "-" +
                   person.Ssn.Replace(person.Ssn.Substring(3, 2), "##").Substring(0, person.Ssn.Length - 4).Substring(3) + "-" +
                    person.Ssn.Substring(5);

            }

            ViewBag.userSSn = formattedSSN;
        }

        /// <summary>
        /// formats the phone to be readable
        /// </summary>
        /// <param name="person"></param>
        private void FormatPhone(Person person)
        {
            string formattedPhone = "(" + person.Phone.Substring(0, 3) + ") " + person.Phone.Substring(3, 3) + "-" + person.Phone.Substring(6);
            ViewBag.userPhone = formattedPhone;
        }

        #endregion


        public void Messagebox(string xMessage)
        {
            Response.Write("<script>alert('" + xMessage + "')</script>");
        }

    }

}
