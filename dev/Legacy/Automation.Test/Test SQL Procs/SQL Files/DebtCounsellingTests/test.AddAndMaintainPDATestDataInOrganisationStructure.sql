USE [2AM]
GO

/****** Object:  StoredProcedure [test].[AddAndMaintainPDATestDataInOrganisationStructure]    Script Date: 01/20/2011 14:17:03 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER OFF
GO

if exists (select * from dbo.sysobjects where id = object_id(N'test.AddAndMaintainPDATestDataInOrganisationStructure') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
begin
	drop procedure test.AddAndMaintainPDATestDataInOrganisationStructure
	print 'Dropped procedure test.AddAndMaintainPDATestDataInOrganisationStructure'
end
GO

create procedure [test].[AddAndMaintainPDATestDataInOrganisationStructure]
as
--begin tran 
--rollback
--commit



---------------------------------------------------
--Add data for Update tests
---------------------------------------------------
Declare @OrgStructureKey int
Select @OrgStructureKey = ControlNumeric from [2am].dbo.[Control] where ControlDescription = 'PaymentDistributionAgenciesRoot'
---------------------------------------------------
-- Insert Company into organisation structure
---------------------------------------------------
execute test.AddPDAOrganisationStructure
					@LENames = 'CompanyToUpdate',
					@Surname = '',
					@OrgTypekey = 1,
					@OrgStructureKey = @OrgStructureKey Output
---------------------------------------------------
-- Insert Branch into organisation structure
---------------------------------------------------
execute test.AddPDAOrganisationStructure
					@LENames = 'BranchToUpdate',
					@Surname = '',
					@OrgTypekey = 3,
					@OrgStructureKey = @OrgStructureKey Output
---------------------------------------------------
-- Insert Department into organisation structure
---------------------------------------------------
execute test.AddPDAOrganisationStructure
					@LENames = 'DepartmentToUpdate',
					@Surname = '',
					@OrgTypekey = 5,
					@OrgStructureKey = @OrgStructureKey Output
---------------------------------------------------
-- Insert Contact into organisation structure
---------------------------------------------------
execute test.AddPDAOrganisationStructure
					@LENames = 'Contact To',
					@Surname = 'Update',
					@OrgTypekey = 7,
					@OrgStructureKey = @OrgStructureKey Output

---------------------------------------------------
--Add data for Remove tests
---------------------------------------------------
Select @OrgStructureKey = ControlNumeric from [2am].dbo.[Control] where ControlDescription = 'PaymentDistributionAgenciesRoot'
---------------------------------------------------
-- Insert Company into organisation structure
---------------------------------------------------
execute test.AddPDAOrganisationStructure
					@LENames = 'CompanyToRemove',
					@Surname = '',
					@OrgTypekey = 1,
					@OrgStructureKey = @OrgStructureKey Output
---------------------------------------------------
-- Insert Branch into organisation structure
---------------------------------------------------
execute test.AddPDAOrganisationStructure
					@LENames = 'BranchToRemove',
					@Surname = '',
					@OrgTypekey = 3,
					@OrgStructureKey = @OrgStructureKey Output
---------------------------------------------------
-- Insert Department into organisation structure
---------------------------------------------------
execute test.AddPDAOrganisationStructure
					@LENames = 'DepartmentToRemove',
					@Surname = '',
					@OrgTypekey = 5,
					@OrgStructureKey = @OrgStructureKey Output
---------------------------------------------------
-- Insert Contact into organisation structure
---------------------------------------------------
execute test.AddPDAOrganisationStructure
					@LENames = 'Contact To',
					@Surname = 'Remove',
					@OrgTypekey = 7,
					@OrgStructureKey = @OrgStructureKey Output

---------------------------------------------------
--Clean data created by previous Add tests
---------------------------------------------------
---------------------------------------------------
--Clean Company PDA details
---------------------------------------------------
update [2am].dbo.OrganisationStructure 
	set GeneralStatusKey = 2 
		where [Description] = 'CompanyAdded'
---------------------------------------------------
--Clean Branch PDA details
---------------------------------------------------
update [2am].dbo.OrganisationStructure 
	set GeneralStatusKey = 2 
		where [Description] = 'BranchAdded'
---------------------------------------------------
--Clean Department PDA details
---------------------------------------------------
update [2am].dbo.OrganisationStructure 
	set GeneralStatusKey = 2 
		where [Description] = 'DepartmentAdded'
---------------------------------------------------
--Clean Contact PDA details
---------------------------------------------------
update os
			set os.GeneralStatuskey = 2
				from OrganisationStructure os
						join legalentityorganisationstructure leos on os.organisationstructurekey = leos.organisationstructurekey
						join legalentity le on leos.legalentitykey = le.legalentitykey
							where le.firstnames = 'Contact' and le.surname = 'Added'
							
---------------------------------------------------
--Add data for Add tests
---------------------------------------------------
Select @OrgStructureKey = ControlNumeric from [2am].dbo.[Control] where ControlDescription = 'PaymentDistributionAgenciesRoot'
---------------------------------------------------
-- Insert Company into organisation structure
---------------------------------------------------
execute test.AddPDAOrganisationStructure
					@LENames = 'CompanyToAddTo',
					@Surname = '',
					@OrgTypekey = 1,
					@OrgStructureKey = @OrgStructureKey Output
---------------------------------------------------
-- Insert Branch into organisation structure
---------------------------------------------------
execute test.AddPDAOrganisationStructure
					@LENames = 'BranchToAddTo',
					@Surname = '',
					@OrgTypekey = 3,
					@OrgStructureKey = @OrgStructureKey Output
---------------------------------------------------
-- Insert Department into organisation structure
---------------------------------------------------
execute test.AddPDAOrganisationStructure
					@LENames = 'DepartmentToAddTo',
					@Surname = '',
					@OrgTypekey = 5,
					@OrgStructureKey = @OrgStructureKey Output

---------------------------------------------------
--Add data for mandatory and optional field validation tests on Update views
---------------------------------------------------
Select @OrgStructureKey = ControlNumeric from [2am].dbo.[Control] where ControlDescription = 'PaymentDistributionAgenciesRoot'
---------------------------------------------------
-- Insert Company into organisation structure
---------------------------------------------------
execute test.AddPDAOrganisationStructure
					@LENames = 'CompanyUpdateValidation',
					@Surname = '',
					@OrgTypekey = 1,
					@OrgStructureKey = @OrgStructureKey Output
---------------------------------------------------
-- Insert Branch into organisation structure
---------------------------------------------------
execute test.AddPDAOrganisationStructure
					@LENames = 'BranchUpdateValidation',
					@Surname = '',
					@OrgTypekey = 3,
					@OrgStructureKey = @OrgStructureKey Output
---------------------------------------------------
-- Insert Department into organisation structure
---------------------------------------------------
execute test.AddPDAOrganisationStructure
					@LENames = 'DepartmentUpdateValidation',
					@Surname = '',
					@OrgTypekey = 5,
					@OrgStructureKey = @OrgStructureKey Output
---------------------------------------------------
-- Insert Contact into organisation structure
---------------------------------------------------
execute test.AddPDAOrganisationStructure
					@LENames = 'Contact',
					@Surname = 'UpdateValidation',
					@OrgTypekey = 7,
					@OrgStructureKey = @OrgStructureKey Output

---------------------------------------------------
--Add data for validation tests on Remove views
---------------------------------------------------
Select @OrgStructureKey = ControlNumeric from [2am].dbo.[Control] where ControlDescription = 'PaymentDistributionAgenciesRoot'
---------------------------------------------------
-- Insert Company into organisation structure
---------------------------------------------------
execute test.AddPDAOrganisationStructure
					@LENames = 'CompanyRemoveParentNodeValidation',
					@Surname = '',
					@OrgTypekey = 1,
					@OrgStructureKey = @OrgStructureKey Output
---------------------------------------------------
-- Insert Branch into organisation structure
---------------------------------------------------
execute test.AddPDAOrganisationStructure
					@LENames = 'BranchRemoveParentNodeValidation',
					@Surname = '',
					@OrgTypekey = 3,
					@OrgStructureKey = @OrgStructureKey Output

---------------------------------------------------
--Add data for adding PDA bank details tests
---------------------------------------------------
--declare @OrgStructureKey varchar(100)
Select @OrgStructureKey = ControlNumeric from [2am].dbo.[Control] where ControlDescription = 'PaymentDistributionAgenciesRoot'
---------------------------------------------------
-- Insert Company into organisation structure
---------------------------------------------------
execute test.AddPDAOrganisationStructure
					@LENames = 'AddPDABankDetails',
					@Surname = '',
					@OrgTypekey = 1,
					@OrgStructureKey = @OrgStructureKey Output

---------------------------------------------------
-- Set all bank details for the legal entity to inactive
---------------------------------------------------
update leba set leba.generalstatuskey = 2 from OrganisationStructure os
							join legalentityorganisationstructure leos on os.organisationstructurekey = leos.organisationstructurekey
							join legalentity le on leos.legalentitykey = le.legalentitykey
							join legalentitybankaccount leba on le.legalentitykey = leba.legalentitykey
where os.OrganisationStructureKey = @OrgStructureKey and le.RegisteredName = 'AddPDABankDetails'
/*
declare	@OrganisationStructureKey int
				
Select @OrganisationStructureKey = ControlNumeric from [2am].dbo.[Control] where ControlDescription = 'PaymentDistributionAgenciesRoot';

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
			where	organisationstructurekey = @OrganisationStructureKey) as os1 left join 
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
GO

select top 10 * from legalentity le (nolock) join
legalentityaddress lea on le.legalentitykey = lea.legalentitykey join
address a on lea.addresskey = a.addresskey 
order by le.legalentitykey desc

select top 10 * from address (nolock) order by addresskey desc

execute test.AddAndMaintainPDATestDataInOrganisationStructure
--begin tran
select * from legalentity where surname = 'UpdateValidation'
*/

GO


