using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.Interfaces.DomainQuery;
using SAHL.Services.Interfaces.DomainQuery.Model;
using SAHL.Services.Interfaces.DomainQuery.Queries;
using SAHL.Services.Interfaces.LifeDomain.Models;
using SAHL.Services.LifeDomain.Managers;
using SAHL.Services.LifeDomain.Rules;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.Services.LifeDomain.Specs.RuleSpecs.DisabilityClaimAuthorisedInstalmentsCannotExceedBondTerm
{
    public class when_the_authorised_instalments_is_less_than_bond_term : WithFakes
    {
        private static DisabilityClaimAuthorisedInstalmentsCannotExceedBondTermRule rule;
        private static ILifeDomainDataManager lifeDomainDataManager;
        private static IDomainQueryServiceClient domainQueryClient;
        private static AuthorisedInstalmentsBondTermTestModel ruleModel;
        private static ISystemMessageCollection messages;
        private static int authorisedInstalments;
        private static int loanRemainingInstalments;
        private static int loanNumber;
        private static int debitOrderDay;
        private static DateTime paymentStartDate;
        private static DateTime paymentEndDate;

        private Establish context = () =>
        {
            messages = SystemMessageCollection.Empty();

            loanNumber = 333333;
            debitOrderDay = 25;
            loanRemainingInstalments = 25;
            authorisedInstalments = 10;
            paymentStartDate = new DateTime(2015, 01, debitOrderDay);
            paymentEndDate = paymentStartDate.AddMonths(authorisedInstalments);

            lifeDomainDataManager = An<ILifeDomainDataManager>();
            domainQueryClient = An<IDomainQueryServiceClient>();

            rule = new DisabilityClaimAuthorisedInstalmentsCannotExceedBondTermRule(lifeDomainDataManager, domainQueryClient);

            ruleModel = new AuthorisedInstalmentsBondTermTestModel(loanNumber, paymentStartDate, paymentEndDate);
   
            domainQueryClient.WhenToldTo(c => c.PerformQuery(Param.IsAny<GetMortgageLoanDetailsQuery>())).Return<GetMortgageLoanDetailsQuery>(y =>
            {
                y.Result = new ServiceQueryResult<GetMortgageLoanDetailsQueryResult>(new GetMortgageLoanDetailsQueryResult[] {
                            new GetMortgageLoanDetailsQueryResult{ AccountKey = loanNumber, RemainingInstalments = loanRemainingInstalments} }
                            );
                return SystemMessageCollection.Empty();
            });

        };

        private Because of = () =>
        {
            rule.ExecuteRule(messages, ruleModel);
        };

        private It should_not_return_a_message = () =>
        {
            messages.ErrorMessages().ShouldBeEmpty();
        };
    }
}