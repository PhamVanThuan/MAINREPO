using SAHL.Core.Services;
using System;
using System.ComponentModel.DataAnnotations;

namespace SAHL.Services.Interfaces.WorkflowTask.Commands
{
    public class CreateTagForUserCommand : ServiceCommand, IWorkflowServiceCommand
    {
        public CreateTagForUserCommand(Guid id, string caption, string backColour, string foreColour, string username):base(id)
        {
            this.ForeColour = foreColour;
            this.BackColour = backColour;
            this.Caption = caption;
            this.UserName = username;
        }
        [StringLength(100, ErrorMessage = "Caption Max Length is 100")]
        public string Caption { get; protected set; }

        public string BackColour { get; protected set; }

        public string ForeColour { get; protected set; }

        public string UserName { get; protected set; }
    }
}