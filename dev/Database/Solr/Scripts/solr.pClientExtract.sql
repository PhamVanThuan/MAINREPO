USE Process

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

IF OBJECT_ID('solr.pClientExtract') is null
BEGIN 
	DECLARE @Qry VARCHAR(1024)
	SET @Qry =	'CREATE PROCEDURE solr.pClientExtract ' +
				'AS ' +
				'BEGIN ' +
				'SET NOCOUNT ON; ' +
				'END '

	EXECUTE (@Qry)
END
GO


ALTER PROCEDURE solr.pClientExtract @Msg VARCHAR(1024) OUTPUT, @LegalEntityKey INT = NULL
AS

/*************************************************************************************************************************
		Author		:	DeanA / VirekR
		CreateDate	:	2015/01/05
		Description	:	This proc will:

						1. Return client related information used by Solr for indexing purposes for all legalentities
						or just one						
																																			
		History:
					
			--------------------------------------------------------------------	
	--Helper Code:	
			DECLARE @msg VARCHAR(1024)
			
			/*Optional Parameter of LegalEntityKey, if populated do that one only, if not do all */  		
			EXEC Process.solr.pClientExtract @msg OUTPUT--,85553
			SELECT @msg
	--------------------------------------------------------------------
**************************************************************************************************************************/

BEGIN
	BEGIN TRY	
	
	--DECLARE @msg VARCHAR(255)
	--,@LegalEntityKey INT --= 1095619	

	/********************* LIST OF EXLUSIONS ****************************/
	-- Exlclusions are Valuers, Attorneys

	IF OBJECT_ID('tempdb.dbo.#Exclusions') IS NOT NULL
		DROP TABLE #Exclusions
	
		SELECT	DISTINCT LegalEntityKey
		INTO	#Exclusions
		FROM	[2am].dbo.ThirdParty (NOLOCK) 
					
	IF OBJECT_ID('tempdb.dbo.#Role') IS NOT NULL
		DROP TABLE #Role
	
		SELECT	DISTINCT
				r.LegalEntityKey, 
				pa.AccountKey, 
				ast.Description, 
				CASE	WHEN p.ProductKey IN (1,2,5,6,9,11,12) 
						THEN 'Mortgage Loan' 
						WHEN p.ProductKey = 4
						THEN 'Life'
						WHEN p.ProductKey = 3
						THEN 'HOC'
						ELSE isnull(p.Description,'')
				END AS 'ProductType' 
		INTO	#Role
		FROM	[2am].dbo.[Role] r (NOLOCK)
		LEFT JOIN [2am].dbo.Account pa	(NOLOCK) ON pa.AccountKey = r.AccountKey
		LEFT JOIN [2am].dbo.AccountStatus ast (NOLOCK) ON ast.AccountStatusKey = pa.AccountStatusKey
		LEFT JOIN [2am].dbo.Account a	(NOLOCK) ON pa.AccountKey = a.ParentAccountKey
		LEFT JOIN [2am].dbo.Product p	(NOLOCK) ON p.ProductKey = pa.RRR_ProductKey
		WHERE	r.LegalEntityKey NOT IN (SELECT LegalEntityKey FROM #Exclusions)		
		AND 
		(	r.LegalEntityKey = @LegalEntityKey 
			OR	@LegalEntityKey IS NULL )
	
		GROUP BY r.LegalEntityKey, pa.AccountKey, ast.Description, p.Description, p.ProductKey

		CREATE INDEX ix_Role ON #Role (LegalEntityKey);						

		IF OBJECT_ID('tempdb.dbo.#Offer') IS NOT NULL
			DROP TABLE #Offer

		SELECT	DISTINCT	
				oro.LegalEntityKey, 
				o.OfferKey, 
				o.ReservedAccountKey, 
				os.Description  AS  'OfferStatus',
				ot.Description  AS  'OfferType',
				ort.Description AS  'RoleType'
		INTO	#Offer
		FROM	[2am].dbo.OfferRole	oro		(NOLOCK)	
		LEFT JOIN	[2am].dbo.OfferRoleType ort	(NOLOCK) ON ort.OfferRoleTypeKey = oro.OfferRoleTypeKey
		LEFT JOIN	[2am].dbo.Offer o	(NOLOCK) ON o.OfferKey = oro.OfferKey
		LEFT JOIN	[2am].dbo.OfferType ot		(NOLOCK) ON ot.OfferTypeKey = o.OfferTypeKey 
		LEFT JOIN	[2am].dbo.OfferStatus os	(NOLOCK) ON os.OfferStatusKey = o.OfferStatusKey
		WHERE	ort.OfferRoleTypeKey IN (8,10,11,12,13)
		AND		oro.LegalEntityKey NOT IN (SELECT LegalEntityKey FROM #Exclusions)
		AND (	oro.LegalEntityKey = @LegalEntityKey 
			OR	@LegalEntityKey IS NULL )					

		CREATE INDEX ix_Offer ON #Offer (LegalEntityKey);
					
		--	SELECT * FROM #Offer

		IF OBJECT_ID('tempdb.dbo.#LE') IS NOT NULL
			DROP TABLE #LE

		SELECT	DISTINCT 
				OfferKey = CASE WHEN o.OfferKey IS NULL THEN '' ELSE CAST(o.OfferKey AS VARCHAR) END, 
				le.LegalEntityKey as LegalEntityKey,
				let.Description as LegalEntityType,
				les.Description as LegalEntityStatusKey,		 		 
				CONVERT(VARCHAR(255),'') AS LegalName,
				le.FirstNames,
				le.Surname,
				LegalIdentity = case 	when le.LegalEntityTypeKey = 2 
										then case	when le.IDNumber is not null 
													then isnull(le.IDNumber,'')
													else isnull(le.PassportNumber,'')
											  end
										when (le.LegalEntityTypeKey > 3 and le.LegalEntityTypeKey < 6) 
										then isnull(le.RegistrationNumber,'')
									else isnull(le.IDNumber,'') + ' ' + isnull(le.RegistrationNumber,'') 
									end,		
				 LegalIdentityType = case
										when le.LegalEntityTypeKey = 2 then case when le.IDNumber is not null then 'SA Identity Number' else 'Passport Number' end
										when le.LegalEntityTypeKey = 3  then 'Company Registration'
										when le.LegalEntityTypeKey = 4  then 'Close Corportation Registration'
										when le.LegalEntityTypeKey = 5  then 'Trust Registration'
										else 'Unknown Registration'
									end,
				cast(le.PreferredName as varchar) as PreferredName,        
				isnull(le.TaxNumber,'') as TaxNumber, 
				isnull(le.HomePhoneCode,'') + isnull(le.HomePhoneNumber,'') as HomePhoneNumber, 
				isnull(le.WorkPhoneCode,'')+ isnull(le.WorkPhoneNumber,'')  as WorkPhoneNumber, 
				isnull(le.CellPhoneNumber,'') as CellPhoneNumber, 
				replace(isnull(le.EmailAddress,''),char(34),' ') as EmailAddress, 
				isnull(le.FaxCode,'') + isnull(le.FaxNumber,'') as FaxNumber,
				isnull(o.RoleType,'') as RoleType,					
				 o.OfferType,
				 CONVERT(VARCHAR(255),'') AS 'LegalEntityAddress',
				 CONVERT(VARCHAR(255),'') AS 'PropertyAddress',
				 lea.AddressKey AS LegalEntityAddressKey,
				 p.AddressKey
			INTO #LE
			--select *
			FROM		[2am].dbo.LegalEntity le		(NOLOCK)			
			INNER JOIN	[2am].dbo.LegalEntityType let	 (NOLOCK) on le.LegalEntityTypeKey = let.LegalEntityTypeKey
			INNER JOIN	[2am].dbo.LegalEntityStatus les  (NOLOCK) on le.LegalEntityStatusKey = les.LegalEntityStatusKey	
			LEFT JOIN	[2am].dbo.LegalEntityAddress lea (NOLOCK) on le.LegalEntityKey = lea.LegalEntityKey
			LEFT JOIN	#Offer o ON o.LegalEntityKey = le.LegalEntityKey	
			LEFT JOIN	[2AM].dbo.OfferMortgageLoan oml  (NOLOCK) on o.OfferKey = oml.OfferKey
			LEFT JOIN	[2AM].dbo.Property p		(NOLOCK) on oml.PropertyKey = p.PropertyKey				
			WHERE		le.LegalEntityKey NOT IN (SELECT LegalEntityKey FROM #Exclusions)
			AND			ISNULL(le.UserID,'') NOT IN ('Attorney - LE Migration','Valuator - LE Migration' )
			AND			(le.LegalEntityKey = @LegalEntityKey 
							OR	@LegalEntityKey IS NULL )					

		CREATE INDEX ix_LE ON #LE (LegalEntityKey);
		
		UPDATE	#LE
		SET		LegalName = [2am].dbo.LegalEntityLegalName(LegalEntityKey, 0),		
				LegalEntityAddress = [2am].dbo.fGetFormattedAddressDelimited (LegalEntityAddressKey, 0),
				PropertyAddress	   = [2am].dbo.fGetFormattedAddressDelimited (AddressKey, 0)

		-- SELECT * FROM #LE

--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
-- Leads that have not been picked up already
--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------		
		
		IF OBJECT_ID('tempdb.dbo.#LELeads') IS NOT NULL
			DROP TABLE #LELeads

		SELECT	DISTINCT 
				OfferKey = CASE WHEN o.OfferKey IS NULL THEN '' ELSE CAST(o.OfferKey AS VARCHAR) END, 
				le.LegalEntityKey as LegalEntityKey,
				let.Description as LegalEntityType,					 		 
				CONVERT(VARCHAR(255),'') AS LegalName,
				isnull(le.FirstNames,'') as FirstNames,
				isnull(le.Surname,'') as Surname,
				LegalIdentity = case 	when le.LegalEntityTypeKey = 2 
										then case	when le.IDNumber is not null 
													then isnull(le.IDNumber,'')
													else isnull(le.PassportNumber,'')
											  end
										when (le.LegalEntityTypeKey > 3 and le.LegalEntityTypeKey < 6) 
										then isnull(le.RegistrationNumber,'')
									else isnull(le.IDNumber,'') + ' ' + isnull(le.RegistrationNumber,'') 
									end,		
				 LegalIdentityType = case
										when le.LegalEntityTypeKey = 2 then case when le.IDNumber is not null then 'SA Identity Number' else 'Passport Number' end
										when le.LegalEntityTypeKey = 3  then 'Company Registration'
										when le.LegalEntityTypeKey = 4  then 'Close Corportation Registration'
										when le.LegalEntityTypeKey = 5  then 'Trust Registration'
										else 'Unknown Registration'
									end,
				cast(le.PreferredName as varchar) as PreferredName,        
				isnull(le.TaxNumber,'') as TaxNumber, 
				isnull(le.HomePhoneCode,'') + isnull(le.HomePhoneNumber,'') as HomePhoneNumber, 
				isnull(le.WorkPhoneCode,'')+ isnull(le.WorkPhoneNumber,'')  as WorkPhoneNumber, 
				isnull(le.CellPhoneNumber,'') as CellPhoneNumber, 
				replace(isnull(le.EmailAddress,''),char(34),' ') as EmailAddress, 
				isnull(le.FaxCode,'') + isnull(le.FaxNumber,'') as FaxNumber,
				isnull(ofrt.Description,'') as RoleType,					
				 ot.Description as 'OfferType',
				 CONVERT(VARCHAR(255),'') AS 'LegalEntityAddress',
				 CONVERT(VARCHAR(255),'') AS 'PropertyAddress',
				 le.LegalEntityStatusKey				 
		INTO #LeLeads
		FROM		[2am].dbo.Offer o				 (NOLOCK)
		INNER JOIN	[2am].dbo.OfferType ot			 (NOLOCK) ON o.OfferTypeKey= ot.OfferTypeKey
		INNER JOIN	[2am].dbo.OfferRole ofr			 (NOLOCK) ON o.OfferKey=ofr.OfferKey
		INNER JOIN	[2am].dbo.OfferRoleType ofrt	 (NOLOCK) on ofr.OfferRoleTypeKey = ofrt.OfferRoleTypeKey
		INNER JOIN	[2am].dbo.LegalEntity le		 (NOLOCK) ON ofr.LegalEntityKey=le.LegalEntityKey
		INNER JOIN	[2am].dbo.OfferInformation oi	 (NOLOCK) ON o.OfferKey=oi.OfferKey 
		INNER JOIN	[2am].dbo.LegalEntityType let	 (NOLOCK) on le.LegalEntityTypeKey = let.LegalEntityTypeKey
		WHERE	o.OfferStatusKey = 1 
		AND		IDNumber IS NOT NULL
		AND		ofrt.OfferRoleTypeKey IN (8,10,11,12,13)
		AND		oi.OfferInformationKey IN (
									SELECT	MAX(offerinformationkey) 
									FROM	[2am].dbo.OfferInformation (NOLOCK)
									WHERE	OfferInformationTypeKey != 3
									AND		OfferKey = o.OfferKey
								)		
		AND (	le.LegalEntityKey = @LegalEntityKey 
				OR	@LegalEntityKey IS NULL ) 	
		
		CREATE INDEX ix_LE ON #LELeads (LegalEntityKey);

		IF OBJECT_ID('tempdb.dbo.#Leads') IS NOT NULL
			DROP TABLE #Leads
		
		SELECT DISTINCT le.*, les.Description as LegalEntityStatus, lea.AddressKey AS LegalEntityAddressKey,p.AddressKey
		INTO #Leads
		FROM #LeLeads le
		LEFT JOIN	[2am].dbo.LegalEntityStatus les  (NOLOCK) on le.LegalEntityStatusKey = les.LegalEntityStatusKey	
		LEFT JOIN	[2am].dbo.LegalEntityAddress lea (NOLOCK) on le.LegalEntityKey = lea.LegalEntityKey
		LEFT JOIN	[2AM].dbo.OfferMortgageLoan oml  (NOLOCK) on le.OfferKey = oml.OfferKey
		LEFT JOIN	[2AM].dbo.Property p			 (NOLOCK) on oml.PropertyKey = p.PropertyKey
		WHERE	le.LegalEntityKey NOT IN (SELECT LegalEntityKey FROM #LE)
		AND		le.LegalEntityKey NOT IN (SELECT LegalEntityKey FROM #Exclusions)

		CREATE INDEX ix_LE ON #Leads (LegalEntityKey);	

		UPDATE	#Leads
		SET		LegalName = [2am].dbo.LegalEntityLegalName(LegalEntityKey, 0),		
				LegalEntityAddress = [2am].dbo.fGetFormattedAddressDelimited (LegalEntityAddressKey, 0),
				PropertyAddress	   = [2am].dbo.fGetFormattedAddressDelimited (AddressKey, 0)

--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
-- Flatten Account and Offer Information into strings so this can be searched on
--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
		IF OBJECT_ID('tempdb.dbo.#FOffer') IS NOT NULL
			DROP TABLE #FOffer

		SELECT DISTINCT LegalEntityKey, 
						OfferKey,
						OfferStatus,
						OfferType,
						RoleType,
						AccountKey,
						AccountStatus,
						ProductType,
						LegalEntityAddress,
						PropertyAddress
		INTO #FOffer
		FROM 
		(SELECT			le.LegalEntityKey, 
						CAST(o.OfferKey AS VARCHAR)    AS 'OfferKey',
						o.OfferStatus,
						le.OfferType  AS 'OfferType',
						le.RoleType,
						CAST(r.AccountKey AS VARCHAR)  AS 'AccountKey',
						CASE WHEN a.AccountStatusKey = 1 THEN 'Open'
							WHEN a.AccountStatusKey = 2 THEN	'Closed'
							WHEN a.AccountStatusKey = 3 THEN	'Application'
							WHEN a.AccountStatusKey = 4 THEN	'Locked'
							WHEN a.AccountStatusKey = 5 THEN	'Dormant'
							WHEN a.AccountStatusKey = 6 THEN	'Application prior to Instruct Attorney'
						END AS 'AccountStatus',
--						r.Description AS 'AccountStatus',
						r.ProductType,
						LegalEntityAddress,
						PropertyAddress
		FROM #LE le
		LEFT JOIN	#Offer o ON o.OfferKey = le.OfferKey
		LEFT JOIN	#Role r  ON r.LegalEntityKey = le.LegalEntityKey
		LEFT JOIN  [2am].dbo.Account a on r.AccountKey = a.AccountKey and a.RRR_ProductKey not in (3,4)
		
		UNION

				SELECT DISTINCT lead.LegalEntityKey,
						CAST(isnull(o.OfferKey,'') AS VARCHAR) AS 'OfferKey',
						isnull(o.OfferStatus, '') AS 'OfferStatus',
						isnull(lead.OfferType,'')  AS 'OfferType',
						isnull(lead.RoleType,'') AS 'RoleType',
						CAST(isnull(r.AccountKey,'') AS VARCHAR)  AS 'AccountKey',
						'' AS 'AccountStatus',
						isnull(r.ProductType,'') AS 'ProductType',
						isnull(LegalEntityAddress,'') as 'LegalEntityAddress',
						isnull(PropertyAddress,'') as 'PropertyAddress'
		FROM #Leads lead
		LEFT JOIN	#Offer o ON o.OfferKey = lead.OfferKey
		LEFT JOIN	#Role r  ON r.LegalEntityKey = lead.LegalEntityKey

		) a

		CREATE INDEX ix_LegalEntityKey ON #FOffer(LegalEntityKey);
				
		IF OBJECT_ID('tempdb.dbo.#FlatInfo') IS NOT NULL
			DROP TABLE #FlatInfo

		SELECT t.LegalEntityKey,
			   STUFF(ISNULL((	SELECT ', ' + x.OfferKey
								FROM #FOffer x
								WHERE x.LegalEntityKey = t.LegalEntityKey
								GROUP BY x.OfferKey
							FOR XML PATH (''), TYPE).value('.','VARCHAR(max)'), ''), 1, 2, '') Offers,
				STUFF(ISNULL((	SELECT ', ' + x.OfferStatus
								FROM #FOffer x
								WHERE x.LegalEntityKey = t.LegalEntityKey
								GROUP BY x.OfferStatus
							FOR XML PATH (''), TYPE).value('.','VARCHAR(max)'), ''), 1, 2, '') OfferStatuses,
				STUFF(ISNULL((	SELECT ', ' + x.OfferType
								FROM #FOffer x
								WHERE x.LegalEntityKey = t.LegalEntityKey
								GROUP BY x.OfferType
							FOR XML PATH (''), TYPE).value('.','VARCHAR(max)'), ''), 1, 2, '') OfferType,
				STUFF(ISNULL((	SELECT ', ' + x.RoleType
								FROM #FOffer x
								WHERE x.LegalEntityKey = t.LegalEntityKey
								GROUP BY x.RoleType
							FOR XML PATH (''), TYPE).value('.','VARCHAR(max)'), ''), 1, 2, '') RoleType,
				STUFF(ISNULL((	SELECT ', ' + x.AccountKey
								FROM #FOffer x
								WHERE x.LegalEntityKey = t.LegalEntityKey
								GROUP BY x.AccountKey
							FOR XML PATH (''), TYPE).value('.','VARCHAR(max)'), ''), 1, 2, '') Accounts,
				STUFF(ISNULL((	SELECT ', ' + x.AccountStatus
								FROM #FOffer x
								WHERE x.LegalEntityKey = t.LegalEntityKey
								GROUP BY x.AccountStatus
							FOR XML PATH (''), TYPE).value('.','VARCHAR(max)'), ''), 1, 2, '') AccountStatus,
				STUFF(ISNULL((	SELECT ', ' + x.ProductType
								FROM #FOffer x
								WHERE x.LegalEntityKey = t.LegalEntityKey
								GROUP BY x.ProductType
							FOR XML PATH (''), TYPE).value('.','VARCHAR(max)'), ''), 1, 2, '') ProductTypes,
				STUFF(ISNULL((	SELECT ', ' + x.LegalEntityAddress
								FROM #FOffer x
								WHERE x.LegalEntityKey = t.LegalEntityKey
								GROUP BY x.LegalEntityAddress
							FOR XML PATH (''), TYPE).value('.','VARCHAR(max)'), ''), 1, 2, '') LegalEntityAddress,
				STUFF(ISNULL((	SELECT ', ' + x.PropertyAddress
								FROM #FOffer x
								WHERE x.LegalEntityKey = t.LegalEntityKey
								GROUP BY x.PropertyAddress
							FOR XML PATH (''), TYPE).value('.','VARCHAR(max)'), ''), 1, 2, '') PropertyAddress
		INTO	#FlatInfo
		FROM	#FOffer t
		GROUP BY t.LegalEntityKey		


		CREATE INDEX ix_LegalEntityKey ON #FlatInfo(LegalEntityKey);
		
		--	SELECT * FROM #FlatInfo
------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
		
		--IF LegalEntity record exists in the table, delete it.			 
		IF @LegalEntityKey IS NOT NULL
		BEGIN
			IF OBJECT_ID('[2am].solr.Client') IS NOT NULL
				BEGIN

					DELETE 
					FROM	[2AM].solr.Client
					WHERE	LegalEntityKey = @LegalEntityKey
				END -- END DELETE
		END

		ELSE
		/*
		This piece of code will execute when we are NOT passing through a parameter 
		which means clean out the table and reload the full batch.

		If Exists then clean out the table.
		*/
		BEGIN

			IF OBJECT_ID('[2am].solr.Client') IS NOT NULL
				TRUNCATE TABLE [2am].solr.Client			
		END
		
		INSERT INTO [2am].solr.Client (
						LegalEntityKey,							
						LegalEntityStatusKey,
						LegalEntityType,
						LegalName,
						FirstNames,		
						Surname,		
						LegalIdentity,
						LegalIdentityType,														
						PreferredName,
						TaxNumber,
						HomePhoneNumber,
						WorkPhoneNumber,
						CellPhoneNumber,
						EmailAddress,
						FaxNumber,
						RoleType,							
						Accounts,
						AccountStatus,
						OfferKey,
						OfferStatus,
						OfferType,
						Product,																		
						LegalEntityAddress,
						PropertyAddress,												
						ClientLead,
						LastModifiedDate)
		SELECT  DISTINCT	le.LegalEntityKey,							
							LegalEntityStatusKey,
							CASE	WHEN LegalEntityType in ('Company','Close Corporation','Trust')
									THEN 'Business'	
									ELSE 'Person'
							END AS LegalEntityType,
							LegalName,
							FirstNames,		
							Surname,		
							LegalIdentity,
							LegalIdentityType,													
							PreferredName,
							TaxNumber,
							HomePhoneNumber,
							WorkPhoneNumber,
							CellPhoneNumber,
							EmailAddress,
							FaxNumber,
							fa.RoleType,							
							CAST(ISNULL(fa.Accounts,'') as varchar(max)) as 'Accounts',
							ISNULL(fa.AccountStatus,'') as AccountStatus,
							CAST(ISNULL(fa.Offers,'')   as varchar(max)) as 'OfferKey',
							ISNULL(fa.OfferStatuses,'') as OfferStatus,
							fa.OfferType,
							fa.ProductTypes AS Product,												
							fa.LegalEntityAddress,
							fa.PropertyAddress,
							'                          ',
							GETDATE()
		FROM		#LE le
		LEFT JOIN	#FlatInfo fa on le.LegalEntityKey = fa.LegalEntityKey				

		INSERT INTO [2am].solr.Client (
						LegalEntityKey,							
						LegalEntityStatusKey,
						LegalEntityType,
						LegalName,
						FirstNames,		
						Surname,		
						LegalIdentity,
						LegalIdentityType,														
						PreferredName,
						TaxNumber,
						HomePhoneNumber,
						WorkPhoneNumber,
						CellPhoneNumber,
						EmailAddress,
						FaxNumber,
						RoleType,							
						--AccountKey,
						Accounts,
						AccountStatus,
						OfferKey,
						OfferStatus,
						OfferType,
						Product,																		
						LegalEntityAddress,
						PropertyAddress,
						ClientLead,
						LastModifiedDate
						)
		SELECT  DISTINCT	lead.LegalEntityKey,							
							lead.LegalEntityStatusKey,
							CASE	WHEN lead.LegalEntityType in ('Company','Close Corporation','Trust')
									THEN 'Business'	
									ELSE 'Person'
							END AS LegalEntityType,
							lead.LegalName,
							lead.FirstNames,		
							lead.Surname,		
							lead.LegalIdentity,
							lead.LegalIdentityType,													
							lead.PreferredName,
							lead.TaxNumber,
							lead.HomePhoneNumber,
							lead.WorkPhoneNumber,
							lead.CellPhoneNumber,
							lead.EmailAddress,
							lead.FaxNumber,
							'' as RoleType,							
							'' as 'Accounts',
							'' as 'AccountStatus',
							'' as 'OfferKey',
							'' as 'OfferStatus',
							'' as 'OfferType',
							'' AS 'Product',												
							isnull (fa.LegalEntityAddress,'') as 'LegalEntityAddress',
							isnull (fa.PropertyAddress,'') as 'PropertyAddress',							
							'                          ',
							GETDATE()		
		FROM #leads lead
		LEFT JOIN #FlatInfo fa on lead.LegalEntityKey = fa.LegalEntityKey
		
		UPDATE [2am].solr.Client SET		 Accounts			= REPLACE(Accounts,',',' ')
											,AccountStatus		= REPLACE(AccountStatus,',',' ')
											,OfferKey			= REPLACE(OfferKey,',',' ')
											,OfferStatus		= REPLACE(OfferStatus,',',' ')
											,OfferType			= REPLACE(OfferType,',',' ')
											,Product			= REPLACE(Product,',',' ')
											,LegalEntityAddress = REPLACE(LegalEntityAddress,',',' ')
											,PropertyAddress	= REPLACE(PropertyAddress,',',' ')		
		WHERE	(	LegalEntityKey = @LegalEntityKey 
					OR	@LegalEntityKey IS NULL )

		IF NOT EXISTS (	SELECT Name
						FROM [2am].sys.indexes
						WHERE Name LIKE 'ix_LegalEntityKey%'
					  )

		CREATE INDEX ix_LegalEntityKey ON [2am].solr.Client(LegalEntityKey)



		Select	c.LegalEntityKey, 1 as 'Sort'
		into	#OpenAccount
		FROM		[2am].solr.Client c   (NOLOCK)
		inner join	[2am].dbo.LegalEntity le (NOLOCK) on c.LegalEntityKey = le.LegalEntityKey
		left join	[2am].dbo.Role r	  (NOLOCK) on le.LegalEntityKey = r.LegalEntityKey
		left join	[2am].dbo.Account a (NOLOCK) on r.AccountKey = a.AccountKey
		WHERE c.AccountStatus like '%Open%'
		AND	(	c.LegalEntityKey = @LegalEntityKey 
					OR	@LegalEntityKey IS NULL )


		Select c.LegalEntityKey, 2 as 'Sort'
		into #OpenOffer
		FROM		[2am].solr.Client c (NOLOCK)
		inner join	[2am].dbo.LegalEntity le (NOLOCK) on c.LegalEntityKey = le.LegalEntityKey
		left join	[2am].dbo.OfferRole r (NOLOCK) on le.LegalEntityKey = r.LegalEntityKey
		left join	[2am].dbo.Offer a (NOLOCK) on r.OfferKey = a.OfferKey
		WHERE (c.OfferStatus like '%Open%'
		OR c.OfferStatus like '%Accepted%')
		and c.LegalEntityKey not in (Select LegalEntityKey from #OpenAccount)
		AND	(	c.LegalEntityKey = @LegalEntityKey 
					OR	@LegalEntityKey IS NULL )

		Select c.LegalEntityKey, 3 as 'Sort'
		into #ClosedAccount
		FROM		[2am].solr.Client c (NOLOCK)
		inner join	[2am].dbo.LegalEntity le (NOLOCK) on c.LegalEntityKey = le.LegalEntityKey
		left join	[2am].dbo.Role r (NOLOCK) on le.LegalEntityKey = r.LegalEntityKey
		left join	[2am].dbo.Account a (NOLOCK) on r.AccountKey = a.AccountKey
		WHERE	c.AccountStatus like '%Closed%'
		and		(		c.LegalEntityKey not in (Select LegalEntityKey from #OpenAccount)
					and	c.LegalEntityKey not in (Select LegalEntityKey from #OpenOffer)
				)
		AND	(	c.LegalEntityKey = @LegalEntityKey 
					OR	@LegalEntityKey IS NULL )


		Select	c.LegalEntityKey, 4 as 'Sort'
		into	#ClosedOffer
		FROM		[2am].solr.Client c (NOLOCK)
		inner join	[2am].dbo.LegalEntity le (NOLOCK) on c.LegalEntityKey = le.LegalEntityKey
		left join	[2am].dbo.OfferRole r (NOLOCK) on le.LegalEntityKey = r.LegalEntityKey
		left join	[2am].dbo.Offer a (NOLOCK) on r.OfferKey = a.OfferKey
		WHERE	(c.OfferStatus like '%Open%'
		AND		c.OfferStatus like '%Accepted%')
		and	(			c.LegalEntityKey not in (Select LegalEntityKey from #OpenAccount)
				and		c.LegalEntityKey not in (Select LegalEntityKey from #ClosedAccount)
				and		c.LegalEntityKey not in (Select LegalEntityKey from #OpenOffer)
			)
		AND	(	c.LegalEntityKey = @LegalEntityKey 
					OR	@LegalEntityKey IS NULL )

		Select c.LegalEntityKey, 5 as 'Sort'
		into #Other
		FROM [2am].solr.Client c (NOLOCK)
		WHERE	(	c.LegalEntityKey not in (Select LegalEntityKey from #OpenAccount)
				and	c.LegalEntityKey not in (Select LegalEntityKey from #ClosedAccount)
				and	c.LegalEntityKey not in (Select LegalEntityKey from #OpenOffer)
				and c.LegalEntityKey not in (Select LegalEntityKey from #ClosedOffer)
			  )
		AND	(	c.LegalEntityKey = @LegalEntityKey 
					OR	@LegalEntityKey IS NULL )

		UPDATE [2am].solr.Client SET ClientLead = Sort
		FROM #OpenAccount oa
		WHERE [2am].solr.Client.LegalEntityKey = oa.LegalEntityKey

		UPDATE [2am].solr.Client SET ClientLead = Sort
		FROM #OpenOffer oa
		WHERE [2am].solr.Client.LegalEntityKey = oa.LegalEntityKey

		UPDATE [2am].solr.Client SET ClientLead = Sort
		FROM #ClosedAccount oa
		WHERE [2am].solr.Client.LegalEntityKey = oa.LegalEntityKey

		UPDATE [2am].solr.Client SET ClientLead = Sort
		FROM #ClosedOffer oa
		WHERE [2am].solr.Client.LegalEntityKey = oa.LegalEntityKey

		UPDATE [2am].solr.Client SET ClientLead = Sort
		FROM #Other oa
		WHERE [2am].solr.Client.LegalEntityKey = oa.LegalEntityKey


		SELECT * 
		FROM [2am].solr.Client
		WHERE ( LegalEntityKey = @LegalEntityKey 
			OR	@LegalEntityKey IS NULL )
	
		END TRY

		BEGIN CATCH

	
			set @Msg = 'solr.pClientExtract: ' + ISNULL(ERROR_MESSAGE(), 'Failed!')
			RAISERROR(@Msg,16,1)

			SELECT TOP 0 * INTO #Errors FROM process.template.errors
	
			DELETE FROM #Errors
			INSERT INTO #Errors (ErrorCodeKey, DateOfError, MSG, SeverityTypeKey)
			SELECT (SELECT ErrorCodeKey FROM process.errorhandling.ErrorCode (NOLOCK) WHERE Description LIKE 'Solr Extract Failure'), GETDATE(), @Msg, 1
	
			EXEC process.errorhandling.pLogErrors @Msg OUTPUT
				
		END CATCH
	/*
	--Helper Code:	
			DECLARE @msg VARCHAR(1024)
		
			EXEC Process.solr.pClientExtract @msg OUTPUT,55841
			SELECT @msg
	*/	
END --End Proc
GO

PRINT 'solr.pClientExtract deployed: ' + cast(getdate() as varchar) + ' to server: '+ @@Servername
GO

GRANT EXECUTE ON OBJECT::solr.pClientExtract TO [Batch];
GRANT EXECUTE ON OBJECT::solr.pClientExtract TO [ProcessRole];
GRANT EXECUTE ON OBJECT::solr.pClientExtract TO [AppRole];



