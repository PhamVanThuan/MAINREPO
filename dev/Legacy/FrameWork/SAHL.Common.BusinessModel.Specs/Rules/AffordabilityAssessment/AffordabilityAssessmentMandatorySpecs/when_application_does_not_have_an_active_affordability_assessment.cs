using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.Rules.AffordabilityAssessment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Common.BusinessModel.Specs.Rules.AffordabilityAssessment.AffordabilityAssessmentMandatorySpecs
{
    [Subject(typeof(SAHL.Common.BusinessModel.Rules.AffordabilityAssessment.AffordabilityAssessmentMandatory))]
    public class when_application_does_not_have_an_active_affordability_assessment : RulesBaseWithFakes<AffordabilityAssessmentMandatory>
    {
        protected static IApplication application;
        protected static IAffordabilityAssessmentRepository affordabilityAssessmentRepo;
        protected static IAffordabilityAssessment affordabilityAssessment;

        private Establish context = () =>
        {
            application = An<IApplication>();
            affordabilityAssessment = An<IAffordabilityAssessment>();
            affordabilityAssessmentRepo = An<IAffordabilityAssessmentRepository>();

            List<IAffordabilityAssessment> activeAffordabilityAssessments = new List<IAffordabilityAssessment>();
            activeAffordabilityAssessments.Add(affordabilityAssessment);

            affordabilityAssessmentRepo.WhenToldTo(x => x.GetActiveApplicationAffordabilityAssessments(Param.IsAny<int>())).Return(new List<IAffordabilityAssessment>());

            AddRepositoryToMockCache(typeof(IAffordabilityAssessmentRepository), affordabilityAssessmentRepo);

            parameters = new object[1] { application };

            businessRule = new AffordabilityAssessmentMandatory();
            RulesBaseWithFakes<AffordabilityAssessmentMandatory>.startrule.Invoke();
        };

        private Because of = () =>
        {
            RuleResult = businessRule.ExecuteRule(messages, parameters);
        };

        private It should_fail_the_rule = () =>
        {
            messages.Count.ShouldEqual(1);
            RuleResult.ShouldEqual(0);
        };
    }
}
