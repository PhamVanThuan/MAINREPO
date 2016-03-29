USE [2AM]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF EXISTS (select * from dbo.sysobjects where id = object_id(N'[test].[GetNextRRAssignmentByWRTKey]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
Begin
	Drop Procedure [test].[GetNextRRAssignmentByWRTKey]
	Print 'Dropped procedure [test].[GetNextRRAssignmentByWRTKey]'
End
Go

CREATE PROCEDURE [test].[GetNextRRAssignmentByWRTKey]

@WorkflowRoleTypeKey int,
@RoundRobinPointerKey int

AS
declare @max_index int
declare @current_index int
declare @min_index int

create table #users 
(
ord int,
username varchar(50),
mapkey int,
workflowRoleTypeKey int,
aduserkey int,
aduserstatus int
)

create table #assigned
(
indexid int,
description varchar(50),
ord int,
username varchar(50),
workflowRoleTypeKey int,
aduserkey int
)

insert into #users
select row_number() over (partition by WorkflowRoleTypeKey order by a.aduserkey) as ord,
a.adusername, map.WorkflowRoleTypeOrganisationStructureMappingKey as mapkey, map.WorkflowRoleTypeKey, a.aduserkey, a.generalstatuskey
from aduser a
join userorganisationstructure uos on a.aduserkey=uos.aduserkey
join workflowRoletypeorganisationstructuremapping map 
on uos.organisationstructurekey=map.organisationstructurekey
where map.WorkflowRoleTypeKey=@WorkflowRoleTypeKey and uos.generickey = @WorkflowRoleTypeKey
and uos.generickeyTypeKey = 34 and a.generalstatuskey=1
order by workflowRoleTypeKey, a.adUserKey

insert into #assigned
select roundrobinpointerindexid, description, ord, username, WorkflowRoleTypeKey, aduserkey 
from roundrobinpointer r
join roundrobinpointerdefinition rd on r.roundrobinpointerkey=rd.roundrobinpointerkey
left join #users u on rd.generickey=u.mapkey and roundrobinpointerindexid=ord
where u.username is not null and r.roundrobinpointerkey=@RoundRobinPointerKey

--we need to find the max index so that we know if we are at the end of the list
select @max_index = (select max(ord)
from #users u 
join aduser ad on u.username=ad.adusername
join userorganisationstructure uos on ad.aduserkey=uos.aduserkey
join userorganisationstructureroundrobinstatus stat 
on uos.userorganisationstructurekey=stat.userorganisationstructurekey)

--this is the person who is first eligible user in the list
select @min_index = (select min(ord)
from #users u 
join aduser ad on u.username=ad.adusername
join userorganisationstructure uos on ad.aduserkey=uos.aduserkey
join userorganisationstructureroundrobinstatus stat 
on uos.userorganisationstructurekey=stat.userorganisationstructurekey)

--get the current index
select @current_index = indexid  
from #assigned  
--if we are not at the end of the list then fetch the next person
declare @isValid bit
set @isValid = 0
declare @aduserkey int

while (@isValid = 0)
	begin

		if (@current_index < @max_index)
			begin
				set @current_index = @current_index + 1
			end
		else
			begin
				set @current_index = @min_index
			end
	
			print @current_index
			select @aduserkey = u.aduserkey
			from #users u
			join aduser ad on u.username=ad.adusername
			join userorganisationstructure uos on ad.aduserkey=uos.aduserkey
			join userorganisationstructureroundrobinstatus stat 
			on uos.userorganisationstructurekey=stat.userorganisationstructurekey
			where u.ord=@current_index 	
	
			if (Select generalstatuskey from AdUser where AdUserKey=@aduserkey) = 2 OR
			(select stat.generalStatusKey from aduser ad
			join userorganisationstructure uos on ad.aduserkey=uos.aduserkey
			join workflowRoleTypeOrganisationStructureMapping map on uos.organisationStructureKey=map.organisationStructureKey
			join roundrobinpointerdefinition defn on map.workflowRoleTypeOrganisationStructureMappingKey=defn.GenericKey
			and defn.roundRobinPointerKey = @RoundRobinPointerKey
			join userorganisationstructureroundrobinstatus stat on uos.userorganisationstructurekey=stat.userorganisationstructurekey
			where map.WorkflowRoleTypeKey=@workflowRoleTypeKey and ad.aduserkey=@aduserkey) = 2
				begin
					set @isValid = 0
				end
			else
				begin
				set @isValid = 1
						select u.*
						from #assigned a 
						join #users u on a.workflowRoleTypeKey=u.workflowRoleTypeKey
						join aduser ad on u.username=ad.adusername
						join userorganisationstructure uos on ad.aduserkey=uos.aduserkey
						join userorganisationstructureroundrobinstatus stat 
						on uos.userorganisationstructurekey=stat.userorganisationstructurekey
						where u.ord=@current_index 
				end
		end

drop table #users
drop table #assigned



GO


SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO

