using DomainService2.SharedServices.Common;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;

namespace DomainService2.Specs.SharedServices.Common.CheckIsComcorpApplicationRuleCommandHandlerSpecs
{
    [Subject(typeof(CheckIsComcorpApplicationRuleCommandHandler))]
    public class when_there_is_an_application : RuleDomainServiceSpec<CheckIsComcorpApplicationRuleCommand, CheckIsComcorpApplicationRuleCommandHandler>
    {
        private static IApplicationReadOnlyRepository applicationReadOnlyRepository;
        private static IApplication application;

        private Establish context = () =>
        {
            application = An<IApplication>();

            applicationReadOnlyRepository = An<IApplicationReadOnlyRepository>();
            applicationReadOnlyRepository.WhenToldTo(x => x.GetApplicationByKey(Param<int>.IsAnything)).Return(application);

            command = new CheckIsComcorpApplicationRuleCommand(Param<int>.IsAnything, Param<bool>.IsAnything);
            handler = new CheckIsComcorpApplicationRuleCommandHandler(commandHandler, applicationReadOnlyRepository);
        };

        private Because of = () =>
        {
            handler.Handle(messages, command);
        };

        private It should_set_rule_parameters = () =>
        {
            command.RuleParameters[0].ShouldNotBeNull();
            command.RuleParameters[0].ShouldBeOfType(typeof(IApplication));
        };
    }
}