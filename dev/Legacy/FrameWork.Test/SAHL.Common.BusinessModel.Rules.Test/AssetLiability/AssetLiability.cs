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
using SAHL.Common.Globals;
using SAHL.Common.BusinessModel.Rules.AssetLiability;

namespace SAHL.Common.BusinessModel.Rules.Test.AssetLiability
{
    [TestFixture]
    public class AssetLiability : RuleBase
    {
        IAssetLiabilityLiabilityLoan _assetLiabilityLiabilityLoan;
        IAssetLiabilitySubType _assetLiabilitySubType;

        [NUnit.Framework.SetUp()]
        public override void Setup()
        {
            base.Setup();
        }

        [TearDown]
        public override void TearDown()
        {
            base.TearDown();
        }

        #region AssetLiabilityLoanLiabilityMin

        [Test]
        public void AssetLiabilityLoanLiabilityMinPass()
        {
            AssetLiabilityLoanLiabilityMin rule = new AssetLiabilityLoanLiabilityMin();
            _assetLiabilityLiabilityLoan = _mockery.StrictMock<IAssetLiabilityLiabilityLoan>();
            _assetLiabilitySubType = _mockery.StrictMock<IAssetLiabilitySubType>();
            SetupResult.For(_assetLiabilityLiabilityLoan.LoanType).Return(_assetLiabilitySubType);
            SetupResult.For(_assetLiabilityLiabilityLoan.FinancialInstitution).Return("Test");
            SetupResult.For(_assetLiabilityLiabilityLoan.LiabilityValue).Return(Convert.ToDouble(1000));
            ExecuteRule(rule, 0, _assetLiabilityLiabilityLoan);
        }

        [Test]
        public void AssetLiabilityLoanLiabilityMinFail()
        {
            AssetLiabilityLoanLiabilityMin rule = new AssetLiabilityLoanLiabilityMin();
            _assetLiabilityLiabilityLoan = _mockery.StrictMock<IAssetLiabilityLiabilityLoan>();
            _assetLiabilitySubType = _mockery.StrictMock<IAssetLiabilitySubType>();
            SetupResult.For(_assetLiabilityLiabilityLoan.LoanType).Return(_assetLiabilitySubType);
            SetupResult.For(_assetLiabilityLiabilityLoan.FinancialInstitution).Return("Test");
            SetupResult.For(_assetLiabilityLiabilityLoan.LiabilityValue).Return(Convert.ToDouble(0));
            ExecuteRule(rule, 1, _assetLiabilityLiabilityLoan);
        }

        #endregion

        #region AssetLiabilityCompanyName

        [Test]
        public void AssetLiabilityCompanyNameTestPass()
        {
            AssetLiabilityCompanyName rule = new AssetLiabilityCompanyName();
            IAssetLiabilityLifeAssurance alLifeAssurance = _mockery.StrictMock<IAssetLiabilityLifeAssurance>();
            SetupResult.For(alLifeAssurance.CompanyName).Return("test");
            ExecuteRule(rule, 0, alLifeAssurance);
        }

        [Test]
        public void AssetLiabilityCompanyNameTestFail()
        {
            AssetLiabilityCompanyName rule = new AssetLiabilityCompanyName();
            IAssetLiabilityLifeAssurance alLifeAssurance = _mockery.StrictMock<IAssetLiabilityLifeAssurance>();
            SetupResult.For(alLifeAssurance.CompanyName).Return(string.Empty);
            ExecuteRule(rule, 1, alLifeAssurance);
        }

        #endregion

        #region AssetLiabilityDescription

        [Test]
        public void AssetLiabilityDescriptionTestPass()
        {
            AssetLiabilityDescription rule = new AssetLiabilityDescription();
            IAssetLiabilityLiabilitySurety assetLiabilityLiabilitySurety = _mockery.StrictMock<IAssetLiabilityLiabilitySurety>();
            SetupResult.For(assetLiabilityLiabilitySurety.Description).Return("Description");
            ExecuteRule(rule, 0, assetLiabilityLiabilitySurety);
        }

        [Test]
        public void AssetLiabilityDescriptionTestFail()
        {
            AssetLiabilityDescription rule = new AssetLiabilityDescription();
            IAssetLiabilityLiabilitySurety assetLiabilityLiabilitySurety = _mockery.StrictMock<IAssetLiabilityLiabilitySurety>();
            SetupResult.For(assetLiabilityLiabilitySurety.Description).Return("");
            ExecuteRule(rule, 1, assetLiabilityLiabilitySurety);
        }

        #endregion

        #region AssetLiabilityFinancialInstitution

        [Test]
        public void AssetLiabilityFinancialInstitutionTestPass()
        {
            AssetLiabilityFinancialInstitution rule = new AssetLiabilityFinancialInstitution();
            IAssetLiabilityLiabilityLoan assetLiabilityLiabilityLoan = _mockery.StrictMock<IAssetLiabilityLiabilityLoan>();
            SetupResult.For(assetLiabilityLiabilityLoan.FinancialInstitution).Return("FinancialInstitution");
            ExecuteRule(rule, 0, assetLiabilityLiabilityLoan);
        }

        [Test]
        public void AssetLiabilityFinancialInstitutionTestFail()
        {
            AssetLiabilityFinancialInstitution rule = new AssetLiabilityFinancialInstitution();
            IAssetLiabilityLiabilityLoan assetLiabilityLiabilityLoan = _mockery.StrictMock<IAssetLiabilityLiabilityLoan>();
            SetupResult.For(assetLiabilityLiabilityLoan.FinancialInstitution).Return("");
            ExecuteRule(rule, 1, assetLiabilityLiabilityLoan);
        }


        #endregion
    }
}
