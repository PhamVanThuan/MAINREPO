using WatiN.Core;

namespace ObjectMaps.Pages
{
    public abstract class NoteMaintenanceControls : BasePageControls
    {
        [FindBy(Id = "ctl00_Main_lblDiaryDate")]
        protected Span ctl00MainlblDiaryDate { get; set; }

        [FindBy(Id = "ctl00_Main_ddlUsers")]
        protected SelectList ctl00MainddlUsers { get; set; }

        [FindBy(Id = "ctl00_Main_ddlDateFrom")]
        protected SelectList ctl00MainddlDateFrom { get; set; }

        [FindBy(Id = "ctl00_Main_ddlDateTo")]
        protected SelectList ctl00MainddlDateTo { get; set; }

        [FindBy(Value = "Add")]
        protected Button Add { get; set; }

        [FindBy(Id = "ctl00_Main_btnSubmit")]
        protected Button SaveDiaryDate { get; set; }

        [FindBy(Id = "Button1")]
        protected Button CheckDiaryDate { get; set; }

        [FindBy(Id = "ctl00_Main_dtDiaryDate")]
        protected TextField DiaryDate { get; set; }

        [FindBy(Value = "Update")]
        protected Button Update { get; set; }

        [FindBy(Value = "Cancel")]
        protected Button Cancel { get; set; }

        [FindBy(Value = "Edit")]
        protected Button Edit { get; set; }

        [FindBy(Id = "ctl00_Main_lblRelatedEntries")]
        protected Span ctl00_Main_lblRelatedEntries { get; set; }

        [FindBy(Id = "ctl00_Main_gvNotes_DXPEForm_efnew_txtTag")]
        protected TextField Tag { get; set; }

        [FindBy(Id = "ctl00_Main_gvNotes_DXPEForm_DXEditingErrorRow")]
        protected TableRow ctl00_Main_gvNotes_DXPEForm_DXEditingErrorRow { get; set; }
    }
}