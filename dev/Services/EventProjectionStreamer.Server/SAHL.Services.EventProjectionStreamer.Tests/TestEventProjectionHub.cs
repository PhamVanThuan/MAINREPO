using System;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using NSubstitute;
using NUnit.Framework;

namespace SAHL.Services.EventProjectionStreamer.Tests
{
    [TestFixture]
    public class TestEventProjectionHub
    {
        public interface IMockClient : IHub
        {
            void updateProjection(string projectionName, dynamic projectionData);
        }

        [Test]
        public void Constructor()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute rTest ----------------------
            var eventProjectionHub = new EventProjectionHub();
            //---------------Test Result -----------------------
            Assert.IsNotNull(eventProjectionHub);
        }

        [Test]
        public void SubscribeToProjection_GivenNoProjectionName_ShouldThrowException()
        {
            //---------------Set up test pack-------------------
            var eventProjectionHub = this.CreateEventProjectionHub();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var exception = Assert.Throws<ArgumentNullException>(() => eventProjectionHub.SubscribeToProjection(string.Empty));
            //---------------Test Result -----------------------
            Assert.AreEqual("projectionName", exception.ParamName);
        }

        [Test]
        public void SubscribeToProjection_GivenProjectionName_ShouldAddConnectionToGroup()
        {
            //---------------Set up test pack-------------------
            var projectionName = "Test";
            var groupManager = this.CreateGroupManager();
            var eventProjectionHub = this.CreateEventProjectionHub(groupManager: groupManager);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            eventProjectionHub.SubscribeToProjection(projectionName);
            //---------------Test Result -----------------------
            groupManager.Received(1).Add(Arg.Any<string>(), projectionName);
        }

        [Test]
        public void UnsubscribeFromProjection_GivenNoProjectionName_ShouldThrowException()
        {
            //---------------Set up test pack-------------------
            var eventProjectionHub = this.CreateEventProjectionHub();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var exception = Assert.Throws<ArgumentNullException>(() => eventProjectionHub.UnsubscribeFromProjection(string.Empty));
            //---------------Test Result -----------------------
            Assert.AreEqual("projectionName", exception.ParamName);
        }

        [Test]
        public void UnsubscribeFromProjection_GivenProjectionName_ShouldRemoveConnectionFromGroup()
        {
            //---------------Set up test pack-------------------
            var projectionName = "Test";
            var groupManager = this.CreateGroupManager();
            var eventProjectionHub = this.CreateEventProjectionHub(groupManager: groupManager);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            eventProjectionHub.UnsubscribeFromProjection(projectionName);
            //---------------Test Result -----------------------
            groupManager.Received(1).Remove(Arg.Any<string>(), projectionName);
        }

        [Test]
        public void ProjectionUpdated_GivenNoProjectionName_ShouldThrowException()
        {
            //---------------Set up test pack-------------------
            var eventProjectionHub = this.CreateEventProjectionHub();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var exception = Assert.Throws<ArgumentNullException>(() => eventProjectionHub.ProjectionUpdated(string.Empty, "test data"));
            //---------------Test Result -----------------------
            Assert.AreEqual("projectionName", exception.ParamName);
        }

        [Test]
        public void ProjectionUpdated_GivenNoProjectionData_ShouldThrowException()
        {
            //---------------Set up test pack-------------------
            var eventProjectionHub = this.CreateEventProjectionHub();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var exception = Assert.Throws<ArgumentNullException>(() => eventProjectionHub.ProjectionUpdated("test", null));
            //---------------Test Result -----------------------
            Assert.AreEqual("projectionData", exception.ParamName);
        }

        [Test]
        public void ProjectionUpdated_GivenProjectionNameAndData_ShouldSendUpdateToProjectionGroup()
        {
            //---------------Set up test pack-------------------
            var projectionName = "Test";
            var projectionData = "Some Test Data";

            var clients = Substitute.For<IHubCallerConnectionContext<dynamic>>();
            var mockClient = Substitute.For<IMockClient>();
            SubstituteExtensions.Returns(clients.Group(projectionName), mockClient);

            var groupManager = this.CreateGroupManager();
            var eventProjectionHub = this.CreateEventProjectionHub(clients: clients,
                groupManager: groupManager);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            eventProjectionHub.ProjectionUpdated(projectionName, projectionData);
            //---------------Test Result -----------------------
            mockClient.Received(1).updateProjection(projectionName, projectionData);
        }

        private EventProjectionHub CreateEventProjectionHub(IHubCallerConnectionContext<dynamic> clients = null,
            IGroupManager groupManager = null,
            IRequest callerContextRequest = null,
            HubCallerContext callerContext = null)
        {
            var eventProjectionHub = new EventProjectionHub
            {
                Clients = clients ?? Substitute.For<IHubCallerConnectionContext<dynamic>>(),
                Groups = groupManager ?? this.CreateGroupManager(),
                Context = callerContext ?? new HubCallerContext(callerContextRequest ?? Substitute.For<IRequest>(), Guid.NewGuid().ToString()),
            };
            return eventProjectionHub;
        }

        private IGroupManager CreateGroupManager()
        {
            var groupManager = Substitute.For<IGroupManager>();
            groupManager.Add(Arg.Any<string>(), Arg.Any<string>()).Returns(new Task(() => { }));

            return groupManager;
        }
    }
}
