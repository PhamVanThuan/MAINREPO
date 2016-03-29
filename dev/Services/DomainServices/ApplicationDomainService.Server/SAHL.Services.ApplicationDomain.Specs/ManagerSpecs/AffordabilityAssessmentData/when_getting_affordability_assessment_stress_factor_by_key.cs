using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Testing;
using SAHL.Core.Testing.Fakes;
using SAHL.Services.ApplicationDomain.Managers.AffordabilityAssessment;

namespace SAHL.Services.ApplicationDomain.Specs.ManagerSpecs.AffordabilityAssessmentData
{
    public class when_getting_affordability_assessment_stress_factor_by_key : WithCoreFakes
    {
        private static IAffordabilityAssessmentDataManager affordabilityAssessmentDataManager;
        private static FakeDbFactory fakedDb;

        private static int affordabilityAssessmentStressFactorKey;

        private Establish context = () =>
        {
            affordabilityAssessmentStressFactorKey = 1;

            fakedDb = new FakeDbFactory();
            fakedDb.FakedDb.DbReadOnlyContext.WhenToldTo(x => x.SelectOneWhere<AffordabilityAssessmentStressFactorDataModel>(Param.IsAny<string>(), Param.IsAny<object>()))
                                             .Return(new AffordabilityAssessmentStressFactorDataModel(1, "5%", 0.5M) { });

            affordabilityAssessmentDataManager = new AffordabilityAssessmentDataManager(fakedDb);
        };

        private Because of = () =>
        {
            affordabilityAssessmentDataManager.GetAffordabilityAssessmentStressFactorByKey(affordabilityAssessmentStressFactorKey);
        };

        private It should_execute_the_get_unconfirmed_affordability_assessments_by_generickey_statement = () =>
        {
            fakedDb.FakedDb.InReadOnlyAppContext().WasToldTo(x => x.SelectOneWhere<AffordabilityAssessmentStressFactorDataModel>(Param.IsAny<string>(), Param.IsAny<object>()));
        };
    }
}