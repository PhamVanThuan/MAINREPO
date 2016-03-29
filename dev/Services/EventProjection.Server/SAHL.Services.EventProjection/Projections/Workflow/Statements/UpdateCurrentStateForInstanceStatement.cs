using SAHL.Core.Data;
using System;

namespace SAHL.Services.EventProjection.Projections.Workflow.Statements
{
    public class UpdateCurrentStateForInstanceStatement : ISqlStatement<object>
    {
        public long InstanceId { get; protected set; }

        public string WorkflowState { get; protected set; }

        public string WorkflowName { get; protected set; }

        public DateTime StateChangeDate { get; protected set; }

        public int GenericKey { get; protected set; }

        public int GenericKeyTypeKey { get; protected set; }

        public UpdateCurrentStateForInstanceStatement(long instanceId, string workflowState, string workflowName, DateTime stateChangeDate, int genericKey, int genericKeyTypeKey)
        {
            this.InstanceId = instanceId;
            this.WorkflowState = workflowState;
            this.WorkflowName = workflowName;
            this.StateChangeDate = stateChangeDate;
            this.GenericKey = genericKey;
            this.GenericKeyTypeKey = genericKeyTypeKey;
        }

        public string GetStatement()
        {
            return @"MERGE [EventProjection].[projection].CurrentStateForInstance WITH(HOLDLOCK) AS target
                    USING (SELECT @InstanceId, @WorkflowState, @WorkflowName,  @GenericKeyTypeKey, @GenericKey, @StateChangeDate)
                    AS source (InstanceId, StateName, WorkflowName, GenericKeyTypeKey, GenericKey, StateChangeDate)
                    ON (target.InstanceId = source.InstanceId)
                    WHEN MATCHED THEN
                        UPDATE SET
	                    WorkflowName = source.WorkflowName,
                        StateName = source.StateName,
                        GenericKeyTypeKey = source.GenericKeyTypeKey,
                        GenericKey = source.GenericKey,
                        StateChangeDate = source.StateChangeDate
                    WHEN NOT MATCHED THEN
                        INSERT (StateChangeDate, InstanceId, StateName, WorkflowName, GenericKeyTypeKey, GenericKey)
                        VALUES (source.StateChangeDate, source.InstanceId, source.StateName, source.WorkflowName, source.GenericKeyTypeKey, source.GenericKey);";
        }
    }
}