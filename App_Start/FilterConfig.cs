using System.Web;
using System.Web.Mvc;

namespace ScheduleBuilder
{
    public class FilterConfig
    {
        /// <summary>
        /// Checks for the GlobalFilters
        /// </summary>
        /// <param name="filters"></param>
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
