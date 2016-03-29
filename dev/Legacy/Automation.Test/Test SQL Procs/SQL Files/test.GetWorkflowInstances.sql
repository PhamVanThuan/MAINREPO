
USE [X2]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[test].[GetWorkflowInstances]') AND type in (N'P', N'PC'))
begin
	DROP PROCEDURE test.GetWorkflowInstances
end
GO
CREATE PROCEDURE test.GetWorkflowInstances 
 @GenericKeyTypeKey int,
 @WorkflowName varchar(max)
AS
BEGIN

declare @WhereClause nvarchar(max)
declare @SelectClause nvarchar(max)
declare @workflowId int
declare @WorkflowStorageTableName varchar(max)
declare @WorkflowStorageKeyName varchar(max)
declare @select_columns varchar(max)

set @WhereClause = ''

select top 01 
	@workflowId = id,
	@WorkflowStorageTableName = StorageTable,
	@WorkflowStorageKeyName = StorageKey
from x2.x2.workflow
where workflow.id = (select max(id) from x2.x2.workflow where name = @WorkflowName)
order by workflow.id desc

if (@workflowId <= 0)
begin
 print 'Workflow not found.'
 return 0;
end


set @SelectClause =
	'select 
		[StorageTableName].*,
		instance.id as instanceid, 
		case when instance.parentinstanceid is null then convert(bit,0) else convert(bit,1) end as IsParentInstance,
		case when instance.sourceinstanceid is null then convert(bit,0) else convert(bit,1) end as IsSourceInstance,
		instance.stateid,
		[StorageTableName].[StorageKeyName] as GenericKey,
		[GenericKeyTypeKey] as GenericKeyTypeKey,
		w.name as WorkflowName
	 from x2.x2.instance
	 inner join x2.x2data.[StorageTableName] on instance.id = [StorageTableName].instanceid
	 inner join x2.x2.state	on instance.stateid = state.id
		and state.type <> 5
	 inner join x2.x2.workflow w on state.workflowID = w.ID
	 where instance.workflowid = [WorkflowID]'

set @SelectClause = REPLACE(@SelectClause,'[GenericKeyTypeKey]',@GenericKeyTypeKey)
set @SelectClause = REPLACE(@SelectClause,'[StorageTableName]',@WorkflowStorageTableName)
set @SelectClause = REPLACE(@SelectClause,'[StorageKeyName]',@WorkflowStorageKeyName)
set @SelectClause = REPLACE(@SelectClause,'[WorkflowID]',@workflowId)	

exec sp_executesql @SelectClause

END

