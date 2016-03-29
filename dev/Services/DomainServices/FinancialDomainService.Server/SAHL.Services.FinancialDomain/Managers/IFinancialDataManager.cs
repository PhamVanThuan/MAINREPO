using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data.Models._2AM;
using SAHL.Services.FinancialDomain.Managers.Models;
using SAHL.Services.Interfaces.FinancialDomain.Models;
using System;
using System.Collections.Generic;

namespace SAHL.Services.FinancialDomain.Managers
{
    public interface IFinancialDataManager
    {
        MortgageLoanApplicationInformationModel GetApplicationInformationMortgageLoan(int applicationInformationKey);

        FeeApplicationAttributesModel GetFeeApplicationAttributes(int ApplicationNumber);

        OriginationFeesModel CalculateOriginationFees(decimal loanAmount, decimal bondRequired, Core.BusinessModel.Enums.OfferType offerType, decimal cashOut, decimal overRideCancelFee,
            bool capitaliseFees, bool isQuickPay, decimal householdIncome, Core.BusinessModel.Enums.EmploymentType employmentType, decimal ltv, bool isStaffLoan, bool isDiscountedInitiationFee,
            DateTime offerStartDate, bool capitaliseInitiationFee, bool isGEPF);

        void RemoveAllApplicationFees(int ApplicationNumber);

        void AddApplicationExpense(int ApplicationNumber, ExpenseType expenseType, decimal amount);

        void SetApplicationExpenses(int applicationNumber, Interfaces.FinancialDomain.Models.OriginationFeesModel fees);

        void SaveOfferInformationVariableLoan(OfferInformationVariableLoanDataModel latestOfferInformationVariableLoan);

        CreditCriteriaDataModel DetermineCreditCriteria(MortgageLoanPurpose mortgageLoanPurpose, EmploymentType employmentType, decimal newLoanAmount, decimal ltv,
            OriginationSource originationSource, Product product, decimal householdIncome);

        RateConfigurationValuesModel GetRateConfigurationValues(int marginKey, int marketRateKey);

        GetValidSPVResultModel GetValidSPV(decimal LTV, string offerAttributesCSV);

        OfferInformationType GetLatestOfferInformationType(int ApplicationNumber);

        OfferDataModel GetApplication(int ApplicationNumber);

        OfferInformationDataModel GetLatestApplicationOfferInformation(int applicationNumber);

        void SetApplicationResetConfiguration(int ApplicationNumber, int SPVKey, int ProductKey);

        OfferInformationVariableLoanDataModel GetApplicationInformationVariableLoan(int applicationInformationKey);

        IEnumerable<GetOfferAttributesModel> DetermineApplicationAttributes(int applicationNumber, decimal LTV, EmploymentType employmentType, decimal householdIncome, bool isStaffLoan, bool isGEPF);

        bool ApplicationHasAttribute(int applicationNumber, int offerAttributeTypeKey);

        void RemoveApplicationAttribute(int applicationNumber, int offerAttributeTypeKey);

        void AddApplicationAttribute(int applicationNumber, int offerAttributeTypeKey);
    }
}