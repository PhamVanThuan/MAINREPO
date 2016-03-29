USE [2AM]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF EXISTS (select * from dbo.sysobjects where id = object_id(N'test.GetWorkflowHistoryByOfferKey') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
Begin
	Drop Procedure test.GetWorkflowHistoryByOfferKey
	Print 'Dropped procedure test.GetWorkflowHistoryByOfferKey'
End
Go


CREATE PROCEDURE test.GetWorkflowHistoryByOfferKey

  @offerkey VARCHAR(10) 

AS

SELECT i.[ID]             AS [Instance ID], 
       i.[Name]           AS [Instance Name], 
       s.name             AS [State], 
       a.name             AS [Activity Name], 
       AT.[Name]          AS [Activity Type], 
       wfh.[ActivityDate] AS [Activity Date], 
       activitymessage    AS [Message], 
       wfh.adusername     AS [User], 
       w.[Name]           AS [Workflow] 
FROM   x2.x2.workflowhistory (nolock) wfh 
JOIN x2.x2.state s 
ON wfh.[StateID] = s.[ID] 
JOIN x2.x2.activity a 
ON wfh.activityid = a.id 
JOIN x2.x2.instance (nolock) i 
ON wfh.instanceid = i.id 
JOIN x2.x2.activitytype AT 
ON a.TYPE = AT.id 
JOIN x2.x2.workflow w 
ON i.workflowid = w.id 
WHERE  i.name = @offerkey 
AND Isnumeric(i.name) = 1
order by wfh.activitydate asc
