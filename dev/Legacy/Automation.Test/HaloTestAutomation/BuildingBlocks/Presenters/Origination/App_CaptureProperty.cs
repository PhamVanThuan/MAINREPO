using BuildingBlocks.Services.Contracts;
using Common.Enums;
using ObjectMaps.Pages;
using System;
using System.Linq;
using WatiN.Core.UtilityClasses;

namespace BuildingBlocks.Presenters.Origination
{
    /// <summary>
    /// The 1st Screen of the Property Capture process
    /// </summary>
    public class App_PropertyCaptureStep1 : App_PropertyCaptureControls_pg1
    {
        public void PerformEmptySearch()
        {
            //PAGE ONE
            base.txtIdentity.TypeText("0");
            base.btnSearch.Click();
            base.btnNext.Click();
        }
    }

    /// <summary>
    /// The 2nd Screen of the Property Capture process
    /// </summary>
    public class App_ProperyCaptureStep2 : App_PropertyCaptureControls_pg2
    {
        private readonly IWatiNService watinService;

        public App_ProperyCaptureStep2()
        {
            watinService = ServiceLocator.Instance.GetService<IWatiNService>();
        }

        public void CaptureProperty(Automation.DataModels.Property property)
        {
            //PAGE TWO
            if (!string.IsNullOrEmpty(property.PropertyAddress.UnitNumber)) 
                base.txtUnitNumber.TypeText(property.PropertyAddress.UnitNumber);
            if (!string.IsNullOrEmpty(property.PropertyAddress.BuildingNumber)) 
                base.txtBuildingNumber.TypeText(property.PropertyAddress.BuildingNumber);
            if (!string.IsNullOrEmpty(property.PropertyAddress.BuildingName)) 
                base.txtBuildingName.TypeText(property.PropertyAddress.BuildingName);
            if (!string.IsNullOrEmpty(property.PropertyAddress.StreetNumber))
                base.txtStreetNumber.TypeText(property.PropertyAddress.StreetNumber);
            if (!string.IsNullOrEmpty(property.PropertyAddress.StreetName))
                base.txtStreetName.TypeText(property.PropertyAddress.StreetName);
            if (!string.IsNullOrEmpty(property.PropertyAddress.RRR_ProvinceDescription))
                base.ddlProvince.Option(property.PropertyAddress.RRR_ProvinceDescription).Select();
            if (!string.IsNullOrEmpty(property.PropertyAddress.RRR_SuburbDescription))
            {
                base.txtSuburb.TypeText(property.PropertyAddress.RRR_SuburbDescription);
                base.SAHLAutoCompleteDiv_iframe.WaitUntilExists(30);
                //attempt to select a Suburb from the ajax control
                bool executed = false;
                SimpleTimer timer = new SimpleTimer(TimeSpan.FromSeconds(30));
                while (!timer.Elapsed)
                {
                    if (base.SAHLAutoComplete_DefaultItem_Collection(property.PropertyAddress.RRR_SuburbDescription).Count > 0)
                    {
                        base.SAHLAutoComplete_DefaultItem_Collection(property.PropertyAddress.RRR_SuburbDescription).ElementAt(0).MouseDown();
                        executed = true;
                        break;
                    }
                }
                if (!executed) throw new WatiN.Core.Exceptions.TimeoutException("attempting to select a Suburb from the ajax control");
            }

            watinService.HandleConfirmationPopup(base.linkSearchAddress(property.FormattedPropertyAddress));
        }
    }

    /// <summary>
    /// The 3rd Screen of the Property Capture process
    /// </summary>
    public class App_PropertyCaptureStep3 : App_PropertyCaptureControls_pg3
    {
        public void CapturePropertyDetails(Automation.DataModels.Property property)
        {
            //PAGE 3
            base.txtPropertyDescription1.TypeText(property.PropertyDescription1);
            base.txtPropertyDescription2.TypeText(property.PropertyDescription2);
            base.txtPropertyDescription3.TypeText(property.PropertyDescription3);
            if (!string.IsNullOrEmpty(property.PropertyTypeDescription))
                base.ddlPropertyType.Option(property.PropertyTypeDescription).Select();
            if (!string.IsNullOrEmpty(property.TitleTypeDescription))
                base.ddlTitleType.Option(property.TitleTypeDescription).Select();
            if (!string.IsNullOrEmpty(property.OccupancyTypeDescription))
                base.ddlOccupancyType.Option(property.OccupancyTypeDescription).Select();
            base.txtInspectionContact1.TypeText("Bob Green");
            base.txtInspectionTel1.TypeText("031 1234567");
            base.btnNext.Click();
        }
    }
}