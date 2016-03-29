using WatiN.Core;

namespace ObjectMaps.Pages
{
    public abstract class PersonalLoanApplicationDeclarationsControls : BasePageControls
    {
        [FindBy(Id = "ctl00_Main_Ctrl841")]
        protected SelectList InsolvencySelect { get; set; }

        [FindBy(Id = "ctl00_Main_Ctrl843")]
        protected SelectList AdminSelect { get; set; }

        [FindBy(Id = "ctl00_Main_Ctrl845")]
        protected SelectList DebtCounselingSelect { get; set; }

        [FindBy(Id = "ctl00_Main_Ctrl846")]
        protected SelectList DebtRearrangementSelect { get; set; }

        [FindBy(Id = "ctl00_Main_Ctrl847")]
        protected SelectList CreditCheckSelect { get; set; }

        [FindBy(Id = "ctl00_Main_Ctrl842")]
        protected TextField RehabDate { get; set; }

        [FindBy(Id = "ctl00_Main_Ctrl844")]
        protected TextField RescindedDate { get; set; }

        [FindBy(Id = "ctl00_Main_btnUpdate")]
        protected Button UpdateButton { get; set; }

        [FindBy(Id = "ctl00_Main_btnCancel")]
        protected Button CancelButton { get; set; }

        protected SelectListCollection GetAllSelectListsOnScreen
        {
            get
            {
                return base.Document.SelectLists;
            }
        }

        protected ButtonCollection GetAllButtonsOnScreen
        {
            get
            {
                return base.Document.Buttons;
            }
        }
    }
}