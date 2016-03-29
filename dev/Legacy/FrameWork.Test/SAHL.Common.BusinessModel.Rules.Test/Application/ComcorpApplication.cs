using NUnit.Framework;
using Rhino.Mocks;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Rules.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Common.BusinessModel.Rules.Test.Application
{
    [TestFixture]
    public class ComcorpApplication : RuleBase
    {
        [SetUp()]
        public void Setup()
        {
            base.Setup();
        }

        [TearDown]
        public void TearDown()
        {
            base.TearDown();
        }

        #region ComcorpApplicationRequired
        [Test]
        public void ComcorpApplicationRequiredPass()
        {
            IApplication app = _mockery.StrictMock<IApplication>();
            SetupResult.For(app.IsComcorp()).Return(true);
            SetupResult.For(app.Key).Return(-1);

            ComcorpApplicationRequired rule = new ComcorpApplicationRequired();

            ExecuteRule(rule, 0, app); // expect no messages back
        }

        [Test]
        public void ComcorpApplicationRequiredFail()
        {
            IApplication app = _mockery.StrictMock<IApplication>();
            SetupResult.For(app.IsComcorp()).Return(false);
            SetupResult.For(app.Key).Return(-1);

            ComcorpApplicationRequired rule = new ComcorpApplicationRequired();

            ExecuteRule(rule, 1, app); // expect 1 message back
        }
        #endregion
    }
}
