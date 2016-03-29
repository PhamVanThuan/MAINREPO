using SAHL.Core.Services;
using SAHL.Core.Services.Attributes;
using System;
using System.ComponentModel.DataAnnotations;

namespace SAHL.Services.Interfaces.DecisionTreeDesign.Commands
{
    [AuthorisedCommand(Roles = "IT Developers")]
    public class DeleteDecisionTreeCommand : ServiceCommand, IDecisionTreeServiceCommand
    {
        [Required]
        public Guid DecisionTreeId { get; protected set; }

        [Required]
        public Guid DecisionTreeVersionId { get; protected set; }

        [Required]
        public string Username { get; protected set; }

        public DeleteDecisionTreeCommand(Guid decisionTreeId, Guid decisionTreeVersionId, string username)
        {
            this.DecisionTreeId = decisionTreeId;
            this.DecisionTreeVersionId = decisionTreeVersionId;
            this.Username = username;
        }
    }
}