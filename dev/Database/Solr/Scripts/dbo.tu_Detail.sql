USE [2am]
GO
/****** Object:  Trigger [dbo].[tu_Detail]    Script Date: 2015-05-08 02:42:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
ALTER TRIGGER dbo.tu_Detail ON dbo.Detail
FOR UPDATE 
AS
/*************************************************************************************************************************
		Author		:	??
		CreateDate	:	??
		Description	:	Trigger to insert into the AuditDetail table
																																									
		History:
					2015/02/24	VirekR		Passing the legalentitykey that was updated to service broker to enable
											Solr Index to be updated.
											Passed via a temp table and a proc that will determine which
											queue to pass to.			
		
**************************************************************************************************************************/ SET NOCOUNT ON 

BEGIN
	INSERT INTO dbo.AuditDetail (
		AuditAddUpdateDelete,
		[DetailKey],
		[DetailTypeKey],
		[AccountKey],
		[DetailDate],
		[Amount],
		[Description],
		[LinkID],
		[UserID],
		[ChangeDate]
	)
	SELECT
		'U' as AuditAddUpdateDelete,
		[DetailKey],
		[DetailTypeKey],
		[AccountKey],
		[DetailDate],
		[Amount],
		[Description],
		[LinkID],
		[UserID],
		[ChangeDate]
	FROM	INSERTED;

	/* Lets get the LegalEntityKey and pass it to service broker to initiate a conversation */
	
	SELECT TOP 0 * INTO #SolrIndexUpdate FROM Process.template.SolrIndexUpdate

	INSERT INTO	#SolrIndexUpdate (ProcessStatusKey, GenericKey, GenericKeyTypeKey)
	SELECT		1,CONVERT(XML, CONVERT(NVARCHAR(MAX), fat.LegalEntityKey)),3 -- SELECT * FROM [2am].dbo.GenericKeyType
	FROM		INSERTED i 
	INNER JOIN	[2am].dbo.ForeclosureAttorneyDetailTypeMapping fat (NOLOCK) ON i.DetailTypeKey = fat.DetailTypeKey
	WHERE	fat.GeneralStatusKey = 1

	/**********************************************************/		EXEC process.solr.pServiceBrokerQueueDetermine	/**********************************************************/

END --END TRIGGER
GO