use [2am]
go


declare @CoreBusinessObjectKey int
declare @ParentCoreBusinessObjectKey int
declare @CoreBusinessObjectKey_Unconfirmed int
declare @CoreBusinessObjectKey_Confirmed int
declare @ContextKey int
declare @ParentContextKey int
declare @ParentContextKey_Unconfirmed int
declare @ParentContextKey_Confirmed int
declare @FeatureKey int
declare @ParentFeatureKey int
declare @CopyFeatureKey int
declare @ADUserGroup varchar(50)

-------------------------------------------------------------------------
-- update sequence of existing flobo items to make space for out new node
-------------------------------------------------------------------------
update [2am]..CoreBusinessObjectMenu set Sequence = 7 where CoreBusinessObjectKey = 120 and Description = 'Application Memo'
update [2am]..CoreBusinessObjectMenu set Sequence = 8 where CoreBusinessObjectKey = 130 and Description = 'Document Checklist'
update [2am]..CoreBusinessObjectMenu set Sequence = 9 where CoreBusinessObjectKey = 140 and Description = 'Correspondence'

--------------------------------------------------------
-- insert new flobo node for 'Affordability Assessments'
--------------------------------------------------------
set @FeatureKey = 4100
if (not exists(select * from [2am]..Feature where FeatureKey = @FeatureKey))
begin
	insert into [2am]..[Feature]([ShortName],[LongName],[HasAccess],[ParentKey],[Sequence],[FeatureKey]) 
		values ('Affordability Assessments','Affordability Assessments',1,3000,1,@FeatureKey)
end

set @CopyFeatureKey = 3001 -- Applicants
insert into [2AM]..[FeatureGroup]([ADUserGroup],[FeatureKey])
select fg.ADUserGroup,@FeatureKey from [2am]..FeatureGroup fg
left join [2am]..[FeatureGroup] fg2 on fg2.ADUserGroup = fg.ADUserGroup and fg2.FeatureKey = @FeatureKey
where fg.FeatureKey = @CopyFeatureKey and fg2.FeatureGroupKey is null 

set @CoreBusinessObjectKey = 150
if (not exists(select * from [2am]..CoreBusinessObjectMenu where CoreBusinessObjectKey = @CoreBusinessObjectKey))
begin
	insert into [2am]..[CoreBusinessObjectMenu] ([CoreBusinessObjectKey], [ParentKey], [Description], [NodeType], [URL], [StatementNameKey], [Sequence], [MenuIcon], [FeatureKey], [HasOriginationSource], [IsRemovable], [ExpandLevel], [IncludeParentHeaderIcons], [GenericKeyTypeKey]) 
		values  (@CoreBusinessObjectKey, 100, N'Affordability Assessments', N'D', N'AffordabilityAssessmentSummary', N'CBOGetAffordabilityAssessmentsByApplicationKey', 6, N'assessments.png', @FeatureKey, 0, 0, 5, 0, 2)
end
else
begin
	update [2am]..[CoreBusinessObjectMenu]
	set [ParentKey] = 100, 
		[Description] = N'Affordability Assessments', 
		[NodeType] = N'D', 
		[URL] = N'AffordabilityAssessmentSummary', 
		[StatementNameKey] = N'CBOGetAffordabilityAssessmentsByApplicationKey', 
		[Sequence] = 6, 
		[MenuIcon] = N'assessments.png', 
		[FeatureKey] = @FeatureKey, 
		[HasOriginationSource] = 0, 
		[IsRemovable] = 0, 
		[ExpandLevel] = 5, 
		[IncludeParentHeaderIcons] = 0, 
		[GenericKeyTypeKey] = 2
	where CoreBusinessObjectKey = @CoreBusinessObjectKey 
end

if(not exists(select * from [2am]..InputGenericType where CoreBusinessObjectKey = @CoreBusinessObjectKey and GenericKeyTypeParameterKey = 8)) 
begin
	insert into [2am]..[InputGenericType] ([CoreBusinessObjectKey], [GenericKeyTypeParameterKey]) values (@CoreBusinessObjectKey, 8)
end

set @ContextKey = 3300
if (not exists(select * from [2am]..ContextMenu where CoreBusinessObjectKey = @CoreBusinessObjectKey and ContextKey = @ContextKey))
begin
	insert into [2am]..[ContextMenu] ([ContextKey], [CoreBusinessObjectKey], [ParentKey], [Description], [URL], [FeatureKey], [Sequence]) 
	values (@ContextKey, @CoreBusinessObjectKey, NULL, N'Affordability Assessments', N'AffordabilityAssessmentSummary', @FeatureKey, 1)
end

set @ParentContextKey = @ContextKey
set @ParentFeatureKey = @FeatureKey

-----------------------------------------------------------
-- insert new flobo node for 'Add Affordability Assessment'
-----------------------------------------------------------

set @FeatureKey = 4105
if (not exists(select * from [2am]..Feature where FeatureKey = @FeatureKey))
begin
	insert into [2am]..[Feature]([ShortName],[LongName],[HasAccess],[ParentKey],[Sequence],[FeatureKey]) 
		values ('Add Affordability Assessment','Add Affordability Assessment',1,@ParentFeatureKey,1,@FeatureKey)
end
set @ADUserGroup = 'ITStaff'
if (not exists(select * from [2am]..FeatureGroup where FeatureKey = @FeatureKey and ADUserGroup = @ADUserGroup))
	insert into [2am]..[FeatureGroup]([ADUserGroup],[FeatureKey]) values (@ADUserGroup, @FeatureKey)
set @ADUserGroup = 'Branch Consultant'
if (not exists(select * from [2am]..FeatureGroup where FeatureKey = @FeatureKey and ADUserGroup = @ADUserGroup))
	insert into [2am]..[FeatureGroup]([ADUserGroup],[FeatureKey]) values (@ADUserGroup, @FeatureKey)
set @ADUserGroup = 'Branch admin'
if (not exists(select * from [2am]..FeatureGroup where FeatureKey = @FeatureKey and ADUserGroup = @ADUserGroup))
	insert into [2am]..[FeatureGroup]([ADUserGroup],[FeatureKey]) values (@ADUserGroup, @FeatureKey)
set @ADUserGroup = 'Branch Manager'
if (not exists(select * from [2am]..FeatureGroup where FeatureKey = @FeatureKey and ADUserGroup = @ADUserGroup))
	insert into [2am]..[FeatureGroup]([ADUserGroup],[FeatureKey]) values (@ADUserGroup, @FeatureKey)

set @ADUserGroup = 'New Business Processor'
if (not exists(select * from [2am]..FeatureGroup where FeatureKey = @FeatureKey and ADUserGroup = @ADUserGroup))
	insert into [2am]..[FeatureGroup]([ADUserGroup],[FeatureKey]) values (@ADUserGroup, @FeatureKey)
set @ADUserGroup = 'New Business Manager'
if (not exists(select * from [2am]..FeatureGroup where FeatureKey = @FeatureKey and ADUserGroup = @ADUserGroup))
	insert into [2am]..[FeatureGroup]([ADUserGroup],[FeatureKey]) values (@ADUserGroup, @FeatureKey)
set @ADUserGroup = 'New Business Supervisor'
if (not exists(select * from [2am]..FeatureGroup where FeatureKey = @FeatureKey and ADUserGroup = @ADUserGroup))
	insert into [2am]..[FeatureGroup]([ADUserGroup],[FeatureKey]) values (@ADUserGroup, @FeatureKey)

set @ADUserGroup = 'FL Application Processor'
if (not exists(select * from [2am]..FeatureGroup where FeatureKey = @FeatureKey and ADUserGroup = @ADUserGroup))
	insert into [2am]..[FeatureGroup]([ADUserGroup],[FeatureKey]) values (@ADUserGroup, @FeatureKey)
set @ADUserGroup = 'FL Collections Admin'
if (not exists(select * from [2am]..FeatureGroup where FeatureKey = @FeatureKey and ADUserGroup = @ADUserGroup))
	insert into [2am]..[FeatureGroup]([ADUserGroup],[FeatureKey]) values (@ADUserGroup, @FeatureKey)
set @ADUserGroup = 'FL Supervisor'
if (not exists(select * from [2am]..FeatureGroup where FeatureKey = @FeatureKey and ADUserGroup = @ADUserGroup))
	insert into [2am]..[FeatureGroup]([ADUserGroup],[FeatureKey]) values (@ADUserGroup, @FeatureKey)
set @ADUserGroup = 'FL Manager'
if (not exists(select * from [2am]..FeatureGroup where FeatureKey = @FeatureKey and ADUserGroup = @ADUserGroup))
	insert into [2am]..[FeatureGroup]([ADUserGroup],[FeatureKey]) values (@ADUserGroup, @FeatureKey)

set @ADUserGroup = 'Registrations Manager'
if (not exists(select * from [2am]..FeatureGroup where FeatureKey = @FeatureKey and ADUserGroup = @ADUserGroup))
	insert into [2am]..[FeatureGroup]([ADUserGroup],[FeatureKey]) values (@ADUserGroup, @FeatureKey)
set @ADUserGroup = 'Registrations Supervisor'
if (not exists(select * from [2am]..FeatureGroup where FeatureKey = @FeatureKey and ADUserGroup = @ADUserGroup))
	insert into [2am]..[FeatureGroup]([ADUserGroup],[FeatureKey]) values (@ADUserGroup, @FeatureKey)
set @ADUserGroup = 'Resubmission Admin'
if (not exists(select * from [2am]..FeatureGroup where FeatureKey = @FeatureKey and ADUserGroup = @ADUserGroup))
	insert into [2am]..[FeatureGroup]([ADUserGroup],[FeatureKey]) values (@ADUserGroup, @FeatureKey)


set @ContextKey = 3305
if (not exists(select * from [2am]..ContextMenu where ParentKey = @ParentContextKey and ContextKey = @ContextKey))
begin
	insert into [2am]..[ContextMenu] ([ContextKey], [CoreBusinessObjectKey], [ParentKey], [Description], [URL], [FeatureKey], [Sequence]) 
	values (@ContextKey, null, @ParentContextKey, N'Add Affordability Assessment', N'AffordabilityAssessmentAdd', @FeatureKey, 1)
end

--------------------------------------------------------------
-- insert new flobo node for 'Delete Affordability Assessment'
-------------------------------------------------------------

set @FeatureKey = 4110
if (not exists(select * from [2am]..Feature where FeatureKey = @FeatureKey))
begin
	insert into [2am]..[Feature]([ShortName],[LongName],[HasAccess],[ParentKey],[Sequence],[FeatureKey]) 
		values ('Delete Affordability Assessment','Delete Affordability Assessment',1,@ParentFeatureKey,1,@FeatureKey)
end

set @ADUserGroup = 'ITStaff'
if (not exists(select * from [2am]..FeatureGroup where FeatureKey = @FeatureKey and ADUserGroup = @ADUserGroup))
	insert into [2am]..[FeatureGroup]([ADUserGroup],[FeatureKey]) values (@ADUserGroup, @FeatureKey)
set @ADUserGroup = 'Branch Consultant'
if (not exists(select * from [2am]..FeatureGroup where FeatureKey = @FeatureKey and ADUserGroup = @ADUserGroup))
	insert into [2am]..[FeatureGroup]([ADUserGroup],[FeatureKey]) values (@ADUserGroup, @FeatureKey)
set @ADUserGroup = 'Branch admin'
if (not exists(select * from [2am]..FeatureGroup where FeatureKey = @FeatureKey and ADUserGroup = @ADUserGroup))
	insert into [2am]..[FeatureGroup]([ADUserGroup],[FeatureKey]) values (@ADUserGroup, @FeatureKey)
set @ADUserGroup = 'Branch Manager'
if (not exists(select * from [2am]..FeatureGroup where FeatureKey = @FeatureKey and ADUserGroup = @ADUserGroup))
	insert into [2am]..[FeatureGroup]([ADUserGroup],[FeatureKey]) values (@ADUserGroup, @FeatureKey)

set @ADUserGroup = 'New Business Processor'
if (not exists(select * from [2am]..FeatureGroup where FeatureKey = @FeatureKey and ADUserGroup = @ADUserGroup))
	insert into [2am]..[FeatureGroup]([ADUserGroup],[FeatureKey]) values (@ADUserGroup, @FeatureKey)
set @ADUserGroup = 'New Business Manager'
if (not exists(select * from [2am]..FeatureGroup where FeatureKey = @FeatureKey and ADUserGroup = @ADUserGroup))
	insert into [2am]..[FeatureGroup]([ADUserGroup],[FeatureKey]) values (@ADUserGroup, @FeatureKey)
set @ADUserGroup = 'New Business Supervisor'
if (not exists(select * from [2am]..FeatureGroup where FeatureKey = @FeatureKey and ADUserGroup = @ADUserGroup))
	insert into [2am]..[FeatureGroup]([ADUserGroup],[FeatureKey]) values (@ADUserGroup, @FeatureKey)

set @ADUserGroup = 'FL Application Processor'
if (not exists(select * from [2am]..FeatureGroup where FeatureKey = @FeatureKey and ADUserGroup = @ADUserGroup))
	insert into [2am]..[FeatureGroup]([ADUserGroup],[FeatureKey]) values (@ADUserGroup, @FeatureKey)
set @ADUserGroup = 'FL Collections Admin'
if (not exists(select * from [2am]..FeatureGroup where FeatureKey = @FeatureKey and ADUserGroup = @ADUserGroup))
	insert into [2am]..[FeatureGroup]([ADUserGroup],[FeatureKey]) values (@ADUserGroup, @FeatureKey)
set @ADUserGroup = 'FL Supervisor'
if (not exists(select * from [2am]..FeatureGroup where FeatureKey = @FeatureKey and ADUserGroup = @ADUserGroup))
	insert into [2am]..[FeatureGroup]([ADUserGroup],[FeatureKey]) values (@ADUserGroup, @FeatureKey)
set @ADUserGroup = 'FL Manager'
if (not exists(select * from [2am]..FeatureGroup where FeatureKey = @FeatureKey and ADUserGroup = @ADUserGroup))
	insert into [2am]..[FeatureGroup]([ADUserGroup],[FeatureKey]) values (@ADUserGroup, @FeatureKey)

set @ADUserGroup = 'Registrations Manager'
if (not exists(select * from [2am]..FeatureGroup where FeatureKey = @FeatureKey and ADUserGroup = @ADUserGroup))
	insert into [2am]..[FeatureGroup]([ADUserGroup],[FeatureKey]) values (@ADUserGroup, @FeatureKey)
set @ADUserGroup = 'Registrations Supervisor'
if (not exists(select * from [2am]..FeatureGroup where FeatureKey = @FeatureKey and ADUserGroup = @ADUserGroup))
	insert into [2am]..[FeatureGroup]([ADUserGroup],[FeatureKey]) values (@ADUserGroup, @FeatureKey)
set @ADUserGroup = 'Resubmission Admin'
if (not exists(select * from [2am]..FeatureGroup where FeatureKey = @FeatureKey and ADUserGroup = @ADUserGroup))
	insert into [2am]..[FeatureGroup]([ADUserGroup],[FeatureKey]) values (@ADUserGroup, @FeatureKey)

set @ContextKey = 3310
if (not exists(select * from [2am]..ContextMenu where ParentKey = @ParentContextKey and ContextKey = @ContextKey))
begin
	insert into [2am]..[ContextMenu] ([ContextKey], [CoreBusinessObjectKey], [ParentKey], [Description], [URL], [FeatureKey], [Sequence]) 
	values (@ContextKey, null, @ParentContextKey, N'Delete Affordability Assessment', N'AffordabilityAssessmentDelete', @FeatureKey, 2)
end

---------------------------------------------------------
-- insert new Dynamic flobo node for the Added Assessment
---------------------------------------------------------
set @FeatureKey = 4101
if (not exists(select * from [2am]..Feature where FeatureKey = @FeatureKey))
begin
	insert into [2am]..[Feature]([ShortName],[LongName],[HasAccess],[ParentKey],[Sequence],[FeatureKey]) 
		values ('Affordability Assessment (Dynamic)','Affordability Assessment (Dynamic)',1,4100,1,@FeatureKey)
end

set @CopyFeatureKey = 4100 -- Affordability Assessments
insert into [2AM]..[FeatureGroup]([ADUserGroup],[FeatureKey])
select fg.ADUserGroup,@FeatureKey from [2am]..FeatureGroup fg
left join [2am]..[FeatureGroup] fg2 on fg2.ADUserGroup = fg.ADUserGroup and fg2.FeatureKey = @FeatureKey
where fg.FeatureKey = @CopyFeatureKey and fg2.FeatureGroupKey is null 

set @ParentCoreBusinessObjectKey = 150
set @CoreBusinessObjectKey = 151 -- unconfirmed affordability assessments
set @CoreBusinessObjectKey_Unconfirmed = @CoreBusinessObjectKey
if (not exists(select * from [2am]..CoreBusinessObjectMenu where CoreBusinessObjectKey = @CoreBusinessObjectKey and ParentKey = @ParentCoreBusinessObjectKey))
begin
	insert into [2am]..[CoreBusinessObjectMenu] ([CoreBusinessObjectKey], [ParentKey], [Description], [NodeType], [URL], [StatementNameKey], [Sequence], [MenuIcon], [FeatureKey], [HasOriginationSource], [IsRemovable], [ExpandLevel], [IncludeParentHeaderIcons], [GenericKeyTypeKey]) 
		values  (@CoreBusinessObjectKey, @ParentCoreBusinessObjectKey, N'Affordability Assessment Unconfirmed (Dynamic)', N'D', N'AffordabilityAssessmentDisplay', N'CBOGetAffordabilityAssessmentUnconfirmedByAppKey', 1, N'assessment_unconfirmed.png', @FeatureKey, 0, 0, 10, 0, 58)
end
else
begin
	update [2am]..[CoreBusinessObjectMenu] set  [Description] = N'Affordability Assessment Unconfirmed (Dynamic)',[StatementNameKey] = N'CBOGetAffordabilityAssessmentUnconfirmedByAppKey', [MenuIcon] = N'assessment_unconfirmed.png'
	where CoreBusinessObjectKey = @CoreBusinessObjectKey and ParentKey = @ParentCoreBusinessObjectKey
end

if(not exists(select * from [2am]..InputGenericType where CoreBusinessObjectKey = @CoreBusinessObjectKey and GenericKeyTypeParameterKey = 8)) 
begin
	insert into [2am]..[InputGenericType] ([CoreBusinessObjectKey], [GenericKeyTypeParameterKey]) values (@CoreBusinessObjectKey, 8)
end

set @CoreBusinessObjectKey = 152 -- confirmed affordability assessments
set @CoreBusinessObjectKey_Confirmed = @CoreBusinessObjectKey
if (not exists(select * from [2am]..CoreBusinessObjectMenu where CoreBusinessObjectKey = @CoreBusinessObjectKey and ParentKey = @ParentCoreBusinessObjectKey))
begin
	insert into [2am]..[CoreBusinessObjectMenu] ([CoreBusinessObjectKey], [ParentKey], [Description], [NodeType], [URL], [StatementNameKey], [Sequence], [MenuIcon], [FeatureKey], [HasOriginationSource], [IsRemovable], [ExpandLevel], [IncludeParentHeaderIcons], [GenericKeyTypeKey]) 
		values  (@CoreBusinessObjectKey, @ParentCoreBusinessObjectKey, N'Affordability Assessment Confirmed (Dynamic)', N'D', N'AffordabilityAssessmentDisplay', N'CBOGetAffordabilityAssessmentConfirmedByAppKey', 2, N'assessment_confirmed.png', @FeatureKey, 0, 0, 10, 0, 58)
end

if(not exists(select * from [2am]..InputGenericType where CoreBusinessObjectKey = @CoreBusinessObjectKey and GenericKeyTypeParameterKey = 8)) 
begin
	insert into [2am]..[InputGenericType] ([CoreBusinessObjectKey], [GenericKeyTypeParameterKey]) values (@CoreBusinessObjectKey, 8)
end

------------------------------------------------------------
-- insert new flobo node for Affordability Assessment Detail
------------------------------------------------------------
set @FeatureKey = 4102
if (not exists(select * from [2am]..Feature where FeatureKey = @FeatureKey))
begin
	insert into [2am]..[Feature]([ShortName],[LongName],[HasAccess],[ParentKey],[Sequence],[FeatureKey]) 
		values ('Affordability Assessment Detail','Affordability Assessment Detail',1,4100,1,@FeatureKey)
end

set @CopyFeatureKey = 4101 -- Affordability Assessment (Dynamic)
insert into [2AM]..[FeatureGroup]([ADUserGroup],[FeatureKey])
select fg.ADUserGroup,@FeatureKey from [2am]..FeatureGroup fg
left join [2am]..[FeatureGroup] fg2 on fg2.ADUserGroup = fg.ADUserGroup and fg2.FeatureKey = @FeatureKey
where fg.FeatureKey = @CopyFeatureKey and fg2.FeatureGroupKey is null 

set @ContextKey = 3315
set @ParentContextKey_Unconfirmed = @ContextKey
if (not exists(select * from [2am]..ContextMenu where CoreBusinessObjectKey = @CoreBusinessObjectKey_Unconfirmed and ContextKey = @ContextKey))
begin
	insert into [2am]..[ContextMenu] ([ContextKey], [CoreBusinessObjectKey], [ParentKey], [Description], [URL], [FeatureKey], [Sequence]) 
	values (@ContextKey, @CoreBusinessObjectKey_Unconfirmed, NULL, N'Affordability Assessment Detail', N'AffordabilityAssessmentDisplay', @FeatureKey, 1)
end

set @ContextKey = 3317
set @ParentContextKey_Confirmed = @ContextKey
if (not exists(select * from [2am]..ContextMenu where CoreBusinessObjectKey = @CoreBusinessObjectKey_Confirmed and ContextKey = @ContextKey))
begin
	insert into [2am]..[ContextMenu] ([ContextKey], [CoreBusinessObjectKey], [ParentKey], [Description], [URL], [FeatureKey], [Sequence]) 
	values (@ContextKey, @CoreBusinessObjectKey_Confirmed, NULL, N'Affordability Assessment Detail', N'AffordabilityAssessmentDisplay', @FeatureKey, 1)
end
--------------------------------------------------------------
-- insert new flobo node for 'Update Affordability Assessment'
-------------------------------------------------------------
set @ParentFeatureKey = 4102
set @FeatureKey = 4115
if (not exists(select * from [2am]..Feature where FeatureKey = @FeatureKey))
begin
	insert into [2am]..[Feature]([ShortName],[LongName],[HasAccess],[ParentKey],[Sequence],[FeatureKey]) 
		values ('Update Affordability Assessment','Update Affordability Assessment',1,@ParentFeatureKey,1,@FeatureKey)
end
set @ADUserGroup = 'ITStaff'
if (not exists(select * from [2am]..FeatureGroup where FeatureKey = @FeatureKey and ADUserGroup = @ADUserGroup))
	insert into [2am]..[FeatureGroup]([ADUserGroup],[FeatureKey]) values (@ADUserGroup, @FeatureKey)
set @ADUserGroup = 'Branch Consultant'
if (not exists(select * from [2am]..FeatureGroup where FeatureKey = @FeatureKey and ADUserGroup = @ADUserGroup))
	insert into [2am]..[FeatureGroup]([ADUserGroup],[FeatureKey]) values (@ADUserGroup, @FeatureKey)
set @ADUserGroup = 'Branch admin'
if (not exists(select * from [2am]..FeatureGroup where FeatureKey = @FeatureKey and ADUserGroup = @ADUserGroup))
	insert into [2am]..[FeatureGroup]([ADUserGroup],[FeatureKey]) values (@ADUserGroup, @FeatureKey)
set @ADUserGroup = 'Branch Manager'
if (not exists(select * from [2am]..FeatureGroup where FeatureKey = @FeatureKey and ADUserGroup = @ADUserGroup))
	insert into [2am]..[FeatureGroup]([ADUserGroup],[FeatureKey]) values (@ADUserGroup, @FeatureKey)

set @ADUserGroup = 'FL Application Processor'
if (not exists(select * from [2am]..FeatureGroup where FeatureKey = @FeatureKey and ADUserGroup = @ADUserGroup))
	insert into [2am]..[FeatureGroup]([ADUserGroup],[FeatureKey]) values (@ADUserGroup, @FeatureKey)
set @ADUserGroup = 'FL Collections Admin'
if (not exists(select * from [2am]..FeatureGroup where FeatureKey = @FeatureKey and ADUserGroup = @ADUserGroup))
	insert into [2am]..[FeatureGroup]([ADUserGroup],[FeatureKey]) values (@ADUserGroup, @FeatureKey)
set @ADUserGroup = 'FL Supervisor'
if (not exists(select * from [2am]..FeatureGroup where FeatureKey = @FeatureKey and ADUserGroup = @ADUserGroup))
	insert into [2am]..[FeatureGroup]([ADUserGroup],[FeatureKey]) values (@ADUserGroup, @FeatureKey)
set @ADUserGroup = 'FL Manager'
if (not exists(select * from [2am]..FeatureGroup where FeatureKey = @FeatureKey and ADUserGroup = @ADUserGroup))
	insert into [2am]..[FeatureGroup]([ADUserGroup],[FeatureKey]) values (@ADUserGroup, @FeatureKey)

set @ADUserGroup = 'Registrations Manager'
if (not exists(select * from [2am]..FeatureGroup where FeatureKey = @FeatureKey and ADUserGroup = @ADUserGroup))
	insert into [2am]..[FeatureGroup]([ADUserGroup],[FeatureKey]) values (@ADUserGroup, @FeatureKey)
set @ADUserGroup = 'Registrations Supervisor'
if (not exists(select * from [2am]..FeatureGroup where FeatureKey = @FeatureKey and ADUserGroup = @ADUserGroup))
	insert into [2am]..[FeatureGroup]([ADUserGroup],[FeatureKey]) values (@ADUserGroup, @FeatureKey)
set @ADUserGroup = 'Resubmission Admin'
if (not exists(select * from [2am]..FeatureGroup where FeatureKey = @FeatureKey and ADUserGroup = @ADUserGroup))
	insert into [2am]..[FeatureGroup]([ADUserGroup],[FeatureKey]) values (@ADUserGroup, @FeatureKey)

set @ADUserGroup = 'New Business Manager'
if (not exists(select * from [2am]..FeatureGroup where FeatureKey = @FeatureKey and ADUserGroup = @ADUserGroup))
	insert into [2am]..[FeatureGroup]([ADUserGroup],[FeatureKey]) values (@ADUserGroup, @FeatureKey)
set @ADUserGroup = 'New Business Processor'
if (not exists(select * from [2am]..FeatureGroup where FeatureKey = @FeatureKey and ADUserGroup = @ADUserGroup))
	insert into [2am]..[FeatureGroup]([ADUserGroup],[FeatureKey]) values (@ADUserGroup, @FeatureKey)
set @ADUserGroup = 'New Business Supervisor'
if (not exists(select * from [2am]..FeatureGroup where FeatureKey = @FeatureKey and ADUserGroup = @ADUserGroup))
	insert into [2am]..[FeatureGroup]([ADUserGroup],[FeatureKey]) values (@ADUserGroup, @FeatureKey)

--set @ADUserGroup = 'Credit'
--if (not exists(select * from [2am]..FeatureGroup where FeatureKey = @FeatureKey and ADUserGroup = @ADUserGroup))
--	insert into [2am]..[FeatureGroup]([ADUserGroup],[FeatureKey]) values (@ADUserGroup, @FeatureKey)
set @ADUserGroup = 'Credit Exceptions'
if (not exists(select * from [2am]..FeatureGroup where FeatureKey = @FeatureKey and ADUserGroup = @ADUserGroup))
	insert into [2am]..[FeatureGroup]([ADUserGroup],[FeatureKey]) values (@ADUserGroup, @FeatureKey)
set @ADUserGroup = 'Credit Manager'
if (not exists(select * from [2am]..FeatureGroup where FeatureKey = @FeatureKey and ADUserGroup = @ADUserGroup))
	insert into [2am]..[FeatureGroup]([ADUserGroup],[FeatureKey]) values (@ADUserGroup, @FeatureKey)
set @ADUserGroup = 'Credit Supervisor'
if (not exists(select * from [2am]..FeatureGroup where FeatureKey = @FeatureKey and ADUserGroup = @ADUserGroup))
	insert into [2am]..[FeatureGroup]([ADUserGroup],[FeatureKey]) values (@ADUserGroup, @FeatureKey)
set @ADUserGroup = 'Credit Underwriter'
if (not exists(select * from [2am]..FeatureGroup where FeatureKey = @FeatureKey and ADUserGroup = @ADUserGroup))
	insert into [2am]..[FeatureGroup]([ADUserGroup],[FeatureKey]) values (@ADUserGroup, @FeatureKey)

set @ContextKey = 3320
if (not exists(select * from [2am]..ContextMenu where ParentKey = @ParentContextKey_Unconfirmed and ContextKey = @ContextKey))
begin
	insert into [2am]..[ContextMenu] ([ContextKey], [CoreBusinessObjectKey], [ParentKey], [Description], [URL], [FeatureKey], [Sequence]) 
	values (@ContextKey, null, @ParentContextKey_Unconfirmed, N'Update Affordability Assessment', N'AffordabilityAssessmentUpdate', @FeatureKey, 1)
end

set @ContextKey = 3322
if (not exists(select * from [2am]..ContextMenu where ParentKey = @ParentContextKey_Confirmed and ContextKey = @ContextKey))
begin
	insert into [2am]..[ContextMenu] ([ContextKey], [CoreBusinessObjectKey], [ParentKey], [Description], [URL], [FeatureKey], [Sequence]) 
	values (@ContextKey, null, @ParentContextKey_Confirmed, N'Update Affordability Assessment', N'AffordabilityAssessmentUpdate', @FeatureKey, 1)
end

-------------------------------------------------------
-- insert new flobo node for 'Update Income Contributors'
-------------------------------------------------------
set @ParentFeatureKey = 4102
set @FeatureKey = 4120
if (not exists(select * from [2am]..Feature where FeatureKey = @FeatureKey))
begin
	insert into [2am]..[Feature]([ShortName],[LongName],[HasAccess],[ParentKey],[Sequence],[FeatureKey]) 
		values ('Update Income Contributors','Update Income Contributors',1,@ParentFeatureKey,1,@FeatureKey)
end
else
begin
	update [2am]..[Feature]
	set [ShortName] = 'Update Income Contributors',
		[LongName] = 'Update Income Contributors',
		[HasAccess] = 1,
		[ParentKey] = @ParentFeatureKey,
		[Sequence] = 1
	where [FeatureKey] = @FeatureKey
end

set @ADUserGroup = 'ITStaff'
if (not exists(select * from [2am]..FeatureGroup where FeatureKey = @FeatureKey and ADUserGroup = @ADUserGroup))
	insert into [2am]..[FeatureGroup]([ADUserGroup],[FeatureKey]) values (@ADUserGroup, @FeatureKey)
set @ADUserGroup = 'Branch Consultant'
if (not exists(select * from [2am]..FeatureGroup where FeatureKey = @FeatureKey and ADUserGroup = @ADUserGroup))
	insert into [2am]..[FeatureGroup]([ADUserGroup],[FeatureKey]) values (@ADUserGroup, @FeatureKey)
set @ADUserGroup = 'Branch admin'
if (not exists(select * from [2am]..FeatureGroup where FeatureKey = @FeatureKey and ADUserGroup = @ADUserGroup))
	insert into [2am]..[FeatureGroup]([ADUserGroup],[FeatureKey]) values (@ADUserGroup, @FeatureKey)
set @ADUserGroup = 'Branch Manager'
if (not exists(select * from [2am]..FeatureGroup where FeatureKey = @FeatureKey and ADUserGroup = @ADUserGroup))
	insert into [2am]..[FeatureGroup]([ADUserGroup],[FeatureKey]) values (@ADUserGroup, @FeatureKey)

set @ADUserGroup = 'FL Application Processor'
if (not exists(select * from [2am]..FeatureGroup where FeatureKey = @FeatureKey and ADUserGroup = @ADUserGroup))
	insert into [2am]..[FeatureGroup]([ADUserGroup],[FeatureKey]) values (@ADUserGroup, @FeatureKey)
set @ADUserGroup = 'FL Collections Admin'
if (not exists(select * from [2am]..FeatureGroup where FeatureKey = @FeatureKey and ADUserGroup = @ADUserGroup))
	insert into [2am]..[FeatureGroup]([ADUserGroup],[FeatureKey]) values (@ADUserGroup, @FeatureKey)
set @ADUserGroup = 'FL Supervisor'
if (not exists(select * from [2am]..FeatureGroup where FeatureKey = @FeatureKey and ADUserGroup = @ADUserGroup))
	insert into [2am]..[FeatureGroup]([ADUserGroup],[FeatureKey]) values (@ADUserGroup, @FeatureKey)
set @ADUserGroup = 'FL Manager'
if (not exists(select * from [2am]..FeatureGroup where FeatureKey = @FeatureKey and ADUserGroup = @ADUserGroup))
	insert into [2am]..[FeatureGroup]([ADUserGroup],[FeatureKey]) values (@ADUserGroup, @FeatureKey)

set @ADUserGroup = 'Registrations Manager'
if (not exists(select * from [2am]..FeatureGroup where FeatureKey = @FeatureKey and ADUserGroup = @ADUserGroup))
	insert into [2am]..[FeatureGroup]([ADUserGroup],[FeatureKey]) values (@ADUserGroup, @FeatureKey)
set @ADUserGroup = 'Registrations Supervisor'
if (not exists(select * from [2am]..FeatureGroup where FeatureKey = @FeatureKey and ADUserGroup = @ADUserGroup))
	insert into [2am]..[FeatureGroup]([ADUserGroup],[FeatureKey]) values (@ADUserGroup, @FeatureKey)
set @ADUserGroup = 'Resubmission Admin'
if (not exists(select * from [2am]..FeatureGroup where FeatureKey = @FeatureKey and ADUserGroup = @ADUserGroup))
	insert into [2am]..[FeatureGroup]([ADUserGroup],[FeatureKey]) values (@ADUserGroup, @FeatureKey)

set @ADUserGroup = 'New Business Processor'
if (not exists(select * from [2am]..FeatureGroup where FeatureKey = @FeatureKey and ADUserGroup = @ADUserGroup))
	insert into [2am]..[FeatureGroup]([ADUserGroup],[FeatureKey]) values (@ADUserGroup, @FeatureKey)
set @ADUserGroup = 'New Business Manager'
if (not exists(select * from [2am]..FeatureGroup where FeatureKey = @FeatureKey and ADUserGroup = @ADUserGroup))
	insert into [2am]..[FeatureGroup]([ADUserGroup],[FeatureKey]) values (@ADUserGroup, @FeatureKey)
set @ADUserGroup = 'New Business Supervisor'
if (not exists(select * from [2am]..FeatureGroup where FeatureKey = @FeatureKey and ADUserGroup = @ADUserGroup))
	insert into [2am]..[FeatureGroup]([ADUserGroup],[FeatureKey]) values (@ADUserGroup, @FeatureKey)

set @ContextKey = 3325
if (not exists(select * from [2am]..ContextMenu where ParentKey = @ParentContextKey_Unconfirmed and ContextKey = @ContextKey))
begin
	insert into [2am]..[ContextMenu] ([ContextKey], [CoreBusinessObjectKey], [ParentKey], [Description], [URL], [FeatureKey], [Sequence]) 
	values (@ContextKey, null, @ParentContextKey_Unconfirmed, N'Update Income Contributors', N'AffordabilityAssessmentIncomeContributorsUpdate', @FeatureKey, 2)
end
else
begin
	update [2am]..[ContextMenu]
	set [CoreBusinessObjectKey] = null,
		[ParentKey] = @ParentContextKey_Unconfirmed, 
		[Description] = N'Update Income Contributors', 
		[URL] = N'AffordabilityAssessmentIncomeContributorsUpdate', 
		[FeatureKey] = @FeatureKey, 
		[Sequence] = 2
	where [ContextKey] = @ContextKey
end

set @ContextKey = 3327
if (not exists(select * from [2am]..ContextMenu where ParentKey = @ParentContextKey_Confirmed and ContextKey = @ContextKey))
begin
	insert into [2am]..[ContextMenu] ([ContextKey], [CoreBusinessObjectKey], [ParentKey], [Description], [URL], [FeatureKey], [Sequence]) 
	values (@ContextKey, null, @ParentContextKey_Confirmed, N'Update Income Contributors', N'AffordabilityAssessmentIncomeContributorsUpdate', @FeatureKey, 2)
end
else
begin
	update [2am]..[ContextMenu]
	set [CoreBusinessObjectKey] = null,
		[ParentKey] = @ParentContextKey_Confirmed, 
		[Description] = N'Update Income Contributors', 
		[URL] = N'AffordabilityAssessmentIncomeContributorsUpdate', 
		[FeatureKey] = @FeatureKey, 
		[Sequence] = 2
	where [ContextKey] = @ContextKey
end


------------------------------------------------------------------------
-- insert new flobo node for 'View Historical Affordability Assessments'
------------------------------------------------------------------------
set @FeatureKey = 4125
if (not exists(select * from [2am]..Feature where FeatureKey = @FeatureKey))
begin
	insert into [2am]..[Feature]([ShortName],[LongName],[HasAccess],[ParentKey],[Sequence],[FeatureKey]) 
		values ('View Historical Affordability','View Historical Affordability Assessments',1,4100,1,@FeatureKey)
end

set @CopyFeatureKey = 4100 -- Affordability Assessments
insert into [2AM]..[FeatureGroup]([ADUserGroup],[FeatureKey])
select fg.ADUserGroup,@FeatureKey from [2am]..FeatureGroup fg
left join [2am]..[FeatureGroup] fg2 on fg2.ADUserGroup = fg.ADUserGroup and fg2.FeatureKey = @FeatureKey
where fg.FeatureKey = @CopyFeatureKey and fg2.FeatureGroupKey is null 

set @ParentContextKey = 3300 -- Affordability Assessments
set @ContextKey = 3330 
if (not exists(select * from [2am]..ContextMenu where ParentKey = @ParentContextKey and ContextKey = @ContextKey))
begin
	insert into [2am]..[ContextMenu] ([ContextKey], [CoreBusinessObjectKey], [ParentKey], [Description], [URL], [FeatureKey], [Sequence]) 
	values (@ContextKey, null, @ParentContextKey, N'View Affordability Assessment History', N'AffordabilityAssessmentHistory', @FeatureKey, 3)
end

