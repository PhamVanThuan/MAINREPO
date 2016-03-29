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
    public class ValuationOutbuildingTest : RuleBase
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
        public void ManualValuationOutbuildingRoof_Pass()
        {
            ManualValuationOutbuildingRoof rule = new ManualValuationOutbuildingRoof();
            IValuationOutbuilding valuationOutbuilding = _mockery.StrictMock<IValuationOutbuilding>();
            IValuation valuation = _mockery.StrictMock<IValuation>();
            IValuationDataProviderDataService provider = _mockery.StrictMock<IValuationDataProviderDataService>();
            IValuationRoofType valuationRoofType = _mockery.StrictMock<IValuationRoofType>();

            SetupResult.For(valuationOutbuilding.Valuation).Return(valuation);
            SetupResult.For(provider.Key).Return((int)SAHL.Common.Globals.ValuationDataProviderDataServices.SAHLManualValuation);
            SetupResult.For(valuation.ValuationDataProviderDataService).Return(provider);

            SetupResult.For(valuationOutbuilding.ValuationRoofType).Return(valuationRoofType);

            ExecuteRule(rule, 0, valuationOutbuilding);
        }

        [NUnit.Framework.Test]
        public void ManualValuationOutbuildingRoof_Fail()
        {
            ManualValuationOutbuildingRoof rule = new ManualValuationOutbuildingRoof();
            IValuationOutbuilding valuationOutbuilding = _mockery.StrictMock<IValuationOutbuilding>();
            IValuation valuation = _mockery.StrictMock<IValuation>();
            IValuationDataProviderDataService provider = _mockery.StrictMock<IValuationDataProviderDataService>();

            SetupResult.For(valuationOutbuilding.Valuation).Return(valuation);
            SetupResult.For(provider.Key).Return((int)SAHL.Common.Globals.ValuationDataProviderDataServices.SAHLManualValuation);
            SetupResult.For(valuation.ValuationDataProviderDataService).Return(provider);

            SetupResult.For(valuationOutbuilding.ValuationRoofType).Return(null);

            ExecuteRule(rule, 1, valuationOutbuilding);
        }

        [NUnit.Framework.Test]
        public void ManualValuationOutbuildingRoof_SkipNonManual()
        {
            ManualValuationOutbuildingRoof rule = new ManualValuationOutbuildingRoof();
            IValuationOutbuilding valuationOutbuilding = _mockery.StrictMock<IValuationOutbuilding>();
            IValuation valuation = _mockery.StrictMock<IValuation>();
            IValuationDataProviderDataService provider = _mockery.StrictMock<IValuationDataProviderDataService>();

            SetupResult.For(valuationOutbuilding.Valuation).Return(valuation);
            SetupResult.For(valuation.ValuationDataProviderDataService).Return(provider);

            SetupResult.For(provider.Key).Return((int)SAHL.Common.Globals.ValuationDataProviderDataServices.LightstoneAutomatedValuation);

            ExecuteRule(rule, 0, valuationOutbuilding);
        }

        [NUnit.Framework.Test]
        public void ManualValuationOutbuildingExtent_Pass()
        {
            ManualValuationOutbuildingExtent rule = new ManualValuationOutbuildingExtent();
            IValuationOutbuilding valuationOutbuilding = _mockery.StrictMock<IValuationOutbuilding>();
            IValuation valuation = _mockery.StrictMock<IValuation>();
            IValuationDataProviderDataService provider = _mockery.StrictMock<IValuationDataProviderDataService>();

            SetupResult.For(valuationOutbuilding.Valuation).Return(valuation);
            SetupResult.For(provider.Key).Return((int)SAHL.Common.Globals.ValuationDataProviderDataServices.SAHLManualValuation);
            SetupResult.For(valuation.ValuationDataProviderDataService).Return(provider);
            double extent = 100;
            SetupResult.For(valuationOutbuilding.Extent).Return(extent);

            ExecuteRule(rule, 0, valuationOutbuilding);
        }

        [NUnit.Framework.Test]
        public void ManualValuationOutbuildingExtent_Fail()
        {
            ManualValuationOutbuildingExtent rule = new ManualValuationOutbuildingExtent();
            IValuationOutbuilding valuationOutbuilding = _mockery.StrictMock<IValuationOutbuilding>();
            IValuation valuation = _mockery.StrictMock<IValuation>();
            IValuationDataProviderDataService provider = _mockery.StrictMock<IValuationDataProviderDataService>();

            SetupResult.For(valuationOutbuilding.Valuation).Return(valuation);
            SetupResult.For(provider.Key).Return((int)SAHL.Common.Globals.ValuationDataProviderDataServices.SAHLManualValuation);
            SetupResult.For(valuation.ValuationDataProviderDataService).Return(provider);

            double extent = 0;
            SetupResult.For(valuationOutbuilding.Extent).Return(extent);

            ExecuteRule(rule, 1, valuationOutbuilding);
        }

        [NUnit.Framework.Test]
        public void ManualValuationOutbuildingExtent_SkipNonManual()
        {
            ManualValuationOutbuildingExtent rule = new ManualValuationOutbuildingExtent();
            IValuationOutbuilding valuationOutbuilding = _mockery.StrictMock<IValuationOutbuilding>();
            IValuation valuation = _mockery.StrictMock<IValuation>();
            IValuationDataProviderDataService provider = _mockery.StrictMock<IValuationDataProviderDataService>();

            SetupResult.For(valuationOutbuilding.Valuation).Return(valuation);
            SetupResult.For(valuation.ValuationDataProviderDataService).Return(provider);

            SetupResult.For(provider.Key).Return((int)SAHL.Common.Globals.ValuationDataProviderDataServices.LightstoneAutomatedValuation);

            ExecuteRule(rule, 0, valuationOutbuilding);
        }

        [NUnit.Framework.Test]
        public void ManualValuationOutbuildingRate_Pass()
        {
            ManualValuationOutbuildingRate rule = new ManualValuationOutbuildingRate();
            IValuationOutbuilding valuationOutbuilding = _mockery.StrictMock<IValuationOutbuilding>();
            IValuation valuation = _mockery.StrictMock<IValuation>();
            IValuationDataProviderDataService provider = _mockery.StrictMock<IValuationDataProviderDataService>();

            SetupResult.For(valuationOutbuilding.Valuation).Return(valuation);
            SetupResult.For(provider.Key).Return((int)SAHL.Common.Globals.ValuationDataProviderDataServices.SAHLManualValuation);
            SetupResult.For(valuation.ValuationDataProviderDataService).Return(provider);
            double rate = 100;
            SetupResult.For(valuationOutbuilding.Rate).Return(rate);

            ExecuteRule(rule, 0, valuationOutbuilding);
        }

        [NUnit.Framework.Test]
        public void ManualValuationOutbuildingRate_Fail()
        {
            ManualValuationOutbuildingRate rule = new ManualValuationOutbuildingRate();
            IValuationOutbuilding valuationOutbuilding = _mockery.StrictMock<IValuationOutbuilding>();
            IValuation valuation = _mockery.StrictMock<IValuation>();
            IValuationDataProviderDataService provider = _mockery.StrictMock<IValuationDataProviderDataService>();

            SetupResult.For(valuationOutbuilding.Valuation).Return(valuation);
            SetupResult.For(provider.Key).Return((int)SAHL.Common.Globals.ValuationDataProviderDataServices.SAHLManualValuation);
            SetupResult.For(valuation.ValuationDataProviderDataService).Return(provider);

            double rate = 0;
            SetupResult.For(valuationOutbuilding.Rate).Return(rate);

            ExecuteRule(rule, 1, valuationOutbuilding);
        }

        [NUnit.Framework.Test]
        public void ManualValuationOutbuildingRate_SkipNonManual()
        {
            ManualValuationOutbuildingRate rule = new ManualValuationOutbuildingRate();
            IValuationOutbuilding valuationOutbuilding = _mockery.StrictMock<IValuationOutbuilding>();
            IValuation valuation = _mockery.StrictMock<IValuation>();
            IValuationDataProviderDataService provider = _mockery.StrictMock<IValuationDataProviderDataService>();

            SetupResult.For(valuationOutbuilding.Valuation).Return(valuation);
            SetupResult.For(valuation.ValuationDataProviderDataService).Return(provider);

            SetupResult.For(provider.Key).Return((int)SAHL.Common.Globals.ValuationDataProviderDataServices.LightstoneAutomatedValuation);

            ExecuteRule(rule, 0, valuationOutbuilding);
        }
    }
}
