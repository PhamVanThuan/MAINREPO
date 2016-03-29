﻿using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.SystemMessages;
using SAHL.Services.FinancialDomain.Managers;
using SAHL.Services.FinancialDomain.Rules;
using SAHL.Services.Interfaces.FinancialDomain.Commands;
using System.Linq;

namespace SAHL.Services.FinancialDomain.Specs.RuleSpecs.MustBeANewPurchaseMortgageLoanApplication
{
    public class when_pricing_an_application_that_is_a_new_mortgageloan : WithFakes
    {
        private static ApplicationMustBeANewBusinessMortgageLoanWhenPricingRule rule;
        private static IFinancialDataManager financialDataManager;
        private static ISystemMessageCollection messages;
        private static int applicationNumber;
        private static PriceNewBusinessApplicationCommand command;

        private Establish context = () =>
        {
            applicationNumber = 12345;
            messages = SystemMessageCollection.Empty();
            financialDataManager = An<IFinancialDataManager>();
            OfferDataModel newBusinessMortgageLoanOfferInfo = new OfferDataModel((int)OfferType.NewPurchaseLoan, (int)OfferStatus.Open, null, null, null, "", null, null, 12345, (int)OriginationSource.SAHomeLoans, 0);
            financialDataManager.WhenToldTo(x => x.GetApplication(applicationNumber)).Return(newBusinessMortgageLoanOfferInfo);
            rule = new ApplicationMustBeANewBusinessMortgageLoanWhenPricingRule(financialDataManager);

            command = new PriceNewBusinessApplicationCommand(applicationNumber);
        };

        private Because of = () =>
        {
            rule.ExecuteRule(messages, command);
        };

        private It should_not_contain_an_error_message = () =>
        {
            messages.ErrorMessages().Count().ShouldEqual(0);
        };
    }
}