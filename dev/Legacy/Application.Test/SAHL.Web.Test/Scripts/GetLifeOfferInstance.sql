declare @WorkflowID int
declare @InstanceID bigint
declare @BusinessKey int

select @WorkflowID = wf.ID from X2.X2.Workflow wf 
inner join
	(
		select max(ID) as ID from X2.X2.Workflow wf2 group by name
	) wf2
on
	wf.id = wf2.id
where 
	wf.name = 'LifeOrigination'

select top 1 wl.InstanceID from X2.X2.WorkList wl
inner join
	X2.X2.Instance I
on
	I.ID = wl.InstanceID
and
	I.WorkflowID = @WorkflowID
and
	I.ParentInstanceId is null
inner join
	X2.X2.State S
on
	S.ID = I.StateID
and
	S.Type <> 5
where wl.ADUSerName = 'snowcrash\eugened'

