USE [2am]
GO
/****** Object:  Trigger [dbo].[tu_Account]    Script Date: 2015-08-25 01:44:17 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
ALTER TRIGGER dbo.tu_Account ON dbo.Account
FOR UPDATE 
AS

/*************************************************************************************************************************
		Author		:	??
		CreateDate	:	??
		Description	:	Trigger to insert into the AuditAccount table
																																									
		History:
					2015/08/25	VirekR		Passing the ThirdPartyInvoiceKey that was updated to service broker to enable
											Solr Index to be updated.
											Passed via a temp table and a proc that will determine which
											queue to pass to.		
		
**************************************************************************************************************************/
 SET NOCOUNT ON 

BEGIN

INSERT INTO dbo.AuditAccount (
	AuditAddUpdateDelete,
	AccountKey,
	FixedPayment,
	AccountStatusKey,
	InsertedDate,
	OriginationSourceProductKey,
	OpenDate,
	CloseDate,
	RRR_ProductKey,
	RRR_OriginationSourceKey,
	UserID,
	ChangeDate,
	SPVKey,
	ParentAccountKey
)
SELECT
	'U' as AuditAddUpdateDelete,
	AccountKey,
	FixedPayment,
	AccountStatusKey,
	InsertedDate,
	OriginationSourceProductKey,
	OpenDate,
	CloseDate,
	RRR_ProductKey,
	RRR_OriginationSourceKey,
	UserID,
	ChangeDate,
	SPVKey,
	ParentAccountKey
FROM
	INSERTED;
 /* Lets get the ThirdPartyInvoiceKey and pass it to service broker to initiate a conversation */
	
	SELECT TOP 0 * INTO #SolrIndexUpdate FROM Process.template.SolrIndexUpdate

	INSERT INTO	#SolrIndexUpdate (ProcessStatusKey, GenericKey, GenericKeyTypeKey)
	SELECT		1,CONVERT(XML, CONVERT(NVARCHAR(MAX), tpi.ThirdPartyInvoiceKey)),54 -- SELECT * FROM [2am].dbo.GenericKeyType
	FROM		INSERTED i 
	INNER JOIN	[2am].dbo.ThirdPartyInvoice tpi (NOLOCK) ON tpi.AccountKey = i.AccountKey
	

	/**********************************************************/		EXEC process.solr.pServiceBrokerQueueDetermine	/**********************************************************/
END;
