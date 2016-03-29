using System;

using SAHL.Core.Services;
using SAHL.Services.Interfaces.Halo.Models;

namespace SAHL.Services.Interfaces.Halo.Queries
{
    public class GetModuleConfigurationQuery : ServiceQuery<ModuleConfigurationQueryResult>, IHaloServiceQuery
    {
        public GetModuleConfigurationQuery(string applicationName, string moduleName, 
                                           bool returnAllRoots = false, string moduleParameters = null, HaloRoleModel role = null)
        {
            if (string.IsNullOrWhiteSpace(applicationName)) { throw new ArgumentNullException("applicationName"); }
            if (string.IsNullOrWhiteSpace(moduleName)) { throw new ArgumentNullException("moduleName"); }

            this.ApplicationName  = applicationName;
            this.ModuleName       = moduleName;
            this.ReturnAllRoots   = returnAllRoots;
            this.ModuleParameters = moduleParameters;
            this.Role             = role;
        }

        public string ApplicationName { get; protected set; }
        public string ModuleName { get; protected set; }
        public bool ReturnAllRoots { get; protected set; }
        public string ModuleParameters { get; protected set; }
        public HaloRoleModel Role { get; protected set; }
    }
}
