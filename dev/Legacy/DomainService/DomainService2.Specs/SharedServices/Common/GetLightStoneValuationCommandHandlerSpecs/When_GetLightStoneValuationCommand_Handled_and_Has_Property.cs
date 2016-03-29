using DomainService2.SharedServices.Common;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;

namespace DomainService2.Specs.SharedServices.Common.GetLightStoneValuationCommandHandlerSpecs
{
    [Subject(typeof(GetLightStoneValuationCommandHandler))]
    internal class When_GetLightStoneValuationCommand_Handled_and_Has_Property : DomainServiceSpec<GetLightStoneValuationCommand, GetLightStoneValuationCommandHandler>
    {
        protected static IPropertyRepository propertyRepository;
        Establish context = () =>
        {
            propertyRepository = An<IPropertyRepository>();
            IProperty property = An<IProperty>();
            propertyRepository.WhenToldTo(x => x.GetPropertyByApplicationKey(Param<int>.IsAnything)).Return(property);

            command = new GetLightStoneValuationCommand(Param<int>.IsAnything, Param<string>.IsAnything, Param<bool>.IsAnything);
            handler = new GetLightStoneValuationCommandHandler(propertyRepository);
        };

        Because of = () =>
        {
            handler.Handle(messages, command);
        };

        // Assert
        It should_return_true = () =>
        {
            command.Result.ShouldBeTrue();
        };
    }
}