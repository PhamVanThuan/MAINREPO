--select * from x2.x2.workflow

declare @old_workflowid int
declare @workflow_name varchar(50)

set @workflow_name = 'Application Capture'
set @old_workflowid=451; --463
/*
445	Readvance Payments
446	Quick Cash
447	Credit
448	Valuations
449	Release And Variations
450	Application Management
451	Application Capture
*/

with currentworkflow as (
select a.name, a.priority, s.name StateName
from x2.x2.activity a
join x2.x2.state s on a.stateid=s.id
where a.workflowid=(
select max(id) from x2.x2.workflow w
where name = @workflow_name
)
)

select a.name, a.priority, s.Name, cw.name, cw.priority, cw.StateName
from x2.x2.activity a
join x2.x2.state s on a.stateid=s.id
join currentworkflow cw on a.name=cw.name and  s.Name = cw.StateName
where a.workflowid=(
@old_workflowid
)
and cw.priority<>a.priority



