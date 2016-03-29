namespace BuildingBlocks.Presenters.InternetComponents.Calculators
{
    public class CalculatorApplicationForm : ObjectMaps.InternetComponents.CalculatorApplicationFormControls
    {
        public void PopulateApplicantDetail
          (
              Automation.DataModels.LegalEntity clientInfo,
              int noApplicants = 1
          )
        {
            base.FirstNames.Value = clientInfo.FirstNames;
            base.Surname.Value = clientInfo.Surname;
            base.PhoneCode.Value = clientInfo.WorkPhoneCode;
            base.PhoneNumber.Value = clientInfo.WorkPhoneNumber;
            base.NumApplicants.Value = noApplicants.ToString();
            base.EmailAddress.Value = clientInfo.EmailAddress;
        }

        public void Submit()
        {
            base.SumitApplication.Click();
        }
    }
}