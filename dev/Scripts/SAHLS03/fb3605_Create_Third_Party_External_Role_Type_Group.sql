
USE [2AM]
Go


If Not Exists (Select * From ExternalRoleTypeGroup Where ExternalRoleTypeGroupKey = 4)
Begin 
	
	SET IDENTITY_INSERT ExternalRoleTypeGroup ON

	Insert Into ExternalRoleTypeGroup (ExternalRoleTypeGroupKey, [Description]) Values (4, 'Third Party Contacts')

	SET IDENTITY_INSERT ExternalRoleTypeGroup OFF


End
Go