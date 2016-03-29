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
using SAHL.Common.BusinessModel.Rules.MortgageRules;

namespace SAHL.Common.BusinessModel.Rules.Test.MortgageLoan
{
    [TestFixture]
    public class MinimumIncomeTest : RuleBase
    {
        MortgageLoanMinimumIncome rule = null;
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
        public void MinimumIncomePass()
        {
            rule = new MortgageLoanMinimumIncome();
            
            // mock the IApplication that is passed into the rule.
            IApplication app = _mockery.StrictMock<IApplication>();            
            // According to the Rule's RuleDBTag: "Miniumum income must be 6000 per application"
            SetupResult.For(app.GetHouseHoldIncome()).IgnoreArguments().Return(Convert.ToDouble(6001)); 

            ExecuteRule(rule, 0, app);
        }

        [NUnit.Framework.Test]
        public void MinimumIncomeFail()
        {
            rule = new MortgageLoanMinimumIncome();

            // mock the IApplication that is passed into the rule.
            IApplication app = _mockery.StrictMock<IApplication>();
            SetupResult.For(app.GetHouseHoldIncome()).IgnoreArguments().Return(Convert.ToDouble(1000));

            // mock the param for the value that would come from the DB
            IRuleParameter param = _mockery.StrictMock<IRuleParameter>();
            SetupResult.For(param.Name).Return("@MinimumIncome");
            SetupResult.For(param.Value).Return("2000");

            // mock the foreach looking for the correct param
            IEventList<IRuleParameter> parametres = new EventList<IRuleParameter>();
            parametres.Add(null, param);
            //SetupResult.For(ruleItem.RuleParameters).Return(parametres);

            ExecuteRule(rule, 1, app);
        }
    }
}
