using Automation.DataAccess;
using Automation.DataAccess.DataHelper;
using Automation.DataModels;
using BuildingBlocks.Services.Contracts;
using Common.Constants;
using Common.Enums;
using Common.Extensions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BuildingBlocks.Services
{
    public class AccountService : _2AMDataHelper, IAccountService
    {
        private readonly IHOCService hocService;
        private readonly ILegalEntityService legalEntityService;
        private readonly ILoanTransactionService loanTransactionService;
        private readonly IDetailTypeService detailTypeService;

        public AccountService(ILegalEntityService legalEntityService, ILoanTransactionService loanTransactionService, IHOCService hocService, IDetailTypeService detailTypeService)
        {
            this.hocService = hocService;
            this.legalEntityService = legalEntityService;
            this.loanTransactionService = loanTransactionService;
            this.detailTypeService = detailTypeService;
        }

        /// <summary>
        /// Gets the latest debit order and then checks if the debit order has a future dated change
        /// </summary>
        /// <param name="accountKey"></param>
        public int GetDebitOrderDayIncludingFutureDatedChanges(int accountKey)
        {
            var r = base.GetDebitOrderByAccountKey(accountKey);
            int financialServiceBankAccountKey = r.Rows(0).Column("financialservicebankaccountkey").GetValueAs<int>();
            int debitOrderDay = r.Rows(0).Column("debitorderday").GetValueAs<int>();
            r.Dispose();
            int futureDODay = base.GetDebitOrderDayFromFutureDatedChange(financialServiceBankAccountKey);
            if (futureDODay > 0)
            {
                debitOrderDay = futureDODay;
            }
            return debitOrderDay;
        }

        /// <summary>
        /// Fetches an ID number for a main applicant on an account.
        /// </summary>
        /// <param name="accountKey">accountKey</param>
        /// <returns></returns>
        public List<string> GetIDNumbersForRoleOnAccount(int accountKey, RoleTypeEnum roleType, GeneralStatusEnum status)
        {
            var results = base.GetLegalEntityIDForAccountByRole(accountKey, roleType);

            //we need a valid id number
            List<string> idNumber = (from r in results
                                     where
                                         !string.IsNullOrEmpty(r.Column("IDNumber").GetValueAs<string>())
                                         && r.Column("IDNumber").GetValueAs<string>().Length == 13
                                         && r.Column("GeneralStatusKey").GetValueAs<int>() == (int)status
                                     select r.Column("IDNumber").GetValueAs<string>()).ToList();
            return idNumber;
        }

        /// <summary>
        /// Fetches that payment amount off the financial service for the specified financial service type.
        /// </summary>
        /// <param name="accountKey">accountKey</param>
        /// <param name="financialServiceTypeKey">financialServiceTypeKey</param>
        public double GetFinancialServicePaymentByType(int accountKey, FinancialServiceTypeEnum financialServiceTypeKey)
        {
            var r = base.GetOpenFinancialServiceRecordByType(accountKey, financialServiceTypeKey);
            return r.Rows(0).Column("Payment").GetValueAs<double>();
        }

        /// <summary>
        /// Returns a true/false depending on whether or not a Mortgage Loan account has an open Life
        /// </summary>
        /// <param name="accountKey"></param>
        /// <returns></returns>
        public bool MortgageLoanAccountHasOpenLife(int accountKey)
        {
            var r = base.GetOpenRelatedAccountsByProductKey(accountKey, ProductEnum.LifePolicy);
            return r.HasResults;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="accounts"></param>
        /// <param name="hasHOC"></param>
        /// <param name="hasLife"></param>
        /// <returns></returns>
        public int GetAccountWithOpenRelatedProducts(List<int> accounts, bool hasHOC, bool hasLife)
        {
            int acc;

            //Life & HOC
            if (hasHOC && hasLife)
            {
                acc = (from a in accounts where hocService.MortgageLoanAccountHasOpenHOC(a) && MortgageLoanAccountHasOpenLife(a) select a).FirstOrDefault();
                return acc;
            }

            //HOC without Life
            if (hasHOC && !hasLife)
            {
                acc = (from a in accounts where hocService.MortgageLoanAccountHasOpenHOC(a) && !MortgageLoanAccountHasOpenLife(a) select a).FirstOrDefault();
                return acc;
            }

            //Life without HOC
            if (!hasHOC && hasLife)
            {
                acc = (from a in accounts where !hocService.MortgageLoanAccountHasOpenHOC(a) && MortgageLoanAccountHasOpenLife(a) select a).FirstOrDefault();
                return acc;
            }

            //Without Life or HOC
            if (!hasHOC && !hasLife)
            {
                acc = (from a in accounts where !hocService.MortgageLoanAccountHasOpenHOC(a) && !MortgageLoanAccountHasOpenLife(a) select a).FirstOrDefault();
                return acc;
            }
            return default(int);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="accountKey"></param>
        /// <returns></returns>
        public Dictionary<int, LegalEntityTypeEnum> AccountRoleLegalEntityKeys(int accountKey)
        {
            Dictionary<int, LegalEntityTypeEnum> keys = new Dictionary<int, LegalEntityTypeEnum>();
            var leRoles = base.GetLegalEntityRoles(accountKey);
            foreach (var role in leRoles)
            {
                var le = legalEntityService.GetLegalEntity(legalentitykey: role.LegalEntityKey);
                keys.Add(role.LegalEntityKey,
                    (LegalEntityTypeEnum)Enum.ToObject(typeof(LegalEntityTypeEnum), le.LegalEntityTypeKey));
            }
            return keys;
        }

        /// <summary>
        /// Raises an instalment equal to FinancialService.Payment in order to put the account into arrears. It will then backdate the transaction.
        /// </summary>
        /// <param name="accountKey">accountKey</param>
        public void PutAccountIntoArrears(int accountKey)
        {
            string date = DateTime.Now.ToString(Formats.DateTimeFormatSQL);

            //first we need to check if the client is in advance
            var arrearTransactionBalance = loanTransactionService.GetLatestArrearBalanceAmount(accountKey);

            //fetch the variable loan financial service payment amount
            double payment = GetFinancialServicePaymentByType(accountKey, FinancialServiceTypeEnum.VariableLoan);

            //get the value to post
            decimal valueToPost = arrearTransactionBalance < 0 ? (decimal)payment + Math.Abs(arrearTransactionBalance) : (decimal)payment;

            //post and back date the Raise Instalment transaction
            var financialServiceKey = base.GetOpenFinancialServiceRecordByType(accountKey, FinancialServiceTypeEnum.VariableLoan).FirstOrDefault().Column("FinancialServiceKey").Value;
            loanTransactionService.pProcessTran(Convert.ToInt32(financialServiceKey), TransactionTypeEnum.RaiseInstalment, (decimal)payment, "Raise Instalment", @"SAHL\Tester");
            int arrearTransactionNumber = loanTransactionService.GetLatestArrearTransactionKey(accountKey);
            loanTransactionService.BackDateArrearTransaction(arrearTransactionNumber, -8);
        }

        /// <summary>
        /// Clears the arrear balance by posting an Instalment Payment Debit Order transaction equal in value to the latest arrear balance.
        /// </summary>
        /// <param name="accountKey">accountKey</param>
        public void ClearArrearBalance(int accountKey)
        {
            //fetch the latest arrear balance amount
            decimal arrearBalance = loanTransactionService.GetLatestArrearBalanceAmount(accountKey);

            //clear the arrear balance by posting an Instalment Payment transaction
            var financialService = base.GetOpenFinancialServiceRecordByType(accountKey, FinancialServiceTypeEnum.VariableLoan).FirstOrDefault();
            loanTransactionService.pProcessTran(Convert.ToInt32(financialService.Column("FinancialServiceKey").Value), TransactionTypeEnum.InstalmentPaymentDebitOrder, arrearBalance,
                "Instalment Payment Debit Order", "SAHL\tester");
        }

        /// <summary>
        /// Gets a variable loan account
        /// </summary>
        /// <param name="accountKey"></param>
        /// <returns></returns>
        public Automation.DataModels.Account GetAccountByKey(int accountKey)
        {
            var acc = base.GetAccountByKeySQL(accountKey);
            acc.FinancialServices = base.GetLoanFinancialServices(acc.AccountKey);
            acc.Instalment = base.GetTotalInstalment(acc.AccountKey);
            acc.TotalInstalment = base.GetTotalInstalment(acc.AccountKey);
            ProductEnum lifeAccount = acc.ProductKey == ProductEnum.PersonalLoan || acc.ProductKey == ProductEnum.SAHLCreditProtectionPlan
                ? ProductEnum.SAHLCreditProtectionPlan
                : ProductEnum.LifePolicy;

            var results = base.GetOpenRelatedAccountsByProductKey(acc.AccountKey, lifeAccount);
            if (results.HasResults)
                acc.LifeAccountKey = base.GetOpenRelatedAccountsByProductKey(acc.AccountKey, lifeAccount).First().Column("AccountKey").GetValueAs<int>();
            return acc;
        }

        /// <summary>
        /// Corrects LE Roles
        /// </summary>
        /// <param name="accountKey"></param>
        public void CorrectAccountRolesWithInvalidIDNumbers(int accountKey)
        {
            var leRoles = base.GetLegalEntityRoles(accountKey);
            var le = (from r in leRoles
                      where IDNumbers.ValidateID(r.IDNumber) == false && r.LegalEntityTypeKey == (int)LegalEntityTypeEnum.NaturalPerson
                      select r.LegalEntityKey).ToList();
            foreach (int i in le)
            {
                //get a new dob
                var date = DateTime.Now.AddYears(-25);
                var month = date.Month.ToString();
                month = month.Length == 1 ? string.Format(@"0{0}", month) : month;
                var day = date.Day.ToString();
                day = day.Length == 1 ? string.Format(@"0{0}", day) : day;
                string dateString = string.Format(@"{0}{1}{2}", date.Year.ToString().Substring(2, 2), month, day);
                var idNumber = IDNumbers.GetNextIDNumber(dateString);

                //update le idnumber
                legalEntityService.UpdateLegalEntityIDNumber(idNumber, i);

                //Set the Date of Birth
                legalEntityService.UpdateDateOfBirth(i, date);
            }
        }

        /// <summary>
        /// Gets an offer of the type provided against an account with the desired status
        /// </summary>
        /// <param name="accountKey"></param>
        /// <param name="offerType"></param>
        /// <offerStatus></offerStatus>
        /// <returns></returns>
        public int GetOfferForAccount(int accountKey, OfferTypeEnum offerType, OfferStatusEnum offerStatus)
        {
            var results = base.GetOffersByAccountKeyAndStatus(accountKey.ToString(), offerStatus);
            int offerKey = (from r in results
                            where r.Column("OfferTypeKey").GetValueAs<int>() == (int)offerType
                            select r.Column("OfferKey").GetValueAs<int>()).FirstOrDefault();
            return offerKey;
        }

        public void GetMortageLoanDetailsForAccount(int accountKey)
        {
        }

        /// <summary>
        /// Fethces a random account with one main applicant
        /// </summary>
        /// <returns>AccountKey</returns>
        public Automation.DataModels.Account GetVariableLoanAccountByMainApplicantCount(int roleCount, int recordCount, AccountStatusEnum status)
        {
            int accountKey = (from r in base.GetRandomVariableLoanAccountByMainApplicantCount(roleCount, recordCount, status)
                              select r.Column("AccountKey").GetValueAs<int>()).FirstOrDefault();
            var account = status == AccountStatusEnum.Closed ? GetAccountByKeySimpleSQL(accountKey) : GetAccountByKey(accountKey);
            return account;
        }

        /// <summary>
        /// Fethces a random personal loans account
        /// </summary>
        /// <returns>AccountKey</returns>
        public Automation.DataModels.Account GetPersonalLoanAccount()
        {
            int accountKey = (from r in base.GetPersonalLoanAccount()
                              select r.Column("AccountKey").GetValueAs<int>()).FirstOrDefault();
            return GetAccountByKey(accountKey);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="productKey"></param>
        /// <param name="accountStatusKey"></param>
        /// <returns></returns>
        public Automation.DataModels.Account GetRandomAccountWithNegativeBalance(ProductEnum productKey, AccountStatusEnum accountStatusKey)
        {
            var accounts = (from r in base.GetAccountsWithCurrentBalance()
                            where r.AccountStatusKey == accountStatusKey && r.RRR_ProductKey == productKey && r.CurrentBalance < -1
                            select r).DefaultIfEmpty<Automation.DataModels.Account>();
            return accounts.SelectRandom();
        }

        /// <summary>
        /// Adds a role onto an account of the type specified if it does not already exist against the account. It will return the legal entity key of the
        /// LE that was added to use in the role.
        /// </summary>
        /// <param name="accountKey">Account Number</param>
        /// <param name="roleType">Role to add</param>
        /// <param name="status">General Status</param>
        /// <returns></returns>
        public int AddRoleToAccount(int accountKey, RoleTypeEnum roleType, GeneralStatusEnum status, int legalEntityKey = 0)
        {
            //we need to add a role onto the account
            if (legalEntityKey == 0)
            {
                string idNumber = IDNumbers.GetNextIDNumber();
                var random = new Random();
                string emailAddress = string.Format(@"testemail-{0}@test.co.za", random.Next(0, 25000).ToString());
                int newLegalEntityKey = legalEntityService.CreateNewLegalEntity(emailAddress, idNumber);
                legalEntityKey = newLegalEntityKey;
            }
            //insert the role
            base.InsertRole(accountKey, legalEntityKey, roleType, status);
            return legalEntityKey;
        }

        /// <summary>
        /// Returns a list of all of the active bank accounts linked to the legal entities on the account.
        /// </summary>
        /// <param name="accountKey">AccountKey</param>
        /// <returns>Returns a list of Bank Accounts</returns>
        public List<Automation.DataModels.BankAccount> GetBankAccountRecordsForAccount(int accountKey)
        {
            var accountRoles = (from r in legalEntityService.GetAllLegalEntityRoles(accountKey)
                                where r.LegalEntityBankAccounts != null
                                select r).AsEnumerable();
            var leba = accountRoles.SelectMany(a => a.LegalEntityBankAccounts)
                .Where(b => b.GeneralStatusKey == GeneralStatusEnum.Active);
            return (from l in leba where l.BankAccount != null select l.BankAccount).ToList();
        }

        /// <summary>
        /// This will fetch an account with a current balance greater than 0 AND that is not currently undergoing debt counselling.
        /// </summary>
        /// <param name="productKey">Product</param>
        /// <param name="accountStatus">Account Status</param>
        /// <returns></returns>
        public Automation.DataModels.Account GetRandomMortgageLoanAccountWithPositiveBalance(ProductEnum productKey, AccountStatusEnum accountStatus)
        {
            var account = (from r in base.GetAccountsWithCurrentBalance()
                           where r.AccountStatusKey == accountStatus && r.RRR_ProductKey == productKey && r.CurrentBalance > 50000
                           select r).DefaultIfEmpty().SelectRandom();

            account.FinancialServices = base.GetLoanFinancialServices(account.AccountKey);

            return account;
        }

        public Automation.DataModels.Account GetRandomMortgageLoanAccountWithRateAdjustment(ProductEnum productEnum, AccountStatusEnum accountStatus, bool isSurchargeRateAdjustment)
        {
            var accounts = from account in base.GetRandomMortgageAccountFinancialServicesWithRateAdjustments()
                           where account.AccountStatusKey == accountStatus
                                && account.ProductKey == productEnum
                                 && account.FinancialService.IsSurchargeRateAdjustment == isSurchargeRateAdjustment
                           select account;
            var acc = accounts.FirstOrDefault();
            acc.FinancialServices = base.GetLoanFinancialServices(acc.AccountKey);
            return acc;
        }

        /// <summary>
        /// Gets an account, allowing for a filter on whether or not the account should have a fixed payment or not.
        /// </summary>
        /// <param name="productKey"></param>
        /// <param name="accountStatusKey"></param>
        /// <param name="fixedPayment"></param>
        /// <returns></returns>
        public Automation.DataModels.Account GetRandomAccountWithFixedPayment(ProductEnum productKey, AccountStatusEnum accountStatusKey, bool fixedPayment)
        {
            var accounts = (from r in base.GetAccountsWithCurrentBalance()
                            where r.AccountStatusKey == accountStatusKey
                            && r.RRR_ProductKey == productKey
                            && r.CurrentBalance > 0 && r.SubsidyClient != 1
                            select r).DefaultIfEmpty();
            return accounts.Where(x => x.FixedPayment.Equals(0) == fixedPayment).SelectRandom();
        }

        /// <summary>
        /// This method will bring back accounts that have a subsidy stop order.
        /// </summary>
        /// <param name="fixedPayment">true = has a fixed payment, false = no fixed payment</param>
        /// <returns></returns>
        public Automation.DataModels.Account GetOpenAccountWithSubsidyStopOrder(bool fixedPayment)
        {
            var account = (from a in base.GetAccountWithSubsidyStopOrders(fixedPayment)
                           where a.SubsidyAmount < a.TotalInstalment
                           select a).SelectRandom();
            return account;
        }

        /// <summary>
        /// Fethces a random account with the specified product and spv.
        /// </summary>
        /// <param name="productKey">ProductKey</param>
        /// <param name="spvKey">SPVKey</param>
        /// <returns></returns>
        public Automation.DataModels.Account GetRandomAccountInSPV(ProductEnum productKey, int spvKey)
        {
            var accounts = (from r in base.GetAccountsWithCurrentBalance()
                            where r.RRR_ProductKey == productKey
                            && r.SPVKey == spvKey
                            && r.CurrentBalance > 0
                            select r).DefaultIfEmpty();
            return accounts.SelectRandom();
        }

        /// <summary>
        /// Fethces a random account with the specified product,spv and detail type.
        /// </summary>
        /// <param name="productKey"></param>
        /// <param name="spvKey"></param>
        /// <param name="detailType"></param>
        /// <param name="detailClass"></param>
        /// <returns></returns>
        public Automation.DataModels.Account GetRandomAccountInSPVWithoutDetailType(ProductEnum productKey, int spvKey, DetailTypeEnum detailType, DetailClassEnum detailClass)
        {
            var accounts = (from r in base.GetAccountsWithCurrentBalance()
                            where r.RRR_ProductKey == productKey
                            && r.SPVKey == spvKey
                            && r.CurrentBalance > 0
                            select r).DefaultIfEmpty();
            foreach (var acc in accounts)
                acc.LoanDetail = detailTypeService.GetLoanDetailRecord(detailType, acc.AccountKey, detailClass);
            return accounts.Where(x => x.LoanDetail == null).SelectRandom();
        }

        /// <summary>
        /// Fetches a random account without the specified product,spv and detail type.
        /// </summary>
        /// <param name="productKey"></param>
        /// <param name="spvKey"></param>
        /// <param name="detailType"></param>
        /// <param name="detailClass"></param>
        /// <returns></returns>
        public Automation.DataModels.Account GetRandomAccountInSPVWithDetailType(ProductEnum productKey, int spvKey, DetailTypeEnum detailType, DetailClassEnum detailClass)
        {
            var accounts = (from r in base.GetAccountsWithCurrentBalance()
                            where r.RRR_ProductKey == productKey
                            && r.SPVKey == spvKey
                            && r.CurrentBalance > 0
                            select r).DefaultIfEmpty();
            foreach (var acc in accounts)
                acc.LoanDetail = detailTypeService.GetLoanDetailRecord(detailType, acc.AccountKey, detailClass);
            return accounts.Where(x => x.LoanDetail != null).SelectRandom();
        }

        /// <summary>
        /// Checks the mortgage loan purpose of the account and then finds the original accepted offer linked to the account.
        /// </summary>
        /// <param name="acc"></param>
        /// <returns></returns>
        public QueryResults GetOriginalAcceptedOfferForAccount(Automation.DataModels.Account acc)
        {
            int mlPurpose = (from ml in acc.FinancialServices select ml.MortgageLoanPurpose).FirstOrDefault();
            OfferTypeEnum offerType = OfferTypeEnum.Unknown;
            switch (mlPurpose)
            {
                case 2:
                    offerType = OfferTypeEnum.SwitchLoan;
                    break;

                case 3:
                    offerType = OfferTypeEnum.NewPurchase;
                    break;

                case 4:
                    offerType = OfferTypeEnum.Refinance;
                    break;
            }
            int offerKey = GetOfferForAccount(acc.AccountKey, offerType, OfferStatusEnum.Accepted);
            return base.GetLatestOfferInformationByOfferKey(offerKey);
        }

        public int GetLifeAccountKey(int noAssureLifeRoles, LifePolicyStatusEnum lifePolicyStatus, bool premiumCalcShouldIncrease = true)
        {
            var accountkey = 0;
            if (premiumCalcShouldIncrease)
                accountkey = (from acc in base.GetLifePolicyAccounts(noAssureLifeRoles, lifePolicyStatus, true, false)
                              select acc.AccountKey).FirstOrDefault();
            else
                accountkey = (from acc in base.GetLifePolicyAccounts(noAssureLifeRoles, lifePolicyStatus, false, true)
                              select acc.AccountKey).FirstOrDefault();
            return accountkey;
        }

        /// <summary>
        /// Fetches a random interest only account for the specified product.
        /// </summary>
        /// <param name="productKey">ProductKey</param>
        /// <returns></returns>
        public Automation.DataModels.Account GetRandomInterestOnlyAccount(ProductEnum productKey)
        {
            var account = (from r in base.GetInterestOnlyAccounts()
                           where r.RRR_ProductKey == productKey
                           && r.CurrentBalance > 0
                           select r).DefaultIfEmpty().SelectRandom();

            account.FinancialServices = base.GetLoanFinancialServices(account.AccountKey);

            return account;
        }

        /// <summary>
        /// Gets an account for MarkNonPerforming tests
        /// </summary>
        /// <param name="isMarkedNonPerforming">check for a FinancialAttribute record of Source: Suspended Interest and Type: Reversal Provision</param>
        /// <param name="hasFurtherLendingOffer">check for an open Further Lending Offer</param>
        /// <param name="hasDetails">check for Detail record of Type (11,180,275,299,592,227,581,582,583,584,590)</param>
        /// <param name="productKeys">filter loans by ProductKey</param>
        /// <returns>Account record</returns>
        public Automation.DataModels.Account GetAccountForNonPerformingLoanTests(bool isMarkedNonPerforming, bool hasFurtherLendingOffer, bool hasDetails, params int[] productKeys)
        {
            var account = base.GetAccountForNonPerformingLoanTests(isMarkedNonPerforming, hasFurtherLendingOffer, hasDetails, productKeys);
            return GetAccountByKey(account.AccountKey);
        }

        /// <summary>
        /// Gets idnumber for personal loan offer
        /// <returns>IDNUmber</returns>
        public string GetIDNumberforExternalRoleOnOffer(int offerkey)
        {
            var idnumber = string.Empty;
            idnumber = base.GetIDNumberforExternalRoleOnOffer(offerkey);
            return idnumber;
        }

        public QueryResults GetOpenFinancialServiceRecordByType(int accountKey, FinancialServiceTypeEnum financialServiceType)
        {
            return base.GetOpenFinancialServiceRecordByType(accountKey, financialServiceType);
        }

        /// <summary>
        /// Returns a result set of personal loan accounts which have a credit life policy
        /// </summary>
        /// <returns></returns>
        public QueryResults GetPersonalLoanAccountWithACreditLifePolicy()
        {
            return base.GetPersonalLoanAccountWithACreditLifePolicy();
        }

        /// <summary>
        /// Returns a result set of personal loan accounts which don't have a credit life policy
        /// </summary>
        /// <returns></returns>
        public QueryResults GetPersonalLoanAccountWithoutACreditLifePolicy()
        {
            return base.GetPersonalLoanAccountWithoutACreditLifePolicy();
        }

        /// <summary>
        /// Returns the balance for a Personal Loan
        /// </summary>
        /// <returns>The Balance of a Personal Loan</returns>
        public double GetCurrentPersonalLoanBalance(int accountKey)
        {
            return base.GetCurrentPersonalLoanBalance(accountKey);
        }

        public Automation.DataModels.Account GetOpenMortgageLoanAccountInSPV(int SPVKey)
        {
            return base.GetOpenMortgageLoanAccountInSPV(SPVKey);
        }

        /// <summary>
        /// Calls method GetNonSAHLAccountWithDetailType(detailType), passing in the detail type parameter.
        /// </summary>
        /// <param name="detailType"></param>
        /// <returns></returns>
        public Automation.DataModels.Account GetNonSAHLAccountWithDetailType(DetailTypeEnum detailType)
        {
            return base.GetNonSAHLAccountWithDetailType(detailType);
        }

        public int GetPersonalLoanAccountWithStageTransition()
        {
            return base.GetPersonalLoanAccountWithStageTransition();
        }

        public int GetPersonalLoanAccountWithoutStageTransition()
        {
            return base.GetPersonalLoanAccountWithoutStageTransition();
        }

        public Automation.DataModels.Account GetAccountRecord(int accountKey)
        {
            return base.GetAccountByKeySimpleSQL(accountKey);
        }

        public QueryResults GetOpenAccountAndAssociatedOfferWithoutAGivenProductType(OfferTypeEnum offerType, ProductEnum productType)
        {
            return base.GetOpenAccountAndAssociatedOfferWithoutAGivenProductType(offerType, productType);
        }

        public QueryResults GetOpenLifeAccountWithAssuredLife()
        {
            return base.GetOpenLifeAccountWithAssuredLife();
        }

        public IEnumerable<Account> GetAccounts(Predicate<Account> predicate)
        {
            foreach (var acc in base.Load<Account>())
            {
                if (predicate.Invoke(acc))
                    yield return acc;
            }
        }

        public IEnumerable<Role> GetRoles(Predicate<Role> predicate)
        {
            foreach (var r in base.Load<Role>())
            {
                if (predicate.Invoke(r))
                    yield return r;
            }
        }
    }
}