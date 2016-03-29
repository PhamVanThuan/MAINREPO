USE [Process]
GO
/****** Object:  StoredProcedure [solr].[pServiceBrokerQueueDetermine]    Script Date: 2014-12-24 11:10:47 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF OBJECT_ID('solr.pServiceBrokerQueueDetermine') is null
BEGIN 
	DECLARE @Qry VARCHAR(1024)
	SET @Qry =	'CREATE PROCEDURE solr.pServiceBrokerQueueDetermine ' +
				'AS ' +
				'BEGIN ' +
				'SET NOCOUNT ON; ' +
				'END '

	EXECUTE (@Qry)
END
GO

ALTER PROCEDURE solr.pServiceBrokerQueueDetermine
AS

/*************************************************************************************************************************
		Author		:	VirekR
		CreateDate	:	2015/08/20
		Description	:	This proc will differentiate which queue the message will go to based on the generic key type
																				
		History:
					
		EXEC process.solr.pServiceBrokerQueueDetermine
	**************************************************************************************************************************/

BEGIN
			
	BEGIN TRANSACTION
		
	DECLARE		@FromService	VARCHAR(100),
				@ToService		VARCHAR(100),
				@Contract		VARCHAR(100),
				@MessageType	VARCHAR(100)

	IF OBJECT_ID('tempdb.dbo.#SBQueue') IS NOT NULL
		DROP TABLE #SBQueue 

	CREATE TABLE #SBQueue
				(
					ID					INT IDENTITY(1,1),
					IndexName			VARCHAR(50),
					IndexStatusKey		INT DEFAULT(1),
					GenericKeyTypeKey	INT,
					FromService			VARCHAR(100),
					ToService			VARCHAR(100),
					[Contract]			VARCHAR(100),
					MessageType			VARCHAR(100)
				)

	INSERT INTO #SBQueue (IndexName, GenericKeyTypeKey, FromService, ToService, [Contract], MessageType)
	SELECT	'ThirdParty'			, 3, 'ThirdPartyInitiatorService'		, 'ThirdPartyTargetService'			, 'ThirdPartyContract'			, 'ThirdPartyInitiatorMessage'		 UNION
	SELECT	'Client'				, 3, 'ClientInitiatorService'			, 'ClientTargetService'				, 'ClientContract'				, 'ClientInitiatorMessage'			 UNION
	SELECT	'Task'					, 18,'TaskInitiatorService'				, 'TaskTargetService'				, 'TaskContract'				, 'TaskInitiatorMessage'			 UNION
	SELECT	'ThirdPartyInvoice'		, 54,'ThirdPartyInvoiceInitiatorService', 'ThirdPartyInvoiceTargetService'	, 'ThirdPartyInvoiceContract'   , 'ThirdPartyInvoiceInitiatorMessage'			

	--	SELECT * FROM #SBQueue

	-- Here we need to distinguish between a client and a third party who both share the same generickeytype
	IF (SELECT DISTINCT GenericKeyTypeKey FROM #SolrIndexUpdate) = 3
		BEGIN
			-- Is this LegalEntity that is being updated a ThirdParty? If so, initiate the ThirdParty Service Broker message else do the Client one
			IF EXISTS	(
							SELECT	t.LegalEntityKey
							FROM	#SolrIndexUpdate s
							INNER JOIN [2am].dbo.ThirdParty t (NOLOCK) ON t.LegalEntityKey = CAST(s.GenericKey AS NVARCHAR)	
						)
				BEGIN
					UPDATE	s
					SET		s.IndexStatusKey = 2 --Inactive
					FROM	#SBQueue s
					WHERE	IndexName = 'Client'
				END -- END IF
			ELSE
				BEGIN
					UPDATE	s
					SET		s.IndexStatusKey = 2 --Inactive
					FROM	#SBQueue s
					WHERE	IndexName = 'ThirdParty'
				END	--END Else
		END -- END GenericKeyType 
	
	-- We want to only have a list of active queues to send messages to. If it has been deactivated it would mean that it does not relate to the generic key being passed in
	SELECT	@FromService =	s.FromService	, 
			@ToService	 =	s.ToService		, 
			@Contract	 =	s.[Contract]	, 
			@MessageType =	s.MessageType
	FROM	#SBQueue s
	INNER JOIN #SolrIndexUpdate i ON i.GenericKeyTypeKey = s.GenericKeyTypeKey
	WHERE	s.IndexStatusKey = 1 --Active

	--	SELECT @FromService ,@ToService ,@Contract ,@MessageType

	-- Call a generic proc which will initiate a service broker conversation
	/*****************************************************************************************************/
		EXEC [2am].dbo.pInitiateServiceBrokerMessage @FromService ,@ToService ,@Contract ,@MessageType
	/*****************************************************************************************************/

COMMIT;

END

GO

PRINT 'solr.pServiceBrokerQueueDetermine deployed: ' + cast(getdate() as varchar) + ' to server: '+ @@Servername
GO

GRANT EXECUTE ON OBJECT::solr.pServiceBrokerQueueDetermine TO [Batch];
GRANT EXECUTE ON OBJECT::solr.pServiceBrokerQueueDetermine TO [ProcessRole];
GRANT EXECUTE ON OBJECT::solr.pServiceBrokerQueueDetermine TO [AppRole];
