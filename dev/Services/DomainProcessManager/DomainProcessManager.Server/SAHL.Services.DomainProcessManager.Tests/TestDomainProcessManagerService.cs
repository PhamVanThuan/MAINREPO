using System;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using NSubstitute;
using NUnit.Framework;

using SAHL.Core;
using SAHL.Services.DomainProcessManager.WcfService;

namespace SAHL.Services.DomainProcessManager.Tests
{
    [TestFixture]
    public class TestDomainProcessManagerService
    {
        [Test]
        public void Constructor()
        {
            //---------------Set up test pack-------------------
            var iocContainer = Substitute.For<IIocContainer>();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var service = new DomainProcessManagerService(iocContainer);
            //---------------Test Result -----------------------
            Assert.IsNotNull(service);
        }
    }
}
