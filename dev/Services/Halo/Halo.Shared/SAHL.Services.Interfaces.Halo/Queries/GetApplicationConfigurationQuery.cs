using System;

using SAHL.Core.Services;
using SAHL.Core.BusinessModel;

namespace SAHL.Services.Interfaces.Halo
{
    public class GetApplicationConfigurationQuery : ServiceQuery<ApplicationConfigurationQueryResult>, IHaloServiceQuery
    {
        public GetApplicationConfigurationQuery(string applicationName)
        {
            if (string.IsNullOrWhiteSpace(applicationName)) { throw new ArgumentNullException("applicationName"); }
            this.ApplicationName = applicationName;
        }

        public string ApplicationName { get; protected set; }
    }
}
