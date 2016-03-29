using SAHL.Common.Attributes;
using SAHL.Common.BusinessModel.Helpers;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.DomainMessages;
using SAHL.Common.Factories;
using SAHL.Common.Globals;
using SAHL.Common.Interfaces;

namespace SAHL.Common.Service
{
    [FactoryType(typeof(IFinancialsService))]
    public class FinancialsService : IFinancialsService
    {
        public ICreditCriteria GetCreditCriteriaByLTVAndIncome(IDomainMessageCollection Messages, int OriginationSourceKey, int ProductKey, int MortgageLoanPurposeKey, int EmploymentTypeKey, double TotalLoanAmount, double PropertyValue, double Income, CreditCriteriaAttributeTypes creditCriteriaAttributeType)
        {
            double LTV = LoanCalculator.CalculateLTV(TotalLoanAmount, PropertyValue);
            ICreditCriteriaRepository ccRepo = RepositoryFactory.GetRepository<ICreditCriteriaRepository>();
            return ccRepo.GetCreditCriteriaByLTVAndIncome(Messages, OriginationSourceKey, ProductKey, MortgageLoanPurposeKey, EmploymentTypeKey, TotalLoanAmount, LTV, Income, creditCriteriaAttributeType);
        }

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
        public ICreditCriteria GetCreditCriteriaByCategory(IDomainMessageCollection Messages, int OriginationSourceKey, int ProductKey, int MortgageLoanPurposeKey, int EmploymentTypeKey, double TotalLoanAmount, double PropertyValue, int CategoryKey)
        {
            double LTV = LoanCalculator.CalculateLTV(TotalLoanAmount, PropertyValue);

            ICreditCriteriaRepository ccRepo = RepositoryFactory.GetRepository<ICreditCriteriaRepository>();
            IReadOnlyEventList<ICreditCriteria> list = ccRepo.GetCreditCriteriaByCategory(Messages, OriginationSourceKey, ProductKey, MortgageLoanPurposeKey, EmploymentTypeKey, TotalLoanAmount, LTV, CategoryKey);

            if (list == null || list.Count == 0)
            {
                return null;
            }

            return list[0];
        }

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
        public ICreditCriteria GetCreditCriteria(IDomainMessageCollection Messages, int OriginationSourceKey, int ProductKey, int MortgageLoanPurposeKey, int EmploymentTypeKey, double TotalLoanAmount)
        {
            ICreditCriteriaRepository ccRepo = RepositoryFactory.GetRepository<ICreditCriteriaRepository>();
            IReadOnlyEventList<ICreditCriteria> list = ccRepo.GetCreditCriteria(Messages, OriginationSourceKey, ProductKey, MortgageLoanPurposeKey, EmploymentTypeKey, TotalLoanAmount);

            if (list == null || list.Count == 0)
            {
                Messages.Add(new Error("No matching Credit Criteria Found.", "No matching Credit Criteria for the given parameters (OriginationSource/Product/MortgateLoanPurpose/EmploymentType)"));
                return null;
            }

            return list[0];
        }

        public ICreditCriteria GetCreditCriteriaException(IDomainMessageCollection Messages, int OriginationSourceKey, int ProductKey, int MortgageLoanPurposeKey, int EmploymentTypeKey, double TotalLoanAmount)
        {
            ICreditCriteriaRepository ccRepo = RepositoryFactory.GetRepository<ICreditCriteriaRepository>();
            ICreditCriteria cc = ccRepo.GetCreditCriteriaException(Messages, OriginationSourceKey, ProductKey, MortgageLoanPurposeKey, EmploymentTypeKey, TotalLoanAmount);

            if (cc == null)
            {
                Messages.Add(new Error("No matching Credit Criteria Found.", "No matching Credit Criteria for the given parameters (OriginationSource/Product/MortgateLoanPurpose/EmploymentType)"));
                return null;
            }

            return cc;
        }

        /// <summary>
        /// Returns the CreditCriteria row with the largest MaxLoanAmount fitting the given parameters.
        /// </summary>
        /// <param name="Messages"></param>
        /// <param name="OriginationSourceKey"></param>
        /// <param name="ProductKey"></param>
        /// <param name="MortgageLoanPurposeKey"></param>
        /// <param name="EmploymentTypeKey"></param>
        /// <returns></returns>
        public double GetMaxLoanAmount(IDomainMessageCollection Messages, int OriginationSourceKey, int ProductKey, int MortgageLoanPurposeKey, int EmploymentTypeKey)
        {
            ICreditCriteriaRepository ccRepo = RepositoryFactory.GetRepository<ICreditCriteriaRepository>();
            IReadOnlyEventList<ICreditCriteria> list = ccRepo.GetCreditCriteria(Messages, OriginationSourceKey, ProductKey, MortgageLoanPurposeKey, EmploymentTypeKey, 0);

            if (list == null || list.Count == 0 || list[list.Count - 1].MaxLoanAmount == null)
            {
                Messages.Add(new Error("No matching Credit Criteria Found.", "No matching Credit Criteria for the given parameters (OriginationSource/Product/MortgateLoanPurpose/EmploymentType)"));
                return -1;
            }

            if (list[list.Count - 1].MaxLoanAmount != null)
                return (double)list[list.Count - 1].MaxLoanAmount;

            return -1;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="Messages"></param>
        /// <param name="OriginationSourceKey"></param>
        /// <param name="ProductKey"></param>
        /// <param name="MortgageLoanPurposeKey"></param>
        /// <param name="EmploymentTypeKey"></param>
        /// <param name="ValuationAmount"></param>
        /// <returns></returns>
        public double GetMaxLoanAmountForFurtherLending(IDomainMessageCollection Messages, int OriginationSourceKey, int ProductKey, int MortgageLoanPurposeKey, int EmploymentTypeKey, double ValuationAmount)
        {
            ICreditCriteriaRepository ccRepo = RepositoryFactory.GetRepository<ICreditCriteriaRepository>();
            IReadOnlyEventList<ICreditCriteria> list = ccRepo.GetCreditCriteria(Messages, OriginationSourceKey, ProductKey, MortgageLoanPurposeKey, EmploymentTypeKey, 0);

            //get highest ltv and multiply by valuationamount
            double max = 0;
            double ret = 0;
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].LTV == null)
                    continue;
                else
                {
                    double ltv = (double)list[i].LTV;

                    if (ltv > max)
                    {
                        max = ltv;
                    }
                }
            }

            ret = max / 100 * ValuationAmount;

            if (ret > ValuationAmount)
                return ValuationAmount;
            else
                return ret;
        }
    }
}