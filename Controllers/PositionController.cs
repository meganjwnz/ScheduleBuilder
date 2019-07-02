﻿using ScheduleBuilder.DAL;
using ScheduleBuilder.Model;
using System;
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
        private readonly TaskDAL taskDAL = new TaskDAL();

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

        // POST: Task/AddTask
        /// <summary>
        /// Creates a new task
        /// </summary>
        /// <param name="taskTitle"></param>
        /// <param name="taskDescription"></param>
        /// <param name="isActive"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AddTaskShift(string taskTitle, string taskDescription, bool isActive)
        {
                Task task = new Task
                {
                    Task_title = taskTitle,
                    Task_description = taskDescription,
                    IsActive = isActive
                };
                return Json(this.taskDAL.AddShiftTask(task)); 
        }

        // POST: Task/AddTask
        /// <summary>
        /// Creates a new task
        /// </summary>
        /// <param name="taskTitle"></param>
        /// <param name="taskDescription"></param>
        /// <param name="isActive"></param>
        /// <returns></returns>
        [HttpPost]
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

        // POST: Position/Edit/5
        [HttpPost]
        public ActionResult UpdateTaskShift(int id, string taskTitle, string taskDescription, bool isActive)
        {
            try
            {
                Task task = new Task
                {
                    Task_title = taskTitle,
                    Task_description = taskDescription,
                    IsActive = isActive,
                    TaskId = id
                };

                return Json(this.taskDAL.UpdateShiftTask(task));
            }
            catch
            {
                return View();
            }
        }

        #endregion

        #region position_tasks method
        private readonly PositionTaskDAL positionTaskDAL = new PositionTaskDAL();

        public ActionResult GetAllPositionTask()
        {
            try
            {
                return Json(this.positionTaskDAL.GetAllPositionTasks());
            }
            catch (Exception e)
            {
                this.Messagebox(e.ToString());
                return null;
            }
        }

        /// <summary>
        /// Creates a new connection between a position and a task
        /// </summary>
        /// <param name="positionTitle"></param>
        /// <param name="positionDescription"></param>
        /// <param name="isActive"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AddPositionTask(int taskId, int positionId)
        {
            try
            {
                PositionTask positionTask = new PositionTask
                {
                    TaskId = taskId,
                    PositionId = positionId,
                };
                this.positionTaskDAL.CreatePostionTasks(positionTask);
                return RedirectToAction("Positions");
            }
            catch
            {
                return View();
            }
        }


        /// <summary>
        /// Updates the relationship between a position and a task
        /// </summary>
        /// <param name="taskId"></param>
        /// <param name="positionId"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult UpdatePositionTask(int taskId, int positionId)
        {
            try
            {
                PositionTask positionTask = new PositionTask
                {
                    TaskId = taskId,
                    PositionId = positionId,
                };

                this.positionTaskDAL.UpdatePositionTask(positionTask);
                return RedirectToAction("Positions");
            }
            catch
            {
                return View();
            }
        }
        #endregion
    }
}
