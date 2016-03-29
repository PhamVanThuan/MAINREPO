﻿using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.SystemMessages;
using SAHL.Services.ApplicationDomain.Managers.Application;
using SAHL.Services.ApplicationDomain.Rules;
using SAHL.Services.Interfaces.ApplicationDomain.Models;
using System;
using System.Linq;

namespace SAHL.Services.ApplicationDomain.Specs.RuleSpecs
{
    public class when_the_loan_amount_is_below_the_required_for_a_refinance : WithFakes
    {
        static RefinanceApplicationModel ruleModel;
        static ISystemMessageCollection messages;
        private static IApplicationDataManager applicationDataManager;
        static RefinanceRequestedLoanAmountMustBeAboveMinimumRequiredRule rule;

        private Establish context = () =>
        {
            ruleModel = new RefinanceApplicationModel(OfferStatus.Open, 1, OriginationSource.Comcorp, 2, 2, 2, Product.NewVariableLoan, "reference1", 1);
            messages = new SystemMessageCollection();
            applicationDataManager = An<IApplicationDataManager>();
            applicationDataManager.WhenToldTo(x => x.GetMinimumLoanAmountForMortgageLoanPurpose(Param.IsAny<MortgageLoanPurpose>())).Return(100000);

            rule = new RefinanceRequestedLoanAmountMustBeAboveMinimumRequiredRule(applicationDataManager);
        };

        private Because of = () =>
        {
            rule.ExecuteRule(messages, ruleModel);
        };

        private It should_contain_error_messages = () =>
        {
            messages.ErrorMessages().Count().ShouldNotEqual(0);
        };
    }
}