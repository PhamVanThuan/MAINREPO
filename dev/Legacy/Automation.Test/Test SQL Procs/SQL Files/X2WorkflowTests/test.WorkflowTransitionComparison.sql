USE [2AM]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF EXISTS (select * from dbo.sysobjects where id = object_id(N'test.WorkflowTransitionComparison') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
Begin
	Drop Procedure test.WorkflowTransitionComparison
	Print 'Dropped Proc test.WorkflowTransitionComparison'
End
Go

CREATE PROCEDURE test.WorkflowTransitionComparison

@workflow_id_1 int,
@workflow_id_2 int

AS

BEGIN

DECLARE @table table (ActivityName varchar(50), SDSDGKey int, TransitionDescription varchar(100))
DECLARE @table2 table (ActivityName varchar(50), SDSDGKey int, TransitionDescription varchar(100))

INSERT INTO @table
select 
a.name as [Activity], 
rtrim(cast(sa.stagedefinitionstagedefinitiongroupkey as varchar(10))) as [SDSDG Key], 
cast(sdg.description as varchar(50)) + ': ' + sd.description as [Transition Description]
from x2.x2.workflow w
join x2.x2.process p on p.id=w.processid
join x2.x2.activity a on a.workflowid=w.id
join x2.x2.StageActivity sa on sa.activityid=a.id
join [2am].dbo.stagedefinitionstagedefinitiongroup sdsdg on sdsdg.stagedefinitionstagedefinitiongroupkey=sa.stagedefinitionstagedefinitiongroupkey
join [2am].dbo.stagedefinitiongroup sdg on sdg.stagedefinitiongroupkey=sdsdg.stagedefinitiongroupkey
join [2am].dbo.StageDefinition sd on sd.StageDefinitionKey=sdsdg.StageDefinitionKey
where w.id = @workflow_id_1

INSERT INTO @table2
select 
a.name as [Activity], 
rtrim(cast(sa.stagedefinitionstagedefinitiongroupkey as varchar(10)))as [SDSDG Key], 
cast(sdg.description as varchar(50)) + ': ' + sd.description as [Transition Description]
from x2.x2.workflow w
join x2.x2.process p on p.id=w.processid
join x2.x2.activity a on a.workflowid=w.id
join x2.x2.StageActivity sa on sa.activityid=a.id
join [2am].dbo.stagedefinitionstagedefinitiongroup sdsdg on sdsdg.stagedefinitionstagedefinitiongroupkey=sa.stagedefinitionstagedefinitiongroupkey
join [2am].dbo.stagedefinitiongroup sdg on sdg.stagedefinitiongroupkey=sdsdg.stagedefinitiongroupkey
join [2am].dbo.StageDefinition sd on sd.StageDefinitionKey=sdsdg.StageDefinitionKey
where w.id = @workflow_id_2

SELECT t.*, @workflow_id_1 as ID FROM @table t
LEFT JOIN @table2 t2 on t.ActivityName=t2.ActivityName
and t.SDSDGKey=t2.SDSDGKey
where t2.ActivityName IS NULL
union all
SELECT t.*, @workflow_id_2 as ID FROM @table2 t
LEFT JOIN @table t2 on t.ActivityName=t2.ActivityName
and t.SDSDGKey=t2.SDSDGKey
where t2.ActivityName IS NULL

END

