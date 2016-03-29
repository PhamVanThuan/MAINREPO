using DomainService2.Workflow.Origination.ApplicationCapture;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections.Interfaces;

namespace DomainService2.Specs.Workflow.Origination.ApplicationCapture.CheckBranchSubmitApplicationRulesCommandHandlerSpecs
{
    [Subject(typeof(CheckBranchSubmitApplicationRulesCommandHandler))]
    public class When_there_is_no_application : RuleSetDomainServiceSpec<CheckBranchSubmitApplicationRulesCommand, CheckBranchSubmitApplicationRulesCommandHandler>
    {
        private static IApplicationRepository applicationRepository;
        private static IApplication application;

        Establish context = () =>
        {
            var ignoreWarnings = false;
            messages = An<IDomainMessageCollection>();
            application = null;

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

        It the_rule_should_fail = () =>
        {
            command.Result.ShouldBeFalse();
        };

        It should_contain_a_null_rule_parameter = () =>
        {
            command.RuleParameters.ShouldContain(new object[] { null });
        };
    }
}