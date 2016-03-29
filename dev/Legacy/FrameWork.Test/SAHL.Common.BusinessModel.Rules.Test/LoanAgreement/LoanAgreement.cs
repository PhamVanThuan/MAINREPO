using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using SAHL.Common.BusinessModel.Interfaces;
using Rhino.Mocks;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.BusinessModel;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.Collections;
using Castle.ActiveRecord;
using Castle.ActiveRecord.Queries;
using SAHL.Common.BusinessModel.Rules.LoanAgreement;
using SAHL.Common.Factories;

namespace SAHL.Common.BusinessModel.Rules.Test.LoanAgreement
{
    [TestFixture]
    public class LoanAgreement : RuleBase
    {
        ILoanAgreement la = null;
        //IRuleParameter paramMin;
        //IRuleParameter paramMax;
        //IEventList<IRuleParameter> parameters;

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

        #region MortgageLoanAgreementAmount

        #region IncorrectArgumentsPassedFail

        [NUnit.Framework.Test]
        [ExpectedException(typeof(ArgumentException), ExpectedMessage = "Parameter[0] is not of type ILoanAgreement.")]
        public void LoanAgreementAmountArgumemntsPassedFail()
        {
            MortgageLoanAgreementAmount rule = new MortgageLoanAgreementAmount();
            ILegalEntityCompany legalEntityCompany = _mockery.StrictMock<ILegalEntityCompany>();
            ExecuteRule(rule, 0, legalEntityCompany);
        }

        #endregion

        #region LoanAgreementAmount

        [NUnit.Framework.Test]
        public void LoanAgreementAmountHighFail()
        {
            MortgageLoanAgreementAmount rule = new MortgageLoanAgreementAmount();
            IBond bond = _mockery.StrictMock<IBond>();

            la = _mockery.StrictMock<ILoanAgreement>();
            SetupResult.For(la.Amount).IgnoreArguments().Return(1000000000.00);
            SetupResult.For(bond.Key).Return(0);
            SetupResult.For(bond.BondRegistrationAmount).Return(9D);
            SetupResult.For(la.Bond).Return(bond);

            ExecuteRule(rule, 1, la);
        }

        [NUnit.Framework.Test]
        public void LoanAgreementAmountLowFail()
        {
            MortgageLoanAgreementAmount rule = new MortgageLoanAgreementAmount();

            IBond bond = _mockery.StrictMock<IBond>();

            la = _mockery.StrictMock<ILoanAgreement>();
            SetupResult.For(la.Amount).Return(0.00);
            SetupResult.For(bond.Key).Return(0);
            SetupResult.For(bond.BondRegistrationAmount).Return(9D);
            SetupResult.For(la.Bond).Return(bond);

            ExecuteRule(rule, 1, la);
        }

        [NUnit.Framework.Test]
        public void LoanAgreementAmountPass()
        {
            MortgageLoanAgreementAmount rule = new MortgageLoanAgreementAmount();
            IBond bond = _mockery.StrictMock<IBond>();
            la = _mockery.StrictMock<ILoanAgreement>();

            SetupResult.For(la.Amount).Return(1.00);
            SetupResult.For(bond.Key).Return(0);
            SetupResult.For(bond.BondRegistrationAmount).Return(9D);
            SetupResult.For(la.Bond).Return(bond);

            ExecuteRule(rule, 0, la);
        }

        #endregion

        #endregion

        #region MortgageLoanAgreementSum

        [NUnit.Framework.Test]
        public void MortgageLoanAgreementSumVariFixPass()
        {
            MortgageLoanAgreementSum rule = new MortgageLoanAgreementSum();

            using (new SessionScope())
            {
                la = _mockery.StrictMock<ILoanAgreement>();

                // we need to get hold of an account that is under cancellation
                string hql = "SELECT a FROM AccountVariFixLoan_DAO a WHERE a.AccountStatus.Key = ?";
                SimpleQuery<AccountVariFixLoan_DAO> q = new SimpleQuery<AccountVariFixLoan_DAO>(hql, (int)SAHL.Common.Globals.AccountStatuses.Open);
                q.SetQueryRange(1);
                AccountVariFixLoan_DAO[] acc = q.Execute();

                if (acc == null || acc.Length == 0)
                {
                    Assert.Fail("Unable to locate any active VariFix Accounts.");
                    return;
                }

                IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                IMortgageLoanAccount mla = BMTM.GetMappedType<IAccount>(acc[0]) as IMortgageLoanAccount;
                SetupResult.For(la.Amount).Return(mla.LoanCurrentBalance + 100000.00);
                IAccountVariFixLoan vfL = BMTM.GetMappedType<IAccountVariFixLoan>(acc[0]) as IAccountVariFixLoan;
                SetupResult.For(la.Bond).Return(vfL.SecuredMortgageLoan.Bonds[0]);

                ExecuteRule(rule, 0, la);
            }

        }

        [NUnit.Framework.Test]
        public void MortgageLoanAgreementSumVariablePass()
        {
            MortgageLoanAgreementSum rule = new MortgageLoanAgreementSum();

            using (new SessionScope())
            {
                la = _mockery.StrictMock<ILoanAgreement>();

                // we need to get hold of an account that is under cancellation
				string hql = "select mort.Account from MortgageLoan_DAO mort where mort.Account.AccountStatus.Key = ? and mort.FinancialServiceType.Key = ? and mort.Account.Product.Key = ?";
				SimpleQuery<AccountVariableLoan_DAO> q = new SimpleQuery<AccountVariableLoan_DAO>(hql, (int)SAHL.Common.Globals.AccountStatuses.Open, (int)SAHL.Common.Globals.FinancialServiceTypes.VariableLoan, (int)SAHL.Common.Globals.Products.VariableLoan);
                q.SetQueryRange(1);
                AccountVariableLoan_DAO[] acc = q.Execute();

                if (acc == null || acc.Length == 0)
                {
                    Assert.Fail("Unable to locate any active Variable Accounts.");
                    return;
                }
                IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                IMortgageLoanAccount mla = BMTM.GetMappedType<IAccount>(acc[0]) as IMortgageLoanAccount;
                SetupResult.For(la.Amount).Return(mla.LoanCurrentBalance + 100000.00);
                IAccountVariableLoan vL = BMTM.GetMappedType<IAccountVariableLoan>(acc[0].As<AccountVariableLoan_DAO>()) as IAccountVariableLoan;
                SetupResult.For(la.Bond).Return(vL.SecuredMortgageLoan.Bonds[0]);

                ExecuteRule(rule, 0, la);
            }

        }


        [NUnit.Framework.Test]
        public void MortgageLoanAgreementSumVariFixFail()
        {
            MortgageLoanAgreementSum rule = new MortgageLoanAgreementSum();

            using (new SessionScope())
            {
                la = _mockery.StrictMock<ILoanAgreement>();
                SetupResult.For(la.Amount).Return(-1000000.00);

                // we need to get hold of an account that is under cancellation
                string hql = "SELECT a FROM AccountVariFixLoan_DAO a WHERE a.AccountStatus.Key = ?";
                SimpleQuery<AccountVariFixLoan_DAO> q = new SimpleQuery<AccountVariFixLoan_DAO>(hql, (int)SAHL.Common.Globals.AccountStatuses.Open);
                q.SetQueryRange(1);
                AccountVariFixLoan_DAO[] acc = q.Execute();

                if (acc == null || acc.Length == 0)
                {
                    Assert.Fail("Unable to locate any active VariFix Accounts.");
                    return;
                }
                IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                IAccountVariFixLoan vfL = BMTM.GetMappedType<IAccountVariFixLoan>(acc[0]) as IAccountVariFixLoan;
                SetupResult.For(la.Bond).Return(vfL.SecuredMortgageLoan.Bonds[0]);

                ExecuteRule(rule, 1, la);
            }

        }

        [NUnit.Framework.Test]
        public void MortgageLoanAgreementSumVariableFail()
        {
            MortgageLoanAgreementSum rule = new MortgageLoanAgreementSum();

            using (new SessionScope())
            {
                la = _mockery.StrictMock<ILoanAgreement>();
                SetupResult.For(la.Amount).Return(-1000000.00);

                // we need to get hold of an account that is under cancellation
				string hql = "select mort.Account from MortgageLoan_DAO mort where mort.Account.AccountStatus.Key = ? and mort.FinancialServiceType.Key = ? and mort.Account.Product.Key = ?";
                SimpleQuery<AccountVariableLoan_DAO> q = new SimpleQuery<AccountVariableLoan_DAO>(hql, (int)SAHL.Common.Globals.AccountStatuses.Open, (int)SAHL.Common.Globals.FinancialServiceTypes.VariableLoan, (int)SAHL.Common.Globals.Products.VariableLoan);
                q.SetQueryRange(1);
                AccountVariableLoan_DAO[] acc = q.Execute();

                if (acc == null || acc.Length == 0)
                {
                    Assert.Fail("Unable to locate any active Variable Accounts.");
                    return;
                }
                IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                IAccountVariableLoan vL = BMTM.GetMappedType<IAccountVariableLoan>(acc[0].As<AccountVariableLoan_DAO>()) as IAccountVariableLoan;
                SetupResult.For(la.Bond).Return(vL.SecuredMortgageLoan.Bonds[0]);

                ExecuteRule(rule, 1, la);
            }

        }


        #endregion

        #region MortgageLoanAgreementBondValue

        [NUnit.Framework.Test]
        public void MortgageLoanAgreementBondValueFail()
        {
            MortgageLoanAgreementBondValue rule = new MortgageLoanAgreementBondValue();

            la = _mockery.StrictMock<ILoanAgreement>();
            IBond bond = _mockery.StrictMock<IBond>();

            SetupResult.For(bond.Key).Return(1);
            SetupResult.For(bond.BondLoanAgreementAmount).Return(1.00);
            SetupResult.For(bond.BondRegistrationAmount).Return(1.00);
            SetupResult.For(la.Amount).Return(10.00);

            SetupResult.For(la.Bond).Return(bond);

            ExecuteRule(rule, 1, la);
        }

        [NUnit.Framework.Test]
        public void MortgageLoanAgreementBondValuePass()
        {
            MortgageLoanAgreementBondValue rule = new MortgageLoanAgreementBondValue();

            la = _mockery.StrictMock<ILoanAgreement>();
            IBond bond = _mockery.StrictMock<IBond>();

            SetupResult.For(bond.Key).Return(1);
            SetupResult.For(bond.BondLoanAgreementAmount).Return(1.00);
            SetupResult.For(bond.BondRegistrationAmount).Return(100.00);
            SetupResult.For(la.Amount).Return(10.00);

            SetupResult.For(la.Bond).Return(bond);

            ExecuteRule(rule, 0, la);


        }

        #endregion

        //private void SetupParams()
        //{
        //    // mock the IRuleParam for min and max age
        //    paramMin = _mockery.StrictMock<IRuleParameter>();
        //    SetupResult.For(paramMin.Name).Return("@MinAmount");
        //    SetupResult.For(paramMin.Value).Return("0.01");

        //    paramMax = _mockery.StrictMock<IRuleParameter>();
        //    SetupResult.For(paramMax.Name).Return("@MaxAmount");
        //    SetupResult.For(paramMax.Value).Return("999999999.99");

        //    // mock the IEventlist that we step over
        //    parameters = new EventList<IRuleParameter>();
        //    parameters.Add(null, paramMin);
        //    parameters.Add(null, paramMax);
        //    SetupResult.For(ruleItem.RuleParameters).Return(parameters);
        //}
    }
}
