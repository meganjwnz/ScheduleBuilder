using ScheduleBuilder.Model;
using ScheduleBuilder.DAL;
using System.Web.Mvc;
using System.Collections.Generic;
using ScheduleBuilder.BusinessLogic;
using System.Data;
using System.Linq;
using ScheduleBuilder.ModelViews;
using System;

namespace ScheduleBuilder.Controllers
{
    /// <summary>
    /// This class insures proper access to the DAL 
    /// and controls all manipulation of the person object(s)
    /// </summary>
    public class PersonController : Controller
    {
        PersonDAL personDAL = new PersonDAL();
        PersonProcessor personProcessor = new PersonProcessor();
        RoleDAL roleDAL = new RoleDAL();
        StatusDAL statusDAL = new StatusDAL();


        #region Add Person
        /// <summary>
        /// Adds a person to the database
        /// </summary>
        /// <param name="addedPerson"></param>
        public ActionResult AddPerson()
        {
            ViewBag.Message = "Add Employee";
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddPerson(PersonViewModel personViewModel)
        {
            if (ModelState.IsValid)
            {
                int numPersonCreated = this.personProcessor.addPerson(personViewModel.LastName
                      , personViewModel.FirstName
                      , personViewModel.DateOfBirth
                      , personViewModel.Ssn
                      , personViewModel.Gender
                      , personViewModel.Phone
                      , personViewModel.StreetAddress
                      , personViewModel.Zipcode
                      , personViewModel.Username
                      , personViewModel.Email);
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        #endregion


        #region Return specified Persons
        /// <summary>
        /// returns a list of all persons - no where clause specified
        /// </summary>
        /// <returns></returns>
        public ActionResult GetAllPeoples()
        {

            string whereClause = "";
            return View(this.personDAL.GetDesiredPersons(whereClause));

        }

        [HttpPost]
        public ActionResult SearchPeople()
        {
            string param = Request.Form["SearchString"];
            List<Person> searchedPeople = new List<Person>();
            if (param != null)
            {
                List<Person> people = StaticPersonDAL.GetDesiredPersons();
                searchedPeople = people.FindAll(x => x.FirstName.IndexOf(param, StringComparison.OrdinalIgnoreCase) >= 0);
                searchedPeople.AddRange(people.FindAll(x => x.LastName.IndexOf(param, StringComparison.OrdinalIgnoreCase) >= 0));
            }
            return View(searchedPeople);
        }

        /// <summary>
        /// Returns list of persons based upon inputed statusid
        /// </summary>
        /// <param name="statusId"></param>
        /// <returns></returns>
        public List<Person> GetAllPeopleByStatusId(int statusId)
        {
            string whereClause = "WHERE statusId = " + statusId.ToString();
            return this.personDAL.GetDesiredPersons(whereClause);
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

        /// <summary>
        /// Returns list of persons based upon inputed RoleId
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public List<Person> GetAllPeopleByRoleId(int roleId)
        {
            string whereClause = "WHERE roleId = " + roleId.ToString();
            return this.personDAL.GetDesiredPersons(whereClause);
        }

        [HttpPost]
        public ActionResult GetAllPeopleById(Person model)
        {
            string whereClause = "WHERE Id = " + model.Id.ToString();
            List<Person> selectedEmployee = this.personDAL.GetDesiredPersons(whereClause);
            return View(selectedEmployee[0]);


        }

        /// <summary>
        /// Returns list of persons based upon inputed first name
        /// </summary>
        /// <param name="FirstName"></param>
        /// <returns></returns>
        public List<Person> GetAllPeopleByFirstName(string FirstName)
        {
            string whereClause = "WHERE first_name = " + FirstName;
            return this.personDAL.GetDesiredPersons(whereClause);
        }

        /// <summary>
        /// Returns list of persons based upon inputed last name
        /// </summary>
        /// <param name="lastName"></param>
        /// <returns></returns>
        public List<Person> GetAllPeopleByLastName(string lastName)
        {
            string whereClause = "WHERE last_name = " + lastName;
            return this.personDAL.GetDesiredPersons(whereClause);
        }

        /// <summary>
        /// Returns list of persons based upon inputed first and last name
        /// </summary>
        /// <param name="FirstName"></param>
        /// <param name="lastName"></param>
        /// <returns></returns>
        public List<Person> GetAllPeopleByFirstAndLastName(string FirstName, string lastName)
        {
            string whereClause = "WHERE last_name = " + lastName + " And first_name = " + FirstName;
            return this.personDAL.GetDesiredPersons(whereClause);
        }
        #endregion

        #region CRUD actions
        public ActionResult Edit(int id)
        {
            string whereClause = "";
            Person person = this.personDAL.GetDesiredPersons(whereClause).Where(p => p.Id == id).FirstOrDefault();
            return View(person);
        }


        [HttpPost]
        public ActionResult Edit(Person person)
        {
            this.personDAL.EditPerson(person);
            return RedirectToAction("GetAllPeoples");
        }

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

        public ActionResult Create()
        {
            return View();
        }


        public ActionResult Delete()
        {
            return View();
        }
        #endregion

        #region person related model methods

        private void SetRole(Person person)
        {
            string roleTitle = this.roleDAL.GetRoleByID(person.RoleId);
            ViewBag.userRoleTitle = roleTitle;
        }

        private void SetStatus(Person person)
        {
            string statusDescription = this.statusDAL.GetStatusByID(person.RoleId).StatusDescription;
            ViewBag.userStatusDescription = statusDescription;
        }

        private void ConvertSSN(Person person)
        {

            string formattedSSN = person.Ssn.Replace(person.Ssn.Substring(0, 3), "###").Substring(0, person.Ssn.Length - 6) + "-" +
                person.Ssn.Replace(person.Ssn.Substring(3, 2), "##").Substring(0, person.Ssn.Length - 4).Substring(3) + "-" +
                person.Ssn.Substring(5);
            ViewBag.userSSn = formattedSSN;
        }

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
