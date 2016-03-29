using System;

namespace SAHL.Services.Interfaces.WorkflowAssignmentDomain.Model
{
    public class GetUserCurrentlyAssignedInCapabilityQueryResult
    {
        public string UserName { get; set; }
        public DateTime AssignmentDate { get; set; }
        public int UserOrganisationStructureKey { get; set; }
    }
}