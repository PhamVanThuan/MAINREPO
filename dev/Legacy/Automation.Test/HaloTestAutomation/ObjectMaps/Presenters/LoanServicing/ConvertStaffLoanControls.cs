using WatiN.Core;

namespace ObjectMaps.Pages
{
    public abstract class ConvertStaffLoanControls : BasePageControls
    {
        [FindBy(Id = "ctl00_Main_btnConvert")]
        protected Button Convert { get; set; }

        [FindBy(Id = "ctl00_Main_btnUnConvert")]
        protected Button UnConvert { get; set; }
    }
}