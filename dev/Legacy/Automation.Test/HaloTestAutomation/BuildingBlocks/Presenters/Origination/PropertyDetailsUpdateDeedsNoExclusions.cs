using Common.Enums;
using ObjectMaps.Pages;

namespace BuildingBlocks.Presenters.Origination
{
    public class PropertyDetailsUpdateDeedsNoExclusions : PropertyDetailsUpdateDeedsNoExclusionsControls
    {
        public void UpdateDeedsOfficeDetails(string TitleDeednumbers, string ErfNumber, string PortionNumber, string ErfSuburb, string ErfMetroDescription, string SectionalSchemaName,
            string SectionalUnitNumber, ButtonTypeEnum Button)
        {
            if (base.ctl00MaintxtTitleDeedNumber.Text != TitleDeednumbers)
                base.ctl00MaintxtTitleDeedNumber.TypeText(TitleDeednumbers);

            if (base.ctl00MaintxtErfNumber.Text != ErfNumber)
                base.ctl00MaintxtErfNumber.TypeText(ErfNumber);

            if (base.ctl00MaintxtPortionNumber.Text != PortionNumber)
                base.ctl00MaintxtPortionNumber.TypeText(PortionNumber);

            if (base.ctl00MaintxtErfSuburb.Text != ErfSuburb)
                base.ctl00MaintxtErfSuburb.TypeText(ErfSuburb);

            if (base.ctl00MaintxtErfMetroDescription.Text != ErfMetroDescription)
                base.ctl00MaintxtErfMetroDescription.TypeText(ErfMetroDescription);

            if (base.ctl00MaintxtSectionalSchemeName.Text != SectionalSchemaName)
                base.ctl00MaintxtSectionalSchemeName.TypeText(SectionalSchemaName);

            if (base.ctl00MaintxtSectionalUnitNumber.Text != SectionalUnitNumber)
                base.ctl00MaintxtSectionalUnitNumber.TypeText(SectionalUnitNumber);

            switch (Button)
            {
                case ButtonTypeEnum.Cancel:
                    base.ctl00MainbtnCancel.Click();
                    break;

                case ButtonTypeEnum.Update:
                    base.ctl00MainbtnUpdate.Click();
                    break;
            }
        }
    }
}