using Common.Enums;

using System.Collections.Generic;

namespace BuildingBlocks.Services.Contracts
{
    public interface IStageTransitionService
    {
        IEnumerable<Automation.DataModels.StageTransition> GetStageTransitionsByGenericKey(int genericKey);

        bool InsertStageTransition(int genericKey, StageDefinitionStageDefinitionGroupEnum sdsdgKey, string testUser);

        bool CheckIfTransitionExists(int genericKey, int sdsdgkey);

        Automation.DataModels.StageTransition GetLatestStageTransitionByGenericKeyAndSDSDGKey(int genericKey, StageDefinitionStageDefinitionGroupEnum sdsdgKey);

        List<Automation.DataModels.StageTransition> GetStageTransition(int genericKey, StageDefinitionStageDefinitionGroupEnum sdsdg);
    }
}