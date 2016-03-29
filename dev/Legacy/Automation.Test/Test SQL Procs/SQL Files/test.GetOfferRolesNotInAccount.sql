USE [2AM]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF EXISTS (select * from dbo.sysobjects where id = object_id(N'test.GetOfferRolesNotInAccount') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
Begin
	Drop Procedure test.GetOfferRolesNotInAccount
	Print 'Dropped Proc test.GetOfferRolesNotInAccount'
End
Go

CREATE PROCEDURE test.GetOfferRolesNotInAccount

@OfferKey int

AS

BEGIN

IF EXISTS (
select distinct ofr.LegalEntityKey,ofr.OfferRoleTypeKey,rt.RoleTypeKey,o.AccountKey  
from [2am].[dbo].offer o (nolock)  
inner join [2am].[dbo].offerRole ofr (nolock)   
on ofr.offerKey = o.offerKey  
inner join [2am].[dbo].offerRoleType ort (nolock)   
on ofr.OfferRoleTypeKey = ort.OfferRoleTypeKey  
inner join [2am].[dbo].RoleType rt (nolock)   
on ort.description = rt.description 
 left join [2am].[dbo].role r (nolock)   
 on ofr.legalEntityKey = r.legalEntityKey 
 and o.accountKey = r.accountKey 
 and rt.RoleTypeKey = r.RoleTypeKey  
 where ofr.offerKey = @offerKey
  and ort.OfferRoleTypeGroupKey = 3 
  and r.LegalEntityKey is null
  )
	BEGIN
		SELECT 1
	END
  
  ELSE
	BEGIN
		SELECT 0
	END

END