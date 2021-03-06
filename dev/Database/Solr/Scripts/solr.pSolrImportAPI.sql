USE [Process]
GO
/****** Object:  StoredProcedure [solr].[pSolrImportAPI]    Script Date: 2014-12-24 11:10:47 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF OBJECT_ID('solr.pSolrImportAPI') is null
BEGIN 
	DECLARE @Qry VARCHAR(1024)
	SET @Qry =	'CREATE PROCEDURE solr.pSolrImportAPI ' +
				'AS ' +
				'BEGIN ' +
				'SET NOCOUNT ON; ' +
				'END '

	EXECUTE (@Qry)
END
GO

ALTER PROCEDURE solr.pSolrImportAPI @IndexType VARCHAR(100)
AS

/*************************************************************************************************************************
		Author		:	VirekR
		CreateDate	:	2014/08/08
		Description	:	This proc will be consumed by solr application to return data for the index
																				
		History:
					
		EXEC process.solr.pSolrImportAPI 'Client'
	**************************************************************************************************************************/

BEGIN
			DECLARE @Select		VARCHAR(200)
				  -- ,@IndexType	VARCHAR(100) = 'ThirdParty'

/********************** BUILD THE SELECT STATEMENT *************************/
			SELECT @Select = 'SELECT * FROM [2am].'+ s.name + '.' + t.name + ' (NOLOCK)' 
			FROM		[2am].sys.tables t
			INNER JOIN	[2am].sys.schemas s ON s.schema_id = t.schema_id			
			WHERE		s.Name = 'solr'
			AND			t.Name = @IndexType
			--SELECT @Select

		/******** EXECUTE SELECT STATEMENT ************/	
					EXEC (@Select)
		/*********************************************/
END

GO

PRINT 'solr.pSolrImportAPI deployed: ' + cast(getdate() as varchar) + ' to server: '+ @@Servername
GO

GRANT EXECUTE ON OBJECT::solr.pSolrImportAPI TO [Batch];
GRANT EXECUTE ON OBJECT::solr.pSolrImportAPI TO [ProcessRole];
GRANT EXECUTE ON OBJECT::solr.pSolrImportAPI TO [AppRole];
