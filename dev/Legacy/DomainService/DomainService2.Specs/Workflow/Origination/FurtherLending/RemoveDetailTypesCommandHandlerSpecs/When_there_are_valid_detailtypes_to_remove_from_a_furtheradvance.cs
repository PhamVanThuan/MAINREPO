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
    public class When_there_are_valid_detailtypes_to_remove_from_a_furtheradvance : RemoveDetailTypesCommandHandlerBase
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
            IDetail readvanceInProgressDetail1 = An<IDetail>();
            IDetail readvanceInProgressDetail2 = An<IDetail>();
            IDetailType readvanceInProgressDetailType1 = An<IDetailType>();
            IDetailType readvanceInProgressDetailType2 = An<IDetailType>();

            messages = new DomainMessageCollection();

            applicationRepository = An<IApplicationRepository>();
            applicationRepository.WhenToldTo(x => x.GetApplicationByKey(Param.IsAny<int>())).
                Return(app);

            accountRepository = An<IAccountRepository>();

            command = new RemoveDetailTypesCommand(1111);
            handler = new RemoveDetailTypesCommandHandler(applicationRepository, accountRepository);

            app.WhenToldTo(x => x.Account).
                Return(account);

            app.WhenToldTo(x => x.ApplicationType).
                Return(appType);

            appType.WhenToldTo(x => x.Key).
                Return((int)SAHL.Common.Globals.OfferTypes.FurtherAdvance);

            // setup the returned detail types

            // Further Loan In Progress
            details.Add(messages, furtherLoaninProgressDetail);
            furtherLoaninProgressDetail.WhenToldTo(x => x.DetailType).
                Return(furtherLoaninProgressDetailType);

            furtherLoaninProgressDetailType.WhenToldTo(x => x.Key).
                Return((int)SAHL.Common.Globals.DetailTypes.FurtherLoanInProgress);

            // Client Wants To NTU
            details.Add(messages, clientWantsToNTUDetail);
            clientWantsToNTUDetail.WhenToldTo(x => x.DetailType).
                Return(clientWantsToNTUDetailType);

            clientWantsToNTUDetailType.WhenToldTo(x => x.Key).
                Return((int)SAHL.Common.Globals.DetailTypes.ClientWantsToNTU);

            // Readvance in progress 1
            details.Add(messages, readvanceInProgressDetail1);
            readvanceInProgressDetail1.WhenToldTo(x => x.DetailType).
                Return(readvanceInProgressDetailType1);

            readvanceInProgressDetailType1.WhenToldTo(x => x.Key).
                Return((int)SAHL.Common.Globals.DetailTypes.ReadvanceInProgress);

            // Readvance in progress 2
            details.Add(messages, readvanceInProgressDetail2);
            readvanceInProgressDetail2.WhenToldTo(x => x.DetailType).
                Return(readvanceInProgressDetailType2);

            readvanceInProgressDetailType2.WhenToldTo(x => x.Key).
                Return((int)SAHL.Common.Globals.DetailTypes.ReadvanceInProgress);

            readvanceInProgressDetail2.WhenToldTo(x => x.Description)
                .Return(command.ApplicationKey.ToString());

            // Return the details list from the account
            account.WhenToldTo(x => x.Details).
                Return(details);
        };

        Because of = () =>
        {
            handler.Handle(messages, command);
        };

        It should_remove_ReadvanceInProgress_if_the_detailtype_description_equals_the_application_key = () =>
        {
            // are there any Furtherloans in progress left
            details.Where(x => x.DetailType.Key == (int)SAHL.Common.Globals.DetailTypes.ReadvanceInProgress && x.Description != command.ApplicationKey.ToString()).Count().ShouldEqual<int>(1);
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