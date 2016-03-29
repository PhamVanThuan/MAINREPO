
USE [ImageIndex]
GO


If Not Exists (Select GroupID, STORid From [dbo].[GroupsAndSTORs] 
				Where GroupID = 134 
				And STORid = 44)
	Begin
		
		Insert Into [dbo].[GroupsAndSTORs] (GroupID, STORid) values (134, 44)
		
	End

