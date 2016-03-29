using SAHL.Core;
using SAHL.Core.Extensions;
using SAHL.Services.Interfaces.WorkflowTask;
using SAHL.Services.WorkflowTask.Server.Managers;
using SAHL.Services.WorkflowTask.Server.Managers.Statements.DataModels;
using SAHL.Services.WorkflowTask.Server.Managers.Statements.TaskStatements;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace SAHL.Services.WorkflowTask.Server
{
    public class TaskQueryCoordinator : ITaskQueryCoordinator
    {
        private const string instanceIdColumnName = "InstanceId";
        private const string tagIdsColumnName = "TagIds";
        private readonly List<string> internalWorkflowTaskProperties = new List<string> { "InstanceId", "GenericKey"
            , "GenericKeyTypeKey", "WorkflowName", "ProcessName", "OriginationSource", "Subject" };

        private readonly ConcurrentDictionary<TaskStatementTypeColumnHeaderPair, ColumnDefinition> columnDefinitionCache;
        private readonly string[] columnsToExcludeFromResultSet = { instanceIdColumnName, tagIdsColumnName };
        private readonly List<string> emptyListColumnHeaderPlaceHolder = new List<string>();
        private readonly ITaskStatementSelector statementSelector;

        private readonly Func<string, string> workFlowItemNameTransformer = a => a.Replace(" ", string.Empty);
        private readonly ConcurrentDictionary<string, string> workflowItemNameCache;
        private readonly IWorkflowTaskDataManager workflowTaskDataManager;

        public TaskQueryCoordinator(IWorkflowTaskDataManager workflowTaskDataManager, ITaskStatementSelector statementSelector,
            ConcurrentDictionary<string, string> workflowItemNameCache,
            ConcurrentDictionary<TaskStatementTypeColumnHeaderPair, ColumnDefinition> columnDefinitionCache)
        {
            this.workflowTaskDataManager = workflowTaskDataManager;
            this.statementSelector = statementSelector;
            this.workflowItemNameCache = workflowItemNameCache;
            this.columnDefinitionCache = columnDefinitionCache;
        }

        public IEnumerable<BusinessProcess> GetWorkflowTasks(string username, List<string> capabilities)
        {
            var workflowStates = workflowTaskDataManager.RetrieveWorkFlowStatesForWorkListItemsForUsername(username, capabilities);

            var listLock = new object();
            var resultCollators = new List<TaskQueryResultCollator>();

            Parallel.ForEach(workflowStates, CoreGlobals.DefaultParallelOptions, item =>
            {
                var type = GetTaskStatementType(item);
                var collator = new TaskQueryResultCollator(username, capabilities, true, item.BusinessProcessName, item.WorkFlowName, item.StateName, type, item.StorageTable);

                lock (listLock)
                {
                    resultCollators.Add(collator);
                }
            });

            workflowTaskDataManager.PerformTaskQueries(resultCollators);

            var results = GetBusinessProcessesFromResultCollators(resultCollators);

            return results;
        }

        private IEnumerable<BusinessProcess> GetBusinessProcessesFromResultCollators(List<TaskQueryResultCollator> resultCollators)
        {
            var results = new List<BusinessProcess>();

            //do this in parallel if there are enough processes being returned to warrant it
            foreach (var processItem in resultCollators.GroupBy(a => a.BusinessProcessName))
            {
                var process = CreateBusinessProcessResultItem(processItem);

                if (!process.WorkFlows.Any())
                {
                    continue;
                }

                results.Add(process);
            }

            var orderedResults = results
                .OrderBy(a => a.Name)
                .ToList();

            return orderedResults;
        }

        private BusinessProcess CreateBusinessProcessResultItem(IGrouping<string, TaskQueryResultCollator> processItem)
        {
            var process = new BusinessProcess();
            process.Name = processItem.Key;

            process.WorkFlows = new List<WorkFlow>();

            //do this in parallel if there are enough workflows being returned to warrant it
            foreach (var workFlowItem in processItem.GroupBy(a => a.WorkFlowName))
            {
                var workFlow = CreateWorkFlowResultItem(workFlowItem);

                if (!workFlow.States.Any())
                {
                    continue;
                }

                process.WorkFlows.Add(workFlow);
            }

            process.WorkFlows = process.WorkFlows
                .OrderBy(a => a.Name)
                .ToList();

            return process;
        }

        private WorkFlow CreateWorkFlowResultItem(IGrouping<string, TaskQueryResultCollator> workFlowItem)
        {
            var workFlow = new WorkFlow();
            workFlow.Name = workFlowItem.Key;

            workFlow.States = new List<State>();

            //do this in parallel if there are enough states being returned to warrant it
            foreach (var stateItem in workFlowItem.GroupBy(a => a.StateName))
            {
                var state = CreateStateResultItem(stateItem);

                if (!state.Tasks.Any())
                {
                    continue;
                }

                workFlow.States.Add(state);
            }

            workFlow.States = workFlow.States
                .OrderBy(a => a.Name)
                .ToList();

            return workFlow;
        }

        private State CreateStateResultItem(IGrouping<string, TaskQueryResultCollator> stateItem)
        {
            var state = new State();
            state.Name = stateItem.Key;
            state.ColumnHeaders = this.emptyListColumnHeaderPlaceHolder;

            var listLock = new object();
            state.Tasks = new List<Interfaces.WorkflowTask.Task>();

            var firstItem = stateItem.FirstOrDefault();
            if (firstItem == null)
            {
                return state;
            }

            var taskStatementType = firstItem.TaskStatementType;

            state.ColumnHeaders = GetColumnHeaders(stateItem, taskStatementType).ToList();

            Parallel.ForEach(stateItem.SelectMany(a => a.Results), CoreGlobals.DefaultParallelOptions, taskFields =>
            {
                var task = CreateTaskResultItem(taskFields, taskStatementType);
                lock (listLock)
                {
                    state.Tasks.Add(task);
                }
            });

            state.Tasks = state.Tasks
                .OrderBy(a => a.InstanceId)
                .ToList();

            return state;
        }

        private IEnumerable<string> GetColumnHeaders(IGrouping<string, TaskQueryResultCollator> stateItem, Type taskStatementType)
        {
            var firstStateItem = stateItem.FirstOrDefault();
            if (firstStateItem == null)
            {
                yield break;
            }

            var firstResultItem = firstStateItem.Results.FirstOrDefault();
            if (firstResultItem == null)
            {
                yield break;
            }

            foreach (var item in firstResultItem)
            {
                if (
                        this.columnsToExcludeFromResultSet.Contains(item.Key, StringComparer.OrdinalIgnoreCase)
                        || this.internalWorkflowTaskProperties.Any(y => y.Equals(item.Key, StringComparison.OrdinalIgnoreCase))
                    )
                {
                    continue;
                }

                var columnDefinition = GetColumnDefinition(taskStatementType, item);

                yield return columnDefinition.ColumnName;
            }
        }

        private ColumnDefinition GetColumnDefinition(Type taskStatementType, KeyValuePair<string, object> item)
        {
            var key = new TaskStatementTypeColumnHeaderPair(taskStatementType.FullName, item.Key);

            var columnDefinitionCreationFunction = new Func<TaskStatementTypeColumnHeaderPair, ColumnDefinition>(
                a => new ColumnDefinition(GetColumnName(item.Key), GetFormatString(item.Key)));

            return this.columnDefinitionCache.TryGetValueIfNotPresentThenAdd(key, columnDefinitionCreationFunction);
        }

        private string GetColumnName(string columnHeader)
        {
            int indexOfDelimiter;
            return ShouldBeFormatted(columnHeader, out indexOfDelimiter) ? columnHeader.Substring(0, indexOfDelimiter) : columnHeader;
        }

        private Interfaces.WorkflowTask.Task CreateTaskResultItem(IDictionary<string, object> taskFields, Type taskStatementType)
        {
            var task = new Interfaces.WorkflowTask.Task();
            task.Row = new List<string>();
            task.TagIds = new List<Guid>();

            SetRowItems(task, taskFields, taskStatementType);

            return task;
        }

        private void SetRowItems(Interfaces.WorkflowTask.Task task, IDictionary<string, object> taskFields, Type taskStatementType)
        {
            var rowItems = new List<string>();
            foreach (var keyValuePair in taskFields)
            {
                if (
                        this.columnsToExcludeFromResultSet.Contains(keyValuePair.Key, StringComparer.OrdinalIgnoreCase)
                        || this.internalWorkflowTaskProperties.Any(y => y.Equals(keyValuePair.Key))
                    )
                {
                    ProcessExcludedField(task, keyValuePair);
                }
                else
                {
                    var value = GetValue(keyValuePair, taskStatementType);
                    rowItems.Add(value);
                }
            }
            task.Row = rowItems;
        }

        private string GetValue(KeyValuePair<string, object> keyValuePair, Type taskStatementType)
        {
            if (keyValuePair.Value == null)
            {
                return null;
            }

            var keyPair = new TaskStatementTypeColumnHeaderPair(taskStatementType.FullName, keyValuePair.Key);

            var formatString = this.columnDefinitionCache[keyPair].FormatString;

            return TryFormat(keyValuePair, formatString);
        }

        private static string TryFormat(KeyValuePair<string, object> keyValuePair, string formatString)
        {
            return formatString.Equals("{0}")
                ? keyValuePair.Value.ToString()
                : SAHL.Core.Strings.StringExtensions.TryFormat(formatString, keyValuePair.Value);
        }

        private static string GetFormatString(string columnHeader)
        {
            var indexOfDelimiter = GetIndexOfFirstPipe(columnHeader);
            if (indexOfDelimiter < 0 || indexOfDelimiter == columnHeader.Length - 1)
            {
                return "{0}";
            }
            var result = columnHeader.Substring(indexOfDelimiter + 1);
            return result;
        }

        private bool ShouldBeFormatted(string key, out int indexOfDelimiter)
        {
            indexOfDelimiter = GetIndexOfFirstPipe(key);
            return indexOfDelimiter >= 0;
        }

        private static int GetIndexOfFirstPipe(string key)
        {
            return key.IndexOf("|");
        }

        private void ProcessExcludedField(Interfaces.WorkflowTask.Task task, KeyValuePair<string, object> keyValuePair)
        {
            var key = keyValuePair.Key;
            var value = keyValuePair.Value;

            if (value == null)
            {
                return;
            }

            //TODO: if this conditional checking grows larger, consider action dictionary pattern
            if (key.Equals(instanceIdColumnName, StringComparison.OrdinalIgnoreCase))
            {
                int instanceId;
                int.TryParse(value.ToString(), out instanceId);
                task.InstanceId = instanceId; //will be 0 if parsing fails
            }
            else if (key.Equals(tagIdsColumnName, StringComparison.OrdinalIgnoreCase))
            {
                task.TagIds = ConvertCommaDelimitedTagIdsToList(value.ToString());
            }
            else if (internalWorkflowTaskProperties.Any(y => y.Equals(key, StringComparison.Ordinal)))
            {
                PropertyInfo propertyInfo = task.GetType().GetProperty(key);
                if (propertyInfo != null)
                {
                    propertyInfo.SetValue(task, value, null);
                }
            }
        }

        private List<Guid> ConvertCommaDelimitedTagIdsToList(string taskIds)
        {
            var result = new List<Guid>();
            if (string.IsNullOrWhiteSpace(taskIds))
            {
                return result;
            }

            foreach (var item in taskIds.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                Guid value;
                if (!Guid.TryParse(item, out value))
                {
                    continue;
                }
                result.Add(value);
            }
            return result;
        }

        private Type GetTaskStatementType(WorkFlowStateItem item)
        {
            var process = workflowItemNameCache.TryGetValueIfNotPresentThenAdd(item.BusinessProcessName, workFlowItemNameTransformer);
            var name = workflowItemNameCache.TryGetValueIfNotPresentThenAdd(item.WorkFlowName, workFlowItemNameTransformer);
            var state = workflowItemNameCache.TryGetValueIfNotPresentThenAdd(item.StateName, workFlowItemNameTransformer);
            return this.statementSelector.GetStatementTypeForWorkFlow(process, name, state);
        }
    }
}
