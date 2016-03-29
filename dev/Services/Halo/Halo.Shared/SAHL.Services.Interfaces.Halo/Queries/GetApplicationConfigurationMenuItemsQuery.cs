using System;

using SAHL.Core.Services;
using SAHL.Services.Interfaces.Halo.Models;

namespace SAHL.Services.Interfaces.Halo
{
    public class GetApplicationConfigurationMenuItemsQuery : ServiceQuery<ApplicationMenuItem>, IHaloServiceQuery
    {
        public GetApplicationConfigurationMenuItemsQuery(string applicationName, HaloRoleModel roleModel)
        {
            if (string.IsNullOrWhiteSpace(applicationName)) { throw new ArgumentNullException("applicationName"); }

            this.ApplicationName = applicationName;
            this.RoleModel       = roleModel;
        }

        public string ApplicationName { get; protected set; }
        public HaloRoleModel RoleModel { get; protected set; }
    }
}
