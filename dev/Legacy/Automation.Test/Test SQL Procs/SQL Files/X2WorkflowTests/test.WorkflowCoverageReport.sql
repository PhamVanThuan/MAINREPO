USE [2AM]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF EXISTS (select * from dbo.sysobjects where id = object_id(N'test.WorkflowCoverageReport ') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
Begin
	Drop Procedure test.WorkflowCoverageReport 
	Print 'Dropped Proc test.WorkflowCoverageReport '
End
Go

CREATE PROCEDURE test.WorkflowCoverageReport 

	@workflow_name varchar(100)

AS

 BEGIN 
 
IF (@workflow_name <> 'ALL')

BEGIN

	DECLARE @wflow_activity TABLE (Activity_Type VARCHAR(50), Activity_Count INT)

	--insert the count for each activity type
	INSERT INTO @wflow_activity 
	SELECT aType, count(*) AS Cnt FROM 
	test.WorkflowCoverage 
	WHERE workflow = @workflow_name
	GROUP BY aType

	--progress per workflow map per activity type
	SELECT wc.workflow, wc.aType, SUM(counter) AS Performed, max(Activity_Count) AS Total, 
	ROUND((CAST(SUM(counter) AS FLOAT)/CAST(MAX(Activity_Count) AS FLOAT))*100,2) AS [Perc{%) Complete]
	FROM test.WorkflowCoverage wc
	JOIN @wflow_activity cnt on wc.aType=cnt.Activity_Type
	WHERE workflow = @workflow_name
	GROUP BY wc.aType, wc.workflow
	UNION 
	SELECT wc.workflow,'-All-', sum(counter) AS Performed, count(counter) AS Total, 
	ROUND((CAST(SUM(counter) AS FLOAT)/ CAST(COUNT(counter) AS FLOAT))*100,2) AS [Perc{%) Complete] 
	FROM test.WorkflowCoverage wc
	JOIN @wflow_activity cnt ON wc.aType=cnt.Activity_Type
	WHERE workflow = @workflow_name
	GROUP BY wc.workflow
	--user activities
	SELECT Activity FROM test.WorkflowCoverage wc
	WHERE counter = 0 AND workflow = @workflow_name and aType='User'
	--decisions
		SELECT Activity FROM test.WorkflowCoverage wc
	WHERE counter = 0 AND workflow = @workflow_name and aType='Decision'
	--timers
		SELECT Activity FROM test.WorkflowCoverage wc
	WHERE counter = 0 AND workflow = @workflow_name and aType='Timed'
	--externals
		SELECT Activity FROM test.WorkflowCoverage wc
	WHERE counter = 0 AND workflow = @workflow_name and aType='External'

END

ELSE

BEGIN

	DECLARE @wflow_activity_all TABLE (Activity_Type VARCHAR(50), Activity_Count INT, Workflow VARCHAR(250))

	--insert the count for each activity type
	INSERT INTO @wflow_activity_all 
	SELECT aType, count(*) AS Cnt, workflow FROM 
	test.WorkflowCoverage 
	GROUP BY aType, workflow

	--progress per workflow map per activity type
	SELECT wc.workflow, wc.aType, SUM(counter) AS Performed, max(Activity_Count) AS Total, 
	ROUND((CAST(SUM(counter) AS FLOAT)/CAST(MAX(Activity_Count) AS FLOAT))*100,2) AS [Perc{%) Complete]
	FROM test.WorkflowCoverage wc
	JOIN @wflow_activity_all cnt on wc.aType=cnt.Activity_Type AND wc.workflow = cnt.Workflow
	GROUP BY wc.aType, wc.workflow
	UNION 
	SELECT wc.workflow,'-All-', sum(counter) AS Performed, count(counter) AS Total, 
	ROUND((CAST(SUM(counter) AS FLOAT)/ CAST(COUNT(counter) AS FLOAT))*100,2) AS [Perc{%) Complete] 
	FROM test.WorkflowCoverage wc
	JOIN @wflow_activity_all cnt ON wc.aType=cnt.Activity_Type AND wc.workflow = cnt.Workflow
	GROUP BY wc.workflow
	ORDER BY 1,2

END

END






