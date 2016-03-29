use [2am]

declare @DeleteLegalEntityAddressFeature varchar(max)
declare @CaptureClientSurveyFeature varchar(max)
declare @FlushCacheFeature varchar(max)
declare @HaloADUserKey int
declare @PMBConsultantOrgStructKey int
declare @ApplicationCalculatorFeature varchar(max)
declare @CalculatorCBOFeature varchar(max)
declare @LeadCaptureFeature varchar(max)
declare @AttorneyDetailsFeature varchar(max)
declare @AddAttorneyDetailsFeature varchar(max)
declare @UpdateAttorneyDetails varchar(max)
declare @EnableUpdateLegalEntity varchar(max)
declare @DeleteAssetLiabilityFeature varchar(max)
declare @UpdatePropertyFeature varchar(max)
declare @PropertyTitleDeedFeature varchar(max)
declare @PropertyAddressFeature varchar(max)
declare @HaloUserAduserKey int
declare @RCSOrganisationStructureKey int
declare @HelpDeskOrganisationStructureKey int

declare @CapLetterFeature varchar(max)
declare @SuperLoIntroductionLetter varchar(max)
declare @BondCancelledLetter varchar(max)

set @DeleteLegalEntityAddressFeature = 'Delete Legal Entity Address'
set @CaptureClientSurveyFeature = 'Capture Client Survey'
set @FlushCacheFeature = 'Admin Flush Cache'
set @HaloADUserKey = (select aduser.aduserkey from dbo.aduser where adusername = 'SAHL\HaloUser')
set @PMBConsultantOrgStructKey = 60
set @ApplicationCalculatorFeature = 'Application Calculator'
set @CalculatorCBOFeature = 'Calculators'
set @LeadCaptureFeature = 'Lead Capture'
set @AttorneyDetailsFeature = 'Attorney Details'
set @AddAttorneyDetailsFeature = 'Add Attorney Details'
set @UpdateAttorneyDetails = 'Update Attorney Details'
set @EnableUpdateLegalEntity = 'Enable Update Legal Entity Details'
set @DeleteAssetLiabilityFeature = 'Delete Legal Entity Asset Liability'
set @UpdatePropertyFeature  = 'Update Property'
set @PropertyTitleDeedFeature = 'Update Property Title Deed'
set @PropertyAddressFeature  = 'Update Property Address'
set @RCSOrganisationStructureKey = 4
set @HelpDeskOrganisationStructureKey = 591

set @CapLetterFeature = 'Cap Letter'
set @SuperLoIntroductionLetter  = 'SuperLo Introduction Letter'
set @BondCancelledLetter = 'Bond Cancelled Letter'

select @HaloUserAduserKey = aduserkey 
from dbo.aduser where adusername = 'SAHL\HaloUser'

--CapLetterFeature CBO Correspondance
if (not exists(select 1 from dbo.featuregroup
					inner join dbo.feature
						on featuregroup.featurekey = feature.featurekey
				where adusergroup = 'HaloUser' and longname = @CapLetterFeature))
begin
	insert into dbo.featuregroup
	select distinct 
		'HaloUser' as ADUserGroup,
		Feature.FeatureKey
	from 
		dbo.feature
	where feature.longname in (@CapLetterFeature)
	print 'Added CapLetterFeature'
end

--SuperLoIntroductionLetter CBO Correspondance
if (not exists(select 1 from dbo.featuregroup
					inner join dbo.feature
						on featuregroup.featurekey = feature.featurekey
				where adusergroup = 'HaloUser' and longname = @SuperLoIntroductionLetter))
begin
	insert into dbo.featuregroup
	select distinct 
		'HaloUser' as ADUserGroup,
		Feature.FeatureKey
	from 
		dbo.feature
	where feature.longname in (@SuperLoIntroductionLetter)
	print 'Added SuperLoIntroductionLetter'
end

--BondCancelledLetter CBO Correspondance
if (not exists(select 1 from dbo.featuregroup
					inner join dbo.feature
						on featuregroup.featurekey = feature.featurekey
				where adusergroup = 'HaloUser' and longname = @BondCancelledLetter))
begin
	insert into dbo.featuregroup
	select distinct 
		'HaloUser' as ADUserGroup,
		Feature.FeatureKey
	from 
		dbo.feature
	where feature.longname in (@BondCancelledLetter)
	print 'Added BondCancelledLetter'
end

--Setup user as an Helpdesk User
if (not exists(select 1 from dbo.userorganisationstructure where aduserkey= @HaloUserAduserKey and organisationstructurekey = @HelpDeskOrganisationStructureKey))
begin
	insert into dbo.userorganisationstructure (aduserkey,organisationstructurekey,generickey,generickeytypekey,generalstatuskey)
	values (@HaloUserAduserKey,@HelpDeskOrganisationStructureKey,NULL,NULL,1)
end

--Setup user as an RCS User
if (not exists(select 1 from dbo.userorganisationstructure where aduserkey= @HaloUserAduserKey and organisationstructurekey = @RCSOrganisationStructureKey))
begin
	insert into dbo.userorganisationstructure (aduserkey,organisationstructurekey,generickey,generickeytypekey,generalstatuskey)
	values (@HaloUserAduserKey,@RCSOrganisationStructureKey,NULL,NULL,1)
end

--UpdatePropertyFeature
if (not exists(select 1 from dbo.featuregroup
					inner join dbo.feature
						on featuregroup.featurekey = feature.featurekey
				where adusergroup = 'HaloUser' and longname = @UpdatePropertyFeature))
begin
	insert into dbo.featuregroup
	select distinct 
		'HaloUser' as ADUserGroup,
		Feature.FeatureKey
	from 
		dbo.feature
	where feature.longname in (@UpdatePropertyFeature)
	print 'Added UpdatePropertyFeature'
end

--PropertyTitleDeedFeature
if (not exists(select 1 from dbo.featuregroup
					inner join dbo.feature
						on featuregroup.featurekey = feature.featurekey
				where adusergroup = 'HaloUser' and longname = @PropertyTitleDeedFeature))
begin
	insert into dbo.featuregroup
	select distinct 
		'HaloUser' as ADUserGroup,
		Feature.FeatureKey
	from 
		dbo.feature
	where feature.longname in (@PropertyTitleDeedFeature)
	print 'Added PropertyTitleDeedFeature'
end

--PropertyAddressFeature
if (not exists(select 1 from dbo.featuregroup
					inner join dbo.feature
						on featuregroup.featurekey = feature.featurekey
				where adusergroup = 'HaloUser' and longname = @PropertyAddressFeature))
begin
	insert into dbo.featuregroup
	select distinct 
		'HaloUser' as ADUserGroup,
		Feature.FeatureKey
	from 
		dbo.feature
	where feature.longname in (@PropertyAddressFeature)
	print 'Added PropertyAddressFeature'
end

--DeleteLegalEntityAddressFeature
if (not exists(select 1 from dbo.featuregroup
					inner join dbo.feature
						on featuregroup.featurekey = feature.featurekey
				where adusergroup = 'HaloUser' and longname = @DeleteLegalEntityAddressFeature))
begin
	insert into dbo.featuregroup
	select distinct 
		'HaloUser' as ADUserGroup,
		Feature.FeatureKey
	from 
		dbo.feature
	where feature.longname in (@DeleteLegalEntityAddressFeature)
	print 'Added DeleteLegalEntityAddressFeature'
end

--DeleteAssetLiability
if (not exists(select 1 from dbo.featuregroup
					inner join dbo.feature
						on featuregroup.featurekey = feature.featurekey
				where adusergroup = 'HaloUser' and longname = @DeleteAssetLiabilityFeature))
begin
	insert into dbo.featuregroup
	select distinct 
		'HaloUser' as ADUserGroup,
		Feature.FeatureKey
	from 
		dbo.feature
	where feature.longname in (@DeleteAssetLiabilityFeature)
	print 'Added DeleteLegalEntityAddressFeature'
end

--CaptureClientSurvey
if (not exists(select 1 from dbo.featuregroup
					inner join dbo.feature
						on featuregroup.featurekey = feature.featurekey
				where adusergroup = 'HaloUser' and longname = @CaptureClientSurveyFeature))
begin
	insert into dbo.featuregroup
	select distinct 
		'HaloUser' as ADUserGroup,
		Feature.FeatureKey
	from 
		dbo.feature
	where feature.longname in (@CaptureClientSurveyFeature)
	print 'Added CaptureClientSurvey Feature'
end

--EnableUpdateLegalEntity
if (not exists(select 1 from dbo.featuregroup
					inner join dbo.feature
						on featuregroup.featurekey = feature.featurekey
				where adusergroup = 'HaloUser' and longname = @EnableUpdateLegalEntity))
begin
	insert into dbo.featuregroup
	select distinct 
		'HaloUser' as ADUserGroup,
		Feature.FeatureKey
	from 
		dbo.feature
	where feature.longname in (@EnableUpdateLegalEntity)
	print 'Added EnableUpdateLegalEntity Feature'
end


--AdminFlushCache
if (not exists(select 1 from dbo.featuregroup
					inner join dbo.feature
						on featuregroup.featurekey = feature.featurekey
				where adusergroup = 'HaloUser' and longname = @FlushCacheFeature))
begin
	insert into dbo.featuregroup
	select distinct 
		'HaloUser' as ADUserGroup,
		Feature.FeatureKey
	from 
		dbo.feature
	where feature.longname in (@FlushCacheFeature)
	print 'Added AdminFlushCache Feature'
end

--Add as a Branch Consultant
if (not exists(select 1 from dbo.userorganisationstructure
			   where aduserkey = @HaloADUserKey and organisationstructurekey = @PMBConsultantOrgStructKey))
insert into dbo.userorganisationstructure (ADUserKey,OrganisationStructureKey,GenericKey,GenericKeyTypeKey,GeneralStatusKey)
values (@HaloADUserKey,@PMBConsultantOrgStructKey,NULL,	NULL,1)


--CalculatorCBO
if (not exists(select 1 from dbo.featuregroup
					inner join dbo.feature
						on featuregroup.featurekey = feature.featurekey
				where adusergroup = 'HaloUser' and longname = @CalculatorCBOFeature))
begin
	insert into dbo.featuregroup
	select distinct 
		'HaloUser' as ADUserGroup,
		Feature.FeatureKey
	from 
		dbo.feature
	where feature.longname in (@CalculatorCBOFeature)
	print 'Added CalculatorCBOFeature Feature'
end

--ApplicationCalculatorFeature
if (not exists(select 1 from dbo.featuregroup
					inner join dbo.feature
						on featuregroup.featurekey = feature.featurekey
				where adusergroup = 'HaloUser' and longname = @ApplicationCalculatorFeature))
begin
	insert into dbo.featuregroup
	select distinct 
		'HaloUser' as ADUserGroup,
		Feature.FeatureKey
	from 
		dbo.feature
	where feature.longname in (@ApplicationCalculatorFeature)
	print 'Added ApplicationCalculator Feature'
end


--LeadCaptureFeature
if (not exists(select 1 from dbo.featuregroup
					inner join dbo.feature
						on featuregroup.featurekey = feature.featurekey
				where adusergroup = 'HaloUser' and longname = @LeadCaptureFeature))
begin
	insert into dbo.featuregroup
	select distinct 
		'HaloUser' as ADUserGroup,
		Feature.FeatureKey
	from 
		dbo.feature
	where feature.longname in (@LeadCaptureFeature)
	print 'Added LeadCapture Feature'
end

--AddAttorneyDetailsFeature
if (not exists(select 1 from dbo.featuregroup
					inner join dbo.feature
						on featuregroup.featurekey = feature.featurekey
				where adusergroup = 'HaloUser' and longname = @AddAttorneyDetailsFeature))
begin
	insert into dbo.featuregroup
	select distinct 
		'HaloUser' as ADUserGroup,
		Feature.FeatureKey
	from 
		dbo.feature
	where feature.longname in (@AddAttorneyDetailsFeature)
	print 'Added AddAttorneyDetails Feature'
end


--UpdateAttorneyDetails
if (not exists(select 1 from dbo.featuregroup
					inner join dbo.feature
						on featuregroup.featurekey = feature.featurekey
				where adusergroup = 'HaloUser' and longname = @UpdateAttorneyDetails))
begin
	insert into dbo.featuregroup
	select distinct 
		'HaloUser' as ADUserGroup,
		Feature.FeatureKey
	from 
		dbo.feature
	where feature.longname in (@UpdateAttorneyDetails)
	print 'Added UpdateAttorneyDetails Feature'
end

--AttorneyDetailsFeature
if (not exists(select 1 from dbo.featuregroup
					inner join dbo.feature
						on featuregroup.featurekey = feature.featurekey
				where adusergroup = 'HaloUser' and longname = @AttorneyDetailsFeature))
begin
	insert into dbo.featuregroup
	select distinct 
		'HaloUser' as ADUserGroup,
		Feature.FeatureKey
	from 
		dbo.feature
	where feature.longname in (@AttorneyDetailsFeature)
	print 'Added AttorneyDetails Feature'
end


--we need to insert transaction type data access for the HALO test user
--non performing transactions
if (not exists(select 1 from dbo.TransactionTypeDataAccess
				where AdCredentials = 'ArrearsManagement' and TransactionTypeKey = 236))
begin				
	insert into dbo.TransactionTypeDataAccess
	( AdCredentials, TransactionTypeKey )
	values ('ArrearsManagement', 236)
end

if (not exists(select 1 from dbo.TransactionTypeDataAccess
				where AdCredentials = 'ArrearsManagement' and TransactionTypeKey = 966))
begin	
	insert into dbo.TransactionTypeDataAccess
	( AdCredentials, TransactionTypeKey )
	values ('ArrearsManagement', 966)
end

if (not exists(select 1 from dbo.TransactionTypeDataAccess
				where AdCredentials = 'ArrearsManagement' and TransactionTypeKey = 967))
begin	
	insert into dbo.TransactionTypeDataAccess
	( AdCredentials, TransactionTypeKey )
	values ('ArrearsManagement', 967)
end

