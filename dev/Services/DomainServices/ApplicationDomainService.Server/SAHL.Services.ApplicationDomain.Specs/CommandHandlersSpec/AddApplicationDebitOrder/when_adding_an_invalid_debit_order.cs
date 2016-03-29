using System;
using System.Linq;
using Machine.Fakes;
using Machine.Specifications;
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
    public class when_adding_an_invalid_debit_order : WithCoreFakes
    {
        private static AddApplicationDebitOrderCommand command;
        private static AddApplicationDebitOrderCommandHandler handler;
        private static ApplicationDebitOrderModel applicationDebitOrderModel;
        private static IApplicationDataManager applicationDataManager;
        private static IDomainRuleManager<ApplicationDebitOrderModel> domainRuleManager;
        private static IDomainQueryServiceClient domainQueryService;
        private static GetClientBankAccountQuery getClientBankAccountQuery;
        private static int invalidDebitOrderDay = 29;
        private static int expectedApplicationNumber = 1234567;
        private static int expectedBankAccountKey = 4567894;
        private static int expectedClientBankAccountKey = 789546244;
        private static IApplicantDataManager applicantDataManager;

        private Establish context = () =>
        {
            domainQueryService = An<IDomainQueryServiceClient>();

            getClientBankAccountQuery = new GetClientBankAccountQuery(expectedClientBankAccountKey);
            getClientBankAccountQuery.Result = new ServiceQueryResult<GetClientBankAccountQueryResult>(
                        new GetClientBankAccountQueryResult[] {
                            new GetClientBankAccountQueryResult{BankAccountKey = expectedBankAccountKey} }
                            );
            domainQueryService.WhenToldTo(c => c.PerformQuery(Param.IsAny<GetClientBankAccountQuery>())).Return<GetClientBankAccountQuery>(y =>
            {
                y.Result = getClientBankAccountQuery.Result;
                return SystemMessageCollection.Empty();
            });

            applicationDataManager = An<IApplicationDataManager>();
            applicantDataManager = An<IApplicantDataManager>();
            applicationDebitOrderModel = new ApplicationDebitOrderModel(expectedApplicationNumber, invalidDebitOrderDay, expectedClientBankAccountKey);
            applicationDataManager.WhenToldTo(x => x.DoesApplicationExist(Param.IsAny<int>())).Return(true);
            domainRuleManager = An<IDomainRuleManager<ApplicationDebitOrderModel>>();

            domainRuleManager.WhenToldTo(x => x.ExecuteRules(Param.IsAny<ISystemMessageCollection>(), Param.IsAny<ApplicationDebitOrderModel>()))
                .Callback<ISystemMessageCollection>(x =>
                    {
                        x.AddMessage(new SystemMessage("A rule failure.", SystemMessageSeverityEnum.Error));
                    });

            command = new AddApplicationDebitOrderCommand(applicationDebitOrderModel);
            handler = new AddApplicationDebitOrderCommandHandler(applicationDataManager, domainRuleManager, domainQueryService, eventRaiser, applicantDataManager, unitOfWorkFactory);
        };

        private Because of = () =>
        {
            messages = handler.HandleCommand(command, serviceRequestMetaData);
        };

        private It should_execute_the_rules = () =>
        {
            domainRuleManager.WasToldTo(x => x.ExecuteRules(messages, applicationDebitOrderModel));
        };

        private It should_fail_with_error_messages_when_debit_order_day_is_greater_than_28 = () =>
        {
            messages.ErrorMessages().Any().ShouldBeTrue();
        };

        private It should_not_get_the_details_of_the_client_bank_account = () =>
        {
            domainQueryService.WasNotToldTo(x => x.PerformQuery(getClientBankAccountQuery));
        };

        private It should_not_save_the_application_debit_order = () =>
        {
            applicationDataManager.WasNotToldTo(x => x.SaveApplicationDebitOrder(applicationDebitOrderModel, expectedBankAccountKey));
        };

        private It should_not_raise_a_debit_order_added_to_application_event = () =>
        {
            eventRaiser.WasNotToldTo(x => x.RaiseEvent(Param.IsAny<DateTime>(), Param.IsAny<DebitOrderAddedToApplicationEvent>()
                , Param.IsAny<int>(), Param.IsAny<int>(), Param.IsAny<IServiceRequestMetadata>()));
        };
    }
}