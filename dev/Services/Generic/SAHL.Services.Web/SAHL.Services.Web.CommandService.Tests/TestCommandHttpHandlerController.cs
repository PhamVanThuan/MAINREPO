using System;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using NSubstitute;
using NUnit.Framework;

using SAHL.Core.Logging;
using SAHL.Core.Identity;
using SAHL.Core.Web.Services;
using SAHL.Core.Services.CommandPersistence;
using SAHL.Services.Web.CommandService.Controllers;

namespace SAHL.Services.Web.CommandService.Tests
{
    [TestFixture]
    public class TestCommandHttpHandlerController
    {
        [Test]
        public void Constructor()
        {
            //---------------Set up test pack-------------------
            var logger                = Substitute.For<ILogger>();
            var loggerSource          = Substitute.For<ILoggerSource>();
            var commandSessionFactory = Substitute.For<ICommandSessionFactory>();
            var httpCommandRun        = Substitute.For<IHttpCommandRun>();
            var jsonActivator         = Substitute.For<IJsonActivator>();
            var hostContext           = Substitute.For<IHostContext>();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var controller = new CommandHttpHandlerController(logger, loggerSource, commandSessionFactory, httpCommandRun, jsonActivator, hostContext);
            //---------------Test Result -----------------------
            Assert.IsNotNull(controller);
        }
    }
}
