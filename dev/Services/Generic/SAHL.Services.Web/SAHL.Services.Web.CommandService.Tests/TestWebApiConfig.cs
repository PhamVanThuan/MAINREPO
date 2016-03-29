using System;
using System.Text;
using System.Linq;
using System.Web.Http;
using System.Threading.Tasks;
using System.Collections.Generic;

using NUnit.Framework;
using SAHL.Config.Services;
using System.Collections.Specialized;

namespace SAHL.Services.Web.CommandService.Tests
{
    [TestFixture]
    public class TestWebApiConfig
    {
        [Test]
        public void Registerr_ShouldNotThrowException()
        {
            //---------------Set up test pack-------------------
            var httpConfiguration = new HttpConfiguration();

            var serviceCORSSetings = new FakeServiceCORSSettings();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            Assert.DoesNotThrow(() => WebApiConfig.Register(httpConfiguration, serviceCORSSetings));
            //---------------Test Result -----------------------
        }
    }
}
