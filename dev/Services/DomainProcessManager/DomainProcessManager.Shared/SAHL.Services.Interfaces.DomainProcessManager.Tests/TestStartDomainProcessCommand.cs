using NSubstitute;
using NUnit.Framework;
using SAHL.Core.Data;
using SAHL.Core.Testing;

namespace SAHL.Services.Interfaces.DomainProcessManager.Tests
{
    [TestFixture]
    public class TestStartDomainProcessCommand
    {
        [Test]
        public void Constructor()
        {
            //---------------Set up test pack-------------------
            var dataModel = Substitute.For<IDataModel>();
            var eventToWaitFor = typeof(FakeEvent1).Name;
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var command = new StartDomainProcessCommand(dataModel, eventToWaitFor);
            //---------------Test Result -----------------------
            Assert.IsNotNull(command);
        }

        [TestCase("dataModel")]
        [TestCase("startEventToWaitFor")]
        public void Constructor_GivenNullConstuctorParameter_ShouldThrowExceptionWithParameterName(string parameterName)
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            ConstructorTestUtils.CheckForExceptionWhenParameterIsNull<StartDomainProcessCommand>(parameterName);
            //---------------Test Result -----------------------
        }

        [Test]
        public void Constructor_GivenDataModel_ShouldSetProperty()
        {
            //---------------Set up test pack-------------------
            var dataModel = Substitute.For<IDataModel>();
            var eventToWaitFor = typeof(FakeEvent1).Name;
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var command = new StartDomainProcessCommand(dataModel, eventToWaitFor);
            //---------------Test Result -----------------------
            Assert.AreSame(dataModel, command.DataModel);
        }

        [Test]
        public void Constructor_GivenDomainProcessEvent_ShouldSetProperty()
        {
            //---------------Set up test pack-------------------
            var dataModel = Substitute.For<IDataModel>();
            var eventToWaitFor = typeof(FakeEvent1).Name;
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var command = new StartDomainProcessCommand(dataModel, eventToWaitFor);
            //---------------Test Result -----------------------
            StringAssert.IsMatch(eventToWaitFor, command.StartEventToWaitFor);
        }
    }
}