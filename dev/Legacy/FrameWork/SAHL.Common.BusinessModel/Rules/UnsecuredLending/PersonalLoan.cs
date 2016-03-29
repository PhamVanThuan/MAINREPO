using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.Interfaces.Service;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.DataAccess;
using SAHL.Common.Globals;
using SAHL.Common.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace SAHL.Common.BusinessModel.Rules.UnsecuredLending.PersonalLoan
{
    [RuleDBTag("CheckUniquePersonalLoanApplication", "Ensures that the personal loan lead doesn't duplicate for that legal entity",
         "SAHL.Rules.DLL", "SAHL.Common.BusinessModel.Rules.UnsecuredLending.PersonalLoan.CheckUniquePersonalLoanApplication")]
    [RuleInfo]
    public class CheckUniquePersonalLoanApplication : BusinessRuleBase
    {
        private SAHL.Common.BusinessModel.Interfaces.Service.ICastleTransactionsService castleTransactionsService;

        public CheckUniquePersonalLoanApplication(SAHL.Common.BusinessModel.Interfaces.Service.ICastleTransactionsService castleTransactionsService)
        {
            this.castleTransactionsService = castleTransactionsService;
        }

        /// <summary>
        /// Execute rule to prevent multiple open offers for the selected legal entity.
        /// </summary>
        /// <param name="Messages"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] parameters)
        {
            var legalEntityKey = (int)parameters[0];

            string sql = @"select er.*
                        from [2am].dbo.ExternalRole er (nolock)
                        join [2am].dbo.Offer ofr (nolock)
	                        on er.GenericKey = ofr.offerKey
                        where
	                        ofr.offerTypeKey = ?
	                        and ofr.OfferStatusKey = ?
	                        and er.GenericKeyTypeKey = ?
	                        and er.LegalEntityKey = ?
	                        and er.ExternalRoleTypeKey = ?
	                        and er.GeneralStatusKey = ?";

            var externalRoles = castleTransactionsService.Many<IExternalRole>(QueryLanguages.Sql, sql, "er", Databases.TwoAM,
                                                        (int)OfferTypes.UnsecuredLending,
                                                        (int)OfferStatuses.Open,
                                                        (int)GenericKeyTypes.Offer,
                                                        legalEntityKey,
                                                        (int)ExternalRoleTypes.Client,
                                                        (int)GeneralStatuses.Active);
            if (externalRoles.Count() > 0)
            {
                AddMessage("Personal Loan Lead already exists for the selected legal entity", "", Messages);
                return 0;
            }
            else
            {
                return 1;
            }
        }
    }

    [RuleDBTag("CheckPersonalLoanAmount", "Checking Personal Loan Amount and Term Validity", "SAHL.Rules.DLL",
                "SAHL.Common.BusinessModel.Rules.UnsecuredLending.PersonalLoan.CheckPersonalLoanAmount")]
    [RuleInfo]
    public class CheckPersonalLoanAmount : BusinessRuleBase
    {
        private ICreditCriteriaUnsecuredLendingRepository creditCriteriaUnsecuredLendingRepository;

        public CheckPersonalLoanAmount(ICreditCriteriaUnsecuredLendingRepository creditCriteriaUnsecuredLendingRepository)
        {
            this.creditCriteriaUnsecuredLendingRepository = creditCriteriaUnsecuredLendingRepository;
        }

        /// <summary>
        /// Execute rule to check that personal loan amount must be between 10000 to 50000.
        /// </summary>
        /// <param name="Messages"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] parameters)
        {
            double PersonalLoanAmount = (double)parameters[0];

            IReadOnlyEventList<ICreditCriteriaUnsecuredLending> list = creditCriteriaUnsecuredLendingRepository.GetCreditCriteriaUnsecuredLendingList();

            double maximumAmount = list.Max(x => x.MaxLoanAmount);
            double minimumAmount = list.Min(x => x.MinLoanAmount);
            minimumAmount = Math.Round(minimumAmount);

            if (PersonalLoanAmount > maximumAmount)
            {
                AddMessage("Personal Loan amount is too high. Loan amount is R " + PersonalLoanAmount + " . The maximum allowed is: R " + maximumAmount + ".", "", Messages);
                return 0;
            }
            else if (PersonalLoanAmount < minimumAmount)
            {
                AddMessage("Personal Loan amount is too low. Loan amount is R " + PersonalLoanAmount + " . The minimum allowed is: R " + minimumAmount + ".", "", Messages);
                return 0;
            }

            return 1;
        }
    }

    [RuleDBTag("CheckPersonalLoanTerm", "Checking Loan Term Validity",
     "SAHL.Rules.DLL", "SAHL.Common.BusinessModel.Rules.UnsecuredLending.PersonalLoan.CheckPersonalLoanTerm")]
    [RuleInfo]
    public class CheckPersonalLoanTerm : BusinessRuleBase
    {
        private ICreditCriteriaUnsecuredLendingRepository creditCriteriaUnsecuredLendingRepository;

        public CheckPersonalLoanTerm(ICreditCriteriaUnsecuredLendingRepository creditCriteriaUnsecuredLendingRepository)
        {
            this.creditCriteriaUnsecuredLendingRepository = creditCriteriaUnsecuredLendingRepository;
        }

        /// <summary>
        /// Execute rule to check that personal loan term must be between 18 and 48.
        /// </summary>
        /// <param name="Messages"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] parameters)
        {
            int PersonalLoanTerm = Convert.ToInt32(parameters[0]);
            IReadOnlyEventList<ICreditCriteriaUnsecuredLending> list = creditCriteriaUnsecuredLendingRepository.GetCreditCriteriaUnsecuredLendingList();

            int minimumTerm = list.Min(x => x.Term);
            int maximumTerm = list.Max(x => x.Term);

            if (PersonalLoanTerm > maximumTerm)
            {
                AddMessage("Loan Term should not be greater than " + maximumTerm + " months.", "", Messages);
                return 0;
            }
            else if (PersonalLoanTerm < minimumTerm)
            {
                AddMessage("Loan Term should not be less than " + minimumTerm + " months.", "", Messages);
                return 0;
            }

            return 1;
        }
    }

    #region #19591 (Submit To Credit)

    #region UnsecuredLendingApplicationMailingAddress

    [RuleDBTag("UnsecuredLendingApplicationMailingAddress",
                "UnsecuredLendingApplicationMailingAddress",
                "SAHL.Rules.DLL",
                "SAHL.Common.BusinessModel.Rules.UnsecuredLending.PersonalLoan.UnsecuredLendingApplicationMailingAddress")]
    [RuleInfo]
    public class UnsecuredLendingApplicationMailingAddress : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            #region Check for allowed object type(s)

            if (Parameters.Length == 0)
            {
                throw new ArgumentException("The UnsecuredLendingApplicationMailingAddress rule expects a Domain object to be passed.");
            }

            if (!(Parameters[0] is IApplicationUnsecuredLending))
            {
                throw new ArgumentException("The UnsecuredLendingApplicationMailingAddress rule expects the following objects to be passed: IApplicationUnsecuredLending.");
            }

            #endregion Check for allowed object type(s)

            IApplicationUnsecuredLending applicationUnsecuredLending = Parameters[0] as IApplicationUnsecuredLending;

            if (applicationUnsecuredLending.ApplicationMailingAddresses.Count == 0)
            {
                AddMessage("Each Application must have one valid Application mailing address.",
                            "Each Application must have one valid Application mailing address.",
                            Messages);
                return 0;
            }

            return 1;
        }
    }

    #endregion UnsecuredLendingApplicationMailingAddress

    #region ITCUnsecuredLending

    [RuleDBTag("ITCUnsecuredLending",
                "Each Legal Entity in an application must have a valid ITC.",
                "SAHL.Rules.DLL",
                "SAHL.Common.BusinessModel.Rules.UnsecuredLending.PersonalLoan.ITCUnsecuredLending")]
    [RuleInfo]
    public class ITCUnsecuredLending : BusinessRuleBase
    {
        private SAHL.Common.BusinessModel.Interfaces.Service.ICastleTransactionsService castleTransactionsService;
        private SAHL.Common.BusinessModel.Interfaces.Service.IUIStatementService uiStatementService;

        public ITCUnsecuredLending(SAHL.Common.BusinessModel.Interfaces.Service.ICastleTransactionsService castleTransactionsService, SAHL.Common.BusinessModel.Interfaces.Service.IUIStatementService uiStatementService)
        {
            this.castleTransactionsService = castleTransactionsService;
            this.uiStatementService = uiStatementService;
        }

        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            if (!(Parameters[0] is IApplicationUnsecuredLending))
            {
                throw new ArgumentException("Parameter[0] is not of type IApplicationUnsecuredLending.");
            }

            IApplicationUnsecuredLending unsecuredLending = (IApplicationUnsecuredLending)Parameters[0];
            int appKey = unsecuredLending.Key;

            ParameterCollection prms = new ParameterCollection();
            prms.Add(new SqlParameter("@appKey", appKey));

            string sqlQuery = uiStatementService.GetStatement("COMMON", "ITCCheckUnsecuredLending");

            object ob = this.castleTransactionsService.ExecuteScalarOnCastleTran(sqlQuery, Databases.TwoAM, prms);

            if (ob == null)
            {
                AddMessage("Every Legal Entity must have a valid ITC enquiry.", "Every Legal Entity must have a valid ITC enquiry.", Messages);
                return 0;
            }

            return 1;
        }
    }

    #endregion ITCUnsecuredLending

    #region UnsecuredLendingConfirmIncome

    [RuleDBTag("UnsecuredLendingConfirmIncome",
                "Shows a warning if no incomes are confirmed.",
                "SAHL.Rules.DLL",
                "SAHL.Common.BusinessModel.Rules.UnsecuredLending.PersonalLoan.UnsecuredLendingConfirmIncome", false)]
    [RuleInfo]
    public class UnsecuredLendingConfirmIncome : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            #region Check for allowed object type(s)

            if (Parameters.Length == 0)
            {
                throw new ArgumentException("The UnsecuredLendingConfirmIncome rule expects a Domain object to be passed.");
            }

            if (!(Parameters[0] is IApplicationUnsecuredLending))
            {
                throw new ArgumentException("The UnsecuredLendingConfirmIncome rule expects the following objects to be passed: IApplicationUnsecuredLending.");
            }

            #endregion Check for allowed object type(s)

            IApplicationUnsecuredLending applicationUnsecuredLending = Parameters[0] as IApplicationUnsecuredLending;

            string errorMessage = CheckIncome(applicationUnsecuredLending);

            if (errorMessage.Length > 0)
            {
                AddMessage(errorMessage, errorMessage, Messages);

                return 0;
            }

            return 1;
        }

        private string CheckIncome(IApplicationUnsecuredLending unsecuredLending)
        {
            bool isEmploymentIncomeConfirmed = false;
            string errorMessage = string.Empty;

            var activeClientRoles = unsecuredLending.ActiveClientRoles;
            var employments = activeClientRoles.SelectMany(x => x.LegalEntity.Employment);
            isEmploymentIncomeConfirmed = employments.Any(employment =>
                                                          employment.EmploymentStatus.Key == (int)EmploymentStatuses.Current &&
                                                          employment.IsConfirmed &&
                                                          employment.ConfirmedIncomeFlag.HasValue && employment.ConfirmedIncomeFlag == true);

            if (!isEmploymentIncomeConfirmed)
            {
                errorMessage = "Employment must be confirmed before the application can be submitted.";
            }

            return errorMessage;
        }
    }

    #endregion UnsecuredLendingConfirmIncome

    #endregion #19591 (Submit To Credit)

    #region UnderDebtReviewDeclaration - #20286 (Submit To Credit)

    [RuleDBTag("UnderDebtReviewDeclaration",
    "One or more Legal Entities, in the application has answered Yes to the Under Debt Review declaration.",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.UnsecuredLending.PersonalLoan.UnderDebtReviewDeclaration")]
    [RuleInfo]
    public class UnderDebtReviewDeclaration : BusinessRuleBase
    {
        private IApplicationRepository ApplicationRepository;

        public UnderDebtReviewDeclaration(IApplicationRepository applicationRepository)
        {
            ApplicationRepository = applicationRepository;
        }

        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            int retVal = 0;

            if (!(Parameters[0] is IApplicationUnsecuredLending))
            {
                throw new ArgumentException("Parameter[0] is not of type IApplicationUnsecuredLending.");
            }

            IApplicationUnsecuredLending app = (IApplicationUnsecuredLending)Parameters[0];

            IReadOnlyEventList<ILegalEntityNaturalPerson> clients = app.GetNaturalPersonsByExternalRoleType(ExternalRoleTypes.Client, GeneralStatuses.Active);

            foreach (ILegalEntityNaturalPerson client in clients)
            {
                if (ApplicationRepository.GetApplicationDeclarationAnswerToQuestion(client.Key, app.Key, (int)OfferDeclarationQuestions.UnderDebtReview) == (int)OfferDeclarationAnswers.No
                    && ApplicationRepository.GetApplicationDeclarationAnswerToQuestion(client.Key, app.Key, (int)OfferDeclarationQuestions.CurrentDebtRearrangement) == (int)OfferDeclarationAnswers.No)
                    retVal = 1;
                else
                {
                    AddMessage("Further processing is not allowed due to legal entity debt counselling declarations.", "Further processing is not allowed due to legal entity debt counselling declarations.", Messages);
                    return 0;
                }
            }
            return retVal;
        }
    }

    #endregion UnderDebtReviewDeclaration - #20286 (Submit To Credit)

    [RuleDBTag("CheckCanEmailPersonalLoanApplication",
                "Checks if we can send email to this applications applicant.",
                "SAHL.Rules.DLL",
                "SAHL.Common.BusinessModel.Rules.UnsecuredLending.PersonalLoan.CheckCanEmailPersonalLoanApplication")]
    [RuleInfo]
    public class CheckCanEmailPersonalLoanApplication : BusinessRuleBase
    {
        private IApplicationRepository applicationRepository;

        public CheckCanEmailPersonalLoanApplication(IApplicationRepository applicationRepository)
        {
            this.applicationRepository = applicationRepository;
        }

        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            int applicationKey = (int)Parameters[0];

            IApplication application = this.applicationRepository.GetApplicationByKey(applicationKey);

            foreach (var appMailingAddress in application.ApplicationMailingAddresses)
            {
                if (appMailingAddress.CorrespondenceMedium.Key == (int)CorrespondenceMediums.Email && !string.IsNullOrEmpty(appMailingAddress.LegalEntity.EmailAddress))
                {
                    return 1;
                }
            }

            AddMessage("Unable to send email correspondence for this application.", "Unable to send email correspondence for this application.", Messages);
            return 0;
        }
    }

    [RuleDBTag("CheckChangeTermStageTransition",
               "Checks if the Change Term stage transition has fired",
               "SAHL.Rules.DLL",
               "SAHL.Common.BusinessModel.Rules.UnsecuredLending.PersonalLoan.CheckChangeTermStageTransition")]
    [RuleInfo]
    public class CheckChangeTermStageTransition : BusinessRuleBase
    {
        public CheckChangeTermStageTransition(IStageDefinitionRepository stageDefinitionRepository)
        {
            this.stageDefinitionRepository = stageDefinitionRepository;
        }

        private IStageDefinitionRepository stageDefinitionRepository;

        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            if (Parameters == null || Parameters.Length == 0 || !(Parameters[0] is int))
                throw new ArgumentException("The CheckChangeTermStageTransition rule expects a Personal Loan Account.");

            int accountKey = (int)Parameters[0];

            List<int> stageDefinition = new List<int>();
            stageDefinition.Add((int)StageDefinitionStageDefinitionGroups.PersonalLoanChangeTerm);

            IList<IStageTransition> transitions = stageDefinitionRepository.GetStageTransitionList(accountKey, (int)GenericKeyTypes.ParentAccount, stageDefinition);

            if (transitions.Count > 0)
                return 1;

            AddMessage("The letter cannot be sent if the 'Change Term' stage transition is not written", "The letter cannot be sent", Messages);
            return 0;
        }
    }

    #region UnsecuredLendingApplicationDomiciliumAddress

    [RuleDBTag("UnsecuredLendingApplicationDomiciliumAddress",
                "A Domicilium Address must be captured for all Applicants on a Personal Loan application",
                "SAHL.Rules.DLL",
                "SAHL.Common.BusinessModel.Rules.UnsecuredLending.PersonalLoan.UnsecuredLendingApplicationDomiciliumAddress")]
    [RuleInfo]
    public class UnsecuredLendingApplicationDomiciliumAddress : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            #region Check for allowed object type(s)

            if (Parameters.Length == 0)
            {
                throw new ArgumentException("The UnsecuredLendingApplicationDomiciliumAddress rule expects a Domain object to be passed.");
            }

            if (!(Parameters[0] is IApplicationUnsecuredLending))
            {
                throw new ArgumentException("The UnsecuredLendingApplicationDomiciliumAddress rule expects the following objects to be passed: IApplicationUnsecuredLending.");
            }

            #endregion Check for allowed object type(s)

            int retVal = 0;
            IApplicationUnsecuredLending applicationUnsecuredLending = Parameters[0] as IApplicationUnsecuredLending;

            IReadOnlyEventList<ILegalEntityNaturalPerson> clients = applicationUnsecuredLending.GetNaturalPersonsByExternalRoleType(ExternalRoleTypes.Client, GeneralStatuses.Active);
            IExternalRole externalRole = null;

            foreach (ILegalEntityNaturalPerson client in clients)
            {
                if (client.ActiveDomicilium != null)
                    retVal = 1;
                else
                {
                    externalRole = client.GetActiveClientExternalRoleForOffer(applicationUnsecuredLending.Key);
                    if (externalRole == null || (externalRole != null && externalRole.PendingExternalRoleDomicilium == null))
                    {
                        AddMessage("A Domicilium Address must be captured for the Applicant on a Personal Loan application.", "A Domicilium Address must be captured for the Applicant on a Personal Loan application.", Messages);
                        return 0;
                    }
                }
            }
            return retVal;
        }
    }

    #endregion UnsecuredLendingApplicationDomiciliumAddress

    [RuleDBTag("CheckAlteredApprovalStageTransition",
        "Checks if the Altered Approval stage transition is the latest stage transition for application",
        "SAHL.Rules.DLL",
        "SAHL.Common.BusinessModel.Rules.UnsecuredLending.PersonalLoan.CheckAlteredApprovalStageTransition")]
    [RuleInfo]
    public class CheckAlteredApprovalStageTransition : BusinessRuleBase
    {
        private IStageDefinitionRepository stageDefinitionRepository;

        public CheckAlteredApprovalStageTransition(IStageDefinitionRepository stageDefinitionRepository)
        {
            this.stageDefinitionRepository = stageDefinitionRepository;
        }

        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            int? applicationKey = Parameters[0] as int?;
            if (!applicationKey.HasValue)
                throw new ArgumentException("The CheckAlteredApprovalComposite rule expects a Personal Loan Application.");

            var unsecuredLendingStageTransitions = stageDefinitionRepository.GetStageTransitionsByGenericKey(applicationKey.Value, (int)StageDefinitionGroups.PersonalLoan);

            var latestStageTransition = unsecuredLendingStageTransitions.OrderByDescending(st => st.TransitionDate).FirstOrDefault();

            if (latestStageTransition != null
                && latestStageTransition.StageDefinitionStageDefinitionGroup.Key
                == (int)StageDefinitionStageDefinitionGroups.PersonalLoanAlteredApproval
                )
            {
                AddMessage("Altered Approval", "Altered Approval", Messages);
                return 0;
            }

            return 1;
        }
    }

    [RuleDBTag("SAHLCreditProtectionPlanExists",
        "Checks if SAHL Credit Protection Plan account already exists",
        "SAHL.Rules.DLL",
        "SAHL.Common.BusinessModel.Rules.UnsecuredLending.PersonalLoan.SAHLCreditProtectionPlanExists")]
    [RuleInfo]
    public class SAHLCreditProtectionPlanExists : BusinessRuleBase
    {
        private IAccountRepository accountRepository;

        public SAHLCreditProtectionPlanExists(IAccountRepository accountRepository)
        {
            this.accountRepository = accountRepository;
        }

        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            int? personalLoanAccountKey = Parameters[0] as int?;
            if (!personalLoanAccountKey.HasValue)
                throw new ArgumentException("The SAHLCreditProtectionPlanExists rule expects a Personal Loan Account Key.");

            IAccount account = accountRepository.GetAccountByKey(personalLoanAccountKey.Value);

            IAccountPersonalLoan plAccount = account as IAccountPersonalLoan;
            if (plAccount == null)
                throw new ArgumentException("The SAHLCreditProtectionPlanExists rule expects a Personal Loan Account.");

            IAccount sahlCreditProtection = plAccount.RelatedChildAccounts.SingleOrDefault(x => x.AccountType == AccountTypes.CreditProtectionPlan);

            if (sahlCreditProtection != null)
            {
                if (sahlCreditProtection.AccountStatus.Key == (int)GeneralStatusKey.Active)
                {
                    AddMessage("An open SAHL Credit Protection Plan account already exists.","Cannot create Credit Life Policy", Messages);
                    return 0;//Exists
                }
            }
            return 1;//Does not exists
        }
    }

    #region Ceded External Life Covers Loan Amount

    [RuleDBTag("CheckCededAmountCoversApplicationAmount",
        "Checks if applied External Life amount is large enough to cover the personal loan amount.",
        "SAHL.Rules.DLL",
        "SAHL.Common.BusinessModel.Rules.UnsecuredLending.PersonalLoan.CheckCededAmountCoversApplicationAmount")]
    [RuleInfo]
    public class CheckCededAmountCoversApplicationAmount : BusinessRuleBase
    {
        private IApplicationRepository applicationRepository;

        public CheckCededAmountCoversApplicationAmount(IApplicationRepository applicationRepository)
        {
            this.applicationRepository = applicationRepository;
        }

        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            int? applicationKey = Parameters[0] as int?;
            if (!applicationKey.HasValue)
                throw new ArgumentException("The CheckCededAmountCoversApplicationAmount rule expects a Personal Loan Application.");

            double? sumInsured = Parameters[1] as double?;
            if (!sumInsured.HasValue)
                throw new ArgumentException("The CheckCededAmountCoversApplicationAmount rule expects the sum insured value.");

            var insuranceAmountFullyCoversLoan = false;

            var application = applicationRepository.GetApplicationByKey(applicationKey.Value);
            IApplicationUnsecuredLending applicationUnsecuredLending = (IApplicationUnsecuredLending)application;
            if (applicationUnsecuredLending != null)
            {
                if (applicationUnsecuredLending.GetLatestApplicationInformation() == null)
                {
                    //throw new ArgumentException("The CheckSAHLLifeIsApplied rule expects a Personal Loan Application and not a lead.");
                    AddMessage("Personal Loan Application expected", "Policy cannot cover a lead", Messages);
                    return 0;
                }

                var applicationProductPersonalLoan = applicationUnsecuredLending.CurrentProduct as IApplicationProductPersonalLoan;
                if (applicationProductPersonalLoan == null)
                {
                    AddMessage("Personal Loan covered by external life policy expected", "Personal Loan covered by external life policy expected", Messages);
                    return 0;
                }
                insuranceAmountFullyCoversLoan = sumInsured.Value >= applicationProductPersonalLoan.ApplicationInformationPersonalLoan.LoanAmount;
            }

            if (!insuranceAmountFullyCoversLoan)
            {
                AddMessage("Sum insured is not enough to cover loan amount", "Sum insured is not enough to cover loan amount", Messages);
                return 0;
            }

            return 1;
        }
    }

    #endregion Ceded External Life Covers Loan Amount

    [RuleDBTag("CheckExternalPolicyIsCeded",
     "Checks if applied External Life policy is fully captured and ceded.",
     "SAHL.Rules.DLL",
     "SAHL.Common.BusinessModel.Rules.UnsecuredLending.PersonalLoan.CheckExternalPolicyIsCeded")]
    [RuleInfo]
    public class CheckExternalPolicyIsCeded : BusinessRuleBase
    {
        private IApplicationRepository applicationRepository;

        public CheckExternalPolicyIsCeded(IApplicationRepository applicationRepository)
        {
            this.applicationRepository = applicationRepository;
        }

        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            int? applicationKey = Parameters[0] as int?;
            if (!applicationKey.HasValue)
                throw new ArgumentException("The CheckExternalPolicyIsCeded rule expects a Personal Loan Application.");

            var policyIsCeded = false;

            var application = applicationRepository.GetApplicationByKey(applicationKey.Value);
            IApplicationUnsecuredLending applicationUnsecuredLending = (IApplicationUnsecuredLending)application;
            if (applicationUnsecuredLending != null)
            {
                if (applicationUnsecuredLending.GetLatestApplicationInformation() == null)
                {
                    //throw new ArgumentException("The CheckSAHLLifeIsApplied rule expects a Personal Loan Application and not a lead.");
                    AddMessage("Personal Loan Application expected got lead instead", "Personal Loan Application expected got lead instead", Messages);
                    return 0;
                }

                //Ensure SAHL Life is not applied
                if (CheckApplicationHasSAHLLifeApplied(Messages, applicationUnsecuredLending))
                {
                    //AddMessage("SAHL Life is applied", "SAHL Life is applied", Messages);
                    return 0;
                }

                var applicationProductPersonalLoan = applicationUnsecuredLending.CurrentProduct as IApplicationProductPersonalLoan;
                var externalLifePolicy = applicationProductPersonalLoan.ExternalLifePolicy;

                if (externalLifePolicy == null)
                {
                    AddMessage("No Life Policy exists.  Please capture an SAHL or External Life Policy", "No Life Policy exists.  Please capture an SAHL or External Life Policy", Messages);
                    return 0;
                }

                //Ensure fully captured
                if (CheckCompulsoryFieldsArePopulated(externalLifePolicy, Messages) == false)
                {
                    return 0;
                }

                policyIsCeded = externalLifePolicy.PolicyCeded;
            }

            if (!policyIsCeded)
            {
                AddMessage("External Policy is not ceded", "External Policy is not ceded", Messages);
                return 0;
            }

            return 1;
        }

        private bool CheckApplicationHasSAHLLifeApplied(IDomainMessageCollection Messages, IApplicationUnsecuredLending applicationUnsecuredLending)
        {
            bool hasSAHLLifeApplied = false;
            if (applicationUnsecuredLending != null)
            {
                if (applicationUnsecuredLending.GetLatestApplicationInformation() == null)
                {
                    AddMessage("Life Policy cannot be captured for a lead", "Life Policy cannot be captured for a lead", Messages);
                }

                var applicationProductPersonalLoan = applicationUnsecuredLending.CurrentProduct as IApplicationProductPersonalLoan;
                var applicationInformationPersonalLoan = applicationProductPersonalLoan.ApplicationInformationPersonalLoan;
                hasSAHLLifeApplied = applicationInformationPersonalLoan.LifePremium > 0;
            }

            return hasSAHLLifeApplied;
        }

        private bool CheckCompulsoryFieldsArePopulated(IExternalLifePolicy externalLifePolicy, IDomainMessageCollection messageCollection)
        {
            string errorMessage = "";
            bool policyDetailsFullyCaptured = true;

            if (externalLifePolicy.Insurer == null)
            {
                errorMessage = "Insurer must be captured.";
                AddMessage(errorMessage, errorMessage, messageCollection);
                policyDetailsFullyCaptured = false;
            }

            if (string.IsNullOrEmpty(externalLifePolicy.PolicyNumber))
            {
                errorMessage = "Policy number must be captured.";
                AddMessage(errorMessage, errorMessage, messageCollection);
                policyDetailsFullyCaptured = false;
            }

            if (externalLifePolicy.CommencementDate == null)
            {
                errorMessage = "Commencement Date must be captured.";
                AddMessage(errorMessage, errorMessage, messageCollection);
                policyDetailsFullyCaptured = false;
            }

            if (externalLifePolicy.LifePolicyStatus == null)
            {
                errorMessage = "Policy Status must be captured.";
                AddMessage(errorMessage, errorMessage, messageCollection);
                policyDetailsFullyCaptured = false;
            }

            if (externalLifePolicy.SumInsured <= 0)
            {
                errorMessage = "Valid sum insured value must be captured.";
                AddMessage(errorMessage, errorMessage, messageCollection);
                policyDetailsFullyCaptured = false;
            }

            if (externalLifePolicy.LifePolicyStatus.Description.Equals("Closed", StringComparison.InvariantCultureIgnoreCase)
                && externalLifePolicy.CloseDate < System.DateTime.Now)
            {
                errorMessage = "Closed policy cannot be used to cover personal loan.";
                AddMessage(errorMessage, errorMessage, messageCollection);
                policyDetailsFullyCaptured = false;
            }

            return policyDetailsFullyCaptured;
        }
    }

    [RuleDBTag("CheckIfCapitecClient", "Ensures that the personal loan lead doesn't get created for a Capitec Client",
     "SAHL.Rules.DLL", "SAHL.Common.BusinessModel.Rules.UnsecuredLending.PersonalLoan.CheckIfCapitecClient")]
    [RuleInfo]
    public class CheckIfCapitecClient : BusinessRuleBase
    {
        private ICastleTransactionsService castleTransactionsService;

        public CheckIfCapitecClient(SAHL.Common.BusinessModel.Interfaces.Service.ICastleTransactionsService castleTransactionsService)
        {
            this.castleTransactionsService = castleTransactionsService;
        }

        /// <summary>
        /// Execute rule to prevent personal loan lead from being created for a capitec client.
        /// </summary>
        /// <param name="Messages"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] parameters)
        {
            var legalEntity = parameters[0] as ILegalEntity;

            ParameterCollection prms = new ParameterCollection();
            prms.Add(new SqlParameter("@LegalEntityKey", legalEntity.Key));

            string sql = @"select [dbo].[fIsCapitecClient] (@LegalEntityKey)";

            // check if the client has an open capitec loan or an open/accepted capitec offer
            var openAcceptedOffers = castleTransactionsService.ExecuteScalarOnCastleTran(sql, Databases.TwoAM, prms);

            // check if the client has a capitec offer that is ntu/decline but not finalized
            bool openNTUNotFinalizedOffer = false;
            OfferRoleTypes[] offerRoleTypes = new OfferRoleTypes[] {OfferRoleTypes.LeadMainApplicant,OfferRoleTypes.LeadSuretor, OfferRoleTypes.MainApplicant, OfferRoleTypes.Suretor};
            IReadOnlyEventList<IApplicationRole> offerRoles = legalEntity.GetApplicationRolesByRoleTypes(offerRoleTypes);
            foreach (var offerRole in offerRoles)
            {
                if (offerRole.Application.IsCapitec() && offerRole.Application.IsOpen)
                {
                    openNTUNotFinalizedOffer = true;
                    break;
                }
            }

            if (Convert.ToBoolean(openAcceptedOffers) || openNTUNotFinalizedOffer)
            {
                string msg = "This is a Capitec Client and therefore cannot be offered a Personal Loan with SAHL.";
                AddMessage(msg, msg, Messages);
                return 0;
            }
            else
            {
                return 1;
            }
        }
    }
}