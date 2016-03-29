using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using SAHL.Test;
using Rhino.Mocks;
using SAHL.Common.Web.UI.Security;

namespace SAHL.Common.Web.UI.Test.Security
{
    [TestFixture]
    public class SecurityHelperTest : TestBase
    {
        [Test]
        [Ignore("Unable to get this test started with the view - will have to revisit this")]
        public void CheckSecurityTest()
        {
            // first test - pass a view name that doesn't exist where there are features listed that the user does not
            // have - this will pass because the view isn't listed
            IViewBase view = _mockery.StrictMock<IViewBase>();
            SetupResult.For(view.ViewName).Return("hhhhh");
            SetupResult.For(view.CurrentPresenter).Return("yyyyyy");

            bool result = SecurityHelper.CheckSecurity("TestViewNotIncluded", view);
            Assert.IsTrue(result);

        }

    }
}
