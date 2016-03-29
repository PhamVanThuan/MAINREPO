using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.SystemMessages;
using SAHL.DomainServiceChecks.CheckHandlers;
using SAHL.DomainServiceChecks.Managers.ThirdPartyInvoiceDataManager;
using System;
using System.Linq;

namespace SAHL.DomainServiceCheck.Specs.CheckHandlers.RequiresThirdParty
{
    public class when_the_third_party_exists : WithFakes
    {
        private static ThirdPartyCommand command;
        private static RequiresThirdPartyHandler handler;
        private static IThirdPartyInvoiceDataManager dataManager;
        private static Guid thirdPartyId;
        private static ISystemMessageCollection messages;

        private Establish context = () =>
        {
            messages = SystemMessageCollection.Empty();
            command = new ThirdPartyCommand(Guid.NewGuid(), Guid.NewGuid());
            dataManager = An<IThirdPartyInvoiceDataManager>();
            dataManager.WhenToldTo(x => x.DoesThirdPartyExist(Arg.Any<Guid>())).Return(true);
            handler = new RequiresThirdPartyHandler(dataManager);
        };

        private Because of = () =>
        {
            messages = handler.HandleCheckCommand(command);
        };

        private It should_not_return_an_error_message = () =>
        {
            messages.ErrorMessages().Count().ShouldEqual(0);
        };

        private It should_use_the_command_third_party_id_when_checking = () =>
        {
            dataManager.Received().DoesThirdPartyExist(command.ThirdPartyId);
        };
    }
}