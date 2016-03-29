using SAHL.DomainServiceChecks.Checks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.DomainServiceCheck.Specs.CheckHandlers.RequiresX2Instance
{
    class RequiresX2InstanceCommand : IRequiresX2Instance
    {
        public int InstanceId
        {
            get; set;
        }

        public Guid Id
        {
            get; set;
        }
        public RequiresX2InstanceCommand(int InstanseId, Guid Id)
        {
            this.InstanceId = InstanseId;
            this.Id = Id;

        }
    }
}
