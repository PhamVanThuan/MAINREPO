using SAHL.Core.Services;
using SAHL.Core.Services.Attributes;
using System;

namespace SAHL.Services.Interfaces.DecisionTreeDesign.Commands
{
    [AuthorisedCommand(Roles = "Domain Users")]
    public class SaveAndPublishEnumerationSetCommand : ServiceCommand, IDecisionTreeServiceCommand
    {
        public Guid Id { get; protected set; }

        public int Version { get; protected set; }

        public string Data { get; protected set; }

        public string Publisher { get; protected set; }

        public SaveAndPublishEnumerationSetCommand(Guid id, int version, string data, string publisher)
        {
            this.Id = id;
            this.Version = version;
            this.Data = data;
            this.Publisher = publisher;
        }
    }
}