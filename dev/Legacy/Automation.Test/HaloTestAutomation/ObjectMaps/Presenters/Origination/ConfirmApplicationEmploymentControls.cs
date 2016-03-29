using ObjectMaps.Pages;
using WatiN.Core;

namespace ObjectMaps.Presenters.Origination
{
    public abstract class ConfirmApplicationEmploymentControls : BasePageControls
    {
        [FindBy(Id = "ctl00_Main_ddlSelectedEmploymentType")]
        protected SelectList EmploymentType { get; set; }

        [FindBy(Id = "ctl00_Main_btnConfirm")]
        protected Button ConfirmButton { get; set; }
    }
}