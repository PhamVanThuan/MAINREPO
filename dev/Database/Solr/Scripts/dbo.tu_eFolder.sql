USE [e-work]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

--IF NOT EXISTS (
--				SELECT DISTINCT ta.Name 
--				FROM [2am].sys.triggers t
--				INNER JOIN [2am].sys.tables ta ON ta.object_id = t.parent_id
--				WHERE ta.Name LIKE '%eFolder%'
--			   )


IF OBJECT_ID('dbo.tu_eFolder') is null
BEGIN 
	DECLARE @Qry VARCHAR(1024)
	SET @Qry =	'CREATE TRIGGER dbo.tu_eFolder ON  dbo.eFolder ' +
				'FOR UPDATE ' +
				'AS ' +
				'BEGIN ' +
				'SET NOCOUNT ON; ' +
				'END '

	EXECUTE (@Qry)
END
GO

ALTER TRIGGER dbo.tu_eFolder ON  dbo.eFolder
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
	WHERE		i.eMapName = 'LossControl'
	AND			ISNUMERIC(coalesce(i.eFolderName, '''')) = 1

END --END TRIGGER
GO