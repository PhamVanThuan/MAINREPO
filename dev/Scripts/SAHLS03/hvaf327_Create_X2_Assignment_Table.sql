USE [X2]
GO

/****** Object:  Table [dbo].[[X2]].X2.Assignment]    Script Date: 2015-06-19 09:42:27 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
if not exists(Select * From INFORMATION_SCHEMA.TABLES Where TABLE_NAME = 'Assignment' And TABLE_SCHEMA = 'X2')
Begin
CREATE TABLE [X2].Assignment(
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[InstanceId] [bigint] NOT NULL,
	[AssignmentDate] [datetime] NOT NULL,
	[UserOrganisationStructureKey] [int] NOT NULL,
	[CapabilityKey] [int] NOT NULL,
 CONSTRAINT [PK_Assignment] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

ALTER TABLE [X2].[Assignment] WITH CHECK ADD CONSTRAINT [FK_Assignment_Instance] FOREIGN KEY([InstanceId])
REFERENCES [X2].[Instance] ([ID])
ALTER TABLE  [X2].[Assignment] CHECK CONSTRAINT [FK_Assignment_Instance]

End
Go

GRANT SELECT ON [X2].[X2].[Assignment] TO AppRole
GO
GRANT SELECT, INSERT, ALTER, UPDATE, DELETE ON [X2].[X2].[Assignment] TO X2User
Go

SET ANSI_PADDING OFF
GO

