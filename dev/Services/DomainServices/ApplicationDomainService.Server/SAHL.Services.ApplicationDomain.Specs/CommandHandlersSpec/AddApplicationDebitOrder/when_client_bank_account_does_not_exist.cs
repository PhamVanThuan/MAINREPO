using System;
using System.Collections.Generic;
using System.Linq;
using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.Rules;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Core.Testing;
using SAHL.Services.ApplicationDomain.CommandHandlers;
using SAHL.Services.ApplicationDomain.Managers.Applicant;
using SAHL.Services.ApplicationDomain.Managers.Application;
using SAHL.Services.Interfaces.ApplicationDomain.Commands;
using SAHL.Services.Interfaces.ApplicationDomain.Events;
using SAHL.Services.Interfaces.ApplicationDomain.Models;
using SAHL.Services.Interfaces.DomainQuery;
using SAHL.Services.Interfaces.DomainQuery.Model;
using SAHL.Services.Interfaces.DomainQuery.Queries;

namespace SAHL.Services.ApplicationDomain.Specs.CommandHandlersSpec.AddApplicationDebitOrder
{
    public class when_client_bank_account_does_not_exist : WithCoreFakes
    {
        private static IApplicantDataManager applicantDataManager;
        private static AddApplicationDebitOrderCommand command;
        private static AddApplicationDebitOrderCommandHandler handler;
        private static ApplicationDebitOrderModel applicationDebitOrderModel;
        private static IApplicationDataManager dataManager;
        private static IDomainRuleManager<ApplicationDebitOrderModel> domainRuleManager;
        private static IDomainQueryServiceClient domainQueryService;
        private static GetClientBankAccountQuery getClientBankAccountQuery;
        private static int validDebitOrderDay = 28;
        private static int expectedApplicationNumber = 1234567;
        private static int expectedBankAccountKey = 4567894;
        private static int expectedClientBankAccountKey = 789546244;
        private static IEnumerable<GetClientBankAccountQueryResult> result;

        private Establish context = () =>
        {
            domainQueryService = An<IDomainQueryServiceClient>();
            applicantDataManager = An<IApplicantDataManager>();
            result = new List<GetClientBankAccountQueryResult>() { new GetClientBankAccountQueryResult() { BankAccountKey = 0 } };
            getClientBankAccountQuery = new GetClientBankAccountQuery(expectedClientBankAccountKey);
            getClientBankAccountQuery.Result = new ServiceQueryResult<GetClientBankAccountQueryResult>(result);
            domainQueryService.WhenToldTo(c => c.PerformQuery(Param.IsAny<GetClientBankAccountQuery>())).Return<GetClientBankAccountQuery>(y =>
            {
                y.Result = getClientBankAccountQuery.Result;
                return SystemMessageCollection.Empty();
            });

            dataManager = An<IApplicationDataManager>();
            dataManager.WhenToldTo(x => x.DoesApplicationExist(expectedApplicationNumber)).Return(true);

            applicationDebitOrderModel = new ApplicationDebitOrderModel(expectedApplicationNumber, validDebitOrderDay, expectedClientBankAccountKey);

            domainRuleManager = An<IDomainRuleManager<ApplicationDebitOrderModel>>();
            domainRuleManager.WhenToldTo(x => x.ExecuteRules(messages, applicationDebitOrderModel));

            command = new AddApplicationDebitOrderCommand(applicationDebitOrderModel);
            handler = new AddApplicationDebitOrderCommandHandler(dataManager, domainRuleManager, domainQueryService, eventRaiser, applicantDataManager, unitOfWorkFactory);
        };

        private Because of = () =>
        {
            messages = handler.HandleCommand(command, serviceRequestMetaData);
        };

        private It should_check_if_client_bank_account_exists = () =>
        {
            domainQueryService.WasToldTo(x => x.PerformQuery(Arg.Is<GetClientBankAccountQuery>(y => y.ClientBankAccountKey == command.ApplicationDebitOrderModel.ClientBankAccountKey)));
        };

        private It should_not_save_the_application_debit_order = () =>
        {
            dataManager.WasNotToldTo(x => x.SaveApplicationDebitOrder(applicationDebitOrderModel, expectedBankAccountKey));
        };

        private It should_not_raise_a_debit_order_added_to_application_event = () =>
        {
            eventRaiser.WasNotToldTo(x => x.RaiseEvent(Param.IsAny<DateTime>(), Param.IsAny<DebitOrderAddedToApplicationEvent>()
                , Param.IsAny<int>(), Param.IsAny<int>(), Param.IsAny<IServiceRequestMetadata>()));
        };

        private It should_return_a_message = () =>
        {
            messages.ErrorMessages().First().Message.ShouldEqual("The client bank account provided does not exist.");
        };
    }
}