using Common.Enums;

namespace BuildingBlocks.Websites
{
    public class NewPurchaseCalculator : ObjectMaps.InternetComponents.NewPurchaseCalculatorControls
    {
        public void PopulateCalculator
            (
                Automation.DataModels.LegalEntityEmployment employmentInfo,
                Automation.DataModels.Offer offerdetail
            )
        {
            base.PurchasePrice.TypeText(offerdetail.PurchasePrice.ToString());
            base.HouseholdIncome.TypeText(employmentInfo.MonthlyIncome.ToString());
            base.CashDeposit.TypeText(offerdetail.CashDeposit.ToString());
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
            base.LoanTerm.TypeText(offerdetail.TermMonths.ToString());
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