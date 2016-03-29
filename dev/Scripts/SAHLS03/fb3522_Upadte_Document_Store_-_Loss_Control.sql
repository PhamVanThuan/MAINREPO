
USE [ImageIndex]
GO

If Not Exists(Select ID From [dbo].[STOR] 
			Where [ID] = 44
			And [Key1] = 'Loan Number'
			And [Key2] = 'Third Party Invoice Key'
			And [Key3] = 'Email Subject'
			And [Key4] = 'From Email Address'
			And [Key5] = 'Invoice File Name') 
	Begin
		
		UPDATE [dbo].[STOR]
		SET [Key1] = 'Loan Number',
			[Key2] = 'Third Party Invoice Key',
			[Key3] = 'Email Subject',
			[Key4] = 'From Email Address', 
			[Key5] = 'Invoice File Name',
			[Key6] = 'Category',
			[Key7] = 'Date Received',
			[Key8] = 'Date Processed',
			[Key1Options] = '00010',
			[Key2Options] = '00010',
			[Key3Options] = '00010',
			[Key4Options] = '00010',
			[Key5Options] = '00010',
			[Key6Options] = '00010',
			[Key7Options] = '00010',
			[Key8Options] = '00010'
		WHERE ID = 44
		
	End	
