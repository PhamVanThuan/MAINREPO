using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.Testing.Fakes;
using SAHL.DomainServiceChecks.Managers.AffordabilityAssessmentDataManager;
using SAHL.DomainServiceChecks.Managers.AffordabilityAssessmentDataManager.Statements;

namespace SAHL.DomainServiceCheck.Specs.Managers.AffordabilityAssessment
{
    public class when_getting_an_affordability_assessment : WithFakes
    {
        private static AffordabilityAssessmentDataManager affordabilityAssessmentDataManager;
        private static FakeDbFactory fakeDbFactory;

        private Establish context = () =>
        {
            fakeDbFactory = new FakeDbFactory();
            affordabilityAssessmentDataManager = new AffordabilityAssessmentDataManager(fakeDbFactory);
        };

        private Because of = () =>
        {
            affordabilityAssessmentDataManager.GetAffordabilityAssessmentByKey(0);
        };

        private It should_check_for_a_disability_claim = () =>
        {
            fakeDbFactory.FakedDb.InReadOnlyAppContext().WasToldTo(x => x.SelectOne(Arg.Is<GetAffordabilityAssessmentByKeyStatement>(y => y.AffordabilityAssessmentKey == 0)));
        };
    }
}