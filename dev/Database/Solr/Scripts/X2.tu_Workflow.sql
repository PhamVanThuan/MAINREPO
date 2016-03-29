USE X2
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF OBJECT_ID('X2.tu_Workflow') is null
BEGIN 
	DECLARE @Qry VARCHAR(1024)
	SET @Qry =	'CREATE TRIGGER X2.tu_Workflow ON  X2.Workflow ' +
				'FOR UPDATE ' +
				'AS ' +
				'BEGIN ' +
				'SET NOCOUNT ON; ' +
				'END '

	EXECUTE (@Qry)
END
GO

ALTER TRIGGER X2.tu_Workflow ON  X2.Workflow
  FOR UPDATE

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
	SELECT		1,CONVERT(XML, CONVERT(NVARCHAR(MAX), ins.ID)),18 -- SELECT * FROM [2am].dbo.GenericKeyType
	FROM		INSERTED i
	INNER JOIN	x2.X2.Instance ins (NOLOCK)  ON ins.WorkFlowID = i.ID 		
	
	/**********************************************************/
		EXEC process.solr.pServiceBrokerQueueDetermine
	/**********************************************************/

END --END TRIGGER
GO
