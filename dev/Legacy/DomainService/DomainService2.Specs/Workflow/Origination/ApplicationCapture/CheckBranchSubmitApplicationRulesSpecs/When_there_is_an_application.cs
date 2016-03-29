using DomainService2.Workflow.Origination.ApplicationCapture;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections.Interfaces;

namespace DomainService2.Specs.Workflow.Origination.ApplicationCapture.CheckBranchSubmitApplicationRulesSpecs
{
    [Subject(typeof(CheckBranchSubmitApplicationRulesCommandHandler))]
    public class When_there_is_an_application : RuleSetDomainServiceSpec<CheckBranchSubmitApplicationRulesCommand, CheckBranchSubmitApplicationRulesCommandHandler>
    {
        private static IApplicationRepository applicationRepository;
        private static IApplication application;

        Establish context = () =>
        {
            var ignoreWarnings = false;
            messages = An<IDomainMessageCollection>();
            application = An<IApplication>();
            application.WhenToldTo(x => x.Key).Return(-1);

            applicationRepository = An<IApplicationRepository>();
            applicationRepository.WhenToldTo(x => x.GetApplicationByKey(Param.IsAny<int>())).Return(application);

            handler = new CheckBranchSubmitApplicationRulesCommandHandler(commandHandler, applicationRepository);
            command = new CheckBranchSubmitApplicationRulesCommand(Param.IsAny<int>(), ignoreWarnings);

            command.ApplicationKey = -1;
        };

        Because of = () =>
        {
            handler.Handle(messages, command);
        };

        It should_contain_an_application_as_rule_parameter = () =>
        {
            command.RuleParameters.ShouldContain(new object[] { application });
        };

        It should_set_the_rule_set_to_application_capture_submit_application = () =>
        {
            command.WorkflowRuleSetName.ShouldBeEqualIgnoringCase(SAHL.Common.RuleSets.ApplicationCaptureSubmitApplication);
        };
    }
}