using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.Data;
using SAHL.Core.Strings;
using SAHL.Services.Interfaces.WorkflowTask;
using SAHL.Services.WorkflowTask.Server.Managers;
using SAHL.Services.WorkflowTask.Server.Managers.Statements.DataModels;
using SAHL.Services.WorkflowTask.Server.Managers.Statements.TaskStatements;
using SAHL.Services.WorkflowTask.Server.Specs.Fakes.Statements.TaskStatements.FakeBusinessProcess.FakeWorkflow;
using SAHL.Services.WorkflowTask.Server.Specs.Fakes.Statements.TaskStatements.FakeBusinessProcess.FakeWorkflow.FakeState;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.Services.WorkflowTask.Server.Specs.TaskQueryCoordinatorSpecs
{
    //TODO: refactor into smaller subset of specs
    public class when_retrieving_workflow_tasks : WithFakes
    {
        private static IWorkflowTaskDataManager workFlowTaskDataManager;
        private static ITaskStatementSelector statementSelector;
        private static TaskQueryCoordinator taskQueryCoordinator;
        private static string username;
        private static List<string> roles;
        private static List<WorkFlowStateItem> workFlowStates;
        private static WorkFlowStateItem firstState;
        private static WorkFlowStateItem secondState;
        private static FakeStateStatement firstTaskQuery;
        private static FakeWorkFlowStatement secondTaskQuery;
        private static List<ISqlStatement<object>> taskQueriesToExecute;
        private static IEnumerable<BusinessProcess> results;
        private static TaskQueryResultCollator firstQueryResult;
        private static TaskQueryResultCollator secondQueryResult;
        private static ConcurrentDictionary<string, string> workflowItemNameCache;
        private static string dateFormatString;
        private static string currencyFormatString;
        private static ConcurrentDictionary<TaskStatementTypeColumnHeaderPair, ColumnDefinition> columnDefinitionCache;

        private Establish that = () =>
        {
            username = @"SAHL\PersonA";
            roles = new List<string>
            {
                "Some Arbitrary Role"
            };

            firstState = new WorkFlowStateItem
            {
                BusinessProcessName = "Origination",
                WorkFlowName = "Application Management",
                StateName = "Application Check",
            };
            secondState = new WorkFlowStateItem
            {
                BusinessProcessName = "Personal Loan",
                WorkFlowName = "Personal Loan",
                StateName = "NTU",
            };

            workFlowStates = new List<WorkFlowStateItem>
            {
                secondState,
                firstState,
            };

            workFlowTaskDataManager = An<IWorkflowTaskDataManager>();
            workFlowTaskDataManager.WhenToldTo(a => a.RetrieveWorkFlowStatesForWorkListItemsForUsername(Param.Is(username), Param.Is(roles)))
                .Return(workFlowStates);

            firstTaskQuery = new FakeStateStatement(username, roles, true, firstState.BusinessProcessName, firstState.WorkFlowName, new List<string> { firstState.StateName }, "");
            secondTaskQuery = new FakeWorkFlowStatement(username, roles, true, secondState.BusinessProcessName, secondState.WorkFlowName, new List<string> { secondState.StateName }, "");

            taskQueriesToExecute = new List<ISqlStatement<object>>
            {
                firstTaskQuery,
                secondTaskQuery,
            };

            workflowItemNameCache = new ConcurrentDictionary<string, string>();
            StringHelpers.WorkflowItemNameTransformFunction = a => a.Replace(" ", string.Empty);

            statementSelector = An<ITaskStatementSelector>();
            statementSelector.WhenToldTo(a => a.GetStatementTypeForWorkFlow(firstState.BusinessProcessName.ToTransformedWorkFlowItemName(),
                            firstState.WorkFlowName.ToTransformedWorkFlowItemName(), firstState.StateName.ToTransformedWorkFlowItemName()))
                .Return(typeof(FakeStateStatement));

            statementSelector.WhenToldTo(a => a.GetStatementTypeForWorkFlow(secondState.BusinessProcessName.ToTransformedWorkFlowItemName(),
                    secondState.WorkFlowName.ToTransformedWorkFlowItemName(), secondState.StateName.ToTransformedWorkFlowItemName()))
                .Return(typeof(FakeWorkFlowStatement));

            workFlowTaskDataManager.WhenToldTo(a => a.PerformTaskQueries(Param.IsAny<IEnumerable<TaskQueryResultCollator>>()))
                .Callback(SetQueryResultsOnSuppliedParameter);

            columnDefinitionCache = new ConcurrentDictionary<TaskStatementTypeColumnHeaderPair, ColumnDefinition>();

            taskQueryCoordinator = new TaskQueryCoordinator(workFlowTaskDataManager, statementSelector, workflowItemNameCache, columnDefinitionCache);
        };

        private static void SetQueryResultsOnSuppliedParameter()
        {
            var calls = workFlowTaskDataManager
                .ReceivedCalls()
                .Single(a => a.GetMethodInfo().Name == "PerformTaskQueries");

            var argument = calls.GetArguments()
                .Single(a => a is IEnumerable<TaskQueryResultCollator>)
                as IEnumerable<TaskQueryResultCollator>;

            firstQueryResult = argument.Single(a => a.BusinessProcessName == firstState.BusinessProcessName
                && a.WorkFlowName == firstState.WorkFlowName
                && a.StateName == firstState.StateName);

            firstQueryResult.Results = new List<IDictionary<string, object>>
            {
                new Dictionary<string, object>
                {
                    { "InstanceId", 100 },
                    { "SomeArbitraryColumn1", "Task1" },
                    { "SomeArbitraryColumn2", Guid.NewGuid() },
                    { "Decimal|{0:00}", 55555.12345m },
                    { "BadDecimal|{99}", 11111.22222m },
                    { "TrailingPipe|", 33333.44444m },
                },
                new Dictionary<string, object>
                {
                    { "InstanceId", 101 },
                    { "SomeArbitraryColumn1", "Task2" },
                    { "SomeArbitraryColumn2", Guid.NewGuid() },
                    { "Decimal|{0:00}", 44444.98765m },
                    { "BadDecimal|{99}", 11111.22222m },
                    { "TrailingPipe|", 33333.44444m },
                },
            };

            secondQueryResult = argument.Single(a => a.BusinessProcessName == secondState.BusinessProcessName
                && a.WorkFlowName == secondState.WorkFlowName
                && a.StateName == secondState.StateName);

            dateFormatString = "{0:yyyy-MM-dd}";
            currencyFormatString = "{0:c}";

            secondQueryResult.Results = new List<IDictionary<string, object>>
            {
                new Dictionary<string, object>
                {
                    { "SomeArbitraryColumn1000", "Task3" },
                    { "SomeArbitraryColumn1001", Guid.NewGuid() },
                    { "InstanceID", 102 },
                    { "TagIds", Guid.NewGuid() + "," + Guid.NewGuid() + "," + Guid.NewGuid() + "," },
                    { "OriginationSource", "Capitec" },
                    { "SomeNullColumn", null },
                    { "Date|" + dateFormatString, DateTime.Today },
                    { "Currency|" + currencyFormatString, 12345.67890m }
                },
            };
        }

        private Because of = () =>
        {
            results = taskQueryCoordinator.GetWorkflowTasks(username, roles);
        };

        private It should_have_retrieved_the_assigned_work_list_work_flow_states = () =>
        {
            workFlowTaskDataManager
                .WasToldTo(a => a.RetrieveWorkFlowStatesForWorkListItemsForUsername(Param.Is(username), Param.Is(roles)))
                .OnlyOnce();
        };

        private It should_have_retrieved_two_statements = () =>
        {
            statementSelector
                .WasToldTo(a => a.GetStatementTypeForWorkFlow(Param.IsAny<string>(), Param.IsAny<string>(), Param.IsAny<string>()))
                .Twice();
        };

        private It should_have_retrieved_the_statement_for_the_first_work_flow_state = () =>
        {
            statementSelector
                .WasToldTo(a => a.GetStatementTypeForWorkFlow(Param.Is(firstState.BusinessProcessName.ToTransformedWorkFlowItemName()),
                                                              Param.Is(firstState.WorkFlowName.ToTransformedWorkFlowItemName()),
                                                              Param.Is(firstState.StateName.ToTransformedWorkFlowItemName())));
        };

        private It should_have_retrieved_the_statement_for_the_second_work_flow_state = () =>
        {
            statementSelector
                .WasToldTo(a => a.GetStatementTypeForWorkFlow(Param.Is(secondState.BusinessProcessName.ToTransformedWorkFlowItemName()),
                                                              Param.Is(secondState.WorkFlowName.ToTransformedWorkFlowItemName()),
                                                              Param.Is(secondState.StateName.ToTransformedWorkFlowItemName())));
        };

        private It should_have_executed_two_queries = () =>
        {
            workFlowTaskDataManager
                .WasToldTo(a => a.PerformTaskQueries(Param<IEnumerable<TaskQueryResultCollator>>
                    .Matches(b => b.Count() == taskQueriesToExecute.Count))
                );
        };

        private It should_have_provided_the_expected_first_statement_for_execution = () =>
        {
            workFlowTaskDataManager
                .WasToldTo(a => a.PerformTaskQueries(Param<IEnumerable<TaskQueryResultCollator>>
                    .Matches(b => b.OrderBy(c => c.TaskStatementType.Name).First().CreateTaskStatement().GetStatement() == firstTaskQuery.GetStatement()))
                );
        };

        private It should_have_provided_the_expected_second_statement_for_execution = () =>
        {
            workFlowTaskDataManager
                .WasToldTo(a => a.PerformTaskQueries(Param<IEnumerable<TaskQueryResultCollator>>
                    .Matches(b => b.OrderBy(c => c.TaskStatementType.Name).Last().CreateTaskStatement().GetStatement() == secondTaskQuery.GetStatement()))
                );
        };

        private It should_have_returned_non_empty_results = () =>
        {
            results.ShouldNotBeEmpty();
        };

        private It should_have_two_business_processes_in_the_results = () =>
        {
            results.Count().ShouldEqual(2);
        };

        private It should_have_provided_the_first_business_process_in_the_results = () =>
        {
            results.Any(a => a.Name.Equals(firstState.BusinessProcessName)).ShouldBeTrue();
        };

        private It should_have_provided_the_second_business_process_in_the_results = () =>
        {
            results.Any(a => a.Name.Equals(secondState.BusinessProcessName)).ShouldBeTrue();
        };

        private It should_have_non_empty_workflows_for_the_all_business_processes = () =>
        {
            results.All(a => a.WorkFlows.Any()).ShouldBeTrue();
        };

        private It should_have_non_empty_states_for_all_the_workflows = () =>
        {
            results.All(a => a.WorkFlows.All(b => b.States.Any())).ShouldBeTrue();
        };

        private It should_have_non_null_results_for_all_the_states = () =>
        {
            results.All(a => a.WorkFlows.All(b => b.States.All(c => c.Tasks != null))).ShouldBeTrue();
        };

        private It should_have_non_empty_results_for_all_the_states = () =>
        {
            results.All(a => a.WorkFlows.All(b => b.States.All(c => c.Tasks.Any()))).ShouldBeTrue();
        };

        private It should_have_a_process_name_that_matches_the_first_state_item = () =>
        {
            results.OrderBy(a => a.Name).First().Name.ShouldEqual(firstState.BusinessProcessName);
        };

        private It should_have_a_process_name_that_matches_the_second_state_item = () =>
        {
            results.OrderBy(a => a.Name).Last().Name.ShouldEqual(secondState.BusinessProcessName);
        };

        private It should_have_a_workflow_name_that_matches_the_first_state_item = () =>
        {
            results.OrderBy(a => a.Name).First().WorkFlows.Single().Name.ShouldEqual(firstState.WorkFlowName);
        };

        private It should_have_a_workflow_name_that_matches_the_second_state_item = () =>
        {
            results.OrderBy(a => a.Name).Last().WorkFlows.Single().Name.ShouldEqual(secondState.WorkFlowName);
        };

        private It should_have_a_state_name_that_matches_the_first_state_item = () =>
        {
            results.OrderBy(a => a.Name).First().WorkFlows.Single().States.Single().Name.ShouldEqual(firstState.StateName);
        };

        private It should_have_a_state_name_that_matches_the_second_state_item = () =>
        {
            results.OrderBy(a => a.Name).Last().WorkFlows.Single().States.Single().Name.ShouldEqual(secondState.StateName);
        };

        private It should_have_a_result_set_where_all_states_have_column_headers_that_are_not_empty = () =>
        {
            results
                .SelectMany(a => a.WorkFlows)
                .SelectMany(a => a.States)
                .All(a => a.ColumnHeaders != null && a.ColumnHeaders.Any())
                .ShouldBeTrue();
        };

        private It should_have_column_headers_that_match_the_query_result_set_for_the_first_state_item = () =>
        {
            results.OrderBy(a => a.Name).First().WorkFlows.Single().States.Single().ColumnHeaders
                .ShouldEqual(firstQueryResult.Results.First().Keys
                .Where(a => !a.Equals("InstanceId", StringComparison.OrdinalIgnoreCase)
                    && !a.Equals("TagIds", StringComparison.OrdinalIgnoreCase)
                    && !a.Equals("OriginationSource", StringComparison.OrdinalIgnoreCase))
                    .Select(a => a.ToColumnHeaderWithoutFormatString())
                );
        };

        private It should_have_column_headers_that_match_the_query_result_set_for_the_second_state_item = () =>
        {
            results.OrderBy(a => a.Name).Last().WorkFlows.Single().States.Single().ColumnHeaders
                .ShouldEqual(secondQueryResult.Results.First().Keys
                    .Where(a => !a.Equals("InstanceId", StringComparison.OrdinalIgnoreCase)
                        && !a.Equals("TagIds", StringComparison.OrdinalIgnoreCase)
                        && !a.Equals("OriginationSource", StringComparison.OrdinalIgnoreCase))
                    .Select(a => a.ToColumnHeaderWithoutFormatString())
                );
        };

        private It should_have_a_matching_instance_ids_for_the_first_state_item = () =>
        {
            var actualResults = results.OrderBy(a => a.Name).First().WorkFlows.Single().States.Single().Tasks.Select(a => a.InstanceId).OrderBy(a => a);

            var expectedResults = firstQueryResult.Results
                .SelectMany(a => a)
                .Where(a => a.Key.Equals("InstanceId", StringComparison.OrdinalIgnoreCase))
                .Select(a => a.Value)
                .OrderBy(a => a);

            actualResults.ShouldBeLike(expectedResults);
        };

        private It should_have_a_matching_instance_ids_for_the_second_state_item = () =>
        {
            var actualResults = results.OrderBy(a => a.Name).Last().WorkFlows.Single().States.Single().Tasks.Select(a => a.InstanceId).OrderBy(a => a);

            var expectedResults = secondQueryResult.Results
                .SelectMany(a => a)
                .Where(a => a.Key.Equals("InstanceId", StringComparison.OrdinalIgnoreCase))
                .Select(a => a.Value)
                .OrderBy(a => a);

            actualResults.ShouldBeLike(expectedResults);
        };

        private It should_have_tasks_that_match_the_query_results_for_the_first_state_item = () =>
        {
            var actualResults = results.OrderBy(a => a.Name).First().WorkFlows.Single().States.Single().Tasks
                .Select(a => a.Row)
                .OrderBy(a => a.ElementAt(0))
                ;

            var expectedResults = firstQueryResult.Results
                .Select(a => a.Where(b => !b.Key.Equals("InstanceId", StringComparison.OrdinalIgnoreCase)
                    && !b.Key.Equals("TagIds", StringComparison.OrdinalIgnoreCase)
                    && !b.Key.Equals("OriginationSource", StringComparison.OrdinalIgnoreCase)
                    )
                .Select(b => StringExtensions.TryFormat(b.Key.ToFormatStringIfAny(), b.Value)))
                ;

            actualResults.ShouldBeLike(expectedResults);
        };

        private It should_have_tasks_that_match_the_query_results_for_the_second_state_item = () =>
        {
            var actualResults = results.OrderBy(a => a.Name).Last().WorkFlows.Single().States.Single().Tasks
                .Select(a => a.Row)
                .OrderBy(a => a.ElementAt(0))
                ;

            var expectedResults = secondQueryResult.Results
                .Select(a => a.Where(b => !b.Key.Equals("InstanceId", StringComparison.OrdinalIgnoreCase)
                    && !b.Key.Equals("TagIds", StringComparison.OrdinalIgnoreCase)
                    && !b.Key.Equals("OriginationSource", StringComparison.OrdinalIgnoreCase)
                    )
                    .Select(b => StringExtensions.TryFormat(b.Key.ToFormatStringIfAny(), b.Value))
                )
                .OrderBy(a => a.ElementAt(0))
                ;

            actualResults.ShouldBeLike(expectedResults);
        };

        private It should_have_empty_tags_for_the_first_state_item = () =>
        {
            results.OrderBy(a => a.Name).First().WorkFlows.Single().States.Single().Tasks.All(a => !a.TagIds.Any()).ShouldBeTrue();
        };

        private It should_have_non_empty_tags_for_the_second_state_item = () =>
        {
            results.OrderBy(a => a.Name).Last().WorkFlows.Single().States.Single().Tasks.All(a => a.TagIds.Any()).ShouldBeTrue();
        };

        private It should_have_the_expected_tag_ids_for_the_second_state_item = () =>
        {
            var actualResults = results.OrderBy(a => a.Name).Last().WorkFlows.Single().States.Single().Tasks.Single().TagIds.OrderBy(a => a);

            var expectedResults = secondQueryResult.Results
                .Select(a => a.Single(b => b.Key.Equals("TagIds", StringComparison.OrdinalIgnoreCase)))
                .Single()
                .Value
                .ToString()
                .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(Guid.Parse)
                .OrderBy(a => a)
                ;

            actualResults.ShouldBeLike(expectedResults);
        };

        private It should_have_no_origination_source_for_the_first_state_item = () =>
        {
            results.OrderBy(a => a.Name).First().WorkFlows.Single().States.Single().Tasks.All(a => a.OriginationSource == null).ShouldBeTrue();
        };

        private It should_have_the_expected_origination_source_for_the_second_state_item = () =>
        {
            results.OrderBy(a => a.Name).Last().WorkFlows.Single().States.Single().Tasks.Single().OriginationSource.ShouldEqual("Capitec");
        };

        private It should_have_cached_at_least_one_workflow_item_name = () =>
        {
            workflowItemNameCache.IsEmpty.ShouldBeFalse();
        };

        private It should_have_five_entries = () =>
        {
            //one item is a duplicate
            workflowItemNameCache.Count.ShouldEqual(5);
        };

        private It should_have_cached_the_transformed_business_process_name_of_the_first_state = () =>
        {
            workflowItemNameCache.ContainsKeyValuePair(firstState.BusinessProcessName, firstState.BusinessProcessName.ToTransformedWorkFlowItemName())
                .ShouldBeTrue();
        };

        private It should_have_cached_the_transformed_business_process_name_of_the_second_state = () =>
        {
            workflowItemNameCache.ContainsKeyValuePair(secondState.BusinessProcessName, secondState.BusinessProcessName.ToTransformedWorkFlowItemName())
                .ShouldBeTrue();
        };

        private It should_have_cached_the_transformed_workflow_name_of_the_first_state = () =>
        {
            workflowItemNameCache.ContainsKeyValuePair(firstState.WorkFlowName, firstState.WorkFlowName.ToTransformedWorkFlowItemName())
                .ShouldBeTrue();
        };

        private It should_have_cached_the_transformed_workflow_name_of_the_second_state = () =>
        {
            workflowItemNameCache.ContainsKeyValuePair(secondState.WorkFlowName, secondState.WorkFlowName.ToTransformedWorkFlowItemName())
               .ShouldBeTrue();
        };

        private It should_have_cached_the_transformed_state_name_of_the_first_state = () =>
        {
            workflowItemNameCache.ContainsKeyValuePair(firstState.StateName, firstState.StateName.ToTransformedWorkFlowItemName())
                .ShouldBeTrue();
        };

        private It should_have_cached_the_transformed_state_name_of_the_second_state = () =>
        {
            workflowItemNameCache.ContainsKeyValuePair(secondState.StateName, secondState.StateName.ToTransformedWorkFlowItemName())
               .ShouldBeTrue();
        };

        private It should_have_cached_the_formatStrings_from_each_column_returned_by_the_queries = () =>
        {
            columnDefinitionCache.Any().ShouldBeTrue();
        };

        private It should_not_have_cached_a_format_string_for_excluded_columns = () =>
        {
            columnDefinitionCache
                .ContainsKey(new TaskStatementTypeColumnHeaderPair(typeof(FakeStateStatement).FullName, "OriginationSource"))
                .ShouldBeFalse();
        };

        private It should_have_cached_a_default_format_string_if_none_was_specified_and_a_pipe_was_included_in_the_column_header = () =>
        {
            columnDefinitionCache.Single(a => a.Key.ColumnHeaderWithFormatString.Equals("TrailingPipe|")).Value.FormatString.ShouldEqual("{0}");
        };

        private It should_have_cached_the_supplied_format_string_for_the_second_state_item = () =>
        {
            var pair = new TaskStatementTypeColumnHeaderPair(typeof(FakeWorkFlowStatement).FullName, "Date|" + dateFormatString);
            columnDefinitionCache[pair].ShouldNotBeNull();
        };

        private It should_have_a_valid_format_string_for_the_first_state_item = () =>
        {
            var columnHeader = firstQueryResult.Results.First().First(a => a.Key.Contains("|")).Key;
            var pair = new TaskStatementTypeColumnHeaderPair(typeof(FakeStateStatement).FullName, columnHeader);
            var formatString = pair.ColumnHeaderWithFormatString.ToFormatStringIfAny();
            var cachedFormatString = columnDefinitionCache[pair].FormatString;

            formatString.ShouldEqual(cachedFormatString);
        };

        private It should_have_cached_the_supplied_column_name_for_the_second_state_item = () =>
        {
            var columnHeader = firstQueryResult.Results.First().First(a => a.Key.Contains("|")).Key;
            var pair = new TaskStatementTypeColumnHeaderPair(typeof(FakeStateStatement).FullName, columnHeader);
            var columnName = pair.ColumnHeaderWithFormatString.ToColumnHeaderWithoutFormatString();
            var cachedColumnName = columnDefinitionCache[pair].ColumnName;

            columnName.ShouldEqual(cachedColumnName);
        };

        private It should_return_business_processes_in_alphabetical_order = () =>
        {
            var actualResults = results.Select(a => a.Name);
            var expectedResults = results.Select(a => a.Name).OrderBy(a => a);

            actualResults.ShouldEqual(expectedResults);
        };

        private It should_return_workflow_names_in_alphabetical_order = () =>
        {
            foreach (var process in results)
            {
                var actualResults = process.WorkFlows.Select(a => a.Name);
                var expectedResults = process.WorkFlows.Select(a => a.Name).OrderBy(a => a);

                actualResults.ShouldEqual(expectedResults);
            }
        };

        private It should_return_state_names_in_alphabetical_order = () =>
        {
            foreach (var process in results)
            {
                foreach (var workflow in process.WorkFlows)
                {
                    var actualResults = workflow.States.Select(a => a.Name);
                    var expectedResults = workflow.States.Select(a => a.Name).OrderBy(a => a);

                    actualResults.ShouldEqual(expectedResults);
                }
            }
        };

        private It should_return_the_task_items_in_ascending_instanceId_order = () =>
        {
            foreach (var process in results)
            {
                foreach (var workflow in process.WorkFlows)
                {
                    foreach (var state in workflow.States)
                    {
                        var actualResults = state.Tasks.Select(a => a.InstanceId);
                        var expectedResults = state.Tasks.Select(a => a.InstanceId).OrderBy(a => a);

                        actualResults.ShouldEqual(expectedResults);
                    }
                }
            }
        };
    }
}