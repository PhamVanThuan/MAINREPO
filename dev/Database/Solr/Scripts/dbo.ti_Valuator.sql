USE [2am]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

--IF NOT EXISTS (
--				SELECT DISTINCT ta.Name 
--				FROM [2am].sys.triggers t
--				INNER JOIN [2am].sys.tables ta ON ta.object_id = t.parent_id
--				WHERE ta.Name LIKE '%Valuator%'
--			   )


IF OBJECT_ID('dbo.ti_Valuator') is null
BEGIN 
	DECLARE @Qry VARCHAR(1024)
	SET @Qry =	'CREATE TRIGGER dbo.ti_Valuator ON  dbo.Valuator ' +
				'FOR INSERT ' +
				'AS ' +
				'BEGIN ' +
				'SET NOCOUNT ON; ' +
				'END '

	EXECUTE (@Qry)
END
GO

ALTER TRIGGER dbo.ti_Valuator ON  dbo.Valuator
FOR INSERT 
AS 

SET NOCOUNT ON

/*************************************************************************************************************************
		Author		:	VirekR
		CreateDate	:	2015/03/25
		Description	:	Passing the legalentitykey that was updated to service broker to enable	Solr Index to be updated.
						Passed via a temp table and a proc that will determine which queue to pass to.
																																											
		
**************************************************************************************************************************/
BEGIN
			/* Lets get the LegalEntityKey and pass it to service broker to initiate a conversation */			SELECT TOP 0 * INTO #SolrIndexUpdate FROM Process.template.SolrIndexUpdate		INSERT INTO	#SolrIndexUpdate (ProcessStatusKey, GenericKey, GenericKeyTypeKey)	SELECT		1, CONVERT(XML, CONVERT(NVARCHAR(MAX), LegalEntityKey)),3 --	SELECT * FROM [2am].dbo.GenericKeyType
	FROM		INSERTED i  	   	 		/**********************************************************/		EXEC process.solr.pServiceBrokerQueueDetermine	/**********************************************************/ 
END --END TRIGGER
GO
