USE X2;
/*
Pre migration Query to map states by X2ID
and Declarations
*/
DECLARE @OldWorkFlowID INT
DECLARE @NewWorkFlowID INT
DECLARE @NewWorkflowIDs TABLE(ID INT)
DECLARE @OLDWorkflowIDs TABLE(ID INT)
CREATE TABLE #PublishedStateMapping (
	[OldWorkFlowID] INT
	,[OldStateID] INT
	,[NewWorkFlowID] INT
	,[NewStateID] INT
)


SET @NewWorkFlowID = :NewWorkFlowID
INSERT INTO @NewWorkflowIDs (ID) SELECT MAX(ID) as ID FROM X2.X2.Workflow (NOLOCK) GROUP BY NAME ORDER BY NAME
INSERT INTO @OLDWorkflowIDs SELECT MAX(ID) as ID FROM [x2].[x2].Workflow (NOLOCK) WHERE ID NOT IN (SELECT ID FROM @NewWorkflowIDs) GROUP BY NAME
INSERT INTO #PublishedStateMapping ([OldWorkFlowID],[OldStateID],[NewWorkFlowID],[NewStateID])
SELECT oldState.WorkflowID,oldState.ID,newState.WorkflowID,newState.ID
FROM [x2].[x2].State newState (NOLOCK)
INNER JOIN [x2].[x2].State oldState (NOLOCK) ON oldState.X2ID = newState.X2ID
INNER JOIN @OLDWorkflowIDs oldWork ON oldState.WorkflowID = oldWork.ID
WHERE newState.workFlowID = @NewWorkFlowID 

CREATE CLUSTERED INDEX IDX_PublishedStateMapping_WorkFlowID ON #PublishedStateMapping(NewWorkFlowID)
CREATE NONCLUSTERED INDEX IDX_PublishedStateMapping_Rows
ON #PublishedStateMapping(NewWorkFlowID)
INCLUDE ([OldWorkFlowID]
		,[OldStateID]
		,[NewStateID])

--SET @OldWorkflowID = (SELECT TOP 1 [OldWorkFlowID] FROM #PublishedStateMapping)
SET @OldWorkflowID = (SELECT MAX ([OldWorkFlowID]) FROM #PublishedStateMapping)



/*
OLD STUFF
declare @OldWorkflowID int
declare @NewWorkflowID int

set @NewWorkflowID = {NEWWORKFLOWNAME}
set @OldWorkflowID = {OLDWORKFLOWNAME}
*/
--IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'#mappedstates') AND type in (N'U'))
--	drop table #mappedstates
--CREATE TABLE #mappedstates(OldName varchar(50), NewName varchar(50))

-- fix active external activities for the new workflow
-- first delete any that no longer exist these should really be mapped

-- update ids to new external activity source ids
update X2.ActiveExternalActivity set ExternalActivityID = EANew.ID, WorkFlowID = @NewWorkflowID
	from
		X2.ActiveExternalActivity AEA
	inner join
		X2.ExternalActivity EAOld
	on
		EAOld.ID = AEA.ExternalActivityID
	inner join
		X2.ExternalActivity EANew
	on
		EANew.[Name] = EAOld.[Name]
	where 
		EAOld.workflowid = @OldWorkflowID
	and
		EANew.workflowid = @NewWorkflowID
	and
		AEA.WorkFlowProviderName = ''

-- fix scheduled activities for the new workflow
-- first delete any that no longer exist these should really be mapped

--OLD - 
--update X2.ScheduledActivity set ActivityID = ANew.ID
--	from
--		X2.ScheduledActivity SA
--	inner join
--		X2.Activity AOld
--	on
--		AOld.ID = SA.ActivityID
--	inner join
--		X2.state AOld_state
--	on
--		AOld_state.id = AOld.stateid
--	inner join
--		X2.Activity ANew
--	on
--		ANew.[Name] = AOld.[Name]
--	inner join
--		X2.state ANew_state
--	on
--		ANew_state.id = ANew.stateid
--	and
--		ANew_state.name = AOld_state.name
--	where 
--		AOld.workflowid = @OldWorkflowID
--	and
--		ANew.workflowid = @NewWorkflowID
--	and
--		SA.WorkFlowProviderName = ''

update X2.ScheduledActivity set ActivityID = ANew.ID
	from X2.ScheduledActivity SA
	inner join X2.Activity AOld on AOld.ID = SA.ActivityID
	inner join #PublishedStateMapping psm ON AOld.stateID = psm.OldStateID
	inner join X2.Activity ANew ON psm.NewStateID = ANew.StateID and AOld.X2ID = ANew.X2ID
	where 
		SA.WorkFlowProviderName = ''

--update X2.ScheduledActivity set ActivityID = ANew.ID
--	from
--		X2.ScheduledActivity SA
--	inner join
--		X2.Activity AOld
--	on
--		AOld.ID = SA.ActivityID
--	inner join
--		X2.Activity ANew
--	on
--		ANew.[Name] = AOld.[Name]
--	where 
--		AOld.workflowid = @OldWorkflowID
--	and
--		ANew.workflowid = @NewWorkflowID
--	and
--		SA.WorkFlowProviderName = ''

-- fix the state ids on the instance table for the new workflow

--OLD INSTANCE MIGRATION
--UPDATE X2.Instance
--SET StateID = SMap.NewStateID,WorkflowID = @NewWorkFlowID
--FROM X2.Instance I
--INNER JOIN (
--SELECT SNew.ID AS NewStateID,SOld.ID AS OldStateID
--FROM X2.STATE SNew
--LEFT OUTER JOIN X2.STATE SOld ON SOld.[Name] = SNew.[Name]
--WHERE SOld.workflowid = @OldWorkflowID AND SNew.workflowid = @NewWorkflowID
--UNION
--SELECT SNew.ID AS NewStateID,SOld.ID AS OldStateID
--FROM X2.STATE SOld
--INNER JOIN #PublishedStateMapping MAP ON SOld.ID = Map.OldStateID
--INNER JOIN X2.STATE SNew ON SNew.ID = Map.NewStateID
--WHERE SOld.workflowid = @OldWorkflowID AND SNew.workflowid = @NewWorkflowID
--) SMAP ON I.StateID = SMAP.OldStateID

update Ins 
set Ins.StateID = SMAP.NewStateID, Ins.WorkflowID = @NewWorkFlowID
from X2.Instance Ins
inner join #PublishedStateMapping SMAP
on Ins.StateID = SMAP.OldStateID and Ins.WorkflowID = @OldWorkflowID


-- fix the instance activity security for the new workflow
--update X2.InstanceActivitySecurity set ActivityID = ANew.ID
--	from
--		X2.InstanceActivitySecurity IAS
--	inner join
--		X2.Activity AOld
--	on
--		AOld.ID = IAS.ActivityID
--	inner join
--		X2.Activity ANew
--	on
--		ANew.[Name] = AOld.[Name]
--	where 
--		AOld.workflowid = @OldWorkflowID
--	and
--		ANew.workflowid = @NewWorkflowID
update IAS set ActivityID = ANew.ID
	from
		X2.InstanceActivitySecurity IAS
	inner join
		X2.Activity AOld
	on
		AOld.workflowid = @OldWorkflowID and AOld.ID = IAS.ActivityID
	inner join 
		X2.State stateOld
	on
		stateOld.ID = AOld.StateID 
	inner join
		X2.Activity ANew
	on
		ANew.workflowid = @NewWorkflowID and ANew.[X2ID] = AOld.[X2ID]
	inner join
		X2.State stateNew
	on
		stateNew.ID = ANew.StateID 
	where stateNew.Name = stateOld.Name

DROP TABLE #PublishedStateMapping
