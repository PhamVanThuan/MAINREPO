using System;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using NUnit.Framework;

namespace SAHL.Websites.Halo.Shared.Tests
{
    [TestFixture]
    public class TestUserDetailsStartupAction
    {
        [Test]
        public void Constructor()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var startupAction = new UserDetailsStartupAction();
            //---------------Test Result -----------------------
            Assert.IsNotNull(startupAction);
        }
    }
}
