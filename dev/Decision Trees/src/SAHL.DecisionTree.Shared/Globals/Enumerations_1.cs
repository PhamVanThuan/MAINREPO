namespace SAHL.DecisionTree.Shared.Globals
{
    public class Enumerations_1
    {
		public class SAHomeLoans
		{
			public class Credit
			{
				public class CreditMatrixCategory
				{
					 public string SalariedCategory0 { get { return "Globals.Enumerations.sAHomeLoans.credit.creditMatrixCategory.SalariedCategory0"; } }
					 public string SalariedCategory1 { get { return "Globals.Enumerations.sAHomeLoans.credit.creditMatrixCategory.SalariedCategory1"; } }
					 public string SalariedCategory3 { get { return "Globals.Enumerations.sAHomeLoans.credit.creditMatrixCategory.SalariedCategory3"; } }
					 public string SalariedCategory4 { get { return "Globals.Enumerations.sAHomeLoans.credit.creditMatrixCategory.SalariedCategory4"; } }
					 public string SalariedCategory5 { get { return "Globals.Enumerations.sAHomeLoans.credit.creditMatrixCategory.SalariedCategory5"; } }
					 public string SalariedwithDeductionCategory0 { get { return "Globals.Enumerations.sAHomeLoans.credit.creditMatrixCategory.SalariedwithDeductionCategory0"; } }
					 public string SalariedwithDeductionCategory1 { get { return "Globals.Enumerations.sAHomeLoans.credit.creditMatrixCategory.SalariedwithDeductionCategory1"; } }
					 public string SalariedwithDeductionCategory3 { get { return "Globals.Enumerations.sAHomeLoans.credit.creditMatrixCategory.SalariedwithDeductionCategory3"; } }
					 public string SalariedwithDeductionCategory4 { get { return "Globals.Enumerations.sAHomeLoans.credit.creditMatrixCategory.SalariedwithDeductionCategory4"; } }
					 public string SalariedwithDeductionCategory5 { get { return "Globals.Enumerations.sAHomeLoans.credit.creditMatrixCategory.SalariedwithDeductionCategory5"; } }
					 public string SalariedwithDeductionCategory10 { get { return "Globals.Enumerations.sAHomeLoans.credit.creditMatrixCategory.SalariedwithDeductionCategory10"; } }
					 public string SelfEmployedCategory0 { get { return "Globals.Enumerations.sAHomeLoans.credit.creditMatrixCategory.SelfEmployedCategory0"; } }
					 public string SelfEmployedCategory1 { get { return "Globals.Enumerations.sAHomeLoans.credit.creditMatrixCategory.SelfEmployedCategory1"; } }
					 public string SelfEmployedCategory3 { get { return "Globals.Enumerations.sAHomeLoans.credit.creditMatrixCategory.SelfEmployedCategory3"; } }
					 public string AlphaCategory6 { get { return "Globals.Enumerations.sAHomeLoans.credit.creditMatrixCategory.AlphaCategory6"; } }
					 public string AlphaCategory7 { get { return "Globals.Enumerations.sAHomeLoans.credit.creditMatrixCategory.AlphaCategory7"; } }
					 public string AlphaCategory8 { get { return "Globals.Enumerations.sAHomeLoans.credit.creditMatrixCategory.AlphaCategory8"; } }
					 public string AlphaCategory9 { get { return "Globals.Enumerations.sAHomeLoans.credit.creditMatrixCategory.AlphaCategory9"; } }
				}
				public CreditMatrixCategory creditMatrixCategory = new CreditMatrixCategory();
			}

			public Credit credit = new Credit();

			public class PropertyOccupancyType
			{
				 public string OwnerOccupied { get { return "Globals.Enumerations.sAHomeLoans.propertyOccupancyType.OwnerOccupied"; } }
				 public string InvestmentProperty { get { return "Globals.Enumerations.sAHomeLoans.propertyOccupancyType.InvestmentProperty"; } }
			}
			public class HouseholdIncomeType
			{
				 public string Salaried { get { return "Globals.Enumerations.sAHomeLoans.householdIncomeType.Salaried"; } }
				 public string SelfEmployed { get { return "Globals.Enumerations.sAHomeLoans.householdIncomeType.SelfEmployed"; } }
				 public string SalariedwithDeduction { get { return "Globals.Enumerations.sAHomeLoans.householdIncomeType.SalariedwithDeduction"; } }
			}
			public class MortgageLoanApplicationType
			{
				 public string Switch { get { return "Globals.Enumerations.sAHomeLoans.mortgageLoanApplicationType.Switch"; } }
				 public string NewPurchase { get { return "Globals.Enumerations.sAHomeLoans.mortgageLoanApplicationType.NewPurchase"; } }
			}
			public PropertyOccupancyType propertyOccupancyType = new PropertyOccupancyType();
			public HouseholdIncomeType householdIncomeType = new HouseholdIncomeType();
			public MortgageLoanApplicationType mortgageLoanApplicationType = new MortgageLoanApplicationType();
		}

		public SAHomeLoans sAHomeLoans = new SAHomeLoans();
    }
}