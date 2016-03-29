USE EventProjection
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS (
				SELECT DISTINCT ta.Name 
				FROM EventProjection.sys.triggers t
				INNER JOIN EventProjection.sys.tables ta ON ta.object_id = t.parent_id
				WHERE ta.Name LIKE '%CurrentlyAssignedUserForInstance%'
			   )


IF OBJECT_ID('projection.tu_CurrentlyAssignedUserForInstance') is null
BEGIN 
	DECLARE @Qry VARCHAR(1024)
	SET @Qry =	'CREATE TRIGGER projection.tu_CurrentlyAssignedUserForInstance ON  projection.CurrentlyAssignedUserForInstance ' +
				'FOR UPDATE ' +
				'AS ' +
				'BEGIN ' +
				'SET NOCOUNT ON; ' +
				'END '

	EXECUTE (@Qry)
END
GO

ALTER TRIGGER projection.tu_CurrentlyAssignedUserForInstance ON  projection.CurrentlyAssignedUserForInstance
FOR UPDATE 
AS 

SET NOCOUNT ON

/*************************************************************************************************************************
		Author		:	VirekR
		CreateDate	:	2015/08/25
		Description	:	Passing the ThirdPartyInvoiceKey that was updated to service broker to enable Solr Index to be updated.
						Passed via a temp table and a proc that will determine which queue to pass to.
																																											
		
**************************************************************************************************************************/
BEGIN
			/* Lets get the ThirdPartyInvoiceKey and pass it to service broker to initiate a conversation */			SELECT TOP 0 * INTO #SolrIndexUpdate FROM Process.template.SolrIndexUpdate		INSERT INTO	#SolrIndexUpdate (ProcessStatusKey, GenericKey, GenericKeyTypeKey)	SELECT		1, CONVERT(XML, CONVERT(NVARCHAR(MAX), GenericKey)),GenericKeyTypeKey	--	SELECT * FROM [2am].dbo.GenericKeyType
	FROM		INSERTED i	WHERE		GenericKeyTypeKey = 54  	   	 		/**********************************************************/		EXEC process.solr.pServiceBrokerQueueDetermine	/**********************************************************/ 
END --END TRIGGER
GO
