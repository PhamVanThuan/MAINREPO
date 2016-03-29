using SAHL.Services.Interfaces.UserProfile.Models.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Services.Interfaces.UserProfile.Models
{
    public class GetCapabilitiesForUserQueryResult
    {
        public IEnumerable<RoleCapabilities> RoleCapabilities { get; set; }

        public GetCapabilitiesForUserQueryResult(IEnumerable<RoleCapabilities> roleCapabilities)
        {
            this.RoleCapabilities = roleCapabilities;
        }
    }
}
