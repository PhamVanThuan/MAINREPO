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
    public class ValuationMainBuildingTest : RuleBase
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
        public void ValuationMainBuildingValuationMandatory_Pass()
        {
            ValuationMainBuildingValuationMandatory rule = new ValuationMainBuildingValuationMandatory();
            IValuationMainBuilding valuationMainBuilding = _mockery.StrictMock<IValuationMainBuilding>();
            IValuation valuation = _mockery.StrictMock<IValuation>();

            SetupResult.For(valuationMainBuilding.Valuation).Return(valuation);
            ExecuteRule(rule, 0, valuationMainBuilding);
        }

        [NUnit.Framework.Test]
        public void ValuationMainBuildingValuationMandatory_Fail()
        {
            ValuationMainBuildingValuationMandatory rule = new ValuationMainBuildingValuationMandatory();
            IValuationMainBuilding valuationMainBuilding = _mockery.StrictMock<IValuationMainBuilding>();

            SetupResult.For(valuationMainBuilding.Valuation).Return(null);
            ExecuteRule(rule, 1, valuationMainBuilding);
        }

        [NUnit.Framework.Test]
        public void ManualValuationMainBuildingRoof_Pass()
        {
            ManualValuationMainBuildingRoof rule = new ManualValuationMainBuildingRoof();
            IValuationMainBuilding valuationMainBuilding = _mockery.StrictMock<IValuationMainBuilding>();
            IValuation valuation = _mockery.StrictMock<IValuation>();
            IValuationDataProviderDataService provider = _mockery.StrictMock<IValuationDataProviderDataService>();
            IValuationRoofType valuationRoofType = _mockery.StrictMock<IValuationRoofType>();

            SetupResult.For(valuationMainBuilding.Valuation).Return(valuation);
            SetupResult.For(provider.Key).Return((int)SAHL.Common.Globals.ValuationDataProviderDataServices.SAHLManualValuation);
            SetupResult.For(valuation.ValuationDataProviderDataService).Return(provider);

            SetupResult.For(valuationMainBuilding.ValuationRoofType).Return(valuationRoofType);

            ExecuteRule(rule, 0, valuationMainBuilding);
        }

        [NUnit.Framework.Test]
        public void ManualValuationMainBuildingRoof_Fail()
        {
            ManualValuationMainBuildingRoof rule = new ManualValuationMainBuildingRoof();
            IValuationMainBuilding valuationMainBuilding = _mockery.StrictMock<IValuationMainBuilding>();
            IValuation valuation = _mockery.StrictMock<IValuation>();
            IValuationDataProviderDataService provider = _mockery.StrictMock<IValuationDataProviderDataService>();

            SetupResult.For(valuationMainBuilding.Valuation).Return(valuation);
            SetupResult.For(provider.Key).Return((int)SAHL.Common.Globals.ValuationDataProviderDataServices.SAHLManualValuation);
            SetupResult.For(valuation.ValuationDataProviderDataService).Return(provider);

            SetupResult.For(valuationMainBuilding.ValuationRoofType).Return(null);

            ExecuteRule(rule, 1, valuationMainBuilding);
        }

        [NUnit.Framework.Test]
        public void ManualValuationMainBuildingRoof_SkipNonManual()
        {
            ManualValuationMainBuildingRoof rule = new ManualValuationMainBuildingRoof();
            IValuationMainBuilding valuationMainBuilding = _mockery.StrictMock<IValuationMainBuilding>();
            IValuation valuation = _mockery.StrictMock<IValuation>();
            IValuationDataProviderDataService provider = _mockery.StrictMock<IValuationDataProviderDataService>();

            SetupResult.For(valuationMainBuilding.Valuation).Return(valuation);
            SetupResult.For(valuation.ValuationDataProviderDataService).Return(provider);

            SetupResult.For(provider.Key).Return((int)SAHL.Common.Globals.ValuationDataProviderDataServices.LightstoneAutomatedValuation);

            ExecuteRule(rule, 0, valuationMainBuilding);
        }

        [NUnit.Framework.Test]
        public void ManualValuationMainBuildingExtent_Pass()
        {
            ManualValuationMainBuildingExtent rule = new ManualValuationMainBuildingExtent();
            IValuationMainBuilding valuationMainBuilding = _mockery.StrictMock<IValuationMainBuilding>();
            IValuation valuation = _mockery.StrictMock<IValuation>();
            IValuationDataProviderDataService provider = _mockery.StrictMock<IValuationDataProviderDataService>();

            SetupResult.For(valuationMainBuilding.Valuation).Return(valuation);
            SetupResult.For(provider.Key).Return((int)SAHL.Common.Globals.ValuationDataProviderDataServices.SAHLManualValuation);
            SetupResult.For(valuation.ValuationDataProviderDataService).Return(provider);
            double extent = 100;
            SetupResult.For(valuationMainBuilding.Extent).Return(extent);

            ExecuteRule(rule, 0, valuationMainBuilding);
        }

        [NUnit.Framework.Test]
        public void ManualValuationMainBuildingExtent_Fail()
        {
            ManualValuationMainBuildingExtent rule = new ManualValuationMainBuildingExtent();
            IValuationMainBuilding valuationMainBuilding = _mockery.StrictMock<IValuationMainBuilding>();
            IValuation valuation = _mockery.StrictMock<IValuation>();
            IValuationDataProviderDataService provider = _mockery.StrictMock<IValuationDataProviderDataService>();

            SetupResult.For(valuationMainBuilding.Valuation).Return(valuation);
            SetupResult.For(provider.Key).Return((int)SAHL.Common.Globals.ValuationDataProviderDataServices.SAHLManualValuation);
            SetupResult.For(valuation.ValuationDataProviderDataService).Return(provider);

            double extent = 0;
            SetupResult.For(valuationMainBuilding.Extent).Return(extent);

            ExecuteRule(rule, 1, valuationMainBuilding);
        }

        [NUnit.Framework.Test]
        public void ManualValuationMainBuildingExtent_SkipNonManual()
        {
            ManualValuationMainBuildingExtent rule = new ManualValuationMainBuildingExtent();
            IValuationMainBuilding valuationMainBuilding = _mockery.StrictMock<IValuationMainBuilding>();
            IValuation valuation = _mockery.StrictMock<IValuation>();
            IValuationDataProviderDataService provider = _mockery.StrictMock<IValuationDataProviderDataService>();

            SetupResult.For(valuationMainBuilding.Valuation).Return(valuation);
            SetupResult.For(valuation.ValuationDataProviderDataService).Return(provider);

            SetupResult.For(provider.Key).Return((int)SAHL.Common.Globals.ValuationDataProviderDataServices.LightstoneAutomatedValuation);

            ExecuteRule(rule, 0, valuationMainBuilding);
        }

        [NUnit.Framework.Test]
        public void ManualValuationMainBuildingRate_Pass()
        {
            ManualValuationMainBuildingRate rule = new ManualValuationMainBuildingRate();
            IValuationMainBuilding valuationMainBuilding = _mockery.StrictMock<IValuationMainBuilding>();
            IValuation valuation = _mockery.StrictMock<IValuation>();
            IValuationDataProviderDataService provider = _mockery.StrictMock<IValuationDataProviderDataService>();

            SetupResult.For(valuationMainBuilding.Valuation).Return(valuation);
            SetupResult.For(provider.Key).Return((int)SAHL.Common.Globals.ValuationDataProviderDataServices.SAHLManualValuation);
            SetupResult.For(valuation.ValuationDataProviderDataService).Return(provider);
            double rate = 100;
            SetupResult.For(valuationMainBuilding.Rate).Return(rate);

            ExecuteRule(rule, 0, valuationMainBuilding);
        }

        [NUnit.Framework.Test]
        public void ManualValuationMainBuildingRate_Fail()
        {
            ManualValuationMainBuildingRate rule = new ManualValuationMainBuildingRate();
            IValuationMainBuilding valuationMainBuilding = _mockery.StrictMock<IValuationMainBuilding>();
            IValuation valuation = _mockery.StrictMock<IValuation>();
            IValuationDataProviderDataService provider = _mockery.StrictMock<IValuationDataProviderDataService>();

            SetupResult.For(valuationMainBuilding.Valuation).Return(valuation);
            SetupResult.For(provider.Key).Return((int)SAHL.Common.Globals.ValuationDataProviderDataServices.SAHLManualValuation);
            SetupResult.For(valuation.ValuationDataProviderDataService).Return(provider);

            double rate = 0;
            SetupResult.For(valuationMainBuilding.Rate).Return(rate);

            ExecuteRule(rule, 1, valuationMainBuilding);
        }

        [NUnit.Framework.Test]
        public void ManualValuationMainBuildingRate_SkipNonManual()
        {
            ManualValuationMainBuildingRate rule = new ManualValuationMainBuildingRate();
            IValuationMainBuilding valuationMainBuilding = _mockery.StrictMock<IValuationMainBuilding>();
            IValuation valuation = _mockery.StrictMock<IValuation>();
            IValuationDataProviderDataService provider = _mockery.StrictMock<IValuationDataProviderDataService>();

            SetupResult.For(valuationMainBuilding.Valuation).Return(valuation);
            SetupResult.For(valuation.ValuationDataProviderDataService).Return(provider);

            SetupResult.For(provider.Key).Return((int)SAHL.Common.Globals.ValuationDataProviderDataServices.LightstoneAutomatedValuation);

            ExecuteRule(rule, 0, valuationMainBuilding);
        }
    }
}
