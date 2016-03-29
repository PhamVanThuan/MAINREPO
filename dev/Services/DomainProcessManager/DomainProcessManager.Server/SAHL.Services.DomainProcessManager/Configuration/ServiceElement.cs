using System.Configuration;

namespace SAHL.Services.DomainProcessManager.Configuration
{
    public class ServiceElement : ConfigurationElement
    {
        [ConfigurationProperty("Name", IsRequired = true)]
        public string Name
        {
            get { return ((string)base["Name"]); }
            set { base["Name"] = value; }
        }

        [ConfigurationProperty("Interface", IsRequired = true)]
        public string Interface
        {
            get { return ((string)base["Interface"]); }
            set { base["Interface"] = value; }
        }

        [ConfigurationProperty("Address", IsRequired = true)]
        public string Address
        {
            get { return ((string)base["Address"]); }
            set { base["Address"] = value; }
        }
    }
}
