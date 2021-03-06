USE [2am]
GO
/****** Object:  Trigger [dbo].[ti_Attorney_ThirdParty]    Script Date: 2015-08-21 09:08:34 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER TRIGGER dbo.ti_Attorney_ThirdParty ON dbo.Attorney
FOR INSERT
AS

/*************************************************************************************************************************
		Author		:	??
		CreateDate	:	??
		Description	:	Trigger used to ensure that the ThirdParty table is in sync with the Attorney table

		
		History		:
							2015/03/27	VirekR		Changed join to look at LegalEntityKey instead of GenericKey
													
							2015/08/21	VirekR		Populating GenericKeyTypeKey.
													Ensure that this trigger fires first so that there is a record in the 
													dbo.ThirdParty table when a new third party is inserted.
													Used as part of	Solr updates.
													
		
**************************************************************************************************************************/

SET NOCOUNT ON

INSERT INTO [dbo].[ThirdParty] (
	Id
	,LegalEntityKey
	,ThirdPartyTypeKey
	,IsPanel
	,GeneralStatusKey
	,GenericKey
	,GenericKeyTypeKey
	)
SELECT dbo.CombGuid()
	,inserted.LegalEntityKey
	,(SELECT DISTINCT ThirdPartyTypeKey FROM [2am].dbo.ThirdPartyType (NOLOCK) WHERE Description LIKE 'Attorney')
	,1
	,inserted.GeneralStatusKey
	,inserted.AttorneyKey
	,35 -- SELECT * FROM [2am].dbo.GenericKeyType
FROM inserted

GO
EXEC sp_settriggerorder @triggername=N'[dbo].[ti_Attorney_ThirdParty]', @order=N'First', @stmttype=N'INSERT'