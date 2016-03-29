using SAHL.Core.Services;
using SAHL.Core.Services.Attributes;
using System;
using System.ComponentModel.DataAnnotations;

namespace SAHL.Services.Interfaces.DecisionTreeDesign.Commands
{
    [AuthorisedCommand(Roles = "Domain Users")]
    public class SaveAndPublishDecisionTreeCommand : ServiceCommand, IDecisionTreeServiceCommand
    {
        [Required]
        public Guid DecisionTreeId { get; protected set; }

        [Required]
        public string Name { get; protected set; }

        [Required]
        public string Description { get; protected set; }

        [Required]
        public bool IsActive { get; protected set; }

        [Required]
        public Guid TreeVersionId { get; protected set; }

        [Required]
        public string Data { get; protected set; }

        [Required]
        public string Publisher { get; protected set; }

        [Required]
        public bool SaveFirst { get; protected set; }

        [Required]
        public string Thumbnail { get; protected set; }

        public SaveAndPublishDecisionTreeCommand(Guid decisionTreeId, string name, string description, bool isActive, string thumbnail, Guid treeVersionId, string data, string publisher, bool saveFirst)
        {
            this.DecisionTreeId = decisionTreeId;
            this.Name = name;
            this.Description = description;
            this.IsActive = isActive;
            this.Thumbnail = thumbnail;
            this.TreeVersionId = treeVersionId;
            this.Data = data;
            this.Publisher = publisher;
            this.SaveFirst = saveFirst;
        }
    }
}