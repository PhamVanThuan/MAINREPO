USE [2am]
GO
/****** Object:  Trigger [dbo].[tu_Attorney_Audit]    Script Date: 2015-04-21 09:27:27 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER trigger dbo.tu_Attorney_Audit on dbo.Attorney
AFTER UPDATE 
AS 

/*************************************************************************************************************************
		Author		:	??
		CreateDate	:	??
		Description	:	Trigger to insert into the 2AM_AuditAttorney table
																																									
		History:
					2015/02/24	VirekR		Passing the legalentitykey that was updated to service broker to enable
											Solr Index to be updated.
											Passed via a temp table and a proc that will determine which
											queue to pass to.		
		
**************************************************************************************************************************/
SET NOCOUNT ON 

BEGIN 

INSERT INTO Warehouse.dbo.[2AM_AuditAttorney]
(
   [AuditAddUpdateDelete]
  ,[AttorneyKey] 
  ,[DeedsOfficeKey] 
  ,[AttorneyContact] 
  ,[AttorneyMandate] 
  ,[AttorneyWorkFlowEnabled] 
  ,[AttorneyLoanTarget] 
  ,[AttorneyFurtherLoanTarget] 
  ,[AttorneyLitigationInd] 
  ,[LegalEntityKey] 
  ,[AttorneyRegistrationInd] 
  ,[GeneralStatusKey] 
)
SELECT 'U' 
       ,[AttorneyKey] 
       ,[DeedsOfficeKey] 
       ,[AttorneyContact] 
       ,[AttorneyMandate] 
       ,[AttorneyWorkFlowEnabled] 
       ,[AttorneyLoanTarget] 
       ,[AttorneyFurtherLoanTarget] 
       ,[AttorneyLitigationInd] 
       ,[LegalEntityKey] 
       ,[AttorneyRegistrationInd] 
       ,[GeneralStatusKey] 
FROM INSERTED;

/* Lets get the LegalEntityKey and pass it to service broker to initiate a conversation */			SELECT TOP 0 * INTO #SolrIndexUpdate FROM Process.template.SolrIndexUpdate		INSERT INTO	#SolrIndexUpdate (ProcessStatusKey, GenericKey, GenericKeyTypeKey)	SELECT		1, CONVERT(XML, CONVERT(NVARCHAR(MAX), LegalEntityKey)),3 --	SELECT * FROM [2am].dbo.GenericKeyType
	FROM		INSERTED i  	   	 	
	/**********************************************************/
		EXEC process.solr.pServiceBrokerQueueDetermine
	/**********************************************************/

END --END TRIGGER
GO