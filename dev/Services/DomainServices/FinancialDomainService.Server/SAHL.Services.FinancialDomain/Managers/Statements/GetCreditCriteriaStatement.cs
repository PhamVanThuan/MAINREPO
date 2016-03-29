using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data;
using SAHL.Core.Data.Models._2AM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Services.FinancialDomain.Managers.Statements
{
    public class GetCreditCriteriaStatement : ISqlStatement<CreditCriteriaDataModel>
    {
        public int MortgageLoanPurposeKey { get; protected set; }
        public int EmploymentTypeKey { get; protected set; }
        public decimal TotalLoanAmount { get; protected set; }
        public decimal LTV { get; protected set; }
        public int OriginationSourceKey { get; protected set; }
        public int ProductKey { get; protected set; }
        public decimal Income { get; protected set; }
        public int CreditCriteriaAttributeTypeKey { get; protected set; }

        public GetCreditCriteriaStatement(MortgageLoanPurpose mortgageLoanPurpose, EmploymentType employmentType, decimal totalLoanAmount, decimal ltv, OriginationSource originationSource, 
            Product product, decimal income, CreditCriteriaAttributeType creditCriteriaAttributeType)
        {
            this.MortgageLoanPurposeKey = (int) mortgageLoanPurpose;
            this.EmploymentTypeKey = (int) employmentType;
            this.TotalLoanAmount = totalLoanAmount;
            this.LTV = ltv;
            this.OriginationSourceKey = (int)originationSource;
            this.ProductKey = (int)product;
            this.Income = income;
            this.CreditCriteriaAttributeTypeKey = (int)creditCriteriaAttributeType;
        }

        public string GetStatement()
        {
            return @"EXECUTE [2AM].[dbo].[GetCreditCriteria] 
                       @MortgageLoanPurposeKey
                      ,@EmploymentTypeKey
                      ,@TotalLoanAmount
                      ,@LTV
                      ,@OriginationSourceKey
                      ,@ProductKey
                      ,@Income
                      ,@CreditCriteriaAttributeTypeKey";
        }
    }
}
