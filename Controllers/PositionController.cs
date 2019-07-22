using ScheduleBuilder.DAL;
using ScheduleBuilder.Model;
using System;
using System.Web.Mvc;

namespace ScheduleBuilder.Controllers
{
    /// <summary>
    /// Controller responsible for Position and Task model classes and actions
    /// </summary>
    public class PositionController : Controller
    {
        #region position methods
        readonly PositionDAL positionDAL = new PositionDAL();

        /// <summary>
        /// Load the positions/task page and content
        /// </summary>
        /// <returns>The view page</returns>
        public ActionResult Positions()
        {
            return View();
        }

        /// <summary>
        /// Get all positions
        /// Called from app.js
        /// </summary>
        /// <returns>A JSON list of all positions</returns>
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

        /// <summary>
        /// Gets all positions assigned to a specific person
        /// Called from app.js
        /// </summary>
        /// <param name="personID">The person's id as an integer</param>
        /// <returns>A JSON list of positions</returns>
        public ActionResult GetPersonPositions(int personID)
        {
            try
            {
                return Json(this.positionDAL.GetPersonPositions(personID));
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
        /// Called from app.js
        /// </summary>
        /// <param name="positionTitle">The title of the position as a string</param>
        /// <param name="positionDescription">The description of the position as a string</param>
        /// <param name="isActive">Boolean of active or inactive</param>
        /// <returns>A value of true if insert was successful, false otherwise</returns>
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

        /// <summary>
        /// Update a position object
        /// Called from app.js
        /// </summary>
        /// <param name="id">The position's id as an integer</param>
        /// <param name="positionTitle">The title as a string</param>
        /// <param name="positionDescription">The description as a string</param>
        /// <param name="isActive">Active or inactive as a boolean</param>
        /// <returns>A true value if update was successful, false otherwise</returns>
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
        private readonly TaskDAL taskDAL = new TaskDAL();

        /// <summary>
        /// Returns all tasks as a json list
        /// Called from app.js
        /// </summary>
        /// <returns>A JSON list of all tasks</returns>
        public ActionResult GetAllTasks()
        {
            try
            {
                return Json(this.taskDAL.GetAllTasks());
            }
            catch (Exception e)
            {
                this.Messagebox(e.ToString());
                return null;
            }
        }

        /// <summary>
        /// Returns all tasks assigned to a specific position
        /// Called from app.js
        /// </summary>
        /// <param name="positionID">The position's id as an integer</param>
        /// <returns>A JSON list of tasks</returns>
        public ActionResult GetPositionTasks(int positionID)
        {
            try
            {
                return Json(this.taskDAL.GetPositionTasks(positionID));
            }
            catch (Exception e)
            {
                this.Messagebox(e.ToString());
                return null;
            }
        }

        /// <summary>
        /// Creates a new task with an assigned position
        /// Called from app.js
        /// </summary>
        /// <param name="taskTitle">Task title as a string</param>
        /// <param name="taskDescription">Task description as a string</param>
        /// <param name="isActive">Active or Inactive as a boolean</param>
        /// <returns>True if task is added successfully, false otherwise</returns
        public ActionResult AddTaskPosition(string taskTitle, string taskDescription, bool isActive, string positionID)
        {
            Task task = new Task
            {
                Task_title = taskTitle,
                Task_description = taskDescription,
                IsActive = isActive,
                PositionID = int.Parse(positionID)
            };
            return Json(this.taskDAL.AddPositionTask(task));
        }

        /// <summary>
        /// Update a task with an assigned position
        /// Called from app.js
        /// </summary>
        /// <param name="id">The id of the task to be updated</param>
        /// <param name="taskTitle">The title as a string</param>
        /// <param name="taskDescription">The description as a string</param>
        /// <param name="isActive">Active or Inactive as a boolean</param>
        /// <param name="positionID">The associated positionID for the task</param>
        /// <returns></returns>
        public ActionResult UpdateTaskPosition(int id, string taskTitle, string taskDescription, bool isActive, int positionID)
        {
            try
            {
                Task task = new Task
                {
                    Task_title = taskTitle,
                    Task_description = taskDescription,
                    IsActive = isActive,
                    TaskId = id,
                    PositionID = positionID
                };

                return Json(this.taskDAL.UpdatePositionTask(task));
            }
            catch
            {
                return View();
            }
        }

        #endregion
    }
}