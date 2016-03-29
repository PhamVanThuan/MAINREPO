using System;
using System.Collections.Generic;
using System.Linq;
using SAHL.Core;
using SAHL.Services.Interfaces.LegacyEventGenerator.Queries;

namespace SAHL.Services.LegacyEventGenerator.Services
{
    public class LegacyEventQueryMappingService : ILegacyEventQueryMappingService
    {
        private IDictionary<int, Type> legacyEventQueryMapping = new Dictionary<int, Type>();

        private ILegacyEventDataService legacyEventDataService;
        private IIocContainer iocContainer;

        public LegacyEventQueryMappingService(ILegacyEventDataService legacyEventDataService, IIocContainer iocContainer)
        {
            this.iocContainer = iocContainer;
            this.legacyEventDataService = legacyEventDataService;
        }

        public dynamic GetLegacyEventQueryByStageDefinitionStageDefinitionGroupKey(int stageDefinitionStageDefinitionGroupKey)
        {
            if (legacyEventQueryMapping == null || legacyEventQueryMapping.Count == 0)
            {
                return null;
            }
            Type legacyEventQueryType = null;
            legacyEventQueryMapping.TryGetValue(stageDefinitionStageDefinitionGroupKey, out legacyEventQueryType);
            return legacyEventQueryType == null ? null : iocContainer.GetConcreteInstance(legacyEventQueryType);
        }

        public void Start()
        {
            IEnumerable<ILegacyEventGeneratorQuery> legacyEventGeneratorQueries = iocContainer.GetAllInstances<ILegacyEventGeneratorQuery>();
            if (legacyEventGeneratorQueries != null && legacyEventGeneratorQueries.Count() > 0)
            {
                foreach (ILegacyEventGeneratorQuery legacyEventGeneratorQuery in legacyEventGeneratorQueries)
                {
                    legacyEventQueryMapping.Add(legacyEventGeneratorQuery.StageDefinitionStageDefinitionGroupKey, legacyEventGeneratorQuery.GetType());
                }
            }
        }

        public void Stop()
        {
            legacyEventQueryMapping = null;
        }
    }
}