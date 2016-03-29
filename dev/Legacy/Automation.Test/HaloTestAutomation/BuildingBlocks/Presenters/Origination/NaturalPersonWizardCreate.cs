using ObjectMaps.Pages;

namespace BuildingBlocks.Presenters.Origination
{
    public class NaturalPersonWizardCreate : ApplicationWizardApplicantControls
    {
        public void ApplicationWizardApplicant(string marketingSource, string id, string firstnames, string surname,
            string phoneCode, string phoneNumber, bool submit)
        {
            base.selectMarketingSource.Select(marketingSource);
            base.textfieldIdentityNumber.TypeText(id);
            base.textfieldFirstNames.TypeText(firstnames);
            base.textfieldSurname.TypeText(surname);
            base.textfieldContactCode.TypeText(phoneCode);
            base.textfieldContactNumber.TypeText(phoneNumber);
            if (submit) base.btnNext.Click();
        }

        public void ApplicationWizardApplicant(string marketingSource, string id, string firstnames,
            string surname, string phoneCode, string phoneNumber)
        {
            base.selectMarketingSource.Select(marketingSource);
            base.textfieldIdentityNumber.TypeText(id);
            base.textfieldFirstNames.TypeText(firstnames);
            base.textfieldSurname.TypeText(surname);
            base.textfieldContactCode.TypeText(phoneCode);
            base.textfieldContactNumber.TypeText(phoneNumber);
            base.btnNext.Click();
        }
    }
}