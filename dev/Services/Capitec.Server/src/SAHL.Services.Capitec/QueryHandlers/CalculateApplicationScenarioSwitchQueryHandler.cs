using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.DecisionTree.Shared.Globals;
using SAHL.Services.Capitec.Managers.Lookup;
using SAHL.Services.Interfaces.Capitec.Models;
using SAHL.Services.Interfaces.Capitec.Queries;
using SAHL.Services.Interfaces.DecisionTree;
using SAHL.Services.Interfaces.DecisionTree.Models;
using SAHL.Services.Interfaces.DecisionTree.Queries;
using System;
using System.Linq;

namespace SAHL.Services.Capitec.QueryHandlers
{
    public class CalculateApplicationScenarioSwitchQueryHandler : IServiceQueryHandler<CalculateApplicationScenarioSwitchQuery>
    {
        private IDecisionTreeServiceClient decisionTreeService;
        private ILookupManager lookupService;

        public CalculateApplicationScenarioSwitchQueryHandler(IDecisionTreeServiceClient decisionTreeService, ILookupManager lookupService)
        {
            this.decisionTreeService = decisionTreeService;
            this.lookupService = lookupService;
        }

        public ISystemMessageCollection HandleQuery(CalculateApplicationScenarioSwitchQuery query)
        {
            ISystemMessageCollection messages = SystemMessageCollection.Empty();

            var applicationType = new Enumerations.SAHomeLoans.MortgageLoanApplicationType().Switch;

            bool capitaliseFees = query.LoanDetails.CapitaliseFees;
            decimal fees = query.LoanDetails.Fees;
            decimal cashRequired = query.LoanDetails.CashRequired;
            decimal currentBalance = query.LoanDetails.CurrentBalance;
            decimal estimatedMarketValueOfTheHome = query.LoanDetails.EstimatedMarketValueOfTheHome;
            var incomeType = lookupService.GetDecisionTreeHouseholdIncomeType(query.LoanDetails.IncomeType);
            decimal interimInterest = query.LoanDetails.InterimInterest;
            int mortgageLoanPurposeKey = query.LoanDetails.MortgageLoanPurposeKey;
            var occupancyType = lookupService.GetDecisionTreeOccupancyType(query.LoanDetails.OccupancyType);

            var householdIncome = query.LoanDetails.HouseholdIncome;

            var termInMonths = SAHL.Services.Interfaces.Capitec.Common.CalculatorConstants.CalculatorTermInMonths;

            CapitecOriginationCreditPricing_Query treeQuery = new CapitecOriginationCreditPricing_Query(
                applicationType,
                occupancyType,
                incomeType,
                (double)householdIncome,
                -1,
                -1,
                (double)cashRequired,
                (double)currentBalance,
                (double)estimatedMarketValueOfTheHome,
                -1,
                -1,
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
                    if (treeResult.EligibleApplication)
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
                    queryResult.EligibleApplication = false;
                    messages.AddMessage(new SystemMessage("An error occurred while running the Capitec Origination Credit Pricing Decision Tree.", SystemMessageSeverityEnum.Error));
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