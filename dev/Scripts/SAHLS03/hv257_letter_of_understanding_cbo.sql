use [2am]
go


declare @CoreBusinessObjectKey int
--declare @ParentCoreBusinessObjectKey int
--declare @CoreBusinessObjectKey_Unconfirmed int
--declare @CoreBusinessObjectKey_Confirmed int
declare @ContextKey int
--declare @ParentContextKey int
--declare @ParentContextKey_Unconfirmed int
--declare @ParentContextKey_Confirmed int
declare @FeatureKey int
--declare @ParentFeatureKey int
--declare @CopyFeatureKey int
declare @ADUserGroup varchar(50)

---------------------------------------------------------------------------------
-- Feature & FeatureGroup access for Letter of Understanding Correspondence node
--------------------------------------------------------------------------------
set @FeatureKey = 6150
if (not exists(select * from [2am]..Feature where FeatureKey = @FeatureKey))
begin
	insert into [2am]..[Feature]([ShortName],[LongName],[HasAccess],[ParentKey],[Sequence],[FeatureKey]) 
		values ('Letter of Understanding','Letter of Understanding',1,null,1,@FeatureKey)
end

set @ADUserGroup = 'ITStaff'
if (not exists (select * from [2am]..FeatureGroup where FeatureKey = @FeatureKey and ADUserGroup = @ADUserGroup))
begin
	insert into [2am].[dbo].[FeatureGroup] ([ADUserGroup],[FeatureKey]) values (@ADUserGroup, @FeatureKey)
end

set @ADUserGroup = 'Correspondence Letter of Understanding'
if (not exists (select * from [2am]..FeatureGroup where FeatureKey = @FeatureKey and ADUserGroup = @ADUserGroup))
begin
	insert into [2am].[dbo].[FeatureGroup] ([ADUserGroup],[FeatureKey]) values (@ADUserGroup, @FeatureKey)
end

----------------------------------------------------------------
-- ContextMenu entry Letter of Understanding Correspondence node
----------------------------------------------------------------
set @CoreBusinessObjectKey = 200 -- open loan
set @ContextKey = 2020

if (not exists (select * from [2am]..ContextMenu where ContextKey = @ContextKey))
begin
	insert into [2am].[dbo].[ContextMenu] ([ContextKey],[CoreBusinessObjectKey],[ParentKey],[Description],[URL],[FeatureKey],[Sequence])
		values (@ContextKey, @CoreBusinessObjectKey,null,'Letter of Understanding','Correspondence_LetterOfUnderstanding',@FeatureKey, 13)
end


