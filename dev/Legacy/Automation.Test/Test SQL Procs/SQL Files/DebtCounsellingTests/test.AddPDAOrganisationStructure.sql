USE [2AM]
GO

/****** Object:  StoredProcedure [test].[AddPDAOrganisationStructure]    Script Date: 01/20/2011 10:00:25 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER OFF
GO

if exists (select * from dbo.sysobjects where id = object_id(N'test.AddPDAOrganisationStructure') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
begin
	drop procedure test.AddPDAOrganisationStructure
	print 'Dropped procedure test.AddPDAOrganisationStructure'
end
GO

create procedure [test].[AddPDAOrganisationStructure]
(
					@LENames varchar(100),
					@Surname varchar(100),
					@OrgTypekey int,
					@OrgStructureKey int Output
)
as

---------------------------------------------------
-- Insert Company, Branch or Department into organisation structure
---------------------------------------------------
/*
begin tran test
declare @LENames varchar(100),
					@Surname varchar(100),
					@OrgTypekey int,
					@OrgStructureKey int
					
set @LENames = 'Test'
set @Surname = 'Test'
set @OrgTypekey = '1'
*/
					
if (@OrgTypekey = 1)
	Select @OrgStructureKey = ControlNumeric from [2am].dbo.[Control] where ControlDescription = 'PaymentDistributionAgenciesRoot'

if (@OrgTypekey in (1, 3, 5))
begin
	if not exists(select 1 from [2am].dbo.OrganisationStructure where ParentKey = @OrgStructureKey 
					and [Description] = @LENames 
					and OrganisationTypeKey = @OrgTypeKey)
	begin
		create table #new ( OrganisationStructureKey int not null)

		insert into [2am].dbo.OrganisationStructure --(ParentKey, [Description], OrganisationTypeKey, GeneralStatusKey) 
			output inserted.OrganisationStructureKey into #new
				select @OrgStructureKey, @LENames, @OrgTypeKey, 1
			
		select @OrgStructureKey = OrganisationStructureKey from #new
		drop table #new
	end
	else
	begin
		Update  [2am].dbo.OrganisationStructure Set GeneralStatusKey = 1 where ParentKey = @OrgStructureKey 
					and [Description] = @LENames 
					and OrganisationTypeKey = @OrgTypeKey 
					
		Select @OrgStructureKey = OrganisationStructureKey from [2am].dbo.OrganisationStructure where ParentKey = @OrgStructureKey 
					and [Description] = @LENames 
					and OrganisationTypeKey = @OrgTypeKey 
	end
	
	execute test.AddPDALegalEntities 
						@LENames = @LENames,
						@Surname = '',
						@OrgStructureKey = @OrgStructureKey
end
---------------------------------------------------
-- Insert Contact into organisation structure
---------------------------------------------------
if (@OrgTypekey = 7)
begin
	if not exists (select 1 from [2am].dbo.OrganisationStructure os
						join legalentityorganisationstructure leos on os.organisationstructurekey = leos.organisationstructurekey
						join legalentity le on leos.legalentitykey = le.legalentitykey
							where os.ParentKey = @OrgStructureKey 
									and os.[Description] = 'Contact' 
									and os.OrganisationTypeKey = @OrgTypeKey 
									and le.firstnames = @LENames and le.Surname = @Surname) 		
	begin			
		create table #new1 ( OrganisationStructureKey int not null)
		
		insert into [2AM].[dbo].[OrganisationStructure] --([ParentKey],[Description],[OrganisationTypeKey],[GeneralStatusKey]) 
			output inserted.OrganisationStructureKey into #new1
				select @OrgStructureKey, 'Contact', @OrgTypekey, 1
				
		select @OrgStructureKey = OrganisationStructureKey from #new1
		drop table #new1
	end
	else
	begin
		Update os set os.GeneralStatusKey = 1
			 from [2am].dbo.OrganisationStructure os
						join legalentityorganisationstructure leos on os.organisationstructurekey = leos.organisationstructurekey
						join legalentity le on leos.legalentitykey = le.legalentitykey
							where os.ParentKey = @OrgStructureKey 
									and os.[Description] = 'Contact' 
									and os.OrganisationTypeKey = @OrgTypeKey
									and le.firstnames = @LENames and le.Surname = @Surname
									
		select @OrgStructureKey = os.OrganisationStructureKey from [2am].dbo.OrganisationStructure os
						join legalentityorganisationstructure leos on os.organisationstructurekey = leos.organisationstructurekey
						join legalentity le on leos.legalentitykey = le.legalentitykey
							where os.ParentKey = @OrgStructureKey 
									and os.[Description] = 'Contact' 
									and os.OrganisationTypeKey = @OrgTypeKey 
									and le.firstnames = @LENames and le.Surname = @Surname 
	end

	execute test.AddPDALegalEntities 
						@LENames = @LENames,
						@Surname = @Surname,
						@OrgStructureKey = @OrgStructureKey
End

GO


