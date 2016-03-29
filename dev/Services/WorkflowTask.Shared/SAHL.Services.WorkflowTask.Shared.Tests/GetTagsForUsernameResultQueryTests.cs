using System;
using NUnit.Framework;
using SAHL.Services.Interfaces.WorkflowTask.Queries;

namespace SAHL.Services.WorkflowTask.Shared.Tests
{
    [TestFixture]
    public class GetTagsForUsernameResultQueryTests
    {
        [Test]
        public void Constructor_GivenNullUsername_ShouldThrowArgumentNullException()
        {
            Assert.That(() =>
            {
                new GetTagsForUsernameResultQuery(null);
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
                new GetTagsForUsernameResultQuery(string.Empty);
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
                new GetTagsForUsernameResultQuery("     ");
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
                new GetTagsForUsernameResultQuery(username);
            },
            Throws.Nothing);
        }

        [Test]
        public void Constructor_GivenValidUsername_ShouldHaveUsernamePropertyEqualToSuppliedUsername()
        {
            const string username = "someone";

            var query = new GetTagsForUsernameResultQuery(username);
            Assert.That(query.Username, Is.EqualTo(username));
        }

        [Test]
        public void PerformQuery_ShouldCompleteSuccessfully()
        {
            var client = ServiceClientHelper.CreateWorkflowTaskServiceClient();

            var exception = Assert.Throws<Exception>(() =>
                client.PerformQuery(new GetTagsForUsernameResultQuery("someone"))
                );

            Assert.AreEqual("Completed successfully", exception.Message);
        }
    }
}