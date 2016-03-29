
USE [ImageIndex]
GO

If Not Exists (Select ID From [dbo].[Groups] Where ID = 134)
Begin

	SET IDENTITY_INSERT [dbo].[Groups] ON
	
	Insert Into [dbo].[Groups] (ID, groupName, GroupPermissions) 
	Values (134, 'Loss Control - Attorney Invoices', null)
	
	SET IDENTITY_INSERT [dbo].[Groups] OFF
End 
