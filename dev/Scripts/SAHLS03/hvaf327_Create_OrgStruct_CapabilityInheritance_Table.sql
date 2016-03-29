USE [2AM]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

IF NOT EXISTS(Select * From INFORMATION_SCHEMA.TABLES Where TABLE_NAME = 'CapabilityInheritance' And TABLE_SCHEMA = 'OrgStruct')
BEGIN
CREATE TABLE [OrgStruct].[CapabilityInheritance](
  [CapabilityInheritanceKey] [int] IDENTITY(1,1) NOT NULL, 
  [UserOrganisationStructureCapabilityKey] [int] NOT NULL, 
  [OrganisationStructureKey] [int] NOT NULL,  
PRIMARY KEY CLUSTERED (
  [CapabilityInheritanceKey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

ALTER TABLE  [OrgStruct].[CapabilityInheritance] WITH CHECK ADD CONSTRAINT [FK_CapabilityInheritance_Inheritance] FOREIGN KEY([OrganisationStructureKey])
REFERENCES [dbo].[OrganisationStructure] ([OrganisationStructureKey])
ALTER TABLE [OrgStruct].[CapabilityInheritance] CHECK CONSTRAINT [FK_CapabilityInheritance_Inheritance]

ALTER TABLE  [OrgStruct].[CapabilityInheritance] WITH CHECK ADD CONSTRAINT [FK_CapabilityInheritance_UserOrgStructCapability] FOREIGN KEY([UserOrganisationStructureCapabilityKey])
REFERENCES [OrgStruct].[UserOrganisationStructureCapability] ([UserOrganisationStructureCapabilityKey])
ALTER TABLE [OrgStruct].[CapabilityInheritance] CHECK CONSTRAINT [FK_CapabilityInheritance_UserOrgStructCapability]

END
GO

GRANT SELECT, INSERT, ALTER, UPDATE, DELETE  
  ON [2AM].[OrgStruct].[CapabilityInheritance] TO AppRole
GO


