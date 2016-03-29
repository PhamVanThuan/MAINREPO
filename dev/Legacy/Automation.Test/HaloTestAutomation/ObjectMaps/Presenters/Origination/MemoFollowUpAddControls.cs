using WatiN.Core;

namespace ObjectMaps.Pages
{
    public abstract class MemoFollowUpAddControls : BasePageControls
    {
        [FindBy(Id = "ctl00_Main_ReminderDateUpdate")]
        protected TextField ReminderDateUpdate { get; set; }

        [FindBy(Id = "ctl00_Main_MemoUpdate")]
        protected TextField MemoUpdate { get; set; }

        [FindBy(Id = "ctl00_Main_ddlMemoStatus")]
        protected SelectList MemoStatusDropdown { get; set; }

        [FindBy(Id = "ctl00_Main_MemoStatusUpdate")]
        protected SelectList MemoStatusUpdate { get; set; }

        [FindBy(Id = "ctl00_Main_ddlHour")]
        protected SelectList HourDropdown { get; set; }

        [FindBy(Id = "ctl00_Main_ddlMin")]
        protected SelectList MinDropdown { get; set; }

        [FindBy(Id = "ctl00_Main_SubmitButton")]
        protected Button AddButton { get; set; }

        [FindBy(Id = "ctl00_Main_CancelButton")]
        protected Button CancelButton { get; set; }
    }
}