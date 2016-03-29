using NUnit.Framework;

namespace BuildingBlocks.Presenters.Admin
{
    public abstract class EmployerBase : ObjectMaps.Pages.EmployerDetailsControls
    {
        public virtual void Populate(Automation.DataModels.Employer employer)
        {
            base.BusinessType.Option(employer.EmployerBusinessTypeDescription).Select();
            base.EmploymentSector.Option(employer.EmploymentSectorDescription).Select();

            base.ContactPerson.Value = (employer.ContactPerson);
            base.ContactPersonPhoneCode.Value = (employer.TelephoneCode);
            base.ContactPersonPhoneNumber.Value = (employer.TelephoneNumber);
            base.ContactEmail.Value = (employer.ContactEmail);

            base.Accountant.Value = (employer.AccountantName);
            base.AccountantContact.Value = (employer.AccountantContactPerson);
            base.AccountantEmail.Value = (employer.AccountantEmail);
            base.AccountantNumber__CODE.Value = (employer.AccountantTelephoneCode);
            base.AccountantNumber__NUMB.Value = (employer.AccountantTelephoneNumber);
        }

        public void Cancel()
        {
            base.CancelButton.Click();
        }

        public void AssertBusinessTypeMandatoryMessage()
        {
            Assert.True(base.divValidationSummaryBody.Text.Contains("Employer Business Type is a mandatory field"));
        }

        public void AssertEmploymentSectorMandatoryMessage()
        {
            Assert.True(base.divValidationSummaryBody.Text.Contains("Employment Sector is a mandatory field"));
        }

        public void AssertContactPersonMandatoryMessage()
        {
            Assert.True(base.divValidationSummaryBody.Text.Contains("Contact Person is a mandatory field"));
        }

        public void AssertTelephoneCodeMandatoryMessage()
        {
            Assert.True(base.divValidationSummaryBody.Text.Contains("Telephone Code is a mandatory field"));
        }

        public void AssertTelephoneNumberMandatoryMessage()
        {
            Assert.True(base.divValidationSummaryBody.Text.Contains("Telephone Number is a mandatory field"));
        }
    }
}