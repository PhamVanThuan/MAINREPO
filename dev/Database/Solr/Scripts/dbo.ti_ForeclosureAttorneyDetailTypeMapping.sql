USE [2am]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF OBJECT_ID('dbo.ti_ForeclosureAttorneyDetailTypeMapping') is null
BEGIN 
	DECLARE @Qry VARCHAR(1024)
	SET @Qry =	'CREATE TRIGGER dbo.ti_ForeclosureAttorneyDetailTypeMapping ON  dbo.ForeclosureAttorneyDetailTypeMapping ' +
				'FOR INSERT ' +
				'AS ' +
				'BEGIN ' +
				'SET NOCOUNT ON; ' +
				'END '

	EXECUTE (@Qry)
END
GO

ALTER TRIGGER dbo.ti_ForeclosureAttorneyDetailTypeMapping ON  dbo.ForeclosureAttorneyDetailTypeMapping
  FOR INSERT

AS 

/*************************************************************************************************************************
		Author		:	VirekR
		CreateDate	:	2015/06/23
		Description	:	Passing the legalentitykey that was updated to service broker to enable
						Solr Index to be updated.
						Passed via a temp table and a proc that will determine which queue to pass to.
																																														
		
**************************************************************************************************************************/
	SET NOCOUNT ON;

BEGIN	
			
	INSERT INTO warehouse.dbo.[2am_AuditForeclosureAttorneyDetailTypeMapping]
	(		
		AuditDate	,
		AuditAddUpdateDelete ,
		ForeclosureAttorneyDetailTypeMappingKey ,
		LegalEntityKey		,
		DetailTypeKey		,
		GeneralStatusKey	
	)
	SELECT 
			GETDATE(),
			'I',
			ForeclosureAttorneyDetailTypeMappingKey,
			LegalEntityKey		,
			DetailTypeKey		,
			GeneralStatusKey	
	FROM INSERTED
	
	SELECT TOP 0 * INTO #SolrIndexUpdate FROM Process.template.SolrIndexUpdate

	INSERT INTO	#SolrIndexUpdate (ProcessStatusKey, GenericKey, GenericKeyTypeKey)
	SELECT		1,CONVERT(XML, CONVERT(NVARCHAR(MAX), i.LegalEntityKey)),3 -- SELECT * FROM [2am].dbo.GenericKeyType
	FROM		INSERTED i  		
	
	/**********************************************************/		EXEC process.solr.pServiceBrokerQueueDetermine	/**********************************************************/

END --END TRIGGER
GO
