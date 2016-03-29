using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Events;
using SAHL.Core.Rules;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Core.Testing;
using SAHL.Services.ApplicationDomain.CommandHandlers;
using SAHL.Services.ApplicationDomain.Managers.Application;
using SAHL.Services.Interfaces.ApplicationDomain.Commands;
using SAHL.Services.Interfaces.ApplicationDomain.Models;
using SAHL.Services.Interfaces.DomainQuery;
using System;
using System.Linq;

namespace SAHL.Services.ApplicationDomain.Specs.CommandHandlersSpec.AddApplication.Switch
{
    public class when_executing_rules_returns_errors : WithCoreFakes
    {
        private static AddSwitchApplicationCommand command;
        private static AddSwitchApplicationCommandHandler handler;
        private static IApplicationDataManager applicationDataManager;        
        private static IDomainRuleManager<SwitchApplicationModel> domainRuleContext;
        private static SwitchApplicationModel switchApplicationModel;
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
            domainRuleContext = An<IDomainRuleManager<SwitchApplicationModel>>();
            domainQueryService = An<IDomainQueryServiceClient>();

            //new up handler
            handler = new AddSwitchApplicationCommandHandler(serviceCommandRouter, applicationDataManager, linkedKeyManager, unitOfWorkFactory, eventRaiser, applicationManager, domainRuleContext);

            //new up command
            switchApplicationModel = new SwitchApplicationModel(OfferStatus.Open, 1, OriginationSource.Comcorp, 250000, 1000000, 240, 65000, Product.NewVariableLoan, "reference1", 1);

            command = new AddSwitchApplicationCommand(switchApplicationModel, Guid.NewGuid());

            //rules must fail
            domainRuleContext.
             WhenToldTo
              (x => x.ExecuteRules(Param.IsAny<ISystemMessageCollection>(), switchApplicationModel))
                .Callback<ISystemMessageCollection>
                 (y => y.AddMessage(new SystemMessage("rule error messages", SystemMessageSeverityEnum.Error)));
        };

        private Because of = () =>
        {
            messages = handler.HandleCommand(command, serviceRequestMetaData);
        };

        private It should_not_return_error_messages = () =>
        {
            messages.ErrorMessages().Count().ShouldNotEqual(0);
        };

        private It should_not_get_a_reserved_account_number_for_the_application = () =>
        {
            applicationDataManager.WasNotToldTo(x => x.GetReservedAccountNumber());
        };

        private It should_not_save_the_application = () =>
        {
            applicationManager.
             WasNotToldTo
              (x => x.SaveApplication(switchApplicationModel.ApplicationType, switchApplicationModel.ApplicationStatus, Param.IsAny<DateTime>(), 
                switchApplicationModel.ApplicationSourceKey, Param.IsAny<int>(), switchApplicationModel.OriginationSource, switchApplicationModel.Reference, 
                switchApplicationModel.ApplicantCount));
        };

        private It should_not_link_the_application_to_the_guid_provided = () =>
        {
            linkedKeyManager.WasNotToldTo(x => x.LinkKeyToGuid(applicationNumber, applicationId));
        };

        private It should_not_save_the_application_mortgage_loan = () =>
        {
            applicationManager.WasNotToldTo(x => x.SaveApplicationMortgageLoan(applicationNumber, MortgageLoanPurpose.Switchloan, command.SwitchApplicationModel.ApplicantCount, null, 
                command.SwitchApplicationModel.EstimatedPropertyValue, null));
        };

        private It should_not_save_the_application_information = () =>
        {
            applicationManager.
             WasNotToldTo
              (x => x.SaveApplicationInformation(Param.IsAny<DateTime>(), applicationNumber, OfferInformationType.OriginalOffer, switchApplicationModel.Product));
        };

        private It should_not_save_the_application_information_variable_loan = () =>
        {
            applicationManager.
             WasNotToldTo
              (x => x.SaveSwitchApplicationInformationVariableLoan(applicationInformationKey, command.SwitchApplicationModel.Term, 
                command.SwitchApplicationModel.ExistingLoan, command.SwitchApplicationModel.EstimatedPropertyValue, command.SwitchApplicationModel.CashOut, 
                command.SwitchApplicationModel.LoanAmountNoFees));
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
            eventRaiser.WasNotToldTo
             (x => x.RaiseEvent(Param.IsAny<DateTime>(), Param.IsAny<Event>(),
                    Param.IsAny<int>(), Param.IsAny<int>(), Param.IsAny<IServiceRequestMetadata>()));
        };
    }
}
