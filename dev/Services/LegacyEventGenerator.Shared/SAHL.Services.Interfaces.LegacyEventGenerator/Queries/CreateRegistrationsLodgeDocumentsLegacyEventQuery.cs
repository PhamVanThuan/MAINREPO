using SAHL.Core.Services;
using SAHL.Services.Interfaces.LegacyEventGenerator.Events.Workflow.Origination.Registration;
using System;

namespace SAHL.Services.Interfaces.LegacyEventGenerator.Queries
{
    public class CreateRegistrationsLodgeDocumentsLegacyEventQuery : ServiceQuery<RegistrationsLodgeDocumentsLegacyEvent>, ILegacyEventGeneratorQuery
    {
        public int StageDefinitionStageDefinitionGroupKey { get { return 4640; } }

        public int StageTransitionCompositeKey { get; protected set; }

        public int GenericKey { get; protected set; }

        public int GenericKeyTypeKey { get; protected set; }

        public DateTime Date { get; protected set; }

        public int ADUserKey { get; protected set; }

        public string ADUserName { get; protected set; }

        public void Initialise(int stageTransitionCompositeKey, int genericKey, int genericKeyTypeKey, DateTime date, int adUserKey,
            string adUserName)
        {
            this.StageTransitionCompositeKey = stageTransitionCompositeKey;
            this.GenericKey = genericKey;
            this.GenericKeyTypeKey = genericKeyTypeKey;
            this.Date = date;
            this.ADUserKey = adUserKey;
            this.ADUserName = adUserName;
        }
    }
}