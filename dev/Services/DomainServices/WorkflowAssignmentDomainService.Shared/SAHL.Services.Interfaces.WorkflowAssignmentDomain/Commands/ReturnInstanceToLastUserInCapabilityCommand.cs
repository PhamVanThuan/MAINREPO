using System;
using System.ComponentModel.DataAnnotations;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Services;
using SAHL.DomainServiceChecks.Checks;

namespace SAHL.Services.Interfaces.WorkflowAssignmentDomain.Commands
{
    public class ReturnInstanceToLastUserInCapabilityCommand : ServiceCommand, IWorkflowAssignmentDomainCommand 
    {
        [Required]
        public GenericKeyType GenericKeyTypeKey { get; private set; }

        [Required, Range(1, Int32.MaxValue)]
        public int GenericKey { get; private set; }

        [Required]
        public Capability Capability { get; private set; }

        [Required, Range(1, Int64.MaxValue)]
        public long InstanceId { get; private set; }

        public ReturnInstanceToLastUserInCapabilityCommand(GenericKeyType genericKeyTypeKey, int genericKey, Capability capability, long instanceId)
        {
            GenericKeyTypeKey = genericKeyTypeKey;
            GenericKey = genericKey;
            Capability = capability;
            InstanceId = instanceId;
        }
    }
}