using SAHL.Core.Services;
using System;

namespace SAHL.Services.Interfaces.LegacyEventGenerator.Queries
{
    public interface ILegacyEventGeneratorQuery : IServiceQuery
    {
        int StageDefinitionStageDefinitionGroupKey { get; }

        int StageTransitionCompositeKey { get; }

        int GenericKey { get; }

        int GenericKeyTypeKey { get; }

        DateTime Date { get; }

        int ADUserKey { get; }

        string ADUserName { get; }

        void Initialise(int stageTransitionCompositeKey, int genericKey, int genericKeyTypeKey, DateTime date, int adUserKey, string adUserName);
    }
}