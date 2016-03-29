using WatiN.Core;

namespace ObjectMaps.Pages
{
    public abstract class CreditProtectionSummaryControls : BasePageControls
    {
        [FindBy(Id = "ctl00_Main_lblPolicyNumber")]
        protected Span PolicyNumber { get; set; }

        [FindBy(Id = "ctl00_Main_lblOpenDate")]
        protected Span OpenDate { get; set; }

        [FindBy(Id = "ctl00_Main_lblAccountStatus")]
        protected Span Status { get; set; }

        [FindBy(Id = "ctl00_Main_lblLifePolicyPremium")]
        protected Span LifePolicyPremium { get; set; }

        [FindBy(Id = "ctl00_Main_lblSumInsured")]
        protected Span SumInsured { get; set; }
    }
}