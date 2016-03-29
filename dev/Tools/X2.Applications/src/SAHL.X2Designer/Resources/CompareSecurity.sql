/*
declare @OldProcessID int
declare @NewProcessID int

set @NewProcessID = 49
set @OldProcessID = 46
*/


CREATE TABLE #INSTANCESTODEL(InstanceID bigint)
INSERT INTO #INSTANCESTODEL 
-- get instances that could possibly be affected by added activities
select distinct I.ID from X2.Instance I
inner join
	X2.Workflow WF
on
	I.workflowid = WF.ID
and
	WF.processid in(@NewProcessID, @OldProcessID)
inner join
(
	-- find any added activities
	select A.StateID from X2.Activity A 
	inner join
		X2.Workflow WF
	on
		A.workflowid = WF.ID
	and
		WF.processid = @NewProcessID
	where 
		A.[Name] not in 
	(
	select B.[Name] from X2.Activity B 
	inner join
		X2.Workflow WF1
	on
		B.workflowid = WF1.ID
	and
		WF1.processid = @OldProcessID
	)
) ADDACT
on
	ADDACT.StateID = I.StateID

union

-- get instances that could possibly be affected by removed activities
select distinct I.ID from X2.Instance I
inner join
	X2.Workflow WF
on
	I.workflowid = WF.ID
and
	WF.processid in(@NewProcessID, @OldProcessID)
inner join
(
	-- find any removed activities
	select A.StateID from X2.Activity A 
	inner join
		X2.Workflow WF
	on
		A.workflowid = WF.ID
	and
		WF.processid = @OldProcessID
	where 
		A.[Name] not in 
	(
	select B.[Name] from X2.Activity B 
	inner join
		X2.Workflow WF1
	on
		B.workflowid = WF1.ID
	and
		WF1.processid = @NewProcessID
	)
) ADDACT
on
	ADDACT.StateID = I.StateID

union

-- get instances that could possibly be affected by activities that have had security added
select distinct I.ID from X2.Instance I
inner join
	X2.Workflow WF
on
	I.workflowid = WF.ID
and
	WF.processid in(@NewProcessID, @OldProcessID)
inner join
(
	select A.StateID from X2.ActivitySecurity ASec
	inner join
		X2.Activity A
	on
		ASec.ActivityID = A.ID
	inner join
		X2.Workflow wf
	on
		wf.id = A.Workflowid
	and
		wf.processid = @NewProcessID
	inner join
		X2.SecurityGroup SG
	on
		ASec.SecurityGroupID = SG.ID
	where
		SG.[Name] not in 
	(
		select SG1.[Name] from X2.ActivitySecurity ASec1
		inner join
			X2.Activity A1
		on
			ASec1.ActivityID = A1.ID
		inner join
			X2.Workflow wf1
		on
			wf1.id = A1.Workflowid
		and
			wf1.processid = @OldProcessID
		inner join
			X2.SecurityGroup SG1
		on
			ASec1.SecurityGroupID = SG1.ID
	)
) ADDSEC
on
	ADDSEC.StateID = I.StateID


union

-- get instances that could possibly be affected by activities that have had security removed
select distinct I.ID from X2.Instance I
inner join
	X2.Workflow WF
on
	I.workflowid = WF.ID
and
	WF.processid in(@NewProcessID, @OldProcessID)
inner join
(
	select A.StateID from X2.ActivitySecurity ASec
	inner join
		X2.Activity A
	on
		ASec.ActivityID = A.ID
	inner join
		X2.Workflow wf
	on
		wf.id = A.Workflowid
	and
		wf.processid = @OldProcessID
	inner join
		X2.SecurityGroup SG
	on
		ASec.SecurityGroupID = SG.ID
	where
		SG.[Name] not in 
	(
		select SG1.[Name] from X2.ActivitySecurity ASec1
		inner join
			X2.Activity A1
		on
			ASec1.ActivityID = A1.ID
		inner join
			X2.Workflow wf1
		on
			wf1.id = A.Workflowid
		and
			wf1.processid = @NewProcessID
		inner join
			X2.SecurityGroup SG1
		on
			ASec1.SecurityGroupID = SG1.ID
	)
) ADDSEC
on
	ADDSEC.StateID = I.StateID

union

-- get instances where the worklist has been added to
select distinct I.ID from X2.Instance I
inner join
	X2.Workflow WF
on
	I.workflowid = WF.ID
and
	WF.processid in(@NewProcessID, @OldProcessID)
inner join
(
	select S.ID as StateID from X2.StateWorkList SWL
	inner join
		X2.SecurityGroup SG
	on
		SWL.SecurityGroupID = SG.ID
	inner join
		X2.State S
	on
		S.ID = SWL.StateID
	inner join
		X2.WorkFlow wf
	on
		wf.id = S.workflowid
		and
			wf.processid = @NewProcessID
	where
		SG.[Name] not in 
	(
	select SG1.[Name] from X2.StateWorkList SWL1
	inner join
		X2.SecurityGroup SG1
	on
		SWL1.SecurityGroupID = SG1.ID
	inner join
		X2.State S1
	on
		S1.ID = SWL1.StateID
	inner join
		X2.WorkFlow wf1
	on
		wf1.id = S1.workflowid
		and
			wf1.processid = @OldProcessID
	)
) ADDSEC
on
	ADDSEC.StateID = I.StateID

union

-- get instances where the worklist has been removed from
select distinct I.ID from X2.Instance I
inner join
	X2.Workflow WF
on
	I.workflowid = WF.ID
and
	WF.processid in(@NewProcessID, @OldProcessID)
inner join
(
	select S.ID as StateID from X2.StateWorkList SWL
	inner join
		X2.SecurityGroup SG
	on
		SWL.SecurityGroupID = SG.ID
	inner join
		X2.State S
	on
		S.ID = SWL.StateID
	inner join
		X2.WorkFlow wf
	on
		wf.id = S.workflowid
		and
			wf.processid = @OldProcessID
	where
		SG.[Name] not in 
	(
	select SG1.[Name] from X2.StateWorkList SWL1
	inner join
		X2.SecurityGroup SG1
	on
		SWL1.SecurityGroupID = SG1.ID
	inner join
		X2.State S1
	on
		S1.ID = SWL1.StateID
	inner join
		X2.WorkFlow wf1
	on
		wf1.id = S1.workflowid
		and
			wf1.processid = @NewProcessID
	)
) ADDSEC
on
	ADDSEC.StateID = I.StateID

select * from #INSTANCESTODEL