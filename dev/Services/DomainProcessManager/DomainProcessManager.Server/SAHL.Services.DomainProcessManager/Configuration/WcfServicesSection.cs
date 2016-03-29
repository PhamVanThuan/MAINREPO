using System.Configuration;

namespace SAHL.Services.DomainProcessManager.Configuration
{
    public class WcfServicesSection : ConfigurationSection
    {
        [ConfigurationProperty("", IsDefaultCollection = true)]
        public ServiceElementCollection Services
        {
            get
            {
                return ((ServiceElementCollection)base[""]);
            }
        }
    }
}
