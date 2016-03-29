using MoreLinq;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data.Models._2AM;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.Services.ApplicationDomain.Managers.Application
{
    public class ApplicationManager : IApplicationManager
    {
        private IApplicationDataManager applicationDataManager;

        public ApplicationManager(IApplicationDataManager applicationDataManager)
        {
            this.applicationDataManager = applicationDataManager;
        }

        public EmploymentType DetermineEmploymentTypeForApplication(IEnumerable<EmploymentDataModel> employmentList)
        {
            int employmentTypeKey = employmentList.GroupBy(x => x.EmploymentTypeKey)
                .Select(g => new { g.Key, Income = g.Sum(y => y.ConfirmedBasicIncome > 0
                                   ? y.ConfirmedBasicIncome : y.BasicIncome) 
                                 })
                .MaxBy(m => m.Income).Key;

            return (EmploymentType)employmentTypeKey;
        }

        public int SaveApplication(OfferType offerType, OfferStatus offerStatus, DateTime openDate, int? applicationSourceKey,
                                   int reservedAccountKey, OriginationSource originationSource, string reference
                                 , int applicantCount)
        {
            var offerDataModel = new OfferDataModel(
                        (int)offerType,
                        (int)offerStatus,
                        openDate,
                        null,
                        null,
                        reference,
                        null,
                        applicationSourceKey,
                        reservedAccountKey,
                        (int)originationSource,
                        applicantCount);

            // save the application
            return applicationDataManager.SaveApplication(offerDataModel);
        }

        public void SaveApplicationMortgageLoan(int applicationNumber, MortgageLoanPurpose mortgageLoanPurpose
                                              , int applicantCount,
            decimal? purchasePrice, decimal? clientEstimatePropertyValuation, string transferAttorney)
        {
            var offerMortgageLoanDataModel = new OfferMortgageLoanDataModel(
                            applicationNumber,
                            null,
                            (int)mortgageLoanPurpose,
                            (int)ApplicantType.Single, //default
                            applicantCount,
                            null,
                            null,
                            null,
                            null,
                            null,
                            Convert.ToDouble(purchasePrice),
                            (int)ResetConfiguration.Eighteenth, //default
                            transferAttorney,
                            mortgageLoanPurpose == MortgageLoanPurpose.Newpurchase ? 
                                                                   Convert.ToDouble(purchasePrice) 
                                                                 : Convert.ToDouble(clientEstimatePropertyValuation),
                            null,
                            null,
                            null,
                            (int)Language.English); //default

            applicationDataManager.SaveApplicationMortgageLoan(offerMortgageLoanDataModel);
        }

        public int SaveApplicationInformation(DateTime offerInsertDate, int applicationNumber
                                            , OfferInformationType offerInformationType
                                            , Product product)
        {
            var offerInformationDataModel = new OfferInformationDataModel(offerInsertDate,
                                                                          applicationNumber,
                                                                          (int)offerInformationType,
                                                                          "System",
                                                                          DateTime.Now,
                                                                          (int)product);

            return applicationDataManager.SaveApplicationInformation(offerInformationDataModel);
        }

        public void SaveNewPurchaseApplicationInformationVariableLoan(int offerInformationKey, int term, decimal deposit,
                                                                      decimal purchasePrice, decimal loanAmountNoFees)
        {
            var offerInformationVariableLoanDataModel = new OfferInformationVariableLoanDataModel(
                        offerInformationKey,
                        null,
                        term,
                        null,
                        Convert.ToDouble(deposit),
                        Convert.ToDouble(purchasePrice),
                        null,
                        null,
                        null,
                        null,
                        null,
                        null,
                        null,
                        null,
                        null,
                        null,
                        null,
                        Convert.ToDouble(loanAmountNoFees),
                        null,
                        null,
                        null,
                        null,
                        null,
                        null,
                        null,
                        null,
                        null,
                        null,
                        null,
                        null);

            applicationDataManager.SaveApplicationInformationVariableLoan(offerInformationVariableLoanDataModel);
        }

        public void SaveSwitchApplicationInformationVariableLoan(int offerInformationKey, int term, decimal existingLoan
                                                               , decimal estimatedPropertyValue, decimal cashOut
                                                               , decimal loanAmountNoFees)
        {
            var offerInformationVariableLoanDataModel = new OfferInformationVariableLoanDataModel(
                        offerInformationKey,
                        null,
                        term,
                        Convert.ToDouble(existingLoan),
                        null,
                        Convert.ToDouble(estimatedPropertyValue),
                        null,
                        null,
                        null,
                        null,
                        null,
                        null,
                        null,
                        null,
                        null,
                        null,
                        null,
                        Convert.ToDouble(loanAmountNoFees),
                        Convert.ToDouble(cashOut),
                        null,
                        null,
                        null,
                        null,
                        null,
                        null,
                        null,
                        null,
                        null,
                        null,
                        null);

            applicationDataManager.SaveApplicationInformationVariableLoan(offerInformationVariableLoanDataModel);
        }

        public void SaveRefinanceApplicationInformationVariableLoan(int offerInformationKey, int term
                                                                  , decimal estimatedPropertyValue, decimal cashOut
                                                                  , decimal loanAmountNoFees)
        {
            var offerInformationVariableLoanDataModel = new OfferInformationVariableLoanDataModel(
                        offerInformationKey,
                        null,
                        term,
                        null,
                        null,
                        Convert.ToDouble(estimatedPropertyValue),
                        null,
                        null,
                        null,
                        null,
                        null,
                        null,
                        null,
                        null,
                        null,
                        null,
                        null,
                        Convert.ToDouble(loanAmountNoFees),
                        Convert.ToDouble(cashOut),
                        null,
                        null,
                        null,
                        null,
                        null,
                        null,
                        null,
                        null,
                        null,
                        null,
                        null);

            applicationDataManager.SaveApplicationInformationVariableLoan(offerInformationVariableLoanDataModel);
        }

        public void SaveApplicationInformationInterestOnly(int offerInformationKey)
        {
            var offerInformationInterestOnly = new OfferInformationInterestOnlyDataModel(offerInformationKey, null, null);
            applicationDataManager.SaveApplicationInformationInterestOnly(offerInformationInterestOnly);
        }

        public void SaveApplicationInformationQuickCash(int offerInformationKey)
        {
            var offerInformationQuickCash = new OfferInformationQuickCashDataModel(offerInformationKey, 0, 0, 0);
            applicationDataManager.SaveOfferInformationQuickCash(offerInformationQuickCash);
        }
    }
}