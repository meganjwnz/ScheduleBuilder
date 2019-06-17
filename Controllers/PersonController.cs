﻿using ScheduleManager.Model;
using ScheduleManager.DAL;
using System.Web.Mvc;
using System.Collections.Generic;
using ScheduleBuilder.DAL;
using System.Data;
using ScheduleBuilder.Controllers;

namespace ScheduleManager.Controllers
{
    /// <summary>
    /// This class insures proper access to the DAL 
    /// and controls all manipulation of the person object(s)
    /// </summary>
    public class PersonController : Controller
    {
        

        /// <summary>
        /// Adds a person to the database
        /// </summary>
        /// <param name="addedPerson"></param>
        public static void AddPerson(Person addedPerson)
        {
            PersonDAL.AddPerson(addedPerson);
        }

        #region Return specified Persons
        /// <summary>
        /// returns a list of all persons - no where clause specified
        /// </summary>
        /// <returns></returns>
        //public static List<Person> GetAllPeoples()
        //{

        //    string whereClause = "";
        //    return PersonDAL.GetDesiredPersons(whereClause);

        //}

        public ActionResult GetAllPeoples()
        {
            string whereClause = "";
            return View(PersonDAL.GetDesiredPersons(whereClause));
        }

        public ActionResult Edit()
        {
            return View();
        }

        public ActionResult Create()
        {
            return View();
        }

        public ActionResult Details()
        {
            return View();
        }

        public ActionResult Delete()
        {
            return View();
        }

        /// <summary>
        /// Returns list of persons based upon inputed statusid
        /// </summary>
        /// <param name="statusId"></param>
        /// <returns></returns>
        public static List<Person> GetAllPeopleByStatusId(int statusId)
        {
            string whereClause = "WHERE statusId = " + statusId.ToString();
            return PersonDAL.GetDesiredPersons(whereClause);
        }

        /// <summary>
        /// Returns list of persons based upon inputed RoleId
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public static List<Person> GetAllPeopleByRoleId(int roleId)
        {
            string whereClause = "WHERE roleId = " + roleId.ToString();
            return PersonDAL.GetDesiredPersons(whereClause);
        }

        public ActionResult GetAllPeopleById()
        {
            return View(); 
        }

        [HttpPost]
        public ActionResult GetAllPeopleById(Person model)
        {
            string whereClause = "WHERE Id = " + model.Id.ToString();
            List<Person> selectedEmployee = PersonDAL.GetDesiredPersons(whereClause);
            return View(selectedEmployee[0]);


        }

        /// <summary>
        /// Returns list of persons based upon inputed first name
        /// </summary>
        /// <param name="FirstName"></param>
        /// <returns></returns>
        public static List<Person> GetAllPeopleByFirstName(string FirstName)
        {
            string whereClause = "WHERE first_name = " + FirstName;
            return PersonDAL.GetDesiredPersons(whereClause);
        }

        /// <summary>
        /// Returns list of persons based upon inputed last name
        /// </summary>
        /// <param name="lastName"></param>
        /// <returns></returns>
        public static List<Person> GetAllPeopleByLastName(string lastName)
        {
            string whereClause = "WHERE last_name = " + lastName;
            return PersonDAL.GetDesiredPersons(whereClause);
        }

        /// <summary>
        /// Returns list of persons based upon inputed first and last name
        /// </summary>
        /// <param name="FirstName"></param>
        /// <param name="lastName"></param>
        /// <returns></returns>
        public static List<Person> GetAllPeopleByFirstAndLastName(string FirstName, string lastName)
        {
            string whereClause = "WHERE last_name = " + lastName + " And first_name = " + FirstName;
            return PersonDAL.GetDesiredPersons(whereClause);
        }
        #endregion
    }
}
