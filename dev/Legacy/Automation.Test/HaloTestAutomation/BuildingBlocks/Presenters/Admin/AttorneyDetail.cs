using BuildingBlocks.Presenters.LegalEntity;
using Common.Enums;
using ObjectMaps.Pages;
using System;

namespace BuildingBlocks.Presenters.Admin
{
    public class AttorneyDetail : AttorneyDetailControls
    {
        /// <summary>
        /// Selecs a deeds office
        /// </summary>
        /// <param name="deedsofficename"></param>
        public void SelectDeedsOffice(string deedsofficename)
        {
            base.ddlDeedsOffice.Option(deedsofficename).Select();
        }

        /// <summary>
        /// Checks if the attorney exists in the dropdown list
        /// </summary>
        /// <param name="attorneyname"></param>
        /// <returns></returns>
        public bool AttorneyExistInDropdownList(string attorneyname)
        {
            if (base.ddlAttorney.Options.Exists(attorneyname))
                return true;
            else
                return false;
        }

        /// <summary>
        /// Selects an attorney from the dropdown lsit
        /// </summary>
        /// <param name="registeredName"></param>
        public void SelectAttorney(string registeredName)
        {
            base.ddlAttorney.Option(registeredName).Select();
        }

        /// <summary>
        /// Populates the details of the attorney from the model.
        /// </summary>
        /// <param name="attorney"></param>
        public void PopulateAttorneyDetails
            (
                Automation.DataModels.Attorney attorney
            )
        {
            if (base.txtAttorneyName.Exists)
                base.txtAttorneyName.Value = attorney.LegalEntity.RegisteredName;
            if (base.txtAttorneyContact.Exists)
                base.txtAttorneyContact.Value = attorney.ContactName;
            if (base.txtPhoneNumberCode.Exists)
                base.txtPhoneNumberCode.Value = attorney.LegalEntity.WorkPhoneCode.ToString();
            if (base.txtPhoneNumber.Exists)
                base.txtPhoneNumber.Value = attorney.LegalEntity.WorkPhoneNumber.ToString();
            if (base.txtEmailAddress.Exists)
                base.txtEmailAddress.Value = attorney.LegalEntity.EmailAddress;
            if (base.txtAttorneyMandate_txtRands.Exists)
                base.txtAttorneyMandate_txtRands.Value = Math.Round((double)attorney.Mandate, 0).ToString();
            if (base.ddlRegistrationAttorney.Exists)
            {
                string registrationAttorneyYesNo = String.Empty;
                if (attorney.IsRegistrationAttorney)
                    registrationAttorneyYesNo = "Yes";
                else
                    registrationAttorneyYesNo = "No";
                base.ddlRegistrationAttorney.Option(registrationAttorneyYesNo).Select();
            }
            if (base.ddlDeedsOfficeChange.Exists)
                base.ddlDeedsOfficeChange.Option(attorney.DeedsOffice).Select();

            if (base.ddlLitigationAttorney.Exists)
            {
                string litigationAttorneyYesNo = String.Empty;
                if (attorney.IsLitigationAttorney)
                    litigationAttorneyYesNo = "Yes";
                else
                    litigationAttorneyYesNo = "No";
                base.ddlLitigationAttorney.Option(litigationAttorneyYesNo).Select();
            }
        }

        /// <summary>
        /// Populate the address control as a residential address, can be changed later to do other address formats
        /// </summary>
        public void PopulateAttorneyAddress(AddressTypeEnum addressType, Automation.DataModels.Address address)
        {
            if (addressType == AddressTypeEnum.Residential)
            {
                base.Document.Page<LegalEntityAddressDetails>().AddResidentialAddress(address, ButtonTypeEnum.None);
            }
        }

        /// <summary>
        /// click the submit button
        /// </summary>
        public void ClickSubmit()
        {
            base.MainSubmitButton.Click();
        }

        /// <summary>
        /// click the cancel button
        /// </summary>
        public void ClickCancel()
        {
            base.MainCancelButton.Click();
        }

        /// <summary>
        /// click on the Contacts nutton
        /// </summary>
        public void ClickContacts()
        {
            base.btnContacts.Click();
        }

        /// <summary>
        /// clears all the fields on the view
        /// </summary>
        public void ClearAllFields()
        {
            if (base.txtAttorneyName.Exists)
                base.txtAttorneyName.Clear();
            if (base.txtAttorneyContact.Exists)
                base.txtAttorneyContact.Clear();
            if (base.txtPhoneNumberCode.Exists)
                base.txtPhoneNumberCode.Clear();
            if (base.txtPhoneNumber.Exists)
                base.txtPhoneNumber.Clear();
            if (base.txtEmailAddress.Exists)
                base.txtEmailAddress.Clear();
            if (base.txtAttorneyMandate_txtRands.Exists)
                base.txtAttorneyMandate_txtRands.Clear();
            if (base.txtUnitNumber.Exists)
                base.txtUnitNumber.Clear();
            if (base.txtBuildingNumber.Exists)
                base.txtBuildingNumber.Clear();
            if (base.txtBuildingName.Exists)
                base.txtBuildingName.Clear();
            if (base.txtStreetNumber.Exists)
                base.txtStreetNumber.Clear();
            if (base.txtStreetName.Exists)
                base.txtStreetName.Clear();
            if (base.txtUnitNumber.Exists)
                base.txtUnitNumber.Clear();
            if (base.txtSuburb.Exists && base.txtSuburb.Enabled)
                base.txtSuburb.Clear();
        }
    }
}