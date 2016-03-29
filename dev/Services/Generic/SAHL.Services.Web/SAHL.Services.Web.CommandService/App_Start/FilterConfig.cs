using System.Web;
using System.Web.Mvc;

namespace SAHL.Services.Web.CommandService
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}