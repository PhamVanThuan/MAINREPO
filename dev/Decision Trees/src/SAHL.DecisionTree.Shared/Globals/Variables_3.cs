
namespace SAHL.DecisionTree.Shared.Globals
{
    public class Variables_3
    {
        static dynamic Enumerations;
        static dynamic Messages;

        public Variables_3(dynamic enumerations,dynamic messages)
        {
            Enumerations = enumerations;
            Messages = messages;
        }

		public class Capitec
		{
			public class Credit
			{
				public class Salaried
				{
					public class Category0
					{
						public class LoanSizeRange1
						{
							public double MaximumPaymentToIncome { get { return 0.30d; } }
						}
						public class LoanSizeRange2
						{
							public double MaximumPaymentToIncome { get { return 0.30d; } }
						}
						public class LoanSizeRange3
						{
							public double MaximumPaymentToIncome { get { return 0.25d; } }
						}

						public LoanSizeRange1 loanSizeRange1 = new LoanSizeRange1();
						public LoanSizeRange2 loanSizeRange2 = new LoanSizeRange2();
						public LoanSizeRange3 loanSizeRange3 = new LoanSizeRange3();

						public double MaximumLoanToValue { get { return 0.70d; } }
						public int MinimumApplicationEmpirica { get { return 575; } }
						public double CategoryLinkRate { get { return 0.028d; } }
						public double MinimumHouseholdIncome { get { return 13000.00d; } }
					}
					public class Category1
					{
						public class LoanSizeRange1
						{
							public double MaximumPaymentToIncome { get { return 0.30d; } }
						}
						public class LoanSizeRange2
						{
							public double MaximumPaymentToIncome { get { return 0.25d; } }
						}

						public LoanSizeRange1 loanSizeRange1 = new LoanSizeRange1();
						public LoanSizeRange2 loanSizeRange2 = new LoanSizeRange2();

						public double MaximumLoanToValue { get { return 0.80d; } }
						public int MinimumApplicationEmpirica { get { return 600; } }
						public double CategoryLinkRate { get { return 0.032d; } }
						public double MinimumHouseholdIncome { get { return 13000.00d; } }
					}
					public class Category3
					{
						public class LoanSizeRange1
						{
							public double MaximumPaymentToIncome { get { return 0.30d; } }
						}
						public class LoanSizeRange2
						{
							public double MaximumPaymentToIncome { get { return 0.25d; } }
						}

						public LoanSizeRange1 loanSizeRange1 = new LoanSizeRange1();
						public LoanSizeRange2 loanSizeRange2 = new LoanSizeRange2();

						public double MaximumLoanToValue { get { return 0.85d; } }
						public int MinimumApplicationEmpirica { get { return 610; } }
						public double CategoryLinkRate { get { return 0.037d; } }
						public double MinimumHouseholdIncome { get { return 20000.00d; } }
					}
					public class Category4
					{
						public class LoanSizeRange1
						{
							public double MaximumPaymentToIncome { get { return 0.30d; } }
						}
						public class LoanSizeRange2
						{
							public double MaximumPaymentToIncome { get { return 0.25d; } }
						}

						public LoanSizeRange1 loanSizeRange1 = new LoanSizeRange1();
						public LoanSizeRange2 loanSizeRange2 = new LoanSizeRange2();

						public double MaximumLoanToValue { get { return 0.90d; } }
						public int MinimumApplicationEmpirica { get { return 625; } }
						public double CategoryLinkRate { get { return 0.039d; } }
						public double MinimumHouseholdIncome { get { return 20000.00d; } }
					}
					public class Category5
					{
						public class LoanSizeRange1
						{
							public double MaximumPaymentToIncome { get { return 0.25d; } }
						}

						public LoanSizeRange1 loanSizeRange1 = new LoanSizeRange1();

						public double MaximumLoanToValue { get { return 0.95d; } }
						public int MinimumApplicationEmpirica { get { return 640; } }
						public double CategoryLinkRate { get { return 0.039d; } }
						public double MinimumHouseholdIncome { get { return 20000.00d; } }
					}

					public Category0 category0 = new Category0();
					public Category1 category1 = new Category1();
					public Category3 category3 = new Category3();
					public Category4 category4 = new Category4();
					public Category5 category5 = new Category5();

					public int MinimumLoanAmount { get { return 150000; } }
					public int MinimumHouseholdIncome { get { return 13000; } }
					public int MaximumLoanAmountLoanSizeRange1 { get { return 1800000; } }
					public int MaximumLoanAmountLoanSizeRange2 { get { return 2750000; } }
					public int MaximumLoanAmountLoanSizeRange3 { get { return 5000000; } }
					public int MaximumLoanAmount { get { return 5000000; } }
				}
				public class SalariedwithDeduction
				{
					public class Category0
					{
						public class LoanSizeRange1
						{
							public double MaximumPaymentToIncome { get { return 0.30d; } }
						}
						public class LoanSizeRange2
						{
							public double MaximumPaymentToIncome { get { return 0.30d; } }
						}
						public class LoanSizeRange3
						{
							public double MaximumPaymentToIncome { get { return 0.25d; } }
						}

						public LoanSizeRange1 loanSizeRange1 = new LoanSizeRange1();
						public LoanSizeRange2 loanSizeRange2 = new LoanSizeRange2();
						public LoanSizeRange3 loanSizeRange3 = new LoanSizeRange3();

						public double MaximumLoanToValue { get { return 0.70d; } }
						public double CategoryLinkRate { get { return 0.028d; } }
						public double MinimumHouseholdIncome { get { return 13000.00d; } }
						public int MinimumApplicationEmpirica { get { return 575; } }
					}
					public class Category1
					{
						public class LoanSizeRange1
						{
							public double MaximumPaymentToIncome { get { return 0.30d; } }
						}
						public class LoanSizeRange2
						{
							public double MaximumPaymentToIncome { get { return 0.25d; } }
						}

						public LoanSizeRange1 loanSizeRange1 = new LoanSizeRange1();
						public LoanSizeRange2 loanSizeRange2 = new LoanSizeRange2();

						public double MaximumLoanToValue { get { return 0.80d; } }
						public double CategoryLinkRate { get { return 0.032d; } }
						public double MinimumHouseholdIncome { get { return 13000.00d; } }
						public int MinimumApplicationEmpirica { get { return 575; } }
					}
					public class Category3
					{
						public class LoanSizeRange1
						{
							public double MaximumPaymentToIncome { get { return 0.30d; } }
						}
						public class LoanSizeRange2
						{
							public double MaximumPaymentToIncome { get { return 0.25d; } }
						}

						public LoanSizeRange1 loanSizeRange1 = new LoanSizeRange1();
						public LoanSizeRange2 loanSizeRange2 = new LoanSizeRange2();

						public double MaximumLoanToValue { get { return 0.85d; } }
						public double CategoryLinkRate { get { return 0.037d; } }
						public double MinimumHouseholdIncome { get { return 20000.00d; } }
						public int MinimumApplicationEmpirica { get { return 600; } }
					}
					public class Category4
					{
						public class LoanSizeRange1
						{
							public double MaximumPaymentToIncome { get { return 0.30d; } }
						}
						public class LoanSizeRange2
						{
							public double MaximumPaymentToIncome { get { return 0.25d; } }
						}

						public LoanSizeRange1 loanSizeRange1 = new LoanSizeRange1();
						public LoanSizeRange2 loanSizeRange2 = new LoanSizeRange2();

						public double MaximumLoanToValue { get { return 0.90d; } }
						public double CategoryLinkRate { get { return 0.039d; } }
						public double MinimumHouseholdIncome { get { return 20000.00d; } }
						public int MinimumApplicationEmpirica { get { return 610; } }
					}
					public class Category5
					{
						public class LoanSizeRange1
						{
							public double MaximumPaymentToIncome { get { return 0.25d; } }
						}

						public LoanSizeRange1 loanSizeRange1 = new LoanSizeRange1();

						public double MaximumLoanToValue { get { return 0.95d; } }
						public double CategoryLinkRate { get { return 0.039d; } }
						public double MinimumHouseholdIncome { get { return 20000.00d; } }
						public int MinimumApplicationEmpirica { get { return 620; } }
					}
					public class Category10
					{
						public class NewPurchase
						{
							public class LoanSizeRange1
							{
								public double MaximumPaymentToIncome { get { return 0.25d; } }
							}

							public LoanSizeRange1 loanSizeRange1 = new LoanSizeRange1();

							public double MaximumLoanToValue { get { return 1.00d; } }
						}

						public NewPurchase newPurchase = new NewPurchase();

						public double CategoryLinkRate { get { return 0.047d; } }
						public double MaximumLoanToValue { get { return 1.00d; } }
						public double MinimumHouseholdIncome { get { return 20000.00d; } }
						public int MinimumApplicationEmpirica { get { return 620; } }
					}

					public Category0 category0 = new Category0();
					public Category1 category1 = new Category1();
					public Category3 category3 = new Category3();
					public Category4 category4 = new Category4();
					public Category5 category5 = new Category5();
					public Category10 category10 = new Category10();

					public int MinimumLoanAmount { get { return 150000; } }
					public int MinimumHouseholdIncome { get { return 13000; } }
					public int MaximumLoanAmountLoanSizeRange1 { get { return 1800000; } }
					public int MaximumLoanAmountLoanSizeRange2 { get { return 2750000; } }
					public int MaximumLoanAmountLoanSizeRange3 { get { return 5000000; } }
					public int MinimumApplicationEmpirica { get { return 575; } }
					public int MaximumLoanAmount { get { return 5000000; } }
				}
				public class SelfEmployed
				{
					public class Category0
					{
						public double MaximumLoanToValue { get { return 0.70d; } }
						public int MinimumApplicationEmpirica { get { return 610; } }
						public double CategoryLinkRate { get { return 0.032d; } }
						public double MaximumPaymentToIncome { get { return 0.25d; } }
						public double MinimumHouseholdIncome { get { return 13000.00d; } }
					}
					public class Category1
					{
						public class LoanSizeRange1
						{
							public double MaximumPaymentToIncome { get { return 0.25d; } }
						}
						public class LoanSizeRange2
						{
							public double MaximumPaymentToIncome { get { return 0.20d; } }
						}

						public LoanSizeRange1 loanSizeRange1 = new LoanSizeRange1();
						public LoanSizeRange2 loanSizeRange2 = new LoanSizeRange2();

						public double MaximumLoanToValue { get { return 0.80d; } }
						public int MinimumApplicationEmpirica { get { return 610; } }
						public double CategoryLinkRate { get { return 0.036d; } }
						public double MinimumHouseholdIncome { get { return 13000.00d; } }
					}
					public class Category3
					{
						public class LoanSizeRange1
						{
							public double MaximumPaymentToIncome { get { return 0.25d; } }
						}
						public class LoanSizeRange2
						{
							public double MaximumPaymentToIncome { get { return 0.20d; } }
						}

						public LoanSizeRange1 loanSizeRange1 = new LoanSizeRange1();
						public LoanSizeRange2 loanSizeRange2 = new LoanSizeRange2();

						public double MaximumLoanToValue { get { return 0.85d; } }
						public int MinimumApplicationEmpirica { get { return 610; } }
						public double CategoryLinkRate { get { return 0.037d; } }
						public double MinimumHouseholdIncome { get { return 20000.00d; } }
					}

					public Category0 category0 = new Category0();
					public Category1 category1 = new Category1();
					public Category3 category3 = new Category3();

					public int MinimumLoanAmount { get { return 150000; } }
					public int MinimumHouseholdIncome { get { return 13000; } }
					public int MaximumLoanAmountLoanSizeRange1 { get { return 1800000; } }
					public int MaximumLoanAmountLoanSizeRange2 { get { return 2750000; } }
					public int MaximumLoanAmountLoanSizeRange3 { get { return 3500000; } }
					public int MaximumLoanAmount { get { return 3500000; } }
				}
				public class Alpha
				{
					public class Category6
					{
						public double MaximumLoanToValue { get { return 0.85d; } }
						public double MaximumPaymentToIncome { get { return 0.30d; } }
						public int MinimumApplicationEmpirica { get { return 600; } }
						public double CategoryLinkRate { get { return 0.039d; } }
					}
					public class Category7
					{
						public double MaximumLoanToValue { get { return 0.92d; } }
						public double MaximumPaymentToIncome { get { return 0.30d; } }
						public int MinimumApplicationEmpirica { get { return 600; } }
						public double CategoryLinkRate { get { return 0.043d; } }
					}
					public class Category8
					{
						public double MaximumLoanToValue { get { return 0.96d; } }
						public double MaximumPaymentToIncome { get { return 0.30d; } }
						public int MinimumApplicationEmpirica { get { return 610; } }
						public double CategoryLinkRate { get { return 0.046d; } }
					}
					public class Category9
					{
						public double MaximumLoanToValue { get { return 1.00d; } }
						public double MaximumPaymentToIncome { get { return 0.30d; } }
						public int MinimumApplicationEmpirica { get { return 610; } }
						public double CategoryLinkRate { get { return 0.047d; } }
						public int MinimumHouseholdIncome { get { return 10000; } }
					}

					public Category6 category6 = new Category6();
					public Category7 category7 = new Category7();
					public Category8 category8 = new Category8();
					public Category9 category9 = new Category9();

					public int MinimumLoanAmount { get { return 100000; } }
					public int MinimumHouseholdIncome { get { return 8000; } }
					public int MaximumHouseholdIncome { get { return 19999; } }
					public int MinimumPropertyValue { get { return 200000; } }
				}

				public Salaried salaried = new Salaried();
				public SalariedwithDeduction salariedwithDeduction = new SalariedwithDeduction();
				public SelfEmployed selfEmployed = new SelfEmployed();
				public Alpha alpha = new Alpha();

				public int MinimumApplicationEmpirica { get { return 575; } }
				public int MinimumApplicantAge { get { return 18; } }
				public int MaximumApplicantAge { get { return 65; } }
				public double PercentVarianceonPaymentToIncomeRatio { get { return 0.1d; } }
				public double PercentVarianceonCategoryEmpirica { get { return 0.015d; } }
				public double InvestmentPropertyMaximumLTV { get { return 0.8d; } }
			}

			public Credit credit = new Credit();

			public double AffordabilityMaximumPTI { get { return 0.30d; } }
			public double AffordabilityMaximumLTV { get { return 1.0d; } }
			public double AffordabilityMinimumHouseholdIncome { get { return 6000.00d; } }
			public double AffordabilityMaximumHouseholdIncome { get { return 250000.00d; } }
		}
		public class SAHomeLoans
		{
			public class NewBusiness
			{
				public class Credit
				{
					public class Salaried
					{
						public class Category0
						{
							public class LoanSizeRange1
							{
								public double MaximumPaymentToIncome { get { return 0.30d; } }
							}
							public class LoanSizeRange2
							{
								public double MaximumPaymentToIncome { get { return 0.3d; } }
							}
							public class LoanSizeRange3
							{
								public double MaximumPaymentToIncome { get { return 0.25d; } }
							}

							public LoanSizeRange1 loanSizeRange1 = new LoanSizeRange1();
							public LoanSizeRange2 loanSizeRange2 = new LoanSizeRange2();
							public LoanSizeRange3 loanSizeRange3 = new LoanSizeRange3();

							public double MaximumLoanToValue { get { return 0.7d; } }
							public int MinimumApplicationEmpirica { get { return 575; } }
							public double CategoryLinkRate { get { return 0.028d; } }
							public double MinimumHouseholdIncome { get { return 13000.00d; } }
						}
						public class Category1
						{
							public class LoanSizeRange1
							{
								public double MaximumPaymentToIncome { get { return 0.3d; } }
							}
							public class LoanSizeRange2
							{
								public double MaximumPaymentToIncome { get { return 0.25d; } }
							}

							public LoanSizeRange1 loanSizeRange1 = new LoanSizeRange1();
							public LoanSizeRange2 loanSizeRange2 = new LoanSizeRange2();

							public double MaximumLoanToValue { get { return 0.8d; } }
							public int MinimumApplicationEmpirica { get { return 575; } }
							public double CategoryLinkRate { get { return 0.032d; } }
							public double MinimumHouseholdIncome { get { return 13000.00d; } }
						}
						public class Category3
						{
							public class LoanSizeRange1
							{
								public double MaximumPaymentToIncome { get { return 0.3d; } }
							}
							public class LoanSizeRange2
							{
								public double MaximumPaymentToIncome { get { return 0.25d; } }
							}

							public LoanSizeRange1 loanSizeRange1 = new LoanSizeRange1();
							public LoanSizeRange2 loanSizeRange2 = new LoanSizeRange2();

							public double MaximumLoanToValue { get { return 0.85d; } }
							public int MinimumApplicationEmpirica { get { return 600; } }
							public double CategoryLinkRate { get { return 0.037d; } }
							public double MinimumHouseholdIncome { get { return 20000.00d; } }
						}
						public class Category4
						{
							public class LoanSizeRange1
							{
								public double MaximumPaymentToIncome { get { return 0.3d; } }
							}
							public class LoanSizeRange2
							{
								public double MaximumPaymentToIncome { get { return 0.25d; } }
							}

							public LoanSizeRange1 loanSizeRange1 = new LoanSizeRange1();
							public LoanSizeRange2 loanSizeRange2 = new LoanSizeRange2();

							public double MaximumLoanToValue { get { return 0.90d; } }
							public int MinimumApplicationEmpirica { get { return 610; } }
							public double CategoryLinkRate { get { return 0.039d; } }
							public double MinimumHouseholdIncome { get { return 20000.00d; } }
						}
						public class Category5
						{
							public class LoanSizeRange1
							{
								public double MaximumPaymentToIncome { get { return 0.25d; } }
							}

							public LoanSizeRange1 loanSizeRange1 = new LoanSizeRange1();

							public double MaximumLoanToValue { get { return 0.95d; } }
							public int MinimumApplicationEmpirica { get { return 630; } }
							public double CategoryLinkRate { get { return 0.039d; } }
							public double MinimumHouseholdIncome { get { return 20000.00d; } }
						}

						public Category0 category0 = new Category0();
						public Category1 category1 = new Category1();
						public Category3 category3 = new Category3();
						public Category4 category4 = new Category4();
						public Category5 category5 = new Category5();

						public int MinimumLoanAmount { get { return 150000; } }
						public int MinimumHouseholdIncome { get { return 13000; } }
						public int MaximumLoanAmountLoanSizeRange1 { get { return 1800000; } }
						public int MaximumLoanAmountLoanSizeRange2 { get { return 2750000; } }
						public int MaximumLoanAmountLoanSizeRange3 { get { return 5000000; } }
						public int MaximumLoanAmount { get { return 5000000; } }
					}
					public class SalariedwithDeduction
					{
						public class Category0
						{
							public class LoanSizeRange1
							{
								public double MaximumPaymentToIncome { get { return 0.3d; } }
							}
							public class LoanSizeRange2
							{
								public double MaximumPaymentToIncome { get { return 0.3d; } }
							}
							public class LoanSizeRange3
							{
								public double MaximumPaymentToIncome { get { return 0.25d; } }
							}

							public LoanSizeRange1 loanSizeRange1 = new LoanSizeRange1();
							public LoanSizeRange2 loanSizeRange2 = new LoanSizeRange2();
							public LoanSizeRange3 loanSizeRange3 = new LoanSizeRange3();

							public double MaximumLoanToValue { get { return 0.70d; } }
							public double CategoryLinkRate { get { return 0.028d; } }
							public double MinimumHouseholdIncome { get { return 13000.00d; } }
							public int MinimumApplicationEmpirica { get { return 575; } }
						}
						public class Category1
						{
							public class LoanSizeRange1
							{
								public double MaximumPaymentToIncome { get { return 0.3d; } }
							}
							public class LoanSizeRange2
							{
								public double MaximumPaymentToIncome { get { return 0.25d; } }
							}

							public LoanSizeRange1 loanSizeRange1 = new LoanSizeRange1();
							public LoanSizeRange2 loanSizeRange2 = new LoanSizeRange2();

							public double MaximumLoanToValue { get { return 0.80d; } }
							public double CategoryLinkRate { get { return 0.032d; } }
							public double MinimumHouseholdIncome { get { return 13000.00d; } }
							public int MinimumApplicationEmpirica { get { return 575; } }
						}
						public class Category3
						{
							public class LoanSizeRange1
							{
								public double MaximumPaymentToIncome { get { return 0.3d; } }
							}
							public class LoanSizeRange2
							{
								public double MaximumPaymentToIncome { get { return 0.25d; } }
							}

							public LoanSizeRange1 loanSizeRange1 = new LoanSizeRange1();
							public LoanSizeRange2 loanSizeRange2 = new LoanSizeRange2();

							public double MaximumLoanToValue { get { return 0.85d; } }
							public double CategoryLinkRate { get { return 0.037d; } }
							public double MinimumHouseholdIncome { get { return 20000.00d; } }
							public int MinimumApplicationEmpirica { get { return 575; } }
						}
						public class Category4
						{
							public class LoanSizeRange1
							{
								public double MaximumPaymentToIncome { get { return 0.3d; } }
							}
							public class LoanSizeRange2
							{
								public double MaximumPaymentToIncome { get { return 0.25d; } }
							}

							public LoanSizeRange1 loanSizeRange1 = new LoanSizeRange1();
							public LoanSizeRange2 loanSizeRange2 = new LoanSizeRange2();

							public double MaximumLoanToValue { get { return 0.90d; } }
							public double CategoryLinkRate { get { return 0.039d; } }
							public double MinimumHouseholdIncome { get { return 20000.00d; } }
							public int MinimumApplicationEmpirica { get { return 575; } }
						}
						public class Category5
						{
							public class LoanSizeRange1
							{
								public double MaximumPaymentToIncome { get { return 0.25d; } }
							}

							public LoanSizeRange1 loanSizeRange1 = new LoanSizeRange1();

							public double MaximumLoanToValue { get { return 0.95d; } }
							public double CategoryLinkRate { get { return 0.039d; } }
							public double MinimumHouseholdIncome { get { return 20000.00d; } }
							public int MinimumApplicationEmpirica { get { return 575; } }
						}
						public class Category10
						{
							public class NewPurchase
							{
								public class LoanSizeRange1
								{
									public double MaximumPaymentToIncome { get { return 0.25d; } }
								}

								public LoanSizeRange1 loanSizeRange1 = new LoanSizeRange1();

								public double MaximumLoanToValue { get { return 0.25d; } }
							}

							public NewPurchase newPurchase = new NewPurchase();

							public double CategoryLinkRate { get { return 0.047d; } }
							public double MaximumLoanToValue { get { return 1.0d; } }
							public double MinimumHouseholdIncome { get { return 20000.00d; } }
							public int MinimumApplicationEmpirica { get { return 595; } }
						}

						public Category0 category0 = new Category0();
						public Category1 category1 = new Category1();
						public Category3 category3 = new Category3();
						public Category4 category4 = new Category4();
						public Category5 category5 = new Category5();
						public Category10 category10 = new Category10();

						public int MinimumLoanAmount { get { return 150000; } }
						public int MinimumHouseholdIncome { get { return 13000; } }
						public int MaximumLoanAmountLoanSizeRange1 { get { return 1800000; } }
						public int MaximumLoanAmountLoanSizeRange2 { get { return 2750000; } }
						public int MaximumLoanAmountLoanSizeRange3 { get { return 5000000; } }
						public int MinimumApplicationEmpirica { get { return 575; } }
						public int MaximumLoanAmount { get { return 5000000; } }
					}
					public class SelfEmployed
					{
						public class Category0
						{
							public double MaximumLoanToValue { get { return 0.7d; } }
							public int MinimumApplicationEmpirica { get { return 595; } }
							public double CategoryLinkRate { get { return 0.032d; } }
							public double MaximumPaymentToIncome { get { return 0.25d; } }
							public double MinimumHouseholdIncome { get { return 13000.00d; } }
						}
						public class Category1
						{
							public class LoanSizeRange1
							{
								public double MaximumPaymentToIncome { get { return 0.25d; } }
							}
							public class LoanSizeRange2
							{
								public double MaximumPaymentToIncome { get { return 0.20d; } }
							}

							public LoanSizeRange1 loanSizeRange1 = new LoanSizeRange1();
							public LoanSizeRange2 loanSizeRange2 = new LoanSizeRange2();

							public double MaximumLoanToValue { get { return 0.8d; } }
							public int MinimumApplicationEmpirica { get { return 600; } }
							public double CategoryLinkRate { get { return 0.036d; } }
							public double MinimumHouseholdIncome { get { return 13000.00d; } }
						}
						public class Category3
						{
							public class LoanSizeRange1
							{
								public double MaximumPaymentToIncome { get { return 0.25d; } }
							}
							public class LoanSizeRange2
							{
								public double MaximumPaymentToIncome { get { return 0.20d; } }
							}

							public LoanSizeRange1 loanSizeRange1 = new LoanSizeRange1();
							public LoanSizeRange2 loanSizeRange2 = new LoanSizeRange2();

							public double MaximumLoanToValue { get { return 0.85d; } }
							public int MinimumApplicationEmpirica { get { return 610; } }
							public double CategoryLinkRate { get { return 0.037d; } }
							public double MinimumHouseholdIncome { get { return 20000.00d; } }
						}

						public Category0 category0 = new Category0();
						public Category1 category1 = new Category1();
						public Category3 category3 = new Category3();

						public int MinimumLoanAmount { get { return 150000; } }
						public int MinimumHouseholdIncome { get { return 13000; } }
						public int MaximumLoanAmountLoanSizeRange1 { get { return 1800000; } }
						public int MaximumLoanAmountLoanSizeRange2 { get { return 2750000; } }
						public int MaximumLoanAmountLoanSizeRange3 { get { return 3500000; } }
						public int MaximumLoanAmount { get { return 3500000; } }
					}
					public class Alpha
					{
						public class Category6
						{
							public double MaximumLoanToValue { get { return 0.85d; } }
							public double MaximumPaymentToIncome { get { return 0.30d; } }
							public int MinimumApplicationEmpirica { get { return 600; } }
							public double CategoryLinkRate { get { return 0.039d; } }
						}
						public class Category7
						{
							public double MaximumLoanToValue { get { return 0.92d; } }
							public double MaximumPaymentToIncome { get { return 0.30d; } }
							public int MinimumApplicationEmpirica { get { return 600; } }
							public double CategoryLinkRate { get { return 0.043d; } }
						}
						public class Category8
						{
							public double MaximumLoanToValue { get { return 0.96d; } }
							public double MaximumPaymentToIncome { get { return 0.30d; } }
							public int MinimumApplicationEmpirica { get { return 610; } }
							public double CategoryLinkRate { get { return 0.046d; } }
						}
						public class Category9
						{
							public double MaximumLoanToValue { get { return 1.0d; } }
							public double MaximumPaymentToIncome { get { return 0.30d; } }
							public int MinimumApplicationEmpirica { get { return 610; } }
							public double CategoryLinkRate { get { return 0.047d; } }
							public int MinimumHouseholdIncome { get { return 10000; } }
						}

						public Category6 category6 = new Category6();
						public Category7 category7 = new Category7();
						public Category8 category8 = new Category8();
						public Category9 category9 = new Category9();

						public int MinimumLoanAmount { get { return 100000; } }
						public int MinimumHouseholdIncome { get { return 8000; } }
						public int MaximumHouseholdIncome { get { return 19999; } }
						public int MinimumPropertyValue { get { return 200000; } }
					}

					public Salaried salaried = new Salaried();
					public SalariedwithDeduction salariedwithDeduction = new SalariedwithDeduction();
					public SelfEmployed selfEmployed = new SelfEmployed();
					public Alpha alpha = new Alpha();

					public int MinimumApplicationEmpirica { get { return 575; } }
					public int MinimumApplicantAge { get { return 18; } }
					public int MaximumApplicantAge { get { return 65; } }
					public double PercentVarianceonPaymentToIncomeRatio { get { return 0.10d; } }
					public double PercentVarianceonCategoryEmpirica { get { return 0.015d; } }
					public double InvestmentPropertyMaximumLTV { get { return 0.80d; } }
				}

				public Credit credit = new Credit();

				public double AffordabilityMaximumPTI { get { return 0.30d; } }
				public double AffordabilityMaximumLTV { get { return 1.0d; } }
			}
			public class Rates
			{
				public double JIBAR3MonthRounded { get { return 0.061d; } }
			}
			public class SpecialPurposeVehicle
			{
				public class AllocationWeighting
				{
					public class Alpha
					{
						public class AlphaHousingWarehousePtyLtd
						{
							public int Precedence { get { return 2; } }
							public double Weighting { get { return 0.7d; } }
						}
						public class BlueBannerAlphaAccount
						{
							public int Precedence { get { return 1; } }
							public double Weighting { get { return 0.3d; } }
						}

						public AlphaHousingWarehousePtyLtd alphaHousingWarehousePtyLtd = new AlphaHousingWarehousePtyLtd();
						public BlueBannerAlphaAccount blueBannerAlphaAccount = new BlueBannerAlphaAccount();
					}

					public Alpha alpha = new Alpha();
				}

				public AllocationWeighting allocationWeighting = new AllocationWeighting();
			}

			public NewBusiness newBusiness = new NewBusiness();
			public Rates rates = new Rates();
			public SpecialPurposeVehicle specialPurposeVehicle = new SpecialPurposeVehicle();
		}

		public Capitec capitec = new Capitec();
		public SAHomeLoans sAHomeLoans = new SAHomeLoans();
    }
}