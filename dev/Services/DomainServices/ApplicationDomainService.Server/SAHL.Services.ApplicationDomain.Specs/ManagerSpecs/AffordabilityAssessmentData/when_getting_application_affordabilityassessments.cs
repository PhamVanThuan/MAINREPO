using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Testing;
using SAHL.Core.Testing.Fakes;
using SAHL.Services.ApplicationDomain.Managers.AffordabilityAssessment;
using SAHL.Services.ApplicationDomain.Managers.AffordabilityAssessment.Statements;

namespace SAHL.Services.ApplicationDomain.Specs.ManagerSpecs.AffordabilityAssessmentData
{
    public class when_getting_application_affordabilityassessments : WithCoreFakes
    {
        private static IAffordabilityAssessmentDataManager affordabilityAssessmentDataManager;
        private static FakeDbFactory fakedDb;

        private static int applicationKey;

        private Establish context = () =>
        {
            applicationKey = 1;

            fakedDb = new FakeDbFactory();
            affordabilityAssessmentDataManager = new AffordabilityAssessmentDataManager(fakedDb);
        };

        private Because of = () =>
        {
            affordabilityAssessmentDataManager.GetActiveAffordabilityAssessmentsForApplication(applicationKey);
        };

        private It should_execute_the_get_unconfirmed_affordability_assessments_by_generickey_statement = () =>
        {
            fakedDb.FakedDb.InReadOnlyAppContext().WasToldTo(x => x.Select<AffordabilityAssessmentDataModel>(
                Arg.Is<GetActiveAffordabilityAssessmentsByGenericKeyStatement>(y => y.GenericKey == applicationKey && y.GenericKeyTypeKey == (int)GenericKeyType.Offer)));
        };
    }
}