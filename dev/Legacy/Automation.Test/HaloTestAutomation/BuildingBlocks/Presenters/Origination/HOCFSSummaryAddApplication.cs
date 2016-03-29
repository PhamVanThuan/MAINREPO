using Common.Enums;
using ObjectMaps.Pages;
using WatiN.Core;

namespace BuildingBlocks.Presenters.Origination
{
    public class HOCFSSummaryAddApplication : HOCFSSummaryAddApplicationControls
    {
        public void UpdateHOCDetails(string HOCInsurer, ButtonTypeEnum Button)
        {
            base.ctl00MainddlHOCInsurer.Option(HOCInsurer).Select();

            switch (Button)
            {
                case ButtonTypeEnum.Add:
                    base.ctl00MainbtnSubmitButton.Click();
                    break;

                case ButtonTypeEnum.Cancel:
                    base.ctl00MainbtnCancelButton.Click();
                    break;
            }
        }

        public void UpdateHOCDetails(int InsurerKey, ButtonTypeEnum Button)
        {
            UpdateHOCDetails(false, InsurerKey, -1, null, Button);
        }

        public void UpdateHOCDetails(int InsurerKey, int TotalHOCSumInsured, ButtonTypeEnum Button)
        {
            UpdateHOCDetails(false, InsurerKey, TotalHOCSumInsured, null, Button);
        }

        public void UpdateHOCDetails(bool PolicyCeded, int InsurerKey, int TotalHOCSumInsured,
            string HOCPolicyNumber, ButtonTypeEnum Button)
        {
            if (InsurerKey != -1)
                base.ctl00MainddlHOCInsurer.Option(Find.ByValue(InsurerKey.ToString())).Select();

            System.Threading.Thread.Sleep(2000);

            if (TotalHOCSumInsured != -1)
                base.ctl00MaintxtTotalHOCSumInsured.TypeText(TotalHOCSumInsured.ToString());
            if (!string.IsNullOrEmpty(HOCPolicyNumber))
                base.ctl00MaintxtHOCPolicyNumber.TypeText(HOCPolicyNumber);
            if (!base.ctl00MainchkCeded.Checked == PolicyCeded && base.ctl00MainchkCeded.Enabled)
                base.ctl00MainchkCeded.Checked = PolicyCeded;
            switch (Button)
            {
                case ButtonTypeEnum.Add:
                    base.ctl00MainbtnSubmitButton.Click();
                    break;

                case ButtonTypeEnum.Cancel:
                    base.ctl00MainbtnCancelButton.Click();
                    break;
            }
        }
    }
}