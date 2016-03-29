using System.Linq;
using DomainService2.Specs.DomainObjects;
using DomainService2.Workflow.Origination.FurtherLending;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;

namespace DomainService2.Specs.Workflow.Origination.FurtherLending.RemoveDetailTypesCommandHandlerSpecs
{
    [Subject(typeof(RemoveDetailTypesCommandHandler))]
    public class When_there_are_valid_detailtypes_to_remove_from_a_furtherloan : RemoveDetailTypesCommandHandlerBase
    {
        static IEventList<IDetail> details;
        static IAccount account;
        Establish context = () =>
            {
                IApplication app = An<IApplication>();
                IApplicationType appType = An<IApplicationType>();
                account = An<IAccount>();
                details = new StubEventList<IDetail>();
                IDetail furtherLoaninProgressDetail = An<IDetail>();
                IDetail clientWantsToNTUDetail = An<IDetail>();
                IDetailType furtherLoaninProgressDetailType = An<IDetailType>();
                IDetailType clientWantsToNTUDetailType = An<IDetailType>();

                messages = new DomainMessageCollection();

                applicationRepository = An<IApplicationRepository>();
                applicationRepository.WhenToldTo(x => x.GetApplicationByKey(Param.IsAny<int>())).
                    Return(app);

                accountRepository = An<IAccountRepository>();

                app.WhenToldTo(x => x.Account).
                    Return(account);

                app.WhenToldTo(x => x.ApplicationType).
                    Return(appType);

                appType.WhenToldTo(x => x.Key).
                    Return((int)SAHL.Common.Globals.OfferTypes.FurtherLoan);

                // setup the returned detail types
                details.Add(messages, furtherLoaninProgressDetail);
                furtherLoaninProgressDetail.WhenToldTo(x => x.DetailType).
                    Return(furtherLoaninProgressDetailType);

                furtherLoaninProgressDetailType.WhenToldTo(x => x.Key).
                    Return((int)SAHL.Common.Globals.DetailTypes.FurtherLoanInProgress);

                details.Add(messages, clientWantsToNTUDetail);
                clientWantsToNTUDetail.WhenToldTo(x => x.DetailType).
                    Return(clientWantsToNTUDetailType);

                clientWantsToNTUDetailType.WhenToldTo(x => x.Key).
                    Return((int)SAHL.Common.Globals.DetailTypes.ClientWantsToNTU);

                account.WhenToldTo(x => x.Details).
                    Return(details);

                command = new RemoveDetailTypesCommand(0);
                handler = new RemoveDetailTypesCommandHandler(applicationRepository, accountRepository);
            };

        Because of = () =>
            {
                handler.Handle(messages, command);
            };

        It should_remove_FurtherLoanInProgress = () =>
            {
                // are there any Furtherloans in progress left
                details.Any(x => x.DetailType.Key == (int)SAHL.Common.Globals.DetailTypes.FurtherLoanInProgress).ShouldBeFalse();
            };

        It should_remove_ClientWantsToNTU = () =>
            {
                // are there any clientwants to NTU left
                details.Any(x => x.DetailType.Key == (int)SAHL.Common.Globals.DetailTypes.ClientWantsToNTU).ShouldBeFalse();
            };

        It should_save_the_account = () =>
            {
                accountRepository.WasToldTo(x => x.SaveAccount(account));
            };
    }
}