using ObjectMaps;
using ObjectMaps.Pages;
using WatiN.Core;

namespace BuildingBlocks.Presenters.LoanServicing
{
    /// <summary>
    /// Contains methods to send surveys
    /// </summary>
    public class SurveyCapture : SurveyCaptureControls
    {
        /// <summary>
        /// Selects a survey to be sent
        /// </summary>
        /// <param name="expression"></param>
        public void SelectClientSurvey(string expression)
        {
            TableRow row = base.ctl00MaingrdSurveyDXMainTable.FindRow(expression, 0);
            row.Click();
        }

        /// <summary>
        /// Selects a legal entity to be sent the survey
        /// </summary>
        /// <param name="expression"></param>
        public void SelectLegalEntityFromDropDown(string expression)
        {
            base.ctl00MainddlLegalEntity.Option(new System.Text.RegularExpressions.Regex(expression)).Select();
        }

        /// <summary>
        /// Sends the survey
        /// </summary>
        public void SendSurvey()
        {
            CommonIEDialogHandler.SelectOK(base.ctl00MainbtnSendSurvey);
        }
    }
}