USE [2AM]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF EXISTS (select * from dbo.sysobjects where id = object_id(N'test.GetRoundRobinPointerAssignment') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
Begin
	Drop Procedure test.GetRoundRobinPointerAssignment 
	Print 'Dropped procedure test.GetRoundRobinPointerAssignment '
End
Go


CREATE PROCEDURE test.GetRoundRobinPointerAssignment 

@OfferRoleTypeKey INT

AS

create table #users 
(
ord int,
username varchar(50),
mapkey int,
offerroletypekey int
)

create table #assigned
(
indexid int,
description varchar(50),
ord int,
username varchar(50),
offerroletypekey int
)

insert into #users
select row_number() over (partition by OfferRoleTypeOrganisationStructureMappingKey order by a.aduserkey) as ord,
a.adusername, map.OfferRoleTypeOrganisationStructureMappingKey as mapkey, map.OfferRoleTypeKey
from aduser a
join userorganisationstructure uos on a.aduserkey=uos.aduserkey
join offerroletypeorganisationstructuremapping map 
on uos.organisationstructurekey=map.organisationstructurekey
order by OfferRoleTypeOrganisationStructureMappingKey, a.adUserKey

insert into #assigned
select roundrobinpointerindexid, description, ord, username, offerroletypekey 
from roundrobinpointer r
join roundrobinpointerdefinition rd on r.roundrobinpointerkey=rd.roundrobinpointerkey
left join #users u on rd.generickey=u.mapkey and roundrobinpointerindexid=ord
where u.username is not null

select * from #assigned
where OfferRoleTypeKey = @OfferRoleTypeKey

select a.description, u.ord, u.username,
case when ad.generalstatuskey = 1 then 'YES'
else 'NO' end 
as WFAStatus,
case when stat.generalstatuskey = 1 AND ad.generalstatuskey = 1 then 'YES'
else 'NO' end
as RoundRobinStatus
from #assigned a 
join #users u on a.offerroletypekey=u.offerroletypekey
join aduser ad on u.username=ad.adusername
join userorganisationstructure uos on ad.aduserkey=uos.aduserkey
join userorganisationstructureroundrobinstatus stat 
on uos.userorganisationstructurekey=stat.userorganisationstructurekey
where a.OfferRoleTypeKey = @OfferRoleTypeKey
and ad.generalstatuskey=1
group by a.description, u.username, u.ord, ad.generalstatuskey, stat.generalstatuskey 
order by 1,2

drop table #users
drop table #assigned
