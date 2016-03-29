using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Data;
using SAHL.Core.Testing.Fakes;
using SAHL.Services.WorkflowTask.Server.Managers;
using SAHL.Services.WorkflowTask.Server.Specs.Fakes.Statements.TaskStatements;
using SAHL.Services.WorkflowTask.Server.Specs.Fakes.Statements.TaskStatements.FakeBusinessProcess;
using SAHL.Services.WorkflowTask.Server.Specs.Fakes.Statements.TaskStatements.FakeBusinessProcess.FakeWorkflow;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.Services.WorkflowTask.Server.Specs.Managers.WorkflowTaskDataManagerSpecs
{
    public class when_executing_a_task_statement : WithFakes
    {
        private static string username;
        private static IDbFactory dbFactory;
        private static WorkflowTaskDataManager workflowTaskDataManager;

        private Establish context = () =>
        {
            results1 = new List<IDictionary<string, object>>();
            results1.Add(new Dictionary<string, object>
            {
                { "ID", 24829207 },
                { "InstanceID", 7868114 },
                { "ADUserName", "Registrations Manager" },
                { "ListDate", new DateTime(2014, 10, 27, 11, 54, 13) },
                { "Message", "Create EWork PipelineCase" },
            });


            results2 = new List<IDictionary<string, object>>();
            results2.Add(new Dictionary<string, object>
            {
                { "RandomID", 12345 },
                { "Text", "Do something" },
            });

            results3 = new List<IDictionary<string, object>>();
            results3.Add(new Dictionary<string, object>
            {
                { "SomeOtherId", 654321 },
                { "Value", Guid.NewGuid() }
            });

            statement1 = new FakeGenericStatement();
            statement2 = new FakeWorkFlowStatement();
            statement3 = new FakeBusinessProcessStatement();

            statement1Collator = new TaskQueryResultCollator(string.Empty, new List<string>(), true, string.Empty, string.Empty, string.Empty, typeof(FakeGenericStatement), "");

            statement2Collator = new TaskQueryResultCollator(string.Empty, new List<string>(), true, string.Empty, string.Empty, string.Empty, typeof(FakeWorkFlowStatement), "");

            statement3Collator = new TaskQueryResultCollator(string.Empty, new List<string>(), true, string.Empty, string.Empty, string.Empty, typeof(FakeBusinessProcessStatement), "");

            queriesToExecute = new List<ITaskQueryResultCollator>
            {
                statement1Collator,
                statement2Collator,
                statement3Collator,
            };

            dbFactory = An<FakeDbFactory>();

            dbFactory.WhenToldTo(a => a.NewDb().InReadOnlyWorkflowContext().Select(Param.Is(statement1Collator.CreateTaskStatement())))
                .Return(results1);

            dbFactory.WhenToldTo(a => a.NewDb().InReadOnlyWorkflowContext().Select(Param.Is(statement2Collator.CreateTaskStatement())))
                .Return(results2);

            dbFactory.WhenToldTo(a => a.NewDb().InReadOnlyWorkflowContext().Select(Param.Is(statement3Collator.CreateTaskStatement())))
                .Return(results3);

            workflowTaskDataManager = new WorkflowTaskDataManager(dbFactory);
        };

        private Because of = () =>
        {
            workflowTaskDataManager.PerformTaskQueries(queriesToExecute);
        };

        public It should_have_results_that_match_the_expected_output = () =>
        {
            queriesToExecute.Any(a => a.Results.SequenceEqual(results1)).ShouldBeTrue();
            queriesToExecute.Any(a => a.Results.SequenceEqual(results2)).ShouldBeTrue();
            queriesToExecute.Any(a => a.Results.SequenceEqual(results3)).ShouldBeTrue();
        };

        private static List<ITaskQueryResultCollator> queriesToExecute;
        private static IEnumerable<IEnumerable<IDictionary<string, object>>> result;
        private static List<IDictionary<string, object>> results1;
        private static List<IDictionary<string, object>> results2;
        private static FakeGenericStatement statement1;
        private static FakeWorkFlowStatement statement2;
        private static ITaskQueryResultCollator statement1Collator;
        private static ITaskQueryResultCollator statement2Collator;
        private static FakeBusinessProcessStatement statement3;
        private static TaskQueryResultCollator statement3Collator;
        private static List<IDictionary<string, object>> results3;
    }
}
