using WatiN.Core;
using ObjectMaps;
using ObjectMaps.Pages;
namespace BuildingBlocks
{
    public static partial class Views
    {
        public class AdminPaymentDistributionAgentLegalEntityUpdate : AdminPaymentDistributionAgentLegalEntityUpdateControls
        {
            public void UpdateLegalEntityDetails_Company(
                string type,
                string organisationType,
                string coRegisteredName,
                string coRegistrationNumber,
                string workPhoneCode,
                string workPhoneNumber,
                string faxCode,
                string faxNumber,
                string emailAddress,
                btn buttonLabel)
            {

                if (!string.IsNullOrEmpty(type)) base.ddlLegalEntityTypes.Option(type).Select();
                if (!string.IsNullOrEmpty(organisationType)) base.ddlOrganisationType.Option(organisationType).Select();
                if (coRegisteredName != null) base.RegisteredNameUpdate.TypeText(coRegisteredName);
                if (coRegistrationNumber != null) base.txtRegistrationNumber.TypeText(coRegistrationNumber);
                if (workPhoneCode != null) base.txtWorkPhoneCode.TypeText(workPhoneCode);
                if (workPhoneNumber != null) base.txtWorkPhoneNumber.TypeText(workPhoneNumber);
                if (faxCode != null) base.txtFaxCode.TypeText(faxCode);
                if (faxNumber != null) base.txtFaxNumber.TypeText(faxNumber);
                if (emailAddress != null) base.txtEmailAddress.TypeText(emailAddress);

                ClickButton(buttonLabel);
            }

            public void UpdateLegalEntityDetails_NaturalPerson(
                string type,
                string organisationType,
                string idNumber,
                string salutation,
                string initials,
                string firstNames,
                string surname,
                string preferredname,
                string gender,
                string status,
                string homePhoneCode,
                string homePhoneNumber,
                string workPhoneCode,
                string workPhoneNumber,
                string faxCode,
                string faxNumber,
                string cellphoneNo,
                string emailAddress,
                btn buttonLabel)
            {
                if (!string.IsNullOrEmpty(type)) base.ddlLegalEntityTypes.Option(type).Select();
                if (!string.IsNullOrEmpty(organisationType)) base.ddlOrganisationType.Option(organisationType).Select();
                if (idNumber != null) base.txtIDNumber.TypeText(idNumber);
                if (!string.IsNullOrEmpty(salutation)) base.ddlSalutation.Option(salutation).Select();
                if (initials != null) base.Initials.TypeText(initials);
                if (firstNames != null) base.txtFirstNames.TypeText(firstNames);
                if (surname != null) base.txtSurname.TypeText(surname);
                if (preferredname != null) base.PreferredName.TypeText(preferredname);
                if (!string.IsNullOrEmpty(gender)) base.ddlGender.Option(gender).Select();
                if (!string.IsNullOrEmpty(status)) base.ddlStatus.Option(status).Select();
                if (homePhoneCode != null) base.txtHomePhoneCode.TypeText(homePhoneCode);
                if (homePhoneNumber != null) base.txtHomePhoneNumber.TypeText(homePhoneNumber);
                if (workPhoneCode != null) base.txtWorkPhoneCode.TypeText(workPhoneCode);
                if (workPhoneNumber != null) base.txtWorkPhoneNumber.TypeText(workPhoneNumber);
                if (faxCode != null) base.txtFaxCode.TypeText(faxCode);
                if (faxNumber != null) base.txtFaxNumber.TypeText(faxNumber);
                if (cellphoneNo != null) base.txtCellPhoneNumber.TypeText(cellphoneNo);
                if (emailAddress != null) base.txtEmailAddress.TypeText(emailAddress);

                ClickButton(buttonLabel);
            }

            public void UpdateLegalEntityDetails_Company(
                string coRegisteredName,
                string coRegisteredNumber,
                string workPhoneCode,
                string workPhoneNumber,
                btn buttonLabel)
            {
                UpdateLegalEntityDetails_Company(null, null, coRegisteredName, coRegisteredNumber, workPhoneCode, workPhoneNumber, null,
                    null, null, buttonLabel);
            }

            public void UpdateLegalEntityDetails_Company(
                string coRegisteredName,
                string workPhoneCode,
                string workPhoneNumber,
                btn buttonLabel)
            {
                UpdateLegalEntityDetails_Company(
                    null,
                    null,
                    coRegisteredName,
                    null,
                    workPhoneCode,
                    workPhoneNumber,
                    null,
                    null,
                    null,
                    buttonLabel);
            }
            public void UpdateLegalEntityDetails_NaturalPerson(
               string salutation,
               string firstNames,
               string surname,
               string homePhoneCode,
               string homePhoneNumber,
               btn buttonLabel)
            {
                UpdateLegalEntityDetails_NaturalPerson(
                    null,
                    null,
                    null,
                    salutation,
                    null,
                    firstNames,
                    surname,
                    null,
                    null,
                    null,
                    homePhoneCode,
                    homePhoneNumber,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    buttonLabel);

            }

            public enum btn
            {
                Update = 1,
                Cancel = 2,
                None = 3
            }

            public void ClickButton(btn buttonLabel)
            {
                switch (buttonLabel)
                {
                    case btn.Update:
                        base.nbtnSubmitButton.Click();
                        break;
                    case btn.Cancel:
                        base.btnCancelButton.Click();
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
