using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data;
using SAHL.Core.Data.Models._2AM.Managers;
using SAHL.Core.Events;
using SAHL.Core.Rules;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Core.Testing;
using SAHL.Services.ApplicationDomain.CommandHandlers;
using SAHL.Services.ApplicationDomain.Managers.Application;
using SAHL.Services.Interfaces.ApplicationDomain.Commands;
using SAHL.Services.Interfaces.ApplicationDomain.Events;
using SAHL.Services.Interfaces.ApplicationDomain.Models;
using SAHL.Services.Interfaces.DomainQuery;
using System;
using System.Collections.Generic;

namespace SAHL.Services.ApplicationDomain.Specs.CommandHandlersSpec.AddApplication.NewPurchase
{
    public class when_adding_a_valid_new_purchase_application : WithCoreFakes
    {
        private static int applicationNumber = 555;
        private static Guid applicationId = Guid.NewGuid();
        private static int applicationInformationKey = 333;
        private static AddNewPurchaseApplicationCommand command;
        private static AddNewPurchaseApplicationCommandHandler handler;
        private static IApplicationDataManager applicationDataManager;
        private static NewPurchaseApplicationModel newPurchaseApplicationModel;
        private static IApplicationManager applicationManager;
        private static IDomainQueryServiceClient domainQueryService;
        private static IDomainRuleManager<NewPurchaseApplicationModel> ruleContext;

        private Establish context = () =>
        {
            messages = new SystemMessageCollection();
            newPurchaseApplicationModel = new NewPurchaseApplicationModel(OfferStatus.Open, 1, OriginationSource.Comcorp, 100000, 550000, 550000, 240, Product.NewVariableLoan, 
                "reference1", 1, null);
            unitOfWorkFactory = An<IUnitOfWorkFactory>();
            unitOfWorkFactory.WhenToldTo(x => x.Build()).Return(An<IUnitOfWork>());
            serviceCommandRouter = An<IServiceCommandRouter>();
            applicationDataManager = An<IApplicationDataManager>();
            linkedKeyManager = An<ILinkedKeyManager>();
            eventRaiser = An<IEventRaiser>();
            applicationManager = An<IApplicationManager>();
            domainQueryService = An<IDomainQueryServiceClient>();
            ruleContext = An<IDomainRuleManager<NewPurchaseApplicationModel>>();

            applicationManager.WhenToldTo(x => x.SaveApplication(Param.IsAny<OfferType>(), Param.IsAny<OfferStatus>(), Param.IsAny<DateTime>(), Param.IsAny<int>(), Param.IsAny<int>(),
                Param.IsAny<OriginationSource>(), Param.IsAny<string>(), Param.IsAny<int>())).Return(applicationNumber);
            applicationManager.WhenToldTo(x => x.SaveApplicationInformation(Param.IsAny<DateTime>(), applicationNumber, Param.IsAny<OfferInformationType>(), Param.IsAny<Product>()))
                .Return(applicationInformationKey);

            command = new AddNewPurchaseApplicationCommand(newPurchaseApplicationModel, applicationId);
            handler = new AddNewPurchaseApplicationCommandHandler(serviceCommandRouter, applicationDataManager, linkedKeyManager, eventRaiser, unitOfWorkFactory, applicationManager, ruleContext);
        };

        private Because of = () =>
        {
            messages = handler.HandleCommand(command, serviceRequestMetaData);
        };

        private It should_get_a_reserved_account_number_for_the_application = () =>
        {
            applicationDataManager.WasToldTo(x => x.GetReservedAccountNumber());
        };

        private It should_save_the_application = () =>
        {
            applicationManager.WasToldTo(x => x.SaveApplication(OfferType.NewPurchaseLoan, OfferStatus.Open, Param.IsAny<DateTime>(), Param.IsAny<int>(),
                Param.IsAny<int>(), OriginationSource.SAHomeLoans, Param.IsAny<string>(), Param.IsAny<int>()));
        };

        private It should_link_the_application_to_the_guid_provided = () =>
        {
            linkedKeyManager.WasToldTo(x => x.LinkKeyToGuid(applicationNumber, applicationId));
        };

        private It should_save_the_application_mortgage_loan = () =>
        {
            applicationManager.WasToldTo(x => x.SaveApplicationMortgageLoan(applicationNumber, MortgageLoanPurpose.Newpurchase, Param.IsAny<int>(), Param.IsAny<decimal>(), null, 
                Param.IsAny<string>()));
        };

        private It should_save_the_application_information = () =>
        {
            applicationManager.WasToldTo(x => x.SaveApplicationInformation(Param.IsAny<DateTime>(), applicationNumber, OfferInformationType.OriginalOffer, Product.NewVariableLoan));
        };

        private It should_save_the_application_information_variable_loan = () =>
        {
            applicationManager.WasToldTo(x => x.SaveNewPurchaseApplicationInformationVariableLoan(applicationInformationKey, command.NewPurchaseApplication.Term,
                command.NewPurchaseApplication.Deposit, command.NewPurchaseApplication.PurchasePrice, command.NewPurchaseApplication.LoanAmountNoFees));
        };

        private It should_save_the_application_information_interest_only = () =>
        {
            applicationManager.WasToldTo(x => x.SaveApplicationInformationInterestOnly(applicationInformationKey));
        };

        private It should_not_have_any_errors = () =>
        {
            messages.HasErrors.ShouldBeFalse();
        };

        private It should_raise_an_event = () =>
        {
            eventRaiser.WasToldTo(x => x.RaiseEvent(Param.IsAny<DateTime>(), Param.IsAny<NewPurchaseApplicationAddedEvent>(), Param.IsAny<int>(), Param.IsAny<int>(), 
                Param.IsAny<IServiceRequestMetadata>()));
        };
    }
}