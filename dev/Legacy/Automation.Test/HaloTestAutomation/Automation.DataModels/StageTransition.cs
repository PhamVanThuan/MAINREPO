using System;

namespace Automation.DataModels
{
    public class StageTransition : IDataModel
    {
        public int StageTransitionKey { get; set; }

        public int GenericKey { get; set; }

        public string StageDefinitionGroup { get; set; }

        public int SDSDGKey { get; set; }

        public string StageDefinition { get; set; }

        public DateTime TransitionDate { get; set; }

        public DateTime EndTransitionDate { get; set; }

        public int StageDefinitionGroupKey { get; set; }

        public int StageDefinitionKey { get; set; }

        public string Comments { get; set; }
    }
}