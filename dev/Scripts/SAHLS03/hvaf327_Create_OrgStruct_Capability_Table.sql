USE [2AM]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

IF NOT EXISTS(Select * From INFORMATION_SCHEMA.TABLES Where TABLE_NAME = 'Capability' And TABLE_SCHEMA = 'OrgStruct')
BEGIN
CREATE TABLE [OrgStruct].[Capability](
  [CapabilityKey] [int] IDENTITY(1,1) NOT NULL,  
  [Description] [varchar](200) NOT NULL,  
PRIMARY KEY CLUSTERED 
(
  [CapabilityKey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

END
GO

GRANT SELECT, INSERT, ALTER, UPDATE, DELETE  
  ON [2AM].[OrgStruct].[Capability] TO AppRole
GO


