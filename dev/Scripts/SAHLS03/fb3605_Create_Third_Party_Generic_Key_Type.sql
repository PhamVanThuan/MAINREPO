
USE [2AM]
Go


If Not Exists (Select * From GenericKeyType Where GenericKeyTypeKey = 56)
Begin 

	Insert Into GenericKeyType (GenericKeyTypeKey, [Description], [TableName], [PrimaryKeyColumn]) Values (56, 'ThirdParty', '[2AM].[dbo].[ThirdParty]', 'ThirdPartyKey')

End
Go

