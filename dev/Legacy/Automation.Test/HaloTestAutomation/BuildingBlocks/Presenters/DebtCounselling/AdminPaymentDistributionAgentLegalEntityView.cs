using WatiN.Core;
using ObjectMaps;
using ObjectMaps.Pages;
namespace BuildingBlocks
{
        public class AdminPaymentDistributionAgentLegalEntityView : AdminPaymentDistributionAgentLegalEntityViewControls
        {
            public void GetCompanyLegalEntityDetails( 
                out string type, 
                out string organisationType, 
                out string companyName, 
                out string registrationNumber, 
                out string workPhone, 
                out string faxNumber, 
                out string emailAddress)
            {
                type = base.lblLEType.Text;
                organisationType = base.lblOrganisationTypeDisplay.Text;
                companyName = base.lblCORegisteredName.Text;
                registrationNumber = base.lblCORegistrationNumber.Text;
                workPhone = base.lblWorkPhone.Text;
                faxNumber = base.lblFaxNumber.Text;
                emailAddress = base.lblEmailAddress.Text;
            }

            public void GetCompanyLegalEntityDetails(
                out string type,
                out string organisationType,
                out string companyName,
                out string workPhone,
                out string faxNumber,
                out string emailAddress)
            {
                type = base.lblLEType.Text;
                organisationType = base.lblOrganisationTypeDisplay.Text;
                companyName = base.lblCORegisteredName.Text;
                workPhone = base.lblWorkPhone.Text;
                faxNumber = base.lblFaxNumber.Text;
                emailAddress = base.lblEmailAddress.Text;
            }

            public void GetCompanyLegalEntityDetails( 
                out string companyName, 
                out string workPhone)
            {
                string type;
                string organisationType;
                string faxNumber;
                string emailAddress;

                GetCompanyLegalEntityDetails(out type, out organisationType, out companyName, out workPhone, out faxNumber, out emailAddress);
            }

            public void GetCompanyLegalEntityDetails( 
                out string companyName,
                out string registrationNumber, 
                out string workPhone)
            {
                string type;
                string organisationType;
                string faxNumber;
                string emailAddress;

                GetCompanyLegalEntityDetails(out type, out organisationType, out companyName, out registrationNumber, out workPhone, out faxNumber, out emailAddress);
            }

            public void GetNaturalPersonLegalEntityDetails(
                out string type,
                out string role,
                out string introductionDate,
                out string idNumber,
                out string salutation,
                out string initials,
                out string firstName,
                out string surname,
                out string preferredName,
                out string gender,
                out string status,
                out string homePhone,
                out string workPhone,
                out string faxNumber,
                out string cellphoneNo,
                out string emailAddress)
            {
                type = base.lblLEType.Text;
                role = base.lblRole.Text;
                introductionDate = base.lblNatIntroductionDate.Text;
                idNumber = base.lblIDNumber.Text;
                salutation = base.lblSalutation.Text;
                initials = base.lblInitials.Text;
                firstName = base.lblFirstNames.Text;
                surname = base.lblSurname.Text;
                preferredName = base.lblPreferredName.Text;
                gender = base.lblGender.Text;
                status = base.lblStatus.Text;
                homePhone = base.lblHomePhone.Text;
                workPhone = base.lblWorkPhone.Text;
                faxNumber = base.lblFaxNumber.Text;
                cellphoneNo = base.lblCellphoneNumber.Text;
                emailAddress = base.lblEmailAddress.Text;
            }

            public void GetNaturalPersonLegalEntityDetails(out string salutation, out string firstName,
               out string surname, out string homePhone)
            {
                string type;
                string role;
                string introductionDate;
                string idNumber;
                string initials;
                string preferredName;
                string gender;
                string status;
                string workPhone;
                string faxNumber;
                string cellphoneNo;
                string emailAddress;

                GetNaturalPersonLegalEntityDetails(out type, out role, out introductionDate,out idNumber,
                    out salutation, out initials,out firstName, out surname, out preferredName,
                    out gender, out status, out homePhone, out workPhone,out faxNumber, out cellphoneNo,
                    out emailAddress);
            }

        }
}
