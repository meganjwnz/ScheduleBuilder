
using ScheduleBuilder.DAL;
using System.Web.Mvc;

namespace ScheduleBuilder.Controllers
{
    public class PositionController : Controller
    {
        PositionDAL positionDAL = new PositionDAL();

        // GET: Position
        public ActionResult Positions()
        {
            return View(this.positionDAL.GetAllActivePositions());
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

        // POST: Position/Create
        [HttpPost]
        public ActionResult AddPosition(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

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
