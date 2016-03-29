using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Testing.Fakes;
using SAHL.Services.ApplicationDomain.Managers.AffordabilityAssessment;
using SAHL.Services.ApplicationDomain.Managers.AffordabilityAssessment.Statements;

namespace SAHL.Services.ApplicationDomain.Specs.ManagerSpecs.AffordabilityAssessmentData
{
    public class when_getting_affordability_assessment_stress_factor : WithFakes
    {
        private static IAffordabilityAssessmentDataManager affordabilityAssessmentDataManager;
        private static FakeDbFactory fakedDb;

        private static string stressfactorPercentage;

        private Establish context = () =>
        {
            stressfactorPercentage = "0%";
            fakedDb = new FakeDbFactory();
            affordabilityAssessmentDataManager = new AffordabilityAssessmentDataManager(fakedDb);

            fakedDb.FakedDb.DbReadOnlyContext.WhenToldTo(x => x.SelectOne<AffordabilityAssessmentStressFactorDataModel>(Param.IsAny<GetAssessmentStressFactorByPercentageStatement>()))
                                             .Return(new AffordabilityAssessmentStressFactorDataModel(1, "0%", 0.4m) { });
        };

        private Because of = () =>
        {
            affordabilityAssessmentDataManager.GetAffordabilityAssessmentStressFactorKeyByStressFactorPercentage(stressfactorPercentage);
        };

        private It should_query_the_database_for_the_specific_stress_factor_percentage = () =>
        {
            fakedDb.FakedDb.InReadOnlyAppContext().WasToldTo(x => x.SelectOne(Arg.Is<GetAssessmentStressFactorByPercentageStatement>(
               y => y.StressFactorPercentage == stressfactorPercentage)));
        };
    }
}