using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using SAHL.Common.BusinessModel.Interfaces;
using Rhino.Mocks;

namespace SAHL.Common.BusinessModel.Rules.Test.Products
{
    [TestFixture]
    public class Variable : RuleBase
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

        #region NotValid

        [NUnit.Framework.Test]
        public void VariableClassTest()
        {
            Assert.IsTrue(true);
        }

        //#region IncorrectArgumemntsPassedFail

        //[NUnit.Framework.Test]
        //[ExpectedException(typeof(ArgumentException), ExpectedMessage = "Parameter[0] is not of type IMortgageLoan.")]
        //public void VariableNoExtendedTermArgumemntsPassedFail()
        //{
        //    SAHL.Rules.Products.VariableNoExtendedTerm rule = new SAHL.Rules.Products.VariableNoExtendedTerm();
        //    ILegalEntityCompany legalEntityCompany = _mockery.StrictMock<ILegalEntityCompany>();
        //    rule.ExecuteRule(Messages, "", true, new Object[] { legalEntityCompany });
        //}


        //#endregion

        //[NUnit.Framework.Test]
        //public void VariableNoExtendedTermVariableExtendedTermFail()
        //{
        //    //ml.Account.Product.Key
        //    SAHL.Rules.Products.VariableNoExtendedTerm rule = new SAHL.Rules.Products.VariableNoExtendedTerm();
        //    SetupGetRule();


        //    ml = _mockery.StrictMock<IMortgageLoan>();
        //    acc = _mockery.StrictMock<IAccount>();
        //    prod = _mockery.StrictMock<IProduct>();
            
        //    SetupResult.For(prod.Key).Return(1);
        //    SetupResult.For(acc.Key).Return(1);
        //    SetupResult.For(acc.Product).Return(prod);

        //    SetupResult.For(ml.Key).Return(1);
        //    SetupResult.For(ml.InitialInstallments).Return((short)276);
        //    SetupResult.For(ml.Account).Return(acc);

        //    _mockery.ReplayAll();
        //    rule.ExecuteRule(Messages, "", true, new Object[] { ml });
        //    _mockery.VerifyAll();

        //    Assert.AreEqual(1, Messages.Count);
        //}

        //[NUnit.Framework.Test]
        //public void VariableNoExtendedTermSuperLoExtendedTermPass()
        //{
        //    //ml.Account.Product.Key
        //    SAHL.Rules.Products.VariableNoExtendedTerm rule = new SAHL.Rules.Products.VariableNoExtendedTerm();
        //    SetupGetRule();


        //    ml = _mockery.StrictMock<IMortgageLoan>();
        //    acc = _mockery.StrictMock<IAccount>();
        //    prod = _mockery.StrictMock<IProduct>();

        //    SetupResult.For(prod.Key).Return((int)SAHL.Common.Globals.Products.SuperLo);
        //    SetupResult.For(acc.Key).Return(1);
        //    SetupResult.For(acc.Product).Return(prod);

        //    SetupResult.For(ml.Key).Return(1);
        //    SetupResult.For(ml.InitialInstallments).Return((short)276);
        //    SetupResult.For(ml.Account).Return(acc);

        //    _mockery.ReplayAll();
        //    rule.ExecuteRule(Messages, "", true, new Object[] { ml });
        //    _mockery.VerifyAll();

        //    Assert.AreEqual(0, Messages.Count);
        //}

        //[NUnit.Framework.Test]
        //public void VariableNoExtendedTermVariable20yrTermPass()
        //{
        //    SAHL.Rules.Products.VariableNoExtendedTerm rule = new SAHL.Rules.Products.VariableNoExtendedTerm();

        //    ml = _mockery.StrictMock<IMortgageLoan>();
        //    acc = _mockery.StrictMock<IAccount>();
        //    prod = _mockery.StrictMock<IProduct>();

        //    SetupResult.For(prod.Key).Return((int)SAHL.Common.Globals.Products.VariableLoan);
        //    SetupResult.For(acc.Key).Return(1);
        //    SetupResult.For(acc.Product).Return(prod);

        //    SetupResult.For(ml.Key).Return(1);
        //    SetupResult.For(ml.InitialInstallments).Return((short)240);
        //    SetupResult.For(ml.Account).Return(acc);

        //    _mockery.ReplayAll();
        //    rule.ExecuteRule(Messages, "", true, new Object[] { ml });
        //    _mockery.VerifyAll();

        //    Assert.AreEqual(0, Messages.Count);
        //}

        //[NUnit.Framework.Test]
        //public void VariableNoExtendedTermVariable18yrTermPass()
        //{
        //    SAHL.Rules.Products.VariableNoExtendedTerm rule = new SAHL.Rules.Products.VariableNoExtendedTerm();

        //    ml = _mockery.StrictMock<IMortgageLoan>();
        //    acc = _mockery.StrictMock<IAccount>();
        //    prod = _mockery.StrictMock<IProduct>();

        //    SetupResult.For(prod.Key).Return((int)SAHL.Common.Globals.Products.VariableLoan);
        //    SetupResult.For(acc.Key).Return(1);
        //    SetupResult.For(acc.Product).Return(prod);

        //    SetupResult.For(ml.Key).Return(1);
        //    SetupResult.For(ml.InitialInstallments).Return((short)216);
        //    SetupResult.For(ml.Account).Return(acc);

        //    _mockery.ReplayAll();
        //    rule.ExecuteRule(Messages, "", true, new Object[] { ml });
        //    _mockery.VerifyAll();

        //    Assert.AreEqual(0, Messages.Count);
        //}

        #endregion
    }
}
