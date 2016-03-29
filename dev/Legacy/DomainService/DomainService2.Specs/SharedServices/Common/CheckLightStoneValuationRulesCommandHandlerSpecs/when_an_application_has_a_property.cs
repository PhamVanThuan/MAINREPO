using DomainService2.SharedServices.Common;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;

namespace DomainService2.Specs.SharedServices.Common.CheckLightStoneValuationRulesCommandHandlerSpecs
{
    [Subject(typeof(CheckLightStoneValuationRulesCommandHandler))]
    public class when_an_application_has_a_property : RuleSetDomainServiceSpec<CheckLightStoneValuationRulesCommand, CheckLightStoneValuationRulesCommandHandler>
    {
        protected static IPropertyRepository propertyRepository;

        // Arrange
        Establish context = () =>
        {
            propertyRepository = An<IPropertyRepository>();
            IProperty property = An<IProperty>();
            propertyRepository.WhenToldTo(x => x.GetPropertyByApplicationKey(Param<int>.IsAnything)).Return(property);
            command = new CheckLightStoneValuationRulesCommand(Param<int>.IsAnything, Param<bool>.IsAnything);
            handler = new CheckLightStoneValuationRulesCommandHandler(commandHandler, propertyRepository);
        };

        // Act
        Because of = () =>
        {
            handler.Handle(messages, command);
        };

        // Assert
        It should_set_rule_parameters = () =>
        {
            command.RuleParameters[0].ShouldNotBeNull();
            command.RuleParameters[0].ShouldBeOfType(typeof(IProperty));
        };
    }
}