using SAHL.Services.Cuttlefish.Services;

namespace SAHL.Services.Cuttlefish.Specs.Fakes
{
    public class FakeMessageBusConfigurationProvider : IMessageBusConfigurationProvider
    {
        public string HostName
        {
            get;
            set;
        }

        public string UserName
        {
            get;
            set;
        }

        public string Password
        {
            get;
            set;
        }
    }
}