namespace SAHL.Config.Web.Mvc.Routing
{
    public class CustomApiRoute : CustomRoute
    {
        public CustomApiRoute(string name, string routeTemplate, object defaults = null, object constraints = null) 
            : base(name, routeTemplate, defaults, constraints)
        {
        }
    }
}