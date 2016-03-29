using SAHL.Common.BusinessModel.Interfaces.DataTransferObjects;
using SAHL.Common.BusinessModel.Interfaces.SearchCriteria;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Globals;
using SAHL.Common.X2.BusinessModel.Interfaces;
using SAHL.Services.Capitec.Models.Shared;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;

namespace SAHL.Common.BusinessModel.Interfaces.Repositories
{
    /// <summary>
    /// The Application repository.
    /// </summary>
    public interface IApplicationRepository
    {
        void RemoveDetailFromApplication(IApplication application, List<int> detailTypes);

        void CreateAccountFromApplication(int offerKey, string adUserName);

        /// <summary>
        ///
        /// </summary>
        /// <param name="LoanAmount"></param>
        /// <param name="BondRequired"></param>
        /// <param name="applicationType"></param>
        /// <param name="cashOut"></param>
        /// <param name="overrideCancelFeeAmount"></param>
        /// <param name="capitaliseFees"></param>
        /// <param name="NCACompliant"></param>
        /// <param name="IsBondExceptionAction"></param>
        /// <param name="IsDiscountedInitiationFee"></param>
        /// <param name="initiationFeeDiscount"></param>
        /// <param name="InitiationFee"></param>
        /// <param name="RegistrationFee"></param>
        /// <param name="CancelFee"></param>
        /// <param name="InterimInterest"></param>
        /// <param name="BondToRegister"></param>
        /// <param name="IsQuickPayLoan"></param>
        /// <param name="HouseholdIncome"></param>
        /// <param name="EmploymentTypeKey"></param>
        /// <param name="PropertyValue"></param>
        /// <param name="ApplicationParentAccountKey"></param>
        /// <param name="IsStaffLoan"></param>
        /// <param name="ApplicationStartDate"></param>
        /// <param name="capitaliseInitiationFee"></param>
        /// <param name="isGEPF"></param>
        void CalculateOriginationFees(double LoanAmount, double BondRequired, OfferTypes applicationType, double cashOut, double overrideCancelFeeAmount, bool capitaliseFees, bool NCACompliant, bool IsBondExceptionAction, bool IsDiscountedInitiationFee, out double? initiationFeeDiscount, out double InitiationFee, out double RegistrationFee, out double CancelFee, out double InterimInterest, out double BondToRegister, bool IsQuickPayLoan, double HouseholdIncome, int EmploymentTypeKey, double PropertyValue, int ApplicationParentAccountKey, bool IsStaffLoan, DateTime ApplicationStartDate, bool capitaliseInitiationFee, bool isGEPF);

        /// <summary>
        /// Gets an Application by the Application key.
        /// </summary>
        /// <param name="ApplicationKey">The integer Application key.</param>
        /// <returns>The <see cref="IApplication">Application found using the supplied Application key, returns null if no Application is found.</see></returns>
        IApplication GetApplicationByKey(int ApplicationKey);

        /// <summary>
        ///
        /// </summary>
        /// <param name="ApplicationKey"></param>
        /// <param name="ApplicationRoleTypeKey"></param>
        /// <returns></returns>
        IApplicationRole GetActiveApplicationRoleForTypeAndKey(int ApplicationKey, int ApplicationRoleTypeKey);

        /// <summary>
        /// Gets the latest accepted application given an account
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        IApplication GetLastestAcceptedApplication(IAccount account);

        /// <summary>
        /// Gets an Application by the ReservedAccount key.
        /// </summary>
        /// <param name="reservedAccountKey"></param>
        /// <returns></returns>
        IApplication GetApplicationByReservedAccountKey(int reservedAccountKey);

        /// <summary>
        /// Get the last disbursed application.
        /// Readvance, Life and unknown applications are ignored.
        /// </summary>
        /// <param name="AccountKey"></param>
        /// <returns></returns>
        IApplication GetLastDisbursedApplicationByAccountKey(int AccountKey);

        /// <summary>
        /// Gets a list of application keys starting with <c>prefix</c>.
        /// </summary>
        /// <param name="prefix">The starting characters.</param>
        /// <param name="maxCount">The maximum number of rows to return.</param>
        /// <returns>A list of application keys.  If prefix is null or empty, and empty list is returned.</returns>
        IList<int> GetApplicationKeys(string prefix, int maxCount);

        /// <summary>
        /// Gets an ApplicationLife by the Application key.
        /// </summary>
        /// <param name="ApplicationKey">The integer Application key.</param>
        /// <returns>The <see cref="IApplicationLife">ApplicationLife found using the supplied Application key, returns null if no ApplicationLife is found.</see></returns>
        IApplicationLife GetApplicationLifeByKey(int ApplicationKey);

        /// <summary>
        ///
        /// </summary>
        /// <param name="AccountKey"></param>
        /// <returns></returns>
        IEventList<IApplication> GetApplicationByAccountKey(int AccountKey);

        /// <summary>
        ///
        /// </summary>
        /// <param name="Application"></param>
        void SaveApplication(IApplication Application);

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        IApplicationInformation GetEmptyApplicationInformation();

        /// <summary>
        ///
        /// </summary>
        /// <param name="ApplicationKey"></param>
        /// <returns></returns>
        IEventList<IApplicationDebitOrder> GetApplicationDebitOrdersByApplicationKey(int ApplicationKey);

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        IApplicationInformationFinancialAdjustment GetEmptyApplicationInformationFinancialAdjustment();

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        IApplicationLife GetEmptyApplicationLife();

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        IApplicationRole GetEmptyApplicationRole();

        /// <summary>
        /// Gets the latest callback for an ApplicationKey.
        /// </summary>
        /// <param name="ApplicationKey">The integer Application key.</param>
        /// <param name="OpenCallbacksOnly">boolean to specify whether you only want open callbacks returned.</param>
        /// <returns>The <see cref="ICallback"></see>.</returns>
        ICallback GetLatestCallBackByApplicationKey(int ApplicationKey, bool OpenCallbacksOnly);

        /// <summary>
        /// Gets all the open callbacks for an ApplicationKey.
        /// </summary>
        /// <param name="ApplicationKey">The integer Application key.</param>
        /// <param name="OpenCallbacksOnly">boolean to specify whether you only want open callbacks returned.</param>
        /// <returns>The <see cref="ICallback"></see>.</returns>
        IEventList<ICallback> GetCallBacksByApplicationKey(int ApplicationKey, bool OpenCallbacksOnly);

        /// <summary>
        /// Saves a Callback record
        /// </summary>
        /// <param name="callback"></param>
        void SaveCallback(ICallback callback);

        /// <summary>
        /// Completes a Callback
        /// </summary>
        /// <param name="ApplicationKey"></param>
        /// <param name="CompletedDate"></param>
        bool CompleteCallback(int ApplicationKey, DateTime CompletedDate);

        /// <summary>
        ///
        /// </summary>
        /// <param name="application"></param>
        /// <param name="term"></param>
        /// <param name="hasPassedTheDecisionTree"></param>
        /// <param name="adjustmentAmount"></param>
        void ConvertAcceptedApplicationToExtendedTerm(IApplication application, int term, bool hasPassedTheDecisionTree, double adjustmentAmount);

        /// <summary>
        /// For applications post credit that need to remain Accepted
        /// </summary>
        /// <param name="application"></param>
        void RevertToPreviousTermAsAcceptedApplication(IApplication application);

        /// <summary>
        /// For resub applications that need to be the revision
        /// </summary>
        /// <param name="application"></param>
        void RevertToPreviousTermAsRevisedApplication(IApplication application);

        /// <summary>
        /// Get the default reset configuration (this is currently hardcoded to return the 18th reset).
        /// </summary>
        /// <returns></returns>
        IResetConfiguration GetApplicationDefaultResetConfiguration();

        /// <summary>
        /// Get the reset configuration based on the SPV and Product configuration.
        /// </summary>
        /// <returns></returns>
        IResetConfiguration GetApplicationResetConfiguration(int SPVKey, int ProductKey);

        /// <summary>
        /// Creates a Life Application with its Account for the specified MortgageLoan
        /// </summary>
        /// <param name="mortgageLoanAccountKey">The accountkey of the loan for which the application will be created.</param>
        /// <param name="mortgageLoanApplicationKey">The OfferKey of the loan for which the application will be created.</param>
        /// <param name="adUserName">The userid of the person to assign the application to.</param>
        /// <returns>The <see cref="IApplicationLife"></see>.</returns>
        IApplicationLife CreateLifeApplication(int mortgageLoanAccountKey, int mortgageLoanApplicationKey, string adUserName);

        /// <summary>
        /// Creates a an Application that models a New Purchase.
        /// </summary>
        /// <param name="OriginationSource">The Bond originator</param>
        /// <param name="ProductType">Type of product for New purchase options are VariFix, SuperLo, New VL</param>
        /// <param name="App">If an unknown application has already been created pass it in. Else pass in null</param>
        /// <returns></returns>
        IApplicationMortgageLoanNewPurchase CreateNewPurchaseApplication(IOriginationSource OriginationSource, ProductsNewPurchaseAtCreation ProductType, IApplicationUnknown App);

        /// <summary>
        ///
        /// </summary>
        /// <param name="OriginationSource"></param>
        /// <param name="ProductType"></param>
        /// <param name="App">If an unknown application has already been created pass it in. Else pass in null</param>
        /// <returns></returns>
        IApplicationMortgageLoanSwitch CreateSwitchLoanApplication(IOriginationSource OriginationSource, ProductsSwitchLoanAtCreation ProductType, IApplicationUnknown App);

        /// <summary>
        ///
        /// </summary>
        /// <param name="OriginationSource"></param>
        /// <param name="ProductType"></param>
        /// <param name="App">If an unknown application has already been created pass it in. Else pass in null</param>
        /// <returns></returns>
        IApplicationMortgageLoanRefinance CreateRefinanceApplication(IOriginationSource OriginationSource, ProductsRefinanceAtCreation ProductType, IApplicationUnknown App);

        /// <summary>
        ///
        /// </summary>
        /// <param name="MortgageLoan"></param>
        /// <param name="save"></param>
        /// <returns></returns>
        IApplicationFurtherLoan CreateFurtherLoanApplication(IMortgageLoanAccount MortgageLoan, bool save);

        /// <summary>
        ///
        /// </summary>
        /// <param name="ApplicationKey"></param>
        /// <returns></returns>
        IEventList<IApplicationInformation> GetApplicationRevisionHistory(int ApplicationKey);

        /// <summary>
        /// Creates an empty applicaiton.
        /// </summary>
        /// <returns></returns>
        IApplicationUnknown GetEmptyUnknownApplicationType(int originationSourceKey);

        /// <summary>
        ///
        /// </summary>
        /// <param name="mla"></param>
        /// <param name="save"></param>
        /// <returns></returns>
        IApplicationReAdvance CreateReAdvanceApplication(IMortgageLoanAccount mla, bool save);

        /// <summary>
        ///
        /// </summary>
        /// <param name="mla"></param>
        /// <param name="save"></param>
        /// <returns></returns>
        IApplicationFurtherAdvance CreateFurtherAdvanceApplication(IMortgageLoanAccount mla, bool save);

        /// <summary>
        ///
        /// </summary>
        /// <param name="calculationDone"></param>
        /// <param name="ltv"></param>
        /// <param name="pti"></param>
        /// <param name="income"></param>
        /// <param name="loanAmount"></param>
        /// <param name="valuationAmount"></param>
        /// <param name="employmentTypeKey"></param>
        /// <param name="isFurtherLending"></param>
        /// <param name="term"></param>
        /// <param name="readvanceOnly"></param>
        void CreditDisqualifications(bool calculationDone, double ltv, double pti, double income, double loanAmount, double valuationAmount, int employmentTypeKey, bool isFurtherLending, int term, bool readvanceOnly);

        /// <summary>
        /// Used to perform a search against applications.
        /// </summary>
        /// <param name="searchCriteria">The search criteria used to perform the search.</param>
        /// <param name="maxRowCount">The maximum number of records to return.</param>
        /// <param name="mustExistInWorkflow">specifies whether to only return applications that exists in workflow.</param>
        /// <returns></returns>
        IEventList<IApplication> SearchApplications(IApplicationSearchCriteria searchCriteria, int maxRowCount, bool mustExistInWorkflow);

        IApplicationInformationInterestOnly GetApplicationInformationInterestOnly(int ApplicationInformationKey);

        IApplicationInformationVariableLoan GetApplicationInformationVariableLoan(int ApplicationInformationKey);

        IApplicationInformationVarifixLoan GetApplicationInformationVarifixLoan(int ApplicationInformationKey);

        IApplicationInformationSuperLoLoan GetApplicationInformationSuperLoLoan(int ApplicationInformationKey);

        IApplicationAttribute GetEmptyApplicationAttribute();

        IApplicationRoleAttribute GetEmptyApplicationRoleAttribute();

        /// <summary>
        /// Gets a new account sequence object.
        /// </summary>
        /// <param name="reserve">If true, the account sequence will be saved and therefore reserved.</param>
        /// <returns></returns>
        IAccountSequence GetEmptyAccountSequence(bool reserve);

        IList<IApplicationDeclarationQuestionAnswerConfiguration> GetApplicationDeclarationsByLETypeAndApplicationRoleAndOSP(int LegalEntityTypeKey, int GenericKey, int GenericKeyTypeKey, int OSPKey);

        int GetApplicationDeclarationAnswerToQuestion(int legalEntityKey, int applicationKey, int appDeclarationQuestionKey);

        IApplicationDeclaration GetEmptyApplicationDeclaration();

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        IApplicationExpense GetEmptyApplicationExpense();

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        IApplicationDebitOrder GetEmptyApplicationDebitOrder();

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        IApplicationInternetReferrer GetEmptyApplicationInternetReferrer();

        IApplicationDebtSettlement GetEmptyApplicationDebtSettlement();

        void SaveApplicationExpense(IApplicationExpense appExpense);

        /// <summary>
        /// Save the application internet referrer record
        /// </summary>
        /// <param name="InternetReferrer"></param>
        void SaveApplicationInternetReferrer(IApplicationInternetReferrer InternetReferrer);

        void SaveApplicationDebtSettlement(IApplicationDebtSettlement appDebtSettlement);

        void SaveApplicationDeclaration(IApplicationDeclaration appDeclaration);

        IList<IApplicationDeclaration> GetApplicationDeclarationsByapplicationRoleKey(int appRoleKey);

        IApplication GetApplicationFromInstance(IInstance instance);

        IDictionary<string, string> GetApplicantRoleTypesForApplication(IApplication application);

        IApplicationExpense GetApplicationExpenseByBankAccountNameAndBankAccountNumber(string accountName, string accountNumber);

        IApplicationRole GetApplicationRoleByKey(int applicationRoleKey);

        /// <summary>
        /// Gets an <see cref="IApplicationRoleType"/> according to the key.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        IApplicationRoleType GetApplicationRoleTypeByKey(int key);

        /// <summary>
        /// Gets an <see cref="IApplicationRoleType"/> according to the key.
        /// </summary>
        /// <param name="roleType"></param>
        /// <returns></returns>
        IApplicationRoleType GetApplicationRoleTypeByKey(OfferRoleTypes roleType);

        IApplicationRole GetApplicationRoleForTypeAndKey(int ApplicationKey, int ApplicationRoleTypeKey);

        IEventList<IApplicationRole> GetApplicationRolesForKey(int ApplicationKey);

        //IEventList<IApplicationRole> GetApplicationRolesForKeyAndRoleTypes(int ApplicationKey, int[] RoleTypeKeys);

        /// <summary>
        /// Gets an <see cref="IApplicationRoleAttributeType"/> according to the key.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        IApplicationRoleAttributeType GetApplicationRoleAttributeTypeByKey(int key);

        #region moved to application repo

        IOriginationSourceProduct GetOriginationSourceProductByKey(int OSPKey);

        IOriginationSourceProduct GetOriginationSourceProductBySourceAndProduct(int OriginationSourceKey, int ProductKey);

        IOriginationSource GetOriginationSource(OriginationSources source);

        ReadOnlyEventList<IProduct> GetOriginationProducts();

        /// <summary>
        /// Return a list of Origination Products, including the ProductKey passed in
        /// An application can be originated for a product, and in the process of origination
        /// the product could be discontinued, but existing applications of this type would need to
        /// continue through the origination process
        /// </summary>
        /// <returns></returns>
        /// <param name="ProductKey"></param>
        ReadOnlyEventList<IProduct> GetOriginationProducts(int ProductKey);

        ReadOnlyEventList<IMortgageLoanPurpose> GetMortgageLoanPurposes(int[] MortgageLoanPurposeKeys);

        #endregion moved to application repo

        string GetFurtherLendingX2Message(int applicationKey);

        string GetFurtherLendingX2NTU(int accountKey);

        IEventList<IApplicationRole> GetApplicationRoleTypesForKeys(List<int> Keys, int ApplicationKey);

        //void SetApplicationTypeToUnKnown(IApplication app);

        IList<IApplicationAttributeType> GetApplicationAttributeTypeByIsGeneric(bool isGeneric);

        /// <summary>
        /// Gets all open applications that a property is loaded against.
        /// </summary>
        /// <param name="PropertyKey"></param>
        /// <returns></returns>
        IEventList<IApplication> GetOpenApplicationsForPropertyKey(int PropertyKey);

        IOriginationSourceProduct GetOriginationSourceProduct(IApplication Application);

        /// <summary>
        /// Gets audit information pertaining to an application.
        /// </summary>
        /// <param name="applicationKey"></param>
        /// <returns></returns>
        IEventList<IAudit> GetApplicationAuditData(int applicationKey);

        /// <summary>
        ///
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="criteria"></param>
        /// <returns></returns>
        IApplication GetApplicationByInstanceAndAddCriteria(IInstance instance, Hashtable criteria);

        /// <summary>
        /// Return the last credit decision and the user who completed the action
        /// </summary>
        /// <param name="appKey"></param>
        /// <param name="CurrentCreditDecision"></param>
        /// <param name="adUserKey"></param>
        /// <param name="decisionDate"></param>
        void GetCurrentCreditDecision(int appKey, out string CurrentCreditDecision, out int adUserKey, out DateTime decisionDate);

        /// <summary>
        ///
        /// </summary>
        /// <param name="ApplicationSourceKey"></param>
        /// <returns></returns>
        IApplicationSource GetApplicationSourceByKey(int ApplicationSourceKey);

        /// <summary>
        /// If there is an Open Further Lending (Readvance, Further Advance or Further Loan) offer or
        /// if there is an NTU or Decline Further Lending offer that does not have an NTU Offer or Decline Offer Stage Transition Composite
        /// then this account has a Further Lending offer in progress
        /// </summary>
        /// <param name="accountKey"></param>
        /// <returns>bool</returns>
        bool HasFurtherLendingInProgress(int accountKey);

        /// <summary>
        /// Get a RateAdjustmentGroup by its primary key
        /// </summary>
        /// <param name="rateAdjustmentGroupKey"></param>
        /// <returns></returns>
        IRateAdjustmentGroup GetRateAdjustmentGroupByKey(int rateAdjustmentGroupKey);

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        IApplicationInformationAppliedRateAdjustment GetEmptyApplicationInformationAppliedRateAdjustment();

        /// <summary>
        ///
        /// </summary>
        /// <param name="ApplicationInformationKey"></param>
        /// <returns></returns>
        IApplicationInformationEdge GetApplicationInformationEdge(int ApplicationInformationKey);

        /// <summary>
        /// Gets the highest and second highest income contributors on an offer
        /// </summary>
        /// <param name="offerKey"></param>
        /// <param name="primaryApplicantLEKey">LegalEntityKey of the highest income earner or -1 if none</param>
        /// <param name="secondaryApplicantLEKey">LegalEntityKey of the 2nd highest income earner or -1 if none</param>
        /// <param name="numApplicants">The number of main applicants (or suretors if main applicant is a company)</param>
        void GetPrimaryAndSecondaryApplicants(int offerKey, out int primaryApplicantLEKey, out int secondaryApplicantLEKey, out int numApplicants);

        /// <summary>
        ///
        /// </summary>
        /// <param name="application"></param>
        /// <returns></returns>
        bool RapidGoToCreditCheckLTV(IApplication application);

        /// <summary>
        ///
        /// </summary>
        /// <param name="application"></param>
        /// <returns></returns>
        DataTable GetOfferRolesNotInAccount(IApplication application);

        //void AddObjectToNHibernateSession(object obj);

        /// <summary>
        /// Opt an Account out of Super Lo
        /// This method MUST be used inside of a transaction
        /// </summary>
        /// <param name="applicationKey"></param>
        /// <param name="userId"></param>
        /// <param name="cancellationReasonKey"></param>
        bool OptOutOfSuperLo(int applicationKey, string userId, int cancellationReasonKey);

        /// <summary>
        /// Return Non Disbursed Loan to Prospect
        /// </summary>
        /// <param name="offerKey"></param>
        void ReturnNonDisbursedLoanToProspect(int offerKey);

        /// <summary>
        /// #19046: Create an empty application
        /// </summary>
        IApplicationUnsecuredLending GetEmptyApplicationUnsecuredLending();

        /// <summary>
        ///#19046: Create Unsecured Lending lead.
        /// </summary>
        IApplicationUnsecuredLending CreateUnsecuredLendingLead();

        /// <summary>
        ///
        /// </summary>
        /// <param name="ApplicationToSave"></param>
        void SaveApplicationUnsecuredLending(IApplicationUnsecuredLending ApplicationToSave);

        void PromoteLeadToMain(IApplication app);

        void DemoteMainToLead(IApplication app);

        /// <summary>
        /// Creates and saves an active application role
        /// No business rules will be run on save
        /// </summary>
        /// <param name="applicationKey"></param>
        /// <param name="offerRoleTypeKey"></param>
        /// <param name="legalEntityKey"></param>
        [Obsolete("If you want to save without rules being executed use rule exclusions instead.")]
        void CreateAndSaveApplicationRole_WithoutRules(int applicationKey, int offerRoleTypeKey, int legalEntityKey);

        /// <summary>
        ///
        /// </summary>
        /// <param name="applicationKey"></param>
        /// <param name="offerStatusKey"></param>
        /// <param name="offerInformationTypeKey"></param>
        void UpdateOfferStatusWithNoValidation(int applicationKey, int offerStatusKey, int offerInformationTypeKey);

        List<ApplicationAttributeToApply> DetermineApplicationAttributeTypes(IApplication application);

        void UpdateApplicationAttributeTypes(List<ApplicationAttributeToApply> applicationAttributeTypeKeys, IApplication application);

        List<ApplicationAttributeToApply> DetermineApplicationAttributeTypes(int applicationKey, double LTV, int employmentTypekey, double houseHoldIncome, bool isStaffLoan, bool isGEPF);

        /// <summary>
        /// This method creates an internet lead.
        /// Called from the SAHL.Web.Service, used by www.sahomeloans.com website.
        /// Creates an Offer\Application and an X2 external activity, either EXTCreateNetLead or EXTCreateNetApplication.
        /// Returns an Offer\Application key or -1
        /// </summary>
        int CreateWebLead(ILeadInputInformation leadInput);

        /// <summary>
        /// An external activity will be created with an empty offer
        /// This method will generate the application data structures with
        /// financial info and roles
        /// </summary>
        /// <param name="leadInput"></param>
        /// <param name="offerKey"></param>
        /// <returns></returns>
        bool GenerateApplicationFromWeb(int offerKey, ILeadInputInformation leadInput);

        /// <summary>
        /// An external activity will be created with an empty offer
        /// This method will generate the unknown application data structures with
        /// a memo and roles
        /// </summary>
        /// <param name="leadInput"></param>
        /// <param name="offerKey"></param>
        /// <returns></returns>
        bool GenerateLeadFromWeb(int offerKey, ILeadInputInformation leadInput);

        string CreateNetLeadXML(ILeadInputInformation leadInput);

        ILeadInputInformation DeserializeNetLeadXML(string leadInput);

        bool IsAlphaHousingLoan(double ltv, int employmentTypeKey, double householdIncome);

        IApplicationRoleDomicilium CreateEmptyApplicationRoleDomicilium();

        IEventList<IApplicationRoleDomicilium> GetApplicationsThatUseLegalEntityDomicilium(ILegalEntityAddress activeLegalEntityAddressDomicilium);

        IEventList<IApplicationRole> GetActiveApplicationRolesByLegalEntityKey(int LegalEntityKey);

        IExternalRoleDeclaration GetEmptyExternalRoleDeclaration();

        void SaveExternalRoleDeclaration(IExternalRoleDeclaration externalRoleDeclaration);

        ///// <summary>
        ///// Check the application is ready to submit to credit
        ///// 1. Validate the application DAO and rules
        ///// 2. Validate all the client role legal entity DAO's and rules
        ///// 3. Check the process related rule sets
        ///// 4. Update the document Checklist
        ///// </summary>
        ///// <param name="ApplicationKey"></param>
        ///// <returns></returns>
        //bool IsApplicationInOrderCommon(int ApplicationKey);

        /// <summary>
        /// Check the application is ready to submit to credit
        ///
        /// Then run the common application in order rules: bool IsApplicationInOrderCommon(int ApplicationKey)
        /// 1. Validate the application DAO and rules
        /// 2. Validate all the client role legal entity DAO's and rules
        /// 3. Check the process related rule sets
        /// 4. Update the document Checklist
        /// </summary>
        /// <param name="ApplicationKey"></param>
        /// <returns></returns>
        bool IsApplicationInOrder(int ApplicationKey);

        /// <summary>
        /// Check the application is ready to submit to credit
        ///
        /// Then run the common application in order rules: bool IsApplicationInOrderCommon(int ApplicationKey)
        /// 1. Validate the application DAO and rules
        /// 2. Validate all the client role legal entity DAO's and rules
        /// 3. Check the process related rule sets
        /// 4. Update the document Checklist
        /// </summary>
        /// <param name="ApplicationKey"></param>
        /// <returns></returns>
        bool ResubmitToCredit(int ApplicationKey);

        IExternalRoleDomicilium CreateEmptyExternalRoleDomicilium();

        void ActivatePendingDomiciliumAddress(int ApplicationKey);

        void CreatePersonalLoanWorkflowCase(int applicationKey);

        string GeneratePasswordFromAccountNumber(int accountNumber);

        #region Capitec Create Applications

        IApplication CapitecCreateApplicationWithKey(int reservedApplicationKey);

        void CreateCapitecApplication(NewPurchaseApplication newPurchaseApplication);

        void CreateCapitecApplication(SwitchLoanApplication switchApplication);

        #endregion Capitec Create Applications

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        IApplicationCapitecDetail CreateEmptyApplicationCapitecDetail();

        /// <summary>
        ///
        /// </summary>
        /// <param name="offerKey"></param>
        /// <returns></returns>
        IApplicationCapitecDetail GetApplicationCapitecDetail(int offerKey);

        /// <summary>
        ///
        /// </summary>
        /// <param name="appCapitec"></param>
        void SaveApplicationCapitecDetail(IApplicationCapitecDetail appCapitec);

        /// <summary>
        ///
        /// </summary>
        /// <param name="offerKey"></param>
        void DeleteApplicationCapitecDetail(int offerKey);

        /// <summary>
        ///
        /// </summary>
        /// <param name="offerKey"></param>
        /// <param name="comment"></param>
        void SaveComcorpLiveRepyMemo(int offerKey, string comment);

        /// <summary>
        ///
        /// </summary>
        /// <param name="application"></param>
        /// <returns></returns>
        bool DoesApplicationRequire30YearLoanTermConversion(IApplication application);

        /// <summary>
        ///
        /// </summary>
        /// <param name="applicationKey"></param>
        /// <returns></returns>
        IApplicationInformation GetLatestAcceptedOfferInformationForApplication(int applicationKey);

        /// <summary>
        ///
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        bool Display20YearFiguresOn30YearLoan(IAccount account);

        /// <summary>
        ///
        /// </summary>
        /// <param name="application"></param>
        /// <param name="offerAttributeType"></param>
        void AddApplicationAttributeIfNotExists(IApplication application, OfferAttributeTypes offerAttributeType);

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        IEventList<ISPV> GetOriginatableSPVList();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="application"></param>
        void DetermineGEPFAttribute(IApplication application);
    }
}