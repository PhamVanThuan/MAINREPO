USE [2am]
GO
/****** Object:  Trigger [dbo].[tu_LegalEntity]    Script Date: 2015-01-15 01:53:09 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER TRIGGER dbo.tu_LegalEntity ON dbo.LegalEntity
FOR UPDATE 
AS

/*************************************************************************************************************************
		Author		:	??
		CreateDate	:	??
		Description	:	Trigger to insert into the AuditLegalEntity table
																																									
		History:
					2015/02/24	VirekR		Passing the legalentitykey that was updated to service broker to enable
											Solr Index to be updated.
											Passed via a temp table and a proc that will determine which
											queue to pass to.
		
**************************************************************************************************************************/


SET NOCOUNT ON 
BEGIN
	INSERT INTO dbo.AuditLegalEntity (
		AuditAddUpdateDelete,
		LegalEntityKey, 
		LegalEntityTypeKey, 
		MaritalStatusKey, 
		GenderKey, 
		PopulationGroupKey, 
		IntroductionDate, 
		Salutationkey, 
		FirstNames, 
		Initials, 
		Surname, 
		PreferredName, 
		IDNumber, 
		PassportNumber, 
		TaxNumber, 
		RegistrationNumber, 
		RegisteredName, 
		TradingName, 
		DateOfBirth, 
		HomePhoneCode, 
		HomePhoneNumber, 
		WorkPhoneCode, 
		WorkPhoneNumber, 
		CellPhoneNumber, 
		EmailAddress, 
		FaxCode, 
		FaxNumber, 
		[Password], 
		CitizenTypeKey, 
		LegalEntityStatusKey, 
		Comments, 
		LegalEntityExceptionStatusKey, 
		UserID, 
		ChangeDate, 
		EducationKey, 
		HomeLanguageKey, 
		DocumentLanguageKey, 
		ResidenceStatusKey
	)
	SELECT
		'U' as AuditAddUpdateDelete,
		LegalEntityKey, 
		LegalEntityTypeKey, 
		MaritalStatusKey, 
		GenderKey, 
		PopulationGroupKey, 
		IntroductionDate, 
		Salutationkey, 
		FirstNames, 
		Initials, 
		Surname, 
		PreferredName, 
		IDNumber, 
		PassportNumber, 
		TaxNumber, 
		RegistrationNumber, 
		RegisteredName, 
		TradingName, 
		DateOfBirth, 
		HomePhoneCode, 
		HomePhoneNumber, 
		WorkPhoneCode, 
		WorkPhoneNumber, 
		CellPhoneNumber, 
		EmailAddress, 
		FaxCode, 
		FaxNumber, 
		[Password], 
		CitizenTypeKey, 
		LegalEntityStatusKey, 
		Comments, 
		LegalEntityExceptionStatusKey, 
		UserID, 
		ChangeDate, 
		EducationKey, 
		HomeLanguageKey, 
		DocumentLanguageKey, 
		ResidenceStatusKey
	FROM
		INSERTED;
						
	/* Lets get the LegalEntityKey and pass it to service broker to initiate a conversation */		SELECT TOP 0 * INTO #SolrIndexUpdate FROM Process.template.SolrIndexUpdate	INSERT INTO	#SolrIndexUpdate (ProcessStatusKey, GenericKey, GenericKeyTypeKey)	SELECT		1, CONVERT(XML, CONVERT(NVARCHAR(MAX), LegalEntityKey)),3 --	SELECT * FROM [2am].dbo.GenericKeyType
	FROM		INSERTED i  	 		/**********************************************************/		EXEC process.solr.pServiceBrokerQueueDetermine	/**********************************************************/

END --END TRIGGER
GO
