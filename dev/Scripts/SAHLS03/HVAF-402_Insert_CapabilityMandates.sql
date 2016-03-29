USE [2AM]
GO

IF NOT EXISTS (SELECT 1 FROM [OrgStruct].[CapabilityMandate] WHERE [CapabilityMandateKey] = 1)
BEGIN
	SET IDENTITY_INSERT [OrgStruct].[CapabilityMandate] ON
	INSERT INTO [OrgStruct].[CapabilityMandate]
		([CapabilityMandateKey], [MandateTypeKey], [CapabilityKey], [StartRange], [EndRange])
		VALUES (1, 1, 2, 0, 14999.99)
	INSERT INTO [OrgStruct].[CapabilityMandate]
		([CapabilityMandateKey], [MandateTypeKey], [CapabilityKey], [StartRange], [EndRange])
		VALUES (2, 1, 3, 15000, 29999.99)
	INSERT INTO [OrgStruct].[CapabilityMandate]
		([CapabilityMandateKey], [MandateTypeKey], [CapabilityKey], [StartRange], [EndRange])
		VALUES (3, 1, 4, 30000, 60000)
	INSERT INTO [OrgStruct].[CapabilityMandate]
		([CapabilityMandateKey], [MandateTypeKey], [CapabilityKey], [StartRange], [EndRange])
		VALUES (4, 1, 5, 60000.01, 2000000)
	SET IDENTITY_INSERT [OrgStruct].[CapabilityMandate] OFF
END
GO
