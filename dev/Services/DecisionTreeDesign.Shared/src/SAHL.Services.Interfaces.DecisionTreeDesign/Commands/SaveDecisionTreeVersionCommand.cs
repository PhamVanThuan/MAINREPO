using SAHL.Core.Services;
using SAHL.Core.Services.Attributes;
using System;
using System.ComponentModel.DataAnnotations;

namespace SAHL.Services.Interfaces.DecisionTreeDesign.Commands
{
    [AuthorisedCommand(Roles = "Domain Users")]
    public class SaveDecisionTreeVersionCommand : ServiceCommand, IDecisionTreeServiceCommand
    {
        public SaveDecisionTreeVersionCommand(Guid decisionTreeId, string name, string description, bool isActive, string thumbnail, Guid decisionTreeVersionId, string data, string username)
        {
            this.DecisionTreeId = decisionTreeId;
            this.Name = name;
            this.Description = description;
            this.IsActive = isActive;
            this.Thumbnail = thumbnail;
            this.DecisionTreeVersionId = decisionTreeVersionId;
            this.Data = data;
            this.Username = username;
        }

        [Required]
        public Guid DecisionTreeId { get; protected set; }

        [Required]
        public string Name { get; protected set; }

        [Required]
        public string Description { get; protected set; }

        [Required]
        public bool IsActive { get; protected set; }

        [Required]
        public string Thumbnail { get; protected set; }

        [Required]
        public Guid DecisionTreeVersionId { get; protected set; }

        [Required]
        public string Data { get; protected set; }

        [Required]
        public string Username { get; protected set; }
    }
}