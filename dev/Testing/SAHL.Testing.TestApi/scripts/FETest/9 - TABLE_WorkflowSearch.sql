USE [FETest]
GO

IF(EXISTS(SELECT 1 FROM SYS.sysobjects where name = 'WorkflowSearch' and type = 'U'))
	BEGIN
		DROP TABLE [FETest].[dbo].[WorkflowSearch]
	end

GO

CREATE TABLE [dbo].[WorkflowSearch](
	[Id] INT IDENTITY(1,1),
	[InstanceID] [int] NOT NULL,
	[Subject] [VARCHAR](MAX) NOT NULL,
	[GenericKey] [int] NOT NULL,
	[State] [VARCHAR](MAX) NOT NULL,
	[Workflow] [VARCHAR](MAX) NOT NULL
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO