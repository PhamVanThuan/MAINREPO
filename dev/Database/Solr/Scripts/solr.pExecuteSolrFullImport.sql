USE [Process]
GO
/****** Object:  StoredProcedure [solr].[pExecuteSolrFullImport]    Script Date: 2014-12-24 11:10:47 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF OBJECT_ID('solr.pExecuteSolrFullImport') is null
BEGIN 
	DECLARE @Qry VARCHAR(1024)
	SET @Qry =	'CREATE PROCEDURE solr.pExecuteSolrFullImport ' +
				'AS ' +
				'BEGIN ' +
				'SET NOCOUNT ON; ' +
				'END '

	EXECUTE (@Qry)
END
GO

ALTER PROCEDURE solr.pExecuteSolrFullImport @Msg VARCHAR(1024) OUTPUT
AS

/*************************************************************************************************************************
		Author		:	VirekR
		CreateDate	:	2015/05/18
		Description	:	This proc will:
										1.	Kick off the datasets required for each index
										2.	Make a call to a CLR to import the entire dataset from the solr tables populated
											to Solr
										3.	Optimise the indexes on Solr
										4.	Rebuild indexes on the tables that have non clustered indexes
										5.	Procs should be named in accordance to the others (start with 'p' and suffix with 'Extract' 
											eg. solr.p@ProcNameExtract
										6.	Table should be named in accordance to the others eg. solr.@TableName
																				
		History:
					
	--------------------------------------------------------------------	
	--Helper Code:	
			DECLARE @Msg VARCHAR(1024)			
			EXEC Process.solr.pExecuteSolrFullImport @Msg OUTPUT
			SELECT @Msg
	--------------------------------------------------------------------
	**************************************************************************************************************************/

BEGIN
	BEGIN TRY
			DECLARE @count			INT = 1,
					@Proc			VARCHAR(100),
					@alter			VARCHAR(200),
					@TableName		VARCHAR(100),
					@IndexName		VARCHAR(50),
					@ImportRequest	VARCHAR(300)
					--,@Msg VARCHAR(1024)

			
			/********************** POPULATE SOLR TABLE RELATED INFORMATION *************************/
			IF OBJECT_ID('tempdb.dbo.#Index') IS NOT NULL
					DROP TABLE #Index

			CREATE TABLE #Index 
			(
				ID				INT IDENTITY(1,1),
				SolrIndexName	VARCHAR(50),
				TableName		VARCHAR(50),
				TableIndexName	VARCHAR(50),
				ProcName		VARCHAR(100),
				ImportRequest	VARCHAR(300) --	Import Type (full-import or delta-import) --	Optimise or not	
			)

			/************* PICKING UP ALL TABLES THAT ARE UNDER THE SOLR SCHEMA + AND TABLE INDEXES ************/
			INSERT INTO #Index (SolrIndexName, TableName, TableIndexName, ImportRequest)
			SELECT		t.name, s.name + '.' + t.name, i.Name, 'full-import&clean=true&optimize=true'
			FROM		[2am].sys.tables t
			INNER JOIN	[2am].sys.schemas s ON s.schema_id = t.schema_id
			LEFT JOIN	[2am].sys.indexes i on i.object_id = t.object_id
						AND i.type = 2 -- return non clustered
			WHERE		s.Name = 'solr'

			/************* MATCH UP THE PROC NAME TO THE TABLE *****************************/
			UPDATE	i
			SET		ProcName = p.SchemaProcName
			--SELECT i.*,p.SchemaProcName
			FROM	#Index i
			INNER JOIN (
							SELECT	'process.'+ s.Name + '.' + p.Name AS SchemaProcName, p.Name AS pName
							FROM	process.sys.procedures p
							INNER JOIN process.sys.schemas s ON s.schema_id = p.schema_id
							WHERE	s.Name = 'solr'
							AND		p.Name LIKE '%Extract' --suffix with the word 'Extract'
						)p ON pName LIKE '%'+i.SolrIndexName+'%' 

			--	SELECT * FROM #Index i

			/********** DO A FULL IMPORT AND OPTIMISE THE SOLR INDEXES ********/
			SELECT @ImportRequest = ImportRequest FROM #Index
								
			WHILE @Count <= (SELECT MAX(ID) FROM #Index)
			BEGIN	
	
				SELECT	@Proc		= ProcName,
						@TableName	= TableName,
						@IndexName	= SolrIndexName
				FROM	#Index 
				WHERE	ID = @count
	
				--SELECT	 @Proc as 'proc',@TableName as 'table',@IndexName as 'index'

				/*********** EXECUTE THE PROC WHICH WILL POPULATE THE SOLR TABLES *********/
								EXEC @Proc @Msg OUTPUT
				/*************************************************************************/

				-- Ignore if tables dont have indexes
				IF ( SELECT TableIndexName FROM #Index WHERE ID = @count) IS NOT NULL
					BEGIN
						SELECT @alter = 'ALTER INDEX ALL ON [2am].'+ @TableName + ' REBUILD'
						
						-- REBUILD THE INDEXES
						EXEC (@alter)
					END -- END IF

				-- Kick off CLR which will kick off full import on Solr for that specific index
				EXEC	[2am].solr.SolrFullImportWebRequest	@IndexName, @ImportRequest	
	
				SELECT @Count = @Count + 1
			END -- END WHILE
									
	END TRY

		BEGIN CATCH
	
			SET @Msg = 'solr.pExecuteSolrFullImport: ' + ISNULL(ERROR_MESSAGE(), 'Failed!')
				RAISERROR(@Msg,16,1)

			SELECT TOP 0 * INTO #Errors FROM process.template.errors
	
			DELETE FROM #Errors
			INSERT INTO #Errors (ErrorCodeKey, DateOfError, MSG, SeverityTypeKey)
			SELECT (SELECT ErrorCodeKey FROM process.errorhandling.ErrorCode (NOLOCK) WHERE Description LIKE 'Solr Extract Failure'), GETDATE(), @Msg, 1
	
			EXEC process.errorhandling.pLogErrors @Msg OUTPUT
				
		END CATCH


		/*
		--Helper Code:	
			DECLARE @Msg VARCHAR(1024)			
			EXEC Process.solr.pExecuteSolrFullImport @Msg OUTPUT
			SELECT @Msg
		*/
END
GO

PRINT 'solr.pExecuteSolrFullImport deployed: ' + cast(getdate() as varchar) + ' to server: '+ @@Servername
GO

GRANT EXECUTE ON OBJECT::solr.pExecuteSolrFullImport TO [Batch];
GRANT EXECUTE ON OBJECT::solr.pExecuteSolrFullImport TO [ProcessRole];
GRANT EXECUTE ON OBJECT::solr.pExecuteSolrFullImport TO [AppRole];
