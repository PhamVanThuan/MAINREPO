USE [EventStore]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER TRIGGER [event].[OnEventInsert]
	ON [event].[Event]
	FOR INSERT
AS 
BEGIN

	DECLARE @MessageBody NVARCHAR(MAX);

	DECLARE insertedCorsor CURSOR FOR
	SELECT [EventKey]
	FROM INSERTED
	WHERE [EventTypeKey] in (23,24,25)
				
    OPEN insertedCorsor
    FETCH NEXT FROM insertedCorsor INTO @MessageBody 
    WHILE @@FETCH_STATUS = 0
    BEGIN
	
		SET @MessageBody =(SELECT @MessageBody 'EventKey' FOR XML PATH)
		IF (@MessageBody IS NOT NULL)
		BEGIN

			BEGIN TRANSACTION;

			DECLARE @Dialog_Handle UNIQUEIDENTIFIER
		
			BEGIN DIALOG CONVERSATION @Dialog_Handle
				FROM SERVICE [EventPublisherServiceInitiator]
				TO SERVICE 'EventPublisherServiceTarget'
				ON CONTRACT [ProcessEventPublisherMessage]
				WITH ENCRYPTION = OFF;

			-- Send the message to the TargetService
			;SEND ON CONVERSATION @Dialog_Handle
			MESSAGE TYPE [EventPublisherMessage](@MessageBody);
			
			COMMIT;

		END
		FETCH NEXT FROM insertedCorsor INTO @MessageBody
    END
    CLOSE insertedCorsor
    DEALLOCATE insertedCorsor
END
