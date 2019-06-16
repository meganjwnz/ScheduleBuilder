using ScheduleBuilder.DAL;
using System.Data;

namespace ScheduleBuilder.Controllers
{
    public class LoginController
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
