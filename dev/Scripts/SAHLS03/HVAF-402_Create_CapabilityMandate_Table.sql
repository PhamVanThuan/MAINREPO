USE [2AM]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

IF NOT EXISTS(Select * From INFORMATION_SCHEMA.TABLES Where TABLE_NAME = 'CapabilityMandate' And TABLE_SCHEMA = 'OrgStruct')
BEGIN
CREATE TABLE [OrgStruct].[CapabilityMandate](
  [CapabilityMandateKey] [int] IDENTITY(1,1) NOT NULL,  
  [MandateTypeKey] [int] NOT NULL,  
  [CapabilityKey] [int] NOT NULL,
  [StartRange] decimal(22,10) NULL,  
  [EndRange] decimal (22,10) NULL,  
PRIMARY KEY CLUSTERED 
(
  [CapabilityMandateKey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]


ALTER TABLE [OrgStruct].[CapabilityMandate]  WITH CHECK ADD  CONSTRAINT [FK_CapabilityMandate_MandateType] FOREIGN KEY([MandateTypeKey])
REFERENCES [OrgStruct].[MandateType] ([MandateTypeKey])
ALTER TABLE [OrgStruct].[CapabilityMandate] CHECK CONSTRAINT [FK_CapabilityMandate_MandateType]

ALTER TABLE [OrgStruct].[CapabilityMandate]  WITH CHECK ADD CONSTRAINT [FK_CapabilityMandate_Capability] FOREIGN KEY([CapabilityKey])
REFERENCES [OrgStruct].[Capability] ([CapabilityKey])
ALTER TABLE [OrgStruct].[CapabilityMandate] CHECK CONSTRAINT [FK_CapabilityMandate_Capability]
END
GO

GRANT SELECT, INSERT, ALTER, UPDATE, DELETE  
  ON [2AM].[OrgStruct].[CapabilityMandate] TO AppRole
GO


