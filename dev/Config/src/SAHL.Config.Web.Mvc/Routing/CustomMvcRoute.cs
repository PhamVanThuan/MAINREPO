namespace SAHL.Config.Web.Mvc.Routing
{
    public class CustomMvcRoute : CustomRoute
    {
        public CustomMvcRoute(string name, string url, object defaults = null, object constraints = null)
            : base(name, url, defaults, constraints)
        {
        }
    }
}