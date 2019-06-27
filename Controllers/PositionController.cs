using ScheduleBuilder.DAL;
using ScheduleBuilder.Model;
using System.Linq;
using System.Web.Mvc;

namespace ScheduleBuilder.Controllers
{
    public class PositionController : Controller
    {
        readonly PositionDAL positionDAL = new PositionDAL();

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
            //populates the position fields on updateposition page
            Position position = this.positionDAL.GetAllPositions().Where(p => p.positionID == id).FirstOrDefault();

            return View(position);
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
        /// Currently deactivates any employee
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult DeactivatePosition(int id)
        {
            //populates the position fields on deactivateposition page
            Position position = this.positionDAL.GetAllPositions().Where(p => p.positionID == id).FirstOrDefault();
            this.positionDAL.DeactivatePosition(position);
            return RedirectToAction("Positions");
        }
    }
}
