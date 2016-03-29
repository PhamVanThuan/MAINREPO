use [2am]
go

declare @StageDefinitionGroupKey int
declare @GenericKeyTypeKey int
declare @Description varchar(250)
set @StageDefinitionGroupKey = 3157
set @GenericKeyTypeKey = 54
set @Description = 'Third Party Invoices'

if not exists (select 1 from dbo.StageDefinitionGroup where StageDefinitionGroupKey = @StageDefinitionGroupKey
and Description = @Description)

begin
	insert into dbo.StageDefinitionGroup
	(StageDefinitionGroupKey, Description, GenericKeyTypeKey, GeneralStatusKey, ParentKey)
	values
	(@StageDefinitionGroupKey, @Description, @GenericKeyTypeKey, 1, NULL)
end

--SDSDGKey for Create Case
declare @StageDefinitionKey int
declare @StageDefinitionStageDefinitionGroupKey int
set @StageDefinitionKey = 220
set @StageDefinitionStageDefinitionGroupKey = 4643

if not exists (select 1 from dbo.StageDefinitionStageDefinitionGroup 
where StageDefinitionStageDefinitionGroupKey = @StageDefinitionStageDefinitionGroupKey and StageDefinitionGroupKey = @StageDefinitionGroupKey)

begin

	insert into dbo.StageDefinitionStageDefinitionGroup
	(StageDefinitionGroupKey, StageDefinitionKey, StageDefinitionStageDefinitionGroupKey)
	values
	(@StageDefinitionGroupKey, @StageDefinitionKey, @StageDefinitionStageDefinitionGroupKey)

end

