USE [2AM]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF EXISTS (select * from dbo.sysobjects where id = object_id(N'[test].[fGetNextRRAssignmentByORTKey]') and xtype='FN')
Begin
	Drop Function [test].[fGetNextRRAssignmentByORTKey]
	Print 'Dropped function [test].[fGetNextRRAssignmentByORTKey]'
End
Go

/****** FUNCTION [test].[GetNextRRAssignmentByORTKey]    Script Date: 04/20/2011 11:10:30 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE FUNCTION [test].[fGetNextRRAssignmentByORTKey]
(
@OfferRoleTypeKey int
)
RETURNS VARCHAR(250)

AS
BEGIN
declare @max_index int
declare @current_index int
declare @min_index int
declare @adusername varchar(250)
declare @users table (ord int, username varchar(50), mapkey int, offerroletypekey int, aduserkey int, aduserstatus int)
declare @assigned table (indexid int, description varchar(50), ord int, username varchar(50), offerroletypekey int, aduserkey int)

insert into @users
select row_number() over (partition by OfferRoleTypeKey order by a.aduserkey) as ord,
a.adusername, map.OfferRoleTypeOrganisationStructureMappingKey as mapkey, map.OfferRoleTypeKey, a.aduserkey, a.generalstatuskey
from aduser a
join userorganisationstructure uos on a.aduserkey=uos.aduserkey
join offerroletypeorganisationstructuremapping map 
on uos.organisationstructurekey=map.organisationstructurekey
where map.OfferRoleTypeKey=@OfferRoleTypeKey
order by OfferRoleTypeKey, a.adUserKey

insert into @assigned
select roundrobinpointerindexid, description, ord, username, offerroletypekey, aduserkey 
from roundrobinpointer r
join roundrobinpointerdefinition rd on r.roundrobinpointerkey=rd.roundrobinpointerkey
left join @users u on rd.generickey=u.mapkey and roundrobinpointerindexid=ord
where u.username is not null

--we need to find the max index so that we know if we are at the end of the list
select @max_index = (select max(ord)
from @users u 
join aduser ad on u.username=ad.adusername
join userorganisationstructure uos on ad.aduserkey=uos.aduserkey
join userorganisationstructureroundrobinstatus stat 
on uos.userorganisationstructurekey=stat.userorganisationstructurekey)

--this is the person who is first eligible user in the list
select @min_index = (select min(ord)
from @users u 
join aduser ad on u.username=ad.adusername
join userorganisationstructure uos on ad.aduserkey=uos.aduserkey
join userorganisationstructureroundrobinstatus stat 
on uos.userorganisationstructurekey=stat.userorganisationstructurekey)

--get the current index
select @current_index = indexid  
from @assigned  
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
	
			select @aduserkey = u.aduserkey
			from @users u
			join aduser ad on u.username=ad.adusername
			join userorganisationstructure uos on ad.aduserkey=uos.aduserkey
			join userorganisationstructureroundrobinstatus stat 
			on uos.userorganisationstructurekey=stat.userorganisationstructurekey
			where u.ord=@current_index 	
	
			if (Select generalstatuskey from AdUser where AdUserKey=@aduserkey) = 2 OR
			(select stat.generalStatusKey from aduser ad
			join userorganisationstructure uos on ad.aduserkey=uos.aduserkey
			join offerRoleTypeOrganisationStructureMapping map on uos.organisationStructureKey=map.organisationStructureKey
			join roundrobinpointerdefinition defn on map.offerRoleTypeOrganisationStructureMappingKey=defn.GenericKey
			join userorganisationstructureroundrobinstatus stat on uos.userorganisationstructurekey=stat.userorganisationstructurekey
			where map.offerRoleTypeKey=@offerRoleTypeKey and ad.aduserkey=@aduserkey) = 2
				begin
					set @isValid = 0
				end
			else
				begin
				set @isValid = 1
						select @adusername = u.username
						from @assigned a 
						join @users u on a.offerroletypekey=u.offerroletypekey
						join aduser ad on u.username=ad.adusername
						join userorganisationstructure uos on ad.aduserkey=uos.aduserkey
						join userorganisationstructureroundrobinstatus stat 
						on uos.userorganisationstructurekey=stat.userorganisationstructurekey
						where u.ord=@current_index 
				end
		end

return @adusername

END


