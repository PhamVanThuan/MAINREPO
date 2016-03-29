using System;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using NUnit.Framework;

namespace SAHL.Config.Services.Tests
{
    [TestFixture]
    public class TestServiceBootstrapper
    {
        [Test]
        public void Initialize_ShouldNotThrowException()
        {
            //---------------Set up test pack-------------------
            var serviceBootstrapper = new ServiceBootstrapper();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            Assert.DoesNotThrow(() => serviceBootstrapper.Initialise());
            //---------------Test Result -----------------------
        }

        [Test]
        public void Initialize_ShouldReturnIocContainer()
        {
            //---------------Set up test pack-------------------
            var serviceBootstrapper = new ServiceBootstrapper();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var iocContainer = serviceBootstrapper.Initialise();
            //---------------Test Result -----------------------
            Assert.IsNotNull(iocContainer);
        }
    }
}
