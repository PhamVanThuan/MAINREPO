using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.DecisionTree.Shared.Globals;
using SAHL.Services.Capitec.Managers.Lookup;
using SAHL.Services.Interfaces.Capitec.Models;
using SAHL.Services.Interfaces.Capitec.Queries;
using SAHL.Services.Interfaces.DecisionTree;
using SAHL.Services.Interfaces.DecisionTree.Models;
using SAHL.Services.Interfaces.DecisionTree.Queries;
using System.Linq;

namespace SAHL.Services.Capitec.QueryHandlers
{
    public class CalculateApplicationScenarioNewPurchaseQueryHandler : IServiceQueryHandler<CalculateApplicationScenarioNewPurchaseQuery>
    {
        private IDecisionTreeServiceClient decisionTreeService;
        private ILookupManager lookupService;

        public CalculateApplicationScenarioNewPurchaseQueryHandler(IDecisionTreeServiceClient decisionTreeService, ILookupManager lookupService)
        {
            this.decisionTreeService = decisionTreeService;
            this.lookupService = lookupService;
        }

        public ISystemMessageCollection HandleQuery(CalculateApplicationScenarioNewPurchaseQuery query)
        {
            ISystemMessageCollection messages = SystemMessageCollection.Empty();

            var applicationType = new Enumerations.SAHomeLoans.MortgageLoanApplicationType().NewPurchase;

            var deposit = query.LoanDetails.Deposit;
            var interimInterest = 0;
            var capitaliseFees = query.LoanDetails.CapitaliseFees;
            var fees = query.LoanDetails.Fees;
            var householdIncome = query.LoanDetails.HouseholdIncome;
            var householdIncomeType = lookupService.GetDecisionTreeHouseholdIncomeType(query.LoanDetails.IncomeType);
            var occupancyType = lookupService.GetDecisionTreeOccupancyType(query.LoanDetails.OccupancyType);
            var propertyPurchasePrice = query.LoanDetails.PurchasePrice;

            var termInMonths = SAHL.Services.Interfaces.Capitec.Common.CalculatorConstants.CalculatorTermInMonths;

            CapitecOriginationCreditPricing_Query treeQuery = new CapitecOriginationCreditPricing_Query(
                applicationType,
                occupancyType,
                householdIncomeType,
                (double)householdIncome,
                (double)propertyPurchasePrice,
                (double)deposit,
                -1, -1, -1,
                -1, -1,
                termInMonths,
                -999,
                -1,
                -999,
                -1,
                true,
                (double)fees,
                (double)interimInterest,
                capitaliseFees);

            try
            {
                var decisionTreeMessages = decisionTreeService.PerformQuery(treeQuery);

                var queryResult = new CalculateApplicationScenarioQueryResult();
                CapitecOriginationCreditPricing_QueryResult treeResult;
                if (treeQuery.Result != null)
                {
                    treeResult = treeQuery.Result.Results.SingleOrDefault();
                    queryResult.EligibleApplication = treeResult.EligibleApplication;
                    if (queryResult.EligibleApplication)
                    {
                        queryResult.InterestRate = (decimal)treeResult.InterestRate;
                        queryResult.Instalment = (decimal)treeResult.Instalment;
                        queryResult.LoanAmount = (decimal)treeResult.LoanAmount;
                        queryResult.LTV = (decimal)treeResult.LoantoValue;
                        queryResult.PTI = (decimal)treeResult.PaymenttoIncome;

                        queryResult.InterestRateAsPercent = treeResult.InterestRateasPercent;
                        queryResult.LTVAsPercent = treeResult.LoantoValueasPercent;
                        queryResult.PTIAsPercent = treeResult.PaymenttoIncomeasPercent;
                        queryResult.TermInMonths = termInMonths;
                    }
                }
                else
                {
                    messages.AddMessage(new SystemMessage("An error occurred while running the Capitec Origination Credit Pricing Decision Tree.", SystemMessageSeverityEnum.Error));
                    queryResult.EligibleApplication = false;
                }
                queryResult.DecisionTreeMessages = SystemMessageCollection.Empty();
                queryResult.DecisionTreeMessages.AddMessages(decisionTreeMessages.WarningMessages());
                queryResult.DecisionTreeMessages.AddMessages(decisionTreeMessages.InfoMessages());
                query.Result = queryResult;
            }
            catch
            {
                messages.AddMessage(new SystemMessage("Could not contact the Capitec Origination Credit Pricing Decision Tree.", SystemMessageSeverityEnum.Error));
            }

            return messages;
        }
    }
}