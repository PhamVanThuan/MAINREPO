using BuildingBlocks.Services.Contracts;
using Common.Constants;
using Common.Enums;
using ObjectMaps.Pages;
using System;
using System.Collections.Generic;
using System.Threading;
using WatiN.Core;
using WatiN.Core.UtilityClasses;

namespace BuildingBlocks.Presenters.LegalEntity
{
    public class LegalEntityAddressDetails : LegalEntityAddressControls
    {
        private readonly IWatiNService watinService;

        public LegalEntityAddressDetails()
        {
            watinService = ServiceLocator.Instance.GetService<IWatiNService>();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="streetNumber"></param>
        /// <param name="streetName"></param>
        /// <param name="province"></param>
        /// <param name="suburb"></param>
        /// <param name="suburbCityPostcode"></param>
        public void AddResidentialAddress(string streetNumber, string streetName, string province, string suburb, string suburbCityPostcode)
        {
            base.txtStreetNumber.TypeText(streetNumber);
            base.txtStreetName.TypeText(streetName);
            base.ddlProvince.Option(province).Select();
            base.txtSuburb.TypeText(suburb);
            base.SAHLAutoCompleteDiv_iframe.WaitUntilExists(30);
            base.SAHLAutoComplete_DefaultItem(suburbCityPostcode).MouseDown();
            base.btnAdd.Click();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="address"></param>
        /// <param name="addressType"></param>
        public void AddStreetAddress(Automation.DataModels.Address address, string addressType)
        {
            base.ddlAddressType.Option(addressType).Select();
            Thread.Sleep(1500);
            base.txtStreetNumber.TypeText(address.StreetNumber.ToString());
            base.txtStreetName.TypeText(address.StreetName);
            base.ddlProvince.Option(address.RRR_ProvinceDescription).Select();
            base.txtSuburb.TypeText(address.RRR_SuburbDescription);

            bool executed = false;
            SimpleTimer timer = new SimpleTimer(TimeSpan.FromSeconds(30));
            while (!timer.Elapsed)
            {
                if (base.SAHLAutoComplete_DefaultItem_Collection().Count > 0)
                {
                    base.SAHLAutoComplete_DefaultItem_Collection()[0].MouseDown();
                    executed = true;
                    break;
                }
            }

            if (!executed) throw new WatiN.Core.Exceptions.TimeoutException("attempting to select Suburb from ajax control");

            base.btnAdd.Click();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="streetNumber"></param>
        /// <param name="streetName"></param>
        /// <param name="province"></param>
        /// <param name="suburb"></param>
        /// <param name="buttonClick"></param>
        public void AddResidentialAddress(string streetNumber, string streetName, string province, string suburb, ButtonTypeEnum buttonClick)
        {
            base.txtStreetNumber.TypeText(streetNumber);
            base.txtStreetName.TypeText(streetName);
            base.ddlProvince.Option(province).Select();
            base.txtSuburb.TypeText(suburb);

            bool executed = false;
            SimpleTimer timer = new SimpleTimer(TimeSpan.FromSeconds(30));
            while (!timer.Elapsed)
            {
                if (base.SAHLAutoComplete_DefaultItem_Collection().Count > 0)
                {
                    base.SAHLAutoComplete_DefaultItem_Collection()[0].MouseDown();
                    executed = true;
                    break;
                }
            }
            if (!executed) throw new WatiN.Core.Exceptions.TimeoutException("attempting to select Suburb from ajax control");

            switch (buttonClick)
            {
                case ButtonTypeEnum.Add:
                    base.btnAdd.Click();
                    break;

                case ButtonTypeEnum.Cancel:
                    base.btnCancel.Click();
                    break;

                case ButtonTypeEnum.Update:
                    base.btnUpdate.Click();
                    break;

                case ButtonTypeEnum.Back:
                    base.btnBack.Click();
                    break;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="buttonClick"></param>
        public void AddResidentialAddress(ButtonTypeEnum buttonClick)
        {
            switch (buttonClick)
            {
                case ButtonTypeEnum.Add:
                    base.btnAdd.Click();
                    break;

                case ButtonTypeEnum.Cancel:
                    base.btnCancel.Click();
                    break;

                case ButtonTypeEnum.Update:
                    base.btnUpdate.Click();
                    break;

                case ButtonTypeEnum.Back:
                    base.btnBack.Click();
                    break;
            }
        }

        /// <summary>
        /// This method will enumerate throught all the rows in the html address table and match each cell
        /// within the row with the field in the "AddressToSelect" object. If a single row's cells contains all the
        /// field values it will select that row on the GUI, and update it to whatever was passed by the UpdateTo object.
        /// Note: This method will only update street and unit and building information.
        /// </summary>
        /// <param name="addressToSelect">ResidentialStreetAddress struct type that contains all the fields of an  street address,
        /// that will be used to determine the address that needs to be selected.</param>
        /// <param name="updateTo">ResidentialStreetAddress struct type that contains all the fields of an  street address,
        /// that will be used to update the selected address with</param>
        public void SelectAndUpdateResidentialStreetAddress
            (
                Automation.DataModels.Address addressToSelect,
                Automation.DataModels.Address updateTo
            )
        {
            var addressGrid = base.ctl00MaingrdAddress;
            var tables = new List<Table>() { addressGrid };

            List<bool> validatedRow = new List<bool>();
            foreach (Table table in tables)
            {
                //Validate each row.
                foreach (TableRow row in table.OwnTableRows)
                {
                    foreach (TableCell cell in row.TableCells)
                    {
                        if (cell.Text.ToLower().Contains(addressToSelect.BuildingNumber.ToString().ToLower()))
                            validatedRow.Add(true);
                        else
                            validatedRow.Add(false);

                        if (cell.Text.ToLower().Contains(addressToSelect.BuildingName.ToLower()))
                            validatedRow.Add(true);
                        else
                            validatedRow.Add(false);

                        if (cell.Text.ToLower().Contains(addressToSelect.StreetNumber.ToString().ToLower()))
                            validatedRow.Add(true);
                        else
                            validatedRow.Add(false);

                        if (cell.Text.ToLower().Contains(addressToSelect.StreetName.ToLower()))
                            validatedRow.Add(true);
                        else
                            validatedRow.Add(false);

                        if (cell.Text.ToLower().Contains(addressToSelect.RRR_SuburbDescription.ToLower()))
                            validatedRow.Add(true);
                        else
                            validatedRow.Add(false);

                        if (cell.Text.ToLower().Contains(addressToSelect.RRR_ProvinceDescription.ToLower()))
                            validatedRow.Add(true);
                        else
                            validatedRow.Add(false);

                        if (!validatedRow.Contains(false))
                        {
                            row.Click();
                            base.txtUnitNumber.TypeText(updateTo.UnitNumber.ToString());
                            base.txtBuildingNumber.TypeText(updateTo.BuildingNumber.ToString());
                            base.txtBuildingName.TypeText(updateTo.BuildingName);
                            base.txtStreetNumber.TypeText(updateTo.StreetNumber.ToString());
                            base.txtStreetName.TypeText(updateTo.StreetName);
                            base.btnUpdate.Click();
                            break;
                        }
                        validatedRow.Clear();
                    }
                }
            }
        }

        /// <summary>
        /// This method will enumerate throught all the rows in the html address table and match each cell
        /// within the row with the field in the "AddressToSelect" object. If a single row's cells contains all the
        /// field values it will select that row on the GUI, and update it to whatever was passed by the UpdateTo object.
        /// Note: This method will only update the Box.
        /// </summary>
        /// <param name="postalBoxToSelect">PostalBoxAddress struct type that contains all the fields of an  postalbox address,
        /// that will be used to determine the address that needs to be selected.</param>
        /// <param name="postalBoxUpdateTo">PostalBoxAddress struct type that contains all the fields of an  postalbox address,
        /// that will be used to update the selected address with</param>
        public void SelectAndUpdatePostalBoxAddress
            (
                Automation.DataModels.Address postalBoxToSelect,
                Automation.DataModels.Address postalBoxUpdateTo
            )
        {
            var addressGrid = base.ctl00MaingrdAddress;
            var tables = new List<Table>() { addressGrid };
            List<bool> validatedRow = new List<bool>();
            foreach (Table table in tables)
            {
                //Validate each row.
                foreach (TableRow row in table.OwnTableRows)
                {
                    foreach (TableCell cell in row.TableCells)
                    {
                        if (cell.Text.ToLower().Contains(postalBoxToSelect.BoxNumber.ToLower()))
                            validatedRow.Add(true);
                        else
                            validatedRow.Add(false);

                        if (cell.Text.ToLower().Contains(postalBoxToSelect.RRR_SuburbDescription.ToLower()))
                            validatedRow.Add(true);
                        else
                            validatedRow.Add(false);

                        if (!validatedRow.Contains(false))
                        {
                            row.Click();
                            base.txtBoxNumber.TypeText(postalBoxUpdateTo.BoxNumber);
                            base.btnUpdate.Click();
                            break;
                        }
                        validatedRow.Clear();
                    }
                }
            }
        }

        /// <summary>
        ///  This method will add a ResidentialAddress.
        /// </summary>
        /// <param name="residentialAddress">Residential Address that will be added</param>
        public void AddResidentialAddress(Automation.DataModels.Address residentialAddress, ButtonTypeEnum buttonToClick = ButtonTypeEnum.Add)
        {
            base.ddlAddressType.Option("Residential").Select();
            base.ddlAddressFormat.Option("Street").Select();
            if (residentialAddress.UnitNumber != null)
                base.txtUnitNumber.Value = residentialAddress.UnitNumber.ToString();

            if (!string.IsNullOrEmpty(residentialAddress.BuildingName))
                base.txtBuildingName.Value = residentialAddress.BuildingName;

            if (residentialAddress.BuildingNumber != null)
                base.txtBuildingNumber.Value = residentialAddress.BuildingNumber.ToString();

            if (!string.IsNullOrEmpty(residentialAddress.StreetName))
                base.txtStreetName.Value = residentialAddress.StreetName;

            if (residentialAddress.StreetNumber != null)
                base.txtStreetNumber.Value = residentialAddress.StreetNumber.ToString();

            if (!string.IsNullOrEmpty(residentialAddress.RRR_ProvinceDescription))
                base.ddlProvince.Option(residentialAddress.RRR_ProvinceDescription).Select();

            if (!string.IsNullOrEmpty(residentialAddress.RRR_SuburbDescription))
            {
                base.txtSuburb.Value = residentialAddress.RRR_SuburbDescription;
                base.txtSuburb.Focus();

                int countDown = 3000;
                while (countDown != 0)
                {
                    countDown -= 1;
                    if (base.SAHLAutoComplete_DefaultItem_Collection().Count > 0)
                    {
                        foreach (Div div in base.SAHLAutoComplete_DefaultItem_Collection())
                        {
                            if (div.Text.Contains(residentialAddress.RRR_SuburbDescription))
                            {
                                div.MouseDown();
                                countDown = 0;
                                break;
                            }
                        }
                    }
                }
            }
            if (buttonToClick == ButtonTypeEnum.Add)
                base.btnAdd.Click();
        }

        /// <summary>
        ///  This method will add a PostalBox to the life offer with the PostalBoxAddress information provided.
        /// </summary>
        /// <param name="postalAddress">Postal Address that will be added</param>
        public void AddPostalBoxAddress(Automation.DataModels.Address postalAddress)
        {
            base.ddlAddressType.Option(AddressType.Postal).Select();
            base.ddlAddressFormat.Option(AddressFormat.Box).Select();
            if (!string.IsNullOrEmpty(postalAddress.BoxNumber))
                base.txtBoxNumber.Value = postalAddress.BoxNumber;
            if (!string.IsNullOrEmpty(postalAddress.RRR_SuburbDescription))
            {
                base.ctl00MainaddressDetailstxtPostOffice.TypeText(postalAddress.RRR_SuburbDescription);
                base.ctl00MainaddressDetailstxtPostOffice.Focus();
                SimpleTimer timer = new SimpleTimer(TimeSpan.FromSeconds(5));
                while (!timer.Elapsed)
                {
                    if (base.SAHLAutoComplete_DefaultItem_Collection().Count > 0)
                    {
                        foreach (Div div in base.SAHLAutoComplete_DefaultItem_Collection())
                        {
                            if (div.Text.Contains(postalAddress.RRR_SuburbDescription))
                            {
                                if (div.Text.Contains(postalAddress.PostalCode))
                                {
                                    div.MouseDown();
                                    break;
                                }
                            }
                        }
                    }
                }
            }
            base.btnAdd.Click();
        }

        /// <summary>
        /// This will add a new address of type Free Text
        /// </summary>
        /// <param name="freeTextAddress">Free Text Address</param>
        public void AddResidentialAddressFreeText(Automation.DataModels.Address freeTextAddress)
        {
            base.ddlAddressFormat.Option(AddressFormat.FreeText).Select();
            Thread.Sleep(2000);
            if (!string.IsNullOrEmpty(freeTextAddress.Line1))
                base.FreeTextLine1.Value = freeTextAddress.Line1;
            if (!string.IsNullOrEmpty(freeTextAddress.Line2))
                base.FreeTextLine2.Value = freeTextAddress.Line2;
            if (!string.IsNullOrEmpty(freeTextAddress.Line3))
                base.FreeTextLine3.Value = freeTextAddress.Line3;
            if (!string.IsNullOrEmpty(freeTextAddress.Line4))
                base.FreeTextLine4.Value = freeTextAddress.Line4;
            if (!string.IsNullOrEmpty(freeTextAddress.Line5))
                base.FreeTextLine5.Value = freeTextAddress.Line5;
            if (!string.IsNullOrEmpty(freeTextAddress.RRR_CountryDescription))
                base.ddlCountry.Option(freeTextAddress.RRR_CountryDescription).Select();
            base.btnAdd.Click();
        }

        /// <summary>
        /// Adds a cluster box address
        /// </summary>
        /// <param name="clusterBox">Cluster Box Address To Add</param>
        public void AddClusterBoxAddress(Automation.DataModels.Address clusterBox)
        {
            base.ddlAddressType.Option(AddressType.Postal).Select();
            base.ddlAddressFormat.Option(AddressFormat.ClusterBox).Select();
            if (!string.IsNullOrEmpty(clusterBox.BoxNumber))
                base.ClusterBoxNumber.Value = clusterBox.BoxNumber;
            if (!string.IsNullOrEmpty(clusterBox.RRR_SuburbDescription))
            {
                base.ctl00MainaddressDetailstxtPostOffice.TypeText(clusterBox.RRR_SuburbDescription);
                base.ctl00MainaddressDetailstxtPostOffice.Focus();
                SimpleTimer timer = new SimpleTimer(TimeSpan.FromSeconds(5));
                while (!timer.Elapsed)
                {
                    if (base.SAHLAutoComplete_DefaultItem_Collection().Count > 0)
                    {
                        foreach (Div div in base.SAHLAutoComplete_DefaultItem_Collection())
                        {
                            if (div.Text.Contains(clusterBox.RRR_SuburbDescription))
                            {
                                div.MouseDown();
                                break;
                            }
                        }
                    }
                }
            }
            base.btnAdd.Click();
        }

        /// <summary>
        /// Adds a PostNet Suite Address
        /// </summary>
        /// <param name="postNet">PostNet</param>
        public void AddPostNetSuiteAddress(Automation.DataModels.Address postNet)
        {
            base.ddlAddressType.Option(AddressType.Postal).Select();
            base.ddlAddressFormat.Option(AddressFormat.PostNetSuite).Select();
            if (!string.IsNullOrEmpty(postNet.SuiteNumber))
                base.PostNetSuiteNumber.Value = postNet.SuiteNumber;
            if (!string.IsNullOrEmpty(postNet.PrivateBag))
                base.PrivateBag.Value = postNet.PrivateBag;
            if (!string.IsNullOrEmpty(postNet.RRR_SuburbDescription))
            {
                base.ctl00MainaddressDetailstxtPostOffice.TypeText(postNet.RRR_SuburbDescription);
                base.ctl00MainaddressDetailstxtPostOffice.Focus();
                SimpleTimer timer = new SimpleTimer(TimeSpan.FromSeconds(5));
                while (!timer.Elapsed)
                {
                    if (base.SAHLAutoComplete_DefaultItem_Collection().Count > 0)
                    {
                        foreach (Div div in base.SAHLAutoComplete_DefaultItem_Collection())
                        {
                            if (div.Text.Contains(postNet.RRR_SuburbDescription))
                            {
                                div.MouseDown();
                                break;
                            }
                        }
                    }
                }
            }
            base.btnAdd.Click();
        }

        /// <summary>
        /// Adds a Private Bag Address
        /// </summary>
        /// <param name="privateBag">Private Bag Address</param>
        public void AddPrivateBagAddress(Automation.DataModels.Address privateBag)
        {
            base.ddlAddressType.Option(AddressType.Postal).Select();
            base.ddlAddressFormat.Option(AddressFormat.PrivateBag).Select();
            if (!string.IsNullOrEmpty(privateBag.PrivateBag))
                base.PrivateBag.Value = privateBag.PrivateBag;
            if (!string.IsNullOrEmpty(privateBag.RRR_SuburbDescription))
            {
                base.ctl00MainaddressDetailstxtPostOffice.TypeText(privateBag.RRR_SuburbDescription);
                base.ctl00MainaddressDetailstxtPostOffice.Focus();
                SimpleTimer timer = new SimpleTimer(TimeSpan.FromSeconds(5));
                while (!timer.Elapsed)
                {
                    if (base.SAHLAutoComplete_DefaultItem_Collection().Count > 0)
                    {
                        foreach (Div div in base.SAHLAutoComplete_DefaultItem_Collection())
                        {
                            if (div.Text.Contains(privateBag.RRR_SuburbDescription))
                            {
                                div.MouseDown();
                                break;
                            }
                        }
                    }
                }
            }
            base.btnAdd.Click();
        }

        /// <summary>
        /// Removes a legal entity address record
        /// </summary>
        /// <param name="addressDescription"></param>
        public void DeleteAddress(string addressDescription)
        {
            base.gridSelectAddress(addressDescription).Click();
            watinService.HandleConfirmationPopup(base.btnDelete);
        }

        /// <summary>
        /// This overload takes in the formatted address string and uses it to select a record from the grid to update rather than using a struct.
        /// </summary>
        /// <param name="formattedDelimitedAddress">addressDescription</param>
        /// <param name="residentialAddress">residentialAddress</param>
        public void SelectAndUpdateResidentialStreetAddress(string formattedDelimitedAddress, Automation.DataModels.Address residentialAddress)
        {
            base.gridSelectAddress(formattedDelimitedAddress).Click();
            Thread.Sleep(1500);
            if (!string.IsNullOrEmpty(residentialAddress.StreetNumber))
            {
                base.txtStreetNumber.Value = residentialAddress.StreetNumber.ToString();
            }
            if (!string.IsNullOrEmpty(residentialAddress.StreetName))
            {
                base.txtStreetName.Value = residentialAddress.StreetName;
            }
            if (!string.IsNullOrEmpty(residentialAddress.RRR_ProvinceDescription))
            {
                base.ddlProvince.Option(residentialAddress.RRR_ProvinceDescription).Select();
            }
            if (!string.IsNullOrEmpty(residentialAddress.BuildingNumber))
            {
                base.txtBuildingNumber.Value = residentialAddress.BuildingNumber.ToString();
            }
            if (!string.IsNullOrEmpty(residentialAddress.BuildingName))
            {
                base.txtBuildingName.Value = residentialAddress.BuildingName;
            }
            if (!string.IsNullOrEmpty(residentialAddress.UnitNumber))
            {
                base.txtUnitNumber.Value = residentialAddress.UnitNumber.ToString();
            }
            if (!string.IsNullOrEmpty(residentialAddress.RRR_SuburbDescription))
            {
                base.txtSuburb.Value = residentialAddress.RRR_SuburbDescription;
                base.txtSuburb.Focus();
                SimpleTimer timer = new SimpleTimer(TimeSpan.FromSeconds(5));
                while (!timer.Elapsed)
                {
                    if (base.SAHLAutoComplete_DefaultItem_Collection().Count > 0)
                    {
                        foreach (Div div in base.SAHLAutoComplete_DefaultItem_Collection())
                        {
                            if (div.Text.Contains(residentialAddress.RRR_SuburbDescription))
                            {
                                div.MouseDown();
                                break;
                            }
                        }
                    }
                }
            }
            base.btnUpdate.Click();
        }

        /// <summary>
        /// Selects the specified address type from the dropdown.
        /// </summary>
        /// <param name="addressType">Address Type</param>
        public void SelectAddressType(string addressType)
        {
            base.ddlAddressType.Option(addressType).Select();
        }

        /// <summary>
        /// Clicks the Add button.
        /// </summary>
        public void ClickAdd()
        {
            base.btnAdd.Click();
        }

        /// <summary>
        /// Selects the specified address format from the dropdown.
        /// </summary>
        /// <param name="addressFormat">Address Format</param>
        public void SelectAddressFormat(string addressFormat)
        {
            base.ddlAddressFormat.Option(addressFormat).Select();
        }

        /// <summary>
        /// Fetches the list of the address formats.
        /// </summary>
        /// <returns>List of Address Formats</returns>
        public SelectList GetAddressFormatList()
        {
            return base.ddlAddressFormat;
        }

        /// <summary>
        /// Fetches a list of the address types.
        /// </summary>
        /// <returns>List of Address Types</returns>
        public SelectList GetAddressTypeList()
        {
            return base.ddlAddressType;
        }
    }
}