using System;
using System.Collections.Generic;
using System.Linq;
using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.Data.Models._2AM;
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
    public class when_an_app_debit_order_exists : WithCoreFakes
    {
        private static AddApplicationDebitOrderCommand command;
        private static AddApplicationDebitOrderCommandHandler handler;
        private static ApplicationDebitOrderModel applicationDebitOrderModel;
        private static IApplicationDataManager applicationDataManager;
        private static IApplicantDataManager applicantDataManager;
        private static IDomainRuleManager<ApplicationDebitOrderModel> domainRuleManager;
        private static IDomainQueryServiceClient domainQueryService;
        private static GetClientBankAccountQuery getClientBankAccountQuery;
        private static int validDebitOrderDay = 28;
        private static int expectedApplicationNumber = 1234567;
        private static int expectedBankAccountKey = 4567894;
        private static int expectedClientBankAccountKey = 789546244;
        private static IEnumerable<OfferDebitOrderDataModel> existingDebitOrders;

        private Establish context = () =>
        {
            applicantDataManager = An<IApplicantDataManager>();
            domainQueryService = An<IDomainQueryServiceClient>();
            existingDebitOrders = new OfferDebitOrderDataModel[] { new OfferDebitOrderDataModel(1, 2, 0, 28, 1) };
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
            applicationDebitOrderModel = new ApplicationDebitOrderModel(expectedApplicationNumber, validDebitOrderDay, expectedClientBankAccountKey);
            applicationDataManager.WhenToldTo(x => x.DoesApplicationExist(Param.IsAny<int>())).Return(true);
            applicationDataManager.WhenToldTo(x => x.GetApplicationDebitOrder(Param.IsAny<int>())).Return(existingDebitOrders);

            domainRuleManager = An<IDomainRuleManager<ApplicationDebitOrderModel>>();
            domainRuleManager.WhenToldTo(x => x.ExecuteRules(messages, applicationDebitOrderModel));

            command = new AddApplicationDebitOrderCommand(applicationDebitOrderModel);
            handler = new AddApplicationDebitOrderCommandHandler(applicationDataManager, domainRuleManager, domainQueryService, eventRaiser, applicantDataManager, unitOfWorkFactory);

            applicantDataManager.WhenToldTo(x => x.DoesBankAccountBelongToApplicantOnApplication(applicationDebitOrderModel.ApplicationNumber, applicationDebitOrderModel.ClientBankAccountKey))
                .Return(true);
        };

        private Because of = () =>
        {
            messages = handler.HandleCommand(command, serviceRequestMetaData);
        };

        private It should_check_if_there_are_any_existing_debit_orders = () =>
        {
            applicationDataManager.WasToldTo(x => x.GetApplicationDebitOrder(applicationDebitOrderModel.ApplicationNumber));
        };

        private It should_contain_warning_messages = () =>
        {
            messages.ErrorMessages().Any().ShouldBeTrue();
        };

        private It should_get_the_details_of_the_client_bank_account = () =>
        {
            domainQueryService.WasToldTo(x => x.PerformQuery(Arg.Is<GetClientBankAccountQuery>(y =>
                y.ClientBankAccountKey == command.ApplicationDebitOrderModel.ClientBankAccountKey
                )
            ));
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