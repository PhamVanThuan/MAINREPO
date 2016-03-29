using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.DecisionTree.Shared.Specs
{
    class Shared
    {
        public static string GlobalsVersion =
        #region GlobalsVersion
        @"{
          'VariablesVersion': 2,
          'MessagesVersion': 2,
          'EnumerationsVersion': 2,
          '_name': 'SAHL.DecisionTree.Shared.QueryGlobalsVersion,SAHL.DecisionTree.Shared'
        }";
        #endregion GlobalsVersion

        public static string TestMessagesClass =
        #region Messages
         @"
        using SAHL.Core.SystemMessages;
        using System.Collections.Generic;
        using System.Linq;

        namespace SAHL.DecisionTree.Shared.Globals
        {
            public class Messages_2
            {
                private ISystemMessageCollection messages;
                public ISystemMessageCollection SystemMessages
                {
                    get { return messages; }
                } 
        
                public Messages_2(ISystemMessageCollection messagesCollection)
                {
                    this.messages = messagesCollection;
                }

                public void AddError(string errorMessage)
                {
                    if(!SystemMessages.ErrorMessages().Any(e => e.Message.Equals(errorMessage)))
                    {
                        this.messages.AddMessage(new SystemMessage(errorMessage, SystemMessageSeverityEnum.Error));
                    }
                }

                public void AddWarning(string warningMessage)
                {
                    if(!SystemMessages.WarningMessages().Any(w => w.Message.Equals(warningMessage)))
                    {
                        this.messages.AddMessage(new SystemMessage(warningMessage, SystemMessageSeverityEnum.Warning));
                    }
                }

                public void AddInfo(string infoMessage)
                {
                    if(!SystemMessages.InfoMessages().Any(i => i.Message.Equals(infoMessage)))
                    {
                        this.messages.AddMessage(new SystemMessage(infoMessage, SystemMessageSeverityEnum.Info));
                    }
                }

                public void AddDebugInfo(string debugMessage)
                {
                    if(!SystemMessages.DebugMessages().Any(d => d.Message.Equals(debugMessage)))
                    {
                        this.messages.AddMessage(new SystemMessage(debugMessage, SystemMessageSeverityEnum.Debug));
                    }
                }

                public class Capitec
                {
                    public class Credit
                    {
                        public class Salaried
                        {
                            public string HouseholdIncomeBelowMinimum { get { return ""\""Total income is below the required minimum for salaried applicants.\""""; } }
                            public string LoanAmountBelowMinimum { get { return ""\""Loan amount requested is below the product minimum.\""""; } }
                            public string LoanAmountAboveMaximum { get { return ""\""Loan amount requested is below the product maximum.\""""; } }
                            public string LTVAboveMaximum { get { return ""\""The application LTV is above the maximum allowed for salaried applicants.\""""; } }
                            public string CreditScoreBelowMinimum { get { return ""\""The best Credit Score applicable for this application is below the minimum requirement for salaried applicants.\""""; } }
                            public string PTIAboveMaximum { get { return ""\""Affordability: Loan repayment to income ratio (PTI) is above the maximum limit. Reducing the loan amount may make the loan more affordable.\""""; } }
                        }
                        public class SalariedwithDeduction
                        {
                            public string HouseholdIncomeBelowMinimum { get { return ""\""Total income is below the required minimum for salaried with deduction applicants.\""""; } }
                            public string LoanAmountBelowMinimum { get { return ""\""Loan amount requested is below the product minimum.\""""; } }
                            public string LoanAmountAboveMaximum { get { return ""\""Loan amount requested is above the product maximum.\""""; } }
                            public string LTVAboveMaximum { get { return ""\""The application LTV is above the maximum allowed for salaried with deduction applicants.\""""; } }
                            public string CreditScoreBelowMinimum { get { return ""\""The best Credit Score applicable for this application is below the minimum requirement for salaried with deduction applicants.\""""; } }
                            public string PTIAboveMaximum { get { return ""\""Affordability: Loan repayment to income ratio (PTI) is above the maximum limit. Reducing the loan amount may make the loan more affordable.\""""; } }
                        }
                        public class SelfEmployed
                        {
                            public string HouseholdIncomeBelowMinimum { get { return ""\""Total income is below the required minimum for self-employed applicants.\""""; } }
                            public string LoanAmountBelowMinimum { get { return ""\""Loan amount requested is below the product minimum.\""""; } }
                            public string LoanAmountAboveMaximum { get { return ""\""Loan amount requested is below the product maximum.\""""; } }
                            public string LTVAboveMaximum { get { return ""\""The application LTV is above the maximum allowed for self-employed applicants.\""""; } }
                            public string CreditScoreBelowMinimum { get { return ""\""The best Credit Score applicable for this application is below the minimum requirement for self-employed applicants.\""""; } }
                            public string PTIAboveMaximum { get { return ""\""Affordability: Loan repayment to income ratio (PTI) is above the maximum limit. Reducing the loan amount may make the loan more affordable.\""""; } }
                        }
                        public class Alpha
                        {
                            public string HouseholdIncomeBelowMinimum { get { return ""\""Total income is below the required minimum for the product.\""""; } }
                            public string HouseholdIncomeAboveMaximum { get { return ""\""Total income is above the maximum for the product.\""""; } }
                            public string LoanAmountBelowMinimum { get { return ""\""Loan amount requested is below the product minimum.\""""; } }
                            public string LoanAmountAboveMaximum { get { return ""\""Loan amount requested is below the product maximum.\""""; } }
                            public string LTVAboveMaximum { get { return ""\""The application LTV is above the maximum allowed for the product.\""""; } }
                            public string CreditScoreBelowMinimum { get { return ""\""The best Credit Score applicable for this application is below the minimum requirement for the product.\""""; } }
                            public string PTIAboveMaximum { get { return ""\""Affordability: Loan repayment to income ratio (PTI) is above the maximum limit. Reducing the loan amount may make the loan more affordable.\""""; } }
                            public string PropertyValueBelowMinimum { get { return ""\""The property value is below the minimum for the product.\""""; } }
                        }

                        public Salaried salaried = new Salaried();
                        public SalariedwithDeduction salariedwithDeduction = new SalariedwithDeduction();
                        public SelfEmployed selfEmployed = new SelfEmployed();
                        public Alpha alpha = new Alpha();

                        public string ApplicantMinimumEmpirica { get { return ""\""The Empirica score is below required minimum.\""""; } }
                        public string ApplicantMaximumJudgementsinLast3Years { get { return ""\""There is record of multiple recent unpaid judgements in the last 3 years.\""""; } }
                        public string MaximumAggregateJudgementValuewith3JudgementsinLast3Years { get { return ""\""There is record of unpaid judgements with a material aggregated rand value.\""""; } }
                        public string MaximumAggregatedJudgementValueUnsettledForBetween13And36Months { get { return ""\""There is record of an outstanding aggregated unpaid judgement of material value.\""""; } }
                        public string MaximumNumberOfUnsettledDefaultsWithinPast2Years { get { return ""\""There is record of numerous unsettled defaults within the past 2 years.\""""; } }
                        public string NoticeOfSequestration { get { return ""\""There is a record of Sequestration.\""""; } }
                        public string NoticeOfAdministrationOrder { get { return ""\""There is a record of an Administration Order.\""""; } }
                        public string NoticeOfDebtCounselling { get { return ""\""There is a record of Debt Counselling.\""""; } }
                        public string NoticeOfDebtReview { get { return ""\""There is a record of Debt Review.\""""; } }
                        public string NoticeOfConsumerIsDeceased { get { return ""\""There is record that the consumer is deceased.\""""; } }
                        public string NoticeOfCreditCardRevoked { get { return ""\""There is record of a revoked credit card.\""""; } }
                        public string NoticeOfAbsconded { get { return ""\""There is record that the applicant has absconded.\""""; } }
                        public string NoticeOfPaidOutOnDeceasedClaim { get { return ""\""There is record that a deceased claim has been paid out.\""""; } }
                        public string NoCreditBureauMatchFound { get { return ""\""No credit bureau match found.\""""; } }
                        public string LoantoValueAboveCreditMaximum { get { return ""\""Insufficient property value for loan amount requested.\""""; } }
                        public string InvestmentPropertyLoanToValueAboveMaximum { get { return ""\""Your loan requested amount to the estimated property value ratio of an investment property application would be #{Variables::outputs.LoantoValueasPercent}, which exceeds the maximum of 80% or less.\""""; } }
                        public string NewPurchaseLTVaboveMaximum { get { return ""\""Your loan amount to purchase price (LTV) would be #{Variables::outputs.LoantoValueasPercent}, which is greater than the maximum of #{maximumloantovalue}%. In order to reduce your LTV, the loan amount would need to be lowered by increasing the deposit by R#{requiredamounttolowerloanamountby} or more.\""""; } }
                        public string SwitchLTVaboveMaximum { get { return ""\""Your loan amount to estimated property value (LTV) would be #{Variables::outputs.LoantoValueasPercent}%, which is greater than the maximum of #{maximumloantovalue}%. In order to reduce your LTV, the loan amount would need to be lowered by decreasing the cash required by R#{requiredamounttolowerloanamountby} or more.\""""; } }
                        public string PTIaboveMaximum { get { return ""\""Your repayment to household income (PTI) would be #{Variables::outputs.PaymenttoIncomeasPercent} and is greater than or equal to the maximum of #{maximumpti}%, which is the maximum per responsible affordability guidelines. In order to reduce your PTI, the loan amount would need to reduce to no more than R#{maximumloanamount} or alternatively additional income so that total income is at least R#{requiredhouseholdincome}.\""""; } }
                        public string HouseholdIncomeTypeIsUnemployed { get { return ""\""Your household income type may not be Unemployed.\""""; } }
                    }

                    public Credit credit = new Credit();

                    public string Insufficientinformation { get { return ""\""The correct information is not available to continue.\""""; } }
                }

                public Capitec capitec = new Capitec();
            }
        }";
        #endregion Messages

        public static string TestEnumerationsClass =
        #region Enumerations
        @"namespace SAHL.DecisionTree.Shared.Globals
        {
            public class Enumerations_2
            {
                public class SAHomeLoans
                {
                    public class Credit
                    {
                        public class CreditMatrixCategory
                        {
                             public string SalariedCategory0 { get { return ""Globals.Enumerations.sAHomeLoans.credit.creditMatrixCategory.SalariedCategory0""; } }
                             public string SalariedCategory1 { get { return ""Globals.Enumerations.sAHomeLoans.credit.creditMatrixCategory.SalariedCategory1""; } }
                             public string SalariedCategory3 { get { return ""Globals.Enumerations.sAHomeLoans.credit.creditMatrixCategory.SalariedCategory3""; } }
                             public string SalariedCategory4 { get { return ""Globals.Enumerations.sAHomeLoans.credit.creditMatrixCategory.SalariedCategory4""; } }
                             public string SalariedCategory5 { get { return ""Globals.Enumerations.sAHomeLoans.credit.creditMatrixCategory.SalariedCategory5""; } }
                             public string SalariedwithDeductionCategory0 { get { return ""Globals.Enumerations.sAHomeLoans.credit.creditMatrixCategory.SalariedwithDeductionCategory0""; } }
                             public string SalariedwithDeductionCategory1 { get { return ""Globals.Enumerations.sAHomeLoans.credit.creditMatrixCategory.SalariedwithDeductionCategory1""; } }
                             public string SalariedwithDeductionCategory3 { get { return ""Globals.Enumerations.sAHomeLoans.credit.creditMatrixCategory.SalariedwithDeductionCategory3""; } }
                             public string SalariedwithDeductionCategory4 { get { return ""Globals.Enumerations.sAHomeLoans.credit.creditMatrixCategory.SalariedwithDeductionCategory4""; } }
                             public string SalariedwithDeductionCategory5 { get { return ""Globals.Enumerations.sAHomeLoans.credit.creditMatrixCategory.SalariedwithDeductionCategory5""; } }
                             public string SalariedwithDeductionCategory10 { get { return ""Globals.Enumerations.sAHomeLoans.credit.creditMatrixCategory.SalariedwithDeductionCategory10""; } }
                             public string SelfEmployedCategory0 { get { return ""Globals.Enumerations.sAHomeLoans.credit.creditMatrixCategory.SelfEmployedCategory0""; } }
                             public string SelfEmployedCategory1 { get { return ""Globals.Enumerations.sAHomeLoans.credit.creditMatrixCategory.SelfEmployedCategory1""; } }
                             public string SelfEmployedCategory3 { get { return ""Globals.Enumerations.sAHomeLoans.credit.creditMatrixCategory.SelfEmployedCategory3""; } }
                             public string AlphaCategory6 { get { return ""Globals.Enumerations.sAHomeLoans.credit.creditMatrixCategory.AlphaCategory6""; } }
                             public string AlphaCategory7 { get { return ""Globals.Enumerations.sAHomeLoans.credit.creditMatrixCategory.AlphaCategory7""; } }
                             public string AlphaCategory8 { get { return ""Globals.Enumerations.sAHomeLoans.credit.creditMatrixCategory.AlphaCategory8""; } }
                             public string AlphaCategory9 { get { return ""Globals.Enumerations.sAHomeLoans.credit.creditMatrixCategory.AlphaCategory9""; } }
                        }
                        public CreditMatrixCategory creditMatrixCategory = new CreditMatrixCategory();
                    }

                    public Credit credit = new Credit();

                    public class PropertyOccupancyType
                    {
                         public string OwnerOccupied { get { return ""Globals.Enumerations.sAHomeLoans.propertyOccupancyType.OwnerOccupied""; } }
                         public string InvestmentProperty { get { return ""Globals.Enumerations.sAHomeLoans.propertyOccupancyType.InvestmentProperty""; } }
                    }
                    public class HouseholdIncomeType
                    {
                         public string Salaried { get { return ""Globals.Enumerations.sAHomeLoans.householdIncomeType.Salaried""; } }
                         public string SelfEmployed { get { return ""Globals.Enumerations.sAHomeLoans.householdIncomeType.SelfEmployed""; } }
                         public string SalariedwithDeduction { get { return ""Globals.Enumerations.sAHomeLoans.householdIncomeType.SalariedwithDeduction""; } }
                         public string Unemployed { get { return ""Globals.Enumerations.sAHomeLoans.householdIncomeType.Unemployed""; } }
                    }
                    public class MortgageLoanApplicationType
                    {
                         public string Switch { get { return ""Globals.Enumerations.sAHomeLoans.mortgageLoanApplicationType.Switch""; } }
                         public string NewPurchase { get { return ""Globals.Enumerations.sAHomeLoans.mortgageLoanApplicationType.NewPurchase""; } }
                    }
                    public class DefaultEnumValue
                    {
                         public string Unknown { get { return ""Globals.Enumerations.sAHomeLoans.defaultEnumValue.Unknown""; } }
                    }
                    public PropertyOccupancyType propertyOccupancyType = new PropertyOccupancyType();
                    public HouseholdIncomeType householdIncomeType = new HouseholdIncomeType();
                    public MortgageLoanApplicationType mortgageLoanApplicationType = new MortgageLoanApplicationType();
                    public DefaultEnumValue defaultEnumValue = new DefaultEnumValue();
                }

                public SAHomeLoans sAHomeLoans = new SAHomeLoans();
            }
        }";
        #endregion Enumerations

        public static string TestVariablesClass =
        #region Variables
        @"namespace SAHL.DecisionTree.Shared.Globals
        {
            public class Variables_2
            {
                static Enumerations_2 Enumerations;
                static Messages_2 Messages;

                public Variables_2(Enumerations_2 enumerations,Messages_2 messages)
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
                                public int MinimumApplicationEmpirica { get { return 575; } }
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
                                public int MinimumApplicationEmpirica { get { return 600; } }
                                public double CategoryLinkRate { get { return 0.037d; } }
                                public double MinimumHouseholdIncome { get { return 18600.00d; } }
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
                                public int MinimumApplicationEmpirica { get { return 610; } }
                                public double CategoryLinkRate { get { return 0.039d; } }
                                public double MinimumHouseholdIncome { get { return 18600.00d; } }
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
                                public double MinimumHouseholdIncome { get { return 18600d; } }
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
                                public double MinimumHouseholdIncome { get { return 18600.00d; } }
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
                                public double MinimumHouseholdIncome { get { return 18600.00d; } }
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
                                public double MinimumHouseholdIncome { get { return 18600.00d; } }
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

                                public double CategoryLinkRate { get { return 0.042d; } }
                                public double MaximumLoanToValue { get { return 1.00d; } }
                                public double MinimumHouseholdIncome { get { return 18600.00d; } }
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

                                public double MaximumLoanToValue { get { return 0.80d; } }
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
                                public double MinimumHouseholdIncome { get { return 18600.00d; } }
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
                                public double CategoryLinkRate { get { return 0.044d; } }
                            }
                            public class Category8
                            {
                                public double MaximumLoanToValue { get { return 0.96d; } }
                                public double MaximumPaymentToIncome { get { return 0.30d; } }
                                public int MinimumApplicationEmpirica { get { return 610; } }
                                public double CategoryLinkRate { get { return 0.048d; } }
                            }
                            public class Category9
                            {
                                public double MaximumLoanToValue { get { return 1.00d; } }
                                public double MaximumPaymentToIncome { get { return 0.30d; } }
                                public int MinimumApplicationEmpirica { get { return 610; } }
                                public double CategoryLinkRate { get { return 0.051d; } }
                                public int MinimumHouseholdIncome { get { return 10000; } }
                            }

                            public Category6 category6 = new Category6();
                            public Category7 category7 = new Category7();
                            public Category8 category8 = new Category8();
                            public Category9 category9 = new Category9();

                            public int MinimumLoanAmount { get { return 100000; } }
                            public int MinimumHouseholdIncome { get { return 8000; } }
                            public int MaximumHouseholdIncome { get { return 18599; } }
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
                }
                public class SAHomeLoans
                {
                    public class NewBusiness
                    {
                        public class Credit
                        {
                        }

                        public Credit credit = new Credit();
                    }
                    public class Rates
                    {
                        public double JIBAR3MonthRounded { get { return 0.0580d; } }
                    }

                    public NewBusiness newBusiness = new NewBusiness();
                    public Rates rates = new Rates();
                }

                public Capitec capitec = new Capitec();
                public SAHomeLoans sAHomeLoans = new SAHomeLoans();
            }
        }";
        #endregion Variables

        public static string TestTreeJSON =
        #region Tree
        @"
        {
            ""jsonversion"" : ""0.4"",
            ""version"" : 1,
            ""name"" : ""EdTest"",
            ""description"" : ""New Decision Tree"",
            ""tree"" : {
                ""variables"" : [{
                        ""id"" : ""1"",
                        ""type"" : ""int"",
                        ""usage"" : ""input"",
                        ""name"" : ""Input""
                    }, {
                        ""id"" : ""6"",
                        ""type"" : ""bool"",
                        ""usage"" : ""output"",
                        ""name"" : ""OtherTruth"",
                        ""defaultValue"" : false
                    }, {
                        ""id"" : ""8"",
                        ""type"" : ""string"",
                        ""usage"" : ""output"",
                        ""name"" : ""Output1"",
                        ""defaultValue"" : """"
                    }, {
                        ""id"" : ""10"",
                        ""type"" : ""SAHomeLoans::PropertyOccupancyType"",
                        ""usage"" : ""output"",
                        ""name"" : ""newvar2"",
                        ""defaultValue"" : {
                            ""group"" : ""Enumerations::sAHomeLoans::defaultEnumValue"",
                            ""name"" : ""Unknown"",
                            ""value"" : ""Enumerations::sAHomeLoans::defaultEnumValue.Unknown""
                        }
                    }, {
                        ""id"" : ""12"",
                        ""type"" : ""string"",
                        ""usage"" : ""output"",
                        ""name"" : ""z"",
                        ""defaultValue"" : """"
                    }
                ],
                ""nodes"" : [{
                        ""id"" : ""1"",
                        ""name"" : ""Start"",
                        ""type"" : ""Start"",
                        ""code"" : """"
                    }, {
                        ""id"" : -1,
                        ""name"" : ""Yes Branch Process"",
                        ""type"" : ""Process"",
                        ""code"" : ""Variables::outputs.Output1 = _quote_Hello From Sub Tree_quote__newline_Messages.AddInfo(_sgl_quote_hello_sgl_quote_)_newline_Variables::outputs.z = 1_newline_""
                    }, {
                        ""id"" : -3,
                        ""name"" : ""End"",
                        ""type"" : ""End"",
                        ""code"" : """"
                    }, {
                        ""id"" : -2,
                        ""name"" : ""When doing Something"",
                        ""type"" : ""Decision"",
                        ""code"" : ""Variables::outputs.OtherTruth = true_newline__newline_if 4000 > 9000 then_newline__tab_Variables::outputs.NodeResult = false_newline_else_newline__tab_Variables::outputs.NodeResult = true_newline_end_newline_  _newline_ ""
                    }
                ],
                ""links"" : [{
                        ""id"" : 0,
                        ""type"" : ""link"",
                        ""fromNodeId"" : ""1"",
                        ""toNodeId"" : -2
                    }, {
                        ""id"" : 1,
                        ""type"" : ""link"",
                        ""fromNodeId"" : -1,
                        ""toNodeId"" : -3
                    }, {
                        ""id"" : 2,
                        ""type"" : ""decision_yes"",
                        ""fromNodeId"" : -2,
                        ""toNodeId"" : -1
                    }, {
                        ""id"" : 3,
                        ""type"" : ""decision_no"",
                        ""fromNodeId"" : -2,
                        ""toNodeId"" : -1
                    }
                ]
            },
            ""layout"" : {
                ""nodes"" : [{
                        ""id"" : ""1"",
                        ""category"" : ""Start"",
                        ""loc"" : ""203.11764853953196 19.303297705144423"",
                        ""text"" : ""Start""
                    }, {
                        ""id"" : -1,
                        ""category"" : ""Process"",
                        ""loc"" : ""377.64705438140413 235.04010688456668"",
                        ""text"" : ""Yes Branch Process""
                    }, {
                        ""id"" : -3,
                        ""category"" : ""End"",
                        ""loc"" : ""423.48247968024987 317"",
                        ""text"" : ""End""
                    }, {
                        ""id"" : -2,
                        ""category"" : ""Decision"",
                        ""loc"" : ""375.64705438140413 111.04010688456673"",
                        ""text"" : ""When doing Something""
                    }
                ],
                ""links"" : [{
                        ""linkid"" : 0,
                        ""from"" : ""1"",
                        ""to"" : -2,
                        ""fromPort"" : ""B"",
                        ""toPort"" : ""T"",
                        ""points"" : [203.1, 17.7, 203.1, 27.7, 203.1, 50.5, 375.6, 50.5, 375.6, 73.4, 375.6, 83.4]
                    }, {
                        ""linkid"" : 1,
                        ""from"" : -1,
                        ""to"" : -3,
                        ""fromPort"" : ""B"",
                        ""toPort"" : ""L"",
                        ""points"" : [377.6, 232.4, 377.6, 242.4, 377.6, 317, 385.3, 317, 393, 317, 403, 317]
                    }, {
                        ""linkid"" : 2,
                        ""from"" : -2,
                        ""to"" : -1,
                        ""fromPort"" : ""L"",
                        ""toPort"" : ""L"",
                        ""points"" : [355.1, 103.9, 345.1, 103.9, 348, 103.9, 348, 103.9, 300, 103.9, 300, 219.9, 347.1, 219.9, 357.1, 219.9]
                    }, {
                        ""linkid"" : 3,
                        ""from"" : -2,
                        ""to"" : -1,
                        ""fromPort"" : ""R"",
                        ""toPort"" : ""R"",
                        ""points"" : [396.1, 103.9, 406.1, 103.9, 404, 103.9, 404, 103.9, 452, 103.9, 452, 219.9, 408.1, 219.9, 398.1, 219.9]
                    }
                ]
            },
            ""testCases"" : [],
            ""testVariables"" : [{
                    ""name"" : ""OtherTruth"",
                    ""id"" : ""6"",
                    ""value"" : true,
                    ""type"" : ""bool"",
                    ""inputType"" : ""bool""
                }, {
                    ""name"" : ""Output1"",
                    ""id"" : ""8"",
                    ""value"" : ""Hello From Sub Tree"",
                    ""type"" : ""string"",
                    ""inputType"" : ""text""
                }
            ]
        }
        ";
        #endregion Tree

        public static string TestMainTreeCallingSubTreeJSON =
        #region SubTree
        @"
        {
            ""jsonversion"" : ""0.4"",
            ""version"" : 1,
            ""name"" : ""TestProcess"",
            ""description"" : ""New Decision Tree"",
            ""tree"" : {
                ""variables"" : [{
                        ""id"" : ""1"",
                        ""type"" : ""string"",
                        ""usage"" : ""input"",
                        ""name"" : ""newvarstr1""
                    }, {
                        ""id"" : ""7"",
                        ""type"" : ""string"",
                        ""usage"" : ""output"",
                        ""name"" : ""newvar5"",
                        ""defaultValue"" : """"
                    }
                ],
                ""nodes"" : [{
                        ""id"" : ""1"",
                        ""name"" : ""Start"",
                        ""type"" : ""Start"",
                        ""code"" : """"
                    }, {
                        ""id"" : -1,
                        ""name"" : ""Process"",
                        ""type"" : ""Process"",
                        ""code"" : """"
                    }, {
                        ""id"" : -3,
                        ""name"" : ""End"",
                        ""type"" : ""End"",
                        ""code"" : """"
                    }, {
                        ""id"" : -4,
                        ""name"" : ""Sub Tree"",
                        ""type"" : ""SubTree"",
                        ""code"" : """",
                        ""subtreeName"" : ""TestSubtree"",
                        ""subtreeVersion"" : 1,
                        ""subtreeVariables"" : [{
                                ""id"" : ""1"",
                                ""type"" : ""string"",
                                ""usage"" : ""input"",
                                ""name"" : ""newvar"",
                                ""parentVariable"" : {
                                    ""name"" : ""newvarstr1"",
                                    ""usage"" : ""input""
                                }
                            }, {
                                ""id"" : ""2"",
                                ""type"" : ""string"",
                                ""usage"" : ""output"",
                                ""name"" : ""newvar"",
                                ""defaultValue"" : """",
                                ""parentVariable"" : {
                                    ""name"" : ""newvar5"",
                                    ""usage"" : ""output""
                                }
                            }
                        ]
                    }
                ],
                ""links"" : [{
                        ""id"" : 0,
                        ""type"" : ""link"",
                        ""fromNodeId"" : -1,
                        ""toNodeId"" : -3
                    }, {
                        ""id"" : 1,
                        ""type"" : ""link"",
                        ""fromNodeId"" : ""1"",
                        ""toNodeId"" : -4
                    }, {
                        ""id"" : 2,
                        ""type"" : ""link"",
                        ""fromNodeId"" : -4,
                        ""toNodeId"" : -1
                    }
                ]
            },
            ""layout"" : {
                ""nodes"" : [{
                        ""id"" : ""1"",
                        ""category"" : ""Start"",
                        ""loc"" : ""201 31.949999999999996"",
                        ""text"" : ""Start""
                    }, {
                        ""id"" : -1,
                        ""category"" : ""Process"",
                        ""loc"" : ""147 175.1532734578829"",
                        ""text"" : ""Process""
                    }, {
                        ""id"" : -3,
                        ""category"" : ""End"",
                        ""loc"" : ""286 285.2032734578829"",
                        ""text"" : ""End""
                    }, {
                        ""id"" : -4,
                        ""category"" : ""SubTree"",
                        ""loc"" : ""391 74.20388971177968"",
                        ""text"" : ""Sub Tree""
                    }
                ],
                ""links"" : [{
                        ""linkid"" : 0,
                        ""from"" : -1,
                        ""to"" : -3,
                        ""fromPort"" : ""B"",
                        ""toPort"" : ""T"",
                        ""points"" : [147, 172.5, 147, 182.5, 147, 224.1, 286, 224.1, 286, 265.7, 286, 275.7]
                    }, {
                        ""linkid"" : 1,
                        ""from"" : ""1"",
                        ""to"" : -4,
                        ""fromPort"" : ""B"",
                        ""toPort"" : ""T"",
                        ""points"" : [201, 30.3, 201, 40.3, 201, 52, 201, 52, 201, 68, 228, 68, 228, 36, 391, 36, 391, 37.6, 391, 47.6]
                    }, {
                        ""linkid"" : 2,
                        ""from"" : -4,
                        ""to"" : -1,
                        ""fromPort"" : ""B"",
                        ""toPort"" : ""T""
                    }
                ]
            },
            ""testCases"" : [],
            ""testVariables"" : []
        }

        ";
        #endregion SubTree

        public static string TestSubTreeJSON =
        #region SubTree
        @"
        {
            ""jsonversion"" : ""0.4"",
            ""version"" : 1,
            ""name"" : ""TestSubtree"",
            ""description"" : ""New Decision Tree"",
            ""tree"" : {
                ""variables"" : [{
                        ""id"" : ""1"",
                        ""type"" : ""string"",
                        ""usage"" : ""input"",
                        ""name"" : ""newvar""
                    }, {
                        ""id"" : ""2"",
                        ""type"" : ""string"",
                        ""usage"" : ""output"",
                        ""name"" : ""newvar"",
                        ""defaultValue"" : """"
                    }
                ],
                ""nodes"" : [{
                        ""id"" : ""1"",
                        ""name"" : ""Start"",
                        ""type"" : ""Start"",
                        ""code"" : """"
                    }, {
                        ""id"" : -1,
                        ""name"" : ""Process"",
                        ""type"" : ""Process"",
                        ""code"" : ""Variables::outputs.newvar = Variables::inputs.newvar""
                    }, {
                        ""id"" : -3,
                        ""name"" : ""End"",
                        ""type"" : ""End"",
                        ""code"" : """"
                    }
                ],
                ""links"" : [{
                        ""id"" : 0,
                        ""type"" : ""link"",
                        ""fromNodeId"" : ""1"",
                        ""toNodeId"" : -1
                    }, {
                        ""id"" : 1,
                        ""type"" : ""link"",
                        ""fromNodeId"" : -1,
                        ""toNodeId"" : -3
                    }
                ]
            },
            ""layout"" : {
                ""nodes"" : [{
                        ""id"" : ""1"",
                        ""category"" : ""Start"",
                        ""loc"" : ""158 35.65"",
                        ""text"" : ""Start""
                    }, {
                        ""id"" : -1,
                        ""category"" : ""Process"",
                        ""loc"" : ""179 136.2032734578829"",
                        ""text"" : ""Process""
                    }, {
                        ""id"" : -3,
                        ""category"" : ""End"",
                        ""loc"" : ""178 213.8532734578829"",
                        ""text"" : ""End""
                    }
                ],
                ""links"" : [{
                        ""linkid"" : 0,
                        ""from"" : ""1"",
                        ""to"" : -1,
                        ""fromPort"" : ""B"",
                        ""toPort"" : ""T"",
                        ""points"" : [158, 34, 158, 44, 158, 52, 158, 52, 158, 68, 179, 68, 179, 98.6, 179, 108.6]
                    }, {
                        ""linkid"" : 1,
                        ""from"" : -1,
                        ""to"" : -3,
                        ""fromPort"" : ""B"",
                        ""toPort"" : ""T"",
                        ""points"" : [179, 133.6, 179, 143.6, 178, 143.6, 178, 143.6, 178, 194.4, 178, 204.4]
                    }
                ]
            },
            ""testCases"" : [],
            ""testVariables"" : []
        }

        ";
        #endregion SubTree
    }
}

        
