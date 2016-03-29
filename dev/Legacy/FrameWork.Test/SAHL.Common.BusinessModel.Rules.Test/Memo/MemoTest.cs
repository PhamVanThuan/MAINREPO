using System;
using System.Collections.Generic;
using System.Text;
using SAHL.Test;
using NUnit.Framework;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Collections;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.Interfaces;
using Rhino.Mocks;
using Microsoft.Practices.EnterpriseLibrary.Caching;
using SAHL.Common.BusinessModel;
using SAHL.Common.Globals;
using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Rules.Memo;

namespace SAHL.Common.BusinessModel.Rules.Test.Memo
{
    [TestFixture]
    public class MemoTest : RuleBase
    {
        IMemo memo = null;

        [SetUp]
        public override void Setup()
        {
            base.Setup();
            memo = _mockery.StrictMock<IMemo>();
            IGeneralStatus generalStatus = _mockery.StrictMock<IGeneralStatus>();
            SetupResult.For(generalStatus.Key).Return((int)GeneralStatuses.Active);
            SetupResult.For(memo.GeneralStatus).Return(generalStatus);
        }

        [TearDown]
        public override void TearDown()
        {
            base.TearDown();
        }


        [NUnit.Framework.Test]
        public void MemoAddUpdateMemoReminderDateTestFail()
        {
            MemoAddUpdateMemoReminderDate rule = new MemoAddUpdateMemoReminderDate();
            
            DateTime? reminderDate = DateTime.Now.AddDays(3);
            DateTime? expiryDate = DateTime.Now.AddDays(2);

            SetupResult.For(memo.ReminderDate).IgnoreArguments().Return(reminderDate);

            SetupResult.For(memo.ExpiryDate).IgnoreArguments().Return(expiryDate);
         
            ExecuteRule(rule, 1, memo);
        }

        [NUnit.Framework.Test]
        public void MemoAddUpdateMemoReminderDateTestPass()
        {
            MemoAddUpdateMemoReminderDate rule = new MemoAddUpdateMemoReminderDate();

            DateTime? reminderDate = DateTime.Now.AddDays(2);
            DateTime? expiryDate = DateTime.Now.AddDays(3);

            SetupResult.For(memo.ReminderDate).IgnoreArguments().Return(reminderDate);
            SetupResult.For(memo.ExpiryDate).IgnoreArguments().Return(expiryDate);

            ExecuteRule(rule, 0, memo);
        }

        [NUnit.Framework.Test]
        public void MemoAddUpdateMemoExpiryDateTestFail()
        {
            MemoAddUpdateMemoExpiryDate rule = new MemoAddUpdateMemoExpiryDate();
            DateTime expiryDate = DateTime.Now.AddDays(-1);
            SetupResult.For(memo.ExpiryDate).IgnoreArguments().Return(expiryDate);
            ExecuteRule(rule, 1, memo);
        }

        [NUnit.Framework.Test]
        public void MemoAddUpdateMemoExpiryDateTestPass()
        {
            MemoAddUpdateMemoExpiryDate rule = new MemoAddUpdateMemoExpiryDate();
            DateTime expiryDate = DateTime.Now.AddDays(3);
            
            SetupResult.For(memo.ExpiryDate).IgnoreArguments().Return(expiryDate);

            ExecuteRule(rule, 0, memo);
        }

        [NUnit.Framework.Test]
        public void MemoAddUpdateReminderDateinTheFutureTestPass()
        {
            MemoAddUpdateReminderDate rule = new MemoAddUpdateReminderDate();
            DateTime reminderDate = DateTime.Now.AddDays(3);

            SetupResult.For(memo.ReminderDate).IgnoreArguments().Return(reminderDate);

            ExecuteRule(rule, 0, memo);
        }

        [NUnit.Framework.Test]
        public void MemoAddUpdateReminderDateinThePastTestFail()
        {
            MemoAddUpdateReminderDate rule = new MemoAddUpdateReminderDate();
            DateTime reminderDate = DateTime.Now.AddDays(-3);

            SetupResult.For(memo.ReminderDate).IgnoreArguments().Return(reminderDate);

            ExecuteRule(rule, 1, memo);
        }

        [NUnit.Framework.Test]
        public void MemoAddUpdateReminderDateTodayTestPass()
        {
            MemoAddUpdateReminderDate rule = new MemoAddUpdateReminderDate();
            DateTime reminderDate = DateTime.Now.Date;
            SetupResult.For(memo.ReminderDate).IgnoreArguments().Return(reminderDate);

            ExecuteRule(rule, 0, memo);
        }


        [NUnit.Framework.Test]
        public void MemoAddUpdateMemoReminderDateMandatoryTestPass()
        {
            MemoAddUpdateMemoReminderDateMandatory rule = new MemoAddUpdateMemoReminderDateMandatory();
            DateTime? reminderDate = DateTime.Now.AddDays(2);
            SetupResult.For(memo.ReminderDate).IgnoreArguments().Return(reminderDate);
            ExecuteRule(rule, 0, memo);
        }

        [NUnit.Framework.Test]
        public void MemoAddUpdateMemoReminderDateMandatoryTestFail()
        {
            MemoAddUpdateMemoReminderDateMandatory rule = new MemoAddUpdateMemoReminderDateMandatory();
            DateTime? reminderDate = null;
            SetupResult.For(memo.ReminderDate).IgnoreArguments().Return(reminderDate);
            ExecuteRule(rule, 1, memo);
        }

        [NUnit.Framework.Test]
        public void MemoAddUpdateMemoExpiryDateMandatoryTestPass()
        {
            MemoAddUpdateMemoExpiryDateMandatory rule = new MemoAddUpdateMemoExpiryDateMandatory();
            DateTime? expiryDate = DateTime.Now.AddDays(3);
            SetupResult.For(memo.ExpiryDate).IgnoreArguments().Return(expiryDate);
            ExecuteRule(rule, 0, memo);
        }

        [NUnit.Framework.Test]
        public void MemoAddUpdateMemoExpiryDateMandatoryTestFail()
        {
            MemoAddUpdateMemoExpiryDateMandatory rule = new MemoAddUpdateMemoExpiryDateMandatory();
            DateTime? expiryDate = null;
            SetupResult.For(memo.ExpiryDate).IgnoreArguments().Return(expiryDate);
            ExecuteRule(rule, 1, memo);
        }

        [NUnit.Framework.Test]
        public void MemoAddUpdateDescriptionTestPass()
        {
            MemoAddUpdateDescription rule = new MemoAddUpdateDescription();
            SetupResult.For(memo.Description).Return("Description");
            ExecuteRule(rule, 0, memo);
        }

        [NUnit.Framework.Test]
        public void MemoAddUpdateDescriptionTestFail()
        {
            MemoAddUpdateDescription rule = new MemoAddUpdateDescription();
            SetupResult.For(memo.Description).Return(" ");
            ExecuteRule(rule, 1, memo);

        }

    }

}
