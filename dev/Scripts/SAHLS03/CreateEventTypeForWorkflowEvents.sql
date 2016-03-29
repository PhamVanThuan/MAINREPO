USE [EventStore]
GO

IF NOT EXISTS (SELECT 1 FROM [EventStore].[event].[EventType] WHERE [Name]= 'Workflow Event')

BEGIN

	INSERT INTO [event].[EventType]
           ([EventTypeKey]
           ,[Name]
           ,[ClassName]
           ,[DomainKey]
           ,[Version])
     VALUES
           ( 25
           , 'Workflow Event'
           , NULL
           , 0
           , 1)
END
