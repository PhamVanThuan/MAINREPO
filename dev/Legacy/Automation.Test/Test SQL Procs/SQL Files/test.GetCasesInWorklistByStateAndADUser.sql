
USE [2AM]
GO
/****** Object:  StoredProcedure [test].[GetCasesInWorklistByStateAndType]    Script Date: 10/19/2010 16:48:58 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[test].[GetCasesInWorklistByStateAndADUser]') AND type in (N'P', N'PC'))
DROP PROCEDURE [test].[GetCasesInWorklistByStateAndADUser]

GO

CREATE PROCEDURE test.GetCasesInWorklistByStateAndADUser
	@State VARCHAR(50), 
	@WorkFlow VARCHAR(50),
	@ADUserName VARCHAR(50),
	@OfferKey INT
AS
BEGIN

	DECLARE  @WorkflowID INT 
	DECLARE  @StateID INT 

	--FETCH THE LATEST WORKFLOW ID
	SELECT @WorkflowID = MAX(id) FROM   x2.x2.workflow WHERE  name = @WorkFlow

	--FIND THE STATE ID FOR THE WORKLOW/STATE
	SELECT @StateID = s.id FROM   x2.x2.state s WHERE  workflowid = @WorkflowID AND s.name = @State

	SELECT wl.*
	FROM     [X2].[X2].[Worklist] wl WITH (NOLOCK) 
			 JOIN [X2].[X2].[Instance] i WITH (NOLOCK) 
			   ON wl.instanceid = i.id 
			 JOIN [2am]..[Offer] o WITH (NOLOCK) 
			   ON CONVERT(VARCHAR,o.offerkey) = i.[Name] 
	WHERE  
		i.workflowid = @WorkflowID  
			and 
		wl.adusername = @ADUserName
			and
		i.stateid = @StateID 
			AND 
		o.OfferKey = @OfferKey

END
