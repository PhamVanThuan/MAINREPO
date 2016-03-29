use [FeTest]
go

IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'PopulateCompositeDefinitions')
	DROP PROCEDURE dbo.PopulateCompositeDefinitions
GO

CREATE PROCEDURE dbo.PopulateCompositeDefinitions

AS

IF(EXISTS(SELECT 1 FROM FETest.dbo.CompositeDefinitions))
	BEGIN
		truncate table FETest.dbo.CompositeDefinitions
	END

INSERT INTO FETest.dbo.CompositeDefinitions 
(CompositeKey, Description, CompositeTransitions, TransitionGroup, TransitionDefinition, DateIndicator, Sequence)
SELECT   SDC.StageDefinitionStageDefinitionGroupCompositeKey AS [Composite Key], 
         SDG_1.DESCRIPTION + ' - ' + SD_1.DESCRIPTION AS [Composite Name], 
         SDC.StageDefinitionStageDefinitionGroupKey	AS [Composite Transitions], 
         SDG.DESCRIPTION AS [Transition Group], 
         SD.DESCRIPTION	AS [Transition Definition], 
         SDC.UseThisDate AS [Date Indicator], 
         SDC.Sequence AS [Sequence] 
FROM     [2am].dbo.StageDefinitionComposite SDC 
         JOIN [2am].dbo.StageDefinitionStageDefinitionGroup SDSDG 
           ON SDC.StageDefinitionStageDefinitionGroupKey = SDSDG.StageDefinitionStageDefinitionGroupKey 
         JOIN [2am].dbo.StageDefinition SD 
           ON SDSDG.StageDefinitionKey = SD.StageDefinitionKey 
         JOIN [2am].dbo.StageDefinitionGroup SDG 
           ON SDSDG.StagedefinitionGroupKey = SDG.StagedefinitionGroupKey 
         JOIN [2am].dbo.StageDefinitionStageDefinitionGroup SDSDG_1 
           ON SDC.StageDefinitionStageDefinitionGroupCompositeKey = SDSDG_1.StageDefinitionStageDefinitionGroupKey 
         JOIN [2am].dbo.StageDefinition SD_1 
           ON SDSDG_1.StageDefinitionKey = SD_1.StageDefinitionKey 
         JOIN [2am].dbo.StageDefinitionGroup SDG_1 
           ON SDSDG_1.StagedefinitionGroupKey = SDG_1.StagedefinitionGroupKey 
ORDER BY SDC.StageDefinitionStageDefinitionGroupCompositeKey, Sequence 