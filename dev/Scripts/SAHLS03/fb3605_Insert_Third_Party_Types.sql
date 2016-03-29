
USE [2AM]
GO


If not exists (Select 1 From [dbo].[ThirdPartyType] Where ThirdPartyTypeKey = 1)
BEGIN

	Declare @ThirdPartyTypeKey int
	Set @ThirdPartyTypeKey = 1

	Insert into [dbo].[ThirdPartyType] (ThirdPartyTypeKey, [Description]) Values (@ThirdPartyTypeKey, 'Attorney')
	
END


