USE [2AM]
GO

IF EXISTS (select * from dbo.sysobjects where id = object_id(N'test.vPDAOrgStructure'))
Begin
	Drop view test.vPDAOrgStructure
	Print 'Dropped view test.vPDAOrgStructure'
End
Go

/****** Object:  View [dbo].[vOpenLoansBasic]    Script Date: 11/10/2010 10:14:05 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


Create view  [test].[vPDAOrgStructure]
as
/*******************************************************************
Description: Flatten out the Organisation Structure related to Payment Distribution Agencies / Agents 
History:
	2010/11/10	Andrewk			Created
						

*******************************************************************/
--declare		@OrganisationStructureKey int,
--				@maxDepth int 
				
--set @OrganisationStructureKey = 4001
--set @maxDepth = 10;

with SubTree as ( 
				Select	os.organisationstructurekey, os.ParentKey, 
							CASE le.legalentitytypekey
								WHEN 1 THEN le.RegisteredName
								WHEN 2 THEN dbo.LegalEntityLegalName(le.legalentitykey, 0)
								WHEN 3 THEN le.RegisteredName
								ELSE ''
							END as DisplayName,
							os.OrganisationTypeKey
				from OrganisationStructure os
							left join legalentityorganisationstructure leos on os.organisationstructurekey = leos.organisationstructurekey
							left join legalentity le on leos.legalentitykey = le.legalentitykey 
				where	os.generalstatuskey = 1
				)  
	Select	os2.DisplayName as Tier1, os2.OrganisationTypeKey as OrganisationTypeKey1, 
				os3.DisplayName as Tier2,  os3.OrganisationTypeKey as OrganisationTypeKey2, 
				os4.DisplayName as Tier3,  os4.OrganisationTypeKey as OrganisationTypeKey3, 
				os5.DisplayName as Tier4,  os5.OrganisationTypeKey as OrganisationTypeKey4, 
				os6.DisplayName as Tier5,  os6.OrganisationTypeKey as OrganisationTypeKey5, 
				os7.DisplayName as Tier6,  os7.OrganisationTypeKey as OrganisationTypeKey6, 
				os8.DisplayName as Tier7,  os8.OrganisationTypeKey as OrganisationTypeKey7, 
				os9.DisplayName as Tier8,  os9.OrganisationTypeKey as OrganisationTypeKey8, 
				os10.DisplayName as Tier9,  os10.OrganisationTypeKey as OrganisationTypeKey9, 
				os11.DisplayName as Tier10,  os11.OrganisationTypeKey as OrganisationTypeKey10 
	from	(select * 
			from	subtree 
			where	organisationstructurekey = 4100) as os1 left join 
				subtree as os2 on os1.organisationstructurekey = os2.parentkey left join 
				subtree as os3 on os2.organisationstructurekey = os3.parentkey left join 
				subtree as os4 on os3.organisationstructurekey = os4.parentkey left join 
				subtree as os5 on os4.organisationstructurekey = os5.parentkey left join 
				subtree as os6 on os5.organisationstructurekey = os6.parentkey left join 
				subtree as os7 on os6.organisationstructurekey = os7.parentkey left join 
				subtree as os8 on os7.organisationstructurekey = os8.parentkey left join 
				subtree as os9 on os8.organisationstructurekey = os9.parentkey left join 
				subtree as os10 on os9.organisationstructurekey = os10.parentkey left join 
				subtree as os11 on os10.organisationstructurekey = os11.parentkey 
	--Order By 2, 4, 6, 8, 10, 12, 14, 16, 18, 20 
go