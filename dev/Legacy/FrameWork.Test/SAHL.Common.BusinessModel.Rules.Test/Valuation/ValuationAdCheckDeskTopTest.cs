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
    public class ValuationAdCheckDeskTopTest : RuleBase
    {
        IValuation val;
        IProperty prop;

        [NUnit.Framework.SetUp()]
        public override void Setup()
        {
            base.Setup();

            val = _mockery.StrictMock<IValuation>();
            prop = _mockery.StrictMock<IProperty>();
        }


        [TearDown]
        public override void TearDown()
        {
            base.TearDown();
        }

        [NUnit.Framework.Test]
        public void AdheckValuationCanBeUpdatedTestFail()
        {
            ValuationAutomatedAdCheckValuation rule = new ValuationAutomatedAdCheckValuation();

            IValuationDiscriminatedAdCheckDesktop val1 = _mockery.StrictMock<IValuationDiscriminatedAdCheckDesktop>();
            SetupResult.For(val1.Property).Return(prop);
            SetupResult.For(prop.CanPerformPropertyAdCheckValuation()).Return(false);

            ExecuteRule(rule, 1, val1);
        }

        [NUnit.Framework.Test]
        public void AdCheckValuationCanBeUpdatedTestPass()
        {
            ValuationAutomatedAdCheckValuation rule = new ValuationAutomatedAdCheckValuation();

            IValuationDiscriminatedAdCheckDesktop val1 = _mockery.StrictMock<IValuationDiscriminatedAdCheckDesktop>();
            SetupResult.For(val1.Property).Return(prop);
            SetupResult.For(prop.CanPerformPropertyAdCheckValuation()).Return(true);

            ExecuteRule(rule, 0, val1);
        }
    }
}
