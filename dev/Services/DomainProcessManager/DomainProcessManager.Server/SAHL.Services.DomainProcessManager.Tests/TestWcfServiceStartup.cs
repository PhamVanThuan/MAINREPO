using System;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using NSubstitute;
using NUnit.Framework;

using SAHL.Core;
using SAHL.Core.Logging;
using SAHL.Services.DomainProcessManager.WcfService;
using SAHL.Core.Testing;
using System.ServiceModel;

namespace SAHL.Services.DomainProcessManager.Tests
{
    [TestFixture]
    public class TestWcfServiceStartup
    {
        [Test]
        public void Constructor()
        {
            //---------------Set up test pack-------------------
            var iocContainer    = Substitute.For<IIocContainer>();
            var rawLogger       = Substitute.For<IRawLogger>();
            var loggerSource    = Substitute.For<ILoggerSource>();
            var loggerAppSource = Substitute.For<ILoggerAppSource>();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var startup = new WcfServiceStartup(iocContainer, rawLogger, loggerSource, loggerAppSource);
            //---------------Test Result -----------------------
            Assert.IsNotNull(startup);
        }

        [TestCase("iocContainer")]
        [TestCase("rawLogger")]
        [TestCase("loggerSource")]
        [TestCase("loggerAppSource")]
        public void Constructor_GivenNullConstuctorParameter_ShouldThrowExceptionWithParameterName(string parameterName)
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            ConstructorTestUtils.CheckForExceptionWhenParameterIsNull<WcfServiceStartup>(parameterName);
            //---------------Test Result -----------------------
        }

        [Test]
        public void Start_ShouldNotThrowException()
        {
            //---------------Set up test pack-------------------
            var iocContainer = Substitute.For<IIocContainer>();
            iocContainer.GetInstance(Arg.Any<Type>()).Returns(new DomainProcessManagerService(iocContainer));

            var wcfServiceStartup = this.CreateWcfServiceStartup(iocContainer);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            Assert.DoesNotThrow(wcfServiceStartup.Start);
            //---------------Test Result -----------------------
        }

        private WcfServiceStartup CreateWcfServiceStartup(IIocContainer iocContainer = null,
                                                          IRawLogger rawLogger= null, 
                                                          ILoggerSource loggerSource = null, 
                                                          ILoggerAppSource loggerAppSource = null)
        {
            var wcfServiceStartup = new FakeWcfServiceStartup(iocContainer ?? Substitute.For<IIocContainer>(),
                rawLogger ?? Substitute.For<IRawLogger>(),
                loggerSource ?? Substitute.For<ILoggerSource>(),
                loggerAppSource ?? Substitute.For<ILoggerAppSource>());

            return wcfServiceStartup;
        }

        private class FakeWcfServiceStartup : WcfServiceStartup
        {
            public FakeWcfServiceStartup(IIocContainer iocContainer, IRawLogger rawLogger, ILoggerSource loggerSource, ILoggerAppSource loggerAppSource) 
                : base(iocContainer, rawLogger, loggerSource, loggerAppSource)
            {
            }

            protected override ServiceHost StartWcfService(object wcfService, Uri wcfServiceAddress)
            {
                return new ServiceHost(wcfService, wcfServiceAddress);
            }
        }
    }
}
