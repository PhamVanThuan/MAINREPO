using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Rules;
using SAHL.Core.Services;
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
    public class when_adding_a_valid_refinance_application : WithCoreFakes
    {
        private static int applicationNumber = 555;
        private static Guid applicationId = Guid.NewGuid();
        private static int applicationInformationKey = 333;
        private static AddRefinanceApplicationCommand command;
        private static AddRefinanceApplicationCommandHandler handler;
        private static IApplicationDataManager applicationDataManager;
        private static IApplicationManager applicationManager;
        private static RefinanceApplicationModel refinanceApplicationModel;
        private static IDomainQueryServiceClient domainQueryService;
        private static IDomainRuleManager<RefinanceApplicationModel> ruleContext;

        private Establish context = () =>
        {
            refinanceApplicationModel = new RefinanceApplicationModel(OfferStatus.Open, 1, OriginationSource.Comcorp, 800000, 240, 120000, Product.NewVariableLoan, "reference1", 1);

            applicationManager = An<IApplicationManager>();
            applicationDataManager = An<IApplicationDataManager>();
            domainQueryService = An<IDomainQueryServiceClient>();
            ruleContext = An<IDomainRuleManager<RefinanceApplicationModel>>();

            applicationManager.WhenToldTo(x => x.SaveApplication(refinanceApplicationModel.ApplicationType, refinanceApplicationModel.ApplicationStatus, Param.IsAny<DateTime>(),
                refinanceApplicationModel.ApplicationSourceKey, Param.IsAny<int>(), OriginationSource.SAHomeLoans, refinanceApplicationModel.Reference, refinanceApplicationModel.ApplicantCount))
                .Return(applicationNumber);
            serviceCommandRouter.WhenToldTo(x => x.HandleCommand(Param.IsAny<IServiceCommand>(), serviceRequestMetaData)).Return(messages);
            applicationManager.WhenToldTo(x => x.SaveApplicationInformation(Param.IsAny<DateTime>(), applicationNumber, OfferInformationType.OriginalOffer, refinanceApplicationModel.Product))
                .Return(applicationInformationKey);

            command = new AddRefinanceApplicationCommand(refinanceApplicationModel, applicationId);
            handler = new AddRefinanceApplicationCommandHandler(serviceCommandRouter, applicationDataManager, linkedKeyManager, unitOfWorkFactory, eventRaiser, applicationManager, ruleContext);
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
            applicationManager.WasToldTo(x => x.SaveApplication(refinanceApplicationModel.ApplicationType, refinanceApplicationModel.ApplicationStatus, Param.IsAny<DateTime>(), 
                refinanceApplicationModel.ApplicationSourceKey, Param.IsAny<int>(), OriginationSource.SAHomeLoans, refinanceApplicationModel.Reference, refinanceApplicationModel.ApplicantCount));
        };

        private It should_link_the_application_to_the_guid_provided = () =>
        {
            linkedKeyManager.WasToldTo(x => x.LinkKeyToGuid(applicationNumber, applicationId));
        };

        private It should_save_the_application_mortgage_loan = () =>
        {
            applicationManager.WasToldTo(x => x.SaveApplicationMortgageLoan(applicationNumber, MortgageLoanPurpose.Refinance, command.RefinanceApplicationModel.ApplicantCount, null, 
                command.RefinanceApplicationModel.EstimatedPropertyValue, null));
        };

        private It should_save_the_application_information = () =>
        {
            applicationManager.WasToldTo(x => x.SaveApplicationInformation(Param.IsAny<DateTime>(), applicationNumber, OfferInformationType.OriginalOffer, refinanceApplicationModel.Product));
        };

        private It should_save_the_application_information_variable_loan = () =>
        {
            applicationManager.WasToldTo(x => x.SaveRefinanceApplicationInformationVariableLoan(applicationInformationKey, command.RefinanceApplicationModel.Term, 
                command.RefinanceApplicationModel.EstimatedPropertyValue, command.RefinanceApplicationModel.CashOut, command.RefinanceApplicationModel.LoanAmountNoFees));
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