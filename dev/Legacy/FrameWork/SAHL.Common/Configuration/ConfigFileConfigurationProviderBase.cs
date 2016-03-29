// -----------------------------------------------------------------------
// <copyright file="ConfigFileConfigurationProviderBase.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace SAHL.Common.Configuration
{
    using System.Configuration;

    public abstract class ConfigFileConfigurationProviderBase
    {
        public ConfigFileConfigurationProviderBase()
        {
            if (System.Web.HttpContext.Current != null && !System.Web.HttpContext.Current.Request.PhysicalPath.Equals(string.Empty))
                this.Config = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration("~");
            else
                this.Config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
        }

        protected Configuration Config { get; private set; }
    }
}