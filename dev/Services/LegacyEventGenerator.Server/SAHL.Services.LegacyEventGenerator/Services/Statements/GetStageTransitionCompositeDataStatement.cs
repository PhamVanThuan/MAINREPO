using SAHL.Core.Data;
using SAHL.Services.LegacyEventGenerator.Services.Models;

namespace SAHL.Services.LegacyEventGenerator.Services.Statements
{
    public class GetStageTransitionCompositeDataStatement : ISqlStatement<StageTransitionCompositeEventDataModel>
    {
        public int StageTransitionCompositeKey { get; protected set; }

        public GetStageTransitionCompositeDataStatement(int stageTransitionCompositeKey)
        {
            this.StageTransitionCompositeKey = stageTransitionCompositeKey;
        }

        public string GetStatement()
        {
            return @"select stc.[StageDefinitionStageDefinitionGroupKey], stc.[TransitionDate], stc.[GenericKey], sdg.[GenericKeyTypeKey], stc.ADUserKey, a.ADUserName
                        from [2am].[dbo].[StageTransitionComposite] stc
                        join [2am].[dbo].[StageDefinitionStageDefinitionGroup] sdsdg on sdsdg.[StageDefinitionStageDefinitionGroupKey] = stc.[StageDefinitionStageDefinitionGroupKey]
                        join [2am].[dbo].[StageDefinitionGroup] sdg on sdg.[StageDefinitionGroupKey] = sdsdg.[StageDefinitionGroupKey]
                        join [2am].[dbo].[ADUser] a on a.ADUserKey = stc.ADUserKey
                        where stc.[StageTransitionCompositeKey] = @StageTransitionCompositeKey";
        }
    }
}