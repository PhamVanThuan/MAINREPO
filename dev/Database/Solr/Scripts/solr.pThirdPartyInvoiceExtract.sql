USE Process

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

IF OBJECT_ID('solr.pThirdPartyInvoiceExtract') is null
BEGIN 
	DECLARE @Qry VARCHAR(1024)
	SET @Qry =	'CREATE PROCEDURE solr.pThirdPartyInvoiceExtract ' +
				'AS ' +
				'BEGIN ' +
				'SET NOCOUNT ON; ' +
				'END '

	EXECUTE (@Qry)
END
GO

ALTER PROCEDURE solr.pThirdPartyInvoiceExtract @Msg VARCHAR(1024) OUTPUT, @ThirdPartyInvoiceKey INT = NULL 
AS

/*************************************************************************************************************************
		Author		:	VirekR
		CreateDate	:	2015/07/30
		Description	:	This proc will:

						1. Return third party invoice information												
						2. Either run for one ThirdPartyInvoice or for all
						3. Populate [2am].solr.ThirdPartyInvoice table with Third Party Invoice related information
															
																				
		History:
					
	--------------------------------------------------------------------	
	--Helper Code:	
			DECLARE @msg VARCHAR(1024)
			/*Optional Parameter of ThirdPartyInvoiceKey, if populated do that one only, if not do all */  
			EXEC Process.solr.pThirdPartyInvoiceExtract @msg OUTPUT--,10
			SELECT @msg
	--------------------------------------------------------------------
**************************************************************************************************************************/
BEGIN
	BEGIN TRY

	--	DECLARE	 @msg VARCHAR(255)
	--			,@ThirdPartyInvoiceKey INT --= 765

	IF OBJECT_ID('tempdb.dbo.#InvoiceInfo') IS NOT NULL
			DROP TABLE #InvoiceInfo

	CREATE TABLE #InvoiceInfo 
		 (
		ThirdPartyInvoiceKey	INT
		,SahlReference			VARCHAR(100)
		,InvoiceStatusKey		INT
		,InvoiceStatusDescription VARCHAR(100)
		,AccountKey				INT
		,ThirdPartyID			VARCHAR(100)
		,InvoiceNumber			VARCHAR(100)
		,InvoiceDate			DATETIME
		,ReceivedFromEmailAddress VARCHAR(100)
		,AmountExcludingVAT		DECIMAL(22,10)
		,VATAmount				DECIMAL(22,10)
		,TotalAmountIncludingVAT DECIMAL(22,10)
		,CapitaliseInvoice		BIT
		,ReceivedDate			DATETIME
		,SpvDescription			VARCHAR(100)
		,WorkflowProcess		VARCHAR(100)
		,WorkflowStage			VARCHAR(100)
		,AssignedTo				VARCHAR(100)
		,ThirdParty				VARCHAR(100)
		,InstanceID				VARCHAR(50)
		,GenericKey				VARCHAR(50)
		,DocumentGuid			VARCHAR(100)		
		)

		
		INSERT INTO #InvoiceInfo (ThirdPartyInvoiceKey	
								,SahlReference			
								,InvoiceStatusKey	
								,InvoiceStatusDescription	
								,AccountKey				
								,ThirdPartyID			
								,InvoiceNumber			
								,InvoiceDate			
								,ReceivedFromEmailAddress 
								,AmountExcludingVAT		
								,VATAmount				
								,TotalAmountIncludingVAT 
								,CapitaliseInvoice		
								,ReceivedDate			
								,SpvDescription			
								,WorkflowProcess		
								,WorkflowStage			
								,AssignedTo				
								,ThirdParty
								,InstanceID
								,GenericKey
								,DocumentGuid
								)	
		SELECT tpi.ThirdPartyInvoiceKey
				,tpi.SahlReference
				,tpi.InvoiceStatusKey
				,st.Description							 AS 'InvoiceStatusDescription'
				,tpi.AccountKey
				,tpi.ThirdPartyID
				,tpi.InvoiceNumber
				,tpi.InvoiceDate
				,tpi.ReceivedFromEmailAddress
				,tpi.AmountExcludingVAT
				,tpi.VATAmount
				,tpi.TotalAmountIncludingVAT
				,tpi.CapitaliseInvoice
				,tpi.ReceivedDate				
				,spv_parent.Description							 AS 'SpvDescription'
				,COALESCE(Acc.eMapName, 'N/A')			 AS 'WorkflowProcess'
				,COALESCE(Acc.eStageName, 'N/A')		 AS 'WorkflowStage'
				,COALESCE(LA.Username, 'Unassigned')	 AS 'AssignedTo'
				,ISNULL(le.RegisteredName, 'Unassigned') AS 'ThirdPartyInvoice'
				,ISNULL(CAST(x2tpi.InstanceID AS VARCHAR), '') AS 'InstanceID'
				,ISNULL(CAST(x2tpi.GenericKey AS VARCHAR),'') AS 'GenericKey'
				,ISNULL(CAST(stor.DocumentGuid AS VARCHAR(100)),'')  AS 'DocumentGuid'
		FROM		[2AM].dbo.ThirdPartyInvoice tpi (NOLOCK)
		INNER JOIN  [2am].dbo.InvoiceStatus st (NOLOCK) ON tpi.InvoiceStatusKey = st.InvoiceStatusKey
		INNER JOIN	[2AM].dbo.Account a (NOLOCK) ON tpi.AccountKey = a.AccountKey
		INNER JOIN	[2am].spv.spv s		(NOLOCK) ON a.SPVKey = s.SPVKey
		INNER JOIN  [2AM].spv.spv spv_parent (NOLOCK) ON s.ParentSPVKey = spv_parent.SPVKey
		INNER JOIN	[X2].X2DATA.Third_Party_Invoices x2tpi ON tpi.ThirdPartyInvoiceKey = x2tpi.ThirdPartyInvoiceKey
		LEFT JOIN	[2AM].datastor.ThirdPartyInvoicesSTOR stor (NOLOCK) ON tpi.ThirdPartyInvoiceKey = stor.ThirdPartyInvoiceKey
						and tpi.AccountKey = stor.AccountKey
		LEFT JOIN	EventProjection.projection.CurrentlyAssignedUserForInstance la (NOLOCK) ON LA.GenericKey = tpi.ThirdPartyInvoiceKey
											AND LA.GenericKeyTypeKey = 54
		LEFT JOIN [2am].dbo.UserOrganisationStructure uos (NOLOCK) ON uos.OrganisationStructureKey = LA.UserOrganisationStructureKey
		LEFT JOIN [2am].dbo.ThirdParty tp	(NOLOCK) ON tpi.ThirdPartyId = tp.Id
											AND tp.GenericKeyTypeKey = 35
		LEFT JOIN [2am].dbo.LegalEntity le	(NOLOCK) ON tp.LegalEntityKey = le.LegalEntityKey		
		LEFT JOIN (
						SELECT	eFolderId
								,COALESCE(f.eFolderName, '''') AS AccountKey
								,ROW_NUMBER() OVER (
									PARTITION BY coalesce(f.eFolderName, '''') ORDER BY f.ecreationtime DESC
									) AS Row
								,f.eMapName
								,f.eStageName
						FROM	[e-work].dbo.eFolder f (NOLOCK)
						WHERE	f.eMapName = 'LossControl'
						AND		ISNUMERIC(coalesce(f.eFolderName, '''')) = 1
					) AS Acc ON a.AccountKey = Acc.AccountKey
			AND Acc.Row = 1
		WHERE (
				tpi.ThirdPartyInvoiceKey = @ThirdPartyInvoiceKey
			OR @ThirdPartyInvoiceKey IS NULL
			)
	
	IF @ThirdPartyInvoiceKey IS NOT NULL 
		BEGIN
			IF OBJECT_ID('[2am].solr.ThirdPartyInvoice') IS NOT NULL
				BEGIN
					DELETE 
					FROM	[2AM].solr.ThirdPartyInvoice
					WHERE	ThirdPartyInvoiceKey = @ThirdPartyInvoiceKey
				END
		END
		
		ELSE
		/*
		This piece of code will execute when we are NOT passing through a parameter 
		which means clean out the table and reload the full batch.

		If Exists then clean out the table.
		*/
		BEGIN
			IF OBJECT_ID('[2am].solr.ThirdPartyInvoice') IS NOT NULL
				TRUNCATE TABLE [2am].solr.ThirdPartyInvoice			
		END

		INSERT INTO [2AM].solr.ThirdPartyInvoice (
												ThirdPartyInvoiceKey	
												,SahlReference			
												,InvoiceStatusKey		
												,InvoiceStatusDescription
												,AccountKey				
												,ThirdPartyID			
												,InvoiceNumber			
												,InvoiceDate			
												,ReceivedFromEmailAddress 
												,AmountExcludingVAT		
												,VATAmount				
												,TotalAmountIncludingVAT 
												,CapitaliseInvoice		
												,ReceivedDate			
												,SpvDescription			
												,WorkflowProcess		
												,WorkflowStage			
												,AssignedTo				
												,ThirdParty
												,InstanceID
												,GenericKey
												,LastModifiedDate
												,DocumentGuid
												)
		SELECT	DISTINCT ThirdPartyInvoiceKey	
				,SahlReference			
				,InvoiceStatusKey	
				,InvoiceStatusDescription	
				,AccountKey				
				,ThirdPartyID			
				,InvoiceNumber			
				,InvoiceDate			
				,ReceivedFromEmailAddress 
				,AmountExcludingVAT		
				,VATAmount				
				,TotalAmountIncludingVAT 
				,CapitaliseInvoice		
				,ReceivedDate			
				,SpvDescription			
				,WorkflowProcess		
				,WorkflowStage			
				,AssignedTo				
				,ThirdParty
				,InstanceID
				,GenericKey
				,GETDATE()
				,DocumentGuid
		FROM	#InvoiceInfo
		ORDER BY	ThirdPartyInvoiceKey 
		
						
		
		SELECT * FROM [2AM].solr.ThirdPartyInvoice (NOLOCK) 
		WHERE ( ThirdPartyInvoiceKey = @ThirdPartyInvoiceKey 
			OR	@ThirdPartyInvoiceKey IS NULL )
		ORDER BY ID



		--	select * from [2AM].solr.ThirdPartyInvoice (NOLOCK) WHERE ThirdPartyInvoiceKey = 474648 and IndexText LIKE '%999563%'
			
		 		
		END TRY

		BEGIN CATCH

	
			set @Msg = 'solr.pThirdPartyInvoiceExtract: ' + ISNULL(ERROR_MESSAGE(), 'Failed!')
			RAISERROR(@Msg,16,1)

			SELECT TOP 0 * INTO #Errors FROM process.template.errors
	
			DELETE FROM #Errors
			INSERT INTO #Errors (ErrorCodeKey, DateOfError, MSG, SeverityTypeKey)
			SELECT (SELECT ErrorCodeKey FROM process.errorhandling.ErrorCode (NOLOCK) WHERE Description LIKE 'Solr Extract Failure'), GETDATE(), @Msg, 1
	
			EXEC process.errorhandling.pLogErrors @Msg OUTPUT
				
		END CATCH

		--	EXEC Process.solr.pThirdPartyInvoiceExtract '',474642
END --End Proc
GO

PRINT 'solr.pThirdPartyInvoiceExtract deployed: ' + CAST(GETDATE() AS VARCHAR) + ' to server: '+ @@Servername
GO

GRANT EXECUTE ON OBJECT::solr.pThirdPartyInvoiceExtract TO [Batch];
GRANT EXECUTE ON OBJECT::solr.pThirdPartyInvoiceExtract TO [ProcessRole];
GRANT EXECUTE ON OBJECT::solr.pThirdPartyInvoiceExtract TO [AppRole];
