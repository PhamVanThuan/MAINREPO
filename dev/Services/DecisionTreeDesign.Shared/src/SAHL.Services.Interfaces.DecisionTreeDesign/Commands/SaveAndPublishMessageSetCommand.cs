using SAHL.Core.Services;
using SAHL.Core.Services.Attributes;
using System;
using System.ComponentModel.DataAnnotations;

namespace SAHL.Services.Interfaces.DecisionTreeDesign.Commands
{
    [AuthorisedCommand(Roles = "Domain Users")]
    public class SaveAndPublishMessageSetCommand : ServiceCommand, IDecisionTreeServiceCommand
    {
        [Required]
        public Guid MessageSetId { get; protected set; }

        [Required]
        public int Version { get; protected set; }

        [Required]
        public string Data { get; protected set; }

        [Required]
        public string Publisher { get; protected set; }

        public SaveAndPublishMessageSetCommand(Guid messageSetId, int version, string data, string publisher)
            : base()
        {
            this.MessageSetId = messageSetId;
            this.Version = version;
            this.Data = data;
            this.Publisher = publisher;
        }
    }
}