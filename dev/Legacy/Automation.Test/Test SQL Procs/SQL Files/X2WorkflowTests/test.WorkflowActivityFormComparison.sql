USE [2AM]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF EXISTS (select * from dbo.sysobjects where id = object_id(N'test.WorkflowActivityFormComparison') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
Begin
	Drop Procedure test.WorkflowActivityFormComparison
	Print 'Dropped Proc test.WorkflowActivityFormComparison'
End
Go

CREATE PROCEDURE test.WorkflowActivityFormComparison

@workflow_id_1 int,
@workflow_id_2 int

AS

declare @table table (StateName varchar(250), ActivityName varchar(250), FormName varchar(250))

insert into @table
select s.Name as [State Name], a.Name as [Activity Name],f.Name as [Form Name]
from 
x2.x2.workflow w 
join x2.x2.activity a on w.id=a.workflowid
join x2.x2.state s on a.stateid=s.id
join x2.x2.Form f on a.FormID=f.id
where w.id=@workflow_id_1

declare @table2 table (StateName varchar(250), ActivityName varchar(250), FormName varchar(250))

insert into @table2
select s.Name as [State Name], a.Name as [Activity Name],f.Name as [Form Name]
from 
x2.x2.workflow w 
join x2.x2.activity a on w.id=a.workflowid
join x2.x2.state s on a.stateid=s.id
join x2.x2.Form f on a.FormID=f.id
where w.id=@workflow_id_2

select 
@workflow_id_1 as ID,
t.StateName as [State Name], 
t.ActivityName as [Activity Name], 
t.FormName as [Form Name]
from 
@table t 
left join @table2 t2 on t.ActivityName = t2.ActivityName and t.FormName = t2.FormName
where t2.ActivityName is null
union all
select
@workflow_id_2 as ID, 
t2.StateName as [State Name], 
t2.ActivityName as [Activity Name], 
t2.FormName as [Form Name]
from 
@table2 t2 
left join @table t on t2.ActivityName = t.ActivityName and t2.FormName = t.FormName
where t.ActivityName is null
