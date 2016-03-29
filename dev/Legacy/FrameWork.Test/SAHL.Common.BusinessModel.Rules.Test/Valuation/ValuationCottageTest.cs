using System;
using System.Collections.Generic;
using System.Text;
using SAHL.Common.BusinessModel.Interfaces;
using NUnit.Framework;
using Rhino.Mocks;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Collections;
using SAHL.Common.Globals;
using SAHL.Common.BusinessModel.Rules.Valuations;

namespace SAHL.Common.BusinessModel.Rules.Test.Valuation
{
    [TestFixture]
    public class ValuationCottageTest : RuleBase
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
        public void ValuationCottageValuationMandatory_Pass()
        {
            ValuationCottageValuationMandatory rule = new ValuationCottageValuationMandatory();
            IValuationCottage valuationCottage = _mockery.StrictMock<IValuationCottage>();
            IValuation valuation = _mockery.StrictMock<IValuation>();

            SetupResult.For(valuationCottage.Valuation).Return(valuation);
            ExecuteRule(rule, 0, valuationCottage);
        }

        [NUnit.Framework.Test]
        public void ValuationCottageValuationMandatory_Fail()
        {
            ValuationCottageValuationMandatory rule = new ValuationCottageValuationMandatory();
            IValuationCottage valuationCottage = _mockery.StrictMock<IValuationCottage>();

            SetupResult.For(valuationCottage.Valuation).Return(null);
            ExecuteRule(rule, 1, valuationCottage);
        }

        [NUnit.Framework.Test]
        public void ManualValuationCottageRoof_Pass()
        {
            ManualValuationCottageRoof rule = new ManualValuationCottageRoof();
            IValuationCottage valuationCottage = _mockery.StrictMock<IValuationCottage>();
            IValuation valuation = _mockery.StrictMock<IValuation>();
            IValuationDataProviderDataService provider = _mockery.StrictMock<IValuationDataProviderDataService>();
            IValuationRoofType valuationRoofType = _mockery.StrictMock<IValuationRoofType>();

            SetupResult.For(valuationCottage.Valuation).Return(valuation);
            SetupResult.For(provider.Key).Return((int)SAHL.Common.Globals.ValuationDataProviderDataServices.SAHLManualValuation);
            SetupResult.For(valuation.ValuationDataProviderDataService).Return(provider);

            SetupResult.For(valuationCottage.ValuationRoofType).Return(valuationRoofType);

            ExecuteRule(rule, 0, valuationCottage);
        }

        [NUnit.Framework.Test]
        public void ManualValuationCottageRoof_Fail()
        {
            ManualValuationCottageRoof rule = new ManualValuationCottageRoof();
            IValuationCottage valuationCottage = _mockery.StrictMock<IValuationCottage>();
            IValuation valuation = _mockery.StrictMock<IValuation>();
            IValuationDataProviderDataService provider = _mockery.StrictMock<IValuationDataProviderDataService>();

            SetupResult.For(valuationCottage.Valuation).Return(valuation);
            SetupResult.For(provider.Key).Return((int)SAHL.Common.Globals.ValuationDataProviderDataServices.SAHLManualValuation);
            SetupResult.For(valuation.ValuationDataProviderDataService).Return(provider);

            SetupResult.For(valuationCottage.ValuationRoofType).Return(null);

            ExecuteRule(rule, 1, valuationCottage);
        }

        [NUnit.Framework.Test]
        public void ManualValuationCottageRoof_SkipNonManual()
        {
            ManualValuationCottageRoof rule = new ManualValuationCottageRoof();
            IValuationCottage valuationCottage = _mockery.StrictMock<IValuationCottage>();
            IValuation valuation = _mockery.StrictMock<IValuation>();
            IValuationDataProviderDataService provider = _mockery.StrictMock<IValuationDataProviderDataService>();

            SetupResult.For(valuationCottage.Valuation).Return(valuation);
            SetupResult.For(valuation.ValuationDataProviderDataService).Return(provider);

            SetupResult.For(provider.Key).Return((int)SAHL.Common.Globals.ValuationDataProviderDataServices.LightstoneAutomatedValuation);

            ExecuteRule(rule, 0, valuationCottage);
        }

        [NUnit.Framework.Test]
        public void ManualValuationCottageExtent_Pass()
        {
            ManualValuationCottageExtent rule = new ManualValuationCottageExtent();
            IValuationCottage valuationCottage = _mockery.StrictMock<IValuationCottage>();
            IValuation valuation = _mockery.StrictMock<IValuation>();
            IValuationDataProviderDataService provider = _mockery.StrictMock<IValuationDataProviderDataService>();

            SetupResult.For(valuationCottage.Valuation).Return(valuation);
            SetupResult.For(provider.Key).Return((int)SAHL.Common.Globals.ValuationDataProviderDataServices.SAHLManualValuation);
            SetupResult.For(valuation.ValuationDataProviderDataService).Return(provider);
            double extent = 100;
            SetupResult.For(valuationCottage.Extent).Return(extent);

            ExecuteRule(rule, 0, valuationCottage);
        }

        [NUnit.Framework.Test]
        public void ManualValuationCottageExtent_Fail()
        {
            ManualValuationCottageExtent rule = new ManualValuationCottageExtent();
            IValuationCottage valuationCottage = _mockery.StrictMock<IValuationCottage>();
            IValuation valuation = _mockery.StrictMock<IValuation>();
            IValuationDataProviderDataService provider = _mockery.StrictMock<IValuationDataProviderDataService>();

            SetupResult.For(valuationCottage.Valuation).Return(valuation);
            SetupResult.For(provider.Key).Return((int)SAHL.Common.Globals.ValuationDataProviderDataServices.SAHLManualValuation);
            SetupResult.For(valuation.ValuationDataProviderDataService).Return(provider);

            double extent = 0;
            SetupResult.For(valuationCottage.Extent).Return(extent);

            ExecuteRule(rule, 1, valuationCottage);
        }

        [NUnit.Framework.Test]
        public void ManualValuationCottageExtent_SkipNonManual()
        {
            ManualValuationCottageExtent rule = new ManualValuationCottageExtent();
            IValuationCottage valuationCottage = _mockery.StrictMock<IValuationCottage>();
            IValuation valuation = _mockery.StrictMock<IValuation>();
            IValuationDataProviderDataService provider = _mockery.StrictMock<IValuationDataProviderDataService>();

            SetupResult.For(valuationCottage.Valuation).Return(valuation);
            SetupResult.For(valuation.ValuationDataProviderDataService).Return(provider);

            SetupResult.For(provider.Key).Return((int)SAHL.Common.Globals.ValuationDataProviderDataServices.LightstoneAutomatedValuation);

            ExecuteRule(rule, 0, valuationCottage);
        }

        [NUnit.Framework.Test]
        public void ManualValuationCottageRate_Pass()
        {
            ManualValuationCottageRate rule = new ManualValuationCottageRate();
            IValuationCottage valuationCottage = _mockery.StrictMock<IValuationCottage>();
            IValuation valuation = _mockery.StrictMock<IValuation>();
            IValuationDataProviderDataService provider = _mockery.StrictMock<IValuationDataProviderDataService>();

            SetupResult.For(valuationCottage.Valuation).Return(valuation);
            SetupResult.For(provider.Key).Return((int)SAHL.Common.Globals.ValuationDataProviderDataServices.SAHLManualValuation);
            SetupResult.For(valuation.ValuationDataProviderDataService).Return(provider);
            double rate = 100;
            SetupResult.For(valuationCottage.Rate).Return(rate);

            ExecuteRule(rule, 0, valuationCottage);
        }

        [NUnit.Framework.Test]
        public void ManualValuationCottageRate_Fail()
        {
            ManualValuationCottageRate rule = new ManualValuationCottageRate();
            IValuationCottage valuationCottage = _mockery.StrictMock<IValuationCottage>();
            IValuation valuation = _mockery.StrictMock<IValuation>();
            IValuationDataProviderDataService provider = _mockery.StrictMock<IValuationDataProviderDataService>();

            SetupResult.For(valuationCottage.Valuation).Return(valuation);
            SetupResult.For(provider.Key).Return((int)SAHL.Common.Globals.ValuationDataProviderDataServices.SAHLManualValuation);
            SetupResult.For(valuation.ValuationDataProviderDataService).Return(provider);

            double rate = 0;
            SetupResult.For(valuationCottage.Rate).Return(rate);

            ExecuteRule(rule, 1, valuationCottage);
        }

        [NUnit.Framework.Test]
        public void ManualValuationCottageRate_SkipNonManual()
        {
            ManualValuationCottageRate rule = new ManualValuationCottageRate();
            IValuationCottage valuationCottage = _mockery.StrictMock<IValuationCottage>();
            IValuation valuation = _mockery.StrictMock<IValuation>();
            IValuationDataProviderDataService provider = _mockery.StrictMock<IValuationDataProviderDataService>();

            SetupResult.For(valuationCottage.Valuation).Return(valuation);
            SetupResult.For(valuation.ValuationDataProviderDataService).Return(provider);

            SetupResult.For(provider.Key).Return((int)SAHL.Common.Globals.ValuationDataProviderDataServices.LightstoneAutomatedValuation);

            ExecuteRule(rule, 0, valuationCottage);
        }
    }
}
