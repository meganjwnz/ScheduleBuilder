using ScheduleBuilder.DAL;
using ScheduleBuilder.Model;
using System;
using System.Linq;
using System.Web.Mvc;

namespace ScheduleBuilder.Controllers
{
    public class PositionController : Controller
    {
        #region position methods
        readonly PositionDAL positionDAL = new PositionDAL();

        // GET: Position
        public ActionResult Positions()
        {
            return View(this.taskDAL.GetAllTasks());
        }

        // GET: Position
        public ActionResult GetAllPositions()
        {
            try
            {
                return Json(this.positionDAL.GetAllPositions());
            }
            catch (Exception e)
            {
                this.Messagebox(e.ToString());
                return null;
            }
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
        
        // POST: Position/Edit/5
        [HttpPost]
        public ActionResult UpdatePosition(int id, string positionTitle, string positionDescription, bool isActive)
        {
            try
            {
                Position position = new Position
                {
                    positionTitle = positionTitle,
                    positionDescription = positionDescription,
                    isActive = isActive,
                    positionID = id
                };

                this.positionDAL.UpdatePosition(position);
                return RedirectToAction("Positions");
            }
            catch
            {
                return View();
            }
        }

        /// <summary>
        /// Returns accepted SQL errors 
        /// </summary>
        /// <param name="xMessage"></param>
        public void Messagebox(string xMessage)
        {
            Response.Write("<script>alert('" + xMessage + "')</script>");
        }

        #endregion

        #region task methods
        private TaskDAL taskDAL = new TaskDAL();

        public ActionResult Tasks()
        {
            return View(this.taskDAL.GetAllTasks());
        }

        #endregion
    }
}
