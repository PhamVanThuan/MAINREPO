using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.SystemMessages;
using SAHL.Services.FinancialDomain.Managers;
using SAHL.Services.FinancialDomain.Rules;
using SAHL.Services.Interfaces.FinancialDomain.Commands;
using System;
using System.Linq;

namespace SAHL.Services.FinancialDomain.Specs.RuleSpecs.ApplicationMayNotBeAccepted
{
    public class when_pricing_an_application_that_is_not_accepted : WithFakes
    {
        private static ApplicationMayNotBeAcceptedWhenPricingNewBusinessApplicationRule rule;
        private static IFinancialDataManager financialDataManager;
        private static ISystemMessageCollection messages;
        private static int applicationNumber;
        private static PriceNewBusinessApplicationCommand command;

        private Establish context = () =>
        {
            applicationNumber = 12345;
            messages = SystemMessageCollection.Empty();
            financialDataManager = An<IFinancialDataManager>();
            OfferInformationDataModel notAcceptedOfferInfo = new OfferInformationDataModel(DateTime.Now, applicationNumber, (int)OfferInformationType.OriginalOffer, "someone", null, null);
            financialDataManager.WhenToldTo(x => x.GetLatestApplicationOfferInformation(applicationNumber)).Return(notAcceptedOfferInfo);
            rule = new ApplicationMayNotBeAcceptedWhenPricingNewBusinessApplicationRule(financialDataManager);

            command = new PriceNewBusinessApplicationCommand(applicationNumber);
        };

        private Because of = () =>
        {
            rule.ExecuteRule(messages, command);
        };

        private It should_contain_the_correct_error_message = () =>
        {
            messages.ErrorMessages().Count().ShouldEqual(0);
        };
    }
}