using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Rules;
using SAHL.Core.Services;
using SAHL.Core.Testing;
using SAHL.Services.FinancialDomain.CommandHandlers;
using SAHL.Services.FinancialDomain.Managers;
using SAHL.Services.Interfaces.FinancialDomain.Commands;
using SAHL.Services.Interfaces.FinancialDomain.Models;
using System;

namespace SAHL.Services.FinancialDomain.Specs.CommandHandlersSpec.FundNewBusinessApplication
{
    public class when_funding_given_accepted_offer : WithCoreFakes
    {
        private static FundNewBusinessApplicationCommand command;
        private static FundNewBusinessApplicationCommandHandler handler;

        private static int applicationNumber;
        private static IFinancialDataManager financialDataManager;
        private static IDomainRuleManager<IApplicationModel> domainRuleManager;
        private static IServiceQueryRouter serviceQueryRouter;
        private static OfferDataModel application;

        private Establish context = () =>
        {
            applicationNumber = 1;
            command = new FundNewBusinessApplicationCommand(applicationNumber);
            domainRuleManager = new DomainRuleManager<IApplicationModel>();
            financialDataManager = An<IFinancialDataManager>();
            serviceQueryRouter = An<IServiceQueryRouter>();
            application = new OfferDataModel(applicationNumber, (int)OfferType.NewPurchaseLoan, null, null, null, "", null, null, 12345, (int)OriginationSource.SAHomeLoans, null);

            var offerInformationDataModel = new OfferInformationDataModel(DateTime.Today.AddDays(-1), applicationNumber, (int)OfferInformationType.AcceptedOffer, "user", null, null);
            financialDataManager.WhenToldTo(x => x.GetLatestApplicationOfferInformation(applicationNumber)).Return(offerInformationDataModel);
            financialDataManager.WhenToldTo(x => x.GetApplication(applicationNumber)).Return(application);

            handler = new FundNewBusinessApplicationCommandHandler(serviceQueryRouter, financialDataManager, An<IFinancialManager>(), eventRaiser, unitOfWorkFactory, domainRuleManager);
        };

        private Because of = () =>
        {
            messages = handler.HandleCommand(command, serviceRequestMetaData);
        };

        private It should_check_if_the_appplications_latest_application_information_is_not_accepted = () =>
        {
            financialDataManager.WasToldTo(x => x.GetLatestApplicationOfferInformation(applicationNumber));
        };

        private It should_not_determine_SPV_for_the_application = () =>
        {
            financialDataManager.WasNotToldTo(x => x.GetValidSPV(Param.IsAny<decimal>(), Param.IsAny<string>()));
        };

        private It should_return_error_message = () =>
        {
            messages.HasErrors.ShouldBeTrue();
            messages.ErrorMessages().ShouldContain(x => x.Message.Contains("Funding cannot be determined once the application has been accepted"));
        };
    }
}