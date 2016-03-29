using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Rules;
using SAHL.Core.SystemMessages;
using SAHL.Core.Testing;
using SAHL.Services.FinancialDomain.CommandHandlers;
using SAHL.Services.FinancialDomain.Managers;
using SAHL.Services.FinancialDomain.Rules;
using SAHL.Services.Interfaces.FinancialDomain.Commands;
using SAHL.Services.Interfaces.FinancialDomain.Models;
using SAHL.Shared.BusinessModel.Calculations;
using System;

namespace SAHL.Services.FinancialDomain.Specs.CommandHandlersSpec.PriceNewBusinessApplication
{
    public class when_pricing_an_application_given_the_application_is_accepted : WithCoreFakes
    {
        private static PriceNewBusinessApplicationCommand command;
        private static PriceNewBusinessApplicationCommandHandler handler;
        private static IFinancialDataManager financialDataManager;
        private static IDomainRuleManager<IApplicationModel> domainRuleContext;
        private static IApplicationCalculator applicationCalculator;
        private static IFinancialManager financialManager;
        private static ILoanCalculations functionsUtils;

        private static int applicationNumber;
        private static int reservedAccountKey;
        private static OfferDataModel application;
        private static OfferInformationDataModel applicationInformation;

        private Establish context = () =>
        {
            domainRuleContext = new DomainRuleManager<IApplicationModel>();            
            applicationCalculator = An<IApplicationCalculator>();
            financialDataManager = An<IFinancialDataManager>();
            financialManager = An<IFinancialManager>();
            functionsUtils = An<ILoanCalculations>();
            messages = SystemMessageCollection.Empty();

            applicationNumber = 1;
            reservedAccountKey = 1;

            application = new OfferDataModel((int)OfferType.NewPurchaseLoan, (int)OfferStatus.Open, DateTime.Now, null, null, null, null, null, reservedAccountKey, (int)OriginationSource.SAHomeLoans, null);
            financialDataManager.WhenToldTo(x => x.GetApplication(applicationNumber)).Return(application);

            applicationInformation = new OfferInformationDataModel(DateTime.Now, applicationNumber, (int)OfferInformationType.AcceptedOffer, "System", DateTime.Now, (int)Product.NewVariableLoan);
            financialDataManager.WhenToldTo(x => x.GetLatestApplicationOfferInformation(applicationNumber)).Return(applicationInformation);            

            command = new PriceNewBusinessApplicationCommand(applicationNumber);
            handler = new PriceNewBusinessApplicationCommandHandler(financialDataManager, unitOfWorkFactory, eventRaiser,
                applicationCalculator, domainRuleContext, financialManager, functionsUtils);
        };

        private Because of = () =>
        {
            messages = handler.HandleCommand(command, serviceRequestMetaData);
        };                

        private It should_return_error_message = () =>
        {
            messages.HasErrors.ShouldBeTrue();
            messages.ErrorMessages().ShouldContain(x => x.Message.Contains(ApplicationMayNotBeAcceptedWhenPricingNewBusinessApplicationRule.ErrorMessage));
        };
    }
}