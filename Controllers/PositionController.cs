﻿using ScheduleBuilder.DAL;
using ScheduleBuilder.Model;
using System.Web.Mvc;

namespace ScheduleBuilder.Controllers
{
    public class PositionController : Controller
    {
        PositionDAL positionDAL = new PositionDAL();

        // GET: Position
        public ActionResult Positions()
        {
            return View(this.positionDAL.GetAllPositions());
        }

        // GET: Position/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Position/Create
        public ActionResult AddPosition()
        {
            return View();
        }

        // POST: Position/AddPosition
        /// <summary>
        /// Creates a new position
        /// </summary>
        /// <param name="positionTitle"></param>
        /// <param name="positionDescription"></param>
        /// <param name="isActive"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AddPosition(string positionTitle, string positionDescription, bool isActive)
        {
            try
            {
                Position position = new Position
                {
                    positionTitle = positionTitle,
                    positionDescription = positionDescription,
                    isActive = isActive
                };
                this.positionDAL.AddPosition(position);
                return RedirectToAction("Positions");
            }
            catch
            {
                return View();
            }
        }

        // GET: Position/Edit/5
        public ActionResult UpdatePosition(int id)
        {
            return View();
        }

        // POST: Position/Edit/5
        [HttpPost]
        public ActionResult UpdatePosition(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Positions");
            }
            catch
            {
                return View();
            }
        }

        // GET: Position/Delete/5
        public ActionResult DeactivatePosition(int id)
        {
            return View();
        }

        // POST: Position/Delete/5
        [HttpPost]
        public ActionResult DeactivatePosition(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Positions");
            }
            catch
            {
                return View();
            }
        }
    }
}
