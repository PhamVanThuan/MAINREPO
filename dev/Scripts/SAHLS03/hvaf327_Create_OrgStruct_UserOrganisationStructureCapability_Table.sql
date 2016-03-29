USE [2AM]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

IF NOT EXISTS(Select * From INFORMATION_SCHEMA.TABLES Where TABLE_NAME = 'UserOrganisationStructureCapability' And TABLE_SCHEMA = 'OrgStruct')
BEGIN
CREATE TABLE [OrgStruct].[UserOrganisationStructureCapability](
  [UserOrganisationStructureCapabilityKey] [int] IDENTITY(1,1) NOT NULL, 
  [UserOrganisationStructureKey] [int] NOT NULL, 
  [CapabilityKey] [int] NOT NULL,  
PRIMARY KEY CLUSTERED (
  [UserOrganisationStructureCapabilityKey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

ALTER TABLE  [OrgStruct].[UserOrganisationStructureCapability] WITH CHECK ADD CONSTRAINT [FK_UserOrgStructCapability_OrgStruct] FOREIGN KEY([UserOrganisationStructureKey])
REFERENCES [dbo].[UserOrganisationStructure] ([UserOrganisationStructureKey])
ALTER TABLE  [OrgStruct].[UserOrganisationStructureCapability] CHECK CONSTRAINT [FK_UserOrgStructCapability_OrgStruct]

ALTER TABLE  [OrgStruct].[UserOrganisationStructureCapability] WITH CHECK ADD CONSTRAINT [FK_UserOrgStructCapability_Capability] FOREIGN KEY([CapabilityKey])
REFERENCES [OrgStruct].[Capability] ([CapabilityKey])
ALTER TABLE [OrgStruct].[UserOrganisationStructureCapability] CHECK CONSTRAINT [FK_UserOrgStructCapability_Capability]

END
GO

GRANT SELECT, INSERT, ALTER, UPDATE, DELETE  
  ON [2AM].[OrgStruct].[UserOrganisationStructureCapability] TO AppRole
GO


