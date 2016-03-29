using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Testing.Fakes;
using SAHL.Services.ApplicationDomain.Managers.AffordabilityAssessment;
using SAHL.Services.ApplicationDomain.Managers.AffordabilityAssessment.Statements;

namespace SAHL.Services.ApplicationDomain.Specs.ManagerSpecs.AffordabilityAssessmentData
{
    public class when_archiveing_unconfirmed_affordability_assessment : WithFakes
    {
        private static IAffordabilityAssessmentDataManager affordabilityAssessmentDataManager;
        private static FakeDbFactory fakedDb;
        private static int affordabilityAssessmentKey;
        private static int aduserKey;

        private Establish context = () =>
        {
            affordabilityAssessmentKey = 0;
            aduserKey = 0;

            fakedDb = new FakeDbFactory();
            affordabilityAssessmentDataManager = new AffordabilityAssessmentDataManager(fakedDb);
        };

        private Because of = () =>
        {
            affordabilityAssessmentDataManager.ArchiveAffordabilityAssessment(affordabilityAssessmentKey, aduserKey);
        };

        private It should_update_the_affordability_assessment_items = () =>
        {
            fakedDb.FakedDb.DbContext.WasToldTo(x => x.Update<AffordabilityAssessmentDataModel>(Param.IsAny<ArchiveAffordabilityAssessmentStatement>()));
        };
    }
}