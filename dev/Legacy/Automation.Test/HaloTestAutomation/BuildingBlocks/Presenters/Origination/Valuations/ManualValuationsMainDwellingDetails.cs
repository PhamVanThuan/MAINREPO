using BuildingBlocks.Services.Contracts;
using Common.Enums;
using ObjectMaps.Pages;

namespace BuildingBlocks.Presenters.Origination.Valuations
{
    public class ManualValuationsMainDwellingDetails : ManualValuationsMainDwellingDetailsControls
    {
        private ICommonService commonService;

        public ManualValuationsMainDwellingDetails()
        {
            commonService = ServiceLocator.Instance.GetService<ICommonService>();
        }

        public void MainDwellingDetails(ButtonTypeEnum Button, out int ReplacementValue, out int TotalConventionalValue, out int TotalThatchValue)
        {
            ReplacementValue = commonService.ConvertCurrencyStringToInt(base.MainBuildingReplacementValue.Text);
            TotalConventionalValue = commonService.ConvertCurrencyStringToInt(base.TotalConventionalValue.Text);
            TotalThatchValue = commonService.ConvertCurrencyStringToInt(base.TotalThatchValue.Text);

            switch (Button)
            {
                case ButtonTypeEnum.Next:
                    base.SubmitButton.Click();
                    break;

                case ButtonTypeEnum.Cancel:
                    base.CancelButton.Click();
                    break;

                case ButtonTypeEnum.Back:
                    base.BackButton.Click();
                    break;
            }
        }

        public void Next()
        {
            base.Next.Click();
        }

        public void Cancel()
        {
            base.CancelButton.Click();
        }
    }
}