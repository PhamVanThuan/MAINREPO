USE [2AM]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF EXISTS (select * from dbo.sysobjects where id = object_id(N'[test].[GetCurrentRRUserByORTKey]') and xtype='FN')
Begin
	Drop Function [test].[GetCurrentRRUserByORTKey]
	Print 'Dropped function [test].[GetCurrentRRUserByORTKey]'
End
Go

/****** FUNCTION [test].[GetNextRRAssignmentByORTKey]    Script Date: 04/20/2011 11:10:30 ******/
SET ANSI_NULLS ON
GO

CREATE  FUNCTION [test].[GetCurrentRRUserByORTKey]
(
	@OfferRoleTypeKey int
)
RETURNS varchar(250)
AS
/************************************************
Description: 
************************************************/
BEGIN

DECLARE @adUserName VARCHAR(250);

with users as (
select row_number() over (partition by OfferRoleTypeKey order by a.aduserkey) as ord,
a.adusername, map.OfferRoleTypeOrganisationStructureMappingKey as mapkey, map.OfferRoleTypeKey, a.aduserkey, a.generalstatuskey
from aduser a
join userorganisationstructure uos on a.aduserkey=uos.aduserkey
join offerroletypeorganisationstructuremapping map 
on uos.organisationstructurekey=map.organisationstructurekey
where map.OfferRoleTypeKey=@OfferRoleTypeKey
)

select @adUserName = u.adusername
from roundrobinpointer r
join roundrobinpointerdefinition rd on r.roundrobinpointerkey=rd.roundrobinpointerkey
left join users u on rd.generickey=u.mapkey and roundrobinpointerindexid=ord
where u.adusername is not null


RETURN @adUserName
END

GO


