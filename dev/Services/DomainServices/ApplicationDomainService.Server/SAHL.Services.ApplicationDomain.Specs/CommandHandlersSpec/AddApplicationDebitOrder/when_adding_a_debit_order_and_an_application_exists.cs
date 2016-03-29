using System;
using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Events;
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
    public class when_adding_a_debit_order_and_an_application_exists : WithCoreFakes
    {
        private static AddApplicationDebitOrderCommand command;
        private static ISystemMessageCollection systemMessageCollection;
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
        private static int expectedOfferDebitOrderKey = 222222;

        private Establish context = () =>
        {
            domainQueryService = An<IDomainQueryServiceClient>();
            eventRaiser = An<IEventRaiser>();

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

            applicantDataManager = An<IApplicantDataManager>();
            applicationDataManager = An<IApplicationDataManager>();
            applicationDataManager.WhenToldTo(x => x.DoesApplicationExist(expectedApplicationNumber)).Return(true);

            systemMessageCollection = An<ISystemMessageCollection>();
            applicationDebitOrderModel = new ApplicationDebitOrderModel(expectedApplicationNumber, validDebitOrderDay, expectedClientBankAccountKey);
            domainRuleManager = An<IDomainRuleManager<ApplicationDebitOrderModel>>();
            domainRuleManager.WhenToldTo(x => x.ExecuteRules(messages, applicationDebitOrderModel));
            command = new AddApplicationDebitOrderCommand(applicationDebitOrderModel);
            handler = new AddApplicationDebitOrderCommandHandler(applicationDataManager, domainRuleManager, domainQueryService, eventRaiser, applicantDataManager, unitOfWorkFactory);

            applicationDataManager.WhenToldTo(x => x.SaveApplicationDebitOrder(Param.IsAny<ApplicationDebitOrderModel>(), Param.IsAny<int>())).Return(expectedOfferDebitOrderKey);
        };

        private Because of = () =>
        {
            messages = handler.HandleCommand(command, serviceRequestMetaData);
        };

        private It should_retrieve_the_client_bank_account = () =>
        {
            domainQueryService.WasToldTo(x => x.PerformQuery(Arg.Is<GetClientBankAccountQuery>(
                y => y.ClientBankAccountKey == command.ApplicationDebitOrderModel.ClientBankAccountKey)));
        };

        private It should_save_the_application_debit_order = () =>
        {
            applicationDataManager.WasToldTo(x => x.SaveApplicationDebitOrder(applicationDebitOrderModel, expectedBankAccountKey));
        };

        private It should_should_raise_a_debitorder_added_to_application_event = () =>
        {
            eventRaiser.WasToldTo(x => x.RaiseEvent(Param.IsAny<DateTime>(), Arg.Is<DebitOrderAddedToApplicationEvent>
                (y => y.ApplicationNumber == expectedApplicationNumber &&
                    y.ClientBankAccountKey == expectedClientBankAccountKey),
                    expectedOfferDebitOrderKey,
                    (int)GenericKeyType.OfferDebitOrder,
                    Param.IsAny<IServiceRequestMetadata>()));
        };
    }
}