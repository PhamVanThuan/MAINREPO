namespace SAHL.Config.Web.Mvc.Routing
{
    public interface ICustomRoute
    {
        string Name { get; set; }
        string Url { get; }
        object Defaults { get; }
        object Constraints { get; }
    }
}