using NUnit.Framework;
using SAHL.Core.Testing;
using SAHL.Services.Interfaces.Search.Models;
using SAHL.Services.Interfaces.Search.Queries;
using SAHL.Testing.Common.Helpers;
using System.Linq;

namespace SAHL.Testing.SolrIndex.Tests
{
    [TestFixture]
    public sealed class TaskSearchTests : SolrIndexTest
    {
        [Test]
        public void TaskApplicationSearchTest()
        {
            var state = "Loss Control Invoice Received";
            var workflow = "Third Party Invoices";
            var record = TestApiClient.Get<WorkflowSearch>(new { state = state, workflow = workflow }).First();

            var searchFilters = new SearchFilter[] { new SearchFilter("Status", "In Progress"), new SearchFilter("Workflow", @"""Third Party Invoices""") };
            var query = new SearchForTaskQuery(record.GenericKey.ToString(), searchFilters, "Task");

            base._searchService.PerformQuery<SearchForTaskQuery>(query).WithoutMessages();
            var results = query.Result.Results;

            Assert.Greater(results.Count(), 0, "No search results for {0}", record.GenericKey.ToString());
            Assert.AreEqual(record.Workflow, results.FirstOrDefault().Workflow, "first seach results did not return the workflow case in the expected workflow");
            Assert.AreEqual(record.State, results.FirstOrDefault().State, "first search results did not return the workflow case at the expected state within the workflow");
            Assert.AreEqual(record.GenericKey.ToString(), results.FirstOrDefault().GenericKeyValue);
            Assert.AreEqual(record.InstanceId, results.FirstOrDefault().InstanceId);
            Assert.AreEqual(record.Subject, results.FirstOrDefault().Subject);
        }

        [Test]
        public void TaskArchiveApplicationSearchTest()
        {
            var state = "Archive Disputes";
            var workflow = "Credit";
            var record = TestApiClient.Get<WorkflowSearch>(new { state = state, workflow = workflow }).First();

            var searchFilters = new SearchFilter[] { new SearchFilter("Status", "Archived"), new SearchFilter("Workflow", "Credit") };
            var query = new SearchForTaskQuery(record.GenericKey.ToString(), searchFilters, "Task");

            base._searchService.PerformQuery<SearchForTaskQuery>(query).WithoutMessages();
            var results = query.Result.Results;

            Assert.Greater(results.Count(), 0, "No search results for {0}", record.GenericKey.ToString());
            Assert.AreEqual(record.Workflow, results.FirstOrDefault().Workflow, "first seach results did not return the workflow case in the expected workflow");
            Assert.AreEqual(record.State, results.FirstOrDefault().State, "first search results did not return the workflow case at the expected state within the workflow");
            Assert.AreEqual(results.FirstOrDefault().GenericKeyValue, record.GenericKey.ToString());
            Assert.AreEqual(results.FirstOrDefault().InstanceId, record.InstanceId);
        }
    }

    public class WorkflowSearch : SAHL.Core.Data.IDataModel
    {
        public int InstanceId { get; set; }

        public int GenericKey { get; set; }

        public string Subject { get; set; }

        public string State { get; set; }

        public string Workflow { get; set; }
    }
}