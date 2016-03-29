using WatiN.Core;

namespace ObjectMaps.NavigationControls
{
    public abstract class CreditWorkflowActionControls : BaseNavigation
    {
		[FindBy(Title = "Agree with decision")]
        protected Link Agreewithdecision { get; set; }

		[FindBy(Title = "Approve Application")]
        protected Link ApproveApplication { get; set; }

		[FindBy(Title = "Approve with Pricing Changes")]
        protected Link ApprovewithPricingChanges { get; set; }

		[FindBy(Title = "Credit Comment")]
        protected Link CreditComment { get; set; }

		[FindBy(Title = "Decline Application")]
        protected Link DeclineApplication { get; set; }

		[FindBy(Title = "Decline with Offer")]
        protected Link DeclinewithOffer { get; set; }

		[FindBy(Title = "Dispute Indicated")]
        protected Link DisputeIndicated { get; set; }

		[FindBy(Title = "Escalate To Exceptions Mgr")]
        protected Link EscalateToExceptionsMgr { get; set; }

		[FindBy(Title = "Escalate to Mgr ")]
        protected Link EscalatetoMgr { get; set; }

		[FindBy(Title = "Exceptions Decline with Offer")]
        protected Link ExceptionsDeclinewithOffer { get; set; }

		[FindBy(Title = "Feedback on Override")]
        protected Link FeedbackonOverride { get; set; }

		[FindBy(Title = "Override Decision")]
        protected Link OverrideDecision { get; set; }

		[FindBy(Title = "Reassign Analyst")]
        protected Link ReassignAnalyst { get; set; }

		[FindBy(Title = "Refer Senior Analyst")]
        protected Link ReferSeniorAnalyst { get; set; }

		[FindBy(Title = "Request Further Info")]
        protected Link RequestFurtherInfo { get; set; }

		[FindBy(Title = "Request Lightstone Valuation")]
        protected Link RequestLightstoneValuation { get; set; }

		[FindBy(Title = "Request Policy Override")]
        protected Link RequestPolicyOverride { get; set; }

		[FindBy(Title = "Return to Processor")]
        protected Link ReturntoProcessor { get; set; }

		[FindBy(Title = "Review Complete")]
        protected Link ReviewComplete  { get; set; }

		[FindBy(Title = "Valuation Approved")]
        protected Link ValuationApproved { get; set; }

        [FindBy(Title = "Confirm Application Employment")]
        protected Link ConfirmApplicationEmployment { get; set; }

        [FindBy(Title = "Exceptions Rate Adjustment")]
        protected Link ExceptionsRateAdjustment { get; set; }
    }
}

