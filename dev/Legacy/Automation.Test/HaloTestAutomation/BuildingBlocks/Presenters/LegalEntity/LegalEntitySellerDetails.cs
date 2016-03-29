using ObjectMaps.Pages;

namespace BuildingBlocks.Presenters.LegalEntity
{
    public class LegalEntitySellerDetails : LegalEntityDetailsSellerAddControls
    {
        /// <summary>
        /// Adds a seller using an existing Legal Entity's ID Number
        /// </summary>
        /// <param name="idNumber">ID Number</param>
        public void AddSeller(string idNumber)
        {
            base.txtNatAddIDNumber.TypeText(idNumber);
            base.SAHLAutoCompleteDiv_iframe.WaitUntilExists(30);
            base.SAHLAutoComplete_DefaultItem(idNumber).MouseDown();
            base.btnSubmitButton.Click();
        }

        /// <summary>
        /// Adds a Seller of Type Natural Person
        /// </summary>
        /// <param name="idNumber"></param>
        /// <param name="salutation"></param>
        /// <param name="firstNames"></param>
        /// <param name="surname"></param>
        /// <param name="gender"></param>
        /// <param name="maritalStatus"></param>
        /// <param name="populationGroup"></param>
        /// <param name="education"></param>
        /// <param name="citizenshipType"></param>
        /// <param name="passportNo"></param>
        /// <param name="taxNo"></param>
        /// <param name="homeLanguage"></param>
        /// <param name="docLanguage"></param>
        /// <param name="status"></param>
        /// <param name="homePhoneCode"></param>
        /// <param name="homePhoneNumber"></param>
        /// <param name="workPhoneCode"></param>
        /// <param name="workPhoneNumber"></param>
        /// <param name="faxCode"></param>
        /// <param name="faxNumber"></param>
        /// <param name="cellPhoneNumber"></param>
        /// <param name="emailAddress"></param>
        public void AddSeller(string idNumber, string salutation, string firstNames, string surname, string gender, string maritalStatus,
            string populationGroup, string education, string citizenshipType, string passportNo, string taxNo, string homeLanguage, string docLanguage, string status,
            string homePhoneCode, string homePhoneNumber, string workPhoneCode, string workPhoneNumber, string faxCode, string faxNumber, string cellPhoneNumber,
            string emailAddress)
        {
            string dob = "";
            if (idNumber != null)
            {
                for (int index = 4; index >= 0; index = (index - 2))
                {
                    if (index == 0) dob = dob + "19" + idNumber.Substring(index, 2);
                    else dob = dob + idNumber.Substring(index, 2) + "/";
                }
            }

            if (idNumber != null) base.txtNatAddIDNumber.TypeText(idNumber);
            base.ddlNatAddSalutation.Option(salutation).Select();
            string initials = firstNames.Remove(1);
            base.txtNatAddInitials.TypeText(initials);
            base.txtNatAddFirstNames.TypeText(firstNames);
            base.txtNatAddSurname.TypeText(surname);
            string preferedName = firstNames.Remove(3);
            base.txtNatAddPreferredName.TypeText(preferedName);
            base.ddlNatAddGender.Option(gender).Select();
            base.ddlNatAddMaritalStatus.Option(maritalStatus).Select();
            if (populationGroup != null) base.ddlNatAddpopulationGroup.Option(populationGroup).Select();
            if (education != null) base.ddlNatAddEducation.Option(education).Select();
            base.ddlNatAddCitizenshipType.Option(citizenshipType).Select();
            base.txtNatAddDateOfBirth.Value = dob;
            if (passportNo != null) base.txtNatAddPassportNumber.TypeText(passportNo);
            if (taxNo != null) base.txtNatAddTaxNumber.TypeText(taxNo);
            base.ddlNatAddHomeLanguage.Option(homeLanguage).Select();
            if (docLanguage != null) base.ddlNatAddDocumentLanguage.Option(docLanguage).Select();
            if (status != null) base.ddlNatAddStatus.Option(status).Select();
            if (homePhoneCode != null) base.txtHomePhoneCode.TypeText(homePhoneCode);
            //contactdetails
            CompleteContactDetails(homePhoneNumber, workPhoneCode, workPhoneNumber, faxCode, faxNumber, cellPhoneNumber, emailAddress);
            //marketing options
            CompleteMarketingOptions();
            base.btnSubmitButton.Click();
        }

        /// <summary>
        /// Completes the contact details portion of the legal entity details screen
        /// </summary>
        /// <param name="homePhoneNumber"></param>
        /// <param name="workPhoneCode"></param>
        /// <param name="workPhoneNumber"></param>
        /// <param name="faxCode"></param>
        /// <param name="faxNumber"></param>
        /// <param name="cellPhoneNumber"></param>
        /// <param name="emailAddress"></param>
        private void CompleteContactDetails(string homePhoneNumber, string workPhoneCode, string workPhoneNumber, string faxCode, string faxNumber, string cellPhoneNumber, string emailAddress)
        {
            if (homePhoneNumber != null) base.txtHomePhoneNumber.TypeText(homePhoneNumber);
            if (workPhoneCode != null) base.txtWorkPhoneCode.TypeText(workPhoneCode);
            if (workPhoneNumber != null) base.txtWorkPhoneNumber.TypeText(workPhoneNumber);
            if (faxCode != null) base.txtFaxCode.TypeText(faxCode);
            if (faxNumber != null) base.txtFaxNumber.TypeText(faxNumber);
            if (cellPhoneNumber != null) base.txtCellPhoneNumber.TypeText(cellPhoneNumber);
            if (emailAddress != null) base.txtEmailAddress.TypeText(emailAddress);
        }

        /// <summary>
        /// Adds a seller allowing the test to specify the legal entity type of the seller
        /// </summary>
        /// <param name="legalEntityType"></param>
        /// <param name="legalEntityRole"></param>
        /// <param name="idNumber"></param>
        /// <param name="salutation"></param>
        /// <param name="initials"></param>
        /// <param name="firstNames"></param>
        /// <param name="surname"></param>
        /// <param name="preferedName"></param>
        /// <param name="gender"></param>
        /// <param name="maritalStatus"></param>
        /// <param name="populationGroup"></param>
        /// <param name="education"></param>
        /// <param name="citizenshipType"></param>
        /// <param name="dob"></param>
        /// <param name="passportNo"></param>
        /// <param name="taxNo"></param>
        /// <param name="homeLanguage"></param>
        /// <param name="docLanguage"></param>
        /// <param name="status"></param>
        /// <param name="homePhoneCode"></param>
        /// <param name="homePhoneNumber"></param>
        /// <param name="workPhoneCode"></param>
        /// <param name="workPhoneNumber"></param>
        /// <param name="faxCode"></param>
        /// <param name="faxNumber"></param>
        /// <param name="cellPhoneNumber"></param>
        /// <param name="emailAddress"></param>
        public void AddSeller(string legalEntityType, string legalEntityRole, string idNumber, string salutation, string initials,
            string firstNames, string surname, string preferedName, string gender, string maritalStatus, string populationGroup, string education,
            string citizenshipType, string dob, string passportNo, string taxNo, string homeLanguage, string docLanguage, string status, string homePhoneCode,
            string homePhoneNumber, string workPhoneCode, string workPhoneNumber, string faxCode, string faxNumber, string cellPhoneNumber, string emailAddress)
        {
            base.ddlLegalEntityTypes.Option(legalEntityType).Select();
            base.ddlRoleTypeAdd.Option(legalEntityRole).Select();
            base.txtNatAddIDNumber.TypeText(idNumber);
            base.ddlNatAddSalutation.Option(salutation).Select();
            base.txtNatAddInitials.TypeText(initials);
            base.txtNatAddFirstNames.TypeText(firstNames);
            base.txtNatAddSurname.TypeText(surname);
            base.txtNatAddPreferredName.TypeText(preferedName);
            base.ddlNatAddGender.Option(gender).Select();
            base.ddlNatAddMaritalStatus.Option(maritalStatus).Select();
            base.ddlNatAddpopulationGroup.Option(populationGroup).Select();
            base.ddlNatAddEducation.Option(education).Select();
            base.ddlNatAddCitizenshipType.Option(citizenshipType).Select();
            base.txtNatAddDateOfBirth.Value = dob;
            base.txtNatAddPassportNumber.TypeText(passportNo);
            base.txtNatAddTaxNumber.TypeText(taxNo);
            base.ddlNatAddHomeLanguage.Option(homeLanguage).Select();
            base.ddlNatAddDocumentLanguage.Option(docLanguage).Select();
            base.ddlNatAddStatus.Option(status).Select();
            base.txtHomePhoneCode.TypeText(homePhoneCode);
            //contact details
            CompleteContactDetails(homePhoneNumber, workPhoneCode, workPhoneNumber, faxCode, faxNumber, cellPhoneNumber, emailAddress);
            CompleteMarketingOptions();
        }

        /// <summary>
        /// Completes the marketing options portion of the legal entity details screen.
        /// </summary>
        private void CompleteMarketingOptions()
        {
            //marketing options
            base.checkboxTelemarketing.Click();
            base.checkboxMarketing.Click();
            base.checkboxCustomerLists.Click();
            base.checkboxEmail.Click();
            base.checkboxSMS.Click();
        }
    }
}