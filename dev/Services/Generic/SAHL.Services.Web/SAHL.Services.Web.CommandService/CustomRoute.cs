using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SAHL.Services.Web.CommandService
{
    public interface ICustomRoute
    {
        string Route { get; }
    }

    public class CustomRoute : ICustomRoute
    {
        public string Route { get; private set; }

        public CustomRoute(string route)
        {
            Route = route;
        }
    }
}