using SAHL.VSExtensions.Interfaces.Configuration;

namespace SAHL.VSExtensions.Configuration.EditorConfiguration
{
    public class EditorConfigurationGroup : IMenuGroup
    {
        public string GroupName
        {
            get { return "Editors"; }
        }
    }
}