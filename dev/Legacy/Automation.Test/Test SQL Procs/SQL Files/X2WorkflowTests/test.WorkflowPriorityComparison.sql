USE [2AM]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF EXISTS (select * from dbo.sysobjects where id = object_id(N'test.WorkflowPriorityComparison') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
Begin
	Drop Procedure test.WorkflowPriorityComparison
	Print 'Dropped Proc test.WorkflowPriorityComparison '
End
Go

CREATE PROCEDURE test.WorkflowPriorityComparison

@workflow_id_1 int,
@workflow_id_2 int

AS

declare @currentworkflow table (ActivityName varchar(50), Priority int, StateName varchar(50))

insert into @currentworkflow
select a.name, a.priority, s.name StateName
from x2.x2.activity a
join x2.x2.state s on a.stateid=s.id
where a.workflowid=@workflow_id_2

select 
@workflow_id_1 as ID,
s.Name as [State Name], 
a.name as [Activity Name], 
a.priority as [Priority]
from x2.x2.activity a
join x2.x2.state s on a.stateid=s.id
join @currentworkflow cw on a.name=cw.ActivityName and  s.Name = cw.StateName
where a.workflowid=(
@workflow_id_1
)
and cw.Priority<>a.priority
union all
select 
@workflow_id_2 as ID,
cw.StateName as [State Name ],
cw.ActivityName [Activity Name],
cw.priority [Priority]
from x2.x2.activity a
join x2.x2.state s on a.stateid=s.id
join @currentworkflow cw on a.name=cw.ActivityName and  s.Name = cw.StateName
where a.workflowid=(
@workflow_id_1
)
and cw.Priority<>a.priority

