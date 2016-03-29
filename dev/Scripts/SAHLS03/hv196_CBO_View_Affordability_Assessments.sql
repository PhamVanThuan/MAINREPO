use [2am]
go

declare @CoreBusinessObjectKey int
declare @ContextKey int
declare @CopyFeatureKey int
declare @FeatureKey int

------------------------------------------------------
-- insert new CBO node for 'Affordability Assessments'
------------------------------------------------------
set @CoreBusinessObjectKey = 6 -- Open Loan Account (Open Parent)
set @ContextKey = 1057 
set @FeatureKey = 4125
if (not exists(select * from [2am]..ContextMenu where CoreBusinessObjectKey = @CoreBusinessObjectKey and ContextKey = @ContextKey))
begin
	insert into [2am]..[ContextMenu] ([ContextKey], [CoreBusinessObjectKey], [ParentKey], [Description], [URL], [FeatureKey], [Sequence]) 
	values (@ContextKey, @CoreBusinessObjectKey, null, N'Affordability Assessments', N'AffordabilityAssessmentHistory', @FeatureKey, 13)
end

set @CoreBusinessObjectKey = 54 -- Prospect Loan
set @ContextKey = 1058 
set @FeatureKey = 4125
if (not exists(select * from [2am]..ContextMenu where CoreBusinessObjectKey = @CoreBusinessObjectKey and ContextKey = @ContextKey))
begin
	insert into [2am]..[ContextMenu] ([ContextKey], [CoreBusinessObjectKey], [ParentKey], [Description], [URL], [FeatureKey], [Sequence]) 
	values (@ContextKey, @CoreBusinessObjectKey, null, N'Affordability Assessments', N'AffordabilityAssessmentHistory', @FeatureKey, 7)
end

----------------------------------------------------------------------
-- insert new CBO node for 'Affordability Assessments' - Personal Loan
----------------------------------------------------------------------
set @FeatureKey = 10045
if (not exists(select * from [2am]..Feature where FeatureKey = @FeatureKey))
begin
	insert into [2am]..[Feature]([ShortName],[LongName],[HasAccess],[ParentKey],[Sequence],[FeatureKey]) 
		values ('View Affordability PL Account','View Affordability Assessments PL Account',1,null,1,@FeatureKey)
end
set @CopyFeatureKey = 1126 -- Affordability Assessments
insert into [2AM]..[FeatureGroup]([ADUserGroup],[FeatureKey])
select fg.ADUserGroup,@FeatureKey from [2am]..FeatureGroup fg
left join [2am]..[FeatureGroup] fg2 on fg2.ADUserGroup = fg.ADUserGroup and fg2.FeatureKey = @FeatureKey
where fg.FeatureKey = @CopyFeatureKey and fg2.FeatureGroupKey is null 

set @CoreBusinessObjectKey = 423 -- Open Personal Loan Account
set @ContextKey = 10265 
if (not exists(select * from [2am]..ContextMenu where CoreBusinessObjectKey = @CoreBusinessObjectKey and ContextKey = @ContextKey))
begin
	insert into [2am]..[ContextMenu] ([ContextKey], [CoreBusinessObjectKey], [ParentKey], [Description], [URL], [FeatureKey], [Sequence]) 
	values (@ContextKey, @CoreBusinessObjectKey, null, N'Affordability Assessments', N'AffordabilityAssessmentHistory', @FeatureKey, 7)
end
