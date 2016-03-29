using System.Collections.Generic;
using SAHL.Core.Data.Models.X2;
using SAHL.Core.Services;
using SAHL.Core.X2.Providers;

namespace SAHL.X2Engine2.Commands
{
    public class BuildSystemRequestToProcessCommand : ServiceCommand
    {
        public InstanceDataModel Instance { get; protected set; }

        public IX2ContextualDataProvider ContextualData { get; protected set; }

        public List<string> DecisionsToProcess { get; set; }

        public BuildSystemRequestToProcessCommand(InstanceDataModel instance, IX2ContextualDataProvider contextualData)
        {
            this.Instance = instance;
            this.ContextualData = contextualData;
            this.DecisionsToProcess = new List<string>();
        }
    }
}