
USE [2AM]
Go


if not exists(Select * From INFORMATION_SCHEMA.TABLES Where TABLE_NAME = 'ThirdParty' And TABLE_SCHEMA = 'dbo')
begin
	
	Create Table [dbo].[ThirdParty]
	(
		Id uniqueidentifier not null,
		ThirdPartyKey int Identity(1,1) not null,
		ThirdPartyTypeId uniqueidentifier not null,
		LegalEntityKey int not null,
		IsPanel bit not null,
		GeneralStatusKey int not null,
		GenericKey int,
		CONSTRAINT [PK_ThirdParty] PRIMARY KEY CLUSTERED 
		(
			[Id] ASC
		)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]

	)

end
go

if not exists(Select * From INFORMATION_SCHEMA.TABLE_CONSTRAINTS Where CONSTRAINT_NAME = 'FK_ThirdParty_LegalEntity' And TABLE_NAME = 'ThirdParty' And TABLE_SCHEMA = 'dbo')
begin

	ALTER TABLE [dbo].[ThirdParty]  WITH NOCHECK ADD  CONSTRAINT [FK_ThirdParty_LegalEntity] FOREIGN KEY([LegalEntityKey])
	REFERENCES [dbo].[LegalEntity] ([LegalEntityKey])


	ALTER TABLE [dbo].[ThirdParty] CHECK CONSTRAINT [FK_ThirdParty_LegalEntity]
end
GO

if not exists(Select * From INFORMATION_SCHEMA.TABLE_CONSTRAINTS Where CONSTRAINT_NAME = 'FK_ThirdParty_ThirdPartyType' And TABLE_NAME = 'ThirdParty' And TABLE_SCHEMA = 'dbo')
begin

	ALTER TABLE [dbo].[ThirdParty]  WITH NOCHECK ADD  CONSTRAINT [FK_ThirdParty_ThirdPartyType] FOREIGN KEY([ThirdPartyTypeId])
	REFERENCES [dbo].[ThirdPartyType] ([Id])

	ALTER TABLE [dbo].[ThirdParty] CHECK CONSTRAINT [FK_ThirdParty_ThirdPartyType]
end
GO

if not exists(Select * From INFORMATION_SCHEMA.TABLE_CONSTRAINTS Where CONSTRAINT_NAME = 'FK_ThirdParty_GeneralStatus' And TABLE_NAME = 'ThirdParty' And TABLE_SCHEMA = 'dbo')
begin

	ALTER TABLE [dbo].[ThirdParty]  WITH NOCHECK ADD  CONSTRAINT [FK_ThirdParty_GeneralStatus] FOREIGN KEY([GeneralStatusKey])
	REFERENCES [dbo].[GeneralStatus] ([GeneralStatusKey])

	ALTER TABLE [dbo].[ThirdParty] CHECK CONSTRAINT [FK_ThirdParty_GeneralStatus]
end
GO

GRANT SELECT ON [dbo].[ThirdParty] TO [AppRole] AS [dbo]
GO
GRANT INSERT ON [dbo].[ThirdParty] TO [AppRole] AS [dbo]
GO
GRANT UPDATE ON [dbo].[ThirdParty] TO [AppRole] AS [dbo]
GO
