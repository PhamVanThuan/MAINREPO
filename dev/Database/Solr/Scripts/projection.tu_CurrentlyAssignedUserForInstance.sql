USE EventProjection
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS (
				SELECT DISTINCT ta.Name 
				FROM EventProjection.sys.triggers t
				INNER JOIN EventProjection.sys.tables ta ON ta.object_id = t.parent_id
				WHERE ta.Name LIKE '%CurrentlyAssignedUserForInstance%'
			   )


IF OBJECT_ID('projection.tu_CurrentlyAssignedUserForInstance') is null
BEGIN 
	DECLARE @Qry VARCHAR(1024)
	SET @Qry =	'CREATE TRIGGER projection.tu_CurrentlyAssignedUserForInstance ON  projection.CurrentlyAssignedUserForInstance ' +
				'FOR UPDATE ' +
				'AS ' +
				'BEGIN ' +
				'SET NOCOUNT ON; ' +
				'END '

	EXECUTE (@Qry)
END
GO

ALTER TRIGGER projection.tu_CurrentlyAssignedUserForInstance ON  projection.CurrentlyAssignedUserForInstance
FOR UPDATE 
AS 

SET NOCOUNT ON

/*************************************************************************************************************************
		Author		:	VirekR
		CreateDate	:	2015/08/25
		Description	:	Passing the ThirdPartyInvoiceKey that was updated to service broker to enable Solr Index to be updated.
						Passed via a temp table and a proc that will determine which queue to pass to.
																																											
		
**************************************************************************************************************************/
BEGIN
		
	FROM		INSERTED i
END --END TRIGGER
GO