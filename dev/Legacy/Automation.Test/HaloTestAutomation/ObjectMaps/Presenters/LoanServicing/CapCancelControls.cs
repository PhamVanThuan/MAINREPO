using ObjectMaps.Pages;
using WatiN.Core;

namespace ObjectMaps.Presenters.LoanServicing
{
    public abstract class CapCancelControls : BasePageControls
    {
        [FindBy(Id = "ctl00_Main_ddlCancellationReason")]
        public SelectList CancellationReason { get; set; }
    }
}