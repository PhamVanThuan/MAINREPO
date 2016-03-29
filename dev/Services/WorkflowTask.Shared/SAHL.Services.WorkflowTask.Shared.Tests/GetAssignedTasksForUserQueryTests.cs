using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using NSubstitute;
using NUnit.Framework;
using SAHL.Core.Services;
using SAHL.Core.Web.Services;
using SAHL.Services.Interfaces.WorkflowTask;
using SAHL.Services.Interfaces.WorkflowTask.Queries;

namespace SAHL.Services.WorkflowTask.Shared.Tests
{
    [TestFixture]
    public class GetAssignedTasksForUserQueryTests
    {
        [Test]
        public void Constructor_GivenNullUsername_ShouldThrowArgumentNullException()
        {
            Assert.That(() =>
            {
                new GetAssignedTasksForUserQuery(null, null);
            },
            Throws.Exception
                .TypeOf<ArgumentNullException>()
                .With.Property("ParamName")
                .EqualTo("username")
            );
        }

        [Test]
        public void Constructor_GivenEmptyUsername_ShouldThrowArgumentException()
        {
            Assert.That(() =>
            {
                new GetAssignedTasksForUserQuery(string.Empty, null);
            },
            Throws.Exception
                .TypeOf<ArgumentException>()
                .With.Property("ParamName")
                .EqualTo("username")
            );
        }

        [Test]
        public void Constructor_GivenWhitespaceUsername_ShouldThrowArgumentException()
        {
            Assert.That(() =>
            {
                new GetAssignedTasksForUserQuery("     ", null);
            },
            Throws.Exception
                .TypeOf<ArgumentException>()
                .With.Property("ParamName")
                .EqualTo("username")
            );
        }

        [Test]
        public void Constructor_GivenValidUsername_ShouldNotThrow()
        {
            const string username = "someone";

            Assert.That(() =>
            {
                new GetAssignedTasksForUserQuery(username, null);
            },
            Throws.Nothing);
        }

        [Test]
        public void Constructor_GivenValidUsername_ShouldHaveUsernamePropertyEqualToSuppliedUsername()
        {
            const string username = "someone";

            var query = new GetAssignedTasksForUserQuery(username, null);
            Assert.That(query.Username, Is.EqualTo(username));
        }

        [Test]
        public void Constructor_GivenNonEmptyRoles_ShouldHaveCapabilitiesPropertyEqualToSuppliedCapabilities()
        {
            const string username = "someone";
            var roles = new List<string>
            {
                "Loss Control Fee Consultant",
                "Invoice Approver"
            };

            var query = new GetAssignedTasksForUserQuery(username, roles);
            Assert.That(query.Capabilites, Is.EqualTo(roles));
        }

        [Test]
        public void PerformQuery_ShouldCompleteSuccessfully()
        {
            var client = ServiceClientHelper.CreateWorkflowTaskServiceClient();

            var exception = Assert.Throws<Exception>(() => 
                client.PerformQuery(new GetAssignedTasksForUserQuery("someone", new List<string>()))
                );

            Assert.AreEqual("Completed successfully", exception.Message);
        }
    }
}