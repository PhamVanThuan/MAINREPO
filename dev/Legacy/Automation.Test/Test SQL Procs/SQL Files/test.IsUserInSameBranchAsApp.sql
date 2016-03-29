USE [2AM]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF EXISTS (select * from dbo.sysobjects where id = object_id(N'test.IsUserInSameBranchAsApp') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
Begin
	Drop Procedure test.IsUserInSameBranchAsApp
	Print 'Dropped Proc test.IsUserInSameBranchAsApp'
End
Go

CREATE PROCEDURE test.IsUserInSameBranchAsApp

@ADUserName VARCHAR(50),
@OfferKey INT

AS



IF EXISTS
(select ad.*  from [2am].[dbo].ADUser ad (nolock)  
inner join [2am].[dbo].UserOrganisationStructure uos (nolock)   
on uos.ADUserKey = ad.ADUserKey  
inner join [2am].[dbo].OfferRoleTypeOrganisationStructureMapping ortosm (nolock)   
on uos.OrganisationStructureKey = ortosm.OrganisationStructureKey  
where ad.adusername = @ADUserName and ortosm.OfferRoleTypeKey in (101,102,103))  

BEGIN   

;with OSParentKeys(ParentKey) as    
(select  ISNULL(os.ParentKey,-1) as ParentKey   
from [2am].[dbo].offerRole ofr (nolock)   
join [2am].[dbo].aduser ad (nolock)   
on ofr.LegalEntityKey = ad.LegalEntityKey  
join [2am].[dbo].UserOrganisationStructure uos (nolock)    
on ad.ADUserKey = uos.ADUserKey   
join [2am].[dbo].OrganisationStructure os (nolock)    
on os.OrganisationStructureKey = uos.OrganisationStructureKey   
where ofr.offerKey = @OfferKey 
and ofr.GeneralStatusKey = 1 
and ofr.offerRoleTypeKey in (101,102,103))   

select count(*) from [2am].[dbo].ADUser ad (nolock)   
inner join [2am].[dbo].UserOrganisationStructure uos (nolock)    
on uos.ADUserKey = ad.ADUserKey   
inner join [2am].[dbo].OrganisationStructure os (nolock)   
 on uos.OrganisationStructureKey = os.OrganisationStructureKey   
 inner join OSParentKeys ospk    
 on ospk.ParentKey = isnull(os.ParentKey,0)   
 where ad.adusername = @ADUserName  
 END  
 ELSE   
 SELECT 1  