USE [2am]
GO
/****** Object:  Trigger [dbo].[tu_OfferRole_Audit]    Script Date: 2015-01-20 03:51:23 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER TRIGGER [dbo].[tu_OfferRole_Audit] on [dbo].[OfferRole] 
AFTER UPDATE 
AS 

/*************************************************************************************************************************
		Author		:	??
		CreateDate	:	??
		Description	:	Trigger to insert into the [2AM_AuditOfferRole] table
																																									
		History:
					2015/03/03	VirekR		Passing the legalentitykey that was updated to service broker to enable
											Solr Index to be updated.
											Passed via a temp table and a proc that will determine which
											queue to pass to.
		
**************************************************************************************************************************/
	SET NOCOUNT ON 
BEGIN 

	INSERT INTO warehouse.dbo.[2AM_AuditOfferRole]
	(
	  AuditAddUpdateDelete,
	  OfferRoleKey, 
	  LegalEntityKey, 
	  OfferKey, 
	  OfferRoleTypeKey, 
	  GeneralStatusKey, 
	  StatusChangeDate
	)
	SELECT 'U', 
		   OfferRoleKey, 
		   LegalEntityKey, 
		   OfferKey, 
		   OfferRoleTypeKey, 
		   GeneralStatusKey, 
		   StatusChangeDate
	FROM INSERTED;


/* Lets get the LegalEntityKey and pass it to service broker to initiate a conversation */		SELECT TOP 0 * INTO #SolrIndexUpdate FROM Process.template.SolrIndexUpdate	INSERT INTO	#SolrIndexUpdate (ProcessStatusKey, GenericKey, GenericKeyTypeKey)	SELECT		1, CONVERT(XML, CONVERT(NVARCHAR(MAX), LegalEntityKey)),3 --	SELECT * FROM [2am].dbo.GenericKeyType
	FROM		INSERTED i  	   	 		/**********************************************************/
		EXEC process.solr.pServiceBrokerQueueDetermine
	/**********************************************************/ 
END --END TRIGGER
GO