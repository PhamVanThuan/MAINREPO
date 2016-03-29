using WatiN.Core;

namespace ObjectMaps.Pages
{
    public abstract class SurveyCaptureControls : BasePageControls
    {
        [FindBy(Id = "ctl00_Main_btnDoSurvey")]
        protected Button ctl00MainbtnDoSurvey { get; set; }

        [FindBy(Id = "ctl00_Main_btnSendSurvey")]
        protected Button ctl00MainbtnSendSurvey { get; set; }

        [FindBy(Id = "ctl00_Main_ddlLegalEntity")]
        protected SelectList ctl00MainddlLegalEntity { get; set; }

        [FindBy(Id = "ctl00_Main_grdSurvey")]
        protected Table ctl00MaingrdSurvey { get; set; }

        [FindBy(Id = "ctl00_Main_grdSurvey_DXTitle")]
        protected Table ctl00MaingrdSurveyDXTitle { get; set; }

        [FindBy(Id = "ctl00_Main_grdSurvey_DXMainTable")]
        protected Table ctl00MaingrdSurveyDXMainTable { get; set; }

        [FindBy(Id = "ctl00_Main_grdSurvey_LP")]
        protected Table ctl00MaingrdSurveyLP { get; set; }

        [FindBy(Id = "ctl00_Main_grdSurvey_DXStyleTable")]
        protected Table ctl00MaingrdSurveyDXStyleTable { get; set; }
    }
}