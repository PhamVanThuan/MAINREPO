using DomainService2.Workflow.Origination.ApplicationManagement;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections.Interfaces;

namespace DomainService2.Specs.Workflow.Origination.ApplicationManagement.SetAccountStatusToApplicationPriorToInstructAttorneyCommandHandlerSpecs
{
    [Subject(typeof(SetAccountStatusToApplicationPriorToInstructAttorneyCommandHandler))]
    public class When_no_account_exists_for_an_application : WithFakes
    {
        protected static SetAccountStatusToApplicationPriorToInstructAttorneyCommand command;
        protected static SetAccountStatusToApplicationPriorToInstructAttorneyCommandHandler handler;
        protected static IDomainMessageCollection messages;
        protected static IApplicationRepository applicationRepository;
        protected static IAccountRepository accountRepository;
        protected static ICommonRepository commonRepository;

        Establish context = () =>
        {
            commonRepository = An<ICommonRepository>();
            messages = An<IDomainMessageCollection>();
            applicationRepository = An<IApplicationRepository>();
            accountRepository = An<IAccountRepository>();

            IApplication application = An<IApplication>();
            IAccount mainAccount = null;

            // MainAccount
            application.WhenToldTo(x => x.Account).Return(mainAccount);

            // repos
            applicationRepository.WhenToldTo(x => x.GetApplicationByKey(Param<int>.IsAnything)).Return(application);

            command = new SetAccountStatusToApplicationPriorToInstructAttorneyCommand(Param<int>.IsAnything, Param<string>.IsAnything);
            handler = new SetAccountStatusToApplicationPriorToInstructAttorneyCommandHandler(applicationRepository, accountRepository, commonRepository);
        };

        Because of = () =>
        {
            handler.Handle(messages, command);
        };

        It should_not_UpdateAccount = () =>
        {
            accountRepository.WasNotToldTo(x => x.UpdateAccount(Param<int>.IsAnything, Param<int>.IsAnything, Param<float>.IsAnything, Param<string>.IsAnything));
        };
    }
}