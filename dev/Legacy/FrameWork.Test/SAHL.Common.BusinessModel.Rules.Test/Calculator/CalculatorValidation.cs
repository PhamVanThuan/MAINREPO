using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using SAHL.Common.BusinessModel.Rules.Calculator;
using SAHL.Test;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using Rhino.Mocks;
using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Globals;
using SAHL.Common.Factories;

namespace SAHL.Common.BusinessModel.Rules.Test.Calculator
{
    [TestFixture]
    public class CalculatorValidation : RuleBase
    {
        [SetUp]
        public override void Setup()
        {
            base.Setup();
        }

        [TearDown]
        public override void TearDown()
        {
            base.TearDown();
        }

        #region CalculatorValidations

        /// <summary>
        /// BondRegistrationNumber exists against this bond record, so it is unique
        /// </summary>
        [NUnit.Framework.Test]
        public void TestAll()
        {
            double amount1 = 100D;
            double amount2 = 150D;
            
            ExceedFurtherLendingLimit rule = new ExceedFurtherLendingLimit();
            ExecuteRule(rule, 1, amount1);

            ExceedFurtherLendingLimitLAA rule1 = new ExceedFurtherLendingLimitLAA();
            ExecuteRule(rule1, 1, amount1);

            ExceedReadvanceLimit rule2 = new ExceedReadvanceLimit();
            ExecuteRule(rule2, 1, amount1);

            ExceedFurtherAdvanceLimit rule3 = new ExceedFurtherAdvanceLimit();
            ExecuteRule(rule3, 1, amount1);

            ExceedFurtherLoanLimit rule4 = new ExceedFurtherLoanLimit();
            ExecuteRule(rule4, 1, amount1);

            NoBondRequired rule5 = new NoBondRequired();
            ExecuteRule(rule5, 1, null);

            BondLessThanFurtherLoan rule6 = new BondLessThanFurtherLoan();
            ExecuteRule(rule6, 1, null);

            VarifixMinimumFixAmount rule7 = new VarifixMinimumFixAmount();
            ExecuteRule(rule7, 1, amount1, amount2);

            VarifixMinimumLoanAmount rule8 = new VarifixMinimumLoanAmount();
            ExecuteRule(rule8, 1, amount1);

            RefinanceMinimumCashOut rule9 = new RefinanceMinimumCashOut();
            ExecuteRule(rule9, 1, amount1);

            SwitchCurrentLoanAmountMinimum rule10 = new SwitchCurrentLoanAmountMinimum();
            ExecuteRule(rule10, 1, null);






        }

        #endregion

        #region TitleDeedsOnFile
        [NUnit.Framework.Test]
        public void TitleDeedsOnFileTest()
        {
            
            TitleDeedsOnFileTestHelper(0, true);
            TitleDeedsOnFileTestHelper(1, false);
        }
        private void TitleDeedsOnFileTestHelper(int expectedMessageCount, bool boolTitleDeeds)
        {

                TitleDeedsOnFile rule = new TitleDeedsOnFile();
                
                IAccountRepository accRepo = _mockery.StrictMock<IAccountRepository>();
                MockCache.Add(typeof(IAccountRepository).ToString(), accRepo);
                SetRepositoryStrategy(TypeFactoryStrategy.Mock);

                SetupResult.For(accRepo.TitleDeedsOnFile(1)).IgnoreArguments().Return(boolTitleDeeds);
                
                ExecuteRule(rule, expectedMessageCount, 1);
        }

        #endregion

        #region CheckMaxLTP
        [NUnit.Framework.Test]
        public void CheckMaxLTPTest()
        {

            CheckMaxLTPTestHelper(0, 0.7, DateTime.Today);  // PASS
            CheckMaxLTPTestHelper(1, 0.86, DateTime.Today); // FAIL
            CheckMaxLTPTestHelper(0, 0.86, DateTime.Today.AddMonths(-13)); // PASS
        }
        private void CheckMaxLTPTestHelper(int expectedMessageCount, double ltp, DateTime openDate)
        {

            CheckMaxLTP rule = new CheckMaxLTP();


            ExecuteRule(rule, expectedMessageCount, ltp, openDate);
        }


        #endregion

        #region CalcCreditDisqualificationEmploymentType
        [Test, Sequential]
        public void CalcCreditDisqualificationEmploymentTypePass([Values(EmploymentTypes.Salaried, EmploymentTypes.SelfEmployed,
                                                                    EmploymentTypes.SalariedwithDeduction)]  EmploymentTypes employmentType)
        {
            var rule = new CalcCreditDisqualificationEmploymentType();
            ExecuteRule(rule, 0, (int)employmentType);
        }
        [Test, Sequential]
        public void CalcCreditDisqualificationEmploymentTypeFail([Values(EmploymentTypes.Unemployed, EmploymentTypes.Unknown)]  EmploymentTypes employmentType)
        {
            var rule = new CalcCreditDisqualificationEmploymentType();
            ExecuteRule(rule, 1, (int)employmentType);
        }
        #endregion

        #region CalcCreditDisqualificationMaxLAA
        /// <summary>
        /// If the LAA value provided is less than the R 6,000,000.00  stipulated in the control table then the rule should pass.
        /// </summary>
        /// <param name="LAA"></param>
        [Test, Sequential]
        public void CalcCreditDisqualificationMaxLAAPass([Values(5999999.99, 6000000.00)] double LAA)
        {
            CalcCreditDisqualificationMaxLAA rule = new CalcCreditDisqualificationMaxLAA();
            maxLAA = MaximumLAA;
            IControl ctrl = _mockery.StrictMock<IControl>();
            IControlRepository repo = _mockery.StrictMock<IControlRepository>();
            string controlDescription = "Calc - maxLAA";
            //
            SetRepositoryStrategy(SAHL.Test.TypeFactoryStrategy.Mock);
            MockCache.Add(typeof(IControlRepository).ToString(), repo);
            //
            SetupResult.For(ctrl.ControlDescription).Return(controlDescription);
            SetupResult.For(ctrl.ControlNumeric).Return(maxLAA);
            SetupResult.For(ctrl.ControlText).Return("Loan amount is too high.");
            SetupResult.For(repo.GetControlByDescription(controlDescription)).IgnoreArguments().Return(ctrl);
            //run the test
            ExecuteRule(rule, 0, LAA);
        }
        /// <summary>
        /// If the LAA value provided is greater than the R 6,000,000.00 stipulated in the control table then the rule should pass.
        /// </summary>
        /// <param name="LAA"></param>
        [Test]
        public void CalcCreditDisqualificationMaxLAAFail([Values(6000000.01)] double LAA)
        {
            CalcCreditDisqualificationMaxLAA rule = new CalcCreditDisqualificationMaxLAA();
            maxLAA = MaximumLAA;
            IControl ctrl = _mockery.StrictMock<IControl>();
            IControlRepository repo = _mockery.StrictMock<IControlRepository>();
            string controlDescription = "Calc - maxLAA";
            //
            SetRepositoryStrategy(SAHL.Test.TypeFactoryStrategy.Mock);
            MockCache.Add(typeof(IControlRepository).ToString(), repo);
            //
            SetupResult.For(ctrl.ControlDescription).Return(controlDescription);
            SetupResult.For(ctrl.ControlNumeric).Return(maxLAA);
            SetupResult.For(ctrl.ControlText).Return("Loan amount is too high.");
            SetupResult.For(repo.GetControlByDescription(controlDescription)).IgnoreArguments().Return(ctrl);
            //run the test
            ExecuteRule(rule, 1, LAA);
        }
        #endregion

        #region CalcCreditDisqualificationMaxLTV

        [Test, Sequential]
        public void CalcCreditDisqualificationMaxLTVPass([Values(0.99, 0.989)] double LTV)
        {
            CalcCreditDisqualificationMaxLTV rule = new CalcCreditDisqualificationMaxLTV();
            maxLTV = MaximumLTV;
            IControl ctrl = _mockery.StrictMock<IControl>();
            IControlRepository repo = _mockery.StrictMock<IControlRepository>();
            string controlDescription = "Calc - maxLTV";
            //
            SetRepositoryStrategy(SAHL.Test.TypeFactoryStrategy.Mock);
            MockCache.Add(typeof(IControlRepository).ToString(), repo);
            //
            SetupResult.For(ctrl.ControlDescription).Return(controlDescription);
            SetupResult.For(ctrl.ControlNumeric).Return(maxLTV);
            SetupResult.For(ctrl.ControlText).Return("LTV is too high.");
            SetupResult.For(repo.GetControlByDescription(controlDescription)).IgnoreArguments().Return(ctrl);
            //run the test
            ExecuteRule(rule, 0, LTV);
        }

        [Test]
        public void CalcCreditDisqualificationMaxLTVFail()
        {
            CalcCreditDisqualificationMaxLTV rule = new CalcCreditDisqualificationMaxLTV();
            maxLTV = MaximumLTV;
            IControl ctrl = _mockery.StrictMock<IControl>();
            IControlRepository repo = _mockery.StrictMock<IControlRepository>();
            string controlDescription = "Calc - maxLTV";
            //
            SetRepositoryStrategy(SAHL.Test.TypeFactoryStrategy.Mock);
            MockCache.Add(typeof(IControlRepository).ToString(), repo);
            //
            SetupResult.For(ctrl.ControlDescription).Return(controlDescription);
            SetupResult.For(ctrl.ControlNumeric).Return(maxLTV);
            SetupResult.For(ctrl.ControlText).Return("LTV is too high.");
            SetupResult.For(repo.GetControlByDescription(controlDescription)).IgnoreArguments().Return(ctrl);
            //run the test
            ExecuteRule(rule, 1, 1.0001);
        }
        #endregion

        #region CalcCreditDisqualificationMaxPTI
        [Test, Sequential]
        public void CalcCreditDisqualificationMaxPTIPass([Values(0.3499, 0.35)] double PTI)
        {
            CalcCreditDisqualificationMaxPTI rule = new CalcCreditDisqualificationMaxPTI();
            maxPTI = MaximumPTI;
            IControl ctrl = _mockery.StrictMock<IControl>();
            IControlRepository repo = _mockery.StrictMock<IControlRepository>();
            string controlDescription = "Calc - maxPTI";
            //
            SetRepositoryStrategy(SAHL.Test.TypeFactoryStrategy.Mock);
            MockCache.Add(typeof(IControlRepository).ToString(), repo);
            //
            SetupResult.For(ctrl.ControlDescription).Return(controlDescription);
            SetupResult.For(ctrl.ControlNumeric).Return(maxPTI);
            SetupResult.For(ctrl.ControlText).Return("PTI is too high.");
            SetupResult.For(repo.GetControlByDescription(controlDescription)).IgnoreArguments().Return(ctrl);
            //run the test
            ExecuteRule(rule, 0, PTI);          
        }
        [Test]
        public void CalcCreditDisqualificationMaxPTIFail([Values(0.3501)] double PTI)
        {
            CalcCreditDisqualificationMaxPTI rule = new CalcCreditDisqualificationMaxPTI();
            maxPTI = MaximumPTI;
            IControl ctrl = _mockery.StrictMock<IControl>();
            IControlRepository repo = _mockery.StrictMock<IControlRepository>();
            string controlDescription = "Calc - maxPTI";
            //
            SetRepositoryStrategy(SAHL.Test.TypeFactoryStrategy.Mock);
            MockCache.Add(typeof(IControlRepository).ToString(), repo);
            //
            SetupResult.For(ctrl.ControlDescription).Return(controlDescription);
            SetupResult.For(ctrl.ControlNumeric).Return(maxPTI);
            SetupResult.For(ctrl.ControlText).Return("PTI is too high.");
            SetupResult.For(repo.GetControlByDescription(controlDescription)).IgnoreArguments().Return(ctrl);
            //run the test
            ExecuteRule(rule, 1, PTI);  
        }
        #endregion

        #region CalcCreditDisqualificationMinIncome
        [Test]
        public void CalcCreditDisqualificationMinIncomeFail()
        {
			ICreditMatrixRepository creditMatrixRepository = RepositoryFactory.GetRepository<ICreditMatrixRepository>();
			var rule = new CalcCreditDisqualificationMinIncome(creditMatrixRepository);
            minIncome = MinimumIncome;
            ExecuteRule(rule, 1, 4999.99); 
        }

        [Test, Sequential]
        public void CalcCreditDisqualificationMinIncomeFail([Values(8000.00, 8000.01)] double Income)
        {
			ICreditMatrixRepository creditMatrixRepository = RepositoryFactory.GetRepository<ICreditMatrixRepository>();
			var rule = new CalcCreditDisqualificationMinIncome(creditMatrixRepository);
            minIncome = MinimumIncome;
            //run the test
            ExecuteRule(rule, 0, Income); 
        }
        #endregion

        #region CalcCreditDisqualificationMinLAFL
        [Test]
        public void CalcCreditDisqualificationMinLAFLPass([Values(0.01, 0.011)] double amount)
        {
            var rule = new CalcCreditDisqualificationMinLAFL();
            minLAAFL = MinimumLAAFL;
            IControl ctrl = _mockery.StrictMock<IControl>();
            IControlRepository repo = _mockery.StrictMock<IControlRepository>();
            string controlDescription = "Calc - minFurtherLendingLAA";
            //
            SetRepositoryStrategy(SAHL.Test.TypeFactoryStrategy.Mock);
            MockCache.Add(typeof(IControlRepository).ToString(), repo);
            //
            SetupResult.For(ctrl.ControlDescription).Return(controlDescription);
            SetupResult.For(ctrl.ControlNumeric).Return(minLAAFL);
            SetupResult.For(ctrl.ControlText).Return("Further Lending Amount is too low.");
            SetupResult.For(repo.GetControlByDescription(controlDescription)).IgnoreArguments().Return(ctrl);
            //run the test
            ExecuteRule(rule, 0, amount);
        }
        [Test]
        public void CalcCreditDisqualificationMinLAFLFail()
        {
            var rule = new CalcCreditDisqualificationMinLAFL();
            minLAAFL = MinimumLAAFL;
            IControl ctrl = _mockery.StrictMock<IControl>();
            IControlRepository repo = _mockery.StrictMock<IControlRepository>();
            string controlDescription = "Calc - minFurtherLendingLAA";
            //
            SetRepositoryStrategy(SAHL.Test.TypeFactoryStrategy.Mock);
            MockCache.Add(typeof(IControlRepository).ToString(), repo);
            //
            SetupResult.For(ctrl.ControlDescription).Return(controlDescription);
            SetupResult.For(ctrl.ControlNumeric).Return(minLAAFL);
            SetupResult.For(ctrl.ControlText).Return("Further Lending Amount is too low.");
            SetupResult.For(repo.GetControlByDescription(controlDescription)).IgnoreArguments().Return(ctrl);
            //run the test
            ExecuteRule(rule, 1, 0.009);
        }
        #endregion

        #region ControlTableValues
        private IControlRepository cR;
        private IControlRepository ctrlRepo
        {
            get
            {
                if (cR == null)
                    cR = RepositoryFactory.GetRepository<IControlRepository>();

                return cR;
            }
        }
        private double maxLTV = 0;
        /// <summary>
        /// Value of max LTV
        /// </summary>
        private double MaximumLTV
        {
            get
            {
                if (maxLTV == 0)
                    maxLTV = ctrlRepo.GetControlByDescription("Calc - maxLTV").ControlNumeric ?? 0;

                return maxLTV;
            }
        }
        private double maxLAA = 0;
        /// <summary>
        /// Value of max LAA
        /// </summary>
        private double MaximumLAA
        {
            get
            {
                if (maxLAA == 0)
                    maxLAA = ctrlRepo.GetControlByDescription("Calc - maxLAA").ControlNumeric ?? 0;

                return maxLAA;
            }
        }

        private double maxPTI = 0;
        /// <summary>
        /// Value of max PTI
        /// </summary>
        private double MaximumPTI
        {
            get
            {
                if (maxPTI == 0)
                    maxPTI = ctrlRepo.GetControlByDescription("Calc - maxPTI").ControlNumeric ?? 0;

                return maxPTI;
            }
        }

        private double minIncome = 0;
        /// <summary>
        /// Value of Minimum Income
        /// </summary>
        private double MinimumIncome
        {
            get
            {
                if (minIncome == 0)
                    minIncome = ctrlRepo.GetControlByDescription("Calc - minIncome").ControlNumeric ?? 0;

                return minIncome;
            }
        }

        private double minLAA = 0;
        /// <summary>
        /// Value of Minimum LAA
        /// </summary>
        private double MinimumLAA
        {
            get
            {
                if (minLAA == 0)
                    minLAA = ctrlRepo.GetControlByDescription("Calc - minLAA").ControlNumeric ?? 0;

                return minLAA;
            }
        }

        private double minValuation = 0;
        /// <summary>
        /// Minimum Valuation Amount
        /// </summary>
        private double MinimumValuation
        {
            get
            {
                if (minValuation == 0)
                    minValuation = ctrlRepo.GetControlByDescription("Calc - minValuation").ControlNumeric ?? 0;

                return minValuation;
            }
        }

        private double minLAAFL = 0;
        /// <summary>
        /// Minimum LAA for Further Lending
        /// </summary>
        private double MinimumLAAFL
        {
            get
            {
                if (minLAAFL == 0)
                    minLAAFL = ctrlRepo.GetControlByDescription("Calc - minFurtherLendingLAA").ControlNumeric ?? 0;

                return minLAAFL;
            }
        }
        
        #endregion
    }
}
