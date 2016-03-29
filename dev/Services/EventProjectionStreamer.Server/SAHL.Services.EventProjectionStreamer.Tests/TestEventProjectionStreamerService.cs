using NSubstitute;
using NUnit.Framework;

using SAHL.Core.Services;
using SAHL.Core.Testing;
using SAHL.Services.Interfaces.EventProjectionStreamer;

namespace SAHL.Services.EventProjectionStreamer.Tests
{
    [TestFixture]
    public class TestEventProjectionStreamerService
    {
        [Test]
        public void Constructor()
        {
            //---------------Set up test pack-------------------
            var serviceCommandRouter = Substitute.For<IServiceCommandRouter>();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var service = new EventProjectionStreamerService(serviceCommandRouter);
            //---------------Test Result -----------------------
            Assert.IsNotNull(service);
        }

        [TestCase("serviceCommandRouter")]
        public void Constructor_GivenNullConstuctorParameter_ShouldThrowExceptionWithParameterName(string parameterName)
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            ConstructorTestUtils.CheckForExceptionWhenParameterIsNull<EventProjectionStreamerService>(parameterName);
            //---------------Test Result -----------------------
        }

        [Test]
        public void PerformCommand_ShouldCallPerformCommandOnServiceCommandRouter()
        {
            //---------------Set up test pack-------------------
            var serviceCommandRouter = Substitute.For<IServiceCommandRouter>();
            var service = new EventProjectionStreamerService(serviceCommandRouter);
            var projectionUpdatedCommand = new ProjectionUpdatedCommand("Test", "Some data");
            var metadata = new ServiceRequestMetadata();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            service.PerformCommand(projectionUpdatedCommand, metadata);
            //---------------Test Result -----------------------
            serviceCommandRouter.Received(1).HandleCommand(projectionUpdatedCommand, metadata);
        }
    }
}