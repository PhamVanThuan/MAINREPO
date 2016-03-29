using SAHL.Core.Services;
using System;

namespace SAHL.Services.Interfaces.WorkflowTask.Commands
{
    public class UpdateUserTagCommand : ServiceCommand, IWorkflowServiceCommand
    {
        public UpdateUserTagCommand(Guid id, string foreColour, string backColour, string caption)
            : base(id)
        {
            Caption = caption;
            BackColour = backColour;
            ForeColour = foreColour;
        }

        public string Caption { get; protected set; }

        public string BackColour { get; protected set; }

        public string ForeColour { get; protected set; }
    }
}