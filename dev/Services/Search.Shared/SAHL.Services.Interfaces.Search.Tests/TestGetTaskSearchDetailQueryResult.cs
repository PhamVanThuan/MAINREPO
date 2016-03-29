using System;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using NUnit.Framework;
using SAHL.Services.Interfaces.Search.Models;

namespace SAHL.Services.Interfaces.Search.Tests
{
    [TestFixture]
    public class TestGetTaskSearchDetailQueryResult
    {
        [Test]
        public void Constructor()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var queryResult = new GetTaskSearchDetailQueryResult(DateTime.Now, DateTime.Now, null, 
                                                                 "parentWorkflow", "sourceWorkflow");
            //---------------Test Result -----------------------
            Assert.IsNotNull(queryResult);
        }

        [Test]
        public void Constructor_ShouldSetProperties()
        {
            //---------------Set up test pack-------------------
            var workflowTaskAge        = DateTime.Now.AddDays(-10);
            var stateAge               = DateTime.Now.AddDays(-5);
            long? parentTaskId         = 1234;
            var parentTaskWorkflowName = "parentWorkflow";
            var sourceWorkflowName     = "sourceWorkflow";
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var queryResult = new GetTaskSearchDetailQueryResult(workflowTaskAge, stateAge, parentTaskId,
                                                                 parentTaskWorkflowName, sourceWorkflowName);
            //---------------Test Result -----------------------
            Assert.AreEqual(workflowTaskAge, queryResult.WorkflowTaskAge);
            Assert.AreEqual(stateAge, queryResult.StateAge);
            Assert.AreEqual(parentTaskId, queryResult.ParentTaskId);
            Assert.AreEqual(parentTaskWorkflowName, queryResult.ParentTaskWorkflowName);
            Assert.AreEqual(sourceWorkflowName, queryResult.SourceWorkflowName);
        }
    }
}
