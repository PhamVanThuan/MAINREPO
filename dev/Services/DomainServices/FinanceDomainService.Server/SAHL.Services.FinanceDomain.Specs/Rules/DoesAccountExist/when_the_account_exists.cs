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
    public class when_the_account_exists : WithFakes
    {
        private static TheInvoicesAccountNumberMustBeAValidSAHLAccountRule doesAccountExistRule;
        private static ISystemMessageCollection messages;
        private static IDomainQueryServiceClient domainQueryService;
        private static AccountRuleModel accountRuleModel;
        private static int accountNumber;

        private Establish context = () =>
        {
            accountNumber = 1408282;
            accountRuleModel = new AccountRuleModel(accountNumber);
            domainQueryService = An<IDomainQueryServiceClient>();
            messages = SystemMessageCollection.Empty();
            GetAccountByAccountNumberQueryResult account = new GetAccountByAccountNumberQueryResult
            {
                AccountKey = accountNumber,
                AccountStatusKey = 1,
                ChangeDate = DateTime.Now,
                CloseDate = DateTime.Now,
                FixedPayment = 0,
                InsertedDate = DateTime.Now,
                OpenDate = DateTime.Now,
                OriginationSourceProductKey = 1
            };
            doesAccountExistRule = new TheInvoicesAccountNumberMustBeAValidSAHLAccountRule(domainQueryService);
            domainQueryService.WhenToldTo(x => x.PerformQuery(Param.IsAny<GetAccountByAccountNumberQuery>())).
                Return<GetAccountByAccountNumberQuery>(y =>
                {
                    y.Result = new ServiceQueryResult<GetAccountByAccountNumberQueryResult>(new List<GetAccountByAccountNumberQueryResult> { account }); return SystemMessageCollection.Empty();
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

        private It should_not_return_any_error_messages = () =>
        {
            messages.ErrorMessages().ShouldBeEmpty();
        };
    }
}