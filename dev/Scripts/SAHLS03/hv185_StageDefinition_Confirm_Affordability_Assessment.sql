USE [2AM]
GO

declare @StageDefinitionKey int
declare @StageDefinitionGroupKey int
declare @StageDefinitionStageDefinitionGroupKey int
set @StageDefinitionKey = 4069
set @StageDefinitionGroupKey = 3155 -- Personal Loan
set @StageDefinitionStageDefinitionGroupKey = 4661

if (not exists(select * from [2am]..StageDefinition where StageDefinitionKey = @StageDefinitionKey))
begin
	insert into [2am].[dbo].[StageDefinition] ([StageDefinitionKey],[Description],[GeneralStatusKey],[IsComposite],[Name],[CompositeTypeName],[HasCompositeLogic])
		values (@StageDefinitionKey,'Confirm Affordability',1,0,'Confirm Affordability',null,0)
end

if (not exists(select * from [2am]..StageDefinitionStageDefinitionGroup where StageDefinitionKey = @StageDefinitionKey and StageDefinitionGroupKey = @StageDefinitionGroupKey))
begin
	insert into [2am].[dbo].[StageDefinitionStageDefinitionGroup] ([StageDefinitionStageDefinitionGroupKey],[StageDefinitionGroupKey],[StageDefinitionKey])
		values (@StageDefinitionStageDefinitionGroupKey,@StageDefinitionGroupKey,@StageDefinitionKey)
end

set @StageDefinitionGroupKey = 631 -- Credit
set @StageDefinitionStageDefinitionGroupKey = 4662
if (not exists(select * from [2am]..StageDefinitionStageDefinitionGroup where StageDefinitionKey = @StageDefinitionKey and StageDefinitionGroupKey = @StageDefinitionGroupKey))
begin
	insert into [2am].[dbo].[StageDefinitionStageDefinitionGroup] ([StageDefinitionStageDefinitionGroupKey],[StageDefinitionGroupKey],[StageDefinitionKey])
		values (@StageDefinitionStageDefinitionGroupKey,@StageDefinitionGroupKey,@StageDefinitionKey)
end