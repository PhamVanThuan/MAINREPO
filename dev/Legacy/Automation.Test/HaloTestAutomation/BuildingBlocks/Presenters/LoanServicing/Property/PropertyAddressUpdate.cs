using BuildingBlocks.Timers;
using ObjectMaps.Pages;

using System;
using System.Collections.Generic;
using WatiN.Core;

namespace BuildingBlocks.Presenters.LoanServicing.Correspondence
{
    public sealed class PropertyAddressUpdate : LegalEntityAddressControls
    {
        public void PopulateView(Automation.DataModels.Address address)
        {
            base.txtUnitNumber.Value = address.UnitNumber;

            base.txtBuildingNumber.Value = address.BuildingNumber;
            base.txtBuildingName.Value = address.BuildingName;

            base.txtStreetNumber.Value = address.StreetNumber;
            base.txtStreetName.Value = address.StreetName;

            //For clearing
            if (String.IsNullOrEmpty(address.RRR_SuburbDescription) && base.txtSuburb.Enabled)
            {
                base.txtSuburb.Value = "";
                base.txtSuburbHidden.Value = "";
            }

            base.ddlCountry.Option(address.RRR_CountryDescription).Select();
            base.ddlProvince.Option(address.RRR_ProvinceDescription).Select();

            if (base.txtSuburb.Enabled)
            {
                base.txtSuburb.TypeText(address.RRR_SuburbDescription);
                GeneralTimer.Wait(5000);
                if (base.SAHLAutoComplete_DefaultItem_Collection().Count > 0)
                    base.SAHLAutoComplete_DefaultItem_Collection()[0].MouseDown();
            }
        }

        public void ClickUpdate()
        {
            base.btnUpdate.Click();
        }

        public void ClickCancel()
        {
            base.btnCancel.Click();
        }

        public void AssertPropertyAddressControlsExists()
        {
            Assertions.WatiNAssertions.AssertFieldsExist
            (
                new List<Element> {
                        base.txtUnitNumber,
                        base.txtBuildingNumber,
                        base.txtBuildingName,
                        base.txtStreetNumber,
                        base.txtStreetName,
                        base.ddlCountry,
                        base.ddlProvince,
                        base.txtSuburb,
                        base.txtCity,
                        base.txtPostalCode,
                        base.btnUpdate,
                        base.btnCancel
                    }
            );
        }
    }
}