using System;
using System.Collections.Generic;
using System.Text;
using SAHL.Common.BusinessModel.Interfaces;
using NUnit.Framework;
using Rhino.Mocks;
using SAHL.Common.Globals;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Collections;
using SAHL.Common.BusinessModel.Rules.Valuations;

namespace SAHL.Common.BusinessModel.Rules.Test.Valuation
{
    [TestFixture]
    public class ValuationLightStoneTest : RuleBase
    {
        IValuation val;
        IValuationDataProviderDataService vdpds1;
        IDataProviderDataService dpds1;
        IValuationDataProviderDataService vdpds2;
        IDataProviderDataService dpds2;
        IValuationDataProviderDataService vdpds3;
        IDataProviderDataService dpds3;
        IProperty prop;

        [NUnit.Framework.SetUp()]
        public override void Setup()
        {
            base.Setup();
           
            val = _mockery.StrictMock<IValuation>();
            vdpds1 = _mockery.StrictMock<IValuationDataProviderDataService>();
            dpds1 = _mockery.StrictMock<IDataProviderDataService>();
            vdpds2 = _mockery.StrictMock<IValuationDataProviderDataService>();
            dpds2 = _mockery.StrictMock<IDataProviderDataService>();
            vdpds3 = _mockery.StrictMock<IValuationDataProviderDataService>();
            dpds3 = _mockery.StrictMock<IDataProviderDataService>();
            prop = _mockery.StrictMock<IProperty>();
        }


        [TearDown]
        public override void TearDown()
        {
            base.TearDown();
        }

        [NUnit.Framework.Test]
        public void LightStoneValuationCanBeUpdatedTestFail()
        {
            LightStoneAutomatedValuationUpdate rule = new LightStoneAutomatedValuationUpdate();

            IValuationDiscriminatedLightstoneAVM val1 = _mockery.StrictMock<IValuationDiscriminatedLightstoneAVM>();
            SetupResult.For(val1.Property).Return(prop);
            SetupResult.For(prop.CanPerformPropertyLightStoneValuation()).Return(false);

            ExecuteRule(rule, 1, val1);

        }

        [NUnit.Framework.Test]
        public void LightStoneValuationCanBeUpdatedTestPass()
        {
            LightStoneAutomatedValuationUpdate rule = new LightStoneAutomatedValuationUpdate();

            IValuationDiscriminatedLightstoneAVM val1 = _mockery.StrictMock<IValuationDiscriminatedLightstoneAVM>();
            SetupResult.For(val1.Property).Return(prop);
            SetupResult.For(prop.CanPerformPropertyLightStoneValuation()).Return(true);


            ExecuteRule(rule, 0, val1);
        }
    }
}
