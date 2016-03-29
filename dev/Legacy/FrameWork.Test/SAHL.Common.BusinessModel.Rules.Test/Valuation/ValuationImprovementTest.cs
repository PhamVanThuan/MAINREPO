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
    public class ValuationImprovementTest : RuleBase
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
        public void ManualValuationImprovementDate_Pass()
        {
            ManualValuationImprovementDate rule = new ManualValuationImprovementDate();
            IValuation valuation = _mockery.StrictMock<IValuation>();
            IValuationImprovement valuationImprovement = _mockery.StrictMock<IValuationImprovement>();
            IValuationDataProviderDataService provider = _mockery.StrictMock<IValuationDataProviderDataService>();

            SetupResult.For(valuationImprovement.Valuation).Return(valuation);
            SetupResult.For(provider.Key).Return((int)SAHL.Common.Globals.ValuationDataProviderDataServices.SAHLManualValuation);
            SetupResult.For(valuation.ValuationDataProviderDataService).Return(provider);

            SetupResult.For(valuationImprovement.ImprovementDate).Return(System.DateTime.Now);

            ExecuteRule(rule, 0, valuationImprovement);
        }

        [NUnit.Framework.Test]
        public void ManualValuationImprovementDate_Fail()
        {
            ManualValuationImprovementDate rule = new ManualValuationImprovementDate();
            IValuation valuation = _mockery.StrictMock<IValuation>();
            IValuationImprovement valuationImprovement = _mockery.StrictMock<IValuationImprovement>();
            IValuationDataProviderDataService provider = _mockery.StrictMock<IValuationDataProviderDataService>();

            SetupResult.For(valuationImprovement.Valuation).Return(valuation);
            SetupResult.For(provider.Key).Return((int)SAHL.Common.Globals.ValuationDataProviderDataServices.SAHLManualValuation);
            SetupResult.For(valuation.ValuationDataProviderDataService).Return(provider);

            SetupResult.For(valuationImprovement.ImprovementDate).Return(null);

            ExecuteRule(rule, 1, valuationImprovement);
        }

        [NUnit.Framework.Test]
        public void ManualValuationImprovementDate_SkipNonManual()
        {
            ManualValuationImprovementDate rule = new ManualValuationImprovementDate();
            IValuation valuation = _mockery.StrictMock<IValuation>();
            IValuationImprovement valuationImprovement = _mockery.StrictMock<IValuationImprovement>();
            IValuationDataProviderDataService provider = _mockery.StrictMock<IValuationDataProviderDataService>();

            SetupResult.For(valuationImprovement.Valuation).Return(valuation);
            SetupResult.For(provider.Key).Return((int)SAHL.Common.Globals.ValuationDataProviderDataServices.LightstoneAutomatedValuation);
            SetupResult.For(valuation.ValuationDataProviderDataService).Return(provider);

            ExecuteRule(rule, 0, valuationImprovement);
        }

        [NUnit.Framework.Test]
        public void ManualValuationImprovementValue_Pass()
        {
            ManualValuationImprovementValue rule = new ManualValuationImprovementValue();
            IValuation valuation = _mockery.StrictMock<IValuation>();
            IValuationImprovement valuationImprovement = _mockery.StrictMock<IValuationImprovement>();
            IValuationDataProviderDataService provider = _mockery.StrictMock<IValuationDataProviderDataService>();

            SetupResult.For(valuationImprovement.Valuation).Return(valuation);
            SetupResult.For(provider.Key).Return((int)SAHL.Common.Globals.ValuationDataProviderDataServices.SAHLManualValuation);
            SetupResult.For(valuation.ValuationDataProviderDataService).Return(provider);

            double value = 100;
            SetupResult.For(valuationImprovement.ImprovementValue).Return(value);

            ExecuteRule(rule, 0, valuationImprovement);
        }

        [NUnit.Framework.Test]
        public void ManualValuationImprovementValue_Fail()
        {
            ManualValuationImprovementValue rule = new ManualValuationImprovementValue();
            IValuation valuation = _mockery.StrictMock<IValuation>();
            IValuationImprovement valuationImprovement = _mockery.StrictMock<IValuationImprovement>();
            IValuationDataProviderDataService provider = _mockery.StrictMock<IValuationDataProviderDataService>();

            SetupResult.For(valuationImprovement.Valuation).Return(valuation);
            SetupResult.For(provider.Key).Return((int)SAHL.Common.Globals.ValuationDataProviderDataServices.SAHLManualValuation);
            SetupResult.For(valuation.ValuationDataProviderDataService).Return(provider);

            double value = 0;
            SetupResult.For(valuationImprovement.ImprovementValue).Return(value);

            ExecuteRule(rule, 1, valuationImprovement);
        }

        [NUnit.Framework.Test]
        public void ManualValuationImprovementValue_SkipNonManual()
        {
            ManualValuationImprovementValue rule = new ManualValuationImprovementValue();
            IValuation valuation = _mockery.StrictMock<IValuation>();
            IValuationImprovement valuationImprovement = _mockery.StrictMock<IValuationImprovement>();
            IValuationDataProviderDataService provider = _mockery.StrictMock<IValuationDataProviderDataService>();

            SetupResult.For(valuationImprovement.Valuation).Return(valuation);
            SetupResult.For(provider.Key).Return((int)SAHL.Common.Globals.ValuationDataProviderDataServices.LightstoneAutomatedValuation);
            SetupResult.For(valuation.ValuationDataProviderDataService).Return(provider);

            ExecuteRule(rule, 0, valuationImprovement);
        }

    }
}
