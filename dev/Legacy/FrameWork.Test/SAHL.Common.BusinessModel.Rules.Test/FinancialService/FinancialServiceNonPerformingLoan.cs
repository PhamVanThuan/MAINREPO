using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Test;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using Rhino.Mocks;
using SAHL.Common.Globals;
using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.Rules.FinancialService;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Collections;

namespace SAHL.Common.BusinessModel.Rules.Test.FinancialService
{
    [TestFixture]
    public class FinancialServiceNonPerformingLoanTestRule : RuleBase
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


        #region FinancialServiceNonPerformingLoanTest
        /// <summary>
        /// If the current offer linked to this account has an offertypekey of 2 (Re-Advance) or 3 (Further Loan) and these offers do not have an NTU Offer 
        /// (stagedefinitionstagedefinitionGroupCompositeKey=110) NTU or Decline it means that the further loan / re-advance is in progress 
        /// hence do not allow the consultant to Proceed; render error message instead.  
        /// </summary>
        [NUnit.Framework.Test]
        public void FinancialServiceNonPerformingLoanTest()
        {
			// FAIL - HasFurtherLendingInProgress generic method returns true
            FinancialServiceNonPerformingLoanHelper(1, true);
			// PASS - HasFurtherLendingInProgress generic method returns false 
            FinancialServiceNonPerformingLoanHelper(0, false);
        }

        /// <summary>
        /// Helper method to set up the expectations for the FinancialServiceNonPerformingLoan test.
        /// </summary>
        /// <param name="gs"></param>
		private void FinancialServiceNonPerformingLoanHelper(int expectedMessageCount, bool hasFurtherLendingInProgress)
        {
            using (new SessionScope())
            {
                FinancialServiceNonPerformingLoan rule = new FinancialServiceNonPerformingLoan();
                SetRepositoryStrategy(TypeFactoryStrategy.Mock);

				IFinancialService fs = _mockery.StrictMock<IFinancialService>();
                IFinancialServiceType fst = _mockery.StrictMock<IFinancialServiceType>();
                SetupResult.For(fst.Key).Return((int)FinancialServiceTypes.VariableLoan);
                SetupResult.For(fs.FinancialServiceType).Return(fst);

				IAccount acc = _mockery.StrictMock<IAccount>();
				SetupResult.For(acc.Key).Return(1);
				SetupResult.For(fs.Account).Return(acc);

                IApplicationRepository AR = _mockery.StrictMock<IApplicationRepository>();
                MockCache.Add(typeof(IApplicationRepository).ToString(), AR);

				SetupResult.For(AR.HasFurtherLendingInProgress(1)).IgnoreArguments().Return(hasFurtherLendingInProgress);

                ExecuteRule(rule, expectedMessageCount, fs);
            }
        }


        #endregion


    }
}
