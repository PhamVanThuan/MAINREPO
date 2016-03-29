using NUnit.Framework;
using ObjectMaps.Presenters.Admin;

namespace BuildingBlocks.Presenters.Admin
{
    public class ViewMyProfileDetails : MyProfileDetailsControls
    {

        public void AssertLegalEntityDetailsAreDisplayed(Automation.DataModels.LegalEntity legalEntity)
        {
            Assert.That(base.FirstName.Text == legalEntity.FirstNames);
            Assert.That(base.Surname.Text == legalEntity.Surname);
            Assert.That(base.PreferredName.Text == legalEntity.PreferredName);
            Assert.That(base.Salutation.Text == legalEntity.SalutationDescription);
            Assert.That(base.EmailAddress.Text == legalEntity.EmailAddress);
            Assert.That(base.CellphoneNumber.Text == legalEntity.CellPhoneNumber);
            Assert.That(base.WorkPhone.Text == FormatPhoneNumber(legalEntity.WorkPhoneCode, legalEntity.WorkPhoneNumber));
            Assert.That(base.HomePhone.Text == FormatPhoneNumber(legalEntity.HomePhoneCode, legalEntity.HomePhoneNumber));
            Assert.That(base.FaxNumber.Text == FormatPhoneNumber(legalEntity.FaxCode, legalEntity.FaxNumber));
            Assert.That(base.Education.Text == legalEntity.EducationDescription);
            Assert.That(base.HomeLanguage.Text == legalEntity.HomeLanguageDescription);
            Assert.That(base.PreferredName.Text == legalEntity.PreferredName);
        }

        private string FormatPhoneNumber(string code, string number)
        {
            return string.Format(@"({0}) {1}", code, number);
        }
    }
}