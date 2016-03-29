using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Rules;
using SAHL.Core.Testing;
using SAHL.Services.ApplicationDomain.CommandHandlers;
using SAHL.Services.ApplicationDomain.Managers.Application;
using SAHL.Services.Interfaces.ApplicationDomain.Commands;
using SAHL.Services.Interfaces.ApplicationDomain.Models;
using System;

namespace SAHL.Services.ApplicationDomain.Specs.CommandHandlersSpec.AddApplication.Switch
{
    public class when_adding_a_valid_switch_application : WithCoreFakes
    {
        private static int applicationNumber = 555;
        private static Guid applicationId = Guid.NewGuid();
        private static int applicationInformationKey = 333;
        private static AddSwitchApplicationCommand command;
        private static AddSwitchApplicationCommandHandler handler;
        private static IApplicationDataManager applicationDataManager;
        private static IApplicationManager applicationManager;
        private static SwitchApplicationModel switchApplicationModel;
        private static IDomainRuleManager<SwitchApplicationModel> ruleContext;

        private Establish context = () =>
        {
            switchApplicationModel = new SwitchApplicationModel(OfferStatus.Open, 1, OriginationSource.SAHomeLoans, 420000, 800000, 240, 120000, Product.NewVariableLoan, "reference1", 1);

            applicationManager = An<IApplicationManager>();
            applicationDataManager = An<IApplicationDataManager>();
            ruleContext = An<IDomainRuleManager<SwitchApplicationModel>>();

            applicationManager.
              WhenToldTo
               (x => x.SaveApplication(switchApplicationModel.ApplicationType, switchApplicationModel.ApplicationStatus, Param.IsAny<DateTime>(), 
               switchApplicationModel.ApplicationSourceKey, Param.IsAny<int>(), switchApplicationModel.OriginationSource, switchApplicationModel.Reference, switchApplicationModel.ApplicantCount))
                .Return(applicationNumber);

            applicationManager.
              WhenToldTo
               (x => x.SaveApplicationInformation(Param.IsAny<DateTime>(), applicationNumber, OfferInformationType.OriginalOffer, switchApplicationModel.Product))
                .Return(applicationInformationKey);

            command = new AddSwitchApplicationCommand(switchApplicationModel, applicationId);
            handler = new AddSwitchApplicationCommandHandler(serviceCommandRouter, applicationDataManager, linkedKeyManager, unitOfWorkFactory, eventRaiser, applicationManager, ruleContext);
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
            applicationManager
             .WasToldTo
              (x => x.SaveApplication(switchApplicationModel.ApplicationType, switchApplicationModel.ApplicationStatus, Param.IsAny<DateTime>(), 
               switchApplicationModel.ApplicationSourceKey, Param.IsAny<int>(), switchApplicationModel.OriginationSource, switchApplicationModel.Reference, switchApplicationModel.ApplicantCount));
        };

        private It should_link_the_application_to_the_guid_provided = () =>
        {
            linkedKeyManager.WasToldTo(x => x.LinkKeyToGuid(applicationNumber, applicationId));
        };

        private It should_save_the_application_mortgage_loan = () =>
        {
            applicationManager.WasToldTo(x => x.SaveApplicationMortgageLoan(applicationNumber, MortgageLoanPurpose.Switchloan, command.SwitchApplicationModel.ApplicantCount, null, 
                command.SwitchApplicationModel.EstimatedPropertyValue, null));
        };

        private It should_save_the_application_information = () =>
        {
            applicationManager.WasToldTo(x => x.SaveApplicationInformation(Param.IsAny<DateTime>(), applicationNumber, OfferInformationType.OriginalOffer, switchApplicationModel.Product));
        };

        private It should_save_the_application_information_variable_loan = () =>
        {
            applicationManager.WasToldTo
             (x => x.SaveSwitchApplicationInformationVariableLoan(applicationInformationKey, command.SwitchApplicationModel.Term, 
                command.SwitchApplicationModel.ExistingLoan, command.SwitchApplicationModel.EstimatedPropertyValue, command.SwitchApplicationModel.CashOut, 
                command.SwitchApplicationModel.LoanAmountNoFees));
        };

        private It should_save_the_application_information_interest_only = () =>
        {
            applicationManager.WasToldTo(x => x.SaveApplicationInformationInterestOnly(applicationInformationKey));
        };

        private It should_save_an_offer_information_quick_cash_record = () =>
        {
            applicationManager.WasToldTo(x => x.SaveApplicationInformationQuickCash(applicationInformationKey));
        };

        private It should_not_have_any_errors = () =>
        {
            messages.HasErrors.ShouldBeFalse();
        };
    }
}