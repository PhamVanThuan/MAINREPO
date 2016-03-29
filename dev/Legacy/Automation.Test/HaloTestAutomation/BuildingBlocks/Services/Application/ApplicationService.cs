using Automation.DataAccess;
using Automation.DataAccess.DataHelper;
using Automation.DataModels;
using BuildingBlocks.Services.Contracts;
using Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using WatiN.Core.Exceptions;

namespace BuildingBlocks.Services
{
    public class ApplicationService : _2AMDataHelper, IApplicationService
    {
        private IX2WorkflowService x2Service;
        private ILegalEntityAddressService legalEntityAddressService;

        public ApplicationService(IX2WorkflowService x2Service, ILegalEntityAddressService legalEntityAddressService)
        {
            this.x2Service = x2Service;
            this.legalEntityAddressService = legalEntityAddressService;
        }

        /// <summary>
        /// Get a random offer at the application capture stage in the application capture workflow by adusername
        /// </summary>
        /// <returns></returns>
        public int GetRandomOfferWithOfferExpense(string workflowName, string workflowState, OfferTypeEnum offerType, double maxFee, ExpenseTypeEnum expenseType)
        {
            var offerKey = (from r in x2Service.GetX2DataByWorkflowAndState(workflowName, workflowState)
                            where (from exp in GetOfferExpenses(r.Column("offerkey").GetValueAs<int>())
                                   where exp.TotalOutstandingAmount == maxFee && exp.ExpenseTypeKey == expenseType
                                   select exp
                                   ).FirstOrDefault() != null
                                  && r.Column("offerkey").GetValueAs<int>() !=
                                          (from att in GetOfferAttributes(r.Column("offerkey").GetValueAs<int>())
                                           where att.OfferAttributeTypeKey == OfferAttributeTypeEnum.StaffHomeLoan
                                           select att.OfferKey).FirstOrDefault()
                            select r.Column("offerkey").GetValueAs<int>()).FirstOrDefault();
            return offerKey;
        }

        /// <summary>
        /// Returns the account key for an offer
        /// </summary>
        /// <param name="offerKey">OfferKey</param>
        /// <returns>AccountKey</returns>
        public int GetOfferAccountKey(int offerKey, bool isReservedAccountKey = false)
        {
            var results = base.GetOfferData(offerKey);
            if (!results.HasResults)
                throw new WatiNException(String.Format(@"No AccountKey found for {0}", offerKey));
            if (isReservedAccountKey)
            {
                return results.Rows(0).Column("ReservedAccountKey").GetValueAs<int>();
            }
            else
            {
                return results.Rows(0).Column("AccountKey").GetValueAs<int>();
            }
        }

        /// <summary>
        /// Returns the account key for an offer
        /// </summary>
        /// <param name="offerKey">OfferKey</param>
        /// <returns>AccountKey</returns>
        public double GetLoanAgreementAmount(int offerKey)
        {
            var r = base.GetLatestOfferInformationByOfferKey(offerKey);
            return r.Rows(0).Column("LoanAgreementAmount").GetValueAs<double>();
        }

        /// <summary>
        /// Returns the adusername of the user in the active role provided against the offer.
        /// </summary>
        /// <param name="offerKey">offerKey</param>
        /// <param name="offerRoleType">OfferRoleType</param>
        /// <returns></returns>
        public string GetADUserNameOfFirstActiveOfferRole(int offerKey, OfferRoleTypeEnum offerRoleType)
        {
            string adUserName = (from r in base.GetActiveOfferRolesByOfferRoleType(offerKey, offerRoleType)
                                 select r.Column("ADUserName").GetValueAs<string>()).FirstOrDefault();
            return adUserName;
        }

        /// <summary>
        /// Returns the first legalEntityKey of a legal entity playing an active role of the type provided against the offer.
        /// </summary>
        /// <param name="offerKey">offerKey</param>
        /// <param name="offerRoleType">OfferRoleType</param>
        /// <returns></returns>
        public int GetLegalEntityKeyOfFirstActiveOfferRole(int offerKey, OfferRoleTypeEnum offerRoleType)
        {
            int legalEntityKey = (from r in base.GetActiveOfferRolesByOfferRoleType(offerKey, offerRoleType)
                                  select r.Column("LegalEntityKey").GetValueAs<int>()).FirstOrDefault();
            return legalEntityKey;
        }

        /// <summary>
        /// Gets the legalEntityKey of the first applicant on the offer.
        /// </summary>
        /// <param name="offerKey"></param>
        /// <returns></returns>
        public int GetFirstApplicantLegalEntityKeyOnOffer(int offerKey)
        {
            var r = base.GetLegalEntityLegalNamesForOffer(offerKey);
            return r.Rows(0).Column("LegalEntityKey").GetValueAs<int>();
        }

        /// <summary>
        /// Gets the ID Number of the first applicant on the offer.
        /// </summary>
        /// <param name="offerKey"></param>
        /// <returns></returns>
        public long GetFirstApplicantIDNumberOnOffer(int offerKey)
        {
            var r = base.GetLegalEntityIDNumberForOffer(offerKey);
            return r.Rows(0).Column("IDNumber").GetValueAs<long>();
        }

        /// <summary>
        /// Returns offer roles that are not on the account.
        /// </summary>
        /// <param name="offerKey"></param>
        /// <returns></returns>
        public QueryResults GetOfferRolesNotOnAccount(int offerKey)
        {
            try
            {
                QueryResults le;
                QueryResults r = base.GetClientOfferRolesNotOnAccount(offerKey);
                if (!r.HasResults)
                {
                    return r;
                }
                else
                {
                    foreach (QueryResultsRow row in r.RowList)
                    {
                        le = base.GetLegalEntityLegalNameByLegalEntityKey(row.Column("LegalEntityKey").GetValueAs<int>());
                        if (le.HasResults)
                        {
                            return le;
                        }
                        return le;
                    }
                    return r;
                }
            }
            catch (Exception ex)
            {
                throw new WatiNException(ex.ToString());
            }
        }

        /// <summary>
        /// Gets a random latest offerinformationvariableloan record from the database
        /// </summary>
        /// <returns></returns>
        public Automation.DataModels.Offer GetRandomLatestOfferInformationMortgageLoanRecord(MortgageLoanPurposeEnum purpose, ProductEnum product,
            OfferStatusEnum offerStatus)
        {
            var offer = new Automation.DataModels.Offer();
            var offerRow = base.GetRandomLatestOfferInformationMortgageLoanRecord(purpose, product, offerStatus);

            offer.TermMonths = offerRow.Column("Term").GetValueAs<int>();
            offer.MonthlyInstalment = offerRow.Column("MonthlyInstalment").GetValueAs<float>();
            offer.TermYears = (offer.TermMonths / 12);

            offer.CashOut = offerRow.Column("requestedcashamount").GetValueAs<float>();

            offer.PurchasePrice = offerRow.Column("PurchasePrice").GetValueAs<float>();

            offer.CashDeposit = offerRow.Column("CashDeposit").GetValueAs<float>();

            offer.InterestRate = offerRow.Column("MarketRate").GetValueAs<double>();
            offer.InterestRate = (offer.InterestRate * 100);
            offer.InterestRate = Math.Round(offer.InterestRate, 1);

            string productkey = offerRow.Column("productkey").Value;
            offer.ProductType = (ProductEnum)Enum.Parse(typeof(ProductEnum), productkey);

            string mortgageloanpurposekey = offerRow.Column("mortgageloanpurposekey").Value;
            offer.LoanPurpose = (MortgageLoanPurposeEnum)Enum.Parse(typeof(MortgageLoanPurposeEnum), mortgageloanpurposekey);
            offer.ClientEstimatePropertyValuation = offerRow.Column("ClientEstimatePropertyValuation").GetValueAs<float>();
            offer.ExistingLoan = offerRow.Column("ExistingLoan").GetValueAs<float>();

            offer.FixedPercentage = offerRow.Column("FixedPercent").GetValueAs<double>();

            return offer;
        }

        /// <summary>
        /// Gets a random latest offerinformationvariableloan record from the database
        /// </summary>
        /// <returns></returns>
        public Automation.DataModels.Offer GetRandomOfferRecord(ProductEnum product, OfferTypeEnum offerType, OfferStatusEnum status)
        {
            var offer = new Automation.DataModels.Offer();
            var offerRow = base.GetRandomOfferRecord(product, offerType, status);

            offer.OfferKey = offerRow.Column("offerkey").GetValueAs<int>();
            offer.AccountKey = offerRow.Column("reservedAccountKey").GetValueAs<int>();
            string offertypekey = offerRow.Column("offertypekey").Value;
            offer.OfferTypeKey = (OfferTypeEnum)Enum.Parse(typeof(OfferTypeEnum), offertypekey);

            string offerstatuskey = offerRow.Column("offerstatuskey").Value;
            offer.OfferStatus = (OfferStatusEnum)Enum.Parse(typeof(OfferStatusEnum), offerstatuskey);

            string productkey = offerRow.Column("productkey").Value;
            offer.ProductType = (ProductEnum)Enum.Parse(typeof(ProductEnum), productkey);

            return offer;
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public Automation.DataModels.Offer GetCapitecMortgageLoanApplication()
        {
            var offers = base.GetCapitecMortgageLoanApplications();
            return offers.FirstOrDefault();
        }

        /// <summary>
        /// Populate minimum data to submit from AppCap
        /// </summary>
        /// <param name="offerKey"></param>
        public void CleanupNewBusinessOffer(int offerKey)
        {
            base.CleanupNewBusinessOffer(offerKey);
            base.CleanUpLegalEntityData(offerKey);
            base.InsertOfferMailingAddress(offerKey);
            base.InsertEmploymentRecords(offerKey);
            base.CleanUpOfferDebitOrder(offerKey);
            base.InsertSeller(offerKey);
            base.InsertSettlementBanking(offerKey);
            base.InsertOfferAssetLiability(offerKey);
            base.InsertITCv4(offerKey, -1, -1);
            this.CleanupAllClientAddressesForOffer(offerKey, true, GeneralStatusEnum.Inactive);
            this.DeleteAllOfferClientDomiciliumAddresses(offerKey);

            foreach (var or in base.GetOfferRolesByOfferKey(offerKey).Where(x => x.OfferRoleTypeGroupKey == OfferRoleTypeGroupEnum.Client && x.OfferRoleTypeKey != OfferRoleTypeEnum.AssuredLife))
            {
                var legalEntityAddress = legalEntityAddressService.InsertLegalEntityAddressByAddressType(or.LegalEntityKey, AddressFormatEnum.Street, AddressTypeEnum.Residential, GeneralStatusEnum.Active);
                var domicilium = legalEntityAddressService.InsertLegalEntityDomiciliumAddress(legalEntityAddress.LegalEntityAddressKey, or.LegalEntityKey, GeneralStatusEnum.Pending);
                var dom = base.InsertOfferRoleDomicilium(domicilium.LegalEntityDomiciliumKey, or.OfferRoleKey, offerKey);
            }
        }

        public void CleanupOfferForEmpiricaScoreTests(int offerKey)
        {
            base.CleanUpLegalEntityData(offerKey);
            base.InsertOfferMailingAddress(offerKey);
            base.InsertSeller(offerKey);
            base.InsertSettlementBanking(offerKey);
            base.InsertOfferAssetLiability(offerKey);
            this.CleanupAllClientAddressesForOffer(offerKey, true, GeneralStatusEnum.Inactive);
            this.DeleteAllOfferClientDomiciliumAddresses(offerKey);

            foreach (var or in base.GetOfferRolesByOfferKey(offerKey).Where(x => x.OfferRoleTypeGroupKey == OfferRoleTypeGroupEnum.Client && x.OfferRoleTypeKey != OfferRoleTypeEnum.AssuredLife))
            {
                var legalEntityAddress = legalEntityAddressService.InsertLegalEntityAddressByAddressType(or.LegalEntityKey, AddressFormatEnum.Street, AddressTypeEnum.Residential, GeneralStatusEnum.Active);
                var domicilium = legalEntityAddressService.InsertLegalEntityDomiciliumAddress(legalEntityAddress.LegalEntityAddressKey, or.LegalEntityKey, GeneralStatusEnum.Pending);
                var dom = base.InsertOfferRoleDomicilium(domicilium.LegalEntityDomiciliumKey, or.OfferRoleKey, offerKey);
            }
        }

        public string GetADUserByActiveOfferRoles(int offerKey, OfferRoleTypeEnum offerRoleType)
        {
            string adUserName = string.Empty;
            adUserName = GetADUserNameOfFirstActiveOfferRole(offerKey, offerRoleType);
            return adUserName;
        }

        public void UpdateNewPurchaseVariableLoanOffer(int offerkey, float householdIncome, float loanAgreementAmount, float cashDeposit, float propertyValuation, float feesTotal,
            float instalment, float bondToRegister, float linkRate, float term, MarketRateEnum marketRateKey, int rateConfigurationKey, EmploymentTypeEnum employmentType, float purchasePrice)
        {
            var marketRate = base.GetMarketRate(marketRateKey);

            float effectiveRate = ((float)marketRate + linkRate);
            double monthlyInterestRate = effectiveRate / 12; // Adjust to monthly rate
            var instalmentAtJIBAR = (monthlyInterestRate + (monthlyInterestRate / (Math.Pow((1 + monthlyInterestRate), term) - 1))) * loanAgreementAmount;

            float LTV = loanAgreementAmount / propertyValuation;
            float PTI = (float)instalmentAtJIBAR / (float)householdIncome;

            base.UpdateOfferInformationMortgageLoan(offerkey, householdIncome, loanAgreementAmount, propertyValuation, feesTotal, cashDeposit, instalment, LTV, PTI, bondToRegister,
                term, marketRateKey, rateConfigurationKey, employmentType, purchasePrice);
        }

        public void DeleteAllOfferClientDomiciliumAddresses(int offerKey)
        {
            foreach (var or in this.GetActiveOfferRolesByOfferKey(offerKey, OfferRoleTypeGroupEnum.Client))
            {
                base.DeleteLegalEntityDomiciliumAddress(or.LegalEntityKey);
            }
        }

        public void UpdateAllOfferClientDomiciliumAddresses(int offerkey, GeneralStatusEnum domiciliumStatus)
        {
            foreach (var or in this.GetActiveOfferRolesByOfferKey(offerkey, OfferRoleTypeGroupEnum.Client))
            {
                var domiciliumAddress = (from dom in base.GetLegalEntityDomiciliumAddresses(or.LegalEntityKey)
                                         where dom.GeneralStatusKey == GeneralStatusEnum.Pending
                                         select dom).FirstOrDefault();

                base.UpdateLegalEntityDomiciliumAddress(domiciliumAddress.LegalEntityDomiciliumKey, domiciliumStatus);
            }
        }

        public void CleanupAllClientAddressesForOffer(int applicationKey, bool delete, GeneralStatusEnum generalStatus)
        {
            foreach (var or in this.GetActiveOfferRolesByOfferKey(applicationKey, OfferRoleTypeGroupEnum.Client))
            {
                legalEntityAddressService.CleanupLegalEntityAddresses(or.LegalEntityKey, delete, GeneralStatusEnum.Inactive);
            }
        }

        public IEnumerable<Automation.DataModels.OfferRole> GetActiveOfferRolesByOfferKey(int offerKey, OfferRoleTypeGroupEnum offerRoleTypeGroup)
        {
            return base.GetOfferRolesByOfferKey(offerKey).Where(x => x.OfferRoleTypeGroupKey == offerRoleTypeGroup && x.GeneralStatusKey == GeneralStatusEnum.Active);
        }

        public IEnumerable<Automation.DataModels.OfferRole> GetActiveOfferRolesByLegalEntityKey(int legalEntityKey, OfferRoleTypeGroupEnum offerRoleTypeGroup)
        {
            return base.GetOfferRolesByLegalEntityKey(legalEntityKey).Where(x => x.OfferRoleTypeGroupKey == offerRoleTypeGroup && x.GeneralStatusKey == GeneralStatusEnum.Active);
        }

        public IEnumerable<Automation.DataModels.OfferRole> GetActiveOfferRolesByLegalEntityKeys(int[] legalEntityKeys, OfferRoleTypeGroupEnum offerRoleTypeGroup)
        {
            var offerRoles = new List<OfferRole>();
            foreach (var legalEntityKey in legalEntityKeys)
            {
                var ofrRoles = base.GetOfferRolesByLegalEntityKey(legalEntityKey).Where(x => x.OfferRoleTypeGroupKey == offerRoleTypeGroup && x.GeneralStatusKey == GeneralStatusEnum.Active);
                offerRoles.AddRange(ofrRoles);
            }
            return offerRoles;
        }

        public void InsertSettlementBanking(int offerKey)
        {
            base.InsertSettlementBanking(offerKey);
        }

        public int GetApplicationEmploymentType(int offerKey)
        {
            var result = base.GetLatestOfferInformationByOfferKey(offerKey);
            return result.Rows(0).Column("EmploymentTypeKey").GetValueAs<int>();
        }

        public bool OfferKeyExists(int applicationKey)
        {
            var results = base.GetOfferData(applicationKey);
            return results.HasResults;
        }

        public int GetLegalEntityKeyFromOfferKey(int offerKey)
        {
            int results = base.GetLegalEntityKeyFromOffer(offerKey);
            return results;
        }

        public void CleanUpLegalEntityRequiredFields(int offerKey)
        {
            base.CleanUpLegalEntityData(offerKey);
        }

        public int GetOfferByWorkflowState(string workflowState)
        {
            return base.GetOfferByWorkflowAndState(workflowState);
        }

        public int GetOfferByOfferTypeAndWorkflowState(int offerType, string workflowState)
        {
            return base.GetOfferByOfferTypeAndWorkflowState(offerType, workflowState);
        }

        public void InsertAffordabilityAssessment(int affordabilityAssessmentStatusKey, int offerKey)
        {
            base.InsertAffordabilityAssessment(affordabilityAssessmentStatusKey, offerKey);
        }

        public void UpdateOfferStartDate(DateTime offerStartDate, int offerKey)
        {
            base.UpdateOfferStartDate(offerStartDate, offerKey);
        }
    }
}