using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Services.Interfaces.UserProfile.Models.Shared
{
    public class RoleCapabilities
    {
        public int Id { get;protected set; }
        public IEnumerable<string> Capabilities { get; protected set; }

        public RoleCapabilities(int id, IEnumerable<string> capabilities)
        {
            this.Id = id;
            this.Capabilities = capabilities;
        }
    }
}
