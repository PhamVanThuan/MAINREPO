USE [x2]
GO

IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'tag' AND  TABLE_NAME = 'WorkflowItemUserTags')
BEGIN
	DROP TABLE [tag].[WorkflowItemUserTags]
END

IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'tag' AND  TABLE_NAME = 'UserTags')
BEGIN
	DROP TABLE [tag].[UserTags]
END

IF (EXISTS (SELECT * FROM sys.schemas WHERE name = 'tag'))
BEGIN
	EXEC ('DROP SCHEMA [tag]')
END

USE [2am]
GO

IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'tag' AND  TABLE_NAME = 'WorkflowItemUserTags')
BEGIN
	DROP TABLE [tag].[WorkflowItemUserTags]
END

IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'tag' AND  TABLE_NAME = 'UserTags')
BEGIN
	DROP TABLE [tag].[UserTags]
END

IF (NOT EXISTS (SELECT * FROM sys.schemas WHERE name = 'tag')) 
BEGIN
    EXEC ('CREATE SCHEMA [tag] AUTHORIZATION [dbo]')
END

CREATE TABLE [tag].[UserTags](
	[Id] [uniqueidentifier] NOT NULL,
	[caption] [varchar](100) NOT NULL,
	[ADUsername] [varchar](255) NOT NULL,
	[BackColour] [varchar](11) NULL,
	[ForeColour] [varchar](11) NULL,
	[CreateDate] [datetime] NOT NULL DEFAULT (getdate()),
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [UT_User_TagCaption] UNIQUE NONCLUSTERED 
(
	[caption] ASC,
	[ADUsername] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

CREATE TABLE [tag].[WorkflowItemUserTags](
	[ItemKey] [int] IDENTITY(1,1) NOT NULL,
	[WorkFlowItemId] [bigint] NOT NULL,
	[ADUsername] [varchar](255) NOT NULL,
	[TagId] [uniqueidentifier] NOT NULL,
	[CreateDate] [datetime] NOT NULL DEFAULT (getdate()),
PRIMARY KEY CLUSTERED 
(
	[ItemKey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [tag].[WorkflowItemUserTags]  WITH CHECK ADD FOREIGN KEY([TagId])
REFERENCES [tag].[UserTags] ([Id])
GO

GRANT SELECT ON [tag].[UserTags] TO [ServiceArchitect]
GRANT INSERT ON [tag].[UserTags] TO [ServiceArchitect]
GRANT UPDATE ON [tag].[UserTags] TO [ServiceArchitect]
GRANT DELETE ON [tag].[UserTags] TO [ServiceArchitect]
GO

GRANT SELECT ON [tag].[WorkflowItemUserTags] TO [ServiceArchitect]
GRANT INSERT ON [tag].[WorkflowItemUserTags] TO [ServiceArchitect]
GRANT UPDATE ON [tag].[WorkflowItemUserTags] TO [ServiceArchitect]
GRANT DELETE ON [tag].[WorkflowItemUserTags] TO [ServiceArchitect]
GO