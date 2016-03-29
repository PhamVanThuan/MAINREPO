using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.Interfaces.Capitec.Models;
using SAHL.Services.Interfaces.Capitec.Queries;
using SAHL.Services.Interfaces.DecisionTree;
using SAHL.Services.Interfaces.DecisionTree.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Services.Capitec.QueryHandlers
{
    public class GetAffordabilityInterestRateQueryHandler : IServiceQueryHandler<GetAffordabilityInterestRateQuery>
    {
        private IDecisionTreeServiceClient decisionTreeService;
        public GetAffordabilityInterestRateQueryHandler(IDecisionTreeServiceClient decisionTreeService)
        {
            this.decisionTreeService = decisionTreeService;
        }
        public ISystemMessageCollection HandleQuery(GetAffordabilityInterestRateQuery query)
        {
            var messages = SystemMessageCollection.Empty();

            CapitecAffordabilityInterestRate_Query treeQuery = new CapitecAffordabilityInterestRate_Query(
                (double)query.HouseholdIncome,
                (double)query.Deposit,
                (double)query.CalcRate,
                query.InterestRateQuery);
            messages = decisionTreeService.PerformQuery(treeQuery);

            var treeResult = treeQuery.Result.Results.SingleOrDefault();
            if(messages.HasErrors)
            {               
                return messages;
            }

            query.Result = new GetAffordabilityInterestRateQueryResult();
            query.Result.InterestRate = (decimal)treeResult.InterestRate;
            query.Result.Instalment = (decimal)treeResult.Instalment;
            query.Result.PaymentToIncome = (decimal)treeResult.PaymentToIncome;
            query.Result.PropertyPriceQualifiedFor = (decimal)treeResult.PropertyPriceQualifiedFor;
            query.Result.TermInMonths = treeResult.TermInMonths;
            query.Result.AmountQualifiedFor = (decimal)treeResult.AmountQualifiedFor;

            return messages;
        }
    }
}
