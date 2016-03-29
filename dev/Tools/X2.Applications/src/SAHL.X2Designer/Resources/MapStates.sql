/*
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

update X2.ScheduledActivity set ActivityID = ANew.ID
	from
		X2.ScheduledActivity SA
	inner join
		X2.Activity AOld
	on
		AOld.ID = SA.ActivityID
	inner join
		X2.state AOld_state
	on
		AOld_state.id = AOld.stateid
	inner join
		X2.Activity ANew
	on
		ANew.[Name] = AOld.[Name]
	inner join
		X2.state ANew_state
	on
		ANew_state.id = ANew.stateid
	and
		ANew_state.name = AOld_state.name
	where 
		AOld.workflowid = @OldWorkflowID
	and
		ANew.workflowid = @NewWorkflowID
	and
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
update X2.Instance set StateID = SMap.NewStateID, WorkflowID = @NewWorkFlowID 
	from 
		X2.Instance I
	inner join
	(
		select SNew.ID as NewStateID, SOld.ID as OldStateID
			from X2.State SNew
		left outer join
			X2.State SOld
		on
			SOld.[Name] = SNew.[Name]
		where 
			SOld.workflowid = @OldWorkflowID
		and
			SNew.workflowid = @NewWorkflowID
		union
		select SNew.ID as NewStateID, SOld.ID as OldStateID
			from X2.State SOld
		inner join
			x2.PublishedStateMapping MAP
		on
			SOld.ID = Map.OldStateID
		inner join
			X2.State SNew
		on
			SNew.ID = Map.NewStateID
		where 
			SOld.workflowid = @OldWorkflowID
		and
			SNew.workflowid = @NewWorkflowID
		)
		SMAP
	on
		I.StateID = SMAP.OldStateID
		
		

-- fix the instance activity security for the new workflow
update X2.InstanceActivitySecurity set ActivityID = ANew.ID
	from
		X2.InstanceActivitySecurity IAS
	inner join
		X2.Activity AOld
	on
		AOld.ID = IAS.ActivityID
	inner join
		X2.Activity ANew
	on
		ANew.[Name] = AOld.[Name]
	where 
		AOld.workflowid = @OldWorkflowID
	and
		ANew.workflowid = @NewWorkflowID


