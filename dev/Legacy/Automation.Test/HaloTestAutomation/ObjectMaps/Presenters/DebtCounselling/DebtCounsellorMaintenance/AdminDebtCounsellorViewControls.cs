using WatiN.Core;

namespace ObjectMaps.Pages
{
    public class AdminDebtCounsellorViewControls : BasePageControls
    {
        private Browser _window;
        private Frame _frame;

        public AdminDebtCounsellorViewControls(Browser window)
        {
            _window = window;
            _frame = _window.Frame(Find.ByIndex(0));
        }

        public Button btnAdd
        {
            get
            {
                return _frame.Button(Find.ById("ctl00_Main_btnAdd"));
            }
        }

        public Button btnRemove
        {
            get
            {
                return _frame.Button(Find.ById("ctl00_Main_btnRemove"));
            }
        }

        public Button btnUpdate
        {
            get
            {
                return _frame.Button(Find.ById("ctl00_Main_btnUpdate"));
            }
        }

        public Button btnView
        {
            get
            {
                return _frame.Button(Find.ById("ctl00_Main_btnView"));
            }
        }

        public Button btnSelect
        {
            get
            {
                return _frame.Button(Find.ById("ctl00_Main_btnSelect"));
            }
        }

        public Button btnAddToMenu
        {
            get
            {
                return _frame.Button(Find.ById("ctl00_Main_btnAddToCBO"));
            }
        }

        public Table tblOrgStructure
        {
            get
            {
                return _frame.Table(Find.ById("ctl00_Main_tlOrgStructure_D"));
            }
        }
    }
}