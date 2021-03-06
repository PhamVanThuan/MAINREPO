USE [2am]
GO
/****** Object:  Trigger [dbo].[ti_Valuator_ThirdParty]    Script Date: 2015-08-21 09:24:06 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER TRIGGER dbo.ti_Valuator_ThirdParty ON  dbo.Valuator
FOR INSERT 
AS 

/*************************************************************************************************************************
		Author		:	VirekR
		CreateDate	:	2015/03/26
		Description	:	Trigger used to ensure that the ThirdParty table is in sync with the Valuator table

		History		:
						2015/08/21	VirekR		Ensure that this trigger fires first so that there is a record in the 
												dbo.ThirdParty table when a new third party is inserted. Used as part of
												Solr updates.	
																																											
		
**************************************************************************************************************************/
BEGIN
	SET NOCOUNT ON

INSERT INTO [2am].dbo.ThirdParty (
	Id
	,LegalEntityKey
	,ThirdPartyTypeKey
	,IsPanel
	,GeneralStatusKey
	,GenericKey
	)
SELECT	[2am].dbo.CombGuid()
		,inserted.LegalEntityKey
		,(SELECT DISTINCT ThirdPartyTypeKey FROM [2am].dbo.ThirdPartyType (NOLOCK) WHERE Description = 'Valuer')
		,0
		,inserted.GeneralStatusKey
		,inserted.ValuatorKey
FROM INSERTED;

END --END TRIGGER
GO

EXEC sp_settriggerorder @triggername=N'[dbo].[ti_Valuator_ThirdParty]', @order=N'First', @stmttype=N'INSERT'

