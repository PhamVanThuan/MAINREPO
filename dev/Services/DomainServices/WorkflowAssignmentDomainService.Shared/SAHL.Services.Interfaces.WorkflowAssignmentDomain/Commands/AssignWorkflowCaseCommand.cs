using System;
using System.ComponentModel.DataAnnotations;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Services;
using SAHL.Services.Interfaces.WorkflowAssignmentDomain.Model;

namespace SAHL.Services.Interfaces.WorkflowAssignmentDomain.Commands
{
    public class AssignWorkflowCaseCommand : ServiceCommand, IWorkflowAssignmentDomainCommand
    {
        [Required]
        public GenericKeyType GenericKeyTypeKey { get; protected set; }

        [Required, Range(1, Int32.MaxValue)]
        public int GenericKey { get; protected set; }

        [Required, Range(1, Int64.MaxValue)]
        public long InstanceId { get; protected set; }

        [Required] 
        [Range(1, Int32.MaxValue, ErrorMessage="A valid user to assign to is required")]
        public int UserOrganisationStructureKey { get; protected set; }

        [Required]
        public Capability Capability { get; protected set; }

        public AssignWorkflowCaseCommand(GenericKeyType genericKeyTypeKey, int genericKey, long instanceId, int userOrganisationStructureKey, Capability capability)
        {
            this.GenericKeyTypeKey = genericKeyTypeKey;
            this.GenericKey = genericKey;
            this.InstanceId = instanceId;
            this.UserOrganisationStructureKey = userOrganisationStructureKey;
            this.Capability = capability;
        }
    }
}