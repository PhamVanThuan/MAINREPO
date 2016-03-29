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
    public class when_unit_of_work_is_provided: WithCoreFakes
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
        private static int expectedOfferDebitOrderKey = 222222;

        private Establish context = () =>
        {
            domainQueryService = An<IDomainQueryServiceClient>();
            applicantDataManager = An<IApplicantDataManager>();
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

            dataManager = An<IApplicationDataManager>();
            dataManager.WhenToldTo(x => x.DoesApplicationExist(expectedApplicationNumber)).Return(true);

            applicationDebitOrderModel = new ApplicationDebitOrderModel(expectedApplicationNumber, validDebitOrderDay, expectedClientBankAccountKey);

            domainRuleManager = An<IDomainRuleManager<ApplicationDebitOrderModel>>();
            domainRuleManager.WhenToldTo(x => x.ExecuteRules(messages, applicationDebitOrderModel));

            command = new AddApplicationDebitOrderCommand(applicationDebitOrderModel);
            handler = new AddApplicationDebitOrderCommandHandler(dataManager, domainRuleManager, domainQueryService, eventRaiser, applicantDataManager, unitOfWorkFactory);

            dataManager.WhenToldTo(x => x.SaveApplicationDebitOrder(Param.IsAny<ApplicationDebitOrderModel>(), Param.IsAny<int>())).Return(expectedOfferDebitOrderKey);
        };

        private Because of = () =>
        {
            messages = handler.HandleCommand(command, serviceRequestMetaData);
        };

        private It should_create_unit_of_work = () =>
        {
            unitOfWorkFactory.WasToldTo(x => x.Build());
        };

        private It should_complete_unit_of_work = () =>
        {
            unitOfWork.WasToldTo(x => x.Complete());
        };
    }

}
