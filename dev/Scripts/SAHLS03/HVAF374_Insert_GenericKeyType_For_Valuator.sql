USE [2AM]
Go

If Not Exists (Select * From GenericKeyType Where GenericKeyTypeKey = 59)
Begin 

	Insert Into GenericKeyType (GenericKeyTypeKey, [Description], [TableName], [PrimaryKeyColumn]) Values (59, 'Valuator', '[2AM].[dbo].[Valuator]', 'ValuatorKey')

End
Go


