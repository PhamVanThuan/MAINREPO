USE [2AM]
GO

declare @StageDefinitionKey int
declare @StageDefinitionGroupKey int
declare @StageDefinitionStageDefinitionGroupKey int
set @StageDefinitionKey = 4070

if (not exists(select * from [2am]..StageDefinition where StageDefinitionKey = @StageDefinitionKey))
begin
	insert into [2am].[dbo].[StageDefinition] ([StageDefinitionKey],[Description],[GeneralStatusKey],[IsComposite],[Name],[CompositeTypeName],[HasCompositeLogic])
		values (@StageDefinitionKey,'Disqualify for GEPF',1,0,'Disqualify for GEPF',null,0)
end

set @StageDefinitionGroupKey = 631 -- Credit
set @StageDefinitionStageDefinitionGroupKey = 4663
if (not exists(select * from [2am]..StageDefinitionStageDefinitionGroup where StageDefinitionKey = @StageDefinitionKey and StageDefinitionGroupKey = @StageDefinitionGroupKey))
begin
	insert into [2am].[dbo].[StageDefinitionStageDefinitionGroup] ([StageDefinitionStageDefinitionGroupKey],[StageDefinitionGroupKey],[StageDefinitionKey])
		values (@StageDefinitionStageDefinitionGroupKey,@StageDefinitionGroupKey,@StageDefinitionKey)
end