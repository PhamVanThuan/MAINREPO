using ObjectMaps.Pages;

namespace BuildingBlocks.Presenters.Origination
{
    public class App_AssignEstateAgent : App_AssignEstateAgentControls
    {
        /// <summary>
        /// Selects the provided estate agency from the dropdown and clicks the Submit button
        /// </summary>
        /// <param name="EstateAgency">Estate Agency to Select</param>
        public void CaptureEstateAgency(string EstateAgency)
        {
            base.ddlConsultant.Option(EstateAgency.Trim()).Select();
            base.btnSubmit.Click();
        }
    }
}