using System;
using NSubstitute;
using NUnit.Framework;
using SAHL.Config.Services.Core;

namespace SAHL.Services.WindowsWeb.CommandService.Tests
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
        public void Main_GivenValidConfiguration_ShouldNotThrowException()
        {
            //---------------Set up test pack-------------------
            var serviceBootstrapper = Substitute.For<IServiceBootstrapper>();
            new ProgramStart(serviceBootstrapper);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            Assert.DoesNotThrow(() => ProgramStart.Main(null));
            //---------------Test Result -----------------------
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
    }
}