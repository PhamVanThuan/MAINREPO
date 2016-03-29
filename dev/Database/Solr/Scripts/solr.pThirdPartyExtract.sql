USE Process

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

IF OBJECT_ID('solr.pThirdPartyExtract') is null
BEGIN 
	DECLARE @Qry VARCHAR(1024)
	SET @Qry =	'CREATE PROCEDURE solr.pThirdPartyExtract ' +
				'AS ' +
				'BEGIN ' +
				'SET NOCOUNT ON; ' +
				'END '

	EXECUTE (@Qry)
END
GO

ALTER PROCEDURE solr.pThirdPartyExtract @Msg VARCHAR(1024) OUTPUT, @LegalEntityKey INT = NULL 
AS

/*************************************************************************************************************************
		Author		:	DeanA/ VirekR
		CreateDate	:	2014/12/04
		Description	:	This proc will:

						1. Return conveyance and litigation attorney information
						2. Updated to remove GeneralStatusKey = 1 (eWorks uses 2 for active attorneys!!)
						3. Return list of Valuers
						4. Populate [2am].solr.ThirdParty table together with any related accounts / offers attached to them 
															
																				
		History:
					
	--------------------------------------------------------------------	
	--Helper Code:	
			DECLARE @msg VARCHAR(1024)
			/*Optional Parameter of LegalEntityKey, if populated do that one only, if not do all */  
			EXEC Process.solr.pThirdPartyExtract @msg OUTPUT--,1022751
			SELECT @msg
	--------------------------------------------------------------------
**************************************************************************************************************************/
BEGIN
	BEGIN TRY

	--	DECLARE @msg VARCHAR(255)
	--	,@LegalEntityKey INT-- = 474649
	/************************************************************ REGISTRATION ATTORNEYS ******************************************************/
	IF OBJECT_ID('tempdb.dbo.#Offers') IS NOT NULL
			DROP TABLE #Offers
	
	CREATE TABLE #Offers
	(	LegalEntityKey			INT,
		OfferKey				VARCHAR(MAX),
		OfferType				VARCHAR(MAX),
		LegalEntityAddressKey	INT,
		AddressKey				INT,
		LegalEntityTypeKey		INT,
		AttorneyKey				INT,
		AttorneyContact			VARCHAR(500),
		AttorneyLitigationInd	BIT,
		AttorneyRegistrationInd BIT,		
		DeedsOfficeKey			INT,
		DeedsOfficeName			VARCHAR(500)
	)

	INSERT INTO #Offers (
		 LegalEntityKey
		,LegalEntityAddressKey
		,AddressKey
		,OfferKey
		,OfferType
		,LegalEntityTypeKey
		,AttorneyKey
		,AttorneyContact
		,AttorneyLitigationInd
		,AttorneyRegistrationInd
		,DeedsOfficeKey
		,DeedsOfficeName
		)
	SELECT a.LegalEntityKey
		,lea.LegalEntityAddressKey
		,lea.AddressKey
		,CAST(o.OfferKey	 AS VARCHAR(MAX))
		,CAST(o.OfferTypeKey AS VARCHAR(MAX))
		,le.LegalEntityTypeKey
		,a.AttorneyKey
		,a.AttorneyContact
		,a.AttorneyLitigationInd
		,a.AttorneyRegistrationInd
		,do.DeedsOfficeKey
		,do.Description	
	FROM [2am].dbo.Attorney a (NOLOCK)
	INNER JOIN [2am].dbo.DeedsOffice do (NOLOCK) ON a.DeedsOfficeKey = do.DeedsOfficeKey
	INNER JOIN [2am].dbo.LegalEntity le (NOLOCK) ON le.LegalEntityKey = a.LegalEntityKey
	LEFT JOIN [2am].dbo.OfferRole orl   (NOLOCK) ON orl.LegalEntityKey = le.LegalEntityKey
									AND orl.OfferRoleTypeKey = 4
	LEFT JOIN [2am].dbo.OfferRoleType ort (NOLOCK) ON ort.OfferRoleTypeKey = orl.OfferRoleTypeKey		
	LEFT JOIN [2am].dbo.Offer o (NOLOCK) ON orl.OfferKey = o.OfferKey
	LEFT JOIN [2am].dbo.OfferType ot (NOLOCK) ON ot.OfferTypeKey = o.OfferTypeKey
	LEFT JOIN [2am].dbo.LegalEntityAddress lea (NOLOCK) ON orl.LegalEntityKey = lea.LegalEntityKey
						AND lea.GeneralStatusKey = 1
	WHERE (
			le.LegalEntityKey = @LegalEntityKey
			OR @LegalEntityKey IS NULL
			)

	CREATE INDEX ix_Offer ON #Offers (LegalEntityKey);
	
	--	SELECT * FROM #Offers WHERE OfferKey = 1094136 ORDER BY OfferKey
	
	--	Flatten out the OfferKey and OfferType so it can one a single line and searchable from Solr
	IF OBJECT_ID('tempdb.dbo.#FlatOffers') IS NOT NULL
			DROP TABLE #FlatOffers
	
	SELECT t.LegalEntityKey,
		   STUFF(ISNULL((	SELECT ', ' + x.OfferKey
							FROM #Offers x
							WHERE x.LegalEntityKey = t.LegalEntityKey
							GROUP BY x.OfferKey
						FOR XML PATH (''), TYPE).value('.','VARCHAR(max)'), ''), 1, 2, '') AS 'Offers',
		  STUFF(ISNULL((	SELECT	', ' + x.OfferType
							FROM	#Offers x
							WHERE	x.LegalEntityKey = t.LegalEntityKey
							GROUP BY x.OfferType
				 FOR XML PATH (''), TYPE).value('.','VARCHAR(max)'), ''), 1, 2, '') AS 'OfferTypes'
			
	INTO	#FlatOffers
	FROM	#Offers t
	GROUP BY t.LegalEntityKey	

	CREATE INDEX ix_Offer ON #FlatOffers (LegalEntityKey)

	--	SELECT * FROM #FlatOffers WHERE LegalEntityKey = 474648 AND Offers LIKE '%999563%'

	----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
	-- Main Registration Attorney Select
	-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
	IF OBJECT_ID('tempdb.dbo.#Attorney') IS NOT NULL
			DROP TABLE #Attorney	
	select 
		DISTINCT
					CAST(le.LegalEntityKey AS VARCHAR) AS 'LegalEntityKey', 
					CASE 
						WHEN orl.LegalEntityTypeKey in (1,2)
							THEN 'Person'
						WHEN orl.LegalEntityTypeKey in (3,4,5)
							THEN 'Business'
					END AS 'LegalEntityType',
					'Attorney' AS 'ThirdPartyType',
					CASE	WHEN orl.AttorneyLitigationInd = 1 and orl.AttorneyRegistrationInd = 1
								THEN 'Litigation Attorney Registration Attorney'
							WHEN orl.AttorneyLitigationInd = 1 and orl.AttorneyRegistrationInd = 0
								THEN 'Litigation Attorney'
							WHEN orl.AttorneyLitigationInd = 0 and orl.AttorneyRegistrationInd = 1
								THEN 'Registration Attorney'				
					END AS 'ThirdPartySubType',
					CAST(orl.AttorneyKey AS VARCHAR) AS 'AttorneyKey', 
					le.RegisteredName AS 'LegalName', 
					CAST(ISNULL(le.TradingName, '')AS VARCHAR) AS 'TradingName',
					CAST(ISNULL(le.RegistrationNumber, '')AS VARCHAR) AS 'LegalIdentity',
					CAST(ISNULL(le.TaxNumber, '')AS VARCHAR) AS 'TaxNumber',
					CASE	WHEN orl.LegalEntityTypeKey IN (1,2)
							THEN 'Person'
							ELSE 'Business'
					END AS 'LegalIdentityType',
					CAST(orl.DeedsOfficeKey AS VARCHAR) AS 'DeedsOfficeKey', 
					orl.DeedsOfficeName, 
					orl.AttorneyContact AS 'AttorneyContact', 
					REPLACE(CAST(ISNULL(le.WorkPhoneCode,'') AS VARCHAR),CHAR(34),' ') + REPLACE(CAST(ISNULL(le.WorkPhoneNumber,'') AS VARCHAR),CHAR(34),' ') AS WorkPhoneNumber, 
					REPLACE(CAST(ISNULL(le.CellPhoneNumber,'') AS VARCHAR),CHAR(34),' ') AS CellPhoneNumber, 
					REPLACE(ISNULL(le.EmailAddress,''),CHAR(34),' ') AS EmailAddress, 
					REPLACE(CAST(ISNULL(le.FaxCode,'') AS VARCHAR),CHAR(34),' ') + REPLACE(CAST(ISNULL(le.FaxNumber,'') AS VARCHAR),CHAR(34),' ') AS FaxNumber,
					orl.LegalEntityAddressKey,
					orl.AddressKey,
					CONVERT(VARCHAR(255),'') AS 'Address',
					fo.Offers AS 'OfferKey',
					fo.OfferTypes AS 'OfferType'
	INTO #Attorney		
	--	select *
	FROM #Offers orl
	INNER JOIN (	SELECT MAX(ISNULL(OfferKey,0)) OfferKey, LegalEntityKey 
					FROM #Offers 
					GROUP BY LegalEntityKey 
				)maxO ON maxO.LegalEntityKey = orl.LegalEntityKey 
						AND ISNULL(maxO.OfferKey,0) = ISNULL(orl.OfferKey,0)
	INNER JOIN [2am].dbo.LegalEntity le (NOLOCK) on orl.LegalEntityKey = le.LegalEntityKey
	INNER JOIN #FlatOffers fo on fo.LegalEntityKey = le.LegalEntityKey				
	--WHERE orl.LegalEntityKey = 1138681

	--	SELECT LegalEntityAddressKey, * FROM #Attorney  WHERE LegalEntityKey = 1144724 AND OfferKey LIKE '%999563%'

	-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
	-- Update Address	
	UPDATE	#Attorney SET [Address] =  ([2am].[dbo].[fGetFormattedAddressDelimited] (AddressKey, 0))		
	
/************************************************************************** LITIGATION ATTORNEYS ***************************************************************************/

		IF OBJECT_ID('tempdb.dbo.#LitigationAccounts') IS NOT NULL
			DROP TABLE #LitigationAccounts
					
		SELECT DISTINCT fat.LegalEntityKey, CAST(d.AccountKey AS VARCHAR(max)) AS 'AccountKey'
		INTO	#LitigationAccounts
		FROM	[2am].dbo.ForeclosureAttorneyDetailTypeMapping fat (NOLOCK)		
		INNER JOIN [2AM].dbo.Attorney a (NOLOCK) ON a.LegalEntityKey = fat.LegalEntityKey		
		LEFT JOIN  [2AM].dbo.Detail d	(NOLOCK) ON d.DetailTypeKey = fat.DetailTypeKey		
		WHERE	fat.GeneralStatusKey = 1
		AND (
				fat.LegalEntityKey = @LegalEntityKey
			OR @LegalEntityKey IS NULL
			)

		--	SELECT * FROM #LitigationAccounts WHERE LegalEntityKey = 1144724 ORDER BY AccountKey
		
		-- Flatten Accounts related to Litigation Attorneys
		IF OBJECT_ID('tempdb.dbo.#FlatLitigationAccounts') IS NOT NULL
			DROP TABLE #FlatLitigationAccounts

		SELECT t.LegalEntityKey,
			   STUFF(ISNULL((	SELECT ', ' + x.AccountKey
								FROM #LitigationAccounts x
								WHERE x.LegalEntityKey = t.LegalEntityKey
								GROUP BY x.AccountKey
							FOR XML PATH (''), TYPE).value('.','VARCHAR(max)'), ''), 1, 2, '') Accounts
		INTO	#FlatLitigationAccounts
		FROM	#LitigationAccounts t
		GROUP BY t.LegalEntityKey
		ORDER BY 1

		CREATE INDEX ix_Offer ON #FlatLitigationAccounts (LegalEntityKey)
		
		-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
		-- Main Litigation Attorney Select

		IF OBJECT_ID('tempdb.dbo.#LitAttorney') IS NOT NULL
			DROP TABLE #LitAttorney

		SELECT DISTINCT	CAST(le.LegalEntityKey AS VARCHAR) AS 'LegalEntityKey', 
				CASE 
					WHEN let.LegalEntityTypeKey in (1,2)
						THEN 'Person'
					WHEN let.LegalEntityTypeKey in (3,4,5)
						THEN 'Business'
				END AS 'LegalEntityType',
				'Attorney' AS 'ThirdPartyType',
				CASE	WHEN a.AttorneyLitigationInd = 1 and a.AttorneyRegistrationInd = 1
								THEN 'Litigation Attorney Registration Attorney'
							WHEN a.AttorneyLitigationInd = 1 and a.AttorneyRegistrationInd = 0
								THEN 'Litigation Attorney'
							WHEN a.AttorneyLitigationInd = 0 and a.AttorneyRegistrationInd = 1
								THEN 'Registration Attorney'				
					END AS 'ThirdPartySubType',
				CAST(a.AttorneyKey AS VARCHAR) AS 'AttorneyKey', 
				le.RegisteredName AS 'LegalName', 
				CAST(ISNULL(le.TradingName, '')AS VARCHAR) AS 'TradingName',
				CAST(ISNULL(le.RegistrationNumber, '')AS VARCHAR) AS 'LegalIdentity',
				CAST(ISNULL(le.TaxNumber, '')AS VARCHAR) AS 'TaxNumber',
				CASE	WHEN let.LegalEntityTypeKey IN (1,2)
							THEN 'Person'
						ELSE 'Business'
					END AS 'LegalIdentityType',
				CAST(a.DeedsOfficeKey AS VARCHAR) AS 'DeedsOfficeKey', 
				do.Description AS 'DeedsOfficeName', 
				a.AttorneyContact AS 'AttorneyContact', 
				REPLACE(CAST(ISNULL(le.WorkPhoneCode,'') AS VARCHAR),CHAR(34),' ') + REPLACE(CAST(ISNULL(le.WorkPhoneNumber,'') AS VARCHAR),CHAR(34),' ') AS WorkPhoneNumber, 
				REPLACE(CAST(ISNULL(le.CellPhoneNumber,'') AS VARCHAR),CHAR(34),' ') AS CellPhoneNumber, 
				REPLACE(ISNULL(le.EmailAddress,''),CHAR(34),' ') AS EmailAddress, 
				REPLACE(CAST(ISNULL(le.FaxCode,'') AS VARCHAR),CHAR(34),' ') + REPLACE(CAST(ISNULL(le.FaxNumber,'') AS VARCHAR),CHAR(34),' ') AS FaxNumber,
				lea.LegalEntityAddressKey,
				CONVERT(VARCHAR(255),([2AM].dbo.fGetFormattedAddressDelimited (lea.AddressKey, 0))) AS 'Address',
				fl.Accounts AS 'AccountKeys'		
		INTO	#LitAttorney		 	
		FROM	[2am].dbo.ForeclosureAttorneyDetailTypeMapping fat (NOLOCK)
		INNER JOIN [2AM].dbo.LegalEntity le (NOLOCK) ON le.LegalEntityKey = fat.LegalEntityKey
		INNER JOIN [2AM].dbo.LegalEntityType let (NOLOCK) on le.LegalEntityTypeKey = let.LegalEntityTypeKey
		INNER JOIN [2AM].dbo.Attorney a (NOLOCK) on le.LegalEntityKey = a.LegalEntityKey
		INNER JOIN #FlatLitigationAccounts fl ON fl.LegalEntityKey = le.LegalEntityKey
		INNER JOIN [2AM].dbo.DeedsOffice do (NOLOCK) on a.DeedsOfficeKey = do.DeedsOfficeKey
		LEFT JOIN [2AM].dbo.LegalEntityAddress lea (NOLOCK) on le.LegalEntityKey = lea.LegalEntityKey		
											AND lea.GeneralStatusKey = 1
		WHERE	fat.GeneralStatusKey = 1

		--	SELECT * FROM #LitAttorney WHERE LegalEntityKey = 474648 AND AccountKeys LIKE '%3580240%'
		
			------------------------- FLATTEN OUT THE ADDRESS/ADDRESSES FOR ALL ATTORNEYS ------------------------- 

			IF OBJECT_ID('tempdb.dbo.#FlatAddress') IS NOT NULL
					DROP TABLE #FlatAddress

			;WITH Addresses (LegalEntityKey, Address)
			AS (
					SELECT LegalEntityKey, Address	FROM #Attorney					
					UNION 
					SELECT LegalEntityKey, Address	FROM #LitAttorney					
			)

				SELECT t.LegalEntityKey,
					   STUFF(ISNULL((	SELECT	 ', ' + ISNULL(x.Address,'')
										FROM	 Addresses x
										WHERE	 x.LegalEntityKey = t.LegalEntityKey
										GROUP BY x.Address
									FOR XML PATH (''), TYPE).value('.','VARCHAR(max)'), ''), 1, 2, '') Addresses
				INTO	#FlatAddress
				FROM	Addresses t
				GROUP BY t.LegalEntityKey
				ORDER BY 1

				CREATE INDEX ix_Offer ON #FlatAddress (LegalEntityKey)	

				--	SELECT * FROM #FlatLitAddress where legalentitykey = 474644
							
		/****************************************************** VALUERS ****************************************************************/
		IF OBJECT_ID('tempdb.dbo.#ValOffer') IS NOT NULL
			DROP TABLE #ValOffer

		SELECT	DISTINCT le.LegalEntityKey,CAST(o.OfferKey AS VARCHAR(20)) OfferKey, CAST(ot.Description AS VARCHAR(100)) OfferType, CAST(o.ReservedAccountKey AS VARCHAR(20)) AccountKey
		INTO	#ValOffer
		FROM	[2AM].dbo.Valuator vl (NOLOCK)
		LEFT JOIN	[2AM].dbo.Valuation v	(NOLOCK) ON vl.ValuatorKey = v.ValuatorKey
		LEFT JOIN	[2am].dbo.LegalEntity le (NOLOCK) ON le.LegalEntityKey = vl.LegalEntityKey
		LEFT JOIN	[2AM].dbo.OfferMortgageLoan  oml (NOLOCK) ON oml.PropertyKey = v.PropertyKey
		LEFT JOIN	[2AM].dbo.Offer o	(NOLOCK) ON o.OfferKey = oml.OfferKey
		LEFT JOIN	[2am].dbo.OfferType ot   (NOLOCK) on ot.OfferTypeKey = o.OfferTypeKey
		WHERE		--le.LegalEntityKey = 1138690
		 (	le.LegalEntityKey = @LegalEntityKey 
			OR	@LegalEntityKey IS NULL )


		CREATE INDEX ix_Val ON #ValOffer (LegalEntityKey);

		--	SELECT * FROM #ValOffer WHERE Legalentitykey = 615700

		-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
			-- Flatten Offers + OfferTypes + AccountKeys
		-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
		IF OBJECT_ID('tempdb.dbo.#FlatLValOffers') IS NOT NULL
					DROP TABLE #FlatLValOffers

		SELECT t.LegalEntityKey,
				STUFF(ISNULL((	SELECT ', ' + x.OfferKey
								FROM #ValOffer x
								WHERE x.LegalEntityKey = t.LegalEntityKey
								GROUP BY x.OfferKey
							FOR XML PATH (''), TYPE).value('.','VARCHAR(max)'), ''), 1, 2, '') OfferKeys,
				STUFF(ISNULL((	SELECT ', ' + x.OfferType
								FROM #ValOffer x
								WHERE x.LegalEntityKey = t.LegalEntityKey
								GROUP BY x.OfferType
							FOR XML PATH (''), TYPE).value('.','VARCHAR(max)'), ''), 1, 2, '') OfferTypes,
				STUFF(ISNULL((	SELECT ', ' + x.AccountKey
								FROM #ValOffer x
								WHERE x.LegalEntityKey = t.LegalEntityKey
								GROUP BY x.AccountKey
							FOR XML PATH (''), TYPE).value('.','VARCHAR(max)'), ''), 1, 2, '') AccountKeys
		INTO	#FlatLValOffers
		FROM	#ValOffer t
		GROUP BY t.LegalEntityKey		

		CREATE INDEX ix_OfferType ON #FlatLValOffers (LegalEntityKey)

		/********************************************************* MAIN VALUER QUERY *****************************************************************/
		IF OBJECT_ID('tempdb.dbo.#Valuer') IS NOT NULL
					DROP TABLE #Valuer

		SELECT 
				DISTINCT
							CAST(le.LegalEntityKey AS VARCHAR) AS 'LegalEntityKey', 
							CASE 
								WHEN let.LegalEntityTypeKey in (1,2)
									THEN 'Person'
								WHEN let.LegalEntityTypeKey in (3,4,5)
									THEN 'Business'
							END AS 'LegalEntityType',
							'Valuer' AS 'ThirdPartyType',
							'' AS 'ThirdPartySubType',					
							le.RegisteredName AS 'LegalName', 
							CAST(ISNULL(le.TradingName, le.RegisteredName)AS VARCHAR) AS 'TradingName',
							CAST(ISNULL(le.RegistrationNumber, '')AS VARCHAR) AS 'LegalIdentity',
							CAST(ISNULL(le.TaxNumber, '')AS VARCHAR) AS 'TaxNumber',
							CAST(ISNULL(let.Description, '')AS VARCHAR) AS 'LegalIdentityType',					
							v.ValuatorContact AS 'Contact', 
							REPLACE(CAST(ISNULL(le.WorkPhoneCode,'') AS VARCHAR),CHAR(34),' ') + REPLACE(CAST(ISNULL(le.WorkPhoneNumber,'') AS VARCHAR),CHAR(34),' ') AS WorkPhoneNumber, 
							REPLACE(CAST(ISNULL(le.CellPhoneNumber,'') AS VARCHAR),CHAR(34),' ') AS CellPhoneNumber, 
							REPLACE(ISNULL(le.EmailAddress,''),CHAR(34),' ') AS EmailAddress, 
							REPLACE(CAST(ISNULL(le.FaxCode,'') AS VARCHAR),CHAR(34),' ') + REPLACE(CAST(ISNULL(le.FaxNumber,'') AS VARCHAR),CHAR(34),' ') AS FaxNumber,
							lea.LegalEntityAddressKey,
								([2am].[dbo].[fGetFormattedAddressDelimited] (AddressKey, 0)) AS 'Address',
							fo.OfferKeys	AS 'OfferKey',
							fo.OfferTypes	AS 'OfferType',
							fo.AccountKeys	AS 'AccountKey'
			INTO #Valuer			
			FROM		[2am].dbo.LegalEntity le (NOLOCK)	
			INNER JOIN	#FlatLValOffers fo on fo.LegalEntityKey = le.LegalEntityKey		
			INNER JOIN	[2am].dbo.LegalEntityType let (NOLOCK) on le.LegalEntityTypeKey = let.LegalEntityTypeKey			
			INNER JOIN	[2am].dbo.Valuator v (NOLOCK) ON v.LegalEntityKey = fo.LegalEntityKey
			LEFT JOIN	[2am].dbo.LegalEntityAddress lea (NOLOCK) on le.LegalEntityKey = lea.LegalEntityKey
							AND	lea.GeneralStatusKey = 1

		-- Flatten Address
		IF OBJECT_ID('tempdb.dbo.#FlatValuerAddress') IS NOT NULL
					DROP TABLE #FlatValuerAddress

		SELECT t.LegalEntityKey,
				STUFF(ISNULL((	SELECT ', ' + x.Address
								FROM #Valuer x
								WHERE x.LegalEntityKey = t.LegalEntityKey
								GROUP BY x.Address
							FOR XML PATH (''), TYPE).value('.','VARCHAR(max)'), ''), 1, 2, '') Address
		INTO	#FlatValuerAddress
		FROM	#Valuer t
		GROUP BY t.LegalEntityKey	 

		
		--IF LegalEntity record exists in the table, delete it.		
        		 
		IF @LegalEntityKey IS NOT NULL 
		BEGIN
			IF OBJECT_ID('[2am].solr.ThirdParty') IS NOT NULL
				BEGIN
					DELETE 
					FROM	[2AM].solr.ThirdParty
					WHERE	LegalEntityKey = @LegalEntityKey
				END
		END
		
		ELSE
		/*
		This piece of code will execute when we are NOT passing through a parameter 
		which means clean out the table and reload the full batch.

		If Exists then clean out the table.
		*/
		BEGIN
			IF OBJECT_ID('[2am].solr.ThirdParty') IS NOT NULL
				TRUNCATE TABLE [2am].solr.ThirdParty			
		END

		/****************************** INSERT BOTH LITIGATION + CONVEYANCE ATTORNEYS INTO THIRD PARTY TABLE *********************************/
		
		IF OBJECT_ID('tempdb.dbo.#RegAttorneyList') IS NOT NULL
					DROP TABLE #RegAttorneyList
		
		SELECT DISTINCT	a.LegalEntityKey,a.LegalEntityType,a.ThirdPartyType,a.ThirdPartySubType, a.LegalName,a.TradingName,a.LegalIdentityType,a.LegalIdentity,a.TaxNumber,a.AttorneyContact,a.WorkPhoneNumber,a.CellPhoneNumber,a.EmailAddress,a.FaxNumber, GETDATE() AS ModifiedDate
						,a.OfferKey, a.OfferType, fa.Addresses			
		INTO		#RegAttorneyList
		FROM		#Attorney a 
		INNER JOIN	#FlatAddress fa ON fa.LegalEntityKey = a.LegalEntityKey
		WHERE		a.LegalEntityKey NOT IN (SELECT DISTINCT LegalEntityKey FROM #LitAttorney)
			
		INSERT INTO [2AM].solr.ThirdParty(	LegalEntityKey,
													LegalEntityType,
													ThirdPartyType,
													ThirdPartySubType,
													LegalName,
													TradingName,
													LegalIdentityType,
													LegalIdentity,
													TaxNumber,
													Contact,
													WorkPhoneNumber,
													CellPhoneNumber,
													EmailAddress,
													FaxNumber,
													LastModifiedDate,
													OfferKey,
													OfferType,
													[Address]																									
													)
		SELECT *
		FROM #RegAttorneyList
		
		-- Insert Attorneys that are not already inserted above
		INSERT INTO [2AM].solr.ThirdParty(	LegalEntityKey,
													LegalEntityType,
													ThirdPartyType,
													ThirdPartySubType,
													LegalName,
													TradingName,
													LegalIdentityType,
													LegalIdentity,
													TaxNumber,
													Contact,
													WorkPhoneNumber,
													CellPhoneNumber,
													EmailAddress,
													FaxNumber,
													LastModifiedDate,
													AccountKey,
													[Address]													
													)

		SELECT		DISTINCT l.LegalEntityKey,l.LegalEntityType,l.ThirdPartyType,l.ThirdPartySubType, l.LegalName,l.TradingName,l.LegalIdentityType,l.LegalIdentity,l.TaxNumber,
					l.AttorneyContact,l.WorkPhoneNumber,l.CellPhoneNumber,l.EmailAddress,l.FaxNumber, GETDATE()
					,l.AccountKeys, fa.Addresses
		FROM		#LitAttorney l
		INNER JOIN	#FlatAddress fa ON fa.LegalEntityKey = l.LegalEntityKey	
		WHERE		l.LegalEntityKey NOT IN (SELECT DISTINCT LegalEntityKey FROM #RegAttorneyList)

		/****************************** INSERT VALUERS INTO THIRD PARTY TABLE *************************************/
		INSERT INTO [2AM].solr.ThirdParty(	LegalEntityKey,
													LegalEntityType,
													ThirdPartyType,
													ThirdPartySubType,
													LegalName,
													TradingName,
													LegalIdentityType,
													LegalIdentity,
													TaxNumber,
													Contact,
													WorkPhoneNumber,
													CellPhoneNumber,
													EmailAddress,
													FaxNumber,
													LastModifiedDate,											
													OfferKey,
													OfferType,
													AccountKey,
													[Address]											
													)
		SELECT  DISTINCT	v.LegalEntityKey,
							LegalEntityType,
							ThirdPartyType,
							ThirdPartySubType,
							LegalName,
							TradingName,
							LegalIdentityType,
							LegalIdentity,
							TaxNumber,
							Contact,
							WorkPhoneNumber,
							CellPhoneNumber,
							EmailAddress,
							FaxNumber,
							GETDATE(),
							OfferKey,
							OfferType,
							AccountKey,
							f.[Address]					
		from #Valuer v
		INNER JOIN #FlatValuerAddress f ON f.LegalEntityKey = v.LegalEntityKey

					
		-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
		-- Strip space and some punctuation from staging table
		-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
		UPDATE [2AM].solr.ThirdParty SET OfferKey = REPLACE(OfferKey,',',' ')
		UPDATE [2AM].solr.ThirdParty SET OfferType = REPLACE(OfferType,',',' ')
		UPDATE [2AM].solr.ThirdParty SET AccountKey = REPLACE(AccountKey,',',' ')
		UPDATE [2AM].solr.ThirdParty SET [Address] = REPLACE([Address],',',' ')
		UPDATE [2AM].solr.ThirdParty SET WorkPhoneNumber = REPLACE(WorkPhoneNumber,' ','')
		UPDATE [2AM].solr.ThirdParty SET WorkPhoneNumber = REPLACE(WorkPhoneNumber,'(','')
		UPDATE [2AM].solr.ThirdParty SET WorkPhoneNumber = REPLACE(WorkPhoneNumber,')','')
		UPDATE [2AM].solr.ThirdParty SET CellPhoneNumber = REPLACE(CellPhoneNumber,' ','')
		UPDATE [2AM].solr.ThirdParty SET FaxNumber = REPLACE(FaxNumber,' ','')

		
		SELECT * FROM [2AM].solr.ThirdParty (NOLOCK) 
		WHERE ( LegalEntityKey = @LegalEntityKey 
			OR	@LegalEntityKey IS NULL )
		ORDER BY ID

		--	select * from [2AM].solr.ThirdParty (NOLOCK) WHERE LegalEntityKey = 474648 and IndexText LIKE '%999563%'
		 		
		END TRY

		BEGIN CATCH

	
			set @Msg = 'solr.pThirdPartyExtract: ' + ISNULL(ERROR_MESSAGE(), 'Failed!')
			RAISERROR(@Msg,16,1)

			SELECT TOP 0 * INTO #Errors FROM process.template.errors
	
			DELETE FROM #Errors
			INSERT INTO #Errors (ErrorCodeKey, DateOfError, MSG, SeverityTypeKey)
			SELECT (SELECT ErrorCodeKey FROM process.errorhandling.ErrorCode (NOLOCK) WHERE Description LIKE 'Solr Extract Failure'), GETDATE(), @Msg, 1
	
			EXEC process.errorhandling.pLogErrors @Msg OUTPUT
				
		END CATCH

		--	EXEC Process.solr.pThirdPartyExtract '',474642
END --End Proc
GO

PRINT 'solr.pThirdPartyExtract deployed: ' + CAST(GETDATE() AS VARCHAR) + ' to server: '+ @@Servername
GO

GRANT EXECUTE ON OBJECT::solr.pThirdPartyExtract TO [Batch];
GRANT EXECUTE ON OBJECT::solr.pThirdPartyExtract TO [ProcessRole];
GRANT EXECUTE ON OBJECT::solr.pThirdPartyExtract TO [AppRole];


/*
Select * from [2AM].solr.ThirdParty
*/