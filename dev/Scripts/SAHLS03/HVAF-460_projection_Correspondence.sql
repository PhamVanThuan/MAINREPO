USE [EventProjection]
GO

/****** Object:  Table [projection].[Correspondence]    Script Date: 27/08/2015 12:47:37 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Correspondence' And TABLE_SCHEMA = 'projection')
	BEGIN

	CREATE TABLE [projection].[Correspondence](
		[Id] [int] IDENTITY(1,1) NOT NULL,
		[CorrespondenceType] varchar(100) NOT NULL,
		[CorrespondenceReason] varchar(100) NOT NULL,
		[CorrespondenceMedium] varchar(100) NOT NULL,
		[Date] [datetime2] NOT NULL,
		[UserName] varchar(100) NOT NULL,
		[MemoText] varchar(max),
		[GenericKey] int NOT NULL,
		[GenericKeyTypeKey] int NOT NULL
		CONSTRAINT [PK_Correspondence] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	)

	END

GO

SET ANSI_PADDING OFF
GO


GRANT INSERT ON Object::[projection].[Correspondence] TO [ServiceArchitect]

GRANT SELECT ON Object::[projection].[Correspondence] TO [ServiceArchitect]

GRANT UPDATE ON Object::[projection].[Correspondence] TO [ServiceArchitect]

GRANT DELETE ON Object::[projection].[Correspondence] TO [ServiceArchitect]

GO

