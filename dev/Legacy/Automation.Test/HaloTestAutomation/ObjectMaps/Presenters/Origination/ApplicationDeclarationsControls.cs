using WatiN.Core;

namespace ObjectMaps.Pages
{
    public abstract class ApplicationDeclarationsControls : BasePageControls
    {
        [FindBy(Id = "ctl00_Main_Ctrl12")]
        protected SelectList InsolvencySelect { get; set; }

        [FindBy(Id = "ctl00_Main_Ctrl172")]
        protected SelectList AdminSelect { get; set; }

        [FindBy(Id = "ctl00_Main_Ctrl332")]
        protected SelectList DebtCounselingSelect { get; set; }

        [FindBy(Id = "ctl00_Main_Ctrl412")]
        protected SelectList DebtRearrangementSelect { get; set; }

        [FindBy(Id = "ctl00_Main_Ctrl492")]
        protected SelectList CreditCheckSelect { get; set; }

        [FindBy(Id = "ctl00_Main_Ctrl92")]
        protected TextField RehabDate { get; set; }

        [FindBy(Id = "ctl00_Main_Ctrl252")]
        protected TextField RescindedDate { get; set; }

        [FindBy(Id = "ctl00_Main_btnUpdate")]
        protected Button UpdateButton { get; set; }

        [FindBy(Id = "ctl00_Main_btnCancel")]
        protected Button CancelButton { get; set; }

        public SelectListCollection GetAllSelectListsOnScreen
        {
            get
            {
                return base.Document.SelectLists;
            }
        }

        public ButtonCollection GetAllButtonsOnScreen
        {
            get
            {
                return base.Document.Buttons;
            }
        }
    }
}