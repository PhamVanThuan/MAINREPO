using SAHL.Core.Rules;
using SAHL.Core.SystemMessages;
using SAHL.Services.Interfaces.DomainQuery;
using SAHL.Services.Interfaces.DomainQuery.Queries;
using SAHL.Services.Interfaces.FinanceDomain.Model;
using System.Linq;

namespace SAHL.Services.FinanceDomain.Rules
{
    public class TheInvoicesAccountNumberMustBeAValidSAHLAccountRule : IDomainRule<IAccountRuleModel>
    {
        private IDomainQueryServiceClient DomainQueryServiceClient;

        public TheInvoicesAccountNumberMustBeAValidSAHLAccountRule(IDomainQueryServiceClient domainQueryServiceClient)
        {
            this.DomainQueryServiceClient = domainQueryServiceClient;
        }

        public void ExecuteRule(ISystemMessageCollection messages, IAccountRuleModel ruleModel)
        {
            var query = new GetAccountByAccountNumberQuery(ruleModel.AccountNumber);
            messages.Aggregate(DomainQueryServiceClient.PerformQuery(query));

            if (query.Result != null && query.Result.Results != null && query.Result.Results.Count() == 0)
            {
                messages.AddMessage(new SystemMessage(
                    string.Format("Account Number {0} is not a valid SA Home Loans account number.", ruleModel.AccountNumber), SystemMessageSeverityEnum.Error));
            }
        }
    }
}