using System.Collections.Generic;
using DomainService2.Workflow.Origination.ApplicationManagement;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Globals;

namespace DomainService2.Specs.Workflow.Origination.ApplicationManagement.SetAccountStatusToApplicationPriorToInstructAttorneyCommandHandlerSpecs
{
    [Subject(typeof(SetAccountStatusToApplicationPriorToInstructAttorneyCommandHandler))]
    public class When_an_account_exists_for_an_application : WithFakes
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
                IAccount mainAccount = An<IAccount>();

                // hoc
                IAccount hocAccount = An<IAccount>();
                IProduct hocProduct = An<IProduct>();
                hocProduct.WhenToldTo(x => x.Key).Return((int)Products.HomeOwnersCover);
                hocAccount.WhenToldTo(x => x.Product).Return(hocProduct);

                // life
                IAccount lifeAccount = An<IAccount>();
                IProduct lifeProduct = An<IProduct>();
                lifeProduct.WhenToldTo(x => x.Key).Return((int)Products.LifePolicy);
                lifeAccount.WhenToldTo(x => x.Product).Return(lifeProduct);

                // MainAccount
                IEventList<IAccount> childAccounts = new EventList<IAccount>(new List<IAccount> { hocAccount, lifeAccount });
                mainAccount.WhenToldTo(x => x.RelatedChildAccounts).Return(childAccounts);
                application.WhenToldTo(x => x.Account).Return(mainAccount);

                //application
                IEventList<IAccount> relatedAccounts = new EventList<IAccount>(new List<IAccount> { hocAccount, lifeAccount });
                application.WhenToldTo(x => x.RelatedAccounts).Return(relatedAccounts);

                // repos
                applicationRepository.WhenToldTo(x => x.GetApplicationByKey(Param<int>.IsAnything)).Return(application);
                applicationRepository.WhenToldTo(x => x.SaveApplication(Param<IApplication>.IsAnything));
                accountRepository.WhenToldTo(x => x.UpdateAccount(Param<int>.IsAnything, Param<int>.IsAnything, Param<float>.IsAnything, Param<string>.IsAnything));
                command = new SetAccountStatusToApplicationPriorToInstructAttorneyCommand(Param<int>.IsAnything, Param<string>.IsAnything);
                handler = new SetAccountStatusToApplicationPriorToInstructAttorneyCommandHandler(applicationRepository, accountRepository, commonRepository);
            };

        Because of = () =>
            {
                handler.Handle(messages, command);
            };

        It should_call_method_UpdateAccount = () =>
            {
                accountRepository.WasToldTo(x => x.UpdateAccount(Param<int>.IsAnything, Param<int>.IsAnything, Param<float>.IsAnything, Param<string>.IsAnything));
            };
    }
}