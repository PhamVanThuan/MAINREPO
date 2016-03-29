using ObjectMaps.Pages;

namespace BuildingBlocks.Presenters.Life
{
    public class Life_LegalEntityAddExistingWorkFlow : LifeLegalEntityAddExistingWorkFlowControls
    {
        public void SetInsurableInterest(string insurableInterest)
        {
            base.ctl00MainddlNatUpdInsurableInterest.Option(insurableInterest).Select();
            base.ctl00MainbtnSubmitButton.Click();
        }
    }
}