using DomainService2.SharedServices.Common;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;

namespace DomainService2.Specs.SharedServices.Common.DoLightStoneValuationForWorkFlowCommandHandlerSpecs
{
    [Subject(typeof(DoLightStoneValuationForWorkFlowCommandHandler))]
    internal class when_an_application_has_a_property : DomainServiceSpec<DoLightStoneValuationForWorkFlowCommand, DoLightStoneValuationForWorkFlowCommandHandler>
    {
        protected static IPropertyRepository propertyRepository;

        // Arrange
        Establish context = () =>
        {
            propertyRepository = An<IPropertyRepository>();
            IProperty property = An<IProperty>();
            propertyRepository.WhenToldTo(x => x.GetLightStoneValuationForWorkFlow(Param<int>.IsAnything, Param<string>.IsAnything));
            command = new DoLightStoneValuationForWorkFlowCommand(Param<int>.IsAnything, Param<string>.IsAnything);
            handler = new DoLightStoneValuationForWorkFlowCommandHandler(propertyRepository);
        };

        // Act
        Because of = () =>
        {
            handler.Handle(messages, command);
        };

        // Assert
        It should_do_valuation = () =>
        {
            propertyRepository.WasToldTo(x => x.GetLightStoneValuationForWorkFlow(Param<int>.IsAnything, Param<string>.IsAnything));
        };
    }
}