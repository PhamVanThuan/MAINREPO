USE [2am]
GO
/****** Object:  Trigger [dbo].[tu_Address]    Script Date: 2015-02-17 03:36:21 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
ALTER TRIGGER [dbo].[tu_Address] ON [dbo].[Address]
FOR UPDATE 
AS


/*************************************************************************************************************************
		Author		:	??
		CreateDate	:	??
		Description	:	Trigger to insert into the AuditAddress table
																																									
		History:
					2015/02/23	VirekR		Passing the legalentitykey that was updated to service broker to enable
											Solr Index to be updated.
											Passed via a temp table and a proc that will determine which
											queue to pass to.			
		
**************************************************************************************************************************/

 SET NOCOUNT ON 


BEGIN
INSERT INTO dbo.AuditAddress (
	AuditAddUpdateDelete,
	AddressKey,
	AddressFormatKey,
	BoxNumber,
	UnitNumber,
	BuildingNumber,
	BuildingName,
	StreetNumber,
	StreetName,
	SuburbKey,
	PostOfficeKey,
	RRR_CountryDescription,
	RRR_ProvinceDescription,
	RRR_CityDescription,
	RRR_SuburbDescription,
	RRR_PostalCode,
	UserID,
	ChangeDate,
	SuiteNumber,
	FreeText1,
	FreeText2,
	FreeText3,
	FreeText4,
	FreeText5
)
SELECT
	'U' as AuditAddUpdateDelete,
	AddressKey,
	AddressFormatKey,
	BoxNumber,
	UnitNumber,
	BuildingNumber,
	BuildingName,
	StreetNumber,
	StreetName,
	SuburbKey,
	PostOfficeKey,
	RRR_CountryDescription,
	RRR_ProvinceDescription,
	RRR_CityDescription,
	RRR_SuburbDescription,
	RRR_PostalCode,
	UserID,
	ChangeDate,
	SuiteNumber,
	FreeText1,
	FreeText2,
	FreeText3,
	FreeText4,
	FreeText5
FROM
	INSERTED;
			
	/* Lets get the LegalEntityKey and pass it to service broker to initiate a conversation */
	
	SELECT TOP 0 * INTO #SolrIndexUpdate FROM Process.template.SolrIndexUpdate

	INSERT INTO	#SolrIndexUpdate (ProcessStatusKey, GenericKey, GenericKeyTypeKey)
	SELECT		1,CONVERT(XML, CONVERT(NVARCHAR(MAX), LegalEntityKey)),3 --	SELECT * FROM [2am].dbo.GenericKeyType
	FROM		INSERTED i  
	INNER JOIN	LegalEntityAddress lea (NOLOCK) ON lea.AddressKey = i.AddressKey 
	
	/**********************************************************/
		EXEC process.solr.pServiceBrokerQueueDetermine
	/**********************************************************/

 END;