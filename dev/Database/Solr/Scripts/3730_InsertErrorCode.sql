USE Process
GO
/*************************************************************************************************************************
		Author		:	VirekR
		CreateDate	:	2015/05/11
		Description	:	Insert new errorcode for solr Extracts
															
**************************************************************************************************************************/

IF NOT EXISTS (
				SELECT	ErrorCodeKey
				FROM	process.ErrorHandling.ErrorCode (NOLOCK)
				WHERE	Description LIKE 'Solr Extract Failure'
			   )
BEGIN 
		INSERT INTO Process.errorhandling.ErrorCode
		SELECT max(errorcodekey)+1,'Solr Extract Failure'
		FROM Process.errorhandling.ErrorCode
END
GO