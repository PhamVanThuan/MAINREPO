using SAHL.Core.Services;
using SAHL.Core.Services.Attributes;
using System;
using System.ComponentModel.DataAnnotations;

namespace SAHL.Services.Interfaces.DecisionTreeDesign.Commands
{
    [AuthorisedCommand(Roles = "Domain Users")]
    public class SaveMessageSetCommand : ServiceCommand, IDecisionTreeServiceCommand
    {
        [Required]
        public Guid MessageSetId { get; protected set; }

        [Required]
        public int Version { get; protected set; }

        [Required]
        public string Data { get; protected set; }

        public SaveMessageSetCommand(Guid messageSetid, int version, string data)
            : base()
        {
            this.MessageSetId = messageSetid;
            this.Version = version;
            this.Data = data;
        }
    }
}