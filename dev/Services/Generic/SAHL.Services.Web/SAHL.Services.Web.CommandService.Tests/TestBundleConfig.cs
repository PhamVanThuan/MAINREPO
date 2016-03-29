using System;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Optimization;
using System.Collections.Generic;

using NUnit.Framework;

namespace SAHL.Services.Web.CommandService.Tests
{
    [TestFixture]
    public class TestBundleConfig
    {
        [Test]
        public void Constructor()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var bundleConfig = new BundleConfig();
            //---------------Test Result -----------------------
            Assert.IsNotNull(bundleConfig);
        }

        [Test]
        public void RegisterBundles_ShouldNotThrowException()
        {
            //---------------Set up test pack-------------------
            var bundleCollection = new BundleCollection();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            Assert.DoesNotThrow(() => BundleConfig.RegisterBundles(bundleCollection));
            //---------------Test Result -----------------------
        }
    }
}
