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
    public class MinimumAge : RuleBase
    {
        MortgageLoanMinimumAge rule = null;
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
        public void MinimumAgePass()
        {
            rule = new MortgageLoanMinimumAge();

            // mock the IApplication that is passed into the rule.
            IApplication app = _mockery.StrictMock<IApplication>();
            //IAccount account = _mockery.StrictMock<IAccount>();
            //SetupResult.For(app.Account).Return(account);
            ILegalEntityNaturalPerson lenp = _mockery.StrictMock<ILegalEntityNaturalPerson>();
            SetupResult.For(lenp.AgeNextBirthday).Return(20);

            IList<ILegalEntity> lenplist = new List<ILegalEntity>();
            lenplist.Add(lenp);

            IReadOnlyEventList<ILegalEntity> les = new ReadOnlyEventList<ILegalEntity>(lenplist);
            //les.Add(null, lenp);

            SetupResult.For(app.GetLegalEntitiesByRoleType(null)).IgnoreArguments().Return(les);

            // mock the param for the value that would come from the DB
            IRuleParameter param = _mockery.StrictMock<IRuleParameter>();
            SetupResult.For(param.Name).Return("@MinimumAge");
            SetupResult.For(param.Value).Return("18");

            // mock the foreach looking for the correct param
            IEventList<IRuleParameter> parametres = new EventList<IRuleParameter>();
            parametres.Add(null, param);
            // SetupResult.For(ruleItem.RuleParameters).Return(parametres);

            ExecuteRule(rule, 0, app);
        }

        [NUnit.Framework.Test]
        public void MinimumAgeFail()
        {
            rule = new MortgageLoanMinimumAge();

            // mock the IApplication that is passed into the rule.
            IApplication app = _mockery.StrictMock<IApplication>();
            //IAccount account = _mockery.StrictMock<IAccount>();
            //SetupResult.For(app.Account).Return(account);
            ILegalEntityNaturalPerson lenp = _mockery.StrictMock<ILegalEntityNaturalPerson>();
            SetupResult.For(lenp.AgeNextBirthday).Return(16);

            IList<ILegalEntity> lenplist = new List<ILegalEntity>();
            lenplist.Add(lenp);

            IReadOnlyEventList<ILegalEntity> les = new ReadOnlyEventList<ILegalEntity>(lenplist);
            //les.Add(null, lenp);

            SetupResult.For(app.GetLegalEntitiesByRoleType(null)).IgnoreArguments().Return(les);

            // mock the param for the value that would come from the DB
            IRuleParameter param = _mockery.StrictMock<IRuleParameter>();
            SetupResult.For(param.Name).Return("@MinimumAge");
            SetupResult.For(param.Value).Return("18");

            // mock the foreach looking for the correct param
            IEventList<IRuleParameter> parametres = new EventList<IRuleParameter>();
            parametres.Add(null, param);
            // SetupResult.For(ruleItem.RuleParameters).Return(parametres);

            ExecuteRule(rule, 1, app);
        }
    }
}
