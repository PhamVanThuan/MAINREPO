USE [2AM]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF EXISTS (select * from dbo.sysobjects where id = object_id(N'[test].[UpdateWorkflowCoverage]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
Begin
	Drop Procedure [test].[UpdateWorkflowCoverage]  
	Print 'Dropped Proc [test].[UpdateWorkflowCoverage] '
End
Go

CREATE PROCEDURE [test].[UpdateWorkflowCoverage] 

  @workflow_name varchar(250),
  @test_start_date datetime
  
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;


IF object_id('2am.test.WorkflowCoverage') IS NULL
	BEGIN
		CREATE TABLE [test].[WorkflowCoverage](
			[Activity] [varchar](100) NULL,
			[Workflow] [varchar](100) NULL,
			[counter] [int] NULL,
			[aType] [varchar](50) NULL,
			[State] [varchar](100) NULL
		) ON [PRIMARY]
    
		Print 'workflow_coverage table created'
	END
		ELSE
			BEGIN
				Print 'Table Already Exists'
			END

IF (@workflow_name = 'ALL') 
 

 BEGIN
 
	PRINT 'Truncating workflow_coverage'
	
	TRUNCATE TABLE [2AM].test.WorkflowCoverage;

	WITH activities (activityname,atype,workflow) 
	AS 
	 (
	SELECT a.name [activity],at.name, maxwflow.wflowname
	FROM x2.x2.activity a 
	JOIN (SELECT MAX(id) id, name wflowname FROM x2.x2.workflow w GROUP BY name) AS maxwflow
	ON a.workflowid=maxwflow.id left join x2.x2.activitytype at ON a.type=at.id
	LEFT JOIN x2.x2.state s ON a.stateid=s.id
	WHERE maxwflow.wflowname not in ('quick cash','rcs','release and variations','interestonlysms')
	GROUP BY a.name,maxwflow.wflowname,at.name
	)
    
	INSERT INTO [2AM].test.WorkflowCoverage (activity,atype, workflow,state,counter)
	SELECT 
	activities.activityname,
	activities.atype,
	activities.workflow,
	'', 
	CASE WHEN activity_performed.activity_id IS NOT NULL THEN 1 ELSE 0 END AS Count_Performed
	FROM activities
	left join (
	SELECT DISTINCT 
	a.name [activity_name],w.name AS [workflow], MAX(activityid) AS activity_id
	FROM x2.x2.workflowhistory wfh (NOLOCK) 
	JOIN x2.x2.activity a (NOLOCK) ON wfh.activityid=a.id
	JOIN x2.x2.workflow w (NOLOCK) ON a.workflowid=w.id 
	WHERE activitydate > @test_start_date
	GROUP BY a.name, w.name 
	) activity_performed on activities.activityname=activity_performed.[activity_name] and activity_performed.workflow=activities.workflow
	ORDER BY 3,1
	
	SELECT 1

END

ELSE

BEGIN 

	PRINT 'Deleting coverage for:' + @workflow_name + ' workflow'

	DELETE FROM [2am].test.WorkflowCoverage WHERE Workflow = @workflow_name;

	WITH activities (activityname,atype,workflow) 
	AS 
	 (
	SELECT a.name [activity],at.name, maxwflow.wflowname
	FROM x2.x2.activity a 
	JOIN (SELECT MAX(id) id, name wflowname FROM x2.x2.workflow w GROUP BY name) AS maxwflow
	ON a.workflowid=maxwflow.id left join x2.x2.activitytype at ON a.type=at.id
	LEFT JOIN x2.x2.state s ON a.stateid=s.id
	WHERE maxwflow.wflowname = @workflow_name
	GROUP BY a.name,maxwflow.wflowname,at.name
	)
    
	INSERT INTO [2AM].test.WorkflowCoverage (activity,atype, workflow,state,counter)
	SELECT 
	activities.activityname,
	activities.atype,
	activities.workflow,
	'', 
	CASE WHEN activity_performed.activity_id IS NOT NULL THEN 1 ELSE 0 END AS Count_Performed
	FROM activities
	left join (
	SELECT DISTINCT 
	a.name [activity_name],w.name AS [workflow], MAX(activityid) AS activity_id
	FROM x2.x2.workflowhistory wfh (NOLOCK) 
	JOIN x2.x2.activity a (NOLOCK) ON wfh.activityid=a.id
	JOIN x2.x2.workflow w (NOLOCK) ON a.workflowid=w.id 
	WHERE activitydate > @test_start_date
	GROUP BY a.name, w.name 
	) activity_performed on activities.activityname=activity_performed.[activity_name] and activity_performed.workflow=activities.workflow
	ORDER BY 3,1
	
	SELECT 1
	
END

END
GO

SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO

