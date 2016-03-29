using Common.Enums;

using ObjectMaps.InternetComponents;

namespace BuildingBlocks.Presenters.InternetComponents.Calculators
{
    public class SwitchCalculator : SwitchCalculatorControls
    {
        public void PopulateCalculator
            (
                Automation.DataModels.LegalEntityEmployment employmentInfo,
                Automation.DataModels.Offer offerdetail
            )
        {
            base.MarketValue.Value = offerdetail.ClientEstimatePropertyValuation.ToString();
            base.CurrentLoanAmount.TypeText(offerdetail.ExistingLoan.ToString());
            base.CashOut.TypeText(offerdetail.CashOut.ToString());
            base.HouseholdIncome.TypeText(employmentInfo.MonthlyIncome.ToString());
            switch (employmentInfo.EmploymentType)
            {
                case EmploymentTypeEnum.Salaried:
                    {
                        base.SalariedCheck.Checked = true;
                        break;
                    }
                case EmploymentTypeEnum.SelfEmployed:
                    {
                        base.SelfEmployedCheck.Checked = true;
                        break;
                    }
            }
            switch (offerdetail.ProductType)
            {
                case ProductEnum.VariableLoan:
                    base.VariableRateLoanCheck.Checked = true;
                    break;

                case ProductEnum.NewVariableLoan:
                    base.VariableRateLoanCheck.Checked = true;
                    break;

                case ProductEnum.VariFixLoan:
                    base.FixedRateLoanCheck.Checked = true;
                    base.FixPercentage.TypeText(offerdetail.FixedPercentage.ToString());
                    break;
            }
            base.LoanTerm.Value = offerdetail.TermMonths.ToString();
            base.CapitaliseFeesCheck.Checked = offerdetail.IsCapitaliseFees;
            base.InterestOnlyCheck.Checked = offerdetail.IsInterestOnly;
        }

        public void Calculate()
        {
            base.Calculate.Click();
        }

        public void Apply()
        {
            base.Apply.Click();
        }
    }
}