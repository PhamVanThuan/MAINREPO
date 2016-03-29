using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.SystemMessages;
using SAHL.DomainServiceChecks.CheckHandlers;
using SAHL.DomainServiceChecks.Checks;
using SAHL.DomainServiceChecks.Managers.AccountDataManager;

namespace SAHL.DomainServiceCheck.Specs.CheckHandlers.RequiresAccount
{
    public class when_an_account_does_exist_in_our_system : WithFakes
    {
        private static IAccountDataManager accountDataManager;
        private static IRequiresAccount check;
        private static RequiresAccountHandler handler;
        private static ISystemMessageCollection systemMessages;

        private Establish context = () =>
        {
            accountDataManager = An<IAccountDataManager>();
            check = An<IRequiresAccount>();
            handler = new RequiresAccountHandler(accountDataManager);
            accountDataManager.WhenToldTo(x => x.DoesAccountExist(check.AccountKey)).Return(true);
        };

        private Because of = () =>
        {
            systemMessages = handler.HandleCheckCommand(check);
        };

        private It should_check_if_the_account_exists = () =>
        {
            accountDataManager.WasToldTo(x => x.DoesAccountExist(check.AccountKey));
        };

        private It should_not_contain_error_messages = () =>
        {
            systemMessages.ErrorMessages().ShouldBeEmpty();
        };
    }
}