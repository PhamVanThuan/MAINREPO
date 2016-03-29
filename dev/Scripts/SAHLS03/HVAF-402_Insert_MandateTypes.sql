USE [2AM]
GO

IF NOT EXISTS (SELECT 1 FROM [OrgStruct].[MandateType] WHERE [MandateTypeKey] = 1)
BEGIN
	SET IDENTITY_INSERT [OrgStruct].[MandateType] ON
	INSERT INTO [OrgStruct].[MandateType]
		([MandateTypeKey], [Description]) VALUES (1, 'Range')
	SET IDENTITY_INSERT [OrgStruct].[MandateType] OFF
END
GO
