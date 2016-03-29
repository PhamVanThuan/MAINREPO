using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.Testing.Fakes;
using SAHL.Services.ApplicationDomain.Managers.AffordabilityAssessment;
using SAHL.Services.ApplicationDomain.Managers.AffordabilityAssessment.Statements;

namespace SAHL.Services.ApplicationDomain.Specs.ManagerSpecs.AffordabilityAssessmentData
{
    public class when_getting_hoc_instalment_for_further_lending : WithFakes
    {
        private static FakeDbFactory fakedDb;
        private static IAffordabilityAssessmentDataManager affordabilityAssessmentDataManager;

        private Establish context = () =>
        {
            fakedDb = new FakeDbFactory();
            affordabilityAssessmentDataManager = new AffordabilityAssessmentDataManager(fakedDb);

            fakedDb.FakedDb.DbReadOnlyContext.WhenToldTo(x => x.SelectOne<double?>(Param.IsAny<GetHocInstalmentForFurtherLendingApplicationStatement>())).Return(Param.IsAny<double>());
        };

        private Because of = () =>
        {
            affordabilityAssessmentDataManager.GetHocInstalmentForFurtherLendingApplication(Param.IsAny<int>());
        };

        private It should_query_the_database_for_the_further_lending_hoc_instalment = () =>
        {
            fakedDb.FakedDb.InReadOnlyAppContext().WasToldTo(x => x.SelectOne<double?>(Arg.Is<GetHocInstalmentForFurtherLendingApplicationStatement>(y => y.ApplicationKey == 0)));
        };
    }
}