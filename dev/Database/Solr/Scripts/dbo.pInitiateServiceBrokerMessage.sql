USE [2am]
GO

IF OBJECT_ID('dbo.pInitiateServiceBrokerMessage') is null
BEGIN 
	DECLARE @Qry VARCHAR(1024)
	SET @Qry =	'CREATE PROCEDURE dbo.pInitiateServiceBrokerMessage ' +
				'AS ' +
				'BEGIN ' +
				'SET NOCOUNT ON; ' +
				'END '

	EXECUTE (@Qry)
END
GO

ALTER PROCEDURE dbo.pInitiateServiceBrokerMessage 
									@FromService	SYSNAME
									,@ToService		SYSNAME
									,@Contract		SYSNAME
									,@MessageType	SYSNAME									
AS

/*************************************************************************************************************************
		Author		:	VirekR
		CreateDate	:	2015/01/20
		Description	:	This proc will initiate a conversation and pass through the unique key which will eventually 
						be consumed by a CLR Proc 
																																									
		History:
					
	Inputs:	
		EXEC [2am].dbo.pInitiateServiceBrokerMessage 
								@FromService  = 'ThirdPartyInitiatorService'
								,@ToService   = 'ThirdPartyTargetService'
								,@Contract    = 'ThirdPartyContract'
								,@MessageType = 'ThirdPartyInitiatorMessage'
**************************************************************************************************************************/
BEGIN
	
  SET NOCOUNT ON;
 
  DECLARE @conversation_handle	UNIQUEIDENTIFIER,
		  @Message				XML,
		  @Count				INT = 1
   	
  BEGIN TRY
	  BEGIN TRANSACTION;
 
	  
	  IF (SELECT COUNT(*) FROM #SolrIndexUpdate) > 0
	  BEGIN
		  -- Loop through all records waiting to be processed and initiate a message for each of them	
		  WHILE @Count <= (SELECT COUNT(*) FROM #SolrIndexUpdate)
		  BEGIN

			  BEGIN DIALOG CONVERSATION @conversation_handle
				FROM SERVICE @FromService
				TO SERVICE @ToService
				ON CONTRACT @Contract
				WITH ENCRYPTION = OFF;
 		
				SELECT @Message =(	SELECT	GenericKey 
									FROM	#SolrIndexUpdate 
									WHERE	ID = @Count
									FOR XML PATH) ;	
	  		
				--SELECT @Message;

				SEND ON CONVERSATION @conversation_handle
				MESSAGE TYPE @MessageType(@Message);
		  		
				SELECT @Count = @Count + 1		  
		   END -- END WHILE
	   END --END IF
	  COMMIT TRANSACTION;
  END TRY
  BEGIN CATCH
		ROLLBACK TRANSACTION
  END CATCH

END -- END PROC
GO 

PRINT '[2am].dbo.pInitiateServiceBrokerMessage deployed: ' + cast(getdate() as varchar) + ' to server: '+ @@Servername
GO

GRANT EXECUTE ON dbo.pInitiateServiceBrokerMessage TO [AppRole]
