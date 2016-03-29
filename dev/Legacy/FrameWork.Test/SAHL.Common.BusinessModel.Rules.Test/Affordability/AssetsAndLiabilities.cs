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
using SAHL.Common.BusinessModel.Rules.Affordability;

namespace SAHL.Common.BusinessModel.Rules.Test.Affordability
{
    [TestFixture]
    public class AssetsAndLiabilities : RuleBase
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
        public void AssetValueMinimumValuePropertyZeroAmountTestFail()
        {
            AssetValueMinimumValue rule = new AssetValueMinimumValue();

            IAssetLiabilityFixedProperty assetFixed = _mockery.StrictMock<IAssetLiabilityFixedProperty>();

            DateTime dte = DateTime.Now.AddDays(-100);
            SetupResult.For(assetFixed.DateAcquired).Return(dte);

            double amt = 0;
            SetupResult.For(assetFixed.AssetValue).Return(amt);
            ExecuteRule(rule, 1, assetFixed);
        }

        [NUnit.Framework.Test]
        public void AssetValueMinimumValuePropertyNonZeroAmountTestPass()
        {
            AssetValueMinimumValue rule = new AssetValueMinimumValue();

            IAssetLiabilityFixedProperty assetFixed = _mockery.StrictMock<IAssetLiabilityFixedProperty>();

            DateTime dte = DateTime.Now.AddDays(-100);
            SetupResult.For(assetFixed.DateAcquired).Return(dte);

            double amt = 1110;
            SetupResult.For(assetFixed.AssetValue).Return(amt);
            ExecuteRule(rule, 0, assetFixed);
        }

        [NUnit.Framework.Test]
        public void AssetValueDateAcquiredInTheFutureTestFail()
        {
            AssetLiabilityDateAcquiredMax rule = new AssetLiabilityDateAcquiredMax();

            IAssetLiabilityFixedProperty assetFixed = _mockery.StrictMock<IAssetLiabilityFixedProperty>();

            DateTime dte = DateTime.Now.AddDays(1);
            SetupResult.For(assetFixed.DateAcquired).Return(dte);

            double amt = 1110;
            SetupResult.For(assetFixed.AssetValue).Return(amt);
            ExecuteRule(rule, 1, assetFixed);
        }

        [NUnit.Framework.Test]
        public void AssetValueMinimumValuePrivateZeroAmountTestFail()
        {
            AssetValueMinimumValue rule = new AssetValueMinimumValue();

            IAssetLiabilityInvestmentPrivate assetPrivate = _mockery.StrictMock<IAssetLiabilityInvestmentPrivate>();

            double amt = 0;
            SetupResult.For(assetPrivate.AssetValue).Return(amt);
            ExecuteRule(rule, 1, assetPrivate);
        }

        [NUnit.Framework.Test]
        public void AssetValueMinimumValuePrivateNonZeroAmountTestPass()
        {
            AssetValueMinimumValue rule = new AssetValueMinimumValue();

            IAssetLiabilityInvestmentPrivate assetPrivate = _mockery.StrictMock<IAssetLiabilityInvestmentPrivate>();

            double amt = 130;
            SetupResult.For(assetPrivate.AssetValue).Return(amt);
            ExecuteRule(rule, 0, assetPrivate);
        }

        [NUnit.Framework.Test]
        public void AssetValueMinimumValueAssuranceZeroAmountTestFail()
        {
            AssetLiabilitySurrenderValueMin rule = new AssetLiabilitySurrenderValueMin();

            IAssetLiabilityLifeAssurance assetAssurance = _mockery.StrictMock<IAssetLiabilityLifeAssurance>();

            double amt = 0;
            SetupResult.For(assetAssurance.SurrenderValue).Return(amt);
            ExecuteRule(rule, 1, assetAssurance);
        }

        [NUnit.Framework.Test]
        public void AssetValueMinimumValueAssuranceNonZeroAmountTestPass()
        {
            AssetLiabilitySurrenderValueMin rule = new AssetLiabilitySurrenderValueMin();

            IAssetLiabilityLifeAssurance assetAssurance = _mockery.StrictMock<IAssetLiabilityLifeAssurance>();

            double amt = 1250;
            SetupResult.For(assetAssurance.SurrenderValue).Return(amt);
            ExecuteRule(rule, 0, assetAssurance);
        }
    }
}
