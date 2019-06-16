using ScheduleBuilder.DAL;
using System.Data;
using System.Web.Mvc;

namespace ScheduleBuilder.Controllers
{
    public class LoginController : Controller
    {
        /// <summary>
        ///  Retrieves login information in the form of a data table   
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public DataTable GetLogin(string username, string password)
        {
            return LoginDAL.GetLogin(username, password);
        }
    }
}
