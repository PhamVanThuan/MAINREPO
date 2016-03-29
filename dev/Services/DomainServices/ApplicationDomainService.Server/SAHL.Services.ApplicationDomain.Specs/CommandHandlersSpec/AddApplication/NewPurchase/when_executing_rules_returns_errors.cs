using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data.Models._2AM;
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
using System;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.Services.ApplicationDomain.Specs.CommandHandlersSpec.AddApplication.NewPurchase
{
    public class when_executing_rules_returns_errors : WithCoreFakes
    {
        private static AddNewPurchaseApplicationCommand command;
        private static AddNewPurchaseApplicationCommandHandler handler;
        private static IApplicationDataManager applicationDataManager;        
        private static IDomainRuleManager<NewPurchaseApplicationModel> domainRuleContext;
        private static NewPurchaseApplicationModel newPurchaseApplicationModel;
        private static IDomainQueryServiceClient domainQueryService;
        private static IApplicationManager applicationManager;
        private static int applicationNumber = 555;
        private static Guid applicationId = Guid.NewGuid();
        private static int applicationInformationKey = 333;

        private Establish context = () =>
        {
            //create mock objects
            applicationDataManager = An<IApplicationDataManager>();
            applicationManager = An<IApplicationManager>();
            domainRuleContext = An<IDomainRuleManager<NewPurchaseApplicationModel>>();
            domainQueryService = An<IDomainQueryServiceClient>();

            //new up handler
            handler = new AddNewPurchaseApplicationCommandHandler(serviceCommandRouter, applicationDataManager, linkedKeyManager, eventRaiser, unitOfWorkFactory, applicationManager, 
                domainRuleContext);

            //new up command
            newPurchaseApplicationModel = new NewPurchaseApplicationModel(OfferStatus.Open, 1, OriginationSource.Comcorp, 195000, 950000, 1000000, 240, Product.NewVariableLoan, "reference1", 
                1, null);

            command = new AddNewPurchaseApplicationCommand(newPurchaseApplicationModel, Guid.NewGuid());

            //rules must fail
            domainRuleContext.WhenToldTo(x => x.ExecuteRules(Param.IsAny<ISystemMessageCollection>(), newPurchaseApplicationModel))
                .Callback<ISystemMessageCollection>(y => y.AddMessage(new SystemMessage("rule error messages", SystemMessageSeverityEnum.Error)));
        };

        private Because of = () =>
        {
            messages = handler.HandleCommand(command, serviceRequestMetaData);
        };

        private It should_return_error_messages = () =>
        {
            messages.ErrorMessages().Count().ShouldNotEqual(0);
        };

        private It should_not_save_the_application = () =>
        {
            applicationManager.WasNotToldTo(x => x.SaveApplication(OfferType.NewPurchaseLoan, OfferStatus.Open, Param.IsAny<DateTime>(), Param.IsAny<int>(),
                Param.IsAny<int>(), OriginationSource.SAHomeLoans, Param.IsAny<string>(), Param.IsAny<int>()));
        };

        private It should_not_link_the_application_to_the_guid_provided = () =>
        {
            linkedKeyManager.WasNotToldTo(x => x.LinkKeyToGuid(applicationNumber, applicationId));
        };

        private It should_not_save_the_application_mortgage_loan = () =>
        {
            applicationManager.WasNotToldTo(x => x.SaveApplicationMortgageLoan(applicationNumber, MortgageLoanPurpose.Newpurchase, Param.IsAny<int>(), Param.IsAny<decimal>(), null, 
                Param.IsAny<string>()));
        };

        private It should_not_save_the_application_information = () =>
        {
            applicationManager.WasNotToldTo(x => x.SaveApplicationInformation(Param.IsAny<DateTime>(), applicationNumber, OfferInformationType.OriginalOffer, Product.NewVariableLoan));
        };

        private It should_not_save_the_application_information_variable_loan = () =>
        {
            applicationManager.WasNotToldTo(x => x.SaveNewPurchaseApplicationInformationVariableLoan(applicationInformationKey, command.NewPurchaseApplication.Term,
                command.NewPurchaseApplication.Deposit, command.NewPurchaseApplication.PurchasePrice, command.NewPurchaseApplication.LoanAmountNoFees));
        };

        private It should_not_save_the_application_information_interest_only = () =>
        {
            applicationManager.WasNotToldTo(x => x.SaveApplicationInformationInterestOnly(applicationInformationKey));
        };

        private It should_not_raise_an_event = () =>
        {
            eventRaiser.WasNotToldTo(x => x.RaiseEvent(Param.IsAny<DateTime>(), Param.IsAny<Event>(),
                    Param.IsAny<int>(), Param.IsAny<int>(), Param.IsAny<IServiceRequestMetadata>()));
        };
    }
}
