namespace SAHL.Core.Web.Services
{
    public interface IServiceUrlConfiguration
    {
        string ServiceName { get; }

        string GetCommandServiceUrl();
    }
}