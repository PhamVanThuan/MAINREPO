using SAHL.Core.Validation;
using System;

namespace SAHL.Services.Interfaces.ApplicationDomain.Models
{
    public class AffordabilityAssessmentDetailModel : ValidatableModel
    {
        public AffordabilityAssessmentDetailModel()
        {
        }

        public AffordabilityAssessmentDetailModel(AffordabilityAssessmentIncomeDetailModel income, 
                                                  AffordabilityAssessmentIncomeDeductionsDetailModel incomeDeductions, 
                                                  AffordabilityAssessmentNecessaryExpensesDetailModel necessaryExpenses, 
                                                  AffordabilityAssessmentPaymentObligationDetailModel paymentObligations, 
                                                  AffordabilityAssessmentSAHLPaymentObligationDetailModel sahlPaymentObligations, 
                                                  AffordabilityAssessmentOtherExpensesDetailModel otherExpenses, 
                                                  string stressFactorPercentageDisplay, 
                                                  double stressFactorPercentageIncrease, 
                                                  int minimumMonthlyFixedExpenses)
        {
            this.Income = income;
            this.IncomeDeductions = incomeDeductions;
            this.NecessaryExpenses = necessaryExpenses;
            this.PaymentObligations = paymentObligations;
            this.SAHLPaymentObligations = sahlPaymentObligations;
            this.OtherExpenses = otherExpenses;
            this.StressFactorPercentageDisplay = stressFactorPercentageDisplay;
            this.StressFactorPercentageIncrease = stressFactorPercentageIncrease;
            this.MinimumMonthlyFixedExpenses = minimumMonthlyFixedExpenses;
        }

        public int AllocableIncome_Client { get { return NetIncome_Client - NecessaryExpenses.MonthlyTotal_Client; } }

        public int AllocableIncome_Credit { get { return NetIncome_Credit - (NecessaryExpenses.MonthlyTotal_Credit + AppliedNCROverride); } }

        public int AppliedNCROverride
        {
            get
            {
                if (MinimumMonthlyFixedExpenses > NecessaryExpenses.MonthlyTotal_Credit)
                {
                    return MinimumMonthlyFixedExpenses - NecessaryExpenses.MonthlyTotal_Credit;
                }
                return 0;
            }
        }

        //Summary
        public int? DebtToConsolidate
        {
            get
            {
                if (PaymentObligations.MonthlyTotal_DebtToConsolidate > 0)
                {
                    return PaymentObligations.MonthlyTotal_DebtToConsolidate;
                }
                else
                {
                    return null;
                }
            }
        }

        public int DiscretionaryIncome_Client { get { return AllocableIncome_Client - PaymentObligations.MonthlyTotal_Client; } }

        public int DiscretionaryIncome_Credit { get { return AllocableIncome_Credit - PaymentObligations.MonthlyTotal_Credit; } }

        public int DiscretionaryIncome_ToBe { get { return AllocableIncome_Credit - PaymentObligations.MonthlyTotal_ToBe; } }

        public AffordabilityAssessmentIncomeDetailModel Income { get; set; }

        public AffordabilityAssessmentIncomeDeductionsDetailModel IncomeDeductions { get; set; }

        public int MinimumMonthlyFixedExpenses { get; set; }

        public AffordabilityAssessmentNecessaryExpensesDetailModel NecessaryExpenses { get; set; }

        public int NetIncome_Client { get { return Income.GrossIncome_Client - IncomeDeductions.GrossIncomeDeductions_Client; } }

        public int NetIncome_Credit { get { return Income.GrossIncome_Credit - IncomeDeductions.GrossIncomeDeductions_Credit; } }

        public AffordabilityAssessmentOtherExpensesDetailModel OtherExpenses { get; set; }

        public AffordabilityAssessmentPaymentObligationDetailModel PaymentObligations { get; set; }

        public AffordabilityAssessmentSAHLPaymentObligationDetailModel SAHLPaymentObligations { get; set; }
        public string StressFactorPercentageDisplay { get; set; }

        public double StressFactorPercentageIncrease { get; set; }

        public int StressFactorValue 
        { 
            get 
            { 
                return Convert.ToInt32((StressFactorPercentageIncrease * (PaymentObligations.OtherBonds.ToBeValue + (SAHLPaymentObligations.SAHLBond.ClientValue ?? 0)))); 
            } 
        }

        public int Surplus_Deficit { get { return (NetIncome_Credit - TotalExpenses); } }

        public int Surplus_Deficit_Client { get { return DiscretionaryIncome_Client - (SAHLPaymentObligations.MonthlyTotal_Client + OtherExpenses.MonthlyTotal_Client); } }

        public int Surplus_Deficit_Credit { get { return DiscretionaryIncome_Credit - (SAHLPaymentObligations.MonthlyTotal_Credit + OtherExpenses.MonthlyTotal_Credit); } }

        public int Surplus_Deficit_ToBe { get { return DiscretionaryIncome_ToBe - (SAHLPaymentObligations.MonthlyTotal_Credit + OtherExpenses.MonthlyTotal_Credit); } }

        public int SurplusToBeAfterStressFactorApplied { get { return Surplus_Deficit_ToBe - StressFactorValue; } }

        public int SurplusToNetHouseholdIncomePercentage 
        { 
            get 
            { 
                return NetIncome_Credit > 0 ? Convert.ToInt32((Convert.ToDouble(Surplus_Deficit) / Convert.ToDouble(NetIncome_Credit)) * 100D) : 0; 
            } 
        }

        public int TotalExpenses 
        { 
            get 
            { 
                return NecessaryExpenses.MonthlyTotal_Credit 
                     + AppliedNCROverride 
                     + PaymentObligations.MonthlyTotal_ToBe 
                     + SAHLPaymentObligations.MonthlyTotal_Credit 
                     + OtherExpenses.MonthlyTotal_Credit; 
            } 
        }
    }
}