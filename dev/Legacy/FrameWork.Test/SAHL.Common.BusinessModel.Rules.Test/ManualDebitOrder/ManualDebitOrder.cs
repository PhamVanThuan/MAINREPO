using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using SAHL.Common.BusinessModel.Interfaces;
using Rhino.Mocks;
using SAHL.Common.BusinessModel.Rules.FinancialService;
using SAHL.Common.Globals;
using SAHL.Common.Service.Interfaces;
using SAHL.Common.Factories;
using System.Data;
using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.X2.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Rules.ManualDebitOrder;

namespace SAHL.Common.BusinessModel.Rules.Test.FinancialService
{
    [TestFixture]
    public class ManualDebitOrder : RuleBase
    {

		//private IManualDebitOrder debtOrder;

        [SetUp]
        public override void Setup()
        {
            base.Setup();
			//debtOrder = _mockery.StrictMock<IManualDebitOrder>();
        }

        [TearDown]
        public override void TearDown()
        {
            base.TearDown();
        }

        [Test]
        public void ManualDebitOrderStartDayMinimumTest()
        {
            ManualDebitOrderStartDayMinimum rule = new ManualDebitOrderStartDayMinimum();
			var debitOrder = _mockery.StrictMock<IManualDebitOrder>();

            // null start date - pass
			//We don't have Nullable Dates anymore
			//SetupResult.For(debitOrder.ActionDate).Return(new DateTime());
			//ExecuteRule(rule, 0, debitOrder);

            // start date before today - fail
			SetupResult.For(debitOrder.ActionDate).Return(DateTime.Now.AddDays(-1));
			ExecuteRule(rule, 1, debitOrder);

            // today's date - will pass before 14h00, will fail after 14h00
			SetupResult.For(debitOrder.ActionDate).Return(DateTime.Now);
			ExecuteRule(rule, (DateTime.Now.Hour >= 14 ? 1 : 0), debitOrder);

            // tomorrow - pass
			SetupResult.For(debitOrder.ActionDate).Return(DateTime.Now.AddDays(1));
			ExecuteRule(rule, 0, debitOrder);

        }

        [Test]
        public void ManualDebitOrderStartDayMaximumTest()
        {
            ManualDebitOrderStartDayMaximum rule = new ManualDebitOrderStartDayMaximum();
			var debitOrder = _mockery.StrictMock<IManualDebitOrder>();

            // null start date - pass
			// no more nullable dates
			//SetupResult.For(debitOrder.ActionDate).Return(new DateTime());
			//ExecuteRule(rule, 0, debitOrder);

            // start date (6 months less 1 day) - pass
			SetupResult.For(debitOrder.ActionDate).Return(DateTime.Now.AddMonths(6).AddDays(-1));
			ExecuteRule(rule, 0, debitOrder);

            // start date (6 months add 1 hour) - fail
			SetupResult.For(debitOrder.ActionDate).Return(DateTime.Now.AddMonths(6).AddHours(1));
			ExecuteRule(rule, 1, debitOrder);
        }

        [Test]
        public void ManualDebitOrderUpdateExpiredTest()
        {
            ManualDebitOrderUpdateExpired rule = new ManualDebitOrderUpdateExpired();
			var debitOrder = _mockery.StrictMock<IManualDebitOrder>();
            var original = _mockery.StrictMock<IManualDebitOrder>();

            // existing item with null start date - pass
			//No more nullable dates
			//SetupResult.For(debitOrder.Key).Return(1);
			//SetupResult.For(debitOrder.Original).Return(original);
			//SetupResult.For(original.ActionDate).Return(new DateTime());
			//ExecuteRule(rule, 0, debitOrder);

            // existing item with future start date - pass
			SetupResult.For(debitOrder.Key).Return(1);
			SetupResult.For(debitOrder.Original).Return(original);
			SetupResult.For(original.ActionDate).Return(DateTime.Now.AddDays(1));
			ExecuteRule(rule, 0, debitOrder);

            // existing item with historical start date - fail
			SetupResult.For(debitOrder.Key).Return(1);
			SetupResult.For(debitOrder.Original).Return(original);
			SetupResult.For(original.ActionDate).Return(DateTime.Now.AddDays(-1));
			ExecuteRule(rule, 1, debitOrder);

            // new item with historical start date - pass
			SetupResult.For(debitOrder.Key).Return(0);
			SetupResult.For(debitOrder.Original).Return(original);
			SetupResult.For(original.ActionDate).Return(DateTime.Now.AddDays(-1));
			ExecuteRule(rule, 0, debitOrder);

            // this check will differ depending on time - use today's date: if it's before 14h00 it will pass
            //  but if it's after 14h00 it will fail
			SetupResult.For(debitOrder.Key).Return(1);
			SetupResult.For(debitOrder.Original).Return(original);
			SetupResult.For(original.ActionDate).Return(DateTime.Today);
			ExecuteRule(rule, (DateTime.Now.Hour > 13 ? 1 : 0), debitOrder);
            

        }

        [Test]
        public void ManualDebitOrderDeleteCheckTest()
        {
            ManualDebitOrderDeleteCheck rule = new ManualDebitOrderDeleteCheck();
            ITransactionType transactionType = _mockery.StrictMock<ITransactionType>();

			var debitOrder = _mockery.StrictMock<IManualDebitOrder>();

            // null transaction type - pass
			SetupResult.For(debitOrder.TransactionType).Return(null);
			ExecuteRule(rule, 0, debitOrder);

            // loop through all transaction types - only ManualDebitOrder should pass
            Array transactionTypes = Enum.GetValues(typeof(TransactionTypes));
            for (int i = 0; i < transactionTypes.Length; i++)
            {
                int transTypeKey = (int)transactionTypes.GetValue(i);
				SetupResult.For(debitOrder.TransactionType).Return(transactionType);
                SetupResult.For(transactionType.Key).Return(Convert.ToInt16(transTypeKey));
				ExecuteRule(rule, (transTypeKey == (int)TransactionTypes.ManualDebitOrderPayment ? 0 : 1), debitOrder);
            }
        }

        [Test]
        public void ManualDebitOrderBankAccountMandatoryTest()
        {
			ManualDebitOrderBankAccountMandatory rule = new ManualDebitOrderBankAccountMandatory();
            ITransactionType transactionType = _mockery.StrictMock<ITransactionType>();
            IBankAccount bankAccount = _mockery.StrictMock<IBankAccount>();
			var debitOrder = _mockery.StrictMock<IManualDebitOrder>();

            // null transaction type - pass
			SetupResult.For(debitOrder.TransactionType).Return(null);
			ExecuteRule(rule, 0, debitOrder);

            // ManualDebitOrder and null BankAccount - fail
			SetupResult.For(debitOrder.TransactionType).Return(transactionType);
			SetupResult.For(debitOrder.BankAccount).Return(null);
            SetupResult.For(transactionType.Key).Return((short)TransactionTypes.ManualDebitOrderPayment);
			ExecuteRule(rule, 1, debitOrder);

            // ManualDebitOrder and BankAccount not null - pass
			SetupResult.For(debitOrder.TransactionType).Return(transactionType);
			SetupResult.For(debitOrder.BankAccount).Return(bankAccount);
            SetupResult.For(transactionType.Key).Return((short)TransactionTypes.ManualDebitOrderPayment);
			ExecuteRule(rule, 0, debitOrder);
        }

        [Test]
        public void ManualDebitOrderAmountMinimumTest()
        {
			ManualDebitOrderAmountMinimum rule = new ManualDebitOrderAmountMinimum();
			var debitOrder = _mockery.StrictMock<IManualDebitOrder>();

            // null - fail
			SetupResult.For(debitOrder.Amount).Return(new double());
			ExecuteRule(rule, 1, debitOrder);

            // 0 - fail
			SetupResult.For(debitOrder.Amount).Return(0);
			ExecuteRule(rule, 1, debitOrder);

            // 1 - pass
			SetupResult.For(debitOrder.Amount).Return(1);
			ExecuteRule(rule, 0, debitOrder);
        }


        #region ManualDebitOrderDeleteDebitOrderCheckUserTest
        /// <summary>
        /// If the current offer linked to this account has an offertypekey of 2 (Re-Advance) or 3 (Further Loan) and these offers do not have an NTU Offer 
        /// (stagedefinitionstagedefinitionGroupCompositeKey=110) NTU or Decline it means that the further loan / re-advance is in progress 
        /// hence do not allow the consultant to Proceed; render error message instead.  
        /// </summary>
        [NUnit.Framework.Test]
        public void ManualDebitOrderDeleteDebitOrderCheckUserTest()
        {
            // PASS - usernames the same
            ManualDebitOrderDeleteDebitOrderCheckUserHelper(0, "SAHL\\BcUser1", "SAHL\\BCUser1");
            // FAIL - usernames different
            ManualDebitOrderDeleteDebitOrderCheckUserHelper(1, "SAHL\\BcUser2", "SAHL\\BCUser1");


        }

        /// <summary>
        /// Helper method to set up the expectations for the ManualDebitOrderDeleteDebitOrderCheckUser test.
        /// </summary>
        private void ManualDebitOrderDeleteDebitOrderCheckUserHelper(int expectedMessageCount, string rtUserName, string passedInUserName)
        {
			using (new SessionScope())
			{
				ManualDebitOrderDeleteDebitOrderCheckUser rule = new ManualDebitOrderDeleteDebitOrderCheckUser();

				//IManualDebitOrder rt = _mockery.StrictMock<IManualDebitOrder>();
				var original = _mockery.StrictMock<IManualDebitOrder>();

				SetupResult.For(original.UserID).Return(rtUserName);

				string userName = passedInUserName;

				ExecuteRule(rule, expectedMessageCount, original, userName);
			}
        }

        #endregion

		#region ManualDebitOrderDeleteDebitOrderCheckExistingCaseTest
		/// <summary>
		/// This rule checks that a delete request does not already exist for the Debit Order (ManualDebitOrders).
		/// </summary>
		[NUnit.Framework.Test]
		public void ManualDebitOrderDeleteDebitOrderCheckExistingCaseTest()
		{
			// PASS - no case returned
			ManualDebitOrderDeleteDebitOrderCheckExistingCaseUserHelper(0);
		}

		/// <summary>
		/// Helper method to set up the expectations for the ManualDebitOrderDeleteDebitOrderCheckExistingCase test.
		/// </summary>
		private void ManualDebitOrderDeleteDebitOrderCheckExistingCaseUserHelper(int expectedMessageCount)
		{
			using (new SessionScope())
			{
				ManualDebitOrderDeleteDebitOrderCheckExistingCase rule = new ManualDebitOrderDeleteDebitOrderCheckExistingCase();
				var debitOrder = _mockery.StrictMock<IManualDebitOrder>();

				SetupResult.For(debitOrder.Key).Return(1);

				ExecuteRule(rule, expectedMessageCount, debitOrder);
			}
		}

		#endregion

        #region ManualDebitOrderStartDateMaximumCheck

        [Test]
        public void ManualDebitOrderStartDateMaximumCheckTestPass()
        {
            ManualDebitOrderStartDateMaximumCheck rule = new ManualDebitOrderStartDateMaximumCheck();
			var debitOrder = _mockery.StrictMock<IManualDebitOrder>();
            DateTime dt = new DateTime(2011, 1, 20);
			SetupResult.For(debitOrder.ActionDate).Return(dt);
			ExecuteRule(rule, 0, debitOrder);
        }

        [Test]
        public void ManualDebitOrderStartDateMaximumCheckTestFails()
        {
            ManualDebitOrderStartDateMaximumCheck rule = new ManualDebitOrderStartDateMaximumCheck();
			var debitOrder = _mockery.StrictMock<IManualDebitOrder>();
			DateTime dt = new DateTime(2011, 1, 29);
			SetupResult.For(debitOrder.ActionDate).Return(dt);
			ExecuteRule(rule, 1, debitOrder);
        }



        #endregion
    }
}
