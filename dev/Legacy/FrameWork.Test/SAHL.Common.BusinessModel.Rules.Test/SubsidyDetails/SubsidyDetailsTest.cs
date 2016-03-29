using System;
using System.Collections.Generic;
using System.Text;
using Castle.ActiveRecord;
using Castle.ActiveRecord.Queries;
using Microsoft.Practices.EnterpriseLibrary.Caching;
using NUnit.Framework;
using Rhino.Mocks;
using SAHL.Common.BusinessModel;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Helpers;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.Interfaces.Service;
using SAHL.Common.BusinessModel.Rules.SubsidyDetails;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Factories;
using SAHL.Common.Globals;
using SAHL.Test;

namespace SAHL.Common.BusinessModel.Rules.Test.SubsidyDetails
{
    [TestFixture]
    public class SubsidyDetailsTest : RuleBase
    {
        ISubsidy subsidy;

        [SetUp]
        public override void Setup()
        {
            base.Setup();
            subsidy = _mockery.StrictMock<ISubsidy>();
        }

        [TearDown]
        public override void TearDown()
        {
            base.TearDown();
        }

        [NUnit.Framework.Test]
        public void SubsidyDetailsValidateStopOrderAmountTest()
        {
            double max = 999999999.99D;

            // amount = minimum -> pass
            SubsidyDetailsValidateStopOrderAmountHelper(EmploymentStatuses.Current, 0.01D, 0);

            // amount = maximum -> pass
            SubsidyDetailsValidateStopOrderAmountHelper(EmploymentStatuses.Current, max, 0);

            // amount in between minimum maximum -> pass
            SubsidyDetailsValidateStopOrderAmountHelper(EmploymentStatuses.Current, 50D, 0);

            // amount < minimum -> fail
            SubsidyDetailsValidateStopOrderAmountHelper(EmploymentStatuses.Current, 0D, 1);

            // amount > maximum -> fail
            SubsidyDetailsValidateStopOrderAmountHelper(EmploymentStatuses.Current, (max + 1D), 1);

            // amount < minimum but with previous employment -> pass
            SubsidyDetailsValidateStopOrderAmountHelper(EmploymentStatuses.Previous, 0D, 0);

            // amount > maximum but with previous employment -> pass
            SubsidyDetailsValidateStopOrderAmountHelper(EmploymentStatuses.Previous, (max + 1D), 0);
        }

        /// <summary>
        /// Helper function for testing the SubsidyDetailsValidateStopOrderAmount rule.
        /// </summary>
        private void SubsidyDetailsValidateStopOrderAmountHelper(EmploymentStatuses status, double stopOrderAmount, int expectedErrorCount)
        {
            SubsidyDetailsValidateStopOrderAmount rule = new SubsidyDetailsValidateStopOrderAmount();

            // set up the subsidy object
            IEmploymentSubsidised employment = _mockery.StrictMock<IEmploymentSubsidised>();
            IEmploymentStatus empStatus = _mockery.StrictMock<IEmploymentStatus>();
            SetupResult.For(employment.EmploymentStatus).Return(empStatus);
            SetupResult.For(empStatus.Key).Return((int)status);

            SetupResult.For(subsidy.StopOrderAmount).IgnoreArguments().Return(stopOrderAmount);
            SetupResult.For(subsidy.Employment).Return(employment);

            ExecuteRule(rule, expectedErrorCount, subsidy);
        }

        [Test]
        public void SubsidyDetailsMandatoryAccountOrApplication()
        {
            IAccount account = _mockery.StrictMock<IAccount>();
            IApplication application = _mockery.StrictMock<IApplication>();

            // null AccountSubsidy and ApplicationSubsidy - should fail
            SubsidyDetailsMandatoryAccountOrApplicationHelper(EmploymentStatuses.Current, null, null, 1);

            // real AccountSubsidy and ApplicationSubsidy with Account or Application - should pass
            SubsidyDetailsMandatoryAccountOrApplicationHelper(EmploymentStatuses.Current, account, application, 1);

            // real AccountSubsidy Account and null Application - should pass
            SubsidyDetailsMandatoryAccountOrApplicationHelper(EmploymentStatuses.Current, account, null, 1);

            // real AccountSubsidy and ApplicationSubsidy with Application but null Account - should pass
            SubsidyDetailsMandatoryAccountOrApplicationHelper(EmploymentStatuses.Current, null, application, 1);

            // real AccountSubsidy and ApplicationSubsidy with Account or Application but previous employment - should pass
            SubsidyDetailsMandatoryAccountOrApplicationHelper(EmploymentStatuses.Previous, account, application, 0);
        }

        private void SubsidyDetailsMandatoryAccountOrApplicationHelper(EmploymentStatuses status,
            IAccount account, IApplication application,
            int expectedMessageCount)
        {
            SubsidyDetailsMandatoryAccountOrApplication rule = new SubsidyDetailsMandatoryAccountOrApplication(RepositoryFactory.GetRepository<ICastleTransactionsService>());
            ISubsidy subsidy = _mockery.StrictMock<ISubsidy>();

            if (account != null)
            {
                SetupResult.For(account.Key).Return(1);
            }
            if (application != null)
                SetupResult.For(application.Key).Return(1);

            SetupResult.For(subsidy.Account).Return(account);
            SetupResult.For(subsidy.Application).Return(application);

            SetupResult.For(subsidy.Key).Return(1);

            // set up the employment details
            IEmploymentSubsidised employment = _mockery.StrictMock<IEmploymentSubsidised>();
            IEmploymentStatus empStatus = _mockery.StrictMock<IEmploymentStatus>();
            SetupResult.For(employment.EmploymentStatus).Return(empStatus);
            SetupResult.For(empStatus.Key).Return((int)status);
            SetupResult.For(subsidy.Employment).Return(employment);

            ExecuteRule(rule, expectedMessageCount, subsidy);
        }

        [Test]
        public void SubsidyDetailsMandatorySalaryNumber()
        {
            SubsidyDetailsMandatorySalaryNumber rule = new SubsidyDetailsMandatorySalaryNumber();

            // null and empty salary number - fail
            SubsidyDetailsMandatorySalaryNumberHelper(EmploymentStatuses.Current, null, 1);
            SubsidyDetailsMandatorySalaryNumberHelper(EmploymentStatuses.Current, "", 1);

            // salary number - pass
            SubsidyDetailsMandatorySalaryNumberHelper(EmploymentStatuses.Current, "TEST", 0);

            // null salary number but with status of previous - pass
            SubsidyDetailsMandatorySalaryNumberHelper(EmploymentStatuses.Previous, null, 0);
        }

        private void SubsidyDetailsMandatorySalaryNumberHelper(EmploymentStatuses status, string salaryNo, int expectedMessageCount)
        {
            SubsidyDetailsMandatorySalaryNumber rule = new SubsidyDetailsMandatorySalaryNumber();

            ISubsidy subsidy = _mockery.StrictMock<ISubsidy>();

            SetupResult.For(subsidy.SalaryNumber).Return(salaryNo);

            // set up the employment details
            IEmploymentSubsidised employment = _mockery.StrictMock<IEmploymentSubsidised>();
            IEmploymentStatus empStatus = _mockery.StrictMock<IEmploymentStatus>();
            SetupResult.For(employment.EmploymentStatus).Return(empStatus);
            SetupResult.For(empStatus.Key).Return((int)status);
            SetupResult.For(subsidy.Employment).Return(employment);

            ExecuteRule(rule, expectedMessageCount, subsidy);
        }

        #region SubsidyDetailsUpdateSubsidyAmount

        [Test]
        public void SubsidyDetailsUpdateSubsidyAmountTest()
        {
            SubsidyDetailsUpdateSubsidyAmountHelper(1, 1000.00, (int)EmploymentStatuses.Current, true, 1000.00, 1000.00, 1000.00, 1000.00);
            SubsidyDetailsUpdateSubsidyAmountHelper(0, 1000.00, (int)EmploymentStatuses.Current, true, 0.00, 0.00, 1000.00, 0.00);
            SubsidyDetailsUpdateSubsidyAmountHelper(0, 1000.00, (int)EmploymentStatuses.Current, true, 1000.00, 1000.00, 5000.00, 1000.00);
        }

        private void SubsidyDetailsUpdateSubsidyAmountHelper(int expectedMessageCount, double fixedPayment, int empStat, bool isAccActive, double fsPayment, double debitOrderAmount, double subStopOrder, double regentPremium)
        {
            SubsidyDetailsUpdateSubsidyAmount rule = new SubsidyDetailsUpdateSubsidyAmount();
            ISubsidy subsidy = _mockery.StrictMock<ISubsidy>();

            IAccount acc = _mockery.StrictMock<IAccount>();
            SetupResult.For(acc.FixedPayment).Return(fixedPayment);
            SetupResult.For(acc.IsActive).Return(isAccActive);
            SetupResult.For(acc.Key).Return(1);

            IEventList<IAccount> childAccs = new EventList<IAccount>();

            SetupResult.For(acc.RelatedChildAccounts).IgnoreArguments().Return(childAccs);

            IEmploymentSubsidised emp = _mockery.StrictMock<IEmploymentSubsidised>();
            IEmploymentStatus empStatus = _mockery.StrictMock<IEmploymentStatus>();
            SetupResult.For(empStatus.Key).Return(empStat);
            SetupResult.For(emp.EmploymentStatus).Return(empStatus);
            SetupResult.For(subsidy.Employment).Return(emp);

            IEventList<IFinancialService> fss = new EventList<IFinancialService>();

            IFinancialService fs = _mockery.StrictMock<IFinancialService>();
            IAccountStatus accStatus = _mockery.StrictMock<IAccountStatus>();
            SetupResult.For(accStatus.Key).Return((int)AccountStatuses.Open);
            SetupResult.For(fs.AccountStatus).Return(accStatus);
            SetupResult.For(fs.Payment).Return(fsPayment);

            IEventList<IManualDebitOrder> manualDebitOrders = new EventList<IManualDebitOrder>();
            IManualDebitOrder manualDebitOrder = _mockery.StrictMock<IManualDebitOrder>();
            SetupResult.For(manualDebitOrder.Active).Return(true);
            ITransactionType transType = _mockery.StrictMock<ITransactionType>();
            SetupResult.For(transType.Key).Return((Int16)TransactionTypes.MonthlyServiceFee);
            SetupResult.For(manualDebitOrder.TransactionType).Return(transType);
            SetupResult.For(manualDebitOrder.Amount).Return(debitOrderAmount);

            SetupResult.For(subsidy.StopOrderAmount).IgnoreArguments().Return(subStopOrder);

            SetupResult.For(fs.ManualDebitOrders).Return(manualDebitOrders);

            IAccountInstallmentSummary accountInstallmentSummary = _mockery.StrictMock<IAccountInstallmentSummary>();
            SetupResult.For(acc.InstallmentSummary).Return(accountInstallmentSummary);
            SetupResult.For(accountInstallmentSummary.TotalRegentPremium).Return(regentPremium);

            manualDebitOrders.Add(Messages, manualDebitOrder);

            fss.Add(Messages, fs);

            SetupResult.For(acc.FinancialServices).Return(fss);
            SetupResult.For(subsidy.Account).Return(acc);

            ExecuteRule(rule, expectedMessageCount, subsidy);
        }

        #endregion SubsidyDetailsUpdateSubsidyAmount
    }
}