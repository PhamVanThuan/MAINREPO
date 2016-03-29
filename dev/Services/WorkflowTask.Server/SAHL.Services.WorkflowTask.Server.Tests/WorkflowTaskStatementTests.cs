using NUnit.Framework;
using SAHL.Services.WorkflowTask.Server.Managers.Statements.TaskStatements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace SAHL.Services.WorkflowTask.Server.Tests
{
    [TestFixture]
    public class WorkflowTaskStatementTests
    {
        [Test]
        public void Convention_ShouldHaveClauseLimitingResultsToV3()
        {
            var assembly = Assembly.Load("SAHL.Services.WorkflowTask");

            var types = assembly.GetTypes()
                .Where(a => a.IsClass
                    && a.Namespace != null
                    && a.Namespace.StartsWith("SAHL.Services.WorkflowTask.Server.Managers.Statements.TaskStatements")
                    && typeof(TaskBaseStatement).IsAssignableFrom(a)
                    && !a.Namespace.Contains("_template")
                    && !a.Name.Contains("TaskBaseStatement")
                );

            var parameters = new object[]
            {
                string.Empty, //string username
                new List<string>(), //List<string> roles
                false, //bool includeTasksWithTheRoleOfEveryone
                string.Empty, //string businessProcessName
                string.Empty, //string workFlowName

                //TODO: this should include multiple states but it makes segregating the query results tricky
                //TODO: relook at a later time
                new List<string> { string.Empty }, //List<string> workFlowStateNames
                string.Empty,
            };
            

            foreach (var item in types)
            {
                var statement = (TaskBaseStatement)Activator.CreateInstance(item, parameters);
                
                var hasVersionCheck = statement.GetStatement().IndexOf("ViewableOnUserInterfaceVersion LIKE '3%'", StringComparison.OrdinalIgnoreCase) >= 0;

                Assert.IsTrue(hasVersionCheck, item.Name + " does not seem to have a limiting clause on the business process' ViewableOnUserInterfaceVersion." +
                                                            "The statement should limit the work list items to only those with a version of 3.");
            }
        }
    }
}
