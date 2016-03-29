using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using SAHL.Common.BusinessModel.Rules.Workflow;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using Castle.ActiveRecord;

namespace SAHL.Common.BusinessModel.Rules.Test.Workflow
{
    [TestFixture]
    public class ReadvancePayments : RuleBase
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
        public void Transaction140_Test()
        {
            using (new SessionScope())
            {
                Transaction140 rule = new Transaction140();

                IApplicationRepository appRepo = RepositoryFactory.GetRepository<IApplicationRepository>();
                IApplication app = appRepo.GetApplicationByKey(644018);

                ExecuteRule(rule, 1, app);
            }
        }
    }
}
