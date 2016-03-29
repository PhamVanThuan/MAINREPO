using System;
using System.Collections.Specialized;
using NSubstitute;
using NUnit.Framework;
using SAHL.Config.Services.Core;
using SAHL.Config.Services.Windows;
using SAHL.Core;

namespace SAHL.Services.Windows.CommandService.Tests
{
    [TestFixture]
    public class TestProgramStart
    {
        [Test]
        public void Constructor()
        {
            //---------------Set up test pack-------------------
            var serviceBootstrapper = Substitute.For<IServiceBootstrapper>();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var programStart = new ProgramStart(serviceBootstrapper);
            //---------------Test Result -----------------------
            Assert.IsNotNull(programStart);
        }

        [Test]
        public void Constructor_GivenNullWindowsServiceBootstrapper_ShouldThrowException()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var exception = Assert.Throws<ArgumentNullException>(() => new ProgramStart(null));
            //---------------Test Result -----------------------
            StringAssert.AreEqualIgnoringCase("serviceBootstrapper", exception.ParamName);
        }

        [Test]
        public void Main_ShouldNotThrowException()
        {
            //---------------Set up test pack-------------------
            var serviceBootstrapper = this.CreateServiceBootstrapper();
            new ProgramStart(serviceBootstrapper);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            Assert.DoesNotThrow(() => ProgramStart.Main(null));
            //---------------Test Result -----------------------
        }

        [Test]
        public void Main_GivenEnableUnhandledExceptionIsTrue_AndExceptionOccurs_ShouldWriteEventToEventLog()
        {
            //---------------Set up test pack-------------------
            var windowsServiceSettings = this.CreateWindowsServiceSettings(enableUnhandledException: true);
            var serviceManager = Substitute.For<IServiceManager>();
            serviceManager.When(manager => manager.StartService()).Do(info =>
                {
                    throw new Exception("Test Exception");
                });
            var iocContainer = CreateIocContainer(serviceManager, windowsServiceSettings);

            var serviceBootstrapper = this.CreateServiceBootstrapper(iocContainer);
            new ProgramStart(serviceBootstrapper);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            ProgramStart.Main(null);
            //---------------Test Result -----------------------
        }

        [Test]
        public void Main_GivenEnableFirstChanceExceptionIsTrue_AndExceptionOccurs_ShouldWriteEventToEventLog()
        {
            //---------------Set up test pack-------------------
            var windowsServiceSettings = this.CreateWindowsServiceSettings(enableFirstChanceException: true);
            var serviceManager = Substitute.For<IServiceManager>();
            serviceManager.When(manager => manager.StartService()).Do(info =>
                {
                    throw new Exception("Test Exception");
                });
            var iocContainer = CreateIocContainer(serviceManager, windowsServiceSettings);

            var serviceBootstrapper = this.CreateServiceBootstrapper(iocContainer);
            new ProgramStart(serviceBootstrapper);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            ProgramStart.Main(null);
            //---------------Test Result -----------------------
        }

        [Test]
        public void Main_GivenNoServiceManager_ShouldThrowException()
        {
            //---------------Set up test pack-------------------
            var iocContainer = Substitute.For<IIocContainer>();
            iocContainer.GetInstance<IServiceManager>().Returns(info => null);

            var serviceBootstrapper = this.CreateServiceBootstrapper(iocContainer);
            new ProgramStart(serviceBootstrapper);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var exception = Assert.Throws<NullReferenceException>(() => ProgramStart.Main(null));
            //---------------Test Result -----------------------
            StringAssert.Contains("Windows Service Manager not found", exception.Message);
        }

        [Test]
        public void Main_GivenNoServiceSettings_ShouldThrowException()
        {
            //---------------Set up test pack-------------------
            var iocContainer = Substitute.For<IIocContainer>();
            iocContainer.GetInstance<IWindowsServiceSettings>().Returns(info => null);

            var serviceBootstrapper = this.CreateServiceBootstrapper(iocContainer);
            new ProgramStart(serviceBootstrapper);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var exception = Assert.Throws<NullReferenceException>(() => ProgramStart.Main(null));
            //---------------Test Result -----------------------
            StringAssert.Contains("Windows Service Settings not found", exception.Message);
        }

        [Test]
        public void Main_ShouldCallInitializeOnBootstrapper()
        {
            //---------------Set up test pack-------------------
            var serviceBootstrapper = Substitute.For<IServiceBootstrapper>();
            new ProgramStart(serviceBootstrapper);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            ProgramStart.Main(null);
            //---------------Test Result -----------------------
            serviceBootstrapper.Received(1).Initialise();
        }

        private IWindowsServiceSettings CreateWindowsServiceSettings(bool enableUnhandledException = false,
                                                                     bool enableFirstChanceException = false)
        {
            var valueCollection = new NameValueCollection
                {
                    { "EnableUnhandledException", enableUnhandledException.ToString() },
                    { "EnableFirstChanceException", enableFirstChanceException.ToString() }
                };

            var windowsServiceSettings = new WindowsServiceSettings(valueCollection);
            return windowsServiceSettings;
        }

        private IIocContainer CreateIocContainer(IServiceManager serviceManager = null,
                                                 IWindowsServiceSettings windowsServiceSettings = null)
        {
            var iocContainer = Substitute.For<IIocContainer>();
            iocContainer.GetInstance<IServiceManager>().Returns(serviceManager ?? Substitute.For<IServiceManager>());
            iocContainer.GetInstance<IWindowsServiceSettings>().Returns(windowsServiceSettings ?? Substitute.For<IWindowsServiceSettings>());

            return iocContainer;
        }

        private IServiceBootstrapper CreateServiceBootstrapper(IIocContainer iocContainer = null)
        {
            var serviceBootstrapper = Substitute.For<IServiceBootstrapper>();
            serviceBootstrapper.Initialise().Returns(iocContainer ?? Substitute.For<IIocContainer>());

            return serviceBootstrapper;
        }
    }
}