USE [Process]

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

IF OBJECT_ID('solr.pTaskExtract') is null
BEGIN 
	DECLARE @Qry VARCHAR(1024)
	SET @Qry =	'CREATE PROCEDURE solr.pTaskExtract ' +
				'AS ' +
				'BEGIN ' +
				'SET NOCOUNT ON; ' +
				'END '
	EXECUTE (@Qry)
END
GO


ALTER PROCEDURE solr.pTaskExtract @Msg VARCHAR(1024) OUTPUT, @InstanceID INT = NULL
AS

/*************************************************************************************************************************
		Author		:	DeanA / VirekR
		CreateDate	:	2015/01/12
		Description	:	This proc will:

						1. Return Workflow Task related information used by Solr for indexing purposes
						
																																			
		History:
						
		
	--------------------------------------------------------------------	
	--Helper Code:	
			DECLARE @msg VARCHAR(1024)
			/*Optional Parameter of InstanceID, if populated do that one only, if not do all */  
			EXEC Process.solr.pTaskExtract @msg OUTPUT--,8146870
			SELECT @msg
	--------------------------------------------------------------------
**************************************************************************************************************************/

BEGIN
	
	BEGIN TRY

	--	DECLARE @Msg VARCHAR(1024)
	--			,@InstanceID INT = 8678822
					
	
	IF OBJECT_ID('tempdb.dbo.#Instance') IS NOT NULL
		DROP TABLE #Instance

	SELECT DISTINCT i.ID, 
					i.CreationDate,
					i.Subject,
					wl.ADUserName,
					wf.GenericKeyTypeKey,
					gk.Description AS GenericKeyType,
					pr.Name AS 'Process',
					wf.Name AS 'WorkFlow',
					s.Name	AS 'State',
					s.Type
	INTO	#Instance		   		
	FROM	x2.X2.Instance i (NOLOCK)  
	INNER JOIN x2.X2.Workflow wf (nolock) on i.WorkflowID = wf.ID	
	LEFT  JOIN x2.x2.State s (NOLOCK) on i.StateID = s.ID and s.Type in (1,5) and i.ParentInstanceID is null-- User and Archive State		
	LEFT JOIN [2AM].dbo.GenericKeyType gk (NOLOCK) ON gk.GenericKeyTypeKey = wf.GenericKeyTypeKey
	LEFT JOIN x2.X2.Process pr (nolock)  on wf.ProcessID = pr.ID
	LEFT JOIN [X2].[X2].[WorkList] wl (nolock) on wl.InstanceID = i.ID --and i.ParentInstanceID is null -- not a clone
	--WHERE i.ID = 8135992
	WHERE (
			i.ID = @InstanceID
			OR @InstanceID IS NULL
			)

	CREATE INDEX ix_InstanceID ON #Instance(ID);

	--SELECT * FROM #Instance
	--WHERE ID = 8135992

	-- Flatten ADUser Information into strings so this can be searched on
	IF OBJECT_ID('tempdb.dbo.#FlatADUser') IS NOT NULL
			DROP TABLE #FlatADUser

	SELECT t.ID,
			 STUFF(ISNULL((	SELECT ', ' + ISNULL(x.ADUserName,'')
							FROM #Instance x
							WHERE x.ID = t.ID
							GROUP BY x.ADUserName
							FOR XML PATH (''), TYPE).value('.','VARCHAR(max)'), ''), 1, 2, '') UserName
	INTO	#FlatADUser
	FROM	#Instance t
	GROUP BY t.ID		

	CREATE INDEX ix_InstanceID ON #FlatADUser(ID);

		--	SELECT * FROM #FlatADUser WHERE UserName IS NOT NULL or UserName NOT IN (' ') ORDER BY ID
	
	IF OBJECT_ID('tempdb.dbo.#Offers') IS NOT NULL
			DROP TABLE #Offers
	
	CREATE TABLE #Offers
	(	InstanceID			INT,
		ADUserName			VARCHAR(100),
		GenericKeyValue		VARCHAR(50),
		GenericKeyType		VARCHAR(50),
		GenericKeyTypeKey	VARCHAR(30),
		Attribute1Type		VARCHAR(100),
		Attribute1Value		VARCHAR(100),
		Attribute1DataType	VARCHAR(50),
		Attribute2Type		VARCHAR(100),
		Attribute2Value		VARCHAR(100),
		Attribute2DataType	VARCHAR(50),
		Attribute3Type		VARCHAR(100),
		Attribute3Value		VARCHAR(100),
		Attribute3DataType	VARCHAR(50)

	)
	
	INSERT INTO #Offers (InstanceID, ADUserName,GenericKeyValue,GenericKeyType,GenericKeyTypeKey,Attribute1Type,Attribute1Value,Attribute1DataType,Attribute2Type,Attribute2Value,Attribute2DataType,Attribute3Type,Attribute3Value,Attribute3DataType)
	SELECT *
	FROM 
	(
		Select	i.ID, 
				i.ADUserName, 
				convert(varchar(25), ac.ApplicationKey)  as 'GenericKeyValue', 
				i.GenericKeyType,
				i.GenericKeyTypeKey,
				'Application Number' as 'Attribute1Type',
				convert(varchar(25), ac.ApplicationKey) as 'Attribute1Value',
				'VarChar' as 'Attribute1DataType',
				'Application Type' as 'Attribute2Type',
				'' as 'Attribute2Value',
				'VarChar' as 'Attribute2DataType',
				'' as 'Attribute3Type',
				'' as 'Attribute3Value',
				'VarChar' as 'Attribute3DataType'
	    FROM	#Instance i		
		INNER JOIN x2.[X2DATA].[Application_Capture] ac (nolock) on i.ID = ac.InstanceID
		WHERE	(	ac.ApplicationKey IS NOT NULL
				AND ac.ApplicationKey <> 0
				)

		UNION

		Select	i.ID, 
				i.ADUserName, 
				convert(varchar(25), am.ApplicationKey)  as 'GenericKeyValue',
				i.GenericKeyType,
				i.GenericKeyTypeKey,
				'Application Number' as 'Attribute1Type',
				convert(varchar(25), am.ApplicationKey) as 'Attribute1Value',
				'VarChar' as 'Attribute1DataType',
				'Application Type' as 'Attribute2Type',
				'' as 'Attribute2Value',
				'VarChar' as 'Attribute2DataType',
				'' as 'Attribute3Type',
				'' as 'Attribute3Value',
				'VarChar' as 'Attribute3DataType'
		FROM	#Instance i
		INNER JOIN x2.[X2DATA].[Application_Management] am (nolock) on i.ID = am.InstanceID
		WHERE	(	am.ApplicationKey IS NOT NULL
				AND am.ApplicationKey <> 0
				)

		UNION


		Select	i.ID, 
				i.ADUserName, 
				convert(varchar(25), dc.AccountKey)  as 'GenericKeyValue', 
				i.GenericKeyType,
				i.GenericKeyTypeKey,
				'Account Number' as 'Attribute1Type',
				convert(varchar(25), dc.AccountKey) as 'Attribute1Value',
				'VarChar' as 'Attribute1DataType',
				'Cap Phase Start Date' as 'Attribute2Type',
				convert(varchar(10), ctc.OfferStartDate,20)  as 'Attribute2Value',
				'Date' as 'Attribute2DataType',
				'' as 'Attribute3Type',
				'' as 'Attribute3Value',
				'VarChar' as 'Attribute3DataType'

		FROM #Instance i 
		INNER JOIN x2.[X2DATA].[CAP2_Offers] dc  (nolock) on i.ID = dc.InstanceID
		INNER JOIN [2am].dbo.CapOffer co (nolock) on dc.CapOfferKey = co.CapOfferKey
		INNER JOIN [2am].dbo.[CapTypeConfiguration] ctc (nolock) on co.CapTypeConfigurationKey = ctc.CapTypeConfigurationKey
		WHERE	(	dc.AccountKey IS NOT NULL
				AND dc.AccountKey <> 0
				)	

		UNION

		Select	i.ID, 
				i.ADUserName, 
				convert(varchar(25), cr.ApplicationKey)  as 'GenericKeyValue', 
				i.GenericKeyType,
				i.GenericKeyTypeKey,
				'Application Number' as 'Attribute1Type',
				convert(varchar(25), cr.ApplicationKey) as 'Attribute1Value',
				'VarChar' as 'Attribute1DataType',
				'Application Type' as 'Attribute2Type',
				'' as 'Attribute2Value',
				'VarChar' as 'Attribute2DataType',
				'' as 'Attribute3Type',
				'' as 'Attribute3Value',
				'VarChar' as 'Attribute3DataType'
		FROM #Instance i
		INNER JOIN x2.[X2DATA].[Credit] cr (nolock) on i.ID = cr.InstanceID
		WHERE	(	ApplicationKey IS NOT NULL
				AND ApplicationKey <> 0
				)

		UNION

		
		Select	i.ID, 
				i.ADUserName, 
				convert(varchar(25), dc1.AccountKey) as 'GenericKeyValue', 
				i.GenericKeyType,
				i.GenericKeyTypeKey,
				'Account Number' as 'Attribute1Type',
				convert(varchar(25), dc1.AccountKey) as 'Attribute1Value',
				'VarChar' as 'Attribute1DataType',
				'Account Type' as 'Attribute2Type',
				CASE	WHEN (p.ProductKey = 1 or p.ProductKey = 2 or p.ProductKey = 5 or p.ProductKey = 6 or p.ProductKey = 9 OR p.ProductKey = 11 or p.ProductKey = 12) 
						THEN 'Mortgage Loan' 
						WHEN p.ProductKey = 4
						THEN 'Life'
						WHEN p.ProductKey = 3
						THEN 'HOC'
						WHEN p.ProductKey = 12
						THEN 'Personal Loan'
						ELSE isnull(p.Description,'')
				END AS 'Attribute2Value',
				'VarChar' as 'Attribute2DataType',
				'Reference' as 'Attribute3Type',
				dc1.ReferenceNumber  as 'Attribute3Value',
				'VarChar' as 'Attribute3DataType'
	FROM #Instance i 
		INNER JOIN x2.[X2DATA].[Debt_Counselling] dc  (nolock) on i.ID = dc.InstanceID
		INNER JOIN [2am].debtcounselling.DebtCounselling dc1 (nolock) on dc.DebtCounsellingKey = dc1.DebtCounsellingKey
		INNER JOIN [2am].dbo.Account a (nolock) on dc1.AccountKey = a.AccountKey
		INNER JOIN [2am].dbo.Product p (nolock) on a.RRR_ProductKey = p.ProductKey
		WHERE	(	dc.AccountKey IS NOT NULL
				AND dc.AccountKey <> 0
				)

 		UNION

		Select	i.ID, 
				i.ADUserName, 
				'GenericKeyValue' = case when le.LegalEntityTypeKey = 2 
										then case	when le.IDNumber is not null 
													then isnull(le.IDNumber,'')
													else isnull(le.PassportNumber,'')
											  end
										when (le.LegalEntityTypeKey > 3 and le.LegalEntityTypeKey < 6) 
										then isnull(le.RegistrationNumber,'')
									else isnull(le.IDNumber,'') + ' ' + isnull(le.RegistrationNumber,'') 
									end,
				'GenericKeyType' = case
										when le.LegalEntityTypeKey = 2 then case when le.IDNumber is not null then 'SA Identity Number' else 'Passport Number' end
										when le.LegalEntityTypeKey = 3  then 'Company Registration'
										when le.LegalEntityTypeKey = 4  then 'Close Corporation Registration'
										when le.LegalEntityTypeKey = 5  then 'Trust Registration'
										else 'Unknown Registration'
									end,
				i.GenericKeyTypeKey,
				'Attribute1Type' = case
										when le.LegalEntityTypeKey = 2 then case when le.IDNumber is not null then 'SA Identity Number' else 'Passport Number' end
										when le.LegalEntityTypeKey = 3  then 'Company Registration'
										when le.LegalEntityTypeKey = 4  then 'Close Corporation Registration'
										when le.LegalEntityTypeKey = 5  then 'Trust Registration'
										else 'Unknown Registration'
									end,
				'Attribute1Value' = case when le.LegalEntityTypeKey = 2 
										then case	when le.IDNumber is not null 
													then isnull(le.IDNumber,'')
													else isnull(le.PassportNumber,'')
											  end
										when (le.LegalEntityTypeKey > 3 and le.LegalEntityTypeKey < 6) 
										then isnull(le.RegistrationNumber,'')
									else isnull(le.IDNumber,'') + ' ' + isnull(le.RegistrationNumber,'') 
									end,
				'VarChar' as 'Attribute1DataType',
				'Query Date' as 'Attribute2Type',
				convert(varchar(10), i.CreationDate,20) as 'Attribute2Value',
				'Date' as 'Attribute2DataType',
				'Help Desk Category' as 'Attribute3Type',
				hdc.Description as 'Attribute3Value',
				'VarChar' as 'Attribute3DataType'
				
		FROM #Instance i
		INNER JOIN	x2.[X2DATA].[Help_Desk] hd (nolock) on i.ID = hd.InstanceID
		INNER JOIN [2am].[dbo].[HelpDeskQuery] hdq (NOLOCK) on hdq.HelpDeskQueryKey = hd.HelpDeskQueryKey
		INNER JOIN [2am].[dbo].[HelpDeskCategory] hdc (NOLOCK) on hdq.HelpDeskCategoryKey = hdc.HelpDeskCategoryKey
		INNER JOIN [2am].dbo.LegalEntity le (nolock) on hd.LegalEntityKey = le.LegalEntityKey
		WHERE	(		hd.LegalEntityKey IS NOT NULL
					AND hd.LegalEntityKey <> 0
				)

		UNION

		Select	i.ID, 
				i.ADUserName, 
				convert(varchar(25), hd.LoanNumber) as 'GenericKeyValue', 
				i.GenericKeyType,
				i.GenericKeyTypeKey,
				'Account Number' as 'Attribute1Type',
				convert(varchar(25), hd.LoanNumber) as 'Attribute1Value',
				'VarChar' as 'Attribute1DataType',
				'' as 'Attribute2Type',
				'' as 'Attribute2Value',
				'VarChar' as 'Attribute2DataType',
				'' as 'Attribute3Type',
				'' as 'Attribute3Value',
				'VarChar' as 'Attribute3DataType'
		FROM #Instance i
		INNER JOIN x2.[X2DATA].[LifeOrigination] hd (nolock) on i.ID = hd.InstanceID		
		WHERE	(		i.ID IS NOT NULL
					AND i.ID <> 0
				)

		UNION

		Select	i.ID, 
				i.ADUserName, 
				convert(varchar(25), pl.ApplicationKey)  as 'GenericKeyValue', 
				i.GenericKeyType,
				i.GenericKeyTypeKey,
				'Application Number' as 'Attribute1Type',
				convert(varchar(25), pl.ApplicationKey) as 'Attribute1Value',
				'VarChar' as 'Attribute1DataType',
				'Application Type' as 'Attribute2Type',
				'Unsecured Lending' as 'Attribute2Value',
				'VarChar' as 'Attribute2DataType',
				'' as 'Attribute3Type',
				'' as 'Attribute3Value',
				'VarChar' as 'Attribute3DataType'
		FROM #Instance i
		INNER JOIN x2.[X2DATA].[Personal_Loans] pl  (nolock) on i.ID = pl.InstanceID
		WHERE	(		ApplicationKey IS NOT NULL
					AND ApplicationKey <> 0
				)

		UNION

		Select	i.ID, 
				i.ADUserName, 
				convert(varchar(25), red.ApplicationKey)  as 'GenericKeyValue', 
				i.GenericKeyType,
				i.GenericKeyTypeKey,
				'Application Number' as 'Attribute1Type',
				convert(varchar(25), red.ApplicationKey) as 'Attribute1Value',
				'VarChar' as 'Attribute1DataType',
				'' as 'Attribute2Type',
				'' as 'Attribute2Value',
				'VarChar' as 'Attribute2DataType',
				'' as 'Attribute3Type',
				'' as 'Attribute3Value',
				'VarChar' as 'Attribute3DataType'
		FROM #Instance i
		INNER JOIN x2.[X2DATA].[Readvance_Payments] red  (nolock) on i.ID = red.InstanceID
		WHERE	(		ApplicationKey IS NOT NULL
					AND ApplicationKey <> 0
				)

		UNION

		Select	i.ID, 
				i.ADUserName, 
				convert(varchar(25), ApplicationKey)  as 'GenericKeyValue', 
				i.GenericKeyType,
				i.GenericKeyTypeKey,
				'Application Number' as 'Attribute1Type',
				convert(varchar(25), val.ApplicationKey) as 'Attribute1Value',
				'VarChar' as 'Attribute1DataType',
				'' as 'Attribute2Type',
				'' as 'Attribute2Value',
				'VarChar' as 'Attribute2DataType',
				'' as 'Attribute3Type',
				'' as 'Attribute3Value',
				'VarChar' as 'Attribute3DataType'
		FROM #Instance i 
		INNER JOIN x2.[X2DATA].[Valuations] val  (nolock) on i.ID = val.InstanceID
		WHERE	(		ApplicationKey IS NOT NULL
					AND ApplicationKey <> 0
				)

		UNION

		Select	i.ID, 
				i.ADUserName, 
				convert(varchar(25), tp.ThirdPartyInvoiceKey)  as 'GenericKeyValue', 
				i.GenericKeyType,
				i.GenericKeyTypeKey,
				'SAHL Ref'	as 'Attribute1Type',
				tpi.SahlReference as 'Attribute1Value',
				'VarChar' as 'Attribute1DataType',
				'Invoice No' as 'Attribute2Type',
				ISNULL(tpi.InvoiceNumber,'') as 'Attribute2Value',
				'VarChar' as 'Attribute2DataType',
				'Account Number' as 'Attribute3Type',
				convert(varchar(25),tpi.AccountKey) as 'Attribute3Value',
				'VarChar' as 'Attribute3DataType'
		FROM #Instance i 
		INNER JOIN x2.[X2DATA].Third_Party_Invoices tp  (nolock) on i.ID = tp.InstanceID
		LEFT OUTER JOIN [2AM].[dbo].[ThirdPartyInvoice] tpi (nolock) on tpi.ThirdPartyInvoiceKey = tp.ThirdPartyInvoiceKey
		WHERE	(		tp.AccountKey IS NOT NULL
					AND tp.AccountKey <> 0
				)

	) Offer
	
	CREATE INDEX ix_OfferInstanceID ON #Offers (InstanceID);

	--	SELECT * FROM #Offers WHERE instanceID = 8135992

	-- Get all data together in a single dataset
	IF OBJECT_ID('tempdb.dbo.#Task') IS NOT NULL
			DROP TABLE #Task

	SELECT DISTINCT
			i.ID as 'InstanceID',
			fu.UserName as 'UserName',
			i.Process,
			i.Workflow,
			i.State,
			i.Subject,			
			o.GenericKeyTypeKey,
			o.GenericKeyType,
			o.[GenericKeyValue],
			o.Attribute1Type,
			o.Attribute1Value,
			o.Attribute1DataType,
			o.Attribute2Type,
			o.Attribute2Value,
			o.Attribute2DataType,
			o.Attribute3Type,
			o.Attribute3Value,
			o.Attribute3DataType,		
			CASE	WHEN i.Type = 1
						THEN 'In Progress'
					ELSE 'Archived'
			END as  'Status',
			GETDATE() AS LastModifiedDate
	INTO	#Task
	FROM	#Instance i 	
	LEFT JOIN #FlatADUser fu on i.ID = fu.ID
	LEFT JOIN #Offers o on o.InstanceID = i.ID			

	CREATE INDEX ix_Task ON #Task (InstanceID);

	--	SELECT * FROM #Task

	UPDATE	t
	SET		Attribute2Type		= 'Application Type', 
			Attribute2Value		= ot.[Description], 
			Attribute2DataType	= 'VarChar'
	--SELECT *
	FROM #Task t
	INNER JOIN [2am].dbo.Offer o (NOLOCK) ON t.GenericKeyValue = o.OfferKey
	INNER JOIN [2am].dbo.OfferType ot (NOLOCK) ON o.OfferTypeKey = ot.OfferTypeKey
	WHERE t.GenericKeyType = 'Offer'
	AND (		t.Attribute2Type IS NULL
			OR	t.Attribute2Type = ''
		)

	UPDATE	t
	SET		Attribute1DataType = ''
	from	#Task t
	WHERE	(		Attribute1Value IS NULL
				OR  Attribute1Value = ''
			)

	UPDATE	t
	SET		Attribute2DataType = ''
	FROM	#Task t
	WHERE	(	Attribute2Value IS NULL
			OR  Attribute2Value = ''
			)


	DELETE from #Task
	WHERE InstanceID IN (
						Select t.InstanceID
						from #Task t
						INNER JOIN x2.X2.Instance i (nolock)  on i.ID = t.InstanceID
						where i.ParentInstanceID IS NOT NULL
					)


--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
-- Main Select
--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

	--IF InstanceID record exists in the table, delete it.		
        		 
		IF @InstanceID IS NOT NULL 
		BEGIN
			IF OBJECT_ID('[2am].solr.Task') IS NOT NULL
				BEGIN
					DELETE 
					FROM	[2AM].solr.Task
					WHERE	InstanceID = @InstanceID
				END
		END
		
		ELSE
		/*
		This piece of code will execute when we are NOT passing through a parameter 
		which means clean out the table and reload the full batch.

		If Exists then clean out the table
		*/
		BEGIN
			IF OBJECT_ID('[2am].solr.Task') IS NOT NULL
				TRUNCATE TABLE [2am].solr.Task						
		END

	/******************************************** INSERT WORKFLOW TASKS HERE ************************************************/
	
	INSERT INTO [2am].solr.Task
			   (InstanceID
			   ,UserName
			   ,Process
			   ,Workflow
			   ,[State]
			   ,[Subject]
			   ,GenericKeyTypeKey
			   ,GenericKeyType
			   ,GenericKeyValue
			   ,Attribute1Type
			   ,Attribute1Value
			   ,Attribute1DataType
			   ,Attribute2Type
			   ,Attribute2Value
			   ,Attribute2DataType
			   ,Attribute3Type
			   ,Attribute3Value
			   ,Attribute3DataType
			   ,[Status]
			   ,LastModifiedDate)
	SELECT	*
	FROM	#Task
	ORDER BY 1

	IF NOT EXISTS (	SELECT Name
						FROM [2am].sys.indexes
						WHERE Name LIKE 'ix_InstanceID%'
					  )

		CREATE INDEX ix_InstanceID ON [2am].solr.Task(InstanceID)

	SELECT * 
	FROM [2AM].solr.Task (NOLOCK) 
	WHERE ( InstanceID = @InstanceID
		OR	@InstanceID IS NULL )
	ORDER BY ID

	END TRY

		BEGIN CATCH

	
			SET @Msg = 'solr.pTaskExtract: ' + ISNULL(ERROR_MESSAGE(), 'Failed!')
			RAISERROR(@Msg,16,1)

			SELECT TOP 0 * INTO #Errors FROM process.template.errors
	
			DELETE FROM #Errors
			INSERT INTO #Errors (ErrorCodeKey, DateOfError, MSG, SeverityTypeKey)
			SELECT (SELECT ErrorCodeKey FROM process.errorhandling.ErrorCode (NOLOCK) WHERE Description LIKE 'Solr Extract Failure'), GETDATE(), @Msg, 1
	
			EXEC process.errorhandling.pLogErrors @Msg OUTPUT
				
		END CATCH

	--------------------------------------------------------------------	
	/*
	Helper Code:	
			DECLARE @msg VARCHAR(1024)
			/*Optional Parameter of InstanceID, if populated do that one only, if not do all */  
			EXEC Process.solr.pTaskExtract @msg OUTPUT,6063084
			SELECT @msg
	*/
	--------------------------------------------------------------------

END --End Proc
GO

PRINT 'solr.pTaskExtract deployed: ' + cast(getdate() as varchar) + ' to server: '+ @@Servername
GO

GRANT EXECUTE ON OBJECT::solr.pTaskExtract TO [Batch];
GRANT EXECUTE ON OBJECT::solr.pTaskExtract TO [ProcessRole];
GRANT EXECUTE ON OBJECT::solr.pTaskExtract TO [AppRole];