using Machine.Specifications;
using Machine.Fakes;
using SAHL.Core.Testing.Fakes;
using SAHL.Services.ApplicationDomain.Managers.AffordabilityAssessment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SAHL.Core.Data.Models._2AM;

namespace SAHL.Services.ApplicationDomain.Specs.ManagerSpecs.AffordabilityAssessmentData
{
    public class when_deleting_affordability_assessment_legalentity : WithFakes
    {
        private static IAffordabilityAssessmentDataManager affordabilityAssessmentDataManager;
        private static FakeDbFactory fakedDb;

        private static int affordabilityAssessmentKey;
        private static int legalEntityKey;

        private Establish context = () =>
        {
            fakedDb = new FakeDbFactory();
            affordabilityAssessmentDataManager = new AffordabilityAssessmentDataManager(fakedDb);

            affordabilityAssessmentKey = 99292;
            legalEntityKey = 99;
        };

        private Because of = () =>
        {
            affordabilityAssessmentDataManager.DeleteAffordabilityAssessmentLegalEntity(affordabilityAssessmentKey, legalEntityKey);
        };

        private It should_delete_affordability_assessment_legal_entity = () =>
        {
            fakedDb.FakedDb.DbContext.WasToldTo(x => x.DeleteWhere<AffordabilityAssessmentLegalEntityDataModel>(Param.IsAny<string>(), Param.IsAny<object>()));
        };
    }
}
