using SAHL.Core.Services;
using SAHL.Core.Services.Attributes;
using System;

namespace SAHL.Services.Interfaces.DecisionTreeDesign.Commands
{
    [AuthorisedCommand(Roles = "Domain Users")]
    public class SaveEnumerationSetCommand : ServiceCommand, IDecisionTreeServiceCommand
    {
        public Guid Id { get; protected set; }

        public int Version { get; protected set; }

        public string Data { get; protected set; }

        public SaveEnumerationSetCommand(Guid id, int version, string data)
        {
            this.Id = id;
            this.Version = version;
            this.Data = data;
        }
    }
}