using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.SystemMessages;
using SAHL.DomainServiceChecks.CheckHandlers;
using SAHL.DomainServiceChecks.Checks;
using SAHL.DomainServiceChecks.Managers.PropertyDataManager;
using System.Linq;

namespace SAHL.DomainService.Check.Specs.CheckHandlers.RequiresProperty
{
    public class when_property_is_in_our_system : WithFakes
    {
        private static IPropertyDataManager propertyDataManager;
        private static int propertykey;
        private static ISystemMessageCollection systemMessages;
        private static RequiresPropertyCheckHandler handler;
        private static IRequiresProperty command;

        private Establish context = () =>
        {
            propertyDataManager = An<IPropertyDataManager>();
            command = An<IRequiresProperty>();
            handler = new RequiresPropertyCheckHandler(propertyDataManager);
            propertykey = 1001;
            command.WhenToldTo(x => x.PropertyKey).Return(propertykey);
            propertyDataManager.WhenToldTo(x => x.IsPropertyOnOurSystem(command.PropertyKey)).Return(true);
        };

        private Because of = () =>
        {
            systemMessages = handler.HandleCheckCommand(command);
        };

        private It should_check_if_property_exists = () =>
        {
            propertyDataManager.WasToldTo(x => x.IsPropertyOnOurSystem(propertykey));
        };

        private It should_not_return_any_error_messages = () =>
        {
            systemMessages.AllMessages.Count().ShouldEqual(0);
        };
    }
}