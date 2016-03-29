using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Globals;

namespace SAHL.Common.BusinessModel.Interfaces.Repositories
{
    public interface ICreditCriteriaRepository
    {
        double GetMaxPTI(IDomainMessageCollection Messages, int OriginationSourceKey, int ProductKey, int MortgageLoanPurposeKey, int EmploymentTypeKey, double TotalLoanAmount);

        IReadOnlyEventList<ICreditCriteria> GetCreditCriteria(IDomainMessageCollection Messages, int OriginationSourceKey, int ProductKey, int MortgageLoanPurposeKey, int EmploymentTypeKey, double TotalLoanAmount);

        IReadOnlyEventList<ICreditCriteria> GetCreditCriteria(IDomainMessageCollection messages, int originationSourceKey, int productKey, int mortgageLoanPurposeKey, int employmentTypeKey, double totalAmount, int creditCriteriaAttributeTypeKey);

        ICreditCriteria GetCreditCriteriaException(IDomainMessageCollection Messages, int OriginationSourceKey, int ProductKey, int MortgageLoanPurposeKey, int EmploymentTypeKey, double TotalLoanAmount);

        ICreditCriteria GetCreditCriteriaByLTVAndIncome(IDomainMessageCollection messages, int originationSourceKey, int productKey, int mortgageLoanPurposeKey, int employmentTypeKey, double totalLoanAmount, double ltv, double income, CreditCriteriaAttributeTypes creditCriteriaAttributeType);

        IReadOnlyEventList<ICreditCriteria> GetCreditCriteriaByCategory(IDomainMessageCollection Messages, int OriginationSourceKey, int ProductKey, int MortgageLoanPurposeKey, int EmploymentTypeKey, double TotalLoanAmount, double LTV, int CategoryKey);

        ICreditCriteria GetCreditCriteriaByKey(int creditCriteriaKey);

        ICreditCriteria GetCreditCriteriaForLatestAcceptedApplicationOnAccount(IAccount account);
    }
}