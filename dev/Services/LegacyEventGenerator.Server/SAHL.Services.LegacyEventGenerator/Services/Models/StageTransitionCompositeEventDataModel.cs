using System;

namespace SAHL.Services.LegacyEventGenerator.Services.Models
{
    public class StageTransitionCompositeEventDataModel
    {
        public int StageDefinitionStageDefinitionGroupKey { get; set; }

        public DateTime TransitionDate { get; set; }

        public int GenericKey { get; set; }

        public int GenericKeyTypeKey { get; set; }

        public int ADUserKey { get; set; }

        public string ADUserName { get; set; }
    }
}