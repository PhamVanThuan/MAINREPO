USE [2AM]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF EXISTS (select * from dbo.sysobjects where id = object_id(N'test.GetCompositesByGenericKey') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
Begin
	Drop Procedure test.GetCompositesByGenericKey
	Print 'Dropped procedure test.GetCompositesByGenericKey'
End
Go

CREATE PROCEDURE test.GetCompositesByGenericKey

	@GenericKey INT

AS

BEGIN

	SELECT   stc.generickey as [GenericKey], 
			 sdg.DESCRIPTION as [Composite Group], 
			 sdsdg.stagedefinitionstagedefinitiongroupkey as [Composite SDSDGKey], 
			 sd.DESCRIPTION as [Composite Definition], 
			 stc.transitiondate as [Composite Date],
			 st.StageDefinitionStageDefinitionGroupKey as [Composite Stage Transitions],
			 sd_1.DESCRIPTION as [Transition Definition],
			 st.TransitionDate as [Transition Date], 
			 sdc.Sequence as [Composite Sequence],
			 sdc.UseThisDate as [Composite Date Indicator]		 
	FROM     [2am].dbo.StageTransitionComposite (NOLOCK) stc
			JOIN [2am].dbo.StageDefinitionStageDefinitionGroup (NOLOCK) sdsdg 
				ON stc.StageDefinitionStageDefinitionGroupKey = sdsdg.StageDefinitionStageDefinitionGroupKey 
			JOIN [2am].dbo.StageDefinition (NOLOCK) sd 
				ON sdsdg.StageDefinitionKey = sd.StageDefinitionKey 
			JOIN [2am].dbo.StageDefinitionGroup (NOLOCK) sdg 
				ON sdsdg.StageDefinitionGroupKey = sdg.StageDefinitionGroupKey 
			JOIN [2am].dbo.StageTransition (NOLOCK) st 
				ON stc.StageTransitionKey=st.StageTransitionKey
			JOIN [2am].dbo.StageDefinitionComposite (NOLOCK) sdc 
				ON sdsdg.StageDefinitionStageDefinitionGroupKey=sdc.StageDefinitionStageDefinitionGroupCompositeKey
				AND st.StageDefinitionStageDefinitionGroupKey=sdc.StageDefinitionStageDefinitionGroupKey
			JOIN [2am].dbo.StageDefinitionStageDefinitionGroup (NOLOCK) sdsdg_1 
				ON sdc.StageDefinitionStageDefinitionGroupKey = sdsdg_1.StageDefinitionStageDefinitionGroupKey 
			JOIN [2am].dbo.StageDefinition (NOLOCK) sd_1 
				ON sdsdg_1.StageDefinitionKey = sd_1.StageDefinitionKey 
	WHERE    stc.generickey = @GenericKey 
	ORDER BY stc.transitiondate
	
END
GO


