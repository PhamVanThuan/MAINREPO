USE [EventProjection]
GO

IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'CurrentlyAssignedUserForInstance' And TABLE_SCHEMA = 'projection')
BEGIN
	CREATE TABLE [projection].[CurrentlyAssignedUserForInstance]
	(
		[Id] [int] IDENTITY(1,1) NOT NULL,
		[LastUpdated] [datetime2](7) NOT NULL,
		[InstanceId] [bigint] NOT NULL,
		[CapabilityKey] [int] NOT NULL,
		[UserOrganisationStructureKey] [int] NOT NULL,
		[GenericKeyTypeKey] [int] NOT NULL,
		[GenericKey] [int] NOT NULL,
		[UserName] [varchar](100) NOT NULL,
		CONSTRAINT [PK_CurrentlyAssignedUserForInstance] PRIMARY KEY CLUSTERED 
		(
			[Id] ASC
		),
		CONSTRAINT [AK_CurrentlyAssignedUserForInstancer_InstanceId] UNIQUE NONCLUSTERED 
		(
			[InstanceId] ASC
		)
	)
	
	GRANT INSERT ON Object::[projection].[CurrentlyAssignedUserForInstance] TO [ServiceArchitect]

	GRANT SELECT ON Object::[projection].[CurrentlyAssignedUserForInstance] TO [ServiceArchitect]

	GRANT UPDATE ON Object::[projection].[CurrentlyAssignedUserForInstance] TO [ServiceArchitect]

	GRANT DELETE ON Object::[projection].[CurrentlyAssignedUserForInstance] TO [ServiceArchitect]

END


