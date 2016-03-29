
USE [2AM]

If ((Select count(A.AttorneyKey) From [dbo].[ThirdParty] TP Right Outer Join [dbo].[Attorney] A On TP.GenericKey = A.AttorneyKey Where TP.GenericKey is null) > 0)
Begin
	
	Declare @ThirdPartyTypeKey int
	Set @ThirdPartyTypeKey = 1
	
	Insert Into [dbo].[ThirdParty] (LegalEntityKey, ThirdPartyTypeKey, IsPanel, GeneralStatusKey, GenericKey)
	Select A.LegalEntityKey, @ThirdPartyTypeKey, 1, A.GeneralStatusKey, A.AttorneyKey
	From [dbo].[ThirdParty] TP 
	Right Outer Join [dbo].[Attorney] A 
		On TP.GenericKey = A.AttorneyKey 
		Where TP.GenericKey is null
		
End
GO




