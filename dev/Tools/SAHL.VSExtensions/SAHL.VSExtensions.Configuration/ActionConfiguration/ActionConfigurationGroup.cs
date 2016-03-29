using SAHL.VSExtensions.Interfaces.Configuration;

namespace SAHL.VSExtensions.Configuration.ActionConfiguration
{
    public class ActionConfigurationGroup : IMenuGroup
    {
        public string GroupName
        {
            get { return "Actions"; }
        }
    }
}