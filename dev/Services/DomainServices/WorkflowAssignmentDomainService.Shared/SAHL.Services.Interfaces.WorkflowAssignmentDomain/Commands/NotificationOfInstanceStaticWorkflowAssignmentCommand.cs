using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Services;
using System;
using System.ComponentModel.DataAnnotations;

namespace SAHL.Services.Interfaces.WorkflowAssignmentDomain.Commands
{
    public class NotificationOfInstanceStaticWorkflowAssignmentCommand : ServiceCommand, IWorkflowAssignmentDomainCommand
    {
        public NotificationOfInstanceStaticWorkflowAssignmentCommand(long instanceId, GenericKeyType genericKeyTypeKey, string staticGroupName, int genericKey)
        {
            this.GenericKey = genericKey;
            this.GenericKeyTypeKey = genericKeyTypeKey;
            this.StaticGroupName = staticGroupName;
            this.InstanceId = instanceId;
        }

        [Required]
        public long InstanceId { get; protected set; }

        [Required]
        public GenericKeyType GenericKeyTypeKey { get; protected set; }

        [Required, Range(1, Int32.MaxValue)]
        public int GenericKey { get; protected set; }

        [Required]
        public string StaticGroupName { get; protected set; }
    }
}