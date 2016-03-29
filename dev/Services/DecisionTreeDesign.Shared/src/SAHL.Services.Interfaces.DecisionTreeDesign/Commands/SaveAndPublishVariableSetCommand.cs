using SAHL.Core.Services;
using SAHL.Core.Services.Attributes;
using System;

namespace SAHL.Services.Interfaces.DecisionTreeDesign.Commands
{
    [AuthorisedCommand(Roles = "Domain Users")]
    public class SaveAndPublishVariableSetCommand : ServiceCommand, IDecisionTreeServiceCommand
    {
        public Guid VariableSetID { get; protected set; }

        public int Version { get; protected set; }

        public string Publisher { get; protected set; }

        public string Data { get; protected set; }

        public SaveAndPublishVariableSetCommand(Guid variableSetID, int version, string data, string publisher)
        {
            this.VariableSetID = variableSetID;
            this.Version = version;
            this.Publisher = publisher;
            this.Data = data;
        }
    }
}