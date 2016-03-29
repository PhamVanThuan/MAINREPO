using System;

namespace SAHL.Tools.Workflow.Common.WorkflowElements
{
    [Serializable]
    public class BusinessStageTransition : AbstractElement
    {
        public BusinessStageTransition(int stageDefinitionStageDefinitionGroupKey, string defintionGroupDescription, string definitionDescription)
            : base()
        {
            this.StageDefinitionStageDefinitionGroupKey = stageDefinitionStageDefinitionGroupKey;
            this.DefintionGroupDescription = defintionGroupDescription;
            this.DefinitionDescription = definitionDescription;
        }

        public int StageDefinitionStageDefinitionGroupKey { get; protected set; }

        public string DefintionGroupDescription { get; protected set; }

        public string DefinitionDescription { get; protected set; }
    }
}