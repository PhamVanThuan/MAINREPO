using DomainService2.SharedServices.Common;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;

namespace DomainService2.Specs.SharedServices.Common.CheckPropertyExistsCommandHandlerSpecs
{
    [Subject(typeof(CheckPropertyExistsCommandHandler))]
    public class when_an_application_has_no_property : DomainServiceSpec<CheckPropertyExistsCommand, CheckPropertyExistsCommandHandler>
    {
        protected static IPropertyRepository propertyRepository;

        // Arrange
        Establish context = () =>
        {
            propertyRepository = An<IPropertyRepository>();
            IProperty property = null;
            propertyRepository.WhenToldTo(x => x.GetPropertyByApplicationKey(Param<int>.IsAnything)).Return(property);
            command = new CheckPropertyExistsCommand(Param<int>.IsAnything);
            handler = new CheckPropertyExistsCommandHandler(propertyRepository);
        };

        // Act
        Because of = () =>
        {
            handler.Handle(messages, command);
        };

        // Assert
        It should_return_false = () =>
        {
            command.Result.ShouldBeFalse();
        };
    }
}