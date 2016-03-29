using System.Collections.Generic;

namespace SAHL.Common
{
    /// <summary>
    /// </summary>
    public class Constants
    {
        // These formats are to be used when casting to string types
        public const string CurrencyFormat = "R ###,###,###,##0.00";

        public const string CurrencyFormatNoCents = "R ###,###,###,##0";
        public const string CurrencyFormatNoSymbolNoCents = "###,###,###,##0";
        public const string DateFormat = "dd/MM/yyyy";
        public const string DateTimeFormat = "dd/MM/yyyy HH:mm";
        public const string DateTimeFormatWithSeconds = "dd/MM/yyyy HH:mm:ss";
        public const string DateFormatNoCentury = "dd/MM/yy";
        public const string NumberFormat = "0.00";
        public const string RateFormat = "0.00%";
        public const string DateFixedMessage = "The captured date has been modified. Please verify that it is correct.";
        public const string DefaultDropDownItem = "-select-";
        public const string RateFormat3Decimal = "0.000%";
        public const string NumberFormat3Decimal = "0.000";

        public const string CboNodeSet = "CBO";
        public const string X2NodeSet = "X2";
        public const string RegentDescription = "Regent Life Policy";

        public const string ReadonlyAccess = "Readonly";

        public class ADUsers
        {
            public const string System = "System";
        }

        public class DetailTypes
        {
            public const string NewLegalAgreementSigned = "New Legal Agreement Signed";
        }

        public class Valuators
        {
            public const string SAHLEstimate = "SAHL Estimate";

            public static List<string> GetFilteredValuators()
            {
                List<string> valList = new List<string>();
                valList.Add(SAHLEstimate);
                return valList;
            }
        }

        public class EstateAgent
        {
            public const string Principal = "Principal";
            public const string Consultant = "Consultant";
        }

        public class PaymentDistributionAgent
        {
            public const string Contact = "Contact";
        }

        public class DebtCounsellor
        {
            public const string Contact = "Contact";
        }

        public class Proposals
        {
            public const int FixedMarketRateKey = 99;
            public const string FixedMarketRateDesc = "Fixed";

            public const int HOCLifeExclusiveKey = 0;
            public const string HOCLifeExclusiveDesc = "Exclusive";

            public const int HOCLifeInclusiveKey = 1;
            public const string HOCLifeInclusiveDesc = "Inclusive";
        }

        public class Reports
        {
            public const string DisbursementLetter = "Disbursement Letter";
        }

        public class ControlTable
        {
            public class HOC
            {
                public const string HOCAdministrationFee = "HOCAdministrationFee";
            }

            public class QuickCash
            {
                public const string MinimumQuickCash = "MinimumQuickCash";
                public const string QuickCashThresholdPercentage = "Quick Cash Threshold%";
                public const string QuickCashMaximum = "QuickCash Maximum";
            }

            public class Life
            {
                public const string AssuredLifeMinAge = "LifeAssuredMinAge";
                public const string AssuredLifeMaxAge = "LifeAssuredMaxAge";
                public const string GroupExposureMaxPolicies = "LifeGroupExposureMaxPolicies";
                public const string IPBenefitMaxAge = "LifeIPBenefitMaxAge";
                public const string RPARMailFrom = "Life RPAR Mail From";
                public const string RPARMailTo = "Life RPAR Mail To";
                public const string RPARMailCC = "Life RPAR Mail CC";
                public const string RPARMailSubject = "Life RPAR Mail Subject";
                public const string LOAMailFrom = "Life LOA Mail From";
                public const string LOAMailTo = "Life LOA Mail To";
                public const string LOAMailCC = "Life LOA Mail CC";
                public const string LOAMailSubject = "Life LOA Mail Subject";
                public const string LifeNotifyChangeOfIDNumber = "Life NotifyChangeOfIDNumber To";
            }

            public class Batch
            {
                public const string CapExtractJob = "BatchCapExtractJob";
                public const string CapImportJob = "BatchCapImportJob";
                public const string CapMailingExportJob = "BatchCapMailingExportJob";
                public const string DataReportJob = "BatchDataReportJob";
                public const string LoanStatementJob = "BatchLoanStatementJob";
            }

            public class ClientSurvey
            {
                public const string ClientSurveyMailTo = "Client Survey Mail";
                public const string ClientSurveyMailFrom = "Client Survey From Mail";
                public const string ClientSurveyURL = "Client Survey SAHL Website URL";
            }

            public class PersonalLoan
            {
                public const string MaxPersonalLoanTerm = "MaxPersonalLoanTerm";
                public const string PersonalLoanDisbursementCutOffTime = "PersonalLoanDisbursementCutOffTime";
            }

            public class CAP
            {
                public const string CapEndDateTolerence = "CAP Endate Tolerance";
            }

            public class AlphaHousingSurvey
            {
                public const string IsActivated = "IsAlphaHousingSurveyActivated";
            }

            public const string FaxEmailDomain = "FaxEmailDomain";
            public const string SMTPHostname = "SMTP Hostname";
            public const string SMTPPort = "SMTP Port";
            public const string CorrespondenceDataStagingFolder = "Correspondence Data Staging Folder";
            public const string SAHLMonthlyFee = "SAHLMonthlyFee";
            public const string SAHLMonthlyFeeDiscounted = "SAHLMonthlyFeeDiscounted";

            public const string ServiceUser = "ServiceUser";
            public const string ServicePassword = "ServicePassword";
            public const string ServiceDomain = "ServiceDomain";

            public const string HALOEmailAddress = "HALO Email";
            public const string HALOExceptionEmailAddress = "HALO Exception Email";

			public const string SAHLHotline = "SAHLHotline";
			public const string HelpDeskContactNumber = "Helpdesk Contact Number";

            public const string ReturningMainApplicantInitiationFeeDiscount = "Returning Main Applicant Initiation Fee Discount";

            public const string CreditMatrixKeyWithGEPF_Introduced = "Credit matrix key with GEPF introduced";

            public const string StopOrderDiscountPercentage = "Stop Order Discount Percentage";
        }

        // WorkFlow States - Debt Counselling WorkFlow Map
        public class DebtCounsellingWorkFlowStates
        {
            public const string ReviewNotification = "Review Notification";
        }

        // WorkFlow Name Constants
        public class WorkFlowName
        {
            public const string ApplicationManagement = "Application Management";
            public const string ApplicationCapture = "Application Capture";
            public const string Cap2Offers = "CAP2 Offers";
            public const string Credit = "Credit";
            public const string DebtCounselling = "Debt Counselling";
            public const string DeleteDebitOrder = "Delete Debit Order";
            public const string LoanAdjustments = "Loan Adjustments";
            public const string HelpDesk = "Help Desk";
            public const string LifeOrigination = "LifeOrigination";
            public const string Origination = "Application Management";
            public const string QuickCash = "Quick Cash";
            public const string RCS = "RCS";
            public const string ReadvancePayments = "Readvance Payments";
            public const string ReleaseAndVariations = "Release And Variations";
            public const string Valuations = "Valuations";
            public const string PersonalLoans = "Personal Loans";
            public const string DisabilityClaim = "Disability Claim";
        }

        // WorkFlow Process Name Constants
        public class WorkFlowProcessName
        {
            public const string Cap2Offers = "CAP2 Offers";
            public const string Credit = "Origination";
            public const string DeleteDebitOrder = "Delete Debit Order";
            public const string DebtCounselling = "Debt Counselling";
            public const string LoanAdjustments = "Loan Adjustments";
            public const string HelpDesk = "Help Desk";
            public const string LifeOrigination = "Life";
            public const string Origination = "Origination";
            public const string QuickCash = "Origination";
            public const string RCS = "RCS";
            public const string PersonalLoan = "Personal Loan";
            public const string LifeClaims = "LifeClaims";
        }

        // WorkFlow ActivityName
        public class WorkFlowActivityName
        {
            public const string ApplicationCreate = "Create Instance";
            public const string ApplicationFurtherLendingCreate = "Further Lending Calc";
            public const string ApplicationWizardCreate = "Create Application Wizard";
            public const string HelpDeskCreate = "Create Request";
            public const string HelpDeskComplete = "Complete Request";
            public const string HelpDeskRoute = "Route to Consultant";
            public const string DeleteDebitOrderCreate = "Create Request";
            public const string LoanAdjustmentCreateInstance = "Create Instance";
            public const string CreateDebtCounsellingCase = "Create Debt Counselling Case";
            public const string CreatePersonalLoanLead = "Create Personal Loan Lead";
            public const string CreateApplication = "Create Application";
            public const string SubmitPersonalLoanAppToCredit = "Application in Order";
            public const string CreditResubmit = "Resubmit";
            public const string RetryAssignTeleConsultant = "RetryAssignTeleConsultant";
            public const string RetryInternetCreate = "RetryInternetCreate";
            public const string BatchCreatePersonalLoanLead = "Batch Create PL Lead";
            public const string ConfirmApplicationEmploymentType = "Confirm Application Employment";
            public const string ClearCache = "Clear Cache";
            public const string ClearRuleCache = "ClearRuleCache";
        }

        public class WorkFlowExternalActivity
        {
            public const string UnderCancellation = "UnderCancellation";
            public const string UnderCancellationRemoved = "UnderCancellationRemoved";
            public const string CancellationRegistered = "CancellationRegistered";
            public const string CreateInstanceFromAppCapture = "CreateInstanceFromAppCapture";
            public const string ApplicationManagementArchiveValuation = "EXTArchiveValuation";
            public const string ApplicationManagementArchiveQuickCash = "EXTArchiveQC";
            public const string ApplicationManagementArchiveMainCase = "EXTArchiveMainCase";
            public const string EXTWatchdog = "EXTWatchdog";
            public const string MoveApplicationToHold = "MoveAppToHold";
            public const string ReturnApplicationFromHold = "ReturnAppFromHold";
            public const string PerformFurtherValuation = "Perform Further Valuation";
            public const string CloneCleanupArchive = "EXTCloneCleanupArchive";
            public const string EXTCleanupArchive = "EXTCleanupArchive";
            public const string ApplicationReceived = "Application Received";
            public const string PersonalLoanExternalNTU = "External NTU";
            public const string Reset30DayTimer = "EXTReset30DayTimer";
            public const string EXTCreateCapitecInstance = "EXTCreateCapitecInstance";
        }

        // WorkFlow DataTables
        public class WorkFlowDataTables
        {
            public const string LifeOrigination = "LifeOrigination";
            public const string Origination = "Application_Management";
            public const string ApplicationCapture = "Application_Capture";
            public const string Credit = "Credit";
            public const string DebtCounselling = "Debt_Counselling";
            public const string QuickCash = "Quick_Cash";
            public const string ReleaseAndVariations = "Release_And_Variations";
            public const string ReadvancePayments = "Readvance_Payments";
            public const string Valuations = "Valuations";
            public const string HelpDesk = "Help_Desk";
            public const string CAP2Offers = "CAP2_Offers";
            public const string DeleteDebitOrder = "Delete_Debit_Order";
            public const string LoanAdjustments = "Loan_Adjustments";
            public const string PersonalLoans = "Personal_Loans";
        }

        // WorkFlow DataTables
        public class EworkActionNames
        {
            public const string X2ClientWonOver = "X2 Client Won Over";
            public const string X2ClientRefused = "X2 Client Refused";
            public const string X2NTUAdvise = "X2 NTU ADVISE";
            public const string X2UNREGISTER = "X2 UN REGISTER";
            public const string X2REINSTRUCTED = "X2 REINSTRUCTED";
            public const string X2ARCHIVE = "X2 ARCHIVE";
            public const string X2ROLLBACKDISBURSEMENT = "X2 ROLLBACK DISBURSEMENT";
            public const string X2DISBURSEMENTTIMER = "X2 DISBURSEMENT TIMER";
            public const string X2RESUB = "X2 RESUB";
            public const string X2HOLDOVER = "X2 HOLD OVER";
            public const string X2DECLINEFINAL = "X2 DECLINEFINAL";
            public const string X2NTUReason = "X2NTUReason";

            public const string X2DebtCounselling = "X2 Debt Counselling";
            public const string X2ReturnDebtCounselling = "X2 Return Debt Counselling";
            public const string CancelRCSDebtCounselling = "Cancel RCS Debt Counselling";
            public const string TerminateDebtCounselling = "Terminate Debt Counselling";
        }

        // WorkFlow Custom Variable
        public class WorkFlowCustomVariable
        {
            public const string DeleteDebitOrder = "DebitOrderKey";
        }

        // LightStone Constants
        public class LightStone
        {
            public class TableNames
            {
                public const string Property = "Property";
                public const string TitleDeed = "TitleDeed";
                public const string Transfers = "Transfers";
            }

            public class PropertyTable
            {
                public const string PropertyID = "prop_id";
                public const string Township = "Township";
                public const string ERF = "ERF";
                public const string Portion = "PORTION";
                public const string Size = "size";
                public const string Registrar = "Registrar";
                public const string SGCode = "SG_Code";
                public const string Suburb = "Suburb";
                public const string X = "X";
                public const string Y = "Y";
                public const string StreetNumber = "STREET_NUMBER";
                public const string StreetName = "STREET_NAME";
                public const string StreetType = "STREET_TYPE";
            }

            public class TitleDeedTable
            {
                public const string PropertyID = "prop_id";
                public const string TitleDeedNo = "title_deed_no";
            }

            public class TransfersTable
            {
                public const string PropertyID = "prop_id";
                public const string BuyersName = "buyer_name";
                public const string SellersName = "seller_name";
                public const string PurchaseDate = "purch_date";
                public const string RegistrationDate = "registration_date";
                public const string PurchasePrice = "purch_price";
                public const string BondBank = "bondbank";
                public const string BondAmount = "bond_amount";
                public const string OrigInstitutionID = "orig_institution_id";
                public const string CurrentInstitutionID = "current_institution_id";
                public const string BondNumber = "bond_number";
                public const string DeedOffice = "deedoffice";
            }
        }

        /// <summary>
        ///
        /// </summary>
        public class StageDefinitionConstants
        {
            /// <summary>
            ///
            /// </summary>
            public const string ContactPersonChanged = "Contact Person Changed";

            /// <summary>
            ///
            /// </summary>
            public const string AddAssuredLife = "Add Assured Life";

            /// <summary>
            ///
            /// </summary>
            public const string RemoveAssuredLife = "Remove Assured Life";

            /// <summary>
            ///
            /// </summary>
            public const string RecalculatePremiums = "Recalculate Premiums";

            /// <summary>
            ///
            /// </summary>
            public const string DocumentProcessed = "Document Processed";

            /// <summary>
            ///
            /// </summary>
            public const string QuoteRequested = "Quote Requested";

            /// <summary>
            ///
            /// </summary>
            public const string QuoteConsidering = "Quote Considering";

            /// <summary>
            ///
            /// </summary>
            public const string QuoteDeclined = "Quote Declined";

            /// <summary>
            ///
            /// </summary>
            public const string ApplicationArchived = "Application Archived";

            /// <summary>
            ///
            /// </summary>
            public const string CallBackSet = "Callback Set";

            /// <summary>
            ///
            /// </summary>
            public const string ApplicationNTUd = "Application NTU'd";

            /// <summary>
            ///
            /// </summary>
            public const string ApplicationReinstated = "Application Reinstated";

            /// <summary>
            ///
            /// </summary>
            public const string PolicyCancelled = "Policy Cancelled";

            /// <summary>
            ///
            /// </summary>
            public const string RPAR_Completed = "RPAR Completed";

            /// <summary>
            ///
            /// </summary>
            public const string RPAR_Considering = "RPAR Considering";

            /// <summary>
            ///
            /// </summary>
            public const string CreateApplication = "Create Application";

            /// <summary>
            ///
            /// </summary>
            public const string AssignApplication = "Assign Application";

            /// <summary>
            ///
            /// </summary>
            public const string CreateLifePolicy = "Create Life Policy";

            /// <summary>
            ///
            /// </summary>
            public const string UpdateFinancialAdjustment = "Inactivated Financial Adjustment";

            /// <summary>
            ///
            /// </summary>
            public const string AddLegalEntityToDebtCounsellingCase = "Add Legal Entity to Debt Counselling Case";

            /// <summary>
            ///
            /// </summary>
            public const string RemoveLegalEntityFromDebtCounsellingCase = "Remove Legal Entity from Debt Counselling Case";

            /// <summary>
            ///
            /// </summary>
            public const string CreateCreditLife = "Create Credit Life";
        }

        // WebConfig Constants
        public class WebConfig
        {
            public const string CorrespondenceSection = "Correspondence";
        }

        // Data STOR Constants
        public class DataSTOR
        {
            public const string CorrespondenceTest = "CorrespondenceTest";
            public const string PersonalLoans = "HALO Correspondence";
        }
    }

    public enum CBONodeType
    {
        StaticCBONode = 1,
        ClientCBONodeType = 2,//2
        AccountCBONodeType = 3,//3
        FinacialServiceCBONodeType = 4,//4
        ParentAccountCBONodeType = 5,//5
        WorkFlowType = 6,//6
        RelatedLegalEntityType = 7,
        CapOfferCBONodeType = 8,
        LifeFinancialServiceCBONodeType = 9
    };

    public enum CboMenuNodeTypes
    {
        Unknown = 'U',
        Static = 'S',
        Dynamic = 'D',
        Workflow = 'W',
        WorkflowFolder = 'F',
        TNode = 'T'
    }

    public class RuleSets
    {
        public const string ApplicationManagementResubmitToCredit = "AM - Resubmit To Credit";
        public const string ApplicationCaptureSubmitApplication = "AC - Submit Application";
        public const string ApplicationCaptureManagerSubmitApplication = "AC - Manager Submit Application";
        public const string CreditApproval = "CR - Credit Approval";
        public const string CreditScoring = "Credit Scoring";
        public const string ApplicationManagementQAComplete = "AM - QA Complete";
        public const string ApproveTermChange = "Approve Term Change";
        public const string ApplicationManagementApplicationInOrder = "AM - Application In Order";
        public const string ApplicationManagementValuationRequired = "ValuationRequired";
        public const string ApplicationManagementResubOverRideCheck = "AM - ResubOverRideCheck";
        public const string ApplicationManagementCreditOverrideCheck = "AM - Credit Override Check";
        public const string DebtCounsellingSendToLitigation = "DC - Send To Litigation";
        public const string DebtCounselling5DayTerminationReminder = "DebtCounselling5DayTerminationReminder";
        public const string DebtCounsellingCheckNoDateNoPay = "DebtCounsellingCheckNoDateNoPay";
        public const string DebtCounselling45DayReminder = "DebtCounselling45DayReminder";
        public const string DebtCounselling10DayTerminationReminder = "DebtCounselling10DayTerminationReminder";
        public const string DebtCounsellingSendTerminationLetter = "DebtCounsellingSendTerminationLetter";
        public const string DebtCounsellingAttorneyToOppose = "DebtCounsellingAttorneyToOppose";
        public const string DebtCounsellingAcceptProposal = "DC - Accept Proposal";
        public const string DebtCounsellingPaymentReceived = "DCPaymentReceived";
        public const string DebtCounsellingSendCounterProposal = "DC - Send Counter Proposal";
        public const string DebtCounsellingProposalDecline = "DebtCounsellingProposalDecline";
        public const string DebtCounsellingApproveProposal = "DC - Approve Proposal";
        public const string ValuationsAutoValuation = "AutoValuation";
        public const string ValuationsValuationInOrder = "ValuationInOrder";
        public const string ApplicationManagementInstructAttorney = "InstructAttorney";
        public const string ApplicationManagementCheckUserOrganisationalStructure = "CheckUserOrganisationalStructure";
        public const string PersonalLoanApplicationInOrder = "PersonalLoan - Application in Order";
        public const string PersonalLoanSendOffer = "PersonalLoan - Send Offer";
        public const string ApplicationMinimumIncome = "Application Minimum Income";
        public const string PersonalLoanApplicationInOrderClient = "PersonalLoan - Application in Order Client";
    }

    public class Rules
    {
        public const string ApplicationDebitOrderCollection = "ApplicationDebitOrderCollection";
        public const string CheckEWorkInResubmitted = "CheckEWorkInResubmitted";
        public const string CheckCanMoveToDisbursement = "CheckCanMoveToDisbursement";
        public const string DisbursementCutOffTimeCheck = "DisbursementCutOffTimeCheck";
        public const string PersonalLoanDisbursementCutOffTimeCheck = "PersonalLoanDisbursementCutOffTimeCheck";
        public const string CheckCanEmailPersonalLoanApplication = "CheckCanEmailPersonalLoanApplication";
        public const string ApplicationIsAlphaHousing = "ApplicationIsAlphaHousing";
        public const string DebtCounsellingProductIsPersonalLoan = "DebtCounsellingProductIsPersonalLoan";
        public const string DebtCounsellingProductIsMortgageLoan = "DebtCounsellingProductIsMortgageLoan";
        public const string AlphaHousingLinkRateCanNotBeLowerThanExistingLinkRate = "AlphaHousingLinkRateCanNotBeLowerThanExistingLinkRate";
        public const string CheckAlteredApprovalStageTransition = "CheckAlteredApprovalStageTransition";
        public const string CheckCededAmountCoversApplicationAmount = "CheckCededAmountCoversApplicationAmount";
        public const string CheckSAHLLifeIsApplied = "CheckSAHLLifeIsApplied";
        public const string CheckExternalPolicyDetailsFullyCaptured = "CheckExternalPolicyDetailsFullyCaptured";
        public const string CheckExternalPolicyIsCeded = "CheckExternalPolicyIsCeded";
        public const string CheckEmploymentTypeConfirmed = "CheckEmploymentTypeConfirmed";
        public const string ApplicationIsReturningClient = "ApplicationIsReturningClient";
        public const string ComcorpApplicationRequired = "ComcorpApplicationRequired";
        public const string ApplicationCannotHave30YearTerm = "ApplicationCannotHave30YearTerm";
        public const string ApplicationHasPrevious30YearTermDisqualification = "ApplicationHasPrevious30YearTermDisqualification";
        public const string ApplicationHas30YearTermDisqualification = "ApplicationHas30YearTermDisqualification";
        public const string ApplicationIsNewBusiness = "ApplicationIsNewBusiness";
        public const string ApplicationHas30YearTerm = "ApplicationHas30YearTerm";
        public const string ApplicationProductEdgeLTVWarning = "ApplicationProductEdgeLTVWarning";
        public const string ApplicationProductEdgeLTVError = "ApplicationProductEdgeLTVError";
        public const string CapitaliseInitiationFeeLTV = "CapitaliseInitiationFeeLTV";
        public const string NaedoDebitOrderPending = "NaedoDebitOrderPending";
    }

    public class OrganisationStructure
    {
        public const string Admin = "Admin";
        public const string Manager = "Manager";
        public const string Consultant = "Consultant";
    }

    public class ApplicationCapture
    {
        public class WorkflowRoles
        {
            public const string BranchAdminD = "Branch Admin D";
            public const string BranchConsultantD = "Branch Consultant D";
            public const string BranchManagerD = "Branch Manager D";
        }
    }

    public class ApplicationManagement
    {
        public class EmailAddresses
        {
            public const string FromHalo = "HALO@sahomeloans.com";
            public const string ToRegQueries = "regqueries@sahomeloans.com";
        }
    }

    public class WorkflowState
    {
        public const string ValuationHold = "Valuation Hold";
        public const string QuickCashHold = "QuickCash Hold";
        public const string AwaitingApplication = "Awaiting Application";
        public const string AssignTeleConsultant = "Assign TeleConsultant";
        public const string ManagerReview = "Manager Review";
    }

    public class LoanTerms
    {
        public const int LoanTerm20YearInMonths = 240;
        public const int LoanTerm30YearInMonths = 360;
    }
}