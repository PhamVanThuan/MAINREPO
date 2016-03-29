namespace SAHL.Core.Web.Services
{
    public class ServiceUrlConfiguration : IServiceUrlConfiguration
    {
        public ServiceUrlConfiguration(string serviceName)
        {
            this.ServiceName = serviceName;
        }

        public string ServiceName { get; protected set; }

        public string GetCommandServiceUrl()
        {
            return string.Format("http://{0}", this.ServiceName);
        }

        public string GetPerformCommandWithResultServiceUrl()
        {
            return string.Format("http://{0}", this.ServiceName);
        }
    }
}