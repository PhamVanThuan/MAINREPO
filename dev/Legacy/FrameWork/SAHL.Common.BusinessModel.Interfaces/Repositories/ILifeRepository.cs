using SAHL.Common.Collections.Interfaces;
using SAHL.Services.Interfaces.LifeDomain.Models;
using System;
using System.Collections.Generic;

namespace SAHL.Common.BusinessModel.Interfaces.Repositories
{
    /// <summary>
    ///
    /// </summary>
    public interface ILifeRepository
    {
        double CalculateLifePremiumForUnsecuredLending(double amount);

        /// <summary>
        /// Gets an LifePolicy object by the AccountKey of the LifePolicy.
        /// </summary>
        /// <param name="Key">The integer AccountKey Key.</param>
        /// <returns>The <see cref="ILifePolicy">LifePolicy found using the supplied AccountKey, returns null if no LifePolicy is found.</see></returns>
        ILifePolicy GetLifePolicyByAccountKey(int Key);

        /// <summary>
        /// Creates and Empty LifePolicy object
        /// </summary>
        /// <returns>ILifePolicy</returns>
        ILifePolicy CreateEmptyLifePolicy();

        /// <summary>
        /// Creates and Empty Callback object
        /// </summary>
        /// <returns>ICallback</returns>
        ICallback CreateEmptyCallback();

        /// <summary>
        /// Saves a Callback object
        /// </summary>
        /// <param name="callback">The <see cref="ICallback">Callback to save.</see></param>
        /// <returns></returns>
        void SaveCallback(ICallback callback);

        /// <summary>
        ///
        /// </summary>
        /// <param name="TextStatementTypes"></param>
        /// <returns></returns>
        IReadOnlyEventList<ITextStatement> GetTextStatementsForTypes(int[] TextStatementTypes);

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        IDictionary<string, int> GetCancellationTypes();

        /// <summary>
        ///
        /// </summary>
        /// <param name="loanNumber"></param>
        /// <returns></returns>
        string GetLoanPipelineStatus(int loanNumber);

        /// <summary>
        /// Get the LifePremiumHistory object
        /// </summary>
        /// <param name="accountKey">Life Account Key</param>
        /// <returns></returns>
        IList<ILifePremiumHistory> GetLifePremiumHistory(int accountKey);

        /// <summary>
        /// Returns whether an assured life is over exposed in terms of the group exposure policy
        /// </summary>
        /// <param name="legalEntity">ILegalEntity</param>
        /// <returns></returns>
        bool IsLifeOverExposed(ILegalEntity legalEntity);

        /// <summary>
        /// Returns whether a user is an "Admin" user
        /// </summary>
        /// <param name="currentPrincipal">Current SAHL Principal</param>
        /// <returns></returns>
        bool IsAdminUser(SAHL.Common.Security.SAHLPrincipal currentPrincipal);

        /// <summary>
        /// Returns whether life is a condition of a loan
        /// </summary>
        /// <param name="loanAccountKey"></param>
        /// <returns></returns>
        bool IsLifeConditionOfLoan(int loanAccountKey);

        /// <summary>
        /// Returns whether a loan is a Regent loan or not
        /// </summary>
        /// <param name="accountKey">accountKey</param>
        /// <returns></returns>
        bool IsRegentLoan(int accountKey);

        /// <summary>
        /// Creates and Empty LifeInsurableInterest object
        /// </summary>
        /// <returns>ILifeInsurableInterest</returns>
        ILifeInsurableInterest CreateEmptyLifeInsurableInterest();

        /// <summary>
        /// Recalculates and Updates the Premium on the LifePolicy/Offer
        /// </summary>
        /// <param name="accountLifePolicy">The <see cref="IAccountLifePolicy"/>.</param>
        /// <param name="recalcDiscountFactor">True or False</param>
        void RecalculateSALifePremium(IAccountLifePolicy accountLifePolicy, bool recalcDiscountFactor);

        /// <summary>
        /// Recalculates and returns the Premium on the LifePolicy/Offer for quotation - no updates
        /// </summary>
        /// <param name="accountKey">int</param>
        /// <param name="lifePolicyTypeKey">int</param>
        /// <param name="recalcDiscountFactor">True or False</param>
        /// <param name="ageList">comma delimited string of ages</param>
        /// <param name="currentSumAssured">Output : Current Sum Assured</param>
        /// <param name="monthlyInstalment">Output : Monthly Instalment</param>
        /// <param name="yearlyPremium">Output : Yearly Premium</param>
        /// <param name="deathBenefitPremium">Output : Death Benefit Premium</param>
        /// <param name="ipBenefitPremium">Output : IP Benefit Premium</param>
        void RecalculateSALifePremiumQuote(int accountKey, int lifePolicyTypeKey, bool recalcDiscountFactor, string ageList, out double currentSumAssured, out double monthlyInstalment, out double yearlyPremium, out double deathBenefitPremium, out double ipBenefitPremium);

        /// <summary>
        /// Returns the Policy Holder LegalEntityKey and AddressKey for the specified AccountKey
        /// </summary>
        /// <param name="AccountKey">The integer AccountKey Key.</param>
        /// <param name="LegalEntityKey">The output integer LegalEntityKey.</param>
        /// <param name="AddressKey">The output integer AddressKey.</param>
        /// <returns>The LegalEntityKey and AddressKey of the policyholder</returns>
        void GetPolicyHolderDetails(int AccountKey, out int LegalEntityKey, out int AddressKey);

        /// <summary>
        /// Handles life policy cancellation
        /// </summary>
        /// <param name="financialServiceKey"></param>
        /// <param name="policyStatusKey"></param>
        void CancelLifePolicy(int financialServiceKey, int policyStatusKey);

        /// <summary>
        /// Returns the IInsurer for the specified Insurer Description
        /// </summary>
        /// <param name="InsurerDescription"></param>
        /// <returns></returns>
        IInsurer GetInsurerByDescription(string InsurerDescription);

        /// <summary>
        /// Returns a list of Life Consultants
        /// </summary>
        /// <returns>IList&lt;IADUser&gt;></returns>
        IList<IADUser> GetLifeConsultants();

        /// <summary>
        /// Returns a list of Life Consultants
        /// </summary>
        /// <param name="includeInactiveConsultants"></param>
        /// <returns>IList&lt;IADUser&gt;></returns>
        IList<IADUser> GetLifeConsultants(bool includeInactiveConsultants);

        /// <summary>
        /// Gets a list of the Life Consultants
        /// </summary>
        /// <param name="ADUserName"></param>
        /// <returns>IList&lt;IADUser&gt;></returns>
        IList<IADUser> GetLifeConsultants(string ADUserName);

        /// <summary>
        /// Returns the age of the legalentity to be used in the premium calculation.
        /// I am using a sql query here so that i can simulate the exact logic that
        /// l_CalculateAnnualPremium uses when working out the ages
        /// </summary>
        /// <param name="legalEntityNaturalPerson"></param>
        /// <param name="dateAddedToPolicy"></param>
        /// <returns></returns>
        int GetAssuredLifeAgeForPremiumCalc(ILegalEntityNaturalPerson legalEntityNaturalPerson, DateTime? dateAddedToPolicy);

        /// <summary>
        /// Creates and Empty LifeOfferAssignment object
        /// </summary>
        /// <returns>ILifeOfferAssignment</returns>
        ILifeOfferAssignment CreateEmptyLifeOfferAssignment();

        /// <summary>
        /// Saves a LifeOfferAssignment object
        /// </summary>
        /// <param name="lifeOfferAssignment">The <see cref="ILifeOfferAssignment">LifeOfferAssignment to save.</see></param>
        /// <returns></returns>
        void SaveLifeOfferAssignment(ILifeOfferAssignment lifeOfferAssignment);

        /// <summary>
        /// Returns the latest lifeofferassignment record for the specified user and loan offer type
        /// </summary>
        /// <param name="ADUserName"></param>
        /// <param name="OfferTypeKey"></param>
        /// <returns></returns>
        ILifeOfferAssignment GetLatestLifeOfferAssignment(string ADUserName, int OfferTypeKey);

        /// <summary>
        /// calculate the next yearly anniversary of the policy based on the policy number
        /// </summary>
        /// <param name="accountkey"></param>
        /// <param name="currentDate"></param>
        /// <returns></returns>
        DateTime GetNextLifeAnniversaryDate(int accountkey, DateTime currentDate);

        /// <summary>
        ///
        /// </summary>
        /// <param name="accountLifePolicy"></param>
        /// <param name="legalEntityNaturalPerson"></param>
        /// <returns></returns>
        bool CheckLegalEntityQualifies(IAccountLifePolicy accountLifePolicy, ILegalEntityNaturalPerson legalEntityNaturalPerson);

        /// <summary>
        /// Updated the account status to closed and writes an application archived stage transition
        /// </summary>
        /// <param name="accountKey"></param>
        /// <param name="applicationKey"></param>
        /// <param name="stageTransitionComments" default="null"></param>
        /// <returns></returns>
        void CloseLifeApplication(int accountKey, int applicationKey, string stageTransitionComments);

        /// <summary>
        /// Sends an internal email with the details of the pending call backs
        /// </summary>
        /// <param name="accountKey"></param>
        /// <param name="applicationKey"></param>
        /// <param name="instanceID"></param>
        /// <param name="consultantEmailAddress"></param>
        /// <returns></returns>
        void LifeApplicationArchivedWithCallBacks_SendInternalEmail(int accountKey, int applicationKey, long instanceID, string consultantEmailAddress);

        /// <summary>
        /// Sends an internal email with the details of the case appearing on the Ready to Callback worklist
        /// </summary>
        /// <param name="accountKey"></param>
        /// <param name="applicationKey"></param>
        /// <param name="instanceID"></param>
        /// <param name="consultantEmailAddress"></param>
        /// <returns></returns>
        void LifeApplicationReadyToCallback_SendInternalEmail(int accountKey, int applicationKey, long instanceID, string consultantEmailAddress);

        /// <summary>
        /// Send an NTU Letter to the client
        /// </summary>
        /// <param name="accountKey"></param>
        /// <param name="originationSourceProductKey"></param>
        /// <returns></returns>
        void LifeApplicationSendNTU_Letter(int accountKey, int originationSourceProductKey);

        /// <summary>
        /// Create life policy using the Life.CreateLifePolicy uiStatement, that executes the pCreateLifePolicy halo API
        /// </summary>
        /// <param name="accountKey">The AccountKey of the LifePolicy</param>
        void CreateLifePolicy(int accountKey);

        /// <summary>
        /// Returns the date that an assured life was added to a life policy
        /// </summary>
        /// <param name="accountkey"></param>
        /// <param name="legalEntityKey"></param>
        /// <returns></returns>
        DateTime? GetDateAssuredLifeAddedToPolicy(int accountkey, int legalEntityKey);

        /// <summary>
        ///
        /// </summary>
        /// <param name="accountKey"></param>
        /// <returns></returns>
        double GetMonthlyPremium(int accountKey);

        ILifePolicyClaim CreateEmptyLifePolicyClaim();

        ILifePolicyClaim GetLifePolicyClaimByKey(int Key);

        void SaveLifePolicyClaim(ILifePolicyClaim lifePolicyClaim);

        IClaimType GetClaimTypeByKey(int key);

        IClaimStatus GetClaimStatusByKey(int key);

        /// <summary>
        ///
        /// </summary>
        /// <param name="personalLoanAccountKey"></param>
        /// <param name="userName"></param>
        void CreateCreditLifeForPersonalLoan(int personalLoanAccountKey, string userName);

        ///// <summary>
        /////
        ///// </summary>
        ///// <param name="parentAccountKey"></param>
        ///// <param name="parentFinancialServiceKey"></param>
        ///// <param name="SPVKey"></param>
        ///// <param name="originationSourceProductKey"></param>
        ///// <param name="legalEntityKey"></param>
        ///// <param name="lifePremium"></param>
        ///// <param name="userId"></param>
        //void CreateCreditLife(int parentAccountKey, int parentFinancialServiceKey, int SPVKey, int originationSourceProductKey, int legalEntityKey, double lifePremium, string userId);

        /// <summary>
        /// Saves an ExternalLifePolicy object
        /// </summary>
        /// <param name="externalLifePolicy"></param>
        void SaveExternalLifePolicy(IExternalLifePolicy externalLifePolicy);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="instanceID"></param>
        /// <returns></returns>
        int GetLoanAccountKeyByDisabilityClaimInstanceID(long instanceID);
    }
}