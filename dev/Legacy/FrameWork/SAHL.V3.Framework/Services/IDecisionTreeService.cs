using SAHL.Services.Interfaces.DecisionTree;
using SAHL.Services.Interfaces.DecisionTree.Models;
using SAHL.Services.Interfaces.DecisionTree.Queries;
using SAHL.V3.Framework.Model;

namespace SAHL.V3.Framework.Services
{
    public interface IDecisionTreeService : IV3Service
    {
        IDecisionTreeServiceClient DecisionTreeServiceClient { get; }

        void QualifyApplicationFor30YearLoanTerm(QualifyApplicationFor30YearLoanTermQuery queryModel);

        void DetermineNCRGuidelineMinMonthlyFixedExpenses(DetermineNCRGuidelineMinMonthlyFixedExpensesQuery query);
    }
}