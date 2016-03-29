using Castle.ActiveRecord.Queries;
using SAHL.Common.Attributes;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.Interfaces.Service;
using SAHL.Common.BusinessModel.Service;
using SAHL.Common.CacheData;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.DataAccess;
using SAHL.Common.DomainMessages;
using SAHL.Common.Factories;
using SAHL.Common.Globals;
using SAHL.Common.Security;
using SAHL.Common.Service.Interfaces;
using SAHL.Common.X2.BusinessModel.Interfaces;
using SAHL.Core.SystemMessages;
using SAHL.Services.Interfaces.LifeDomain.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace SAHL.Common.BusinessModel.Repositories
{
    /// <summary>
    ///
    /// </summary>
    [FactoryType(typeof(ILifeRepository))]
    public class LifeRepository : AbstractRepositoryBase, ILifeRepository
    {
        public LifeRepository()
        {
            if (castleTransactionService == null)
            {
                castleTransactionService = new CastleTransactionsService();
            }
        }

        public LifeRepository(ICastleTransactionsService castleTransactionService)
        {
            this.castleTransactionService = castleTransactionService;
        }

        private ICastleTransactionsService castleTransactionService;

        public double CalculateLifePremiumForUnsecuredLending(double amount)
        {
            IControlRepository controlRepo = RepositoryFactory.GetRepository<IControlRepository>();
            IControl lifePremium = controlRepo.GetControlByDescription("PersonalLoanCreditLifePremium");
            return (amount * lifePremium.ControlNumeric.Value);
        }

        /// <summary>
        /// Creates and Empty LifePolicy object
        /// </summary>
        /// <returns>ILifePolicy</returns>
        public ILifePolicy CreateEmptyLifePolicy()
        {
            return base.CreateEmpty<ILifePolicy, LifePolicy_DAO>();
        }

        /// <summary>
        /// Creates and Empty Callback object
        /// </summary>
        /// <returns>ICallback</returns>
        public ICallback CreateEmptyCallback()
        {
            return base.CreateEmpty<ICallback, Callback_DAO>();
        }

        /// <summary>
        /// Saves a Callback object
        /// </summary>
        /// <param name="callback"></param>
        public void SaveCallback(ICallback callback)
        {
            base.Save<ICallback, Callback_DAO>(callback);
        }

        /// <summary>
        /// Gets an LifePolicy object by the AccountKey of the LifePolicy.
        /// </summary>
        /// <param name="Key">The integer AccountKey Key.</param>
        /// <returns>The <see cref="ILifePolicy">LifePolicy found using the supplied AccountKey, returns null if no LifePolicy is found.</see></returns>
        public ILifePolicy GetLifePolicyByAccountKey(int Key)
        {
            string HQL = UIStatementRepository.GetStatement("Life", "GetLifePolicyByAccountKey");

            SimpleQuery query = new SimpleQuery(typeof(SAHL.Common.BusinessModel.DAO.LifePolicy_DAO), HQL, Key);

            object o = LifePolicy_DAO.ExecuteQuery(query);
            LifePolicy_DAO[] LifePolicies = o as LifePolicy_DAO[];
            if (LifePolicies != null && LifePolicies.Length == 1)
            {
                return new LifePolicy(LifePolicies[0]);
            }
            return null;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="TextStatementTypes"></param>
        /// <returns></returns>
        public IReadOnlyEventList<ITextStatement> GetTextStatementsForTypes(int[] TextStatementTypes)
        {
            return TextStatement.GetTextStatementsForTypes(TextStatementTypes);
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public IDictionary<string, int> GetCancellationTypes()
        {
            IDictionary<string, int> dict = new Dictionary<string, int>();

            dict.Add("Cancel from Inception (within 30 days)", 4);

            dict.Add("Cancel with Authorisation (more than 30 days, full refund)", 4);

            dict.Add("Cancel with ProRata (more than 30 days)", 5);

            dict.Add("Cancel with No Refund", 15);

            return dict;
        }

        /// <summary>
        /// calculate the next yearly anniversary of the policy based on the policy number
        /// </summary>
        /// <param name="accountkey"></param>
        /// <param name="currentDate"></param>
        /// <returns></returns>
        public DateTime GetNextLifeAnniversaryDate(int accountkey, DateTime currentDate)
        {
            DateTime anniversaryDate = System.DateTime.Now;
            string query = UIStatementRepository.GetStatement("Life", "GetNextLifeAnniversaryDate");

            ParameterCollection parameters = new ParameterCollection();
            parameters.Add(new SqlParameter("@PolicyNumber", accountkey));
            parameters.Add(new SqlParameter("@CurrentDate", currentDate));

            object o = castleTransactionService.ExecuteScalarOnCastleTran(query, typeof(GeneralStatus_DAO), parameters);
            if (o != null)
            {
                anniversaryDate = (DateTime)o;
            }

            return anniversaryDate;
        }

        /// <summary>
        /// Returns the Pipeline Status of the Loan
        /// </summary>
        /// <param name="loanNumber"></param>
        /// <returns>string</returns>

        public string GetLoanPipelineStatus(int loanNumber)
        {
            string sPipelineStatus = null;

            string query = UIStatementRepository.GetStatement("Life", "GetLoanPipelineStatus");

            ParameterCollection parameters = new ParameterCollection();
            parameters.Add(new SqlParameter("@AccountKey", loanNumber));

            object o = castleTransactionService.ExecuteScalarOnCastleTran(query, typeof(GeneralStatus_DAO), parameters);
            if (o != null)
                sPipelineStatus = (string)o;

            return sPipelineStatus;
        }

        /// <summary>
        /// Get the LifePremiumHistory object
        /// </summary>
        /// <param name="accountKey">Life Account Key</param>
        /// <returns></returns>
        public IList<ILifePremiumHistory> GetLifePremiumHistory(int accountKey)
        {
            string HQL = UIStatementRepository.GetStatement("Life", "GetLifePremiumHistory");
            SimpleQuery<LifePremiumHistory_DAO> q = new SimpleQuery<LifePremiumHistory_DAO>(HQL, accountKey);
            LifePremiumHistory_DAO[] res = q.Execute();

            IList<ILifePremiumHistory> retval = new List<ILifePremiumHistory>();

            for (int i = 0; i < res.Length; i++)
            {
                retval.Add(new LifePremiumHistory(res[i]));
            }
            return retval;
        }

        /// <summary>
        /// Returns whether an assured life is over exposed in terms of the group exposure policy
        /// </summary>
        /// <param name="legalEntity">ILegalEntity</param>
        /// <returns>boolean</returns>
        public bool IsLifeOverExposed(ILegalEntity legalEntity)
        {
            bool lifeIsOverExposed = false;

            string query = UIStatementRepository.GetStatement("Life", "isLifeOverExposed");

            // Create a collection
            ParameterCollection parameters = new ParameterCollection();

            //Add the required parameters
            parameters.Add(new SqlParameter("@LegalentityKey", legalEntity.Key));

            // execute
            object o = castleTransactionService.ExecuteScalarOnCastleTran(query, typeof(GeneralStatus_DAO), parameters);

            // Get the Return Values
            lifeIsOverExposed = Convert.ToBoolean(o);

            return lifeIsOverExposed;
        }

        /// <summary>
        /// Returns whether life is a condition of a loan
        /// </summary>
        /// <param name="loanAccountKey"></param>
        /// <returns></returns>
        public bool IsLifeConditionOfLoan(int loanAccountKey)
        {
            bool lifeIsConditionOfLoan = false;

            string query = UIStatementRepository.GetStatement("Life", "isLifeConditionOfLoan");

            // Create a collection
            ParameterCollection parameters = new ParameterCollection();

            //Add the required parameters
            parameters.Add(new SqlParameter("@AccountKey", loanAccountKey));

            // execute
            object o = castleTransactionService.ExecuteScalarOnCastleTran(query, typeof(GeneralStatus_DAO), parameters);

            // Get the Return Values
            int recs = o == null ? 0 : Convert.ToInt32(o);
            if (recs > 0)
                lifeIsConditionOfLoan = true;

            return lifeIsConditionOfLoan;
        }

        /// <summary>
        /// Returns whether a user is an "Admin" user
        /// </summary>
        /// <param name="currentPrincipal">Current SAHL Principal</param>
        /// <returns></returns>
        public bool IsAdminUser(SAHL.Common.Security.SAHLPrincipal currentPrincipal)
        {
            bool adminUser = false;

            if (currentPrincipal.IsInRole("LifeManager") || currentPrincipal.IsInRole("LifeAdmin"))
                adminUser = true;

            return adminUser;
        }

        /// <summary>
        /// Returns whether a loan is a Regent loan or not
        /// </summary>
        /// <param name="accountKey">accountKey</param>
        /// <returns></returns>
        public bool IsRegentLoan(int accountKey)
        {
            bool regentLoan = false;

            string query = UIStatementRepository.GetStatement("Life", "isRegentLoan");

            // Create a collection
            ParameterCollection parameters = new ParameterCollection();

            //Add the required parameters
            parameters.Add(new SqlParameter("@AccountKey", accountKey));

            // execute
            object o = castleTransactionService.ExecuteScalarOnCastleTran(query, typeof(GeneralStatus_DAO), parameters);

            // Get the Return Values
            regentLoan = o == null ? false : ((int)o > 0 ? true : false);

            return regentLoan;
        }

        /// <summary>
        /// Creates and Empty LifeInsurableInterest object
        /// </summary>
        /// <returns>ILifeInsurableInterest</returns>
        public ILifeInsurableInterest CreateEmptyLifeInsurableInterest()
        {
            return base.CreateEmpty<ILifeInsurableInterest, LifeInsurableInterest_DAO>();
        }

        /// <summary>
        /// Recalculates and Updates the Premium on the LifePolicy/Offer
        /// </summary>
        /// <param name="accountLifePolicy">The <see cref="IAccountLifePolicy"/>.</param>
        /// <param name="recalcDiscountFactor">True or False</param>
        public void RecalculateSALifePremium(IAccountLifePolicy accountLifePolicy, bool recalcDiscountFactor)
        {
            string sRecalcDiscountFactor = recalcDiscountFactor ? "Yes" : "No";

            // Get Query
            string query = UIStatementRepository.GetStatement("Life", "RecalculateSALifePremium");

            // Add the required parameters
            SqlParameter outputMsgParam = new SqlParameter("@Msg", SqlDbType.VarChar, 1024);
            outputMsgParam.Direction = ParameterDirection.Output;
            outputMsgParam.Value = null;
            SqlParameter currentSumAssuredOutput = new SqlParameter("@CurrentSumAssured", SqlDbType.Float);
            currentSumAssuredOutput.Direction = ParameterDirection.Output;
            SqlParameter monthlyInstalmentOutput = new SqlParameter("@MonthlyInstalment", SqlDbType.Float);
            monthlyInstalmentOutput.Direction = ParameterDirection.Output;
            SqlParameter yearlyPremiumOutput = new SqlParameter("@YearlyPremium", SqlDbType.Float);
            yearlyPremiumOutput.Direction = ParameterDirection.Output;
            SqlParameter deathBenefitOutput = new SqlParameter("@DeathBenefit", SqlDbType.Float);
            deathBenefitOutput.Direction = ParameterDirection.Output;
            SqlParameter iPBenefitOutput = new SqlParameter("@IPBenefit", SqlDbType.Float);
            iPBenefitOutput.Direction = ParameterDirection.Output;

            ParameterCollection parameters = new ParameterCollection();
            parameters.Add(new SqlParameter("@PolicyNumber", accountLifePolicy.Key));
            parameters.Add(new SqlParameter("@RecalcDiscountFactor", sRecalcDiscountFactor));
            parameters.Add(new SqlParameter("@UpdateDatabase", true));
            parameters.Add(new SqlParameter("@AgeList", System.DBNull.Value));
            parameters.Add(new SqlParameter("@LifePolicyTypeKey", System.DBNull.Value));

            parameters.Add(currentSumAssuredOutput);
            parameters.Add(monthlyInstalmentOutput);
            parameters.Add(yearlyPremiumOutput);
            parameters.Add(deathBenefitOutput);
            parameters.Add(iPBenefitOutput);
            parameters.Add(outputMsgParam);

            // Execute Query
            castleTransactionService.ExecuteNonQueryOnCastleTran(query, typeof(GeneralStatus_DAO), parameters);
        }

        /// <summary>
        /// Recalculates and Updates the Premium on the LifePolicy/Offer
        /// </summary>
        /// <param name="accountKey">The <see cref="IAccountLifePolicy"/>.</param>
        /// <param name="lifePolicyTypeKey">int</param>
        /// <param name="recalcDiscountFactor">True or False</param>
        /// <param name="ageList">comma delimited string of ages</param>
        /// <param name="currentSumAssured">Output : Current Sum Asured</param>
        /// <param name="monthlyInstalment">Output : Monthly Instalment</param>
        /// <param name="yearlyPremium">Output : Yearly Premium</param>
        /// <param name="deathBenefitPremium">Output : Death Benefit Premium</param>
        /// <param name="ipBenefitPremium">Output : IP Benefit Premium</param>
        public void RecalculateSALifePremiumQuote(int accountKey, int lifePolicyTypeKey, bool recalcDiscountFactor, string ageList, out double currentSumAssured, out double monthlyInstalment, out double yearlyPremium, out double deathBenefitPremium, out double ipBenefitPremium)
        {
            //assign initial values for output params
            currentSumAssured = 0;
            monthlyInstalment = 0;
            yearlyPremium = 0;
            deathBenefitPremium = 0;
            ipBenefitPremium = 0;

            string sRecalcDiscountFactor = recalcDiscountFactor ? "Yes" : "No";

            DataTable dtResults = new DataTable();

            string query = UIStatementRepository.GetStatement("Life", "RecalculateSALifePremium");

            // Create a collection
            ParameterCollection parameters = new ParameterCollection();

            //Add the required parameters

            SqlParameter outputMsgParam = new SqlParameter("@Msg", SqlDbType.VarChar, 1024);
            outputMsgParam.Direction = ParameterDirection.Output;
            outputMsgParam.Value = null;
            SqlParameter currentSumAssuredOutput = new SqlParameter("@CurrentSumAssured", SqlDbType.Float);
            currentSumAssuredOutput.Direction = ParameterDirection.Output;
            SqlParameter monthlyInstalmentOutput = new SqlParameter("@MonthlyInstalment", SqlDbType.Float);
            monthlyInstalmentOutput.Direction = ParameterDirection.Output;
            SqlParameter yearlyPremiumOutput = new SqlParameter("@YearlyPremium", SqlDbType.Float);
            yearlyPremiumOutput.Direction = ParameterDirection.Output;
            SqlParameter deathBenefitOutput = new SqlParameter("@DeathBenefit", SqlDbType.Float);
            deathBenefitOutput.Direction = ParameterDirection.Output;
            SqlParameter iPBenefitOutput = new SqlParameter("@IPBenefit", SqlDbType.Float);
            iPBenefitOutput.Direction = ParameterDirection.Output;

            parameters.Add(new SqlParameter("@PolicyNumber", accountKey));
            parameters.Add(new SqlParameter("@RecalcDiscountFactor", sRecalcDiscountFactor));
            parameters.Add(new SqlParameter("@UpdateDatabase", false));
            parameters.Add(new SqlParameter("@AgeList", ageList));
            parameters.Add(new SqlParameter("@LifePolicyTypeKey", lifePolicyTypeKey));
            parameters.Add(currentSumAssuredOutput);
            parameters.Add(monthlyInstalmentOutput);
            parameters.Add(yearlyPremiumOutput);
            parameters.Add(deathBenefitOutput);
            parameters.Add(iPBenefitOutput);
            parameters.Add(outputMsgParam);

            // execute
            castleTransactionService.ExecuteNonQueryOnCastleTran(query, typeof(GeneralStatus_DAO), parameters);

            currentSumAssured = currentSumAssuredOutput.Value != DBNull.Value ? Convert.ToDouble(currentSumAssuredOutput.Value) : 0;
            monthlyInstalment = monthlyInstalmentOutput.Value != DBNull.Value ? Convert.ToDouble(monthlyInstalmentOutput.Value) : 0;
            yearlyPremium = yearlyPremiumOutput.Value != DBNull.Value ? Convert.ToDouble(yearlyPremiumOutput.Value) : 0;
            deathBenefitPremium = deathBenefitOutput.Value != DBNull.Value ? Convert.ToDouble(deathBenefitOutput.Value) : 0;
            ipBenefitPremium = iPBenefitOutput.Value != DBNull.Value ? Convert.ToDouble(iPBenefitOutput.Value) : 0;
        }

        /// <summary>
        /// Returns the Policy Holder LegalEntityKey and AddressKey for the specified AccountKey
        /// </summary>
        /// <param name="AccountKey">The integer AccountKey Key.</param>
        /// <param name="LegalEntityKey">The output integer LegalEntityKey.</param>
        /// <param name="AddressKey">The output integer AddressKey.</param>
        /// <returns>The LegalEntityKey and AddressKey of the policyholder</returns>
        public void GetPolicyHolderDetails(int AccountKey, out int LegalEntityKey, out int AddressKey)
        {
            ILegalEntity legalEntity = null;
            IAddress address = null;
            LegalEntityKey = -1;
            AddressKey = -1;

            IApplicationRepository applicationRepo = RepositoryFactory.GetRepository<IApplicationRepository>();
            IAccountRepository accountRepo = RepositoryFactory.GetRepository<IAccountRepository>();

            IAccountLifePolicy lifePolicyAccount = accountRepo.GetAccountByKey(AccountKey) as IAccountLifePolicy;
            IApplicationLife lifePolicyApplication = lifePolicyAccount.CurrentLifeApplication;// GetLatestOpenApplicationByType(ApplicationTypes.Life) as IApplicationLife;
            if (lifePolicyApplication == null)
                return;

            IMortgageLoanAccount loanAccount = lifePolicyAccount.MortgageLoanAccount;

            // 1. Look for PolicyHolderLEKey and use that as the Legal Entity Key
            if (lifePolicyAccount.LifePolicy == null)
                legalEntity = lifePolicyApplication.PolicyHolderLegalEntity;
            else
                legalEntity = lifePolicyAccount.LifePolicy.PolicyHolderLE;

            // 2. If no PolicyHolderLEKey then look for the first Assured Life (active role & not deceased) on the account
            if (legalEntity == null)
            {
                foreach (IRole role in lifePolicyAccount.Roles)
                {
                    if (role.GeneralStatus.Key == Convert.ToInt32(SAHL.Common.Globals.GeneralStatuses.Active)
                        && role.RoleType.Key == Convert.ToInt32(SAHL.Common.Globals.RoleTypes.AssuredLife)) //&& role.LegalEntity.LegalEntityStatus.Key = Convert.ToInt32(SAHL.Common.Globals.LegalEntityStatus.Alive)
                    {
                        legalEntity = role.LegalEntity;
                        break;
                    }
                }
            }

            // 3. If still no LegalEntity found then look for the first Main Applicant (active role & not deceased) on the Loan
            if (legalEntity == null)
                legalEntity = loanAccount.MainApplicants[0]; // todo : use ActiveMainApplicants[0]

            // Now that we have the LegalEntityKey, we need to find the Address to use

            // 4. First Look for an Address on the Mailing Address table for the Loan
            //address  = loanAccount.MailingAddress,

            if (address == null)
            {
                IAddress postalAddress = null, residentialAddress = null;

                // Look for the First Postal Address (or Residential if no Postal) for the LegalEntity
                bool postalAddressFound = false, residentialAddressFound = false;
                foreach (ILegalEntityAddress leAddress in legalEntity.LegalEntityAddresses)
                {
                    if (leAddress.AddressType.Key == Convert.ToInt32(SAHL.Common.Globals.AddressTypes.Postal) && postalAddressFound == false)
                    {
                        postalAddressFound = true;
                        postalAddress = leAddress.Address;
                        break;
                    }
                    if (leAddress.AddressType.Key == Convert.ToInt32(SAHL.Common.Globals.AddressTypes.Residential) && residentialAddressFound == false)
                    {
                        residentialAddressFound = true;
                        residentialAddress = leAddress.Address;
                    }
                }
                if (postalAddressFound)
                    address = postalAddress;
                else
                    address = residentialAddress;
            }

            if (legalEntity != null)
                LegalEntityKey = legalEntity.Key;
            if (address != null)
                AddressKey = address.Key;
        }

        /// <summary>
        /// Handles the cancelling of a Life Policy
        /// </summary>
        /// <param name="financialServiceKey"></param>
        /// <param name="policyStatusKey"></param>
        /// <returns>int</returns>
        public void CancelLifePolicy(int financialServiceKey, int policyStatusKey)
        {
            ParameterCollection prms = new ParameterCollection();
            prms.Add(new SqlParameter("@LifeFinancialServiceKey", financialServiceKey));
            prms.Add(new SqlParameter("@PolicyStatusKey", policyStatusKey));

            this.castleTransactionService.ExecuteHaloAPI_uiS_OnCastleTranForUpdate("Life", "CancelLifePolicy", prms);
        }

        /// <summary>
        /// Returns the IInsurer for the specified Insurer Description
        /// </summary>
        /// <param name="InsurerDescription"></param>
        /// <returns></returns>
        public IInsurer GetInsurerByDescription(string InsurerDescription)
        {
            string HQL = UIStatementRepository.GetStatement("Life", "GetInsurerByDescription");
            SimpleQuery query = new SimpleQuery(typeof(SAHL.Common.BusinessModel.DAO.Insurer_DAO), HQL, InsurerDescription);

            object o = Insurer_DAO.ExecuteQuery(query);
            Insurer_DAO[] insurers = o as Insurer_DAO[];
            if (insurers != null && insurers.Length >= 1)
            {
                return new Insurer(insurers[0]);
            }
            return null;
        }

        /// <summary>
        /// Returns a list of Life Consultants
        /// </summary>
        /// <returns>IList&lt;IADUser&gt;></returns>
        public IList<IADUser> GetLifeConsultants()
        {
            return GetLifeConsultants("All", false);
        }

        /// <summary>
        /// Returns a list of Life Consultants
        /// </summary>
        /// <param name="includeInactiveConsultants"></param>
        /// <returns>IList&lt;IADUser&gt;></returns>
        public IList<IADUser> GetLifeConsultants(bool includeInactiveConsultants)
        {
            return GetLifeConsultants("All", includeInactiveConsultants);
        }

        /// <summary>
        /// Gets a list of the Life Consultants
        /// </summary>
        /// <param name="adUserName"></param>
        /// <returns>IList&lt;IADUser&gt;></returns>
        public IList<IADUser> GetLifeConsultants(string adUserName)
        {
            return GetLifeConsultants(adUserName, true);
        }

        /// <summary>
        /// Gets a list of the Life Consultants
        /// </summary>
        /// <param name="adUserName"></param>
        /// <param name="includeInactiveConsultants"></param>
        /// <returns>IList&lt;IADUser&gt;></returns>
        private IList<IADUser> GetLifeConsultants(string adUserName, bool includeInactiveConsultants)
        {
            List<IADUser> adUsers = new List<IADUser>();

            IOrganisationStructureRepository osRepo = RepositoryFactory.GetRepository<IOrganisationStructureRepository>();
            IOrganisationStructure parentStructure = osRepo.GetOrganisationStructureForDescription("CCC");
            if (parentStructure != null)
            {
                foreach (IOrganisationStructure childStructure in parentStructure.ChildOrganisationStructures)
                {
                    if (String.Compare(childStructure.Description, "Consultant", true) == 0)
                    {
                        foreach (IADUser user in childStructure.ADUsers)
                        {
                            if (user.LegalEntity != null)
                            {
                                if (includeInactiveConsultants == false && user.GeneralStatusKey.Key == (int)Globals.GeneralStatuses.Inactive)
                                    continue;

                                if (String.Compare(adUserName, "All", true) == 0)
                                    adUsers.Add(user);
                                else if (String.Compare(user.ADUserName, adUserName, true) == 0)
                                {
                                    adUsers.Add(user);
                                    break;
                                }
                            }
                        }
                    }
                }
            }

            if (adUsers.Count > 1) // sort the list by ADUserName
                adUsers.Sort(delegate(IADUser u1, IADUser u2) { return u1.ADUserName.CompareTo(u2.ADUserName); });

            IList<IADUser> adUsersSorted = new List<IADUser>(adUsers);

            return adUsersSorted;
        }

        /// <summary>
        /// Returns the age of the legalentity to be used in the premium calculation.
        /// I am using a sql query here so that i can simulate the exact logic that
        /// l_CalculateAnnualPremium uses when working out the ages
        /// </summary>
        /// <param name="legalEntityNaturalPerson"></param>
        /// <param name="dateAddedToPolicy"></param>
        /// <returns></returns>
        public int GetAssuredLifeAgeForPremiumCalc(ILegalEntityNaturalPerson legalEntityNaturalPerson, DateTime? dateAddedToPolicy)
        {
            int age = -1;
            if (legalEntityNaturalPerson == null || !legalEntityNaturalPerson.DateOfBirth.HasValue)
                return age;

            string query = UIStatementRepository.GetStatement("Life", "GetAssuredLifeAgeForPremiumCalc");

            // Create a collection
            ParameterCollection parameters = new ParameterCollection();

            //Add the required parameters
            object parmDateAddedToPolicy = DBNull.Value;
            if (dateAddedToPolicy.HasValue)
                parmDateAddedToPolicy = dateAddedToPolicy.Value;
            parameters.Add(new SqlParameter("@DateAddedToPolicy", parmDateAddedToPolicy));
            parameters.Add(new SqlParameter("@DOB", legalEntityNaturalPerson.DateOfBirth.Value));

            // execute
            object o = castleTransactionService.ExecuteScalarOnCastleTran(query, typeof(GeneralStatus_DAO), parameters);

            // Get the Return Values
            age = o == null ? -1 : Convert.ToInt32(o);

            return age;
        }

        /// <summary>
        /// Creates and Empty LifeOfferAssignment object
        /// </summary>
        /// <returns>ILifeOfferAssignment</returns>
        public ILifeOfferAssignment CreateEmptyLifeOfferAssignment()
        {
            return base.CreateEmpty<ILifeOfferAssignment, LifeOfferAssignment_DAO>();
        }

        /// <summary>
        /// Saves a LifeOfferAssignment object
        /// </summary>
        /// <param name="lifeOfferAssignment">The <see cref="ILifeOfferAssignment">LifeOfferAssignment to save.</see></param>
        /// <returns></returns>
        public void SaveLifeOfferAssignment(ILifeOfferAssignment lifeOfferAssignment)
        {
            base.Save<ILifeOfferAssignment, LifeOfferAssignment_DAO>(lifeOfferAssignment);
        }

        /// <summary>
        /// Returns the latest lifeofferassignment record for the specified user and loan offer type
        /// </summary>
        /// <param name="ADUserName"></param>
        /// <param name="OfferTypeKey"></param>
        /// <returns></returns>
        public ILifeOfferAssignment GetLatestLifeOfferAssignment(string ADUserName, int OfferTypeKey)
        {
            string query = UIStatementRepository.GetStatement("Life", "GetLatestLifeOfferAssignment");

            // Create a collection
            ParameterCollection parameters = new ParameterCollection();

            //Add the required parameters
            parameters.Add(new SqlParameter("@ADUserName", ADUserName));
            parameters.Add(new SqlParameter("@OfferTypeKey", OfferTypeKey));

            // Execute
            DataSet dsResults = this.castleTransactionService.ExecuteQueryOnCastleTran(query, typeof(GeneralStatus_DAO), parameters);
            if (dsResults != null && dsResults.Tables.Count > 0)
            {
                DataTable dtResults = dsResults.Tables[0];
                if (dtResults.Rows.Count > 0)
                {
                    int lifeOfferAssignmentKey = Convert.ToInt32(dtResults.Rows[0][0]);
                    if (lifeOfferAssignmentKey > 0)
                    {
                        ILifeOfferAssignment lifeOfferAssignment = new LifeOfferAssignment(LifeOfferAssignment_DAO.Find(lifeOfferAssignmentKey));
                        return lifeOfferAssignment;
                    }
                }
            }
            return null;
        }

        public bool CheckLegalEntityQualifies(IAccountLifePolicy accountLifePolicy, ILegalEntityNaturalPerson legalEntityNaturalPerson)
        {
            ILookupRepository lookupRepo = RepositoryFactory.GetRepository<ILookupRepository>();

            // 1. check the person is alive
            if (legalEntityNaturalPerson.LegalEntityStatus.Key == (int)SAHL.Common.Globals.LegalEntityStatuses.Deceased)
            {
                //spc.DomainMessages.Add(new Error("Assured Life cannot be Deceased", "Assured Life cannot be Deceased"));
                //args.Cancel = true;
                return false;
            }

            // 2. check the age between 18 & 65
            int minAge = Convert.ToInt32(lookupRepo.Controls.ObjectDictionary[SAHL.Common.Constants.ControlTable.Life.AssuredLifeMinAge].ControlNumeric);
            int maxAge = Convert.ToInt32(lookupRepo.Controls.ObjectDictionary[SAHL.Common.Constants.ControlTable.Life.AssuredLifeMaxAge].ControlNumeric);
            if (legalEntityNaturalPerson.AgeNextBirthday < minAge || legalEntityNaturalPerson.AgeNextBirthday > maxAge)
            {
                //spc.DomainMessages.Add(new Error("Assured Life must be between the ages of " + minAge + " and " + maxAge, "Assured Life must be between the ages of " + minAge + " and " + maxAge));
                //args.Cancel = true;
                return false;
            }

            // 3. check the group exposure (cannot have more than 2 life policies)
            int maxPolicies = Convert.ToInt32(lookupRepo.Controls.ObjectDictionary[SAHL.Common.Constants.ControlTable.Life.GroupExposureMaxPolicies].ControlNumeric);
            bool overExposed = IsLifeOverExposed(legalEntityNaturalPerson);
            if (overExposed == true)
            {
                //spc.DomainMessages.Add(new Error("The selected Legal Entity is already covered on " + maxPolicies + " Life Policies.", "The selected Legal Entity is already covered on  " + maxPolicies + " Life Policies."));
                //args.Cancel = true;
                return false;
            }

            // 4. check the person doesnt already play a role on the account
            foreach (IRole r in legalEntityNaturalPerson.Roles)
            {
                if (r.Account.Key == accountLifePolicy.Key)
                {
                    //spc.DomainMessages.Add(new Error("The selected Legal entity is already an Assured Life on this Policy.", "The selected Legal entity is already an Assured Life on this Policy."));
                    //args.Cancel = true;
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Updated the account status to closed and writes an application archived stage transition
        /// </summary>
        /// <param name="accountKey"></param>
        /// <param name="applicationKey"></param>
        /// <param name="stageTransitionComments" default="null"></param>
        /// <returns></returns>
        public void CloseLifeApplication(int accountKey, int applicationKey, string stageTransitionComments = null)
        {
            IAccountRepository accRepo = RepositoryFactory.GetRepository<IAccountRepository>();
            IOrganisationStructureRepository osRepo = RepositoryFactory.GetRepository<IOrganisationStructureRepository>();
            IStageDefinitionRepository sDRepo = RepositoryFactory.GetRepository<IStageDefinitionRepository>();

            IADUser adUser = osRepo.GetAdUserForAdUserName("System");

            // Close the Account
            accRepo.UpdateAccount(accountKey, (int)AccountStatuses.Closed, 0, adUser.ADUserName);

            // insert the stage transition
            sDRepo.SaveStageTransition(applicationKey, (int)StageDefinitionGroups.LifeOrigination, Constants.StageDefinitionConstants.ApplicationArchived, stageTransitionComments, adUser);
        }

        /// <summary>
        /// Sends an internal email with the details of the pending call backs
        /// </summary>
        /// <param name="accountKey"></param>
        /// <param name="applicationKey"></param>
        /// <param name="instanceID"></param>
        /// <param name="consultantEmailAddress"></param>
        /// <returns></returns>
        public void LifeApplicationArchivedWithCallBacks_SendInternalEmail(int accountKey, int applicationKey, long instanceID, string consultantEmailAddress)
        {
            IApplicationRepository appRepo = RepositoryFactory.GetRepository<IApplicationRepository>();
            IX2Repository x2Repo = RepositoryFactory.GetRepository<IX2Repository>();

            ICallback callback = appRepo.GetLatestCallBackByApplicationKey(applicationKey, false);
            IInstance instance = x2Repo.GetInstanceByKey(instanceID);

            string Subject = string.Format("Callback Notification : Policy Auto Archived after 45 Days - Policy Number : {0} {1}", accountKey, instance.Subject);

            StringBuilder sbBody = new StringBuilder();
            sbBody.AppendFormat("The above archived case had pending callbacks.");
            sbBody.AppendLine();
            sbBody.AppendLine("Details of this callback below:");
            sbBody.AppendLine();
            sbBody.AppendFormat("CallBack Due Date : {0}", callback.CallbackDate.ToString(Constants.DateFormat));
            sbBody.AppendLine();
            sbBody.AppendFormat("Latest Callback Note : {0}", callback.Reason.Comment);
            sbBody.AppendLine();

            // now send the mail
            IMessageService messageService = ServiceFactory.GetService<IMessageService>();
            messageService.SendEmailInternal("sqlmail@sahomeloans.com", consultantEmailAddress, null, null, Subject, sbBody.ToString(), false);
        }

        /// <summary>
        /// Sends an internal email with the details of the case appearing on the Ready to Callback worklist
        /// </summary>
        /// <param name="accountKey"></param>
        /// <param name="applicationKey"></param>
        /// <param name="instanceID"></param>
        /// <param name="consultantEmailAddress"></param>
        /// <returns></returns>
        public void LifeApplicationReadyToCallback_SendInternalEmail(int accountKey, int applicationKey, long instanceID, string consultantEmailAddress)
        {
            IApplicationRepository appRepo = RepositoryFactory.GetRepository<IApplicationRepository>();
            IX2Repository x2Repo = RepositoryFactory.GetRepository<IX2Repository>();

            ICallback callback = appRepo.GetLatestCallBackByApplicationKey(applicationKey, false);
            IInstance instance = x2Repo.GetInstanceByKey(instanceID);

            string Subject = string.Format("Callback Notification : Ready to Callback - Policy Number : {0} {1}", accountKey, instance.Subject);

            StringBuilder sbBody = new StringBuilder();
            sbBody.AppendFormat("The callback date on the above case has been reached. ");
            sbBody.AppendLine();
            sbBody.AppendLine("This case will be on your worklist under \"Ready to Callback\"");
            sbBody.AppendLine();
            sbBody.AppendFormat("CallBack Due Date      : {0}", callback.CallbackDate.ToString(SAHL.Common.Constants.DateFormat));
            sbBody.AppendFormat("Latest Callback Reason : {0}", callback.Reason.ReasonDefinition.ReasonDescription.Description);
            sbBody.AppendFormat("Latest Callback Note   : {0}", callback.Reason.Comment);
            sbBody.AppendLine();

            // now send the mail
            IMessageService messageService = ServiceFactory.GetService<IMessageService>();
            messageService.SendEmailInternal("sqlmail@sahomeloans.com", consultantEmailAddress, null, null, Subject, sbBody.ToString(), false);
        }

        /// <summary>
        /// Send an NTU Letter to the client
        /// </summary>
        /// <param name="accountKey"></param>
        /// <param name="originationSourceProductKey"></param>
        /// <returns></returns>
        public void LifeApplicationSendNTU_Letter(int accountKey, int originationSourceProductKey)
        {
            IReportRepository reportRepo = RepositoryFactory.GetRepository<IReportRepository>();

            IReportStatement reportStatement = reportRepo.GetReportStatementByNameAndOSP("NTU Letter", originationSourceProductKey);
            if (reportStatement != null)
            {
                IOrganisationStructureRepository osRepo = RepositoryFactory.GetRepository<IOrganisationStructureRepository>();
                ILegalEntityRepository leRepo = RepositoryFactory.GetRepository<ILegalEntityRepository>();
                ICorrespondenceRepository correspRepo = RepositoryFactory.GetRepository<ICorrespondenceRepository>();
                ILookupRepository lookupRepo = RepositoryFactory.GetRepository<ILookupRepository>();
                IStageDefinitionRepository sDRepo = RepositoryFactory.GetRepository<IStageDefinitionRepository>();

                IADUser adUser = osRepo.GetAdUserForAdUserName("System");

                // get the policyholder details
                int legalEntityKey = -1, addressKey = -1;
                ILegalEntity legalEntity = null;

                GetPolicyHolderDetails(accountKey, out legalEntityKey, out addressKey);
                if (legalEntityKey != -1)
                    legalEntity = leRepo.GetLegalEntityByKey(legalEntityKey);

                // insert the correspondence record
                ICorrespondence correspondence = correspRepo.CreateEmptyCorrespondence();
                correspondence.GenericKey = accountKey;
                correspondence.GenericKeyType = lookupRepo.GenericKeyType.ObjectDictionary[Convert.ToString((int)GenericKeyTypes.Account)];
                correspondence.ReportStatement = reportStatement;
                correspondence.CorrespondenceMedium = lookupRepo.CorrespondenceMediums.ObjectDictionary[Convert.ToString((int)SAHL.Common.Globals.CorrespondenceMediums.Post)];
                correspondence.DueDate = DateTime.Now;
                correspondence.ChangeDate = DateTime.Now;
                correspondence.UserID = adUser.ADUserName;
                if (legalEntity != null)
                    correspondence.LegalEntity = legalEntity;

                // insert the correpondenceparameters records
                foreach (IReportParameter reportParameter in reportStatement.ReportParameters)
                {
                    ICorrespondenceParameters correspondenceParm = correspRepo.CreateEmptyCorrespondenceParameter();
                    correspondenceParm.Correspondence = correspondence;
                    correspondenceParm.ReportParameter = reportParameter;
                    if (reportParameter.ParameterName.ToLower() == "accountkey")
                        correspondenceParm.ReportParameterValue = accountKey.ToString();
                    else if (reportParameter.ParameterName.ToLower() == "mailingtype")
                        correspondenceParm.ReportParameterValue = Convert.ToString((int)SAHL.Common.Globals.CorrespondenceMediums.Post);
                    else if (reportParameter.ParameterName.ToLower() == "legalentitykey")
                        correspondenceParm.ReportParameterValue = legalEntityKey > 0 ? legalEntityKey.ToString() : null;
                    else if (reportParameter.ParameterName.ToLower() == "addresskey")
                        correspondenceParm.ReportParameterValue = addressKey > 0 ? addressKey.ToString() : null;

                    // add the correspondenceparameter object to the correspondence object
                    correspondence.CorrespondenceParameters.Add(new DomainMessageCollection(), correspondenceParm);
                }

                // save the correspondence record
                correspRepo.SaveCorrespondence(correspondence);

                // insert the correspondence stage transition
                sDRepo.SaveStageTransition(accountKey, Convert.ToInt32(StageDefinitionGroups.LifeAdmin), Constants.StageDefinitionConstants.DocumentProcessed, reportStatement.ReportName, adUser);
            }
        }

        /// <summary>
        /// Create life policy using the Life.CreateLifePolicy uiStatement, that executes the pCreateLifePolicy halo API
        /// </summary>
        /// <param name="accountKey">The AccountKey of the LifePolicy</param>
        public void CreateLifePolicy(int accountKey)
        {
            IStageDefinitionRepository sDRepo = RepositoryFactory.GetRepository<IStageDefinitionRepository>();
            IOrganisationStructureRepository osRepo = RepositoryFactory.GetRepository<IOrganisationStructureRepository>();

            IADUser adUser = osRepo.GetAdUserForAdUserName("System");

            // Add the required parameters
            ParameterCollection parameters = new ParameterCollection();
            parameters.Add(new SqlParameter("@AccountKey", accountKey));
            this.castleTransactionService.ExecuteHaloAPI_uiS_OnCastleTranForUpdate("Life", "CreateLifePolicy", parameters);

            // insert the CreateLifePolicy stage transition
            sDRepo.SaveStageTransition(accountKey, (int)SAHL.Common.Globals.StageDefinitionGroups.LifeOrigination, SAHL.Common.Constants.StageDefinitionConstants.CreateLifePolicy, "Life Policy Created", adUser);
        }

        public DateTime? GetDateAssuredLifeAddedToPolicy(int accountkey, int legalEntityKey)
        {
            string query = UIStatementRepository.GetStatement("Life", "GetDateAssuredLifeAddedToPolicy");

            // Create a collection
            ParameterCollection parameters = new ParameterCollection();

            //Add the required parameters
            parameters.Add(new SqlParameter("@AccountKey", accountkey));
            parameters.Add(new SqlParameter("@LegalEntityKey", legalEntityKey));

            // Execute
            DataSet dsResults = this.castleTransactionService.ExecuteQueryOnCastleTran(query, typeof(GeneralStatus_DAO), parameters);
            if (dsResults != null && dsResults.Tables.Count > 0)
            {
                DataTable dtResults = dsResults.Tables[0];
                if (dtResults.Rows.Count > 0)
                {
                    return Convert.ToDateTime(dtResults.Rows[0][0]);
                }
            }
            return null;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="accountKey"></param>
        /// <returns></returns>
        public double GetMonthlyPremium(int accountKey)
        {
            double monthlyPremium = 0;

            DateTime anniversaryDate = System.DateTime.Now;
            string query = UIStatementRepository.GetStatement("Life", "GetMonthlyPremium");

            ParameterCollection parameters = new ParameterCollection();
            parameters.Add(new SqlParameter("@LifeAccountKey", accountKey));

            object premium = this.castleTransactionService.ExecuteScalarOnCastleTran(query, typeof(GeneralStatus_DAO), parameters);
            if (premium != null)
            {
                monthlyPremium = Convert.ToDouble(premium);
            }

            return monthlyPremium;
        }

        public ILifePolicyClaim CreateEmptyLifePolicyClaim()
        {
            return base.CreateEmpty<ILifePolicyClaim, LifePolicyClaim_DAO>();
        }

        public ILifePolicyClaim GetLifePolicyClaimByKey(int Key)
        {
            return base.GetByKey<ILifePolicyClaim, LifePolicyClaim_DAO>(Key);
        }

        public void SaveLifePolicyClaim(ILifePolicyClaim lifePolicyClaim)
        {
            //if (lifePolicyClaim.ClaimType.Key == (int)SAHL.Common.Globals.ClaimTypes.DeathClaim)
            //{
            //    if (lifePolicyClaim.ClaimStatus.Key == (int)SAHL.Common.Globals.ClaimStatuses.Settled)
            //    {
            //        // Debit Order is cancelled
            //    }
            //    else if (lifePolicyClaim.Key > 0) // update
            //    {
            //        // undo Debit Order is cancelled
            //    }
            //}
            //else if ((lifePolicyClaim.ClaimType.Key == (int)SAHL.Common.Globals.ClaimTypes.DisabilityClaim) || (lifePolicyClaim.ClaimType.Key == (int)SAHL.Common.Globals.ClaimTypes.RetrenchmentClaim))
            //{
            //    if (lifePolicyClaim.ClaimStatus.Key == (int)SAHL.Common.Globals.ClaimStatuses.Settled)
            //    {
            //        // Debit Order is suspend
            //    }
            //    else if (lifePolicyClaim.Key > 0) // update
            //    {
            //        // undo Debit Order is suspend
            //    }
            //}

            base.Save<ILifePolicyClaim, LifePolicyClaim_DAO>(lifePolicyClaim);
        }

        public IClaimType GetClaimTypeByKey(int key)
        {
            return base.GetByKey<IClaimType, ClaimType_DAO>(key);
        }

        public IClaimStatus GetClaimStatusByKey(int key)
        {
            return base.GetByKey<IClaimStatus, ClaimStatus_DAO>(key);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="personalLoanAccountKey"></param>
        /// <param name="userName"></param>
        public void CreateCreditLifeForPersonalLoan(int personalLoanAccountKey, string userName)
        {
            SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(SAHLPrincipal.GetCurrent());

            IAccountRepository accountRepository = RepositoryFactory.GetRepository<IAccountRepository>();
            ILookupRepository lookupRepository = RepositoryFactory.GetRepository<ILookupRepository>();
            IApplicationRepository applicationRepository = RepositoryFactory.GetRepository<IApplicationRepository>();
            IStageDefinitionRepository stageDefinitionRepository = RepositoryFactory.GetRepository<IStageDefinitionRepository>();
            IOrganisationStructureRepository organisationStructureRepository = RepositoryFactory.GetRepository<IOrganisationStructureRepository>();

            IAccountPersonalLoan accountPersonalLoan = accountRepository.GetAccountByKey(personalLoanAccountKey) as IAccountPersonalLoan;

            if (accountPersonalLoan == null ||
                accountPersonalLoan.InstallmentSummary == null ||
                accountPersonalLoan.InstallmentSummary.CurrentBalance <= 0)
            {
                spc.DomainMessages.Add(new DomainMessage("Create Credit Life failed on Personal Loan Account.", "Create Credit Life failed on Personal Loan Account."));
                return;
            }

            var originationSourceProduct = applicationRepository.GetOriginationSourceProductBySourceAndProduct(accountPersonalLoan.OriginationSource.Key, (int)Products.SAHLCreditProtectionPlan);
            var financialServicePersonalLoan = accountPersonalLoan.FinancialServices.Single(x => x.FinancialServiceType.Key == (int)FinancialServiceTypes.PersonalLoan && x.AccountStatus.Key == (int)AccountStatuses.Open);

            if (financialServicePersonalLoan == null || originationSourceProduct == null)
            {
                spc.DomainMessages.Add(new DomainMessage("Create Credit Life failed on Personal Loan Financial Service.", "Create Credit Life failed on Personal Loan Financial Service."));
                return;
            }
            //accountPersonalLoan.ExternalLifePolicy.LegalEntity.Key
            //create SAHL life policy
            int legalEntityKey = accountPersonalLoan.Roles.FirstOrDefault(x => x.RoleType.Key == (int)RoleTypes.MainApplicant && x.GeneralStatus.Key == (int)GeneralStatusKey.Active).LegalEntity.Key;

            CreateCreditLife(personalLoanAccountKey, financialServicePersonalLoan.Key, accountPersonalLoan.SPV.Key, originationSourceProduct.Key, legalEntityKey,
                CalculateLifePremiumForUnsecuredLending(accountPersonalLoan.InstallmentSummary.CurrentBalance), userName);

            //close external life policy
            if (accountPersonalLoan.ExternalLifePolicy != null)
            {
                accountPersonalLoan.ExternalLifePolicy.LifePolicyStatus = lookupRepository.LifePolicyStatuses.Single(i => i.Key == (int)LifePolicyStatuses.Closed);
                accountPersonalLoan.ExternalLifePolicy.PolicyCeded = false;
                SaveExternalLifePolicy(accountPersonalLoan.ExternalLifePolicy);
            }

            // insert the CreateLifePolicy stage transition
            IADUser adUser = organisationStructureRepository.GetAdUserForAdUserName(userName);
            if (adUser == null)
                adUser = organisationStructureRepository.GetAdUserForAdUserName("System");

            stageDefinitionRepository.SaveStageTransition(personalLoanAccountKey, (int)StageDefinitionGroups.PersonalLoanAccount, Constants.StageDefinitionConstants.CreateCreditLife, "SAHL Credit Life Policy Created", adUser);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="parentAccountKey"></param>
        /// <param name="parentFinancialServiceKey"></param>
        /// <param name="SPVKey"></param>
        /// <param name="originationSourceProductKey"></param>
        /// <param name="legalEntityKey"></param>
        /// <param name="lifePremium"></param>
        /// <param name="userId"></param>
        private void CreateCreditLife(int parentAccountKey, int parentFinancialServiceKey, int SPVKey, int originationSourceProductKey, int legalEntityKey, double lifePremium, string userId)
        {
            // Update the required fields on the Account
            ParameterCollection parameters = new ParameterCollection();
            parameters.Add(new SqlParameter("@ParentAccountKey", parentAccountKey));
            parameters.Add(new SqlParameter("@ParentFinancialServiceKey", parentFinancialServiceKey));
            parameters.Add(new SqlParameter("@SPVKey", SPVKey));
            parameters.Add(new SqlParameter("@OriginationSourceProductKey", originationSourceProductKey));
            parameters.Add(new SqlParameter("@LegalEntityKey", legalEntityKey));
            parameters.Add(new SqlParameter("@LifePremium", lifePremium));
            parameters.Add(new SqlParameter("@UserID", userId));

            this.castleTransactionService.ExecuteHaloAPI_uiS_OnCastleTranForUpdate("Life", "CreateCreditLife", parameters);
        }

        /// <summary>
        /// Saves an ExternalLifePolicy object
        /// </summary>
        /// <param name="externalLifePolicy"></param>
        public void SaveExternalLifePolicy(IExternalLifePolicy externalLifePolicy)
        {
            base.Save<IExternalLifePolicy, ExternalLifePolicy_DAO>(externalLifePolicy);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="instanceID"></param>
        /// <returns></returns>
        public int GetLoanAccountKeyByDisabilityClaimInstanceID(long instanceID)
        {
            using (IDbConnection con = Helper.GetSQLDBConnection())
            {
                string query = UIStatementRepository.GetStatement("Life", "GetLoanAccountKeyByDisabilityClaimInstanceID");
                ParameterCollection parameters = new ParameterCollection();
                Helper.AddBigIntParameter(parameters, "@InstanceID", instanceID);
                object o = Helper.ExecuteScalar(con, query, parameters);

                if (o != null)
                    return Convert.ToInt32(o);
                else
                    return 0;
            }
        }
    }
}