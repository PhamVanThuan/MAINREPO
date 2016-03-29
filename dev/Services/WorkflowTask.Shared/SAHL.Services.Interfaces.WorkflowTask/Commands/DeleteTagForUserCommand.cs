using SAHL.Core.Services;
using System;

namespace SAHL.Services.Interfaces.WorkflowTask.Commands
{
    public class DeleteTagForUserCommand : ServiceCommand, IWorkflowServiceCommand
    {

        public DeleteTagForUserCommand(Guid id)
            : base(id)
        {
        }
    }
}