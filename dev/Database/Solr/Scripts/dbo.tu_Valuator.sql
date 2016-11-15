USE [2am]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

--IF NOT EXISTS (
--				SELECT DISTINCT ta.Name 
--				FROM [2am].sys.triggers t
--				INNER JOIN [2am].sys.tables ta ON ta.object_id = t.parent_id
--				WHERE ta.Name LIKE '%Valuator%'
--			   )


IF OBJECT_ID('dbo.tu_Valuator') is null
BEGIN 
	DECLARE @Qry VARCHAR(1024)
	SET @Qry =	'CREATE TRIGGER dbo.tu_Valuator ON  dbo.Valuator ' +
				'FOR UPDATE ' +
				'AS ' +
				'BEGIN ' +
				'SET NOCOUNT ON; ' +
				'END '

	EXECUTE (@Qry)
END
GO

ALTER TRIGGER dbo.tu_Valuator ON  dbo.Valuator
FOR UPDATE 
AS 

/*************************************************************************************************************************
		Author		:	VirekR
		CreateDate	:	2015/03/25
		Description	:	Passing the legalentitykey that was updated to service broker to enable
						Solr Index to be updated.
						Passed via a temp table and a proc that will determine which queue to pass to. 

		
		History		:
																																											
		
**************************************************************************************************************************/
SET NOCOUNT ON

BEGIN
	/* Lets get the LegalEntityKey and pass it to service broker to initiate a conversation */	
	FROM		INSERTED i  	   	 
GO