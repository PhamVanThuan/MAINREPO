
USE [ImageIndex]
Go

If Not Exists (Select ID From [dbo].[Lookup] Where ID = 1563)
Begin
	
	SET IDENTITY_INSERT [dbo].[Lookup] ON

	Insert Into [dbo].[Lookup] ([STORid],[Field],[Text],[ID]) Values (44, 5, 'Attorney Invoice', 1563)

	SET IDENTITY_INSERT [dbo].[Lookup] OFF

End