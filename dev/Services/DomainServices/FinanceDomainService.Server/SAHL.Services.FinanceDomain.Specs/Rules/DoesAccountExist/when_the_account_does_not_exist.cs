using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.FinanceDomain.Rules;
using SAHL.Services.FinanceDomain.Specs.Rules.DoesAccountExist;
using SAHL.Services.Interfaces.DocumentManager.Models;
using SAHL.Services.Interfaces.DomainQuery;
using SAHL.Services.Interfaces.DomainQuery.Model;
using SAHL.Services.Interfaces.DomainQuery.Queries;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.Services.FinanceDomain.Specs.Rules
{
    public class when_the_account_does_not_exist : WithFakes
    {
        private static TheInvoicesAccountNumberMustBeAValidSAHLAccountRule doesAccountExistRule;
        private static ISystemMessageCollection messages;
        private static IDomainQueryServiceClient domainQueryService;
        private static AccountRuleModel accountRuleModel;

        private Establish context = () =>
        {
            accountRuleModel = new AccountRuleModel(1408282);
            domainQueryService = An<IDomainQueryServiceClient>();
            messages = SystemMessageCollection.Empty();
            doesAccountExistRule = new TheInvoicesAccountNumberMustBeAValidSAHLAccountRule(domainQueryService);
            domainQueryService.WhenToldTo(x => x.PerformQuery(Param.IsAny<GetAccountByAccountNumberQuery>())).
                Return<GetAccountByAccountNumberQuery>(y =>
                {
                    y.Result = new ServiceQueryResult<GetAccountByAccountNumberQueryResult>(new List<GetAccountByAccountNumberQueryResult> { }); return SystemMessageCollection.Empty();
                });
        };

        private Because of = () =>
        {
            doesAccountExistRule.ExecuteRule(messages, accountRuleModel);
        };

        private It should_try_find_the_account_using_the_query_service = () =>
        {
            domainQueryService.WasToldTo(x => x.PerformQuery(Param<GetAccountByAccountNumberQuery>.Matches(y => y.AccountNumber == accountRuleModel.AccountNumber)));
        };

        private It should_return_error_messages = () =>
        {
            messages.ErrorMessages().First().Message.ShouldEqual(string.Format(@"Account Number {0} is not a valid SA Home Loans account number.",
                accountRuleModel.AccountNumber));
        };
    }
}