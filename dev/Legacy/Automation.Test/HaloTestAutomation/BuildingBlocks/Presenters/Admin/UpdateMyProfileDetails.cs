using ObjectMaps.Presenters.Admin;
using Common.Extensions;
using Common.Enums;

namespace BuildingBlocks.Presenters.Admin
{
    public class UpdateMyProfileDetails : UpdateMyProfileDetailsControls
    {

        public void RemoveFirstNameAndSurnameAndSubmit()
        {
            base.FirstName.Clear();
            base.Surname.Clear();
            base.SubmitButton.Click();
        }

        public void UpdateContactDetails(Automation.DataModels.LegalEntityContactDetails newContactDetails)
        {
            base.HomePhoneCode.ClearExistingValueAndSetTo(newContactDetails.HomePhoneNumber.Code);
            base.HomePhoneNumber.ClearExistingValueAndSetTo(newContactDetails.HomePhoneNumber.Number);
            base.WorkphoneCode.ClearExistingValueAndSetTo(newContactDetails.WorkPhoneNumber.Code);
            base.WorkphoneNumber.ClearExistingValueAndSetTo(newContactDetails.WorkPhoneNumber.Number);
            base.FaxCode.ClearExistingValueAndSetTo(newContactDetails.FaxNumber.Code);
            base.FaxNumber.ClearExistingValueAndSetTo(newContactDetails.FaxNumber.Number);
            base.CellphoneNumber.ClearExistingValueAndSetTo(newContactDetails.CellphoneNumber.ToString());
            base.EmailAddress.ClearExistingValueAndSetTo(newContactDetails.EmailAddress);
            base.SubmitButton.Click();
        }


        public void UpdateDetails(string firstName, string surname, string preferredName, string initials)
        {
            base.FirstName.ClearExistingValueAndSetTo(firstName);
            base.Surname.ClearExistingValueAndSetTo(surname);
            base.PreferredName.ClearExistingValueAndSetTo(preferredName);
            base.Initials.ClearExistingValueAndSetTo(initials);
            base.SubmitButton.Click();
        }

        public string UpdateDocumentLanguage()
        {
            var newValue = base.DocumentLanguage.SelectNewOptionReturningNewSelection();
            base.SubmitButton.Click();
            return newValue;
        }

        public string UpdateHomeLanguage()
        {
            var newValue = base.HomeLanguage.SelectNewOptionReturningNewSelection();
            base.SubmitButton.Click();
            return newValue;
        }

        public string UpdateEducationLevel()
        {
            var newValue = base.Education.SelectNewOptionReturningNewSelection();
            base.SubmitButton.Click();
            return newValue;
        }
    }
}