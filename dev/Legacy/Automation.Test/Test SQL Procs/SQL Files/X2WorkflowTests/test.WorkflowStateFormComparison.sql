USE [2AM]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF EXISTS (select * from dbo.sysobjects where id = object_id(N'test.WorkflowStateFormComparison') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
Begin
	Drop Procedure test.WorkflowStateFormComparison
	Print 'Dropped Proc test.WorkflowStateFormComparison'
End
Go

CREATE PROCEDURE test.WorkflowStateFormComparison

@workflow_id_1 int,
@workflow_id_2 int

AS

declare @table table (StateName varchar(50), FormOrder int, FormName varchar(50), FormDescription varchar(50))

insert into @table
select s.Name as [State Name], sf.FormOrder as [Order], f.Name as [Form Name], f.Description as [Form Description] 
from 
x2.x2.workflow w 
join x2.x2.state s on w.id=s.workflowid
join x2.X2.StateForm sf on s.id=sf.stateid
join x2.x2.Form f on sf.FormID = f.id
where w.id=@workflow_id_1
order by 1,4

declare @table2 table (StateName varchar(50), FormOrder int, FormName varchar(50), FormDescription varchar(50))

insert into @table2
select s.Name as [State Name], sf.FormOrder as [Order], f.Name as [Form Name], f.Description as [Form Description] 
from 
x2.x2.workflow w 
join x2.x2.state s on w.id=s.workflowid
join x2.X2.StateForm sf on s.id=sf.stateid
join x2.x2.Form f on sf.FormID = f.id
where w.id=@workflow_id_2
order by 1,4


select 
@workflow_id_1 as ID,
ISNULL(t.StateName,'') as [State Name], 
ISNULL(t.FormName,'') as [Form Name],  
ISNULL(t.FormDescription,'') as [Form Description], 
ISNULL(t.FormOrder,'') as [Form Order]
from 
@table t
left join @table2 t2 on t.StateName = t2.StateName and t.FormName = t2.FormName
where t2.StateName is null
union all
select
@workflow_id_2 as ID,
ISNULL(t.StateName,'') as [State Name], 
ISNULL(t.FormName,'') as [Form Name],  
ISNULL(t.FormDescription,'') as [Form Description], 
ISNULL(t.FormOrder,'') as [Form Order]
from 
@table2 t
left join @table t2 on t.StateName = t2.StateName and t.FormName = t2.FormName
where t2.StateName is null
