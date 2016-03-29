using SAHL.Common.CacheData;
using SAHL.Common.DomainMessages;
using SAHL.Common.Security;
using SAHL.Core.SystemMessages;
using SAHL.DecisionTree.Shared;
using SAHL.Services.Interfaces.DecisionTree;
using SAHL.Services.Interfaces.DecisionTree.Models;
using SAHL.Services.Interfaces.DecisionTree.Queries;
using SAHL.V3.Framework.Model;
using System;
using System.Linq;

namespace SAHL.V3.Framework.Services
{
    public class DecisionTreeService : IDecisionTreeService
    {
        private IDecisionTreeServiceClient decisionTreeServiceClient;

        public DecisionTreeService(IDecisionTreeServiceClient decisionTreeServiceClient)
        {
            this.decisionTreeServiceClient = decisionTreeServiceClient;
        }

        public IDecisionTreeServiceClient DecisionTreeServiceClient
        {
            get { return this.decisionTreeServiceClient; }
        }

        public void QualifyApplicationFor30YearLoanTerm(QualifyApplicationFor30YearLoanTermQuery queryModel)
        {
            var treeQuery = new ThirtyYearMortgageLoanEligibility_Query(queryModel.DisqualifiedByCredit,
                queryModel.Product.ToString(),
                queryModel.HighestIncomeContributor.SalaryType.ToString(),
                queryModel.HighestIncomeContributor.Age,
                (double)queryModel.HouseholdIncome,
                queryModel.HighestIncomeContributor.CreditScore,
                (double)queryModel.LTV,
                (double)queryModel.PTI,
                queryModel.ApplicationType.ToString(),
                (double)queryModel.LoanAmount,
                (double)queryModel.PropertyValue,
                (double)queryModel.EffectiveRate,
                queryModel.HighestIncomeContributor.FullName,
                queryModel.HighestIncomeContributor.IdNumber,
                queryModel.IsAlphaHousingApplication,
                queryModel.IsInterestOnly,
                null);
            ISystemMessageCollection dtMessages = null;
            try
            {
                dtMessages = DecisionTreeServiceClient.PerformQuery(treeQuery);
                queryModel.SetTreeResult(treeQuery.Result.Results.SingleOrDefault(),dtMessages);
            }
            catch (Exception ex)
            {
                SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(SAHLPrincipal.GetCurrent());
                AddMessagesToSPC("30 Year Loan Term", dtMessages, ex);
                throw;
            }
        }


        private static void AddMessagesToSPC(string TreeName, ISystemMessageCollection dtMessages, Exception ex)
        {
            SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(SAHLPrincipal.GetCurrent());

            if (dtMessages != null && dtMessages.AllMessages.Count() > 0)
            {
                var errorMessage = string.Format("The {0} returned messages but no result.", TreeName);
                spc.DomainMessages.Add(new Error(errorMessage, errorMessage));
                foreach (var message in dtMessages.AllMessages)
                {
                    spc.DomainMessages.Add(new Error(message.Message, message.Message));
                }
            }
            else
            {
                var errorMessage = string.Format("Could not contact the {0} Decision Tree.", TreeName);
                spc.DomainMessages.Add(new Error(errorMessage, errorMessage));
                string error = String.Format("Exception: {0}", ex.ToString());
                spc.DomainMessages.Add(new Error(error, error));
            }
        }


        public void DetermineNCRGuidelineMinMonthlyFixedExpenses(DetermineNCRGuidelineMinMonthlyFixedExpensesQuery query)
        {
            var ncrTreeQuery = new NCRAffordabilityAssessment_1Query(Convert.ToDouble(query.GrossMonthlyIncome), null);

            ISystemMessageCollection messages = null;
            try
            {
                messages = DecisionTreeServiceClient.PerformQuery(ncrTreeQuery);
                query.SetResult(Convert.ToDecimal(ncrTreeQuery.Result.Results.FirstOrDefault().MinMonthlyFixedExpenses), messages);
            }
            catch (Exception ex)
            {
                AddMessagesToSPC("NCR Assessment Guideline", messages, ex);
                throw;
            }
        }
    }
}