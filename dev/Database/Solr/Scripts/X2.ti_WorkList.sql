USE X2
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF OBJECT_ID('X2.ti_WorkList') is null
BEGIN 
	DECLARE @Qry VARCHAR(1024)
	SET @Qry =	'CREATE TRIGGER X2.ti_WorkList ON  X2.WorkList ' +
				'FOR INSERT ' +
				'AS ' +
				'BEGIN ' +
				'SET NOCOUNT ON; ' +
				'END '

	EXECUTE (@Qry)
END
GO

ALTER TRIGGER X2.ti_WorkList ON  [X2].[WorkList]
  FOR INSERT

AS 

/*************************************************************************************************************************
		Author		:	VirekR
		CreateDate	:	2015/06/25
		Description	:	Passing the instance ID that was updated to service broker to enable Solr Index to be updated.
						Passed via a temp table and a proc that will determine which queue to pass to.
																																														
		
**************************************************************************************************************************/
	SET NOCOUNT ON;

BEGIN	
			
	SELECT TOP 0 * INTO #SolrIndexUpdate FROM Process.template.SolrIndexUpdate

	INSERT INTO	#SolrIndexUpdate (ProcessStatusKey, GenericKey, GenericKeyTypeKey)
	SELECT		1,CONVERT(XML, CONVERT(NVARCHAR(MAX), i.InstanceID)),18 -- SELECT * FROM [2am].dbo.GenericKeyType
	FROM		INSERTED i  		
	
	/**********************************************************/
		EXEC process.solr.pServiceBrokerQueueDetermine
	/**********************************************************/

END --END TRIGGER
GO