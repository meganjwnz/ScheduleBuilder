﻿using ScheduleBuilder.Model;
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

        #region Search People
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

        #endregion

        #region CRUD actions
        public ActionResult Edit(int id)
        {
            string whereClause = "";
            Person person = this.personDAL.GetDesiredPersons(whereClause).Where(p => p.Id == id).FirstOrDefault();
            return View(person);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Person person)
        {
            try
            {
                this.personDAL.EditPerson(person);
                return RedirectToAction("GetAllPeoples");
            }
            catch
            {
                return View(person);
            }
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

        public ActionResult Delete(int id)
        {
           
            string whereClause = "";
            Person person = this.personDAL.GetDesiredPersons(whereClause).Where(p => p.Id == id).FirstOrDefault();
            return View(person);
        }

        [HttpPost]
        public ActionResult Delete(Person person)
        {
            person = this.personDAL.SeperateEmployee(person);
            return RedirectToAction("GetAllActivePeoplePage");
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
            string statusDescription = this.statusDAL.GetStatusByID(person.StatusId).StatusDescription;
            ViewBag.userStatusDescription = statusDescription;
        }

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
