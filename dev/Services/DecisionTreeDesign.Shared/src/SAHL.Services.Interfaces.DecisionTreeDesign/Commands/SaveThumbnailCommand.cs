using SAHL.Core.Services;
using SAHL.Core.Services.Attributes;
using System;
using System.ComponentModel.DataAnnotations;

namespace SAHL.Services.Interfaces.DecisionTreeDesign.Commands
{
    [AuthorisedCommand(Roles = "Domain Users")]
    public class SaveThumbnailCommand : ServiceCommand, IDecisionTreeServiceCommand
    {
        public SaveThumbnailCommand(Guid decisionTreeId, string thumbnail) 
        {
            this.DecisionTreeId = decisionTreeId;
            this.Thumbnail = thumbnail;
        }

        [Required]
        public string Thumbnail { get; protected set; }

        [Required]
        public Guid DecisionTreeId { get; protected set; }
    }
}