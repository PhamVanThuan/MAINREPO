using Automation.DataAccess;
using Automation.DataModels;
using Common.Enums;
using System;
using System.Collections.Generic;

namespace BuildingBlocks.Services.Contracts
{
    public interface IAccountService
    {
        QueryResults GetOpenLifeAccountWithAssuredLife();

        QueryResults GetAccountFinancialServiceFinancialAdjustments(int accountkey);

        int GetFinancialServicePaymentTypeByAccountKey(int accountKey);

        string GetMortgageLoanColumn(int accountKey, string columnName);

        string GetAccountByAccountStatus(AccountStatusEnum accountStatus);

        List<int> GetMortgageAccountsByLegalEntity(ref int legalentityKey, ref string legalentityIdNumber);

        void UpdateRemainingTerm(int accountKey, int term);

        void UpdateLoanMonthsInArrears(int accountkey, decimal monthsInArrears);

        bool fIsAccountUnderDebtCounselling(int accountKey);

        QueryResults GetRandomVariableLoanAccountByMainApplicantCount(int roleCount, int recordCount, AccountStatusEnum status);

        string GetMortgageLoanColumn(int accountKey, int financialServiceTypeKey, string columnName);

        IEnumerable<Automation.DataModels.ManualDebitOrder> GetManualDebitOrders(int accountKey);

        double GetTotalInstalment(int accountKey);

        double GetCurrentBalance(int accountKey);

        void UpdateFinancialServiceBalance(int lifeAccountKey, FinancialServiceTypeEnum financialServiceType, decimal balanceAmount);

        double GetSumOfChildFinancialServiceBalance(int accountKey);

        IEnumerable<Automation.DataModels.AccountKeyWithIndicator> GetAccountMailingAddressInfo();

        int GetDebitOrderDayIncludingFutureDatedChanges(int accountKey);

        System.Collections.Generic.Dictionary<int, Common.Enums.LegalEntityTypeEnum> AccountRoleLegalEntityKeys(int accountKey);

        int AddRoleToAccount(int accountKey, Common.Enums.RoleTypeEnum roleType, Common.Enums.GeneralStatusEnum status, int legalEntityKey = 0);

        void ClearArrearBalance(int accountKey);

        void CorrectAccountRolesWithInvalidIDNumbers(int accountKey);

        Automation.DataModels.Account GetAccountByKey(int accountKey);

        Automation.DataModels.Account GetAccountForNonPerformingLoanTests(bool isMarkedNonPerforming, bool hasFurtherLendingOffer, bool hasDetails, params int[] productKeys);

        int GetAccountWithOpenRelatedProducts(System.Collections.Generic.List<int> accounts, bool hasHOC, bool hasLife);

        System.Collections.Generic.List<Automation.DataModels.BankAccount> GetBankAccountRecordsForAccount(int accountKey);

        double GetFinancialServicePaymentByType(int accountKey, Common.Enums.FinancialServiceTypeEnum financialServiceTypeKey);

        System.Collections.Generic.List<string> GetIDNumbersForRoleOnAccount(int accountKey, Common.Enums.RoleTypeEnum roleType, Common.Enums.GeneralStatusEnum status);

        int GetLifeAccountKey(int noAssureLifeRoles, Common.Enums.LifePolicyStatusEnum lifePolicyStatus, bool premiumCalcShouldIncrease = true);

        int GetOfferForAccount(int accountKey, Common.Enums.OfferTypeEnum offerType, Common.Enums.OfferStatusEnum offerStatus);

        string GetIDNumberforExternalRoleOnOffer(int offerkey);

        Automation.DataModels.Account GetOpenAccountWithSubsidyStopOrder(bool fixedPayment);

        QueryResults GetOriginalAcceptedOfferForAccount(Automation.DataModels.Account acc);

        Automation.DataModels.Account GetRandomAccountInSPV(Common.Enums.ProductEnum productKey, int spvKey);

        Automation.DataModels.Account GetRandomAccountWithFixedPayment(Common.Enums.ProductEnum productKey, Common.Enums.AccountStatusEnum accountStatusKey, bool fixedPayment);

        Automation.DataModels.Account GetRandomAccountWithNegativeBalance(Common.Enums.ProductEnum productKey, Common.Enums.AccountStatusEnum accountStatusKey);

        Automation.DataModels.Account GetRandomInterestOnlyAccount(Common.Enums.ProductEnum productKey);

        Automation.DataModels.Account GetRandomMortgageLoanAccountWithPositiveBalance(Common.Enums.ProductEnum productKey, Common.Enums.AccountStatusEnum accountStatus);

        Automation.DataModels.Account GetVariableLoanAccountByMainApplicantCount(int roleCount, int recordCount, Common.Enums.AccountStatusEnum status);

        Automation.DataModels.Account GetPersonalLoanAccount();

        Automation.DataModels.Account GetOpenMortgageLoanAccountInSPV(int SPVKey);

        bool MortgageLoanAccountHasOpenLife(int accountKey);

        void PutAccountIntoArrears(int accountKey);

        string GetAccountColumn(int accountKey, string columnName);

        int GetLatestOpenAccountWithOneMainApplicantAndOneEmploymentRecord();

        int GetAccountKeyByFinancialServiceKey(int financialServiceKey);

        QueryResults GetMailingAddress(int accountKey);

        QueryResults GetOpenRelatedAccountsByProductKey(int accountKey, ProductEnum product);

        void UpdateOpenDate(int accountkey, DateTime dateTime);

        DateTime GetOpenDate(int accountkey);

        QueryResults GetOpenFinancialServiceRecordByType(int accountKey, FinancialServiceTypeEnum financialServiceType);

        QueryResults GetPersonalLoanAccountWithACreditLifePolicy();

        QueryResults GetPersonalLoanAccountWithoutACreditLifePolicy();

        double GetCurrentPersonalLoanBalance(int accountKey);

        IEnumerable<Automation.DataModels.Balance> GetAccountBalances(int accountKey);

        int GetPersonalLoanAccountWithoutStageTransition();

        int GetPersonalLoanAccountWithStageTransition();

        Automation.DataModels.Account GetAccountByPropertyKey(int propertykey, AccountStatusEnum status, OriginationSourceEnum originationSource);

        Automation.DataModels.Account GetNonSAHLAccountWithDetailType(DetailTypeEnum detailType);

        Automation.DataModels.Account GetAccountRecord(int accountKey);

        void RemoveRoleFromAccount(int accountKey, RoleTypeEnum roleType, int legalEntityKey);

        Automation.DataModels.Account GetRandomAccountInSPVWithoutDetailType(ProductEnum productKey, int spvKey, DetailTypeEnum detailType, DetailClassEnum detailClass);

        Automation.DataModels.Account GetRandomAccountInSPVWithDetailType(ProductEnum productKey, int spvKey, DetailTypeEnum detailType, DetailClassEnum detailClass);

        Automation.DataModels.Account GetRandomMortgageLoanAccountWithRateAdjustment(ProductEnum productEnum, AccountStatusEnum accountStatus, bool isSurchargeRateAdjustment);

        void AddRecuringDiscountAttributeToAccountsForLegalEntity(int legalEntityKey);

        void AddRecuringDiscountAttributeToOffer(int offerKey);

        QueryResults GetFinancialServiceRecordByType(int accountKey, FinancialServiceTypeEnum financialServiceType);

        QueryResults GetOpenAccountAndAssociatedOfferWithoutAGivenProductType(OfferTypeEnum offerType, ProductEnum productType);

        int GetParentAccountKey(int accountKey);

        IEnumerable<Account> GetAccounts(Predicate<Account> predicate);

        IEnumerable<Role> GetRoles(Predicate<Role> predicate);

        Automation.DataModels.Account GetAccountsWithActiveSubsidyAndAcceptedOfferWithLoanConditions222Or223();
    }
}