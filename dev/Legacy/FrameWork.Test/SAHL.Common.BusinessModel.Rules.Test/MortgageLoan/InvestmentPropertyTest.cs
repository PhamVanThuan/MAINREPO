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

namespace SAHL.Common.BusinessModel.Rules.Test.MortgageLoan
{
    [TestFixture]
    public class InvestmentPropertyTest : RuleBase
    {
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

        [NUnit.Framework.Test]
        public void InvestmentPropertyTestLTVPass()
        {
        }

        [NUnit.Framework.Test]
        public void InvestmentPropertyTestLTVFail()
        {
        }
    }
}
