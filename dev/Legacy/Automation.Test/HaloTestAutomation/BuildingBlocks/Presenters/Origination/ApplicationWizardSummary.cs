using Common.Enums;
using ObjectMaps.Pages;

namespace BuildingBlocks.Presenters.Origination
{
    public class ApplicationWizardSummary : ApplicationWizardSummaryControls
    {
        public void ApplicationDetails(ButtonTypeEnum ButtonClick)
        {
            switch (ButtonClick)
            {
                case ButtonTypeEnum.Finish:
                    base.btnFinish.Click();
                    break;

                case ButtonTypeEnum.NextApplicant:
                    base.btnNextApplicant.Click();
                    break;

                case ButtonTypeEnum.UpdateApplicant:
                    base.btnUpdateApplicant.Click();
                    break;

                case ButtonTypeEnum.UpdateCalculator:
                    base.btnUpdateCalculator.Click();
                    break;
            }
        }
    }
}