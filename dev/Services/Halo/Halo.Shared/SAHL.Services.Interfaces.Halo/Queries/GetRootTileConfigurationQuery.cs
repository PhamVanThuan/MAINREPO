using System;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using SAHL.Core.Services;
using SAHL.Core.BusinessModel;
using SAHL.Services.Interfaces.Halo.Models;

namespace SAHL.Services.Interfaces.Halo.Queries
{
    public class GetRootTileConfigurationQuery : ServiceQuery<RootTileConfigurationQueryResult>, IHaloServiceQuery
    {
        public GetRootTileConfigurationQuery(string applicationName, string moduleName, string rootTileName, 
                                             BusinessContext businessContext, HaloRoleModel roleModel)
        {
            if (string.IsNullOrWhiteSpace(applicationName)) { throw new ArgumentNullException("applicationName"); }
            if (string.IsNullOrWhiteSpace(moduleName)) { throw new ArgumentNullException("moduleName"); }
            if (string.IsNullOrWhiteSpace(rootTileName)) { throw new ArgumentNullException("rootTileName"); }
            if (roleModel == null) { throw new ArgumentNullException("roleModel"); } 

            this.ApplicationName = applicationName;
            this.ModuleName      = moduleName;
            this.RootTileName    = rootTileName;
            this.BusinessContext = businessContext;
            this.RoleModel       = roleModel;
        }

        public string ApplicationName { get; protected set; }
        public string ModuleName { get; protected set; }
        public string RootTileName { get; protected set; }
        public BusinessContext BusinessContext { get; protected set; }
        public HaloRoleModel RoleModel { get; protected set; }
    }
}
