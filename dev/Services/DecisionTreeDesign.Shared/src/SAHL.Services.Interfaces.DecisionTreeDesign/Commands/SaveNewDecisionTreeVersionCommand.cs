using SAHL.Core.Services;
using SAHL.Core.Services.Attributes;
using System;
using System.ComponentModel.DataAnnotations;

namespace SAHL.Services.Interfaces.DecisionTreeDesign.Commands
{
    [AuthorisedCommand(Roles = "Domain Users")]
    public class SaveNewDecisionTreeVersionCommand : ServiceCommand, IDecisionTreeServiceCommand
    {
        public SaveNewDecisionTreeVersionCommand(Guid decisionTreeVersionId, Guid decisionTreeId, int version, string data, string username)
        {
            this.DecisionTreeVersionId = decisionTreeVersionId;
            this.DecisionTreeId = decisionTreeId;
            this.Data = data;
            this.Username = username;
            this.Version = version;
        }

        [Required]
        public Guid DecisionTreeVersionId { get; protected set; }

        [Required]
        public int Version { get; protected set; }

        [Required]
        public Guid DecisionTreeId { get; protected set; }

        [Required]
        public string Data { get; protected set; }

        [Required]
        public string Username { get; protected set; }
    }
}