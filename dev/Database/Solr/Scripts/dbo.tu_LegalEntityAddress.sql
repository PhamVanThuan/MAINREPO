USE [2am]
GO
/****** Object:  Trigger [dbo].[tu_LegalEntityAddress]    Script Date: 2015-01-20 03:40:50 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
ALTER TRIGGER [dbo].[tu_LegalEntityAddress] ON [dbo].[LegalEntityAddress]
FOR UPDATE 
AS

/*************************************************************************************************************************
		Author		:	??
		CreateDate	:	??
		Description	:	Trigger to insert into the AuditLegalEntityAddress table
																																									
		History:
					2015/02/24	VirekR		Passing the legalentitykey that was updated to service broker to enable
											Solr Index to be updated.
											Passed via a temp table and a proc that will determine which
											queue to pass to.			
		
**************************************************************************************************************************/
 SET NOCOUNT ON 

BEGIN
INSERT INTO dbo.AuditLegalEntityAddress (
	AuditAddUpdateDelete,
	LegalEntityAddressKey,
	LegalEntityKey,
	AddressKey,
	AddressTypeKey,
	EffectiveDate,
	GeneralStatusKey
)
SELECT
	'U' as AuditAddUpdateDelete,
	LegalEntityAddressKey,
	LegalEntityKey,
	AddressKey,
	AddressTypeKey,
	EffectiveDate,
	GeneralStatusKey
FROM
	INSERTED;
				
	/* Lets get the LegalEntityKey and pass it to service broker to initiate a conversation */		SELECT TOP 0 * INTO #SolrIndexUpdate FROM Process.template.SolrIndexUpdate	INSERT INTO	#SolrIndexUpdate (ProcessStatusKey, GenericKey, GenericKeyTypeKey)	SELECT		1,CONVERT(XML, CONVERT(NVARCHAR(MAX), LegalEntityKey)) ,3 --	SELECT * FROM [2am].dbo.GenericKeyType
	FROM		INSERTED i  			/**********************************************************/		EXEC process.solr.pServiceBrokerQueueDetermine	/**********************************************************/END --END TRIGGER
GO
