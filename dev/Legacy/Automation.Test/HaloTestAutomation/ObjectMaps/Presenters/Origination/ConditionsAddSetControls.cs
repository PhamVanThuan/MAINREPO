using WatiN.Core;

namespace ObjectMaps.Pages
{
    public abstract class ConditionsAddSetControls : BasePageControls
    {
        [FindBy(Id = "btnAdd")]
        public Button btnAdd { get; set; }

        [FindBy(Id = "ctl00_Main_btnAddCondition")]
        public Button btnAddCondition { get; set; }

        [FindBy(Id = "ctl00_Main_btnEditCondition")]
        public Button btnEditCondition { get; set; }

        [FindBy(Id = "btnRemove")]
        public Button btnRemove { get; set; }

        [FindBy(Id = "ctl00_Main_btnCancel")]
        public Button btnCancel { get; set; }

        [FindBy(Id = "ctl00_Main_btnSave")]
        public Button btnSave { get; set; }

        [FindBy(Id = "ctl00_Main_listGenericConditions")]
        public SelectList AvailableConditionsList { get; set; }

        [FindBy(Id = "ctl00_Main_listSelectedConditions")]
        public SelectList SelectedLoanConditionsList { get; set; }

        [FindBy(Id = "ctl00_Main_txtDisplay")]
        public TextField ConditionDisplay { get; set; }

        [FindBy(Id = "ctl00_Main_btnUpdate")]
        public Button btnUpdateConditionSet { get; set; }
    }
}