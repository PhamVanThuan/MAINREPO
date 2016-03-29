USE [2AM]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF EXISTS (select * from dbo.sysobjects where id = object_id(N'test.GetCompositeDefinition') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
Begin
	Drop Procedure test.GetCompositeDefinition
	Print 'Dropped procedure test.GetCompositeDefinition'
End
Go

CREATE PROCEDURE test.GetCompositeDefinition 

	@SDSDGCKey INT
	
AS

BEGIN

SELECT   SDC.StageDefinitionStageDefinitionGroupCompositeKey AS [Composite Key], 
         SDG_1.DESCRIPTION + ' - ' + SD_1.DESCRIPTION AS [Composite Name], 
         SDC.StageDefinitionStageDefinitionGroupKey	AS [Composite Transitions], 
         SDG.DESCRIPTION AS [Transition Group], 
         SD.DESCRIPTION	AS [Transition Definition], 
         SDC.UseThisDate AS [Date Indicator], 
         SDC.Sequence AS [Sequence] 
FROM     StageDefinitionComposite SDC 
         JOIN StageDefinitionStageDefinitionGroup SDSDG 
           ON SDC.StageDefinitionStageDefinitionGroupKey = SDSDG.StageDefinitionStageDefinitionGroupKey 
         JOIN StageDefinition SD 
           ON SDSDG.StageDefinitionKey = SD.StageDefinitionKey 
         JOIN StageDefinitionGroup SDG 
           ON SDSDG.StagedefinitionGroupKey = SDG.StagedefinitionGroupKey 
         JOIN StageDefinitionStageDefinitionGroup SDSDG_1 
           ON SDC.StageDefinitionStageDefinitionGroupCompositeKey = SDSDG_1.StageDefinitionStageDefinitionGroupKey 
         JOIN StageDefinition SD_1 
           ON SDSDG_1.StageDefinitionKey = SD_1.StageDefinitionKey 
         JOIN StageDefinitionGroup SDG_1 
           ON SDSDG_1.StagedefinitionGroupKey = SDG_1.StagedefinitionGroupKey 
WHERE    SDC.StageDefinitionStageDefinitionGroupCompositeKey = @SDSDGCKey 
ORDER BY SDC.Sequence 

END