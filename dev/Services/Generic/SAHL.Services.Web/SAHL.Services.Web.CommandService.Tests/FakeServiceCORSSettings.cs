using SAHL.Config.Services;

namespace SAHL.Services.Web.CommandService.Tests
{
    public class FakeServiceCORSSettings : IServiceCORSSettings
    {
        public string AllowedHeaders
        {
            get { return "*"; }
        }

        public string AllowedMethods
        {
            get { return "*"; }
        }

        public string AllowedOrigins
        {
            get { return "*"; }
        }

        public string ExposedHeaders
        {
            get { return "*"; }
        }
    }
}