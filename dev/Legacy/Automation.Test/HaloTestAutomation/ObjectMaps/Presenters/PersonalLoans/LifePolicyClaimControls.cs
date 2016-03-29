using WatiN.Core;

namespace ObjectMaps.Pages
{
    public abstract class LifePolicyClaimControls : BasePageControls
    {
        [FindBy(Id = "ctl00_Main_ddlClaimType")]
        protected SelectList ClaimType { get; set; }

        [FindBy(Id = "ctl00_Main_lblClaimType")]
        protected Span ClaimTypeReadonly { get; set; }

        [FindBy(Id = "ctl00_Main_ddlClaimStatus")]
        protected SelectList ClaimStatus { get; set; }

        [FindBy(Id = "ctl00_Main_dtClaimStatusDate")]
        protected TextField ClaimStatusDate { get; set; }

        [FindBy(Id = "ctl00_Main_LifePolicyClaimGrid")]
        protected Table PolicyClaimGrid { get; set; }
    }
}