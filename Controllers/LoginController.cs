using ScheduleManager.DAL;
using System.Data;

namespace ScheduleManager.Controllers
{
    public class LoginController
    {
        private readonly LoginDAL loginDAL;

        /// <summary>
        ///  Retrieves login information in the form of a data table   
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public DataTable GetLogin(string username, string password)
        {
            return loginDAL.GetLogin(username, password);
        }
    }
}
