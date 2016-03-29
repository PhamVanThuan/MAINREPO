using WatiN.Core;
using System.Text.RegularExpressions;

namespace ObjectMaps.NavigationControls
{
    public abstract class ApplicationManagementWorkflowActionControls : BaseNavigation
    {

        //Application Management workflow node
        protected Link CaseNode(string WorkflowState)
        {
            return base.Document.Link(Find.ByTitle(new Regex(@"^[\x20-\x7E]* \(Stage\: " + WorkflowState + @"\)$")));

        }

        protected bool ActionExists(string actionName)
        {
            return base.Document.Link(Find.ByTitle(actionName)).Exists;
        }

        [FindBy(Title = "Application in Order")]
        protected Link ApplicationinOrder { get; set; }

        [FindBy(Title = "Application Received")]
        protected Link ApplicationReceived { get; set; }

        [FindBy(Title = "Assign Admin")]
        protected Link AssignAdmin { get; set; }

        [FindBy(Title = "Branch Rework Application")]
        protected Link BranchReworkApplication { get; set; }

        [FindBy(Title = "Client Accepts")]
        protected Link ClientAccepts { get; set; }

        [FindBy(Title = "Client Refuse")]
        protected Link ClientRefuse { get; set; }

        [FindBy(Title = "Complete Followup")]
        protected Link CompleteFollowup { get; set; }        

        [FindBy(Title = "Continue with Followup")]
        protected Link ContinuewithFollowup { get; set; }

        [FindBy(Title = "Create Account For Application")]
        protected Link CreateAccountForApplication { get; set; }

        [FindBy(Title = "Create Followup")]
        protected Link CreateFollowup { get; set; }

        [FindBy(Title = "Create Instance")]
        protected Link CreateInstance { get; set; }

        [FindBy(Title = "Decline")]
        protected Link Decline { get; set; }

        [FindBy(Title = "Decline Final")]
        protected Link DeclineFinal { get; set; }

        [FindBy(Title = "Decline Finalised")]
        protected Link DeclineFinalised { get; set; }

        [FindBy(Title = "Disbursement Incorrect")]
        protected Link DisbursementIncorrect { get; set; }

        [FindBy(Title = "Dispute Finalised")]
        protected Link DisputeFinalised { get; set; }

        [FindBy(Title = "Feedback on Query")]
        protected Link FeedbackonQuery { get; set; }

        [FindBy(Title = "FL Rework Application")]
        protected Link FLReworkApplication { get; set; }

        [FindBy(Title = "Force Disbursement Timer")]
        protected Link ForceDisbursementTimer { get; set; }

        [FindBy(Title = "Further Lending Calc")]
        protected Link FurtherLendingCalc { get; set; }

        [FindBy(Title = "Held Over")]
        protected Link HeldOver { get; set; }

        [FindBy(Title = "Info Request Complete")]
        protected Link InfoRequestComplete { get; set; }

        [FindBy(Title = "Instruct Attorney")]
        protected Link InstructAttorney { get; set; }

        [FindBy(Title = "Instruction Failed")]
        protected Link InstructionFailed { get; set; }

        [FindBy(Title = "LOA Accepted")]
        protected Link LOAAccepted { get; set; }

        [FindBy(Title = "LOA Received")]
        protected Link LOAReceived { get; set; }

        [FindBy(Title = "Motivate")]
        protected Link Motivate { get; set; }

        [FindBy(Title = "Note Comment")]
        protected Link NoteComment { get; set; }

        [FindBy(Title = "NTU")]
        protected Link NTU { get; set; }

        [FindBy(Title = "NTU Finalised")]
        protected Link NTUFinalised { get; set; }

        [FindBy(Title = "NTU PipeLine")]
        protected Link NTUPipeLine { get; set; }

        [FindBy(Title = "Override Check")]
        protected Link OverrideCheck { get; set; }

        [FindBy(Title = "Perform Further Valuation")]
        protected Link PerformFurtherValuation { get; set; }

        [FindBy(Title = "Perform Valuation")]
        protected Link PerformValuation { get; set; }

        [FindBy(Title = "QA Complete")]
        protected Link QAComplete { get; set; }

        [FindBy(Title = "QA Query")]
        protected Link QAQuery { get; set; }

        [FindBy(Title = "Query Complete")]
        protected Link QueryComplete { get; set; }

        [FindBy(Title = "Query on Application")]
        protected Link QueryonApplication { get; set; }

        [FindBy(Title = "Query on LOA")]
        protected Link QueryonLOA { get; set; }

        [FindBy(Title = "ReAssign Commission Consultant")]
        protected Link ReAssignCommissionConsultant { get; set; }

        [FindBy(Title = "Reassign User")]
        protected Link ReassignUser { get; set; }

        [FindBy(Title = "Reinstate Decline")]
        protected Link ReinstateDecline { get; set; }

        [FindBy(Title = "Reinstate Followup")]
        protected Link ReinstateFollowup { get; set; }

        [FindBy(Title = "Reinstate NTU")]
        protected Link ReinstateNTU { get; set; }

        [FindBy(Title = "Reinstruct Attorney")]
        protected Link ReinstructAttorney { get; set; }

        [FindBy(Title = "Reload CaseName")]
        protected Link ReloadCaseName { get; set; }

        [FindBy(Title = "Request Lightstone Valuation")]
        protected Link RequestLightstoneValuation { get; set; }

        [FindBy(Title = "Request Resolved")]
        protected Link RequestResolved { get; set; }

        [FindBy(Title = "Require Arrear Comment")]
        protected Link RequireArrearComment { get; set; }

        [FindBy(Title = "Resend Instruction")]
        protected Link ResendInstruction { get; set; }

        [FindBy(Title = "Resend LOA")]
        protected Link ResendLOA { get; set; }

        [FindBy(Title = "Resubmit")]
        protected Link Resubmit { get; set; }

        [FindBy(Title = "Resubmit to Credit")]
        protected Link ResubmittoCredit { get; set; }

        [FindBy(Title = "Retry Instruction")]
        protected Link RetryInstruction { get; set; }

        [FindBy(Title = "Return to Manage Application")]
        protected Link ReturntoManageApplication { get; set; }

        [FindBy(Title = "Review Disbursement Setup")]
        protected Link ReviewDisbursementSetup { get; set; }

        [FindBy(Title = "Review Valuation Required")]
        protected Link ReviewValuationRequired { get; set; }

        [FindBy(Title = "Rework Application")]
        protected Link ReworkApplication { get; set; }

        [FindBy(Title = "Rollback Disbursement")]
        protected Link RollbackDisbursement { get; set; }

        [FindBy(Title = "Select Attorney")]
        protected Link SelectAttorney { get; set; }

        [FindBy(Title = "Send AIP")]
        protected Link SendAIP { get; set; }

        [FindBy(Title = "Send LOA")]
        protected Link SendLOA { get; set; }

        [FindBy(Title = "Translation Complete")]
        protected Link TranslationComplete { get; set; }

        [FindBy(Title = "Update Followup")]
        protected Link UpdateFollowup { get; set; }

        [FindBy(Title = "Activity128")]
        protected Link Activity128 { get; set; }

        [FindBy(Title = "Request Complete")]
        protected Link RequestComplete { get; set; }

        [FindBy(Title = "Confirm Application Employment")]
        protected Link ConfirmApplicationEmployment { get; set; }
    }
}


