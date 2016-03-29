USE [EventProjection]
GO

/****** Object:  Table [projection].[CurrentStateForInstance]    Script Date: 27/08/2015 12:47:37 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'CurrentStateForInstance' And TABLE_SCHEMA = 'projection')
BEGIN

	CREATE TABLE [projection].[CurrentStateForInstance](
		[Id] [int] IDENTITY(1,1) NOT NULL,
		[StateChangeDate] [datetime2](7) NOT NULL,
		[InstanceId] [bigint] NOT NULL,
		[StateName] [varchar](100) NOT NULL,
		[WorkflowName] [varchar](100) NOT NULL,
		[GenericKeyTypeKey] [int] NOT NULL,
		[GenericKey] [int] NOT NULL
	 CONSTRAINT [PK_CurrentStateForInstance] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
	 CONSTRAINT [AK_CurrentStateForInstance_InstanceId] UNIQUE NONCLUSTERED 
	(
		[InstanceId] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]


	END

GO

SET ANSI_PADDING OFF
GO


IF NOT EXISTS(SELECT * FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'CurrentStateForInstance' AND COLUMN_NAME = 'DaysInState') 

BEGIN

	ALTER TABLE [projection].[CurrentStateForInstance]  ADD DaysInState AS datediff(dd, StateChangeDate, getdate())

END

GO

GRANT INSERT ON Object::[projection].[CurrentStateForInstance] TO [ServiceArchitect]

GRANT SELECT ON Object::[projection].[CurrentStateForInstance] TO [ServiceArchitect]

GRANT UPDATE ON Object::[projection].[CurrentStateForInstance] TO [ServiceArchitect]

GRANT DELETE ON Object::[projection].[CurrentStateForInstance] TO [ServiceArchitect]

GO

