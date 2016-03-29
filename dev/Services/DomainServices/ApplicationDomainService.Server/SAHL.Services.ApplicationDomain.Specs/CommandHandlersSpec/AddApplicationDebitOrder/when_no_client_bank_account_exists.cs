using System;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Events;
using SAHL.Core.Rules;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Core.Testing;
using SAHL.Services.ApplicationDomain.CommandHandlers;
using SAHL.Services.ApplicationDomain.Managers.Applicant;
using SAHL.Services.ApplicationDomain.Managers.Application;
using SAHL.Services.Interfaces.ApplicationDomain.Commands;
using SAHL.Services.Interfaces.ApplicationDomain.Models;
using SAHL.Services.Interfaces.DomainQuery;
using SAHL.Services.Interfaces.DomainQuery.Model;
using SAHL.Services.Interfaces.DomainQuery.Queries;

namespace SAHL.Services.ApplicationDomain.Specs.CommandHandlersSpec.AddApplicationDebitOrder
{
    public class when_no_client_bank_account_exists : WithCoreFakes
    {
        private static IApplicationDataManager applicationDataManager;
        private static IApplicantDataManager applicantDataManager;
        private static IDomainQueryServiceClient domainQueryService;
        private static IDomainRuleManager<ApplicationDebitOrderModel> domainRuleContext;
        private static ApplicationDebitOrderModel applicationDebitOrderModel;
        private static AddApplicationDebitOrderCommand command;
        private static AddApplicationDebitOrderCommandHandler handler;
        private static int expectedApplicationNumber;
        private static int expectedBankAccountKey;

        private Establish context = () =>
        {
            applicationDataManager = An<IApplicationDataManager>();
            applicantDataManager = An<IApplicantDataManager>();
            domainQueryService = An<IDomainQueryServiceClient>();
            expectedApplicationNumber = 12346;
            expectedBankAccountKey = 3234;
            domainRuleContext = An<IDomainRuleManager<ApplicationDebitOrderModel>>();

            applicationDebitOrderModel = new ApplicationDebitOrderModel(expectedApplicationNumber, 28, expectedBankAccountKey);
            command = new AddApplicationDebitOrderCommand(applicationDebitOrderModel);
            handler = new AddApplicationDebitOrderCommandHandler(applicationDataManager, domainRuleContext, domainQueryService, eventRaiser, applicantDataManager, unitOfWorkFactory);

            var getClientBankAccountQueryResult = new ServiceQueryResult<GetClientBankAccountQueryResult>(new GetClientBankAccountQueryResult[] { });
            domainQueryService.WhenToldTo(c => c.PerformQuery(Param.IsAny<GetClientBankAccountQuery>())).Return<GetClientBankAccountQuery>(y =>
            {
                y.Result = getClientBankAccountQueryResult;
                return SystemMessageCollection.Empty();
            });
        };

        private Because of = () =>
        {
            messages = handler.HandleCommand(command, serviceRequestMetaData);
        };

        private It should_not_get_the_client_bank_accounts = () =>
        {
            domainQueryService.WasToldTo(x => x.PerformQuery(Param.IsAny<GetClientBankAccountQuery>()));
        };

        private It should_not_save_the_application_debit_order = () =>
        {
            applicationDataManager.WasNotToldTo(x => x.SaveApplicationDebitOrder(applicationDebitOrderModel, expectedBankAccountKey));
        };

        private It should_not_Raise_an_event = () =>
        {
            eventRaiser.WasNotToldTo(x => x.RaiseEvent(Param.IsAny<DateTime>(), Param.IsAny<IEvent>()
                , Param.IsAny<int>(), Param.IsAny<int>(), Param.IsAny<IServiceRequestMetadata>()));
        };
    }
}