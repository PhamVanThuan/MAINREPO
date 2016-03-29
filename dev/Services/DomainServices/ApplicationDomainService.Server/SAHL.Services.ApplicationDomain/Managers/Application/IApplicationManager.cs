using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data.Models._2AM;
using System;
using System.Collections.Generic;

namespace SAHL.Services.ApplicationDomain.Managers.Application
{
    public interface IApplicationManager
    {
        EmploymentType DetermineEmploymentTypeForApplication(IEnumerable<EmploymentDataModel> employmentList);

        int SaveApplication(OfferType offerType, OfferStatus offerStatus, DateTime openDate, int? applicationSourceKey,
            int reservedAccountKey, OriginationSource originationSource, string reference, int applicantCount);

        void SaveApplicationMortgageLoan(int applicationNumber, MortgageLoanPurpose mortgageLoanPurpose, int applicantCount,
            decimal? purchasePrice, decimal? clientEstimatePropertyValuation, string transferAttorney);

        int SaveApplicationInformation(DateTime offerInsertDate, int applicationNumber, OfferInformationType offerInformationType, Product product);

        void SaveNewPurchaseApplicationInformationVariableLoan(int offerInformationKey, int term, decimal deposit,
            decimal purchasePrice, decimal loanAmountNoFees);

        void SaveSwitchApplicationInformationVariableLoan(int offerInformationKey, int term, decimal existingLoan, decimal estimatedPropertyValue,
            decimal cashOut, decimal loanAmountNoFees);

        void SaveRefinanceApplicationInformationVariableLoan(int offerInformationKey, int term, decimal estimatedPropertyValue,
            decimal cashOut, decimal loanAmountNoFees);

        void SaveApplicationInformationInterestOnly(int offerInformationKey);

        void SaveApplicationInformationQuickCash(int offerInformationKey);
    }
}