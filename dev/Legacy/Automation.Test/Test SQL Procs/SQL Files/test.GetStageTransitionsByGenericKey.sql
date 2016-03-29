USE [2AM]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF EXISTS (select * from dbo.sysobjects where id = object_id(N'test.GetStageTransitionsByGenericKey') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
Begin
	Drop Procedure test.GetStageTransitionsByGenericKey
	Print 'Dropped procedure test.GetStageTransitionsByGenericKey'
End
Go


CREATE PROCEDURE test.GetStageTransitionsByGenericKey

	@GenericKey INT

AS

BEGIN

	SELECT   st.generickey as [GenericKey], 
			 sdg.DESCRIPTION as [StageDefinitionGroup], 
			 sdsdg.stagedefinitionstagedefinitiongroupkey as [SDSDGKey], 
			 sd.DESCRIPTION as [StageDefinition], 
			 st.transitiondate as [TransitionDate],
			 st.endtransitiondate as [EndTransitionDate],
			 sdg.StageDefinitionGroupKey,
			 sd.stageDefinitionKey,
			 st.stageTransitionKey,
			 Comments 
	FROM     [2am].dbo.StageTransition (NOLOCK) st 
			 JOIN [2am].dbo.StageDefinitionStageDefinitionGroup (NOLOCK) sdsdg 
			   ON st.StageDefinitionStageDefinitionGroupKey = sdsdg.StageDefinitionStageDefinitionGroupKey 
			 JOIN [2am].dbo.StageDefinition (NOLOCK) sd 
			   ON sdsdg.StageDefinitionKey = sd.StageDefinitionKey 
			 JOIN [2am].dbo.StageDefinitionGroup (NOLOCK) sdg 
			   ON sdsdg.StageDefinitionGroupKey = sdg.StageDefinitionGroupKey 
	WHERE    st.generickey = @GenericKey 
	GROUP BY st.generickey, 
			 sdsdg.stagedefinitionstagedefinitiongroupkey, 
			 sdg.DESCRIPTION, 
			 sd.DESCRIPTION, 
			 st.transitiondate,
			 st.endtransitiondate,
			 st.stagetransitionkey,			 
			 sdg.StageDefinitionGroupKey,
			 sd.stageDefinitionKey,			 
			 st.stageTransitionKey,
			 Comments   
	ORDER BY st.stagetransitionkey desc
	
END