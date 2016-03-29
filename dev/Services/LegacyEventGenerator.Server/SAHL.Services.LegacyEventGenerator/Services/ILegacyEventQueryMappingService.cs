using SAHL.Core.IoC;

namespace SAHL.Services.LegacyEventGenerator.Services
{
    public interface ILegacyEventQueryMappingService : IStartable, IStoppable
    {
        dynamic GetLegacyEventQueryByStageDefinitionStageDefinitionGroupKey(int stageDefinitionStageDefinitionGroupKey);
    }
}