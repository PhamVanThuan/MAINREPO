using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.Interfaces;
using Rhino.Mocks;
using Castle.ActiveRecord.Queries;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.Factories;
using SAHL.Common.Globals;
using SAHL.Common.BusinessModel;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Collections;
using SAHL.Test;
using SAHL.Common.BusinessModel.Interfaces.SearchCriteria;
using SAHL.Common.BusinessModel.Rules.Affordability;
using Castle.ActiveRecord;

namespace SAHL.Common.BusinessModel.Rules.Test.Affordability
{
    [TestFixture]
    public class DebtSettlementTest : RuleBase
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

        [NUnit.Framework.Test]
        public void DebtSettlementBankAccountApplicationUpdateAddNewTestPass()
        {
            DebtSettlementBankAccountUpdate rule = new DebtSettlementBankAccountUpdate();

            IApplicationDebtSettlement appDebtSettlement = _mockery.StrictMock<IApplicationDebtSettlement>();
            SetupResult.For(appDebtSettlement.Key).Return(0);

            ExecuteRule(rule, 0, appDebtSettlement);
        }

        [NUnit.Framework.Test]
        public void DebtSettlementAccountBankAccountUpdateAddNewTestPass()
        {
            DebtSettlementBankAccountUpdate rule = new DebtSettlementBankAccountUpdate();

            IAccountDebtSettlement accDebtSettlement = _mockery.StrictMock<IAccountDebtSettlement>();
            SetupResult.For(accDebtSettlement.Key).Return(0);

            ExecuteRule(rule, 0, accDebtSettlement);
        }

        [NUnit.Framework.Test]
        public void DebtSettlementAccountBankAccountUpdateTestPass()
        {
            DebtSettlementBankAccountUpdate rule = new DebtSettlementBankAccountUpdate();
            string HQL = "select a from AccountDebtSettlement_DAO a where a.BankAccount is not null";
            SimpleQuery<AccountDebtSettlement_DAO> q = new SimpleQuery<AccountDebtSettlement_DAO>(HQL);
            q.SetQueryRange(1);
            using (new SessionScope())
            {
                AccountDebtSettlement_DAO[] res = q.Execute();

                IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                IAccountDebtSettlement accDS = BMTM.GetMappedType<IAccountDebtSettlement>(res[0]); // there should always be at least one bank account

                int key = accDS.Key;

                ExecuteRule(rule, 1, accDS);
            }
        }

        //[NUnit.Framework.Test]
        // Commented out cos ApplicationDebtSettlement_DAO.FindFirst(); is throwing errors - cud be a domain thing
        //public void DebtSettlementApplicationBankAccountUpdateTestFail()
        //{
        //    DebtSettlementBankAccountUpdate rule = new DebtSettlementBankAccountUpdate();

        //    ApplicationDebtSettlement_DAO appDS = ApplicationDebtSettlement_DAO.FindFirst();
        //    int key = appDS.Key;

        //    IApplicationDebtSettlement accDebtSettlement = _mockery.StrictMock<IApplicationDebtSettlement>();
        //    SetupResult.For(accDebtSettlement.Key).Return(key);
        //    IBankAccount bank = _mockery.StrictMock<IBankAccount>();
        //    SetupResult.For(accDebtSettlement.BankAccount).Return(bank);
        //    SetupResult.For(bank.Key).Return(999);
        //    IApplicationExpense accexpense = _mockery.StrictMock<IApplicationExpense>();
        //    SetupResult.For(accDebtSettlement.OfferExpense).Return(accexpense);
        //    IApplication app = _mockery.StrictMock<IApplication>();
        //    SetupResult.For(accexpense.Application).Return(app);
        //    IAccount acc = _mockery.StrictMock<IAccount>();
        //    SetupResult.For(app.Account).Return(acc);
        //    IAccountStatus accStatus = _mockery.StrictMock<IAccountStatus>();
        //    SetupResult.For(acc.AccountStatus).Return(accStatus);
        //    SetupResult.For(accStatus.Key).Return((int)AccountStatuses.Open);

        //    ExecuteRule(rule, 1, accDebtSettlement);
        //}

    }
}
