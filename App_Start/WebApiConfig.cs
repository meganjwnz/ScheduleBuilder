using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace ScheduleBuilder
{
    public static class WebApiConfig
    {
        /// <summary>
        /// WebApiConfig class
        /// </summary>
        /// <param name="config"></param>
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
