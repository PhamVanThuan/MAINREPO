
USE [2AM]
Go


If Not Exists (Select * From ExternalRoleType Where ExternalRoleTypeKey = 11)
Begin 
	
	Insert Into ExternalRoleType (ExternalRoleTypeKey, ExternalRoleTypeGroupKey, [Description]) Values (11, 4, 'Attorney Contact')

End
Go

