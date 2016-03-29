
USE [ImageIndex]
GO

If Not Exists(Select ID From [dbo].[STOR] 
			Where [ID] = 44) 
	Begin
	
		SET IDENTITY_INSERT [dbo].[STOR] ON

		INSERT INTO [dbo].[STOR]
		([ID],[Name],[Description],[Folder],[bFulltext],[NonIndexableChars]
			,[Key1],[Key2],[Key3],[Key4],[Key5],[Key6],[Key7],[Key8]
			,[Key1Options],[Key2Options],[Key3Options],[Key4Options],[Key5Options],[Key6Options],[Key7Options],[Key8Options]
			,[Key1MinMax],[Key2MinMax],[Key3MinMax],[Key4MinMax],[Key5MinMax],[Key6MinMax],[Key7MinMax],[Key8MinMax]
			,[BAudit],[LogFolder],[DefaultDocTitle]
			,[Key9],[Key10],[Key11],[Key12],[Key13],[Key14],[Key15],[Key16]
			,[Key9Options],[Key10Options],[Key11Options],[Key12Options],[Key13Options],[Key14Options],[Key15Options],[Key16Options]
			,[Key9MinMax],[Key10MinMax],[Key11MinMax],[Key12MinMax],[Key13MinMax],[Key14MinMax],[Key15MinMax],[Key16MinMax]
			,[Exclusions],[bNotes],[bCheckinout])
		 VALUES
			(44, 'Loss Control - Attorney Invoices', 'Loss Control - Attorney Invoices', '\\sahl-ds02\losscontrol$\attorneyinvoices',0,'?!#;:<>-_=+()*&{}[]|,~'
			, 'Loan Number', 'Email Subject', 'From Email Address', 'Invoice File Name', 'Category', 'Date Received', 'Date Processed', null
			, '00010', '00010', '00010', '00010', '00010', '00010', '00010', null
			, null, null, null, null, null, null, null, null
			, 1, '\\sahl-ds02\losscontrol$\attorneyinvoices', '{K4}'
			, null, null, null, null, null, null, null, null
			, null, null, null, null, null, null, null, null
			, null, null, null, null, null, null, null, null
			, '', 1, 0)
		
		SET IDENTITY_INSERT [dbo].[STOR] OFF
		
	End	
