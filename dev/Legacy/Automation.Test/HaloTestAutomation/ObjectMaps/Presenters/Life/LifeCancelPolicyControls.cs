using WatiN.Core;

namespace ObjectMaps.Pages
{
    public abstract class LifeCancelPolicyControls : BasePageControls
    {
        [FindBy(Id = "ctl00_Main_ddlReason")]
        protected SelectList ctl00MainddlReason { get; set; }

        [FindBy(Id = "ctl00_Main_chkLetter")]
        protected CheckBox ctl00MainchkLetter { get; set; }

        [FindBy(Id = "ctl00_Main_rblType_0")]
        protected RadioButton CancelFromInception { get; set; }

        [FindBy(Id = "ctl00_Main_rblType_1")]
        protected RadioButton CancelWithAuthorisation { get; set; }

        [FindBy(Id = "ctl00_Main_rblType_2")]
        protected RadioButton CancelWithProRata { get; set; }

        [FindBy(Id = "ctl00_Main_rblType_3")]
        protected RadioButton CancelWithNoRefund { get; set; }
    }
}