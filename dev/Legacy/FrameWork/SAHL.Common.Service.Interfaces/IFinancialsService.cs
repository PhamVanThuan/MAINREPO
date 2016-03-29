using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Globals;

namespace SAHL.Common.Interfaces
{
    public interface IFinancialsService
    {
        #region Credit Matrix Methods

        ICreditCriteria GetCreditCriteriaByLTVAndIncome(IDomainMessageCollection Messages, int OriginationSourceKey, int ProductKey, int MortgageLoanPurposeKey, int EmploymentTypeKey, double TotalLoanAmount, double PropertyValue, double Income, CreditCriteriaAttributeTypes creditCriteriaAttributeType);

        /// <summary>
        /// Returns the CreditCriteria object with the lowest max loan amount that matches the given parameters. LTV is calculated and used internally.
        /// </summary>
        /// <param name="Messages"></param>
        /// <param name="OriginationSourceKey"></param>
        /// <param name="ProductKey"></param>
        /// <param name="MortgageLoanPurposeKey"></param>
        /// <param name="EmploymentTypeKey"></param>
        /// <param name="TotalLoanAmount"></param>
        /// <param name="PropertyValue"></param>
        /// <param name="CategoryKey"></param>
        /// <returns>An ICreditCriteria if found, or null if not.</returns>
        ICreditCriteria GetCreditCriteriaByCategory(IDomainMessageCollection Messages, int OriginationSourceKey, int ProductKey, int MortgageLoanPurposeKey, int EmploymentTypeKey, double TotalLoanAmount, double PropertyValue, int CategoryKey);

        /// <summary>
        /// Returns the CreditCriteria object with the lowest link rate that matches the given parameters. LTV is not considered.
        /// </summary>
        /// <param name="Messages"></param>
        /// <param name="OriginationSourceKey"></param>
        /// <param name="ProductKey"></param>
        /// <param name="MortgageLoanPurposeKey"></param>
        /// <param name="EmploymentTypeKey"></param>
        /// <param name="TotalLoanAmount"></param>
        /// <returns>An ICreditCriteria if found, or null if not.</returns>
        ICreditCriteria GetCreditCriteria(IDomainMessageCollection Messages, int OriginationSourceKey, int ProductKey, int MortgageLoanPurposeKey, int EmploymentTypeKey, double TotalLoanAmount);

        /// <summary>
        ///
        /// </summary>
        /// <param name="Messages"></param>
        /// <param name="OriginationSourceKey"></param>
        /// <param name="ProductKey"></param>
        /// <param name="MortgageLoanPurposeKey"></param>
        /// <param name="EmploymentTypeKey"></param>
        /// <param name="TotalLoanAmount"></param>
        /// <returns></returns>
        ICreditCriteria GetCreditCriteriaException(IDomainMessageCollection Messages, int OriginationSourceKey, int ProductKey, int MortgageLoanPurposeKey, int EmploymentTypeKey, double TotalLoanAmount);

        /// <summary>
        /// Returns the CreditCriteria row with the largest MaxLoanAmount fitting the given parameters.
        /// </summary>
        /// <param name="Messages"></param>
        /// <param name="OriginationSourceKey"></param>
        /// <param name="ProductKey"></param>
        /// <param name="MortgageLoanPurposeKey"></param>
        /// <param name="EmploymentTypeKey"></param>
        /// <returns></returns>
        double GetMaxLoanAmount(IDomainMessageCollection Messages, int OriginationSourceKey, int ProductKey, int MortgageLoanPurposeKey, int EmploymentTypeKey);

        /// <summary>
        /// Gets tha maximum allowed loan amount for a further loan
        /// </summary>
        /// <param name="Messages"></param>
        /// <param name="OriginationSourceKey"></param>
        /// <param name="ProductKey"></param>
        /// <param name="MortgageLoanPurposeKey"></param>
        /// <param name="EmploymentTypeKey"></param>
        /// <param name="ValuationAmount"></param>
        /// <returns>the highest LTV from the resultant CreditCriteria multiplied by the Valuation amount</returns>
        double GetMaxLoanAmountForFurtherLending(IDomainMessageCollection Messages, int OriginationSourceKey, int ProductKey, int MortgageLoanPurposeKey, int EmploymentTypeKey, double ValuationAmount);

        #endregion Credit Matrix Methods
    }
}