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

namespace SAHL.Services.ApplicationDomain.Specs.CommandHandlersSpec.AddApplication.Refinance
{
    public class when_executing_rules_returns_errors : WithCoreFakes
    {
        private static AddRefinanceApplicationCommand command;
        private static AddRefinanceApplicationCommandHandler handler;
        private static IApplicationDataManager applicationDataManager;        
        private static IDomainRuleManager<RefinanceApplicationModel> domainRuleContext;
        private static RefinanceApplicationModel refinanceApplicationModel;
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
            domainRuleContext = An<IDomainRuleManager<RefinanceApplicationModel>>();
            domainQueryService = An<IDomainQueryServiceClient>();

            //new up handler
            handler = new AddRefinanceApplicationCommandHandler(serviceCommandRouter, applicationDataManager, linkedKeyManager, unitOfWorkFactory, eventRaiser, applicationManager, domainRuleContext);

            //new up command
            refinanceApplicationModel = new RefinanceApplicationModel(OfferStatus.Open, 1, OriginationSource.Comcorp, 800000, 240, 120000, Product.NewVariableLoan, "reference1", 1);

            command = new AddRefinanceApplicationCommand(refinanceApplicationModel, Guid.NewGuid());

            //rules must fail
            domainRuleContext.WhenToldTo(x => x.ExecuteRules(Param.IsAny<ISystemMessageCollection>(), refinanceApplicationModel))
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
            applicationManager.WasNotToldTo(x => x.SaveApplication(refinanceApplicationModel.ApplicationType, refinanceApplicationModel.ApplicationStatus, Param.IsAny<DateTime>(), 
                refinanceApplicationModel.ApplicationSourceKey, Param.IsAny<int>(), OriginationSource.SAHomeLoans, refinanceApplicationModel.Reference, refinanceApplicationModel.ApplicantCount));
        };

        private It should_not_link_the_application_to_the_guid_provided = () =>
        {
            linkedKeyManager.WasNotToldTo(x => x.LinkKeyToGuid(applicationNumber, applicationId));
        };

        private It should_not_save_the_application_mortgage_loan = () =>
        {
            applicationManager.WasNotToldTo(x => x.SaveApplicationMortgageLoan(applicationNumber, MortgageLoanPurpose.Refinance, command.RefinanceApplicationModel.ApplicantCount, null, 
                command.RefinanceApplicationModel.EstimatedPropertyValue, null));
        };

        private It should_not_save_the_application_information = () =>
        {
            applicationManager.WasNotToldTo(x => x.SaveApplicationInformation(Param.IsAny<DateTime>(), applicationNumber, OfferInformationType.OriginalOffer, refinanceApplicationModel.Product));
        };

        private It should_not_save_the_application_information_variable_loan = () =>
        {
            applicationManager.WasNotToldTo(x => x.SaveRefinanceApplicationInformationVariableLoan(applicationInformationKey, command.RefinanceApplicationModel.Term, 
                command.RefinanceApplicationModel.EstimatedPropertyValue, command.RefinanceApplicationModel.CashOut, command.RefinanceApplicationModel.LoanAmountNoFees));
        };

        private It should_not_save_the_application_information_interest_only = () =>
        {
            applicationManager.WasNotToldTo(x => x.SaveApplicationInformationInterestOnly(applicationInformationKey));
        };

        private It should_not_save_an_offer_information_quick_cash_record = () =>
        {
            applicationManager.WasNotToldTo(x => x.SaveApplicationInformationQuickCash(applicationInformationKey));
        };

        private It should_not_raise_an_event = () =>
        {
            eventRaiser.WasNotToldTo(x => x.RaiseEvent(Param.IsAny<DateTime>(), Param.IsAny<Event>(),
                    Param.IsAny<int>(), Param.IsAny<int>(), Param.IsAny<IServiceRequestMetadata>()));
        };
    }
}
