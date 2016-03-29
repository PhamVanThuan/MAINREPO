using SAHL.Common.Collections.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces.Repositories
{
    /// <summary>
    /// #19048
    /// </summary>
    public interface ICreditCriteriaUnsecuredLendingRepository
    {
		ICreditCriteriaUnsecuredLending GetCreditCriteriaUnsecuredLendingByKey(int key);
        IReadOnlyEventList<ICreditCriteriaUnsecuredLending> GetCreditCriteriaUnsecuredLendingByLoanAmount(double LoanAmount);
		IReadOnlyEventList<ICreditCriteriaUnsecuredLending> GetCreditCriteriaUnsecuredLendingList();
        ICreditCriteriaUnsecuredLending GetCreditCriteriaUnsecuredLendingByLoanAmountAndTerm(double loanAmount, int term);
    }
}