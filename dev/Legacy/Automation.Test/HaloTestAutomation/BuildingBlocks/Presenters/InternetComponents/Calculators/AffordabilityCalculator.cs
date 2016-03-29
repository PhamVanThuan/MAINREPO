using ObjectMaps.InternetComponents;

namespace BuildingBlocks.Presenters.InternetComponents.Calculators
{
    public class AffordabilityCalculator : AffordabilityCalculatorControls
    {
        public void PopulateCalculator
            (
                Automation.DataModels.LegalEntityEmployment employmentInfo,
                Automation.DataModels.Offer offerdetail
            )
        {
            base.InterestRate.Value = offerdetail.InterestRate.ToString();
            base.LoanTermYears.Value = offerdetail.TermYears.ToString();
            base.MonthlyInstalment.Value = offerdetail.MonthlyInstalment.ToString();
            base.OtherContribution.Value = offerdetail.OtherContributions.ToString();
            base.ProfitFromSale.Value = offerdetail.ProfitFromSale.ToString();
            base.SalaryOne.Value = employmentInfo.Income01.ToString();
            base.SalaryTwo.Value = employmentInfo.Income02.ToString();
        }

        public void Apply()
        {
            base.Apply.FireEvent("onclick");
        }

        public void Calculate()
        {
            base.Calculate.Click();
        }
    }
}