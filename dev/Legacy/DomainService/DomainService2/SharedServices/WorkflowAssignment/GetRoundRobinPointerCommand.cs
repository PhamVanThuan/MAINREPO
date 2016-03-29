using SAHL.Common.Globals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainService2.SharedServices.WorkflowAssignment
{
    public class GetRoundRobinPointerCommand : StandardDomainServiceCommand
    {
        public GetRoundRobinPointerCommand(OfferRoleTypes offerRoleType, int organisationStructureKey)
        {
            this.OfferRoleType = offerRoleType;
            this.OrganisationStructureKey = organisationStructureKey;
        }

        public OfferRoleTypes OfferRoleType { get; protected set; } 
        public int OrganisationStructureKey { get; protected set; }

        public int Result { get; set; }
    }
}
