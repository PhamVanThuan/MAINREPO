using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

using NSubstitute;
using NUnit.Framework;

using SAHL.Core.Logging;
using SAHL.Core.Services;
using SAHL.Core.Web.Services;
using SAHL.Core.Services.Metrics;
using SAHL.Services.Web.CommandService.Controllers;

namespace SAHL.Services.Web.CommandService.Tests
{
    [TestFixture]
    public class TestQueryHttpHandlerController
    {
        [Test]
        public void Constructor()
        {
            //---------------Set up test pack-------------------
            var serviceQueryRouter           = Substitute.For<IServiceQueryRouter>();
            var commandServiceRequestMetrics = Substitute.For<ICommandServiceRequestMetrics>();
            var httpCommandAuthoriser        = Substitute.For<IHttpCommandAuthoriser>();
            var jsonActivator                = Substitute.For<IJsonActivator>();
            var queryParameterManager        = Substitute.For<IQueryParameterManager>();
            var logger                       = Substitute.For<ILogger>();
            var loggerSource                 = Substitute.For<ILoggerSource>();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var controller = new QueryHttpHandlerController(serviceQueryRouter, commandServiceRequestMetrics, httpCommandAuthoriser,
                                                            jsonActivator, queryParameterManager, logger, loggerSource);
            //---------------Test Result -----------------------
            Assert.IsNotNull(controller);
        }
    }
}
