using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Common.Constants
{
    /// <summary>
    /// AD User Groups
    /// </summary>
    public class ADUserGroups
    {
        public const string RegistrationsAdministrator = "Registrations Administrator";
        public const string RegistrationsSupervisor = "Registrations Supervisor";
        public const string RegistrationsLOAAdmin = "Registrations LOA Admin";
        public const string RegistrationsManager = "Registrations Manager";
        public const string ResubmissionAdmin = "Resubmission Admin";
    }

    public class Formats
    {
        /// <summary>
        /// R ###,###,###,##0.00
        /// </summary>
        public const string CurrencyFormat = "R ###,###,###,##0.00";

        /// <summary>
        /// R ###,###,###,##0
        /// </summary>
        public const string CurrencyFormatNoCents = "R ###,###,###,##0";

        /// <summary>
        /// dd/MM/yyyy
        /// </summary>
        public const string DateFormat = "dd/MM/yyyy";

        /// <summary>
        /// yyyy/MM/dd
        /// </summary>
        public const string DateFormat2 = "yyyy/MM/dd";

        /// <summary>
        /// dd/MM/yyyy HH:mm
        /// </summary>
        public const string DateTimeFormat = "dd/MM/yyyy HH:mm";

        /// <summary>
        /// yyyy-MM-dd HH:mm:ss
        /// </summary>
        public const string DateTimeFormatSQL = "yyyy-MM-dd HH:mm:ss";

        /// <summary>
        /// dd/MM/yy
        /// </summary>
        public const string DateFormatNoCentury = "dd/MM/yy";

        /// <summary>
        /// ##0.00%
        /// </summary>
        public const string Percentage = "##0.00%";

        /// <summary>
        /// ##0.## %
        /// </summary>
        public const string Percentage2 = "##0.## %";

        /// <summary>
        /// R ###,###,###,##0.00
        /// </summary>
        public const string CurrencyFormat2 = "R ### ### ##0.00";
    }

    /// <summary>
    /// Memo Status
    /// </summary>
    public class MemoStatus
    {
        public const string Resolved = "Resolved";
        public const string UnResolved = "UnResolved";
        public const string All = "All";
    }

    public class ADUserStatus
    {
        public const string Active = "Active";
        public const string Inactive = "Inactive";
    }

    /// <summary>
    /// Loan Adjustment Type
    /// </summary>
    public class LoanAdjustmentType
    {
        public const int TermChange = 1;
    }

    /// <summary>
    /// CAP Status used for the CapOffer and CapOfferDetail tables
    /// </summary>
    public class CAPStatus
    {
        public const string Open = "Open";
        public const string TakenUp = "Taken Up";
        public const string NotTakenUp = "Not Taken Up";
        public const string Expired = "Expired";
        public const string Recalculated = "Recalculated";
        public const string ReadvanceRequired = "Readvance Required";
        public const string OfferDeclined = "Offer Declined";
        public const string PrepareForCredit = "Prepare For Credit";
        public const string CreditApproval = "Credit Approval";
        public const string CheckCashPayment = "Check Cash Payment";
        public const string GrantedCap2Offer = "Granted Cap2 Offer";
        public const string AwaitingDocuments = "Awaiting Documents";
        public const string AwaitingLA = "Awaiting LA";
        public const string CallbackHold = "Callback Hold";
        public const string ReadytoCallback = "Ready to Callback";
    }

    /// <summary>
    /// Workflow Names
    /// </summary>
    public class Workflows
    {
        public const string ApplicationCapture = "Application Capture";
        public const string CAP2Offers = "CAP2 Offers";
        public const string Credit = "Credit";
        public const string DeleteDebitOrder = "Delete Debit Order";
        public const string LoanAdjustments = "Loan Adjustments";
        public const string HelpDesk = "Help Desk";
        public const string LifeOrigination = "LifeOrigination";
        public const string ApplicationManagement = "Application Management";
        public const string QuickCash = "Quick Cash";
        public const string RCS = "RCS";
        public const string ReadvancePayments = "Readvance Payments";
        public const string ReleaseAndVariations = "Release And Variations";
        public const string Valuations = "Valuations";
        public const string DebtCounselling = "Debt Counselling";
        public const string IT = "IT";
        public const string PersonalLoans = "Personal Loans";
        public const string DisabilityClaim = "Disability Claim";
    }

    public class Processes
    {
        public const string PersonalLoan = "Personal Loan";
        public const string Origination = "Origination";
        public const string LoanAdjustments = "Loan Adjustments";
        public const string Life = "Life";
        public const string IT = "IT";
        public const string HelpDesk = "Help Desk";
        public const string DebtCounselling = "Debt Counselling";
        public const string CAP2Offers = "CAP2 Offers";
        public const string LifeClaims = "LifeClaims";
    }

    /// <summary>
    /// Workflow States
    /// </summary>
    public class WorkflowStates
    {
        /// <summary>
        /// Constants for the Application Capture Workflow States
        /// </summary>
        public class ApplicationCaptureWF
        {
            public const string AppNotAcceptedHold = "App Not Accepted Hold";
            public const string ApplicationCapture = "Application Capture";
            public const string ApplicationNotAccepted = "Application Not Accepted";
            public const string ApplicationReactivated = "Application Reactivated";
            public const string ApplicationSubmitted = "Application Submitted";
            public const string ArchiveDecline = "Archive Decline";
            public const string ArchiveReminder = "Archive Reminder";
            public const string AssignTeleConsultant = "Assign TeleConsultant";
            public const string AwaitingTimeout = "Awaiting Timeout";
            public const string CheckConsultant = "Check Consultant";
            public const string CheckisEAApplication = "Check is EA Application";
            public const string CheckSourceandLeadType = "Check Source and LeadType";
            public const string ConsultantAssignment = "Consultant Assignment";
            public const string ContactClient = "Contact Client";
            public const string Decline = "Decline";
            public const string EstateAgentApplication = "Estate Agent Application";
            public const string EstateAgentAssign = "Estate Agent Assign";
            public const string EstateAgentLeadAssign = "Estate Agent Lead Assign";
            public const string FollowupHold = "Followup Hold";
            public const string ForwardtolastState = "Forward to last State";
            public const string InternetApplication = "Internet Application";
            public const string InternetLead = "Internet Lead";
            public const string InvalidAppHold = "InvalidAppHold";
            public const string ManageLead = "Manage Lead";
            public const string ManagerReview = "Manager Review";
            public const string ReadytoFollowup = "Ready to Followup";
            public const string UserAssignCheck = "UserAssignCheck";
            public const string CapitecApplications = "Capitec Applications";
            public const string CapitecBranchDeclined = "Capitec Branch Decline";
        }

        /// <summary>
        /// Constants for the Application Management Workflow States
        /// </summary>
        public class ApplicationManagementWF
        {
            public const string AccountCreateFailHold = "Account Create Fail Hold";
            public const string AppCheck = "App Check";
            public const string ApplicationCheck = "Application Check";
            public const string ApplicationHold = "Application Hold";
            public const string ApplicationQuery = "Application Query";
            public const string ArchiveAIP = "Archive AIP";
            public const string ArchiveDecline = "Archive Decline";
            public const string ArchiveDOComplete = "Archive DO Complete";
            public const string ArchiveExpLOA = "Archive Exp LOA";
            public const string ArchiveFL = "Archive FL";
            public const string ArchiveNoAppForm = "Archive No App Form";
            public const string ArchiveNTU = "Archive NTU";
            public const string ArchiveOrigination = "Archive Origination";
            public const string ArchiveProcessor = "Archive Processor";
            public const string ArchiveQAQuery = "Archive QA Query";
            public const string ArchiveQCComplete = "Archive QC Complete";
            public const string ArchiveTranslate = "Archive Translate";
            public const string ArchiveValuationComplete = "Archive Valuation Complete";
            public const string Arrears = "Arrears";
            public const string AssignatQA = "Assign at QA";
            public const string AttorneyCheck = "Attorney Check";
            public const string AwaitingApplication = "Awaiting Application";
            public const string BacktoCredit = "Back to Credit";
            public const string BackToCreditHold = "Back To Credit Hold";
            public const string ChangeDebitOrder = "Change Debit Order";
            public const string CheckQCAndSendToPipeline = "CheckQCAndSendToPipeline";
            public const string ContinueLoan = "ContinueLoan";
            public const string Credit = "Credit";
            public const string CreditHold = "Credit Hold";
            public const string Decline = "Decline";
            public const string DeclineBin = "Decline Bin";
            public const string DeclinedbyCredit = "Declined by Credit";
            public const string Disbursed = "Disbursed";
            public const string Disbursement = "Disbursement";
            public const string DisbursementReview = "Disbursement Review";
            public const string Disputes = "Disputes";
            public const string FollowupComplete = "Followup Complete";
            public const string FollowupHold = "Followup Hold";
            public const string FollowupReturn = "FollowupReturn";
            public const string FurtherInfoRequest = "Further Info Request";
            public const string FurtherValuationRequired = "Further Valuation Required";
            public const string InstructFailed = "Instruct Failed";
            public const string IssueAIP = "Issue AIP";
            public const string LOA = "LOA";
            public const string LOAQuery = "LOA Query";
            public const string ManageApplication = "Manage Application";
            public const string ManualArchive = "Manual Archive";
            public const string MultipleApplications = "Multiple Applications";
            public const string NextStep = "Next Step";
            public const string NTU = "NTU";
            public const string QA = "QA";
            public const string QuickCashHold = "QuickCash Hold";
            public const string RapidHold = "Rapid Hold";
            public const string RapidPostCreditHold = "Rapid Post Credit Hold";
            public const string ReadyToFollowup = "Ready To Followup";
            public const string RegistrationPipeline = "Registration Pipeline";
            public const string RequestatQA = "Request at QA";
            public const string ResubApplicationCheck = "Resub Application Check";
            public const string Resubmission = "Resubmission";
            public const string RetManageApp = "RetManageApp";
            public const string Returntosender = "Return to sender";
            public const string ReturnToSender = "ReturnToSender";
            public const string SendtoFLHold = "Send to FL Hold";
            public const string SignedLOAReview = "Signed LOA Review";
            public const string State95 = "State95";
            public const string State99 = "State99";
            public const string SysFurtherVal = "SysFurtherVal";
            public const string SysReviewVal = "SysReviewVal";
            public const string SystemAssignProcessor = "System Assign Processor";
            public const string SystemValComplete = "System Val Complete";
            public const string TranslateConditions = "Translate Conditions";
            public const string ValuationHold = "Valuation Hold";
            public const string ValuationReviewRequired = "Valuation Review Required";
            public const string CommonArchiveFollowup = "Common Archive Followup";
        }

        /// <summary>
        /// Constants for the CAP 2 Offers Workflow States
        /// </summary>
        public class CAP2OffersWF
        {
            public const string AwaitingForms = "Awaiting Forms";
            public const string AwaitingLA = "Awaiting LA";
            public const string CallbackHold = "Callback Hold";
            public const string CancelledCAP2Offer = "Cancelled CAP2 Offer";
            public const string CAP2Expired = "CAP2 Expired";
            public const string CAP2OfferDeclined = "CAP2 Offer Declined";
            public const string CheckCashPayment = "Check Cash Payment";
            public const string CompletedCAP2Offer = "Completed CAP2 Offer";
            public const string CreditApproval = "Credit Approval";
            public const string ForwardtolastState = "Forward to last State";
            public const string GrantedCAP2Offer = "Granted CAP2 Offer";
            public const string NTUOffer = "NTU Offer";
            public const string OpenCAP2Offer = "Open CAP2 Offer";
            public const string PrepareForCredit = "Prepare For Credit";
            public const string ReadvanceCreditCheck = "Readvance Credit Check";
            public const string ReadvanceRequired = "Readvance Required";
            public const string ReadytoCallback = "Ready to Callback";
        }

        /// <summary>
        /// Constants for the Credit Workflow States
        /// </summary>
        public class CreditWF
        {
            public const string AppCheck = "App Check";
            public const string ArchiveApproveQC = "Archive Approve QC";
            public const string ArchiveApproveResub = "Archive Approve Resub";
            public const string ArchiveAprAndOfferApp = "Archive Apr&Offer App";
            public const string ArchiveDeclineQC = "Archive Decline QC";
            public const string ArchiveDeclinedbyCredit = "Archive Declined by Credit";
            public const string ArchiveDisputes = "Archive Disputes";
            public const string ArchiveFurtherInfo = "Archive Further Info";
            public const string ArchiveProcessor = "Archive Processor";
            public const string ArchiveRandVComplete = "Archive R&V Complete";
            public const string ArchiveResubProcessor = "Archive Resub Processor";
            public const string ArchiveResubRemain = "Archive Resub Remain";
            public const string ArchiveResubmission = "Archive Resubmission";
            public const string AssignCredit = "Assign Credit";
            public const string CheckOutcome = "Check Outcome";
            public const string Credit = "Credit";
            public const string CreditCreateFail = "Credit Create Fail";
            public const string CreditDecisionCheck = "Credit Decision Check";
            public const string PolicyOverride = "Policy Override";
            public const string ReleaseandVariationReview = "Release and Variation Review";
            public const string Review = "Review";
            public const string ReviewCheck = "Review Check";
            public const string ReviewDecision = "Review Decision";
            public const string ReviewQuickCash = "Review Quick Cash";
            public const string State31 = "State31";
            public const string ValuationApprovalRequired = "Valuation Approval Required";
        }

        /// <summary>
        /// Constants for the Life Origination Workflow States
        /// </summary>
        public class LifeOriginationWF
        {
            public const string AwaitingConfirmation = "Awaiting Confirmation";
            public const string AwaitingTimeout = "Awaiting Timeout";
            public const string Benefits = "Benefits";
            public const string CallbackHold = "Callback Hold";
            public const string ChildClosed = "Child Closed";
            public const string ContactClient = "Contact Client";
            public const string Declaration = "Declaration";
            public const string DeclarationConfirmation = "Declaration Confirmation";
            public const string Exclusions = "Exclusions";
            public const string ExclusionsConfirmation = "Exclusions Confirmation";
            public const string FAIS = "FAIS";
            public const string FAISConfirmation = "FAIS Confirmation";
            public const string FAISConfirmationCheck = "FAIS Confirmation Check";
            public const string ForwardtocorrectState = "Forward to correct State";
            public const string ForwardtolastState = "Forward to last State";
            public const string LOA = "LOA";
            public const string LOAQuery = "LOA Query";
            public const string NTUHold = "NTU Hold";
            public const string NTUReactivated = "NTU Reactivated";
            public const string Olderthan45Days = "Older than 45 Days";
            public const string ParentClosed = "Parent Closed";
            public const string PolicyActivated = "Policy Activated";
            public const string PolicyNTUd = "Policy NTUd";
            public const string Quote = "Quote";
            public const string ReadytoCallback = "Ready to Callback";
            public const string ReadytoSend = "Ready to Send";
            public const string RPAR = "RPAR";
        }

        /// <summary>
        /// Constants for the Readvance Payments Workflow States
        /// </summary>
        public class ReadvancePaymentsWF
        {
            public const string Archive = "Archive";
            public const string ArchiveDecline = "Archive Decline";
            public const string ArchiveFurtherLoan = "Archive Further Loan";
            public const string ArchiveNTU = "Archive NTU";
            public const string ArchiveProcessor = "Archive Processor";
            public const string AwaitingSchedule = "Awaiting Schedule";
            public const string ContactClient = "Contact Client";
            public const string Decline = "Decline";
            public const string DeedofSuretyInstruction = "Deed of Surety Instruction";
            public const string DisburseFunds = "Disburse Funds";
            public const string Disbursed = "Disbursed";
            public const string FolderArchive = "Folder Archive";
            public const string FollowupComplete = "Followup Complete";
            public const string FollowupHold = "Followup Hold";
            public const string FollowupReturn = "FollowupReturn";
            public const string NextStep = "Next Step";
            public const string NTU = "NTU";
            public const string RapidDecision = "Rapid Decision";
            public const string ReadvanceCreateFail = "Readvance Create Fail";
            public const string ReadyToFollowup = "Ready To Followup";
            public const string ReturnState = "Return State";
            public const string Returntosender = "Return to sender";
            public const string SendSchedule = "Send Schedule";
            public const string SetupPayment = "Setup Payment";
            public const string State29 = "State29";
            public const string RequireSuperLoOptOut = "Require SuperLo Opt-Out";
        }

        /// <summary>
        /// Constants for the Loan Adjustments Workflow States
        /// </summary>
        public class LoanAdjustmentsWF
        {
            public const string ApplicationAdjustment = "Application Adjustment";
            public const string ArchiveNoLongerRequired = "Archive No Longer Required";
            public const string ArchiveTermChange = "Archive Term Change";
            public const string RequestFailed = "Request Failed";
            public const string TermChangeRequest = "Term Change Request";
            public const string ReviewDecision = "Review Decision";
            public const string ArchiveAgreeTermChange = "Archive Agree Term Change";
        }

        /// <summary>
        /// Constants for the Valuations Workflow States
        /// </summary>
        public class ValuationsWF
        {
            public const string ArchiveReturnedAssessment = "Archive Returned Assessment";
            public const string ArchiveValuationClone = "Archive Valuation Clone";
            public const string ArchiveValuationComplete = "Archive Valuation Complete";
            public const string AssessInstructionCheck = "Assess Instruction Check";
            public const string FurtherValuationRequest = "Further Valuation Request";
            public const string ManagerArchive = "Manager Archive";
            public const string ManagerReview = "Manager Review";
            public const string ManualArchive = "Manual Archive";
            public const string MigratedValuationAssessment = "Migrated Valuation Assessment";
            public const string ParentCaseArchive = "ParentCaseArchive";
            public const string PendingAssessmentStatus = "Pending Assessment Status";
            public const string ResubValuationComplete = "Resub Valuation Complete";
            public const string ScheduleValuationAssessment = "Schedule Valuation Assessment";
            public const string ValCreateFail = "Val Create Fail";
            public const string ValuationAssessmentPending = "Valuation Assessment Pending ";
            public const string ValuationComplete = "Valuation Complete";
            public const string ValuationReviewRequest = "Valuation Review Request";
            public const string ValuationStatusChange2AM = "Valuation Status Change 2AM";
            public const string ValutionAssessmentReturned = "Valution Assessment Returned";
            public const string WithdrawValuationAssessment = "Withdraw Valuation Assessment";
            public const string WithdrawalInstructionCheck = "Withdrawal Instruction Check";
            public const string SysFurtherVal = "SysFurtherVal";
            public const string SysReviewVal = "SysReviewVal";
        }

        /// <summary>
        /// Debt Counselling Workflow States
        /// </summary>
        public class DebtCounsellingWF
        {
            public const string ReviewNotification = "Review Notification";
            public const string DecisiononProposal = "Decision on Proposal";
            public const string Pend1stPayment = "Pend 1st Payment";
            public const string Termination = "Termination ";
            public const string PendCourtDecision = "Pend Court Decision";
            public const string ArchiveDebtCounselling = "Archive Debt Counselling";
            public const string PendProposal = "Pend Proposal";
            public const string PersonalLoanPendProposal = "Personal Loan Pend Proposal";
            public const string DebtReviewApproved = "Debt Review Approved";
            public const string DefaultinPayment = "Default in Payment";
            public const string TerminationAdviceSent = "Termination Advice Sent";
            public const string TermReview = "Term Review";
            public const string ManageProposal = "Manage Proposal";
            public const string Litigation = "Litigation";
            public const string ArchiveLitigation = "Archive Litigation";
            public const string PendCancellation = "Pend Cancellation";
            public const string AcceptedProposal = "Accepted Proposal";
            public const string TerminationLetterSent = "Termination Letter Sent";
            public const string Sixtydayarchive = "60day archive";
            public const string CheckNoDateNoPay = "Check No Date No Pay";
            public const string ReturnToPreviousState = "Return To Previous State";
            public const string FortyFiveDayReminder = "45 Day Reminder";
            public const string _60DayTimerHold = "60DayTimerHold";
            public const string IntenttoTerminate = "Intent to Terminate";
            public const string Archive60days = "Archive 60days";
            public const string _60daysReview = "60 Days Review";
            public const string _45DayReminder = "45 Day Reminder";
            public const string RecoveriesProposalDecision = "Recoveries Proposal Decision";
            public const string PendPayment = "Pend Payment";
            public const string PaymentReview = "Payment Review";
            public const string EstatesandSequestration = "Estates and Sequestration";
            public const string BondExclusions = "Bond Exclusions";
            public const string BondExclusionsArrears = "Bond Exclusions Arrears";
            public const string RecoveriesProposal = "Recoveries Proposal";
            public const string SendLoanAgreements = "Send Loan Agreements";
            public const string TerminationArchive = "Termination Archive";
        }

        public class PersonalLoansWF
        {
            public const string ArchiveDisbursed = "Archive Disbursed";
            public const string ArchiveNTUDeclines = "Archive NTU Declines";
            public const string Credit = "Credit";
            public const string DeclinedbyCredit = "Declined by Credit";
            public const string Disbursement = "Disbursement";
            public const string LegalAgreements = "Legal Agreements";
            public const string ManageApplication = "Manage Application";
            public const string VerifyDocuments = "Verify Documents";
            public const string ManageLead = "Manage Lead";
            public const string NTU = "NTU";
            public const string ReturntoState = "Return to State";
            public const string Disbursed = "Disbursed";
            public const string SendDisbursementLetter = "Send Disbursement Letter";
            public static string ArchiveNTULead = "Archive NTU Lead";
            public const string DocumentCheck = "Document Check";
        }

        public class DisabilityClaimWF
        {
            public const string ClaimDetails = "Claim Details";
            public const string AssessClaim = "Assess Claim";
            public const string SendApprovalLetter = "Send Approval Letter";
            public const string ApprovedClaim = "Approved Claim";
            public const string ArchiveTerminated = "Archive Terminated";
            public const string ArchiveSettled = "Archive Settled";
            public const string ArchiveRepudiated = "Archive Repudiated";
        }
    }

    /// <summary>
    /// Scheduled Actvities
    /// </summary>
    public class ScheduledActivities
    {
        /// <summary>
        /// Scheduled Activity names for Application Capture
        /// </summary>
        public class ApplicationCapture
        {
            public const string _15DayAutoReminder = "15 Day Auto Reminder";
            public const string _45daytimer = "45 day timer";
            public const string ArchiveApplication = "Archive Application";
            public const string CreateClone = "Create Clone";
            public const string DeclineTimeout = "Decline Timeout";
            public const string WaitforFollowup = "Wait for Followup";
        }

        /// <summary>
        /// Scheduled Activity names for Application Management
        /// </summary>
        public class ApplicationManagement
        {
            public const string _1monthtimer = "1 month timer";
            public const string _2Months = "2 Months";
            public const string _30Days = "30 Days";
            public const string _30DaysTimer = "30 Day Timer";
            public const string _45days = "45 days";
            public const string _90DayTimer = "90 Day Timer";
            public const string ArchiveCompletedFollowup = "Archive Completed Followup";
            public const string DeclineByCreditTimer = "Decline By Credit Timer";
            public const string DeclineTimeout = "Decline Timeout";
            public const string DisbursementTimer = "Disbursement Timer";
            public const string NTUTimeout = "NTU Timeout";
            public const string OnFollowup = "OnFollowup";
        }

        /// <summary>
        /// Scheduled Activity names for CAP 2 Offers
        /// </summary>
        public class CAP2Offers
        {
            public const string CompletedExpired = "Completed Expired";
            public const string Declined = "Declined";
            public const string NTUOffer = "NTU Offer";
            public const string OfferExpired = "Offer Expired";
            public const string WaitforCallback = "Wait for Callback";
        }

        /// <summary>
        /// Scheduled Activity names for Readvance Payments
        /// </summary>
        public class ReadvancePayments
        {
            public const string _12hrs = "12hrs";
            public const string ArchiveCompleteFollowup = "Archive Complete Followup";
            public const string DeclineTimeout = "Decline Timeout";
            public const string NTUTimeout = "NTU Timeout";
            public const string OnFollowup = "OnFollowup";
        }

        /// <summary>
        /// Scheduled Activity names for Life Origination
        /// </summary>
        public class LifeOrigination
        {
            public const string _45DayTimeout = "45 Day Timeout";
            public const string ArchiveNTU = "Archive NTU";
            public const string AwaitingConfirmationTimout = "Awaiting Confirmation Timout";
            public const string CreateClone = "Create Clone";
            public const string WaitforCallback = "Wait for Callback";
            public const string CreateInstance = "Create Instance";
        }

        /// <summary>
        /// Scheduled Activity names for Valuations
        /// </summary>
        public class Valuations
        {
            public const string AssessmentOverdue = "Assessment Overdue";
            public const string Check2AMValuation15min = "Check 2AM Valuation 15min";
            public const string EscalateToManager = "Escalate to Manager";
            public const string ManagerArchive = "Manager Archive";
            public const string ValuationinOrder = "Valuation in Order";
            public const string RenstructValuer = "Re-instruct Valuer";
            public const string RequestValuationReview = "Request Valuation Review";
            public const string ReviewValuationRequired = "Review Valuation Required";
            public const string FurtherValuationRequired = "Further Valuation Required";
            public const string InstructEzValValuer = "Instruct Ez-Val Valuer";
        }

        /// <summary>
        /// Scheduled Activity names for Loan Adjustments
        /// </summary>
        public class LoanAdjustments
        {
            public const string TermRequestTimeout = "Term Request Timeout";
        }

        /// <summary>
        /// Debt Counselling Scheduled Activities
        /// </summary>
        public class DebtCounselling
        {
            public const string Tendays = "10 days";
            public const string Fivedays = "5days";
            public const string Termexpired = "Term expired";
            public const string _45Days = "45 Days";
            public const string _60days = "60 days";
            public const string CaptureRecoveriesProposal = "Capture Recoveries Proposal";
        }

        public class PersonalLoans
        {
            public const string NTUTimer = "NTU Timer";
            public const string DisbursedTimer = "Disbursed Timer";
            public const string EmailDisbursedLetter = "Email Disbursed Letter";
        }
    }

    /// <summary>
    /// Reason Types
    /// </summary>
    public class ReasonType
    {
        public const string AdministrativeDecline = "Administrative Decline";
        public const string ApplicationNTU = "Application NTU";
        public const string BranchDecline = "Branch Decline";
        public const string CapCancellation = "Cap Cancellation";
        public const string ChangeFinancialInstitution = "Change Financial Institution";
        public const string CreditDecline = "Credit Decline";
        public const string CreditScoringDecline = "Credit Scoring Decline";
        public const string CreditScoringQuery = "Credit Scoring Query";
        public const string DebtConsolidation = "Debt Consolidation";
        public const string HomeRenovation = "Home Renovation";
        public const string InvestmentProperty = "Investment Property";
        public const string LeadNTU = "Lead NTU";
        public const string LifeCallback = "Life Callback";
        public const string LifeNTU = "Life NTU";
        public const string LifeProductSwitch = "Life Product Switch";
        public const string LifePolicyCancellation = "LifePolicy Cancellation";
        public const string MovableAssetsFinance = "Movable Assets Finance";
        public const string NewHomePurchase = "New Home Purchase";
        public const string OriginationNTU = "Origination NTU";
        public const string PipelineNTU = "Pipeline NTU";
        public const string ProcessorQuery = "Processor Query";
        public const string QAComplete = "QA Complete";
        public const string QAQuery = "QA Query";
        public const string QuickCashDecline = "Quick Cash Decline";
        public const string QuickCashNTU = "Quick Cash NTU";
        public const string Resubmission = "Resubmission";
        public const string RevisionCreditDecline = "Revision Credit Decline";
        public const string RevisionInitial = "Revision Initial";
        public const string RevisionReSubmission = "Revision ReSubmission";
        public const string Initiator = "Initiator";
        public const string LitigationReallocation = "Litigation Reallocation";
        public const string DebtCounsellingTermination = "Debt Counselling Termination";
        public const string ProposalAccepted = "Proposal Accepted";
        public const string ProposalDeclined = "Proposal Declined";
        public const string DebtCounsellingCancelled = "Debt Counselling Cancelled";
        public const string CounterProposals = "Counter Proposals";
        public const string DebtCounsellingNotification = "Debt Counselling Notification";
        public const string ConsultantDeclined = "Consultant Declined";
        public const string PersonalLoanDecline = "Personal Loan Decline";
        public const string PersonalLoanNTU = "NTU Personal Loan";
        public const string PersonalLoanAdminDecline = "Personal Loan Admin Decline";
        public const string CapitecBranchDecline = "Capitec Branch Decline";
        public const string ContactWithClient = "Contact with Client";
    }

    /// <summary>
    /// Language
    /// </summary>
    public class Language
    {
        public const string English = "English";
        public const string Afrikaans = "Afrikaans";
    }

    /// <summary>
    /// Citizenship Type
    /// </summary>
    public class CitizenType
    {
        public const string SACitizen = "SA Citizen";
        public const string SACitizenNonResident = "SA Citizen – Non Resident";
        public const string Foreigner = "Foreigner";
        public const string NonResident = "Non - Resident";
        public const string NonResidentRefugee = "Non - Resident – Refugee";
        public const string NonResidentConsulate = "Non - Resident – Consulate";
        public const string NonResidentDiplomat = "Non - Resident – Diplomat";
        public const string NonResidentHighCommissioner = "Non - Resident – High Commissioner";
        public const string NonResidentCMAResidentCitizen = "Non - Resident – CMA Resident/Citizen";
        public const string NonResidentContractWorker = "Non - Resident – Contract Worker";
    }

    /// <summary>
    /// Population Group
    /// </summary>
    public class PopulationGroup
    {
        public const string Unknown = "Unknown";
        public const string White = "White";
        public const string African = "African";
        public const string Coloured = "Coloured";
        public const string Asian = "Asian";
    }

    /// <summary>
    /// Education
    /// </summary>
    public class Education
    {
        public const string Unknown = "Unknown";
        public const string Matric = "Matric";
        public const string UniversityDegree = "University Degree";
        public const string Diploma = "Diploma";
        public const string Other = "Other";
    }

    /// <summary>
    /// Education
    /// </summary>
    public class Exclusions
    {
        public const string FLAutomation = "FLAutomation";
        public const string OrginationAutomation = "OrginationAutomation";
    }

    /// <summary>
    /// Marital Status
    /// </summary>
    public class MaritalStatus
    {
        public const string Single = "Single";
        public const string MarriedCommunityofProperty = "Married - Community of Property";
        public const string MarriedAnteNuptualContract = "Married - Ante Nuptual Contract";
        public const string Divorced = "Divorced";
        public const string Widowed = "Widowed";
        public const string MarriedForeign = "Married – Foreign";
    }

    /// <summary>
    /// Offer Role Types
    /// </summary>
    public class OfferRoleTypes
    {
        public const string LeadMainApplicant = "Lead - Main Applicant";
        public const string LeadSuretor = "Lead - Suretor";
        public const string MainApplicant = "Main Applicant";
        public const string Suretor = "Suretor";
        public const string AssuredLife = "Assured Life";
        public const string CreditUnderwriterD = "Credit Underwriter D";
        public const string CreditSupervisorD = "Credit Supervisor D";
        public const string BranchConsultantD = "Branch Consultant D";
        public const string RegistrationsLOAAdmin = "Registrations LOA Admin";
        public const string RegistrationsAdmin = "Registrations Administrator";
        public const string RegistrationsManager = "Registrations Manager";
        public const string RegistrationsSupervisor = "Registrations Supervisor";
        public const string ValuationsAdministrator = "Valuations Administrator D";
        public const string ValuationsManager = "Valuations Manager D";
        public const string NewBusinessProcessor = "New Business Processor D";
        public const string FurtherLendingProcessor = "FL Processor D";
    }

    /// <summary>
    /// Test users
    /// </summary>
    public class TestUsers
    {
        public const string BranchAdmin = @"SAHL\BAUser";
        public const string BranchAdmin1 = @"SAHL\BAUser1";
        public const string BranchAdmin2 = @"SAHL\BAUser2";
        public const string BranchAdmin3 = @"SAHL\BAUser3";
        public const string BranchAdmin4 = @"SAHL\BAUser4";
        public const string BranchConsultant = @"SAHL\BCUser";
        public const string BranchConsultant1 = @"SAHL\BCUser1";
        public const string BranchConsultant2 = @"SAHL\BCUser2";
        public const string BranchConsultant3 = @"SAHL\BCUser3";
        public const string BranchConsultant4 = @"SAHL\BCUser4";
        public const string BranchConsultant5 = @"SAHL\BCUser5";
        public const string BranchConsultant6 = @"SAHL\BCUser6";
        public const string BranchConsultant7 = @"SAHL\BCUser7";
        public const string BranchConsultant8 = @"SAHL\BCUser8";
        public const string BranchConsultant9 = @"SAHL\BCUser9";
        public const string BranchConsultant10 = @"SAHL\BCUser10";
        public const string BranchManager = @"SAHL\BMUser";
        public const string BranchManager1 = @"SAHL\BMUser1";
        public const string CollectAdminUser = @"SAHL\CollectAdminUser";
        public const string CreditUnderwriter = @"SAHL\CUUser";
        public const string CreditUnderwriter2 = @"SAHL\CUUser2";
        public const string CreditUnderwriter3 = @"SAHL\CUUser3";
        public const string CreditUnderwriter4 = @"SAHL\CUUser4";
        public const string CreditUnderwriter5 = @"SAHL\CUUser5";
        public const string CreditUnderwriter6 = @"SAHL\CUUser6";
        public const string ClintonS = @"SAHL\ClintonS";
        public const string CreditSupervisor = @"SAHL\CSUser";
        public const string CreditSupervisor2 = @"SAHL\CSUser2";
        public const string CreditSupervisor3 = @"SAHL\CSUser3";
        public const string CreditManager = @"SAHL\CMUser";
        public const string CreditManager1 = @"SAHL\CMUser1";
        public const string CreditExceptions = @"SAHL\CEUser";
        public const string DebtCounsellingSupervisor = @"SAHL\DCSUser";
        public const string DebtCounsellingSupervisor1 = @"SAHL\DCSUser1";
        public const string DebtCounsellingAdmin1 = @"SAHL\DCAUser1";
        public const string DebtCounsellingAdmin = @"SAHL\DCAUser";
        public const string DebtCounsellingManager = @"SAHL\DCMUser";
        public const string DebtCounsellingCourtConsultant = @"SAHL\DCCCUser";
        public const string DebtCounsellingConsultant = @"SAHL\DCCUser";
        public const string EstateAgentConsultant = @"SAHL\EACUser";
        public const string EstateAgentConsultant1 = @"sahl\EACUser1";
        public const string FLProcessor = @"SAHL\FLAppProcUser";
        public const string FLProcessor1 = @"SAHL\FLAppProcUser1";
        public const string FLProcessor2 = @"SAHL\FLAppProcUser2";
        public const string FLProcessor3 = @"SAHL\FLAppProcUser3";
        public const string FLProcessor4 = @"SAHL\FLAppProcUser4";
        public const string FLProcessor5 = @"SAHL\FLAppProcUser5";
        public const string FLProcessor6 = @"SAHL\FLAppProcUser6";
        public const string FLProcessor7 = @"SAHL\FLAppProcUser7";
        public const string FLProcessor8 = @"SAHL\FLAppProcUser8";
        public const string FLProcessor9 = @"SAHL\FLAppProcUser9";
        public const string FLProcessor10 = @"SAHL\FLAppProcUser10";
        public const string FLSupervisor = @"SAHL\FLSUser";
        public const string FLSupervisor1 = @"SAHL\FLSUser1";
        public const string FLSupervisor2 = @"SAHL\FLSUser2";
        public const string FLManager = @"SAHL\FLMUser";
        public const string ForeclosureConsultant = @"SAHL\LCLCUser";
        public const string ForeclosureConsultant1 = @"SAHL\LCLCUser1";
        public const string ForeclosureConsultant2 = @"SAHL\LCLCUser2";
        public const string ForeclosureConsultant3 = @"SAHL\LCLCUser3";
        public const string ForeclosureConsultant4 = @"SAHL\LCLCUser4";
        public const string HaloUser = @"SAHL\HaloUser";
        public const string HelpdeskAdminUser = @"SAHL\HAUser";
        public const string HelpdeskAdminUser1 = @"SAHL\HAUser1";
        public const string HelpdeskAdminUser3 = @"SAHL\HAUser3";
        public const string HelpdeskAdminUser4 = @"SAHL\HAUser4";
        public const string HelpdeskAdminUser5 = @"SAHL\HAUser5";
        public const string HelpdeskAdminUser6 = @"SAHL\HAUser6";
        public const string HelpdeskAdminUser7 = @"SAHL\HAUser7";
        public const string HelpdeskAdminUser8 = @"SAHL\HAUser8";
        public const string HelpdeskAdminUser9 = @"SAHL\HAUser9";
        public const string HelpdeskManager = @"SAHL\HMUser";
        public const string HelpdeskSupervisor = @"SAHL\HSUser";
        public const string LifeAdmin = @"SAHL\LAUser";
        public const string LifeClaimsAssessor = @"SAHL\LCAUser1";
        public const string LifeConsultant = @"SAHL\LCUser";
        public const string LifeManager = @"SAHL\LMUser";
        public const string LossControlLitigationBackup = @"SAHL\LCLCBUser";
        public const string NewBusinessProcessor = @"SAHL\NBPUser";
        public const string NewBusinessProcessor1 = @"SAHL\NBPUser1";
        public const string NewBusinessProcessor2 = @"SAHL\NBPUser2";
        public const string NewBusinessProcessor3 = @"SAHL\NBPUser3";
        public const string NewBusinessProcessor4 = @"SAHL\NBPUser4";
        public const string NewBusinessProcessor5 = @"SAHL\NBPUser5";
        public const string NewBusinessProcessor6 = @"SAHL\NBPUser6";
        public const string NewBusinessProcessor7 = @"SAHL\NBPUser7";
        public const string NewBusinessProcessor8 = @"SAHL\NBPUser8";
        public const string NewBusinessProcessor9 = @"SAHL\NBPUser9";
        public const string NewBusinessManager = @"SAHL\NBMUser";
        public const string Password_TristanZ = "Carni345";
        public const string Password = "Natal123";
        public const string Password_ClintonS = "Secret02";
        public const string PersonalLoanConsultant1 = @"SAHL\PLCUser1";
        public const string PersonalLoanConsultant2 = @"SAHL\PLCUser2";
        public const string PersonalLoanConsultant3 = @"SAHL\PLCUser3";
        public const string PersonalLoanConsultant4 = @"SAHL\PLCUser4";
        public const string PersonalLoanCreditAnalyst1 = @"SAHL\PLCAUser1";
        public const string PersonalLoanCreditAnalyst2 = @"SAHL\PLCAUser2";
        public const string PersonalLoanCreditAnalyst3 = @"SAHL\PLCAUser3";
        public const string PersonalLoanCreditAnalyst4 = @"SAHL\PLCAUser4";
        public const string PersonalLoanSupervisor1 = @"SAHL\PLSUser1";
        public const string PersonalLoanSupervisor2 = @"SAHL\PLSUser2";
        public const string PersonalLoanManager = @"SAHL\PLMUser";
        public const string PersonalLoanAdmin1 = @"SAHL\PLAdmin1";
        public const string PersonalLoanAdmin2 = @"SAHL\PLAdmin2";
        public const string PersonalLoanAdmin3 = @"SAHL\PLAdmin3";
        public const string PersonalLoanAdminGI = @"SAHL\GIAdmin";
        public const string PersonalLoanAdminCB = @"SAHL\CBAdmin";
        public const string PersonalLoanAdminJM = @"SAHL\JMAdmin";
        public const string PersonalLoanClientServiceUser = @"SAHL\PLCSUser1";
        public const string RegAdminUser = @"SAHL\RAUser";
        public const string RegistrationsManager = @"SAHL\RegMUser";
        public const string RegistrationsSupervisor = @"SAHL\RSUser";
        public const string RegistrationsLOAAdmin = @"SAHL\RLOAAUser";
        public const string ResubmissionAdmin = @"SAHL\ResubUser";
        public const string ReleaseVariationsAdmin = @"SAHL\RVAUser";
        public const string ReleaseVariationsAdmin1 = @"SAHL\RVAUser1";
        public const string ReleaseVariationsManager = @"SAHL\RVMUser";
        public const string TristanZ = @"SAHL\TristanZ";

        public const string TELAUser = @"SAHL\TELAUser";
        public const string TELCUser = @"SAHL\TELCUser";
        public const string TELCUser1 = @"SAHL\TELCUser1";
        public const string TELCUser2 = @"SAHL\TELCUser2";
        public const string TELCUser3 = @"SAHL\TELCUser3";
        public const string TELMUser = @"SAHL\TELMUser";
        public static string[] TeleTestUsers = new string[] { TELAUser, TELCUser, TELCUser1, TELCUser2, TELCUser3, TELMUser };

        public const string ValuationsProcessor = @"SAHL\VPUser";
        public const string ValuationsProcessor1 = @"SAHL\VPUser1";
        public const string ValuationsProcessor2 = @"SAHL\VPUser2";
        public const string ValuationsManager = @"SAHL\VMUser";
        public const string ValuationsManager1 = @"SAHL\VMUser1";
        public const string NewBusinessSupervisor = @"SAHL\NBSUser";
        public const string PersonalLoansClientServiceManager = @"SAHL\PLCSMUser1";
        public const string PersonalLoansClientServiceSupervisor = @"SAHL\PLCSSUser1";
        public const string PersonalLoansCreditExceptionsManager1 = @"SAHL\PLCEUser1";
        public const string MarchuanV = @"SAHL\Marchuanv";
    }

    /// <summary>
    /// CAP offer types
    /// </summary>
    public class CapTypes
    {
        public const string OnePerc = "1% Above Current Rate";
        public const string TwoPerc = "2% Above Current Rate";
        public const string ThreePerc = "3% Above Current Rate";
    }

    /// <summary>
    /// Deeds offices
    /// </summary>
    public class DeedsOffice
    {
        public const string Bloemfontein = "Bloemfontein";
        public const string CapeTown = "Cape Town";
        public const string Johannesburg = "Johannesburg";
        public const string Kimberley = "Kimberley";
        public const string KingWilliamsTown = "King Williams Town";
        public const string Pietermaritzburg = "Pietermaritzburg";
        public const string Pretoria = "Pretoria";
        public const string Vryberg = "Vryberg";
        public const string Mpumalanga = "Mpumalanga";
        public const string Umtata = "Umtata";
    }

    /// <summary>
    /// Common Config Setting Variables
    /// </summary>
    public class ConfigValue
    {
        public const string HaloWebServiceURL = "HaloWebServiceURL";
        public const string BrowserVisability = "BrowserVisability";
        public const string TestWebsite = "TestWebsite";
        public const string SAHLDataBaseServer = "SAHLDataBaseServer";
        public const string WaitForCompleteTimeOut = "WaitForCompleteTimeOut";
        public const string AttachToIETimeOut = "AttachToIETimeOut";
        public const string WaitUntilExistsTimeOut = "WaitUntilExistsTimeOut";
    }

    /// <summary>
    /// Asset & Liability types
    /// </summary>
    public class AssetLiabilityType
    {
        public const string FixedProperty = "Fixed Property";
        public const string ListedInvestments = "Listed Investments";
        public const string UnlistedInvestments = "Unlisted Investments";
        public const string OtherAsset = "Other Asset";
        public const string LifeAssurance = "Life Assurance";
        public const string LiabilityLoan = "Liability Loan";
        public const string LiabilitySurety = "Liability Surety";
        public const string FixedLongTermInvestment = "Fixed Long Term Investment";
    }

    /// <summary>
    /// Employment Type constants
    /// </summary>
    public class EmploymentType
    {
        public const string Salaried = "Salaried";
        public const string SelfEmployed = "Self Employed";
        public const string SalariedWithDeductions = "Salaried with Deduction";
        public const string Unemployed = "Unemployed";
        public const string Unknown = "Unknown";
    }

    /// <summary>
    /// Remuneration Type constants
    /// </summary>
    public class RemunerationType
    {
        public const string Unknown = "Unknown";
        public const string Salaried = "Salaried";
        public const string HourlyRate = "Hourly Rate";
        public const string BasicPlusCommission = "Basic + Commission";
        public const string CommissionOnly = "Commission Only";
        public const string RentalIncome = "Rental Income";
        public const string InvestmentIncome = "Investment Income";
        public const string Drawings = "Drawings";
        public const string Pension = "Pension";
        public const string Maintenance = "Maintenance";
        public const string BusinessProfits = "Business Profits";
    }

    /// <summary>
    /// Columns of the Account Table
    /// </summary>
    public class AccountTable
    {
        public const string AccountKey = "AccountKey";
        public const string FixedPayment = "FixedPayment";
        public const string AccountStatusKey = "AccountStatusKey";
        public const string InsertedDate = "InsertedDate";
        public const string OriginationSourceProductKey = "OriginationSourceProductKey";
        public const string OpenDate = "OpenDate";
        public const string CloseDate = "CloseDate";
        public const string RRR_ProductKey = "RRR_ProductKey";
        public const string RRR_OriginationSourceKey = "RRR_OriginationSourceKey";
        public const string UserID = "UserID";
        public const string ChangeDate = "ChangeDate";
    }

    /// <summary>
    ///
    /// </summary>
    public class MemoTable
    {
        public const string MemoKey = "MemoKey";
        public const string GenericKeyTypeKey = "GenericKeyTypeKey";
        public const string GenericKey = "GenericKey";
        public const string InsertedDate = "InsertedDate";
        public const string Memo = "Memo";
        public const string ADUserKey = "ADUserKey";
        public const string ChangeDate = "ChangeDate";
        public const string GeneralStatusKey = "GeneralStatusKey";
        public const string ReminderDate = "ReminderDate";
        public const string ExpiryDate = "ExpiryDate";
    }

    public class X2Data
    {
        public const string Application_Capture = "Application_Capture";
        public const string Application_Management = "Application_Management";
    }

    public class Features
    {
        public const string UserStatusMaintenance = "User Status Maintenance";
    }

    /// <summary>
    /// Reports that are being set via the Correspondence Processing functionality
    /// </summary>
    public class CorrespondenceReports
    {
        public const string CAPLetter = "CAP Letter";
        public const string LegalAgreementStandardVariable = "Legal Agreement - Standard Variable";
        public const string AttorneyFurtherLoanInstruction = "Attorney Further Loan Instruction";
        public const string ApprovalInPrinciple = "Approval In Principle";
        public const string LetterOfAcceptance = "Letter Of Acceptance";
        public const string AttorneyInstruction = "Attorney Instruction";
        public const string DebtCounsellingTerminationLetter = "Debt Counselling Termination Letter";
        public const string IntentionToTerminate = "Intention To Terminate";
        public const string DebtCounsellingCounterProposalLetter = "Debt Counselling Counter Proposal Letter";
        public const string CounterProposalDetail = "Counter Proposal Detail";
        public const string AttorneytoOpposeLetter = "Attorney to Oppose Letter";
        public const string DebtCounsellingDeclineLetter = "Debt Counselling Decline Letter";
        public const string _171ResponseLetter = "17.1 Response Letter";
        public const string HelpDeskSuperLoIntroductionLetter = "HelpDesk Super Lo Introduction Letter";
        public const string LoanStatement = "Loan Statement";
        public const string BondCancellationLetter = "Bond Cancellation Letter";
        public const string LegalInstructionForeclosure = "Legal Instruction - Foreclosure";
        public const string LegalInstructionLetterOfDemand = "Legal Instruction - LetterOfDemand";
        public const string Form27 = "Form 27";
        public const string MS65 = "MS65";
        public const string HOCCoverLetter = "HOC Cover Letter";
        public const string PolicySchedule = "?";
        public const string HOCEndorsement = "HOC Endorsement Letter";
        public const string HOCCancellationLetter = "HOC Cancellation Letter";
        public const string EndorsementLetter = "Endorsement Letter";
        public const string InstalmentLetter = "Letter - Instalment";
        public const string AcknowledgeCancellationInstruction = "Cancellation Letter";
        public const string IncomeTaxStatement = "Income Tax Statement";
        public const string Z299 = "Z299";
        public const string InsolvenciesPowerofAttorney = "Insolvencies - Power of Attorney";
        public const string InsolvenciesStatementofAccount = "Insolvency - Statement of Account";
        public const string InsolvenciesClaimAffidavit = "Insolvencies - Claim Affidavit";
        public const string PersonalLoanOffer = "Personal Loans Offer";
        public const string PersonalLoanLegalAgreement = "Personal Loans Legal Agreements";
        public const string DisbursementLetter = "Disbursement Letter";
        public const string PersonalLoanChangeTermExtensionLetter = "Personal Loan Term Extension Letter";
        public const string CapitalisationLetter = "Capitalisation Letter";
        public const string CreditLifePolicyInstatementLetter = "Credit Life Policy Instatement Letter";
        public const string HOCForm23Letter = "HOC Form 23 Letter";
        public const string DomiciliumElectionForm = "Domicilium Election Form";
        public const string NaedoDebitOrderAuthorization = "Naedo Debit Order Authorization";
        public const string ConfirmationofEnquiry = "Confirmation of Enquiry";
        public const string FurtherAdvanceAndFurtherLoan = "Further Advance & Further Loan";
        public const string RapidReAdvance = "Rapid ReAdvance";
        public const string DisabilityClaimApprovalLetter = "Disability Claim Approval Letter";
    }

    /// <summary>
    /// Correspondence Mediums
    /// </summary>
    public class CorrespondenceMedium
    {
        public const string Post = "Post";
        public const string Email = "Email";
        public const string Fax = "Fax";
        public const string SMS = "SMS";
    }

    /// <summary>
    /// Ework Stages
    /// </summary>
    public class EworkStages
    {
        public const string ArrearCases = "Arrear Cases";
        public const string UpForFees = "Up for Fees";
        public const string Assign = "Assign";
        public const string Allocation = "Allocation";
        public const string PermissionToRegister = "Permission to Register";
        public const string PrepFile = "Prep File";
        public const string LodgeDocuments = "Lodge Documents";
        public const string NTUReview = "NTU Review";
        public const string AssignAttorney = "Assign Attorney";
        public const string DebtCounselling = "Debt Counselling";
        public const string DebtCounselling_Arrears = "Debt Counselling (Arrears)";
        public const string Collections = "Collections";
        public const string DebtCounselling_Collections = "Debt Counselling (Collections)";
        public const string DebtCounselling_Estates = "Debt Counselling (Estates)";
        public const string DebtCounselling_Facilitation = "Debt Counselling (Facilitation)";
        public const string DebtCounselling_Seq = "Debt Counselling (Seq)";
        public const string Recoveries = "Recoveries";
        public const string AllOver = "All Over";
        public const string RefusalNoted = "Refusal Noted";
        public const string PendLitigation = "Pend Litigation";
    }

    /// <summary>
    /// Ework Maps
    /// </summary>
    public class EworkMaps
    {
        public const string Pipeline = "Pipeline";
        public const string LossControl = "LossControl";
        public const string DebtCounselling = "Debt Counselling";
        public const string LegalAction = "Legal Action";
    }

    /// <summary>
    /// Ework Actions
    /// </summary>
    public class EworkActions
    {
        public const string X2NTUAdvise = "X2 NTU ADVISE";
        public const string X2ReturnDebtCounselling = "X2 Return Debt Counselling";
    }

    /// <summary>
    /// Workflow Role Type Groups
    /// </summary>
    public class WorkflowRoleTypeGroups
    {
        public const string DebtCounselling = "Debt Counselling";
    }

    /// <summary>
    /// Debt Counselling Proposal Types
    /// </summary>
    public class DebtCounsellingProposalType
    {
        public const string Proposal = "Proposal";
        public const string CounterProposal = "Counter Proposal";
    }

    /// <summary>
    /// Debt Counselling Proposal Status
    /// </summary>
    public class DebtCounsellingProposalStatus
    {
        public const string Draft = "Draft";
        public const string Active = "Active";
        public const string Inactive = "Inactive";
    }

    /// <summary>
    /// Occupancy Types
    /// </summary>
    public class OccupancyType
    {
        public const string OwnerOccupied = "Owner Occupied";
        public const string HolidayHome = "Holiday Home";
        public const string Other = "Other";
        public const string Rental = "Rental";
        public const string InvestmentProperty = "Investment Property";
    }

    /// <summary>
    /// Market Rates
    /// </summary>
    public class MarketRate
    {
        public const string JIBAR_Nacm_Yield_rounded_21 = "JIBAR Nacm Yield (rounded) 21";
        public const string JIBAR_91_Day_Discount_21 = "JIBAR 91 Day Discount 21";
        public const string Twenty_Year_Fixed_Mortgage_Rate = "20 Year Fixed Mortgage Rate";
        public const string JIBAR_Nacm_Yield_rounded_18 = "JIBAR Nacm Yield (rounded) 18";
        public const string JIBAR_91_Day_Discount_18 = "JIBAR 91 Day Discount 18";
        public const string Repo_Rate = "Repo Rate";
        public const string Prime_Lending_Rate = "Prime Lending Rate";
        public const string JIBAR_Nacm_Yield_rounded_15 = "JIBAR Nacm Yield (rounded) 15";
        public const string JIBAR_91_day_Discount_15 = "JIBAR 91 day Discount 15";
        public const string Five_Year_Reset_Fixed_Mortgage_Rate = "5 Year Reset Fixed Mortgage Rate";
        public const string Fixed = "Fixed";
    }

    /// <summary>
    /// Gender
    /// </summary>
    public class Gender
    {
        public const string Male = "Male";
        public const string Female = "Female";
    }

    /// <summary>
    /// Disbursement Transaction Types
    /// </summary>
    public class DisbursementTransactionTypes
    {
        public const string PaymentNoInterest = "Payment (No Interest)";
        public const string GuaranteePayment = "Guarantee Payment";
        public const string CashRequired = "Cash Required";
        public const string OtherDisbursement = "Other Disbursement";
        public const string CancellationRefund = "Cancellation Refund";
        public const string ReAdvance = "ReAdvance";
        public const string Refund = "Refund";
        public const string CashRequiredInterest = "Cash Required (Interest)";
        public const string QuickCash = "Quick Cash";
        public const string CAP2ReAdvance = "CAP2 ReAdvance";
    }

    /// <summary>
    /// Offer Types
    /// </summary>
    public class OfferTypes
    {
        public const string ReAdvance = "Re-Advance";
        public const string FurtherAdvance = "Further Advance";
        public const string FurtherLoan = "Further Loan";
        public const string Life = "Life";
        public const string SwitchLoan = "Switch Loan";
        public const string NewPurchaseLoan = "New Purchase Loan";
        public const string RefinanceLoan = "Refinance Loan";
        public const string Unknown = "Unknown";
        public const string CAP2 = "CAP2";
    }

    /// <summary>
    /// Products
    /// </summary>
    public class Products
    {
        public const string VariableLoan = "Variable Loan";
        public const string VariFixLoan = "VariFix Loan";
        public const string HomeOwnersCover = "Home Owners Cover(HOC)";
        public const string LifePolicy = "Life Policy";
        public const string SuperLo = "Super Lo";
        public const string DefendingDiscountRate = "Defending Discount Rate";
        public const string NewVariableLoan = "New Variable Loan";
        public const string QuickCash = "Quick Cash";
        public const string Edge = "Edge";
    }

    /// <summary>
    /// Reason Descriptions
    /// </summary>
    public class ReasonDescription
    {
        public const string AccountsinArrears = "Accounts in Arrears";
        public const string Arrears = "Arrears";
        public const string ClientatMaxage = "Client at Max age";
        public const string ConditionsofLoan = "Conditions of Loan";
        public const string Credithistorynotclear = "Credit history not clear";
        public const string EffectiveLTVoverNinetyPercent = "Effective LTV over 90%";
        public const string FraudulentActivity = "Fraudulent Activity ";
        public const string HighLTVPTI = "High LTV / PTI";
        public const string Interest = "Interest";
        public const string MaxLoan = "Max Loan";
        public const string MiscellaneousReason = "Miscellaneous Reason";
        public const string Norelianceonincome = "No reliance on income";
        public const string Norelianceonsuretyincome = "No reliance on surety income";
        public const string Poorbondconduct = "Poor bond conduct";
        public const string Poorcreditrecord = "Poor credit record";
        public const string Poorratingonfullgeneral = "Poor rating on full general";
        public const string ProcessingFee = "Processing Fee";
        public const string RDsonbankstatements = "R/D's on bank statements";
        public const string RatesinArrears = "Rates in Arrears";
        public const string RatesOutstanding = "Rates Outstanding";
        public const string RegistrationClose = "Registration Close";
        public const string Restrictivecondonprevbondval = "Restrictive cond on prev bond/val";
        public const string TransferInvolved = "Transfer Involved";
        public const string LetterFaxnotreceived = "Letter/Fax not received";
        public const string Justafterendofcoolingoffperiod = "Just after end of cooling off period";
        public const string Other = "Other";
        public const string MissingDocuments = "Missing Documents";
        public const string IllegibleDocuments = "Illegible Documents";
        public const string InvalidDocuments = "Invalid Documents";
        public const string ValuationResultImpactingApplication = "Valuation Result – Impacting Application";
        public const string ValuerCommentRequirement = "Valuer Comment / Requirement";
        public const string IncomeDoesntmatchBankStatementsSalariedCommission = "Income – Doesn’t match Bank Statements (Salaried/Commission)";
        public const string IncomebankstatementsdoesnotsupportsdeclaredincomeSelfEmployed = "Income – bank statements does not supports declared income (Self Employed)";
        public const string AffordabilityDeficitDebtConsolidationAdditionalIncome = "Affordability (Deficit – Debt Consolidation / Additional Income)";
        public const string ReversalsonBondStatements = "Reversals on Bond Statements";
        public const string UnpaidsonBankStatements = "Unpaids on Bank Statements";
        public const string FullGeneralQuery = "Full General Query";
        public const string RatesQuery = "Rates Query";
        public const string ITCQuery = "ITC Query";
        public const string DeedsQuery = "Deeds Query";
        public const string DeedsPropertyIdentification = "Deeds – Property Identification";
        public const string ITCErrorMessage = "ITC – Error Message";
        public const string DeedsInterdictAttachment = "Deeds – Interdict & Attachment";
        public const string DeedsOverBond = "Deeds – Over Bond";
        public const string DeedsExtent = "Deeds – Extent";
        public const string DeedsOwnership = "Deeds – Ownership";
        public const string DeedsBondGrantRights = "Deeds – Bond Grant Rights";
        public const string ITCJudgement = "ITC – Judgement";
        public const string ITCDefault = "ITC – Default";
        public const string ITCDispute = "ITC – Dispute";
        public const string ITCNotices = "ITC – Notices";
        public const string ITCTraceAlert = "ITC – Trace Alert";
        public const string ITCPaymentProfileArrears = "ITC – Payment Profile Arrears";
        public const string ITCFraudRating = "ITC – Fraud Rating";
        public const string OneHundredPercentLoan = "100% Loan";
        public const string AmendBondAmount = "Amend Bond Amount";
        public const string AmendBondHoldersDetails = "Amend Bond Holders Details";
        public const string AmendCashPortion = "Amend Cash Portion";
        public const string AmendConditions = "Amend Condition/s";
        public const string AmendDeposit = "Amend Deposit";
        public const string AmendLinkRate = "Amend Link Rate";
        public const string AmendLoanAmount = "Amend Loan Amount";
        public const string AmendPropertyDetails = "Amend Property Details";
        public const string AmendPurchasePrice = "Amend Purchase Price";
        public const string AmendSPVName = "Amend SPV Name";
        public const string AmendSuretyDetails = "Amend Surety Details";
        public const string AmendSwitchAmount = "Amend Switch Amount";
        public const string AmendTerm = "Amend Term";
        public const string BondInOneName = "Bond In One Name";
        public const string ChangeToLegalEntity = "Change To Legal Entity";
        public const string ChangeToPreviousGrant = "Change To Previous Grant";
        public const string CodeInHoc = "Code In HOC";
        public const string ExtendedTerm = "Extended Term";
        public const string JointNames = "Joint Names";
        public const string RemainAsPerPreviousGrant = "Remain As Per Previous Grant";
        public const string SuperLo = "Super Lo";
        public const string TreatAsNewPurchase = "Treat As New Purchase";
        public const string TreatAsRefinance = "Treat As Refinance";
        public const string TreatAsSwitch = "Treat As Switch";
        public const string Varifix = "Varifix";
        public const string ClientConsidering = "Client Considering";
        public const string Declaration = "Declaration";
        public const string LeftMessage = "Left Message";
        public const string LOAAmendment = "LOA Amendment";
        public const string NotAvailable = "Not Available";
        public const string Quoted = "Quoted";
        public const string RPARConsidering = "RPAR - Considering";
        public const string CancellationReceived = "Cancellation Received";
        public const string SMSSent = "SMS Sent";
        public const string AwaitingSpouseConfirmation = "Awaiting Spouse Confirmation";
        public const string Cededapolicy = "Ceded a policy";
        public const string Covernotrequired = "Cover not required";
        public const string Declarationnotacceptable = "Declaration not acceptable";
        public const string Donotcall = "Do not call";
        public const string Exclusionsnotacceptable = "Exclusions not acceptable";
        public const string LoanNTUd = "Loan NTU'd";
        public const string Mayconsiderinfuture = "May consider in future";
        public const string Maximumcallsreached = "Maximum calls reached";
        public const string Noresponse = "No response";
        public const string Requiresdeathanddisability = "Requires death and disability";
        public const string Requireswholelife = "Requires whole life";
        public const string RPARnotreplacing = "RPAR - not replacing";
        public const string Sufficientcover = "Sufficient cover";
        public const string Tooexpensive = "Too expensive";
        public const string Cantafford = "Can't afford";
        public const string Requirecashvalue = "Require cash value";
        public const string DebtConsolidation = "Debt Consolidation";
        public const string MovableAssetsFinance = "Movable Assets Finance";
        public const string HomeRenovation = "Home Renovation";
        public const string InvestmentProperty = "Investment Property";
        public const string NewHomePurchase = "New Home Purchase";
        public const string ChangeFinancialInstitution = "Change Financial Institution";
        public const string NeedsForensicinvestigation = "Needs Forensic investigation";
        public const string SaleFellThrough = "Sale Fell Through";
        public const string DeclinedbyCredit = "Declined by Credit";
        public const string HOCLifeRequirement = "HOC/Life Requirement";
        public const string PurchasePriceDiscrepancy = "Purchase Price Discrepancy";
        public const string ArrearRates = "Arrear Rates";
        public const string Withdrawn = "Withdrawn";
        public const string NHBRCCertificateProblems = "NHBRC Certificate Problems";
        public const string ITC = "ITC";
        public const string Applicanthasahighaggregatedvalueofjudgements = "Applicant has a high aggregated value of judgements";
        public const string Applicanthasbeenunderadministrationorderinthepast = "Applicant has been under administration order in the past";
        public const string ApplicanthashadfinancialjudgementswithinthelastTwoyears = "Applicant has had financial judgements within the last 2 years";
        public const string Applicanthasnonsettledjudgements = "Applicant has non-settled judgements";
        public const string Applicanthasundergonesequestrationinthepast = "Applicant has undergone sequestration in the past";
        public const string Areaclassificationisunacceptable = "Area classification is unacceptable";
        public const string Commercialpropertyisunacceptablesecurity = "Commercial property is unacceptable security";
        public const string ContraventionofExhangeControlRegulations = "Contravention of Exhange Control Regulations";
        public const string FailedAffordabilityTest = "Failed Affordability Test";
        public const string Insufficientproofofincome = "Insufficient proof of income";
        public const string LoanamountexceptionallyhighLoanGreaterThanFiveMil = "Loan amount exceptionally high, Loan > 5Mil";
        public const string LoanamountexceptionallyhighValueGreaterThanTenMil = "Loan amount exceptionally high, Value > 10Mil";
        public const string LTVexceedspolicyrequirements = "LTV exceeds policy requirements";
        public const string LTVisunacceptableforasmallholding = "LTV is unacceptable for a small holding";
        public const string MixedConstruction = "Mixed Construction";
        public const string Mortgagepropertyisunacceptable = "Mortgage property is unacceptable";
        public const string NoticeAdministrationOrder = "Notice - Administration Order";
        public const string NoticeDebtreview = "Notice - Debt review";
        public const string NoticeInsolvent = "Notice - Insolvent";
        public const string PoorCreditBehaviourITCJudgements = "Poor Credit Behaviour - ITC Judgements";
        public const string PoorCreditBehaviourUtilityLoanandBondaccount = "Poor Credit Behaviour - Utility, Loan and Bond account";
        public const string PoorEmpiricaScore = "Poor Empirica Score";
        public const string Poormaintenanceofutilityaccounts = "Poor maintenance of utility accounts";
        public const string PrimaryApplicantisunacceptablyyoung = "Primary Applicant is unacceptably young";
        public const string LoanvalueistoolowLoanAmountLessThan140 = "Loan value is too low - Loan Amount < 140K";
        public const string PropertyvalueistoolowPropertyValLessThan170 = "Property value is too low - Property Val < 170K";
        public const string PTIexceedspolicyrequirements = "PTI exceeds policy requirements";
        public const string RehabilitatedInsolventunder12months = "Rehabilitated Insolvent - under 12months";
        public const string Secondarypropertyisunacceptable = "Secondary property is unacceptable.";
        public const string Shareblockisanunacceptablepropertytype = "Share block is an unacceptable property type";
        public const string SustainableIncomenotavailable = "Sustainable Income not available";
        public const string Unabletostructurefinance = "Unable to structure finance";
        public const string Vacantlandisanunacceptablepropertytype = "Vacant land is an unacceptable property type";
        public const string ApplicationReason = "Application Reason";
        public const string LegalEntityReason = "Legal Entity Reason";
        public const string ApplicationCreatedinError = "Application Created in Error";
        public const string Condition33stillineffect = "Condition 33 still in effect.";
        public const string Accountslistedonclientspaymentprofilehavearrearrecords = "Accounts listed on clients payment profile have arrear records.";
        public const string Priorloanconditiontosettledebtnotmet = "Prior loan condition to settle debt not met.";
        public const string ITCKnockoutRuleDefaults = "ITC Knockout Rule - Defaults";
        public const string ITCKnockoutRuleWorstEverPaymentProfile = "ITC Knockout Rule - WorstEverPaymentProfile";
        public const string ITCKnockoutRuleDebtReview = "ITC Knockout Rule – Debt Review";
        public const string CreditScoreRiskMatrixDecline = "Credit Score – Risk Matrix Decline";
        public const string ITCKnockoutRuleJudgments = "ITC Knockout Rule – Judgments";
        public const string ITCKnockoutRuleNotices = "ITC Knockout Rule - Notices";
        public const string ITCKnockoutRuleDisputes = "ITC Knockout Rule - Disputes";
        public const string NocreditscoreEmpiricaScorenotavailable = "No credit score - Empirica Score not available";
        public const string NocreditscoreMissingIncompleteSBCinformation = "No credit score -  Missing/Incomplete SBC information";
        public const string Benefitsnotacceptable = "Benefits not acceptable";
        public const string BankCounterOfferconditions = "Bank Counter Offer – conditions";
        public const string BankCounterOfferLTV = "Bank Counter Offer – LTV";
        public const string BankCounterOfferRate = "Bank Counter Offer – Rate";
        public const string ClientCancelledother = "Client Cancelled – other";
        public const string Clientcancelledpoorservice = "Client cancelled – poor service";
        public const string ClientunabletogetsufficientCashOut = "Client unable to get sufficient Cash Out";
        public const string Documentationrequirementstooonerous = "Documentation requirements too onerous";
        public const string Expiryduetoexcessivetimelapse = "Expiry due to excessive time lapse";
        public const string PurchaseSaleagreementcancelledcollapsed = "Purchase & Sale agreement cancelled / collapsed";
        public const string SAHLratenotasgoodasoriginallyexpected = "SAHL rate not as good as originally expected";
        public const string Valuationtoolow = "Valuation too low";
        public const string NocreditscoreNoITCdata = "No credit score - No ITC data";
        public const string CreditScoreRiskMatrixRefer = "Credit Score – Risk Matrix Refer";
        public const string _171Received = "17.1 Received";
        public const string _172Received = "17.2 Received";
        public const string Proposalreceived = "Proposal received";
        public const string CourtApplicationreceived = "Court Application received";
        public const string CourtOrderReceived = "Court Order Received";
        public const string ReallocatetoDC = "Reallocate to DC";
        public const string ManageDC = "Manage DC";
        public const string ProposalAcceptance = "Proposal Acceptance";
        public const string CourtOrderAcceptance = "Court Order Acceptance";
        public const string CourtOrderwithAppeal = "Court Order with Appeal";
        public const string _86_10_Noarrangementandnotreferredtocourt = "86(10) - No arrangement and not referred to court";

        public const string _86_10_Noarrangement_notreferredtocourtandnopaymentsintermsofproposal =
        @"86(10) - No arrangement, not referred to court and no payments in terms of proposal";

        public const string _86_10_Noproposalreceived = "86(10) - No proposal received";
        public const string _88_3_Defaultintermsofapprovedarrangement = "88(3) - Default in terms of approved arrangement";
        public const string _88_3_Defaultintermsofcourtorder = "88(3) - Default in terms of court order";
        public const string CaseCreatedInError = "Case Created In Error";
        public const string ClientHasVoluntarilyCancelled = "Client Has Voluntarily Cancelled";
        public const string DCCancelledDuetoConsumerNonCompliance = "DC Cancelled Due to Consumer Non Compliance";
        public const string DCCancelledDuetoNonPayment = "DC Cancelled Due to Non Payment";
        public const string DCCancelledClientRehabilitated = "DC Cancelled Client Rehabilitated";
        public const string DCCancelledClientSequestrated = "DC Cancelled Client Sequestrated";
        public const string DCCancelledClientDeceased = "DC Cancelled Client Deceased";
        public const string BondExcludedInArrears = "Bond Excluded In Arrears";
        public const string NotificationofDeath = "Notification of Death";
        public const string NotificationofSequestration = "Notification of Sequestration";
        public const string UnderDebtCounselling = "Under Debt Counselling";
        public const string NoNeedForCredit = "No need for credit";
        public const string LoanSizeInsufficient = "Loan size insufficient";
        public const string AlreadyOverIndebted = "Already over indebted";
        public const string CapitecBranchDeclineReason = "Capitec Branch Decline Reason";
        public const string CapitecClientContacted = "Capitec Client Contacted";
    }

    /// <summary>
    /// Provinces
    /// </summary>
    public class Province
    {
        public const string NorthWest = "North West";
        public const string WesternCape = "Western Cape";
        public const string Mpumalanga = "Mpumalanga";
        public const string Kwazulunatal = "Kwazulu-natal";
        public const string NorthernCape = "Northern Cape";
        public const string EasternCape = "Eastern Cape";
        public const string Limpopo = "Limpopo";
        public const string Gauteng = "Gauteng";
        public const string FreeState = "Free State";
    }

    /// <summary>
    /// Address Types
    /// </summary>
    public class AddressType
    {
        public const string Postal = "Postal";
        public const string Residential = "Residential";
    }

    /// <summary>
    /// Address Formats
    /// </summary>
    public class AddressFormat
    {
        public const string Street = "Street";
        public const string Box = "Box";
        public const string PostNetSuite = "PostNet Suite";
        public const string PrivateBag = "Private Bag";
        public const string FreeText = "Free Text";
        public const string ClusterBox = "Cluster Box";
    }

    /// <summary>
    /// Relationship Types
    /// </summary>
    public class RelationshipType
    {
        public const string Spouse = "Spouse";
        public const string Partner = "Partner";
        public const string ImmediateFamilyMember = "Immediate Family Member";
        public const string CloseRelative = "Close Relative";
        public const string PowerofAttorney = "Power of Attorney";
        public const string OfficeroftheOrganization = "Officer of the Organization";
        public const string Trustee = "Trustee";
        public const string CompanyEmployee = "Company Employee";
        public const string CompanyEmployer = "Company Employer";
        public const string Member = "Member";
        public const string Director = "Director";
        public const string ExecutiveDirector = "Executive Director";
    }

    public class LoanType
    {
        public const string PersonalLoan = "Personal Loan";
        public const string StudentLoan = "Student Loan";
    }

    /// Salutations
    /// </summary>
    public class SalutationType
    {
        public const string Mr = "Mr";
        public const string Mrs = "Mrs";
        public const string Prof = "Prof";
        public const string Capt = "Capt";
        public const string Dr = "Dr";
        public const string Miss = "Miss";
        public const string Past = "Past";
        public const string Sir = "Sir";
        public const string Ms = "Ms";
        public const string Lord = "Lord";
        public const string Rev = "Rev";
    }

    public class HearingType
    {
        public const string Court = "Court";
        public const string Tribunal = "Tribunal";
    }

    public class AppearanceType
    {
        public const string Appeal = "Appeal";
        public const string AppealDeclined = "Appeal Declined";
        public const string AppealGranted = "Appeal Granted";
        public const string AppealPostponed = "Appeal Postponed";
        public const string CourtApplication = "Court Application";
        public const string CourtApplicationPostponed = "Court Application Postponed";
        public const string OrderGranted = "Order Granted";
        public const string ConsentOrderGranted = "Consent Order Granted";
    }

    public class Court
    {
        public const string Pinetown = "Pinetown";
    }

    public class OnlineStatementFormat
    {
        public const string HTML = "HTML";
        public const string NotApplicable = "Not Applicable";
        public const string PDFFormat = "PDF Format";
        public const string Text = "Text";
    }

    public class ApplicationType
    {
        public const string Any = "Any";
        public const string DebtCounselling = "Debt Counselling";
        public const string CAP2 = "CAP2";
        public const string FurtherAdvance = "Further Advance";
        public const string FurtherLoan = "Further Loan";
        public const string Life = "Life";
        public const string NewPurchaseLoan = "New Purchase Loan";
        public const string ReAdvance = "Re-Advance";
        public const string RefinanceLoan = "Refinance Loan";
        public const string SwitchLoan = "SwitchLoan";
        public const string Unknown = "Unknown";
        public const string Capitec = "Capitec";
    }

    public class ConditionalActivities
    {
        public class DebtCounselling
        {
            public const string _45DaysCancel = "45 Days Cancel";
            public const string _45DaysLapsed = "45 Days Lapsed";
            public const string NoCourtDateorDeposit = "No Court Date or Deposit";
            public const string CourtDateOrDeposit = "Court Date or Deposit";
            public const string TerminateSuccess = "Terminate Success";
            public const string OptOutRequired = "Opt Out Required";
            public const string OptOutNotRequired = "Opt Out Not Required";
            public const string Post973FinTran = "Post973FinTran";
        }

        public class ApplicationManagement
        {
            public const string AssignProcessor = "Assign Processor";
            public const string HighestPriority = "Highest Priority";
            public const string AssignedQA = "Assigned QA";
            public const string NewPurchase = "New Purchase";
            public const string CreateEWorkPipelineCase = "Create EWork PipelineCase";
            public static string CreditDeclineApplication = "Credit Decline Application";
            public static string SystemBackToCredit = "SystemBackToCredit";
            public static string SysDeclinebyCredit = "Sys Decline by Credit";
        }

        public class Credit
        {
            public const string DeclinedbyCredit = "Declined by Credit";
        }

        public class Valuations
        {
            public const string ValuationRequested = "Valuation Requested";
        }
    }

    public class PaymentType
    {
        public const string DebitOrderPayment = "Debit Order Payment";
        public const string SubsidyPayment = "Subsidy Payment";
        public const string DirectPayment = "Direct Payment";
    }

    public class WorkflowRoleType
    {
        public const string DebtCounsellingAdminD = "Debt Counselling Admin D";
        public const string DebtCounsellingConsultantD = "Debt Counselling Consultant D";
        public const string DebtCounsellingCourtConsultantD = "Debt Counselling Court Consultant D";
        public const string RecoveriesManagerD = "Recoveries Manager D";
        public const string DebtCounsellingSupervisorD = "Debt Counselling Supervisor D";
        public const string ForeclosureConsultantD = "Foreclosure Consultant D";
        public const string ForeclosureSupportParalegalD = "Foreclosure Support Paralegal D";
        public const string LossControlDirectorD = "Loss Control Director D";
        public const string PLConsultantD = "PL Consultant D";
        public const string PLManagerD = "PL Manager D";
        public const string PLSupervisorD = "PL Supervisor D";
        public const string PLAdminD = "PL Admin D";
        public const string PLCreditAnalystD = "PL CreditAnalyst D";
    }

    public class ExternalActivities
    {
        public class Life
        {
            public const string CreateInstance = "Create Instance";
        }

        public class ApplicationManagement
        {
            public const string PipelineUpForFees = "Pipeline_UpForFees";
            public const string ExtComplete = "Ext Complete";
            public const string EXTDataStoreFLDocReceived = "EXT DataStore FL Doc Recieved";
            public const string EXTDisburse = "EXT Disburse";
            public const string ExtHeldOver = "Ext Held Over";
            public const string EXTHoldApplication = "EXT Hold Application";
            public const string EXTNTU = "EXT NTU";
            public const string EXTNTUFinal = "EXT NTU Final";
            public const string EXTReactivateApp = "EXT Reactivate App";
            public const string EXTReinstate = "EXT Reinstate";
            public const string EXT_ArchiveFollowup = "EXT_ArchiveFollowup";
            public const string EXT_ArchiveValuationClone = "EXT_ArchiveValuationClone";
            public const string EXT_CommonArchiveMain = "EXT_CommonArchiveMain";
            public const string OnStuck = "OnStuck";
            public const string PipelineRelodge = "Pipeline Relodge";
            public const string Pipeline_UpForFees = "Pipeline_UpForFees";
            public const string ValuationComplete = "ValuationComplete";
        }

        public class DebtCounselling
        {
            public const string EXTIntoArrears = "DebtCounsellingArrears";
            public const string EXTOutofArrears = "DebtCounsellingArrears";
            public const string EXTUnderCancellation = "EXT Under Cancellation";
            public const string UnderCancellationRemoved = "Under Cancellation Removed";
            public const string EXTCancellationRegistered = "EXT Cancellation Registered";
            public const string ExtOutBondExclArrears = "Out Bond Exclusion Arrears";
            public const string ExtIntoBondExclArrears = "Into Bond Exclusion Arrears";
        }

        public class Valuations
        {
        }

        public class ReadvancePayments
        {
            public const string FurtherAdvanceBelowLAA = "Further Advance Below LAA";
        }
    }

    public class OrganisationType
    {
        public const string Company = "Company";
        public const string RegionOrChannel = "Region/Channel";
        public const string BranchOrOriginator = "Branch/Originator";
        public const string Team = "Team";
        public const string Department = "Department";
        public const string SalesOffice = "Sales Office";
        public const string Designation = "Designation";
        public const string Division = "Division";
        public const string ExternalBranch = "External Branch";
        public const string SubDepartment = "Sub-Department";
    }

    public class LoanPurposeType
    {
        public const string SwitchLoan = "Switch loan";
        public const string NewPurchase = "New purchase";
        public const string Refinance = "Refinance";
    }

    public class ExpenseType
    {
        public const string InitiationFeeBondPreparationFee = "Initiation Fee – Bond Preparation Fee";
        public const string TransferFee = "Transfer Fee";
        public const string InitiationFeeValuationFee = "Initiation fee – Valuation Fee";
        public const string RegistrationFee = "Registration Fee";
        public const string CancellationFee = "Cancellation Fee";
        public const string InitiationFeeQuickCashProcessingFee = "Initiation Fee – Quick Cash Processing Fee";
        public const string LifeAdministrationFee = "Life Administration Fee";
        public const string ExistingMortgageAmount = "Existing mortgage amount";
        public const string InterimInterest = "Interim interest";
        public const string StoreCard = "Store Card";
        public const string VehicleFinance = "Vehicle Finance";
        public const string Other = "Other";
        public const string QuickCash = "Quick Cash";
        public const string QuickCashSettlement = "Quick Cash Settlement";
        public const string DeedsOfficeFee = "Deeds Office Fee";
        public const string PersonalLoanInitiationFee = "Personal Loan Initiation Fee";
    }

    public class ExternalRoleType
    {
        public const string Client = "Client";
        public const string DebtCounsellor = "Debt Counsellor";
        public const string PaymentDistributionAgent = "Payment Distribution Agent";
        public const string NationalCreditRegulator = "National Credit Regulator";
        public const string LitigationAttorney = "Litigation Attorney";
        public const string DebtCounselling = "Debt Counselling";
        public const string DeceasedEstates = "Deceased Estates";
        public const string Foreclosure = "Foreclosure";
        public const string Sequestrations = "Sequestrations";
        public const string WebAccess = "WebAccess";
    }

    public class SAHLWebsiteValidationMessages
    {
        //Validation Messages
        public const string LoanYearTerm_ValidationMessage = "The Term of Loan must be between 1 and 20 years.";

        public const string LoanMonthTerm_ValidationMessage = "The loan term must be between 1 and 240 months.";
        public const string MonthlyInstallment_ValidationMessage = "Your Monthly installment should not exceed R [Token] per month";
        public const string GrossIncome_ValidationMessage = "Your gross income must exceed R 5,000.00 to qualify.";
        public const string HomeValue_ValidationMessage = "Your home's value must be at least R 150,000.00";
        public const string InterestRate_ValidationMessage = "The interest rate must be at least 1% and less than 100%";
        public const string CurrentLoanToSwitch_ValidationMessage = "You must have a current loan to switch.";
        public const string PurchasePrice_ValidationMessage = "The purchase price must be between R 170,000 & R 5,000,000.";
        public const string FixedAmount_ValidationMessage = "The selected fixed amount is R [Token], less than the minimum of R 50000";
        public static string DepositCantBeEqualPurchasePrice = "Deposit cannot be equal the purchase price.";

        //Field Required Validation Messages
        public const string IncomeAmountRequired_ValidationMessage = "An Income amount must be entered.";

        public const string ProfitFromSaleRequired_ValidationMessage = "Profit from sale of existing home must be entered.";
        public const string OtherContributionsRequired_ValidationMessage = "Other Contributions must be entered.";
        public const string MonthlyInstallmentRequired_ValidationMessage = "A Monthly Installment is required.";
        public const string PurchasePriceRequired_ValidationMessage = "Purchase Price is required.";
        public const string LoanTermRequired_ValidationMessage = "Loan Term is required.";
        public const string TermOfLoanRequired_ValidationMessage = "Term of Loan is required.";
        public const string GrossIncomeRequired_ValidationMessage = "Gross monthly income is required.";
        public const string InterestRateRequired_ValidationMessage = "Interest Rate is required.";
        public const string HomeValueRequired_ValidationMessage = "Value of your home required.";
        public static string CurrentLoanAmountRequired_ValidationMessage = "Current Loan Amount required.";
    }

    public class ManualDebitOrderValidationMessages
    {
        public const string EffectiveDateGreaterTodayUnless14h00 = "Effective Date must be greater than today, or if today's date it must be captured before 14h00";
        public const string BankAccountMandatory = "Bank Account is a mandatory field for Manual Debit Order Payments.";
        public const string AmountGreaterZero = "Amount must be greater than 0.";
        public const string NoPaymentsRequired = "Please enter the number of payments.";
    }

    public class ACBType
    {
        public const string Unknown = "Unknown";
        public const string Current = "Current";
        public const string Savings = "Savings";
        public const string Bond = "Bond";
        public const string CreditCard = "Credit Card";
    }

    public class CreditScoreDecision
    {
        public const string NoScore = "No Score";
        public const string Accept = "Accept";
        public const string Refer = "Refer";
        public const string Decline = "Decline";
    }

    public class DebtCounsellingLoadBalanceStates
    {
        /// <summary>
        /// States to exclude in the consultant assignment for debt counselling
        /// </summary>
        public static readonly IList<string> consultantAssignmentExclusionStates
            = new ReadOnlyCollection<string>(
                new List<string>() {
                                    WorkflowStates.DebtCounsellingWF.DebtReviewApproved,
                                    WorkflowStates.DebtCounsellingWF.BondExclusions,
                                    WorkflowStates.DebtCounsellingWF.BondExclusionsArrears
                                    });

        /// <summary>
        /// states to include when load balancing to supervisors in debt counselling.
        /// </summary>
        public static readonly IList<string> supervisorAssignmentInclusionStates
                = new ReadOnlyCollection<string>(
                    new List<string>()  {
                                        WorkflowStates.DebtCounsellingWF.DecisiononProposal,
                                        WorkflowStates.DebtCounsellingWF.PaymentReview,
                                        WorkflowStates.DebtCounsellingWF.RecoveriesProposalDecision
                                        });
    }

    public class DetailClass
    {
        public const string RegistrationProcess = "Registration Process";
        public const string LoanManagement = "Loan Management";
        public const string LoanIdentification = "Loan Identification";
        public const string CivilServants = "Civil Servants";
        public const string Parastatal = "Parastatal";
        public const string Corporate = "Corporate";
        public const string LocalAuthorities = "Local Authorities";
        public const string CancellationFollowup = "Cancellation Followup";
    }

    public class CancellationType
    {
        public const string Switch = "Switch";
        public const string Sale = "Sale";
        public const string Unkown = "Unkown";
    }

    public class RoleType
    {
        public const string AssuredLife = "Assured Life";
        public const string MainApplicant = "Main Applicant";
        public const string Suretor = "Suretor";
        public const string LeadMainApplicant = "Lead - Main Applicant";
    }

    public class LegalEntityType
    {
        public const string Unknown = "Unknown";
        public const string NaturalPerson = "Natural Person";
        public const string Company = "Company";
        public const string CloseCorporation = "Close Corporation";
        public const string Trust = "Trust";
    }

    public enum LegalEntityStatus
    {
        Alive = 0,
        Deceased,
        Disabled
    }

    public class Catalogs
    {
        public static string _2am = "2am";
        public static string SAHLDB = "sahldb";
        public static string eWork = "e-work";
        public static string x2 = "x2";
    }

    public class CapPaymentOptions
    {
        public const string DebitOrderBankAccount = "Debit Order Bank Account";
        public const string LoanAccount = "Loan Account";
    }

    public class RoofTypes
    {
        public const string Conventional = "Conventional";
        public const string Thatch = "Thatch";
        public const string Partial = "Partial";
    }

    public class DetailType
    {
        public const string CancellationRegistered = "Cancellation Registered";
        public const string UnderCancellation = "Under Cancellation";
        public const string RemoveCancellation = "Remove Cancellation";
        public const string DebitOrderSuspended = "Debit Order Suspended";
        public const string DirectMortgageBondRegistered = "Direct Mortgage Bond Registered";
        public const string SuperLoelectedbyclient = "Super Lo elected by client";
    }

    public class GeneralStatusConst
    {
        public const string Active = "Active";
        public const string InActive = "Inactive";
    }

    public class PropertyType
    {
        public const string Unknown = "Unknown";
        public const string House = "House";
        public const string Flat = "Flat";
        public const string Duplex = "Duplex";
        public const string Simplex = "Simplex";
        public const string ClusterHome = "Cluster Home";
        public const string Maisonette = "Maisonette";
    }

    public class TitleType
    {
        public const string Unknown = "Unknown";
        public const string House = "Freehold";
        public const string SectionalTitle = "Sectional Title";
        public const string FreeholdEstate = "Freehold Estate";
        public const string Shareblock = "Shareblock";
        public const string Leasehold = "Leasehold";
        public const string SectionalTitleWithHOC = "Sectional Title With HOC";
    }

    public class AreaClassification
    {
        public const string Unknown = "Unknown";
        public const string _1Class = "1 Class";
        public const string _2Class = "2 Class";
        public const string _3Class = "3 Class";
        public const string _4Class = "4 Class";
        public const string _5Class = "5 Class";
        public const string _6Class = "6 Class";
    }

    public class DeedsPropertyType
    {
        public const string Erf = "Erf";
        public const string Unit = "Unit";
    }

    public class HOCConstruction
    {
        public const string BrickandTile = "Brick and Tile";
        public const string Wooden = "Wooden";
    }

    public class HOCSubsidence
    {
        public const string NotRequired = "Not Required";
        public const string Limited = "Limited";
        public const string Extended = "Extended";
    }

    public class ValuationClassification
    {
        public const string Budgetstandard = "Budget standard";
        public const string Normalstandard = "Normal standard";
        public const string Luxury = "Luxury";
        public const string Luxurywithslatethatchroof = "Luxury with slate/thatch roof";
        public const string Exclusive = "Exclusive";
    }

    public class ValuationImprovementType
    {
        public const string Walls = "Walls";
        public const string SwimmingPool = "Swimming Pool";
        public const string TennisCourt = "Tennis Court";
        public const string Lapa = "Lapa";
        public const string Paving = "Paving";
        public const string Other = "Other";
    }

    public class ValuationRoofType
    {
        public const string Conventional = "Conventional";
        public const string Thatch = "Thatch";
        public const string Other = "Other";
    }

    public class ValuationStatus
    {
        public const string Pending = "Pending";
        public const string Complete = "Complete";
        public const string Withdrawn = "Withdrawn";
        public const string Returned = "Returned";
    }

    public class HOCRoof
    {
        public const string Thatch = "Thatch";
        public const string Conventional = "Conventional";
        public const string Partial = "Partial";
        public const string Shingle = "Shingle";
    }

    public class ValuationUserActivity
    {
        public const string ArchiveAssessment = "Archive Assessment";
        public const string CancelWithdrawnAssesment = "Cancel Withdrawn Assesment";
        public const string CaptureManualValuation = "Capture Manual Valuation";
        public const string EscalatetoManager = "Escalate to Manager";
        public const string FurtherValuationRequired = "Further Valuation Required";
        public const string InstructAdcheckValuer = "Instruct Adcheck Valuer";
        public const string InstructEzValValuer = "Instruct Ez-Val Valuer";
        public const string InstructValuertoWithdraw = "Instruct Valuer to Withdraw";
        public const string ManagerAdCheckReSchedule = "Manager AdCheck Re-Schedule";
        public const string ManagerArchive = "Manager Archive";
        public const string ManualValuation = "Manual Valuation";
        public const string Note = "Note";
        public const string PerformManualValuation = "Perform Manual Valuation";
        public const string ReinstructValuer = "Re-instruct Valuer";
        public const string ReScheduleAdCheckAssessment = "Re-Schedule AdCheck Assessment";
        public const string ReassignUser = "Reassign User";
        public const string RequestAutomatedValuation = "Request Automated Valuation";
        public const string RequestLightstoneValuation = "Request Lightstone Valuation";
        public const string RequestValuationReview = "Request Valuation Review";
        public const string RetryFurtherVal = "Retry Further Val";
        public const string RetryReviewRequest = "Retry Review Request";
        public const string RetryValuationCreate = "Retry Valuation Create";
        public const string ReviewValuationRequired = "Review Valuation Required";
        public const string SetActiveValuation = "Set Active Valuation";
        public const string ValuationinOrder = "Valuation in Order";
        public const string WithdrawAssessment = "Withdraw Assessment";
    }

    public class X2SecurityGroups
    {
        public const string DebtCounsellingCourtConsultantD = "Debt Counselling Court Consultant D";
        public const string RCSSales_D = "RCSSales_D";
        public const string QCManager = "QC Manager";
        public const string RCSExceptionCommittee = "RCSExceptionCommittee";
        public const string PLManagerD = "PL Manager D";
        public const string ForeclosureConsultant = "Foreclosure Consultant";
        public const string RVAdminD = "RV Admin D";
        public const string QCCollectionsAdminD = "QC Collections Admin D";
        public const string LossControlLitigationConsultant = "Loss Control Litigation Consultant";
        public const string RegistrationsSupervisorD = "Registrations Supervisor D";
        public const string DeleteDebitOrderRole = "DeleteDebitOrderRole";
        public const string LossControlDirector = "Loss Control Director";
        public const string FLManager = "FL Manager";
        public const string CapAdmin = "CapAdmin";
        public const string RegistrationsAdministratorD = "Registrations Administrator D";
        public const string Everyone = "Everyone";
        public const string FLApplicationProcessor = "FL Application Processor";
        public const string QCSupervisorD = "QC Supervisor D";
        public const string DebtCounsellingConsultantD = "Debt Counselling Consultant D";
        public const string NBOverride = "NBOverride";
        public const string FLSupervisor = "FL Supervisor";
        public const string CreditSupervisor = "Credit Supervisor";
        public const string DebtCounsellingSupervisorD = "Debt Counselling Supervisor D";
        public const string CreditExceptionsD = "Credit Exceptions D";
        public const string HelpDesk = "HelpDesk";
        public const string BranchConsultantD = "Branch Consultant D";
        public const string SPVTermChange = "SPVTermChange";
        public const string RegistrationsSupervisor = "Registrations Supervisor";
        public const string CapManagement = "CapManagement";
        public const string NewBusinessSupervisor = "New Business Supervisor";
        public const string RecoveriesManagerD = "Recoveries Manager D";
        public const string PLCreditAnalystD = "PL Credit Analyst D";
        public const string FLCollectionsAdmin = "FL Collections Admin";
        public const string DebtCounsellingAdmin = "Debt Counselling Admin";
        public const string ValuationsAdmin = "Valuations Admin";
        public const string QCManagerD = "QC Manager D";
        public const string RegistrationsManagerD = "Registrations Manager D";
        public const string PLConsultant = "PL Consultant";
        public const string PLSupervisor = "PL Supervisor";
        public const string CapManager = "CapManager";
        public const string BranchManagerD = "Branch Manager D";
        public const string PLAdmin = "PL Admin";
        public const string NewBusinessSupervisorD = "New Business Supervisor D";
        public const string LifeManagement = "Life Management";
        public const string LossControlQCCollectionsAdmin = "Loss Control QC Collections Admin";
        public const string NewBusinessProcessorD = "New Business Processor D";
        public const string LossControlLitigationConsultantD = "Loss Control Litigation Consultant D";
        public const string ITStaff = "IT Staff";
        public const string RCSSales = "RCSSales";
        public const string CreditUnderwriterD = "Credit Underwriter D";
        public const string CCCLifeAdmin = "CCC Life Admin";
        public const string LossControlLitigationBackupConsultant = "Loss Control Litigation Backup Consultant";
        public const string RVManagerD = "RV Manager D";
        public const string ValuationsManagerD = "Valuations Manager D";
        public const string RegistrationsLOAAdminD = "Registrations LOA Admin D";
        public const string RVAdmin = "RV Admin";
        public const string RCSManager = "RCSManager";
        public const string PLManager = "PL Manager";
        public const string CapCreditBroker = "CapCreditBroker";
        public const string WorkList = "WorkList";
        public const string CurrentConsultant = "Current Consultant";
        public const string DebtCounsellingManagerD = "Debt Counselling Manager D";
        public const string CapBroker = "CapBroker";
        public const string DebtCounsellingManager = "Debt Counselling Manager";
        public const string PLComplianceConsultant = "PL Compliance Consultant";
        public const string CCCLifeSales = "CCC Life Sales";
        public const string TranslateConditionsD = "Translate Conditions D";
        public const string PLComplianceConsultantD = "PL Compliance Consultant D";
        public const string DebtCounsellingAdminD = "Debt Counselling Admin D";
        public const string ForeclosureConsultantD = "Foreclosure Consultant D";
        public const string LossControlDirectorD = "Loss Control Director D";
        public const string TrackList = "TrackList";
        public const string RegistrationsManager = "Registrations Manager";
        public const string BranchAdmin = "Branch Admin";
        public const string FLSupervisorD = "FL Supervisor D";
        public const string Originator = "Originator";
        public const string PLAdminD = "PL Admin D";
        public const string RegistrationsLOAAdmin = "Registrations LOA Admin";
        public const string QCAdmin = "QC Admin";
        public const string NewBusinessProcessor = "New Business Processor";
        public const string BranchConsultant = "Branch Consultant";
        public const string FLManagerD = "FL Manager D";
        public const string DebtCounsellingSupervisor = "Debt Counselling Supervisor";
        public const string QCCollectionsAdmin = "QC Collections Admin";
        public const string LifeAdmin = "Life Admin";
        public const string ValuationsAdministratorD = "Valuations Administrator D";
        public const string ResubmissionAdmin = "Resubmission Admin";
        public const string CCCLifeManagement = "CCC Life Management";
        public const string CreditManagerD = "Credit Manager D";
        public const string Resubmission = "Resubmission";
        public const string FLCollectionsAdminD = "FL Collections Admin D";
        public const string PLConsultantD = "PL Consultant D";
        public const string ResubmissionAdminD = "Resubmission Admin D";
        public const string RegistrationsAdministrator = "Registrations Administrator";
        public const string SPVTermChangeReview = "SPVTermChangeReview";
        public const string ReleaseAndVariationsAdmin = "Release And Variations Admin";
        public const string ValuationsManager = "Valuations Manager";
        public const string CreditSupervisorD = "Credit Supervisor D";
        public const string LossControlLitigationBackupConsultantD = "Loss Control Litigation Backup Consultant D";
        public const string CapSales = "CapSales";
        public const string QCAdminD = "QC Admin D";
        public const string QAAdministratorD = "QA Administrator D";
        public const string LossControlQCCollectionsAdminD = "Loss Control QC Collections Admin D";
        public const string CreditManager = "Credit Manager";
        public const string CreditExceptions = "Credit Exceptions";
        public const string NewBusinessManager = "New Business Manager";
        public const string QCSupervisor = "QC Supervisor";
        public const string TranslateConditions = "Translate Conditions";
        public const string QCConsultant = "QC Consultant";
        public const string RequestingUser = "RequestingUser";
        public const string HelpDeskConsultant = "HelpDeskConsultant";
        public const string PLSupervisorD = "PL Supervisor D";
        public const string RCSSupervisor = "RCSSupervisor";
        public const string RVManager = "RV Manager";
        public const string DebtCounsellingCourtConsultant = "Debt Counselling Court Consultant";
        public const string CreditUnderwriter = "Credit Underwriter";
        public const string FLProcessorD = "FL Processor D";
        public const string QCProcessorD = "QC Processor D";
        public const string RCSAdmin = "RCSAdmin";
        public const string PLCreditAnalyst = "PL Credit Analyst";
        public const string BranchAdminD = "Branch Admin D";
        public const string DebtCounsellingConsultant = "Debt Counselling Consultant";
        public const string RecoveriesManager = "Recoveries Manager";
        public const string LifeOrigination = "LifeOrigination";
        public const string BranchManager = "Branch Manager";
    }

    public class WorkflowActivities
    {
        public class PersonalLoans
        {
            public const string ReworkApplication = "Rework Application";
            public const string CalculateApplication = "Calculate Application";
            public const string ApplicationinOrder = "Application in Order";
            public const string Approve = "Approve";
            public const string Decline = "Decline";
            public const string DeclineFinalised = "Decline Finalised";
            public const string DisburseFunds = "Disburse Funds";
            public const string ReturntoManageApplication = "Return to Manage Application";
            public const string CreatePersonalLoanLead = "Create Personal Loan Lead";
            public const string DocumentsVerified = "Documents Verified";
            public const string NTU = "NTU";
            public const string ReinstateNTU = "Reinstate NTU";
            public const string NTUFinalised = "NTU Finalised";
            public const string NTUTimer = "NTU Timer";
            public const string SendDocuments = "Send Documents";
            public const string SendOffer = "Send Offer";
            public const string DisbursedTimer = "Disbursed Timer";
            public const string EXT_NTU = "EXT_NTU";
            public const string EmailDisbursedLetter = "Email Disbursed Letter";
            public const string EmailDisbursedLetterFailed = "Email Disbursed Letter Failed";
            public const string SendDisbursementLetter = "Send Disbursement Letter";
            public const string ReturntoLegalAgreements = "Return to Legal Agreements";
            public const string RollbackDisbursement = "Rollback Disbursement";
            public const string AdminDecline = "Admin Decline";
            public const string NTULead = "NTU Lead";
            public static string EscalateToExceptionsManager = "Escalate to Exceptions Manager";
            public const string DocumentCheck = "Document Check";
            public const string AlteredApproval = "Altered Approval";
            public const string EscalatetoExceptionsManager = "Escalate to Exceptions Manager";
            public const string ConfirmAffordabilityAssessment = "Confirm Affordability";
        }

        public class ApplicationManagement
        {
            public const string OverrideCheck = "Override Check";
            public const string NTUPipeLine = "NTU PipeLine";
            public const string EXTNTU = "EXT NTU";
            public const string NTUTimeout = "NTU Timeout";
            public const string RequestResolved = "Request Resolved";
            public const string CreateFollowup = "Create Followup";
            public const string QAComplete = "QA Complete";
            public const string ClientAccepts = "Client Accepts";
            public const string QAQuery = "QA Query";
            public const string Decline = "Decline";
            public const string NTU = "NTU";
            public const string NTUFinalised = "NTU Finalised";
            public const string QueryOnApplication = "Query on Application";
            public const string AssignedQA = "Assigned QA";
            public const string OtherTypes = "Other Types";
            public const string NewPurchase = "New Purchase";
            public const string ApplicationReceived = "Application Received";
            public const string ReturnWithApproveOrOffer = "Return with Approve or Offer";
            public const string NewBusiness = "New Business";
            public const string FeedbackonQuery = "Feedback on Query";
            public const string ApplicationinOrder = "Application in Order";
            public const string AssignAdmin = "Assign Admin";
            public const string ConfirmApplicationEmployment = "Confirm Application Employment";
            public const string BranchReworkApplication = "Branch Rework Application";
            public const string ClientRefuse = "Client Refuse";
            public const string CompleteFollowup = "Complete Followup";
            public const string ContinuewithFollowup = "Continue with Followup";
            public const string CreateAccountForApplication = "Create Account For Application";
            public const string CreateInstance = "Create Instance";
            public const string DeclineFinal = "Decline Final";
            public const string DeclineFinalised = "Decline Finalised";
            public const string DisbursementIncorrect = "Disbursement Incorrect";
            public const string DisputeFinalised = "Dispute Finalised";
            public const string FLReworkApplication = "FL Rework Application";
            public const string ForceDisbursementTimer = "Force Disbursement Timer";
            public const string FurtherLendingCalc = "Further Lending Calc";
            public const string HeldOver = "Held Over";
            public const string InfoRequestComplete = "Info Request Complete";
            public const string InstructAttorney = "Instruct Attorney";
            public const string InstructionFailed = "Instruction Failed";
            public const string LOAAccepted = "LOA Accepted";
            public const string LOAReceived = "LOA Received";
            public const string Motivate = "Motivate";
            public const string NoteComment = "Note Comment";
            public const string PerformFurtherValuation = "Perform Further Valuation";
            public const string PerformValuation = "Perform Valuation";
            public const string QueryComplete = "Query Complete";
            public const string QueryonApplication = "Query on Application";
            public const string QueryonLOA = "Query on LOA";
            public const string ReAssignCommissionConsultant = "ReAssign Commission Consultant";
            public const string ReassignUser = "Reassign User";
            public const string ReinstateDecline = "Reinstate Decline";
            public const string ReinstateFollowup = "Reinstate Followup";
            public const string ReinstateNTU = "Reinstate NTU";
            public const string ReinstructAttorney = "Reinstruct Attorney";
            public const string ReloadCaseName = "Reload CaseName";
            public const string RequestLightstoneValuation = "Request Lightstone Valuation";
            public const string RequireArrearComment = "Require Arrear Comment";
            public const string ResendInstruction = "Resend Instruction";
            public const string ResendLOA = "Resend LOA";
            public const string Resubmit = "Resubmit";
            public const string ResubmittoCredit = "Resubmit to Credit";
            public const string RetryCreditPreApproved = "Retry Credit Pre-Approved";
            public const string RetryDeclinebyCredit = "Retry Decline by Credit";
            public const string RetryDispute = "Retry Dispute";
            public const string RetryFLReturnCommon = "Retry FL Return Common";
            public const string RetryFurtherInfoRequest = "Retry Further Info Request";
            public const string RetryInstruction = "Retry Instruction";
            public const string RetryReturnApproveorOffer = "Retry Return Approve or Offer";
            public const string RetryReturnProcessorCredit = "Retry Return Processor Credit";
            public const string RetryReturnProcessorReAdv = "Retry Return Processor ReAdv";
            public const string RetryReturnReAdvPaymentFL = "Retry Return ReAdv Payment FL";
            public const string RetryReturnResubApprove = "Retry Return Resub Approve";
            public const string RetryValuationinOrder = "Retry Valuation in Order";
            public const string ReturntoManageApplication = "Return to Manage Application";
            public const string ReviewDisbursementSetup = "Review Disbursement Setup";
            public const string ReviewValuationRequired = "Review Valuation Required";
            public const string ReworkApplication = "Rework Application";
            public const string RollbackDisbursement = "Rollback Disbursement";
            public const string SelectAttorney = "Select Attorney";
            public const string SendAIP = "Send AIP";
            public const string SendLOA = "Send LOA";
            public const string TranslationComplete = "Translation Complete";
            public const string UpdateFollowup = "Update Followup";
            public const string ReworkFigures = "Rework Figures";
            public const string HelpDeskNTU = "HelpDesk NTU";
        }

        public class ApplicationCapture
        {
            public const string AppNotAcceptedFinalised = "App Not Accepted Finalised";
            public const string ApplicationNotAccepted = "Application Not Accepted";
            public const string Archive = "Archive";
            public const string ArchiveLead = "Archive Lead";
            public const string AssignAdmin = "Assign Admin";
            public const string AssignEstateAgent = "Assign EstateAgent";
            public const string AssignEstateAgentLead = "Assign EstateAgent Lead";
            public const string AssignConsultant = "AssignConsultant";
            public const string ContinueApplication = "Continue Application";
            public const string ContinuewithApplication = "Continue with Application";
            public const string CreateApplication = "Create Application";
            public const string CreateApplicationWizard = "Create Application Wizard";
            public const string CreateDirectApplication = "Create Direct Application";
            public const string CreateFollowup = "Create Followup";
            public const string CreateInstance = "Create Instance";
            public const string CreateLead = "Create Lead";
            public const string Decline = "Decline";
            public const string DeclineFinalised = "Decline Finalised";
            public const string EscalatetoManager = "Escalate to Manager";
            public const string EstateAgentAssignment = "Estate Agent Assignment";
            public const string ManagerArchive = "Manager Archive";
            public const string ManagerReassign = "Manager Reassign";
            public const string ManagerSubmitApplication = "Manager Submit Application";
            public const string ProcessInternetApplication = "Process Internet Application";
            public const string ProcessInternetLead = "Process Internet Lead";
            public const string ReactivateAppNotAccepted = "Reactivate App Not Accepted";
            public const string ReactivateDecline = "Reactivate Decline";
            public const string ReAssign = "ReAssign";
            public const string ReAssignCommissionConsultant = "ReAssign Commission Consultant";
            public const string RefreshApplicationTimeout = "Refresh Application Timeout";
            public const string ReinstateFollowup = "Reinstate Followup";
            public const string Retry = "Retry";
            public const string RetryAssignTeleConsultant = "RetryAssignTeleConsultant";
            public const string RetryInternetCreate = "RetryInternetCreate";
            public const string SelectEstateAgentorAgency = "Select Estate Agent or Agency";
            public const string SubmitApplication = "Submit Application";
            public const string UpdateEstateAgentorAgency = "Update Estate Agent or Agency";
            public const string UpdateFollowup = "Update Followup";
            public const string UpdateLoanDetails = "Update Loan Details";
            public const string IsEAApplication = "is EA Application";
            public const string FortyFiveDayTimer = "45 day timer";
            public const string ContactwithClient = "Contact with Client";
        }

        public class Valuations
        {
            public const string EscalatetoManager = "Escalate to Manager";
            public const string FurtherValuationRequired = "Further Valuation Required";
            public const string InstructEzValValuer = "Instruct Ez-Val Valuer";
            public const string ManagerArchive = "Manager Archive";
            public const string ManualValuation = "Manual Valuation";
            public const string Note = "Note";
            public const string PerformManualValuation = "Perform Manual Valuation";
            public const string ReinstructValuer = "Re-instruct Valuer";
            public const string ReassignUser = "Reassign User";
            public const string RequestAutomatedValuation = "Request Automated Valuation";
            public const string RequestLightstoneValuation = "Request Lightstone Valuation";
            public const string RequestValuationReview = "Request Valuation Review";
            public const string RetryFurtherVal = "Retry Further Val";
            public const string RetryReviewRequest = "Retry Review Request";
            public const string RetryValuationCreate = "Retry Valuation Create";
            public const string ReviewValuationRequired = "Review Valuation Required";
            public const string SetActiveValuation = "Set Active Valuation";
            public const string ValuationinOrder = "Valuation in Order";
        }

        public class Credit
        {
            public const string Agreewithdecision = "Agree with decision";
            public const string ApproveApplication = "Approve Application";
            public const string ApprovewithPricingChanges = "Approve with Pricing Changes";
            public const string DeclineApplication = "Decline Application";
            public const string DeclinewithOffer = "Decline with Offer";
            public const string DisputeIndicated = "Dispute Indicated";
            public const string EscalateToExceptionsMgr = "Escalate To Exceptions Mgr";
            public const string EscalatetoMgr = "Escalate to Mgr ";
            public const string ExceptionsDeclinewithOffer = "Exceptions Decline with Offer";
            public const string ExceptionsRateAdjustment = "Exceptions Rate Adjustment";
            public const string FeedbackonOverride = "Feedback on Override";
            public const string OverrideDecision = "Override Decision";
            public const string ReassignAnalyst = "Reassign Analyst";
            public const string ReferSeniorAnalyst = "Refer Senior Analyst";
            public const string RequestFurtherInfo = "Request Further Info";
            public const string RequestLightstoneValuation = "Request Lightstone Valuation";
            public const string RequestPolicyOverride = "Request Policy Override";
            public const string RetryFromOrigCreate = "Retry From Orig Create";
            public const string ReturntoProcessor = "Return to Processor";
            public const string ValuationApproved = "Valuation Approved";
            public const string ConfirmApplicationEmployment = "Confirm Application Employment";
            public const string NotResub = "Not Resub";
            public static string ConfirmAffordabilityAssessment = "Confirm Affordability";
        }

        public class DebtCounselling
        {
            public const string EscalateRecoveriesProposal = "Escalate Recoveries Proposal";
            public const string _45dayremindersent = "45 day reminder sent";
            public const string Accept = "Accept";
            public const string ApproveShortfall = "Approve Shortfall";
            public const string AttorneytoOppose = "Attorney to Oppose";
            public const string Bypass10DayTimer = "Bypass 10 Day Timer";
            public const string CancelDebtCounselling = "Cancel Debt Counselling";
            public const string CancelPLDebtCounselling = "Cancel PL Debt Counselling";
            public const string CaptureRecoveriesProposal = "Capture Recoveries Proposal";
            public const string ChangeinCircumstance = "Change in Circumstance";
            public const string ConsultantDecline = "Consultant Decline";
            public const string Continueanotherterm = "Continue another term";
            public const string ContinueDebtCounselling = "Continue Debt Counselling";
            public const string ContinueDebtReview = "Continue Debt Review";
            public const string ContinueLitigation = "Continue Litigation";
            public const string CourtDetails = "Court Details";
            public const string CourtOrderGranted = "Court Order Granted";
            public const string Decline = "Decline";
            public const string DeclineShortfall = "Decline Shortfall";
            public const string ExcludeBond = "Exclude Bond";
            public const string InadequatePayment = "Inadequate Payment";
            public const string NegotiateProposal = "Negotiate Proposal";
            public const string NotificationofDecision = "Notification of Decision";
            public const string NotifiedofDeath = "Notified of Death";
            public const string NotifiedofSequestration = "Notified of Sequestration";
            public const string OptOut = "Opt Out";
            public const string PaymentinOrder = "Payment in Order";
            public const string PaymentReceived = "Payment Received";
            public const string ReallocateUser = "Reallocate User";
            public const string RespondtoDebtCounsellor = "Respond to Debt Counsellor";
            public const string RetryTermination = "Retry Termination";
            public const string SendCounterProposal = "Send Counter Proposal";
            public const string SendDeclineLetter = "Send Decline Letter";
            public const string SendDocuments = "Send Documents";
            public const string SendIntenttoTerminate = "Send Intent to Terminate";
            public const string SendProposalforApproval = "Send Proposal for Approval";
            public const string SendTerminationLetter = "Send Termination Letter";
            public const string SendtoLitigation = "Send to Litigation";
            public const string SignedDocumentsReceived = "Signed Documents Received";
            public const string TerminateApplication = "Terminate Application";
            public const string UpdateCaseDetails = "Update Case Details";
            public const string EXTUnderCancellation = "EXT Under Cancellation";
            public const string BondExclusionsArrears = "Bond Exclusions Arrears";
            public const string SixtyDayTimer = "60 Days";
            public const string EXTIntoArrears = "EXT Into Arrears";
            public const string TermExpired = "Term Expired";
            public const string TenDayTimer = "10 Days";
            public const string FiveDayTimer = "5Days";
            public const string EXT_60daynodateorpay = "EXT_60daynodateorpay";
            public const string FortyFiveDayTimer = "45 Days";
            public const string EXTOutofArrears = "EXT Out of Arrears";
            public const string EXTCancellationRegistered = "EXT Cancellation Registered";
            public const string CourtApplicationWithdrawn = "Court Application Withdrawn";
        }

        public class Cap2Offers
        {
            public const string CancelCAP2Offer = "Cancel CAP2 Offer";
            public const string CashPayment = "Cash Payment";
            public const string CashPaymentVerified = "Cash Payment Verified";
            public const string ChangePaymentOption = "Change Payment Option";
            public const string ConfirmCancellation = "Confirm Cancellation";
            public const string ContinueSale = "Continue Sale";
            public const string ContinuewithSale = "Continue with Sale";
            public const string CreateCallback = "Create Callback";
            public const string CreateCAP2lead = "Create CAP2 lead";
            public const string CreditApproval = "Credit Approval";
            public const string DeclineCAP2 = "Decline CAP2";
            public const string FormsSent = "Forms Sent";
            public const string FurtherAdvanceDecision = "Further Advance Decision";
            public const string GrantCAP2Offer = "Grant CAP2 Offer";
            public const string LASent = "LA Sent";
            public const string NTUCAP2Offer = "NTU CAP2 Offer";
            public const string PrintCapLetter = "Print Cap Letter";
            public const string PromotionClient = "Promotion Client";
            public const string ReadvanceDone = "Readvance Done";
            public const string ReadvanceRequired = "Readvance Required";
            public const string ReadyForReadvance = "Ready For Readvance";
            public const string RecalculateCAP2 = "Recalculate CAP2";
            public const string ReworkOffer = "Rework Offer";
        }

        public class ReadvancePayments
        {
            public const string _12HourOverride = "12 Hour Override";
            public const string ApproveRapid = "Approve Rapid";
            public const string CancelOptOutRequest = "Cancel Opt-Out Request";
            public const string CompleteFollowup = "Complete Followup";
            public const string ContinuewithFollowup = "Continue with Followup";
            public const string CreateFollowup = "Create Followup";
            public const string Decline = "Decline";
            public const string DeclineFinal = "Decline Final";
            public const string DisbursementComplete = "Disbursement Complete";
            public const string DisbursementIncorrect = "Disbursement Incorrect";
            public const string InformClient = "Inform Client";
            public const string NTU = "NTU";
            public const string NTUFinal = "NTU Final";
            public const string OptOutSuperLo = "Opt-Out SuperLo";
            public const string PaymentPrepared = "Payment Prepared";
            public const string ReassignUser = "Reassign User";
            public const string ReceiveSchedule = "Receive Schedule";
            public const string RefertoCredit = "Refer to Credit";
            public const string ReinstateDecline = "Reinstate Decline";
            public const string ReinstateFollowup = "Reinstate Followup";
            public const string ReinstateNTU = "Reinstate NTU";
            public const string RequestLightstoneValuation = "Request Lightstone Valuation";
            public const string RetryApproFLandFA = "Retry Appro FL and FA";
            public const string RetryDeclinebyCredit = "Retry Decline by Credit";
            public const string RetryPostCredCreate = "Retry Post Cred Create";
            public const string RetryReadvancePayment = "Retry Readvance Payment";
            public const string ReturntoManageApplication = "Return to Manage Application";
            public const string RollbackDisbursement = "Rollback Disbursement";
            public const string SendSchedule = "Send Schedule";
            public const string SuretyshipSigned = "Suretyship Signed";
            public const string UpdateFollowup = "Update Followup";
        }

        public class LoanAdjustments
        {
            public const string AgreeWithDecision = "Agree With Decision";
            public const string ApproveTermChange = "Approve Term Change";
            public const string DeclineTermChange = "Decline Term Change";
            public const string DisagreeWithDecision = "Disagree With Decision";
            public const string NoLongerRequired = "No Longer Required";
        }

        public class LifeOrigination
        {
            public const string AcceptBenefits = "Accept Benefits";
            public const string AcceptDeclaration = "Accept Declaration";
            public const string AcceptExclusions = "Accept Exclusions";
            public const string AcceptFAIS = "Accept FAIS";
            public const string AcceptQuote = "Accept Quote";
            public const string AcceptRPAR = "Accept RPAR";
            public const string ChangePolicyType = "Change Policy Type";
            public const string ConfirmDeclaration = "Confirm Declaration";
            public const string ConfirmDetails = "Confirm Details";
            public const string ConfirmExclusions = "Confirm Exclusions";
            public const string ConfirmFAIS = "Confirm FAIS";
            public const string ConfirmLOA = "Confirm LOA";
            public const string ConfirmQuote = "Confirm Quote";
            public const string ContinueSale = "Continue Sale";
            public const string ContinuewithSale = "Continue with Sale";
            public const string CreateCallback = "Create Callback";
            public const string CreateInstance = "Create Instance";
            public const string DeclineQuote = "Decline Quote";
            public const string NTUPolicy = "NTU Policy";
            public const string ReactivateNTUdPolicy = "Reactivate NTUd Policy";
            public const string SendPolicyDocument = "Send Policy Document";
            public const string SendQuote = "Send Quote";
        }

        public class HelpDesk
        {
            public const string CreateRequest = "Create Request";
            public const string Proceed = "Proceed";
            public const string InvalidClient = "Invalid Client";
            public const string CompleteRequest = "Complete Request";
            public const string RouteToConsultant = "Route to Consultant";
            public const string AutoArchive = "AutoArchive";
        }

        public class DisabilityClaim
        {
            public const string Approve = "Approve";
            public const string Repudiate = "Repudiate";
            public const string CreateDisabilityClaim = "Create Disability Claim";
            public const string CaptureDetails = "Capture Details";
            public const string EXTClaimSettled = "EXT Claim Settled";
            public const string Terminate = "Terminate";
            public const string EXTClaimTerminated = "EXT Claim Terminated";
            public const string SendApprovalLetter = "Send Approval Letter";
        }
    }

    public class EzValTestDataIDs
    {
        public const string CompletedValuation = "CompletedValuation";
        public const string RejectedValuation = "RejectedValuation";
        public const string CompletedValuation2 = "CompletedValuation2";
    }

    public class RateAdjustmentElements
    {
        public const string PricingForRisk = "Emprica v4 - Pricing for Risk (575-595)";

        public const string CM54_Category_0_Salaried_Emp_575_595 = "CM54 Category 0 Salaried [Emp 575-595]";
        public const string CM54_Category_0_Salaried_LAA_R2750000 = "CM54 Category 0 Salaried [LAA > R2,750,000]";
        public const string CM54_Category_0_Salaried_Emp_575_600_Capitec = "CM54 Category 0 Salaried [Emp 575-600 Capitec]";
        public const string CM54_Category_1_Salaried_Emp_575_595 = "CM54 Category 1 Salaried [Emp 575-595]";
        public const string CM54_Category_1_Salaried_Emp_575_600_Capitec = "CM54 Category 1 Salaried [Emp 575-600 Capitec]";
        public const string CM54_Category_2_Salaried_Emp_575_594 = "CM54 Category 2 Salaried [Emp 575-594]";
        public const string CM54_Category_2_Salaried_Emp_595_609 = "CM54 Category 2 Salaried [Emp 595-609]";
        public const string CM54_Category_2_Salaried_Emp_610_629 = "CM54 Category 2 Salaried [Emp 610-629]";
        public const string CM54_Category_3_Salaried_Emp_595_629 = "CM54 Category 3 Salaried [Emp 595-629]";
        public const string CM54_Category_4_Salaried_Emp_595_629 = "CM54 Category 4 Salaried [Emp 595-629]";
        public const string CM54_Category_6_Salaried_Emp_575_594 = "CM54 Category 6 Salaried [Emp 575-594]";
        public const string CM54_Category_6_Salaried_Emp_595_609 = "CM54 Category 6 Salaried [Emp 595-609]";
        public const string CM54_Category_6_Salaried_Emp_610_629 = "CM54 Category 6 Salaried [Emp 610-629]";
        public const string CM54_Category_6_Salaried_Emp_630_649 = "CM54 Category 6 Salaried [Emp 630-649]";
        public const string CM54_Category_7_Salaried_Emp_575_594 = "CM54 Category 7 Salaried [Emp 575-594]";
        public const string CM54_Category_7_Salaried_Emp_595_609 = "CM54 Category 7 Salaried [Emp 595-609]";
        public const string CM54_Category_7_Salaried_Emp_610_629 = "CM54 Category 7 Salaried [Emp 610-629]";
        public const string CM54_Category_7_Salaried_Emp_630_649 = "CM54 Category 7 Salaried [Emp 630-649]";
        public const string CM54_Category_8_Salaried_Emp_575_594 = "CM54 Category 8 Salaried [Emp 575-594]";
        public const string CM54_Category_8_Salaried_Emp_595_609 = "CM54 Category 8 Salaried [Emp 595-609]";
        public const string CM54_Category_8_Salaried_Emp_610_629 = "CM54 Category 8 Salaried [Emp 610-629]";
        public const string CM54_Category_8_Salaried_Emp_630_649 = "CM54 Category 8 Salaried [Emp 630-649]";
        public const string CM54_Category_9_Salaried_Emp_575_594 = "CM54 Category 9 Salaried [Emp 575-594]";
        public const string CM54_Category_9_Salaried_Emp_595_609 = "CM54 Category 9 Salaried [Emp 595-609]";
        public const string CM54_Category_9_Salaried_Emp_610_629 = "CM54 Category 9 Salaried [Emp 610-629]";
        public const string CM54_Category_9_Salaried_Emp_630_649 = "CM54 Category 9 Salaried [Emp 630-649]";
        public const string CM54_Category_11_Salaried_Emp_640_649 = "CM54 Category 11 Salaried [Emp 640-649]";
        public const string CM54_Category_0_Self_Employed_LAA_R1800000 = "CM54 Category 0 Self Employed [LAA > R1,800,000]";
        public const string CM54_Category_1_Self_Employed_LAA_R1800000 = "CM54 Category 1 Self Employed [LAA > R1,800,000]";
        public const string CM54_Category_0_SWD_Emp_575_595 = "CM54 Category 0 SWD [Emp 575-595]";
        public const string CM54_Category_1_SWD_Emp_575_600 = "CM54 Category 1 SWD [Emp 575-600]";
        public const string CM54_Category_2_SWD_Emp_575_594 = "CM54 Category 2 SWD [Emp 575-594]";
        public const string CM54_Category_2_SWD_Emp_595_609 = "CM54 Category 2 SWD [Emp 595-609]";
        public const string CM54_Category_2_SWD_Emp_610_629 = "CM54 Category 2 SWD [Emp 610-629]";
        public const string CM54_Category_3_SWD_Emp_575_600 = "CM54 Category 3 SWD [Emp 575-600]";
        public const string CM54_Category_4_SWD_Emp_575_600 = "CM54 Category 4 SWD [Emp 575-600]";
        public const string CM54_Category_5_SWD_Emp_575_600 = "CM54 Category 5 SWD [Emp 575-600]";
        public const string CM54_Category_6_SWD_Emp_575_594 = "CM54 Category 6 SWD [Emp 575-594]";
        public const string CM54_Category_6_SWD_Emp_595_609 = "CM54 Category 6 SWD [Emp 595-609]";
        public const string CM54_Category_6_SWD_Emp_610_629 = "CM54 Category 6 SWD [Emp 610-629]";
        public const string CM54_Category_6_SWD_Emp_630_649 = "CM54 Category 6 SWD [Emp 630-649]";
        public const string CM54_Category_7_SWD_Emp_575_594 = "CM54 Category 7 SWD [Emp 575-594]";
        public const string CM54_Category_7_SWD_Emp_595_609 = "CM54 Category 7 SWD [Emp 595-609]";
        public const string CM54_Category_7_SWD_Emp_610_629 = "CM54 Category 7 SWD [Emp 610-629]";
        public const string CM54_Category_7_SWD_Emp_630_649 = "CM54 Category 7 SWD [Emp 630-649]";
        public const string CM54_Category_8_SWD_Emp_575_594 = "CM54 Category 8 SWD [Emp 575-594]";
        public const string CM54_Category_8_SWD_Emp_595_609 = "CM54 Category 8 SWD [Emp 595-609]";
        public const string CM54_Category_8_SWD_Emp_610_629 = "CM54 Category 8 SWD [Emp 610-629]";
        public const string CM54_Category_8_SWD_Emp_630_649 = "CM54 Category 8 SWD [Emp 630-649]";
        public const string CM54_Category_9_SWD_Emp_575_594 = "CM54 Category 9 SWD [Emp 575-594]";
        public const string CM54_Category_9_SWD_Emp_595_609 = "CM54 Category 9 SWD [Emp 595-609]";
        public const string CM54_Category_9_SWD_Emp_610_629 = "CM54 Category 9 SWD [Emp 610-629]";
        public const string CM54_Category_9_SWD_Emp_630_649 = "CM54 Category 9 SWD [Emp 630-649]";
        public const string CM54_Category_10_SWD_Emp_595_600 = "CM54 Category 10 SWD [Emp 595-600]";
        public const string CM54_Category_12_SWD_Emp_575_594 = "CM54 Category 12 SWD [Emp 575-594]";
        public const string CM54_Category_13_SWD_Emp_575_594 = "CM54 Category 13 SWD [Emp 575-594]";
        public const string CM54_Category_13_SWD_Emp_595_629 = "CM54 Category 13 SWD [Emp 595-629]";
        public const string CM54_Category_14_SWD_Emp_575_594 = "CM54 Category 14 SWD [Emp 575-594]";
        public const string CM54_Category_14_SWD_Emp_595_609 = "CM54 Category 14 SWD [Emp 595-609]";
        public const string CM54_Category_14_SWD_Emp_610_629 = "CM54 Category 14 SWD [Emp 610-629]";
        public const string CM54_Category_14_SWD_Emp_630_649 = "CM54 Category 14 SWD [Emp 630-649]";
        public const string CM54_Category_15_SWD_Emp_575_594 = "CM54 Category 15 SWD [Emp 575-594]";
        public const string CM54_Category_15_SWD_Emp_595_609 = "CM54 Category 15 SWD [Emp 595-609]";
        public const string CM54_Category_15_SWD_Emp_610_629 = "CM54 Category 15 SWD [Emp 610-629]";
        public const string CM54_Category_15_SWD_Emp_630_649 = "CM54 Category 15 SWD [Emp 630-649]";
        public const string CM54_Category_16_SWD_Emp_575_594 = "CM54 Category 16 SWD [Emp 575-594]";
        public const string CM54_Category_16_SWD_Emp_595_609 = "CM54 Category 16 SWD [Emp 595-609]";
        public const string CM54_Category_16_SWD_Emp_610_629 = "CM54 Category 16 SWD [Emp 610-629]";
        public const string CM54_Category_16_SWD_Emp_630_649 = "CM54 Category 16 SWD [Emp 630-649]";
        public const string CM54_Category_17_SWD_Emp_575_594 = "CM54 Category 17 SWD [Emp 575-594]";
        public const string CM54_Category_17_SWD_Emp_595_609 = "CM54 Category 17 SWD [Emp 595-609]";
        public const string CM54_Category_17_SWD_Emp_610_629 = "CM54 Category 17 SWD [Emp 610-629]";
        public const string CM54_Category_17_SWD_Emp_630_649 = "CM54 Category 17 SWD [Emp 630-649]";
        public const string CM54_Category_18_SWD_Emp_575_594 = "CM54 Category 18 SWD [Emp 575-594]";
        public const string CM54_Category_18_SWD_Emp_595_629 = "CM54 Category 18 SWD [Emp 595-629]";
        public const string CM54_Category_19_SWD_Emp_575_594 = "CM54 Category 19 SWD [Emp 575-594]";
        public const string CM54_Category_19_SWD_Emp_595_609 = "CM54 Category 19 SWD [Emp 595-609]";
        public const string CM54_Category_19_SWD_Emp_610_629 = "CM54 Category 19 SWD [Emp 610-629]";
        public const string CM54_Category_19_SWD_Emp_630_649 = "CM54 Category 19 SWD [Emp 630-649]";
        public const string CM54_Category_20_SWD_Emp_575_594 = "CM54 Category 20 SWD [Emp 575-594]";
        public const string CM54_Category_20_SWD_Emp_595_609 = "CM54 Category 20 SWD [Emp 595-609]";
        public const string CM54_Category_20_SWD_Emp_610_629 = "CM54 Category 20 SWD [Emp 610-629]";
        public const string CM54_Category_20_SWD_Emp_630_649 = "CM54 Category 20 SWD [Emp 630-649]";
        public const string CM54_Category_21_SWD_Emp_575_594 = "CM54 Category 21 SWD [Emp 575-594]";
        public const string CM54_Category_21_SWD_Emp_595_609 = "CM54 Category 21 SWD [Emp 595-609]";
        public const string CM54_Category_21_SWD_Emp_610_629 = "CM54 Category 21 SWD [Emp 610-629]";
        public const string CM54_Category_21_SWD_Emp_630_649 = "CM54 Category 21 SWD [Emp 630-649]";
        public const string CM54_Category_22_SWD_Emp_575_594 = "CM54 Category 22 SWD [Emp 575-594]";
        public const string CM54_Category_22_SWD_Emp_595_609 = "CM54 Category 22 SWD [Emp 595-609]";
        public const string CM54_Category_22_SWD_Emp_610_629 = "CM54 Category 22 SWD [Emp 610-629]";
        public const string CM54_Category_22_SWD_Emp_630_649 = "CM54 Category 22 SWD [Emp 630-649]";
    }

    public class WorkflowAutomationScripts
    {
        public class DebtCounselling
        {
            public const string CaseCreate = "CaseCreate";
            public const string RespondToDebtCounsellor = "RespondToDebtCounsellor";
            public const string NegotiateProposal = "NegotiateProposal";
            public const string SendProposalForApproval = "SendProposalForApproval";
            public const string SendProposalForApprovalWithExistingProposal = "SendProposalForApprovalWithExistingProposal";
            public const string SendProposalForApprovalAssignToManager = "SendProposalForApprovalAssignToManager";
            public const string AcceptProposal = "AcceptProposal";
            public const string AcceptProposalAsManager = "AcceptProposalAsManager";
            public const string SignedDocsReceived = "SignedDocsReceived";
            public const string NotificationOfDecision = "NotificationOfDecision";
            public const string PaymentReceived = "PaymentReceived";
            public const string PaymentInOrder = "PaymentInOrder";
            public const string EXTUnderCancellation = "EXTUnderCancellation";
            public const string CaptureRecoveriesProposal = "CaptureRecoveriesProposal";
            public const string EscalateRecoveriesProposal = "EscalateRecoveriesProposal";
            public const string SendToLitigation = "SendToLitigation";
            public const string BondExclusionsArrears = "BondExclusionsArrears";
            public const string NotificationOfSequestration = "NotificationOfSequestration";
            public const string Fire60DayTimer = "Fire60DayTimer";
            public const string TerminateApplication = "TerminateApplication";
            public const string SendTerminationLetter = "SendTerminationLetter";
            public const string EXTIntoArrears = "EXTIntoArrears";
            public const string FireTermExpiredTimer = "FireTermExpiredTimer";
            public const string CaptureCourtDetailsToAttorneyToOppose = "CaptureCourtDetailsToAttorneyToOppose";
            public const string Fire10DaysTimer = "Fire10DaysTimer";
            public const string ExcludeBond = "ExcludeBond";
            public const string Fire5DaysTimer = "Fire5DaysTimer";
            public const string RaiseEXT60DateNoDateOrPayment = "RaiseEXT60DateNoDateOrPayment";
            public const string Fire45DaysTimer = "Fire45DaysTimer";
            public const string DeclineProposal = "DeclineProposal";
            public const string DeclineProposalCourtOrderWithAppeal = "DeclineProposalCourtOrderWithAppeal";
            public const string ApproveShortfall = "ApproveShortfall";
            public const string EXTCancellationRegistered = "EXTCancellationRegistered";
            public const string ChangeInCircumstance = "ChangeInCircumstance";
            public const string Bypass10DayTimer = "Bypass10DayTimer";
        }

        public class FurtherLending
        {
            public const string ApplicationReceived = "ApplicationReceived";
            public const string QACompleteFL = "QACompleteFL";
            public const string Fire2MonthsTimer = "Fire2MonthsTimer";
            public const string FurtherLendingToCredit = "FurtherLendingToCredit";
            public const string ApplicationInOrder = "ApplicationInOrder";
        }

        public class PersonalLoans
        {
            public const string FireDisbursementTimer = "FireDisbursementTimer";
            public const string CalculateApplicationToDisbursementWithNoLifeCover = "CalculateApplicationToDisbursementWithNoLifeCover";
        }

        public class ApplicationCapture
        {
            public const string EXTCreateCapitecInstance = "EXT Create Capitec Instance";
            public const string EscalateToManager = "EscalateToManager";
            public const string SubmitApplicationNoCleanup = "SubmitApplicationNoCleanup";
            public const string SubmitApplication = "SubmitApplication";
            public const string FireArchiveApplicationTimer = "FireArchiveApplicationTimer";
            public const string FireDeclineTimeoutTimer = "FireDeclineTimeoutTimer";
            public const string FireWaitForFollowupTimer = "FireWaitForFollowupTimer";
            public const string Fire45DayTimer = "Fire45DayTimer";
        }

        public class ReadvancePayments
        {
            public const string FireNTUTimeoutTimer = "FireNTUTimeoutTimer";
            public const string FireDeclineTimeoutTimer = "FireDeclineTimeoutTimer";
            public const string OverrideTimer = "OverrideTimer";
            public const string ApproveRapid = "ApproveRapid";
            public const string OnFollowupTimer = "OnFollowupTimer";
            public const string TwelveHourOverride = "TwelveHourOverride";
        }

        public class ApplicationManagement
        {
            public const string NTUPipeline = "NTUPipeline";
            public const string FireNTUTimeoutTimer = "FireNTUTimeoutTimer";
            public const string QueryOnApplication = "QueryOnApplication";
            public const string FeedbackOnQuery = "FeedbackOnQuery";
            public const string NTU = "NTU";
            public const string TwoMonthQAQueryTimer = "TwoMonthQAQueryTimer";
            public const string FireArchivedCompletedFollowupTimer = "FireArchivedCompletedFollowupTimer";
            public const string FireOnFollowupTimer = "FireOnFollowupTimer";
            public const string LOAReceived = "LOA Received";
            public const string QueryOnLOA = "Query on LOA";
            public const string FireDeclineTimeoutTimer = "FireDeclineTimeoutTimer";
            public const string Decline = "Decline";
        }

        public class Valuations
        {
            public const string EscalateToManager = "Escalate to Manager";
            public const string ManagerArchive = "Manager Archive";
            public const string ValuationinOrder = "Valuation in Order";
            public const string InstructEzValValuer = "Instruct Ez-Val Valuer";
            public const string RenstructValuer = "Re-instruct Valuer";
            public const string RequestValuationReview = "Request Valuation Review";
            public const string ReviewValuationRequired = "Review Valuation Required";
            public const string FurtherValuationRequired = "Further Valuation Required";
            public const string PerformManualValuation = "PerformManualValuation";
        }

        public class Credit
        {
            public const string ApproveApplication = "ApproveApplication";
            public const string ConfirmApplicationEmployment = " ConfirmApplicationEmployment";
            public static string ApproveWithPricingChanges = "ApproveWithPricingChanges";
            public static string DeclineWithOffer = "DeclineWithOffer";
            public static string DeclineApplication = "DeclineApplication";
            public static string ExceptionsDeclinewithOffer = "ExceptionsDeclinewithOffer";
            public static string ExceptionsRateAdjustment = "ExceptionsRateAdjustment";
        }

        public class HelpDesk
        {
            public const string CreateCaseToRequestComplete = "CreateCaseToRequestComplete";
        }

        public class Cap2Offers
        {
            public const string FormsSent = "FormsSent";
            public const string CAPCaseCreate = "CAPCaseCreate";
            public const string CAP2OfferGranted = "CAP2OfferGranted";
            public const string FurtherAdvanceCAPToCredit = "FurtherAdvanceCAPToCredit";
            public const string CompletedExpiredTimer = "CompletedExpiredTimer";
            public const string NTUOfferTimer = "NTUOfferTimer";
            public const string DeclinedTimer = "DeclinedTimer";
            public const string OfferExpiredTimer = "OfferExpiredTimer";
            public const string ReadvanceDone = "ReadvanceDone";
            public const string WaitForCallbackTimer = "WaitForCallbackTimer";
        }

        public class LifeOrigination
        {
            public const string _45DayTimeout = "_45DayTimeout";
            public const string WaitforCallback = "WaitforCallback";
            public const string ArchiveNTU = "ArchiveNTU";
            public const string CreateInstance = "Create Instance";
        }

        public class LoanAdjustments
        {
            public const string TermRequestTimeout = "TermRequestTimeout";
        }
    }

    public class FurtherLendingTestCases
    {
        public const string ReadvanceCreate1 = "ReadvanceCreate1";
        public const string ReadvanceCreate2 = "ReadvanceCreate2";
        public const string ReadvanceCreate3 = "ReadvanceCreate3";
        public const string ReadvanceCreate4 = "ReadvanceCreate4";
        public const string ReadvanceCreate5 = "ReadvanceCreate5";
        public const string ReadvanceCreate6 = "ReadvanceCreate6";
        public const string ReadvanceCreate7 = "ReadvanceCreate7";
        public const string ReadvanceCreate8 = "ReadvanceCreate8";
        public const string ReadvanceCreate9 = "ReadvanceCreate9";
        public const string ReadvanceCreate10 = "ReadvanceCreate10";
        public const string FurtherAdvanceCreate1 = "FurtherAdvanceCreate1";
        public const string FurtherAdvanceCreate2 = "FurtherAdvanceCreate2";
        public const string FurtherAdvanceCreate3 = "FurtherAdvanceCreate3";
        public const string FurtherAdvanceCreate4 = "FurtherAdvanceCreate4";
        public const string FurtherAdvanceCreate5 = "FurtherAdvanceCreate5";
        public const string FurtherAdvanceValRequired = "FurtherAdvanceValRequired";
        public const string FurtherLoanValRequired = "FurtherLoanValRequired";
        public const string FurtherLoanCreate1 = "FurtherLoanCreate1";
        public const string FurtherLoanCreate2 = "FurtherLoanCreate2";
        public const string FurtherLoanCreate3 = "FurtherLoanCreate3";
        public const string FurtherLoanCreate4 = "FurtherLoanCreate4";
        public const string FurtherLoanCreate5 = "FurtherLoanCreate5";
        public const string ReadvanceAndFAdvCreate1 = "ReadvanceAndFAdvCreate1";
        public const string ReadvFAdvAndFLCreate1 = "ReadvFAdvAndFLCreate1";
        public const string ReadvFAdvAndFLCreate2 = "ReadvFAdvAndFLCreate2";
        public const string ReadvanceOver80Percent = "ReadvanceOver80Percent";
        public const string FurtherAdvanceLessThanLAA = "FurtherAdvanceLessThanLAA";
        public const string ReadvanceLessThanLAA = "ReadvanceLessThanLAA";
        public const string SuperLoNoOptOut1 = "SuperLoNoOptOut1";
        public const string SuperLoNoOptOut2 = "SuperLoNoOptOut2";
        public const string SuperLoSPVChange1 = "SuperLoSPVChange1";
        public const string SuperLoGreaterThan85Percent1 = "SuperLoGreaterThan85Percent1";
        public const string SuperLoGreaterThan85Percent2 = "SuperLoGreaterThan85Percent2";
        public const string LAAExceeded = "LAAExceeded";
        public const string Basel2HighRisk = "Basel2HighRisk";
        public const string Basel2ModerateRiskLowerBound = "Basel2ModerateRiskLowerBound";
        public const string Basel2ModerateRiskUpperBound = "Basel2ModerateRiskUpperBound";
        public const string Basel2LowRisk = "Basel2LowRisk";
        public const string ReturningCustomerDiscountNotApplied = "ReturningCustomerDiscountNotApplied";
    }

    public class ClaimType
    {
        public const string DeathClaim = "Death Claim";
        public const string DisabilityClaim = "Disability Claim";
        public const string RetrenchmentClaim = "Retrenchment Claim";
    }

    public class ClaimStatus
    {
        public const string Pending = "Pending";
        public const string Settled = "Settled";
        public const string Repudiated = "Repudiated";
        public const string Invalid = "Invalid";
    }

    public class X2DataTable
    {
        public const string ReadvancePayments = "x2.x2data.readvance_payments";
        public const string IT = "x2.x2data.IT";
        public const string DebtCounselling = "x2.x2data.debt_counselling";
        public const string PersonalLoans = "x2.x2data.personal_loans";
        public const string ApplicationCapture = "x2.x2data.application_capture";
        public const string ApplicationManagement = "x2.x2data.application_management";
        public const string Cap2 = "x2.x2data.cap2_offers";
        public const string Credit = "x2.x2data.credit";
        public const string LoanAdjustments = "x2.x2data.loan_adjustments";
        public const string Valuations = "x2.x2data.valuations";
        public const string HelpDesk = "x2.x2data.help_desk";
        public const string LifeOrigination = "x2.x2data.LifeOrigination";
        public const string DisabilityClaim = "x2.x2data.Disability_Claim";
    }

    public class EzValTemplates
    {
        public const string EzVal = @"Templates\EasyVal.xml";
        public const string EzVal_Invalid = @"Templates\EasyVal_Invalid.xml";

        public const string InstructEzValUnvalidated = @"Templates\InstructEzValUnvalidated.xml";
        public const string InstructEzValValidated = @"Templates\InstructEzValValidated.xml";

        public const string SubmitCompletedValuationLightstone = @"Templates\SubmitCompletedValuationLightstone.xml";
        public const string SubmitRejectedValuationLightstone = @"Templates\SubmitRejectedValuationLightstone.xml";
        public const string SubmitAmendedValuationLightstone = @"Templates\SubmitAmendedValuationLightstone.xml";

        public const string NewPhysicalInstruction = @"Templates\NewPhysicalInstruction.xml";
        public const string NewPhysicalInstructionUnvalidated = @"Templates\NewPhysicalInstructionUnvalidated.xml";
    }

    public class Insurer
    {
        public const string SAHLLife = "SAHL Life";
        public const string OldMutual = "Old Mutual";
        public const string Sanlam = "Sanlam";
        public const string ABSALife = "ABSA Life";
        public const string AfricanLife = "African Life";
        public const string Aegis = "Aegis";
        public const string AIG = "AIG";
        public const string AnchorLifeChannelLife = "Anchor Life/Channel Life";
        public const string AvbobLife = "Avbob Life";
        public const string BoELifeNedLife = "BoE Life/NedLife";
        public const string CapitalAllianceAALifeAGA = "Capital Alliance/AA Life/AGA";
        public const string CharterLife = "Charter Life";
        public const string ClienteleLife = "Clientele Life";
        public const string CommercialUnion = "Commercial Union";
        public const string DiscoveryLife = "Discovery Life";
        public const string FNBLife = "FNB Life";
        public const string Hollard = "Hollard";
        public const string KGALife = "KGA Life";
        public const string LibertyLife = "Liberty Life";
        public const string MetropolitanLife = "Metropolitan Life";
        public const string MomentumLife = "Momentum Life";
        public const string PPS = "PPS";
        public const string Rentmeester = "Rentmeester";
        public const string SageLife = "Sage Life";
        public const string Samib = "Samib";
        public const string StandardBankLife = "Standard Bank Life";
        public const string RegentLife = "Regent Life";
        public const string Other = "Other";
        public const string CovisionLife = "Covision Life";
    }

    public class lifePolicyStatus
    {
        public const string Prospect = "Prospect";
        public const string Accepted = "Accepted";
        public const string Inforce = "Inforce";
        public const string CancelledfromInception = "Cancelled from Inception";
        public const string CancelledwithProrata = "Cancelled with Prorata";
        public const string Lapsed = "Lapsed";
        public const string Closed = "Closed";
        public const string Acceptedtocommenceon1st = "Accepted - to commence on 1st";
        public const string ClosedSystemError = "Closed - System Error";
        public const string CancelledNoRefund = "Cancelled – No Refund";
        public const string NotTakenUp = "Not Taken Up";
        public const string ExternalInsurer = "External Insurer";
        public const string LapsePending = "Lapse Pending";
    }

    public class Passwords
    {
        public const string HashedVersionofNatal1 = "bP3Cqj2mnJXc7KUNlns1VsHTHz8=";
    }

    public class ControlNumeric
    {
        public const float ReturningMainApplicantInitiationFeeDiscount = 0.5f;
        public const float ReturningMainApplicantInitiationFeeDiscount2 = 0.25f;
        public const string DiscountedInitiationFeeDateSwitch = "2015/1/1 00:00:00";
        public const float InitiationFee = 5700.0f;
    }

    public class DisabilityClaimRepudiationReasons
    {
        public const string Alcohol = "Alcohol";
        public const string Fibromyalgia = "Fibromyalgia";
        public const string SpinalConditions = "Spinal Conditions";
        public const string PolicyLapse = "Policy has lapsed";
        public const string ChronicFatigueSyndrome = "Chronic Fatigue Syndrome";
        public const string PreExistingMedicalConditions = "Pre Existing Medical Conditions";
        public const string SelfInflictedInjuryOrSuicideAttempt = "Self inflicted injury or suicide attempt";
    }

    public class DisabilityClaimTerminationReasons
    {
        public const string ClientReturnedToWork = "Client Returned to Work";
        public const string DeathClaimReceived = "Death Claim Received";
    }
}