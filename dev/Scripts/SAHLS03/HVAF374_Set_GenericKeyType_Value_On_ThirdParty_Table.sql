
USE [2AM]
GO

IF EXISTS (SELECT 1 FROM dbo.ThirdParty TP WHERE TP.GenericKeyTypeKey is null)
BEGIN
	
	Declare @genericKeyTypeKey int

	Select @genericKeyTypeKey = GKT.GenericKeyTypeKey From GenericKeyType GKT Where GKT.TableName = '[2am].[dbo].[Attorney]'

	If not @genericKeyTypeKey is null
	Begin
		Update dbo.ThirdParty 
			Set GenericKeyTypeKey = @genericKeyTypeKey
		Where ThirdPartyTypeKey = 1
	End
		
	Select @genericKeyTypeKey = GKT.GenericKeyTypeKey From GenericKeyType GKT Where GKT.TableName = '[2am].[dbo].[Valuator]'

	If not @genericKeyTypeKey is null
	Begin
		Update dbo.ThirdParty 
			Set GenericKeyTypeKey = @genericKeyTypeKey
		Where ThirdPartyTypeKey = 2
	End

END
GO
