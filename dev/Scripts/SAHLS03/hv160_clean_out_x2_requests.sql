USE [X2]
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[X2].[Request]') AND type in (N'U'))
BEGIN
	DROP TABLE [X2].[Request]
END
GO

CREATE TABLE [X2].[Request](
	[RequestID] [uniqueidentifier] NOT NULL,
	[Contents] [nvarchar](max) NOT NULL,
	[RequestStatusID] [int] NOT NULL,
	[RequestDate] [datetime] NOT NULL,
	[RequestUpdatedDate] [datetime] NOT NULL,
	[RequestTimeoutRetries] [int] NOT NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
