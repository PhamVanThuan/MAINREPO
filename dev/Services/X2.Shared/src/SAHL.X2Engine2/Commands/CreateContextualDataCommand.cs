using System.Collections.Generic;
using SAHL.Core.Data.Models.X2;
using SAHL.Core.Services;
using SAHL.Core.X2.Providers;

namespace SAHL.X2Engine2.Commands
{
    public class CreateContextualDataCommand : ServiceCommand
    {
        public InstanceDataModel Instance { get; protected set; }

        public Dictionary<string, string> MapVariables { get; protected set; }

        public IX2ContextualDataProvider ContextualData { get; protected set; }

        public CreateContextualDataCommand(InstanceDataModel instance, IX2ContextualDataProvider contextualData, Dictionary<string, string> mapVariables = null)
        {
            this.Instance = instance;
            this.MapVariables = mapVariables;
            this.ContextualData = contextualData;
        }
    }
}