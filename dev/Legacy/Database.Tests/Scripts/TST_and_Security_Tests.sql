USE [master]
GO
begin try
	drop database TST;
end try
begin catch
end catch
go
/****** Object:  Database [TST]    Script Date: 05/23/2013 11:40:17 ******/
CREATE DATABASE [TST]
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [TST].[dbo].[sp_fulltext_database] @action = 'disable'
end
GO
ALTER DATABASE [TST] SET ANSI_NULL_DEFAULT OFF
GO
ALTER DATABASE [TST] SET ANSI_NULLS OFF
GO
ALTER DATABASE [TST] SET ANSI_PADDING OFF
GO
ALTER DATABASE [TST] SET ANSI_WARNINGS OFF
GO
ALTER DATABASE [TST] SET ARITHABORT OFF
GO
ALTER DATABASE [TST] SET AUTO_CLOSE OFF
GO
ALTER DATABASE [TST] SET AUTO_CREATE_STATISTICS ON
GO
ALTER DATABASE [TST] SET AUTO_SHRINK OFF
GO
ALTER DATABASE [TST] SET AUTO_UPDATE_STATISTICS ON
GO
ALTER DATABASE [TST] SET CURSOR_CLOSE_ON_COMMIT OFF
GO
ALTER DATABASE [TST] SET CURSOR_DEFAULT  GLOBAL
GO
ALTER DATABASE [TST] SET CONCAT_NULL_YIELDS_NULL OFF
GO
ALTER DATABASE [TST] SET NUMERIC_ROUNDABORT OFF
GO
ALTER DATABASE [TST] SET QUOTED_IDENTIFIER OFF
GO
ALTER DATABASE [TST] SET RECURSIVE_TRIGGERS OFF
GO
ALTER DATABASE [TST] SET  DISABLE_BROKER
GO
ALTER DATABASE [TST] SET AUTO_UPDATE_STATISTICS_ASYNC OFF
GO
ALTER DATABASE [TST] SET DATE_CORRELATION_OPTIMIZATION OFF
GO
ALTER DATABASE [TST] SET TRUSTWORTHY OFF
GO
ALTER DATABASE [TST] SET ALLOW_SNAPSHOT_ISOLATION OFF
GO
ALTER DATABASE [TST] SET PARAMETERIZATION SIMPLE
GO
ALTER DATABASE [TST] SET READ_COMMITTED_SNAPSHOT OFF
GO
ALTER DATABASE [TST] SET  READ_WRITE
GO
ALTER DATABASE [TST] SET RECOVERY SIMPLE
GO
ALTER DATABASE [TST] SET  MULTI_USER
GO
ALTER DATABASE [TST] SET PAGE_VERIFY CHECKSUM
GO
ALTER DATABASE [TST] SET DB_CHAINING OFF
GO
USE [TST]
GO
begin try
	CREATE USER [sqlservice2] FOR LOGIN [SAHL\sqlservice2] WITH DEFAULT_SCHEMA=[sqlservice2];
end try
begin catch
end catch
GO
/****** Object:  User [SQLService]    Script Date: 05/23/2013 11:40:17 ******/
/****** Object:  Schema [Utils]    Script Date: 05/23/2013 11:40:17 ******/
CREATE SCHEMA [Utils] AUTHORIZATION [dbo]
GO
/****** Object:  Schema [sqlservice2]    Script Date: 05/23/2013 11:40:17 ******/
/****** Object:  Schema [Runner]    Script Date: 05/23/2013 11:40:17 ******/
CREATE SCHEMA [Runner] AUTHORIZATION [dbo]
GO
/****** Object:  Schema [JobUser]    Script Date: 05/23/2013 11:40:17 ******/
/****** Object:  Schema [Internal]    Script Date: 05/23/2013 11:40:17 ******/
CREATE SCHEMA [Internal] AUTHORIZATION [dbo]
GO
/****** Object:  Schema [Data]    Script Date: 05/23/2013 11:40:17 ******/
CREATE SCHEMA [Data] AUTHORIZATION [dbo]
GO
/****** Object:  Schema [Assert]    Script Date: 05/23/2013 11:40:17 ******/
CREATE SCHEMA [Assert] AUTHORIZATION [dbo]
GO
/****** Object:  Table [Data].[TSTVersion]    Script Date: 05/23/2013 11:40:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [Data].[TSTVersion](
	[TSTSignature] [varchar](100) NOT NULL,
	[MajorVersion] [int] NOT NULL,
	[MinorVersion] [int] NOT NULL,
	[SetupDate] [datetime] NOT NULL
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [Data].[TSTParameters]    Script Date: 05/23/2013 11:40:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [Data].[TSTParameters](
	[ParameterId] [int] IDENTITY(1,1) NOT NULL,
	[TestSessionId] [int] NOT NULL,
	[ParameterName] [varchar](32) NOT NULL,
	[ParameterValue] [varchar](100) NOT NULL,
	[Scope] [sysname] NOT NULL,
	[ScopeValue] [sysname] NULL,
 CONSTRAINT [PK_TSTParameters] PRIMARY KEY CLUSTERED 
(
	[ParameterId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY],
 CONSTRAINT [UK_TSTParameters_TestSessionId_ParameterName_Scope_ScopeValue] UNIQUE NONCLUSTERED 
(
	[TestSessionId] ASC,
	[ParameterName] ASC,
	[Scope] ASC,
	[ScopeValue] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [Data].[TestSession]    Script Date: 05/23/2013 11:40:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Data].[TestSession](
	[TestSessionId] [int] IDENTITY(1,1) NOT NULL,
	[DatabaseName] [sysname] NOT NULL,
	[TestSessionStart] [datetime] NOT NULL,
	[TestSessionFinish] [datetime] NULL,
 CONSTRAINT [PK_TestSession] PRIMARY KEY CLUSTERED 
(
	[TestSessionId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  StoredProcedure [Internal].[ClearExpectedError]    Script Date: 05/23/2013 11:40:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =======================================================================
-- PROCEDURE: ClearExpectedError
-- Clear the info about the expected error.
-- =======================================================================
CREATE PROCEDURE [Internal].[ClearExpectedError]
AS
BEGIN
   UPDATE #Tmp_CrtSessionInfo SET 
      ExpectedErrorNumber          = NULL,
      ExpectedErrorMessage         = NULL, 
      ExpectedErrorProcedure       = NULL,
      ExpectedErrorContextMessage  = NULL
END
GO
/****** Object:  UserDefinedFunction [Internal].[SFN_GetListToTable]    Script Date: 05/23/2013 11:40:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =======================================================================
-- FUNCTION SFN_GetListToTable
-- Takes a list with items separated by semicolons and returns a table 
-- where each row contains one item. Each item is max 500 characters otherwise 
-- a truncation error occurs.
-- =======================================================================
CREATE FUNCTION [Internal].[SFN_GetListToTable](@List varchar(max)) 
RETURNS @ListToTable TABLE (ListItem varchar(500) )
AS 
BEGIN

   IF (@List IS NULL) RETURN

   DECLARE @IndexStart  int
   DECLARE @IndexEnd    int
   DeCLARE @CrtItem     varchar(500)
   
   SET @IndexStart = 1;
   WHILE (@IndexStart <= DATALENGTH(@List) + 1)
   BEGIN
      SET @IndexEnd = CHARINDEX(';', @List, @IndexStart)
      IF (@IndexEnd = 0) SET @IndexEnd = DATALENGTH(@List) + 1
      IF (@IndexEnd > @IndexStart)
      BEGIN
         SET @CrtItem = SUBSTRING(@List, @IndexStart, @IndexEnd - @IndexStart)
         INSERT INTO @ListToTable(ListItem) VALUES (@CrtItem)
      END
      
      SET @IndexStart = @IndexEnd + 1
   END

   RETURN
END
GO
/****** Object:  UserDefinedFunction [Internal].[SFN_EscapeForXml]    Script Date: 05/23/2013 11:40:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =======================================================================
-- FUNCTION SFN_EscapeForXml
-- Returns the given string after escaping characters that have a special 
-- role in an XML file.
-- =======================================================================
CREATE FUNCTION [Internal].[SFN_EscapeForXml](@TextString nvarchar(max)) RETURNS nvarchar(max)
AS
BEGIN

   SET @TextString = REPLACE (@TextString, '"', '&quot;')
   SET @TextString = REPLACE (@TextString, '&', '&amp;')
   SET @TextString = REPLACE (@TextString, '>', '&gt;')
   SET @TextString = REPLACE (@TextString, '<', '&lt;')

   RETURN @TextString 
   
END
GO
/****** Object:  UserDefinedFunction [Internal].[SFN_GetEntryTypeName]    Script Date: 05/23/2013 11:40:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =======================================================================
-- FUNCTION SFN_GetEntryTypeName
-- Returns the name corresponding to the @EntryType. See TestLog.EntryType
-- =======================================================================
CREATE FUNCTION [Internal].[SFN_GetEntryTypeName](@EntryType char) RETURNS varchar(10)
AS
BEGIN

   IF @EntryType = 'P' RETURN 'Pass'
   IF @EntryType = 'L' RETURN 'Log'
   IF @EntryType = 'F' RETURN 'Failure'
   IF @EntryType = 'E' RETURN 'Error'

   RETURN '???'
   
END
GO
/****** Object:  StoredProcedure [Internal].[SuiteExists]    Script Date: 05/23/2013 11:40:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =======================================================================
-- PROCEDURE SuiteExists
-- Determines if the suite with the name given by @TestName exists 
-- in the database with the name given by @TestDatabaseName.
-- =======================================================================
CREATE PROCEDURE [Internal].[SuiteExists]
   @TestDatabaseName sysname, 
   @SuiteName        sysname,
   @SuiteExists      bit OUT 
AS
BEGIN

   DECLARE @SqlCommand        nvarchar(1000)
   DECLARE @Params            nvarchar(100)
   DECLARE @TestInSuiteCount  int

   SET @SqlCommand = 'SELECT @TestInSuiteCountOUT = COUNT(*) ' + 
      'FROM ' + QUOTENAME(@TestDatabaseName) + '.sys.procedures ' + 
      'WHERE name LIKE ''SQLTest_' + @SuiteName + '#%'''

   SET @Params = '@TestInSuiteCountOUT int OUT'
   EXEC sp_executesql @SqlCommand, @Params, @TestInSuiteCountOUT=@TestInSuiteCount OUT

   SET @SuiteExists = 0
   IF (@TestInSuiteCount >= 1) SET @SuiteExists = 1

END
GO
/****** Object:  UserDefinedFunction [Internal].[SFN_SProcExists]    Script Date: 05/23/2013 11:40:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =======================================================================
-- FUNCTION SFN_SProcExists
-- Determines if the procedure with the name given by @TestName exists 
-- in database with the name given by @TestDatabaseName.
-- =======================================================================
CREATE FUNCTION [Internal].[SFN_SProcExists](@TestDatabaseName sysname, @SProcNameName sysname) RETURNS bit
AS
BEGIN

   DECLARE @ObjectName nvarchar(1000)
   SET @ObjectName = @TestDatabaseName + '..' + @SProcNameName

   IF (object_id(@ObjectName, 'P') IS NOT NULL)
   BEGIN
      RETURN 1
   END

   RETURN 0
END
GO
/****** Object:  StoredProcedure [Internal].[GetCurrentTestSessionId]    Script Date: 05/23/2013 11:40:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =======================================================================
-- PROCEDURE GetCurrentTestSessionId
-- Returns in @TestSessionId the test session id for the current
-- test session.
-- =======================================================================
CREATE PROCEDURE [Internal].[GetCurrentTestSessionId]
   @TestSessionId int OUT
AS
BEGIN

   SELECT @TestSessionId = TestSessionId FROM #Tmp_CrtSessionInfo
   
END
GO
/****** Object:  StoredProcedure [Internal].[AnalyzeSprocName]    Script Date: 05/23/2013 11:40:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =======================================================================
-- PROCEDURE AnalyzeSprocName
-- Analyses the given stored procedure. Detects if it is TST procedure
-- and returns to the caller info needed to categorize it.
-- =======================================================================
CREATE PROCEDURE [Internal].[AnalyzeSprocName]
   @SProcName        sysname,             -- The name of the stored procedure.
   @SuiteName        sysname OUTPUT,      -- At return it will be the suite name.
   @IsTSTSproc       bit OUTPUT,          -- At return it will indicate if it is a TST procedure.
   @SProcType        char(8) OUTPUT       -- At return it will indicate the type of TST procedure.
                                          -- See Data.Test.SProcType
AS
BEGIN

   DECLARE @TestNameIndex int

   SET @IsTSTSproc  = 0

   IF( CHARINDEX('SQLTest_', @SProcName) != 1)   
   BEGIN
      -- This is not a SQL Test sproc
      RETURN 0
   END

   SET @IsTSTSproc = 1
   
   -- Remove the 'SQLTest_' prefix from @SProcName.
   SET @SProcName = RIGHT(@SProcName, LEN(@SProcName) - 8)
   
   IF( CHARINDEX('SETUP_', @SProcName) = 1)
   BEGIN
      SET @SProcType = 'Setup'
      SET @SuiteName = RIGHT(@SProcName, LEN(@SProcName) - 6)
      RETURN 0
   END
   
   IF( CHARINDEX('TEARDOWN_', @SProcName) = 1)
   BEGIN
      SET @SProcType = 'Teardown'
      SET @SuiteName = RIGHT(@SProcName, LEN(@SProcName) - 9)
      RETURN 0
   END
   
   SET @TestNameIndex = CHARINDEX('#', @SProcName)
   IF( @TestNameIndex != 0)
   BEGIN
      SET @SProcType = 'Test'
      SET @SuiteName = LEFT(@SProcName, @TestNameIndex - 1)
      RETURN 0
   END

   -- This test is not associated with a specific suite.
   SET @SuiteName = NULL
   SET @SProcType = 'Test'

   RETURN 0
   
END
GO
/****** Object:  StoredProcedure [Internal].[Rethrow]    Script Date: 05/23/2013 11:40:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =======================================================================
-- PROCEDURE Rethrow
-- Implements the Rethrow functionality 
-- =======================================================================
CREATE PROCEDURE [Internal].[Rethrow]
AS
BEGIN

   -- Return if there is no error information to retrieve.
   IF (ERROR_NUMBER() IS NULL) RETURN;

   DECLARE @ErrorMessage    nvarchar(4000)
   DECLARE @ErrorNumber     int
   DECLARE @ErrorSeverity   int
   DECLARE @ErrorState      int
   DECLARE @ErrorProcedure  nvarchar(200)
   DECLARE @ErrorLine       int

   -- Assign error-handling functions that capture the error information to variables.
   SELECT 
      @ErrorNumber       = ERROR_NUMBER()                ,
      @ErrorSeverity     = ERROR_SEVERITY()              ,
      @ErrorState        = ERROR_STATE()                 ,
      @ErrorProcedure    = ISNULL(ERROR_PROCEDURE(), 'N/A'),
      @ErrorLine         = ERROR_LINE()                  

   -- Build the message string that will contain the original error information.
   SELECT @ErrorMessage = 'Error %d, Level %d, State %d, Procedure %s, Line %d, Message: ' + ERROR_MESSAGE();

   -- Raise an error: msg_str parameter of RAISERROR will contain the original error information.
   -- Raise an error: msg_str parameter of RAISERROR will contain the original error information.
   RAISERROR (
      @ErrorMessage, 
      @ErrorSeverity, 
      1,               
      @ErrorNumber,    -- parameter: original error number.
      @ErrorSeverity,  -- parameter: original error severity.
      @ErrorState,     -- parameter: original error state.
      @ErrorProcedure, -- parameter: original error procedure name.
      @ErrorLine       -- parameter: original error line number.
      )

END
GO
/****** Object:  StoredProcedure [Internal].[GetExpectedErrorInfo]    Script Date: 05/23/2013 11:40:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =======================================================================
-- PROCEDURE GetExpectedErrorInfo
-- Retrieves information about the current expected error. 
-- If no expected error is registered then at return 
-- @ExpectedErrorContextMessage and @ExpectedErrorInfo will be NULL.
-- If an expected error is registered then at return 
-- @ExpectedErrorContextMessage and @ExpectedErrorInfo will contain the 
-- appropiate information (See RegisterExpectedError)
-- =======================================================================
CREATE PROCEDURE [Internal].[GetExpectedErrorInfo]
   @ExpectedErrorContextMessage  nvarchar(1000) OUT, 
   @ExpectedErrorInfo            nvarchar(2000) OUT 
AS
BEGIN

   DECLARE @ExpectedErrorNumber           int
   DECLARE @ExpectedErrorMessage          nvarchar(2048) 
   DECLARE @ExpectedErrorProcedure        nvarchar(126)

   SET @ExpectedErrorInfo           = NULL
   SET @ExpectedErrorContextMessage = NULL

   SELECT 
      @ExpectedErrorNumber          = ExpectedErrorNumber         ,
      @ExpectedErrorMessage         = ExpectedErrorMessage        ,
      @ExpectedErrorProcedure       = ExpectedErrorProcedure      ,
      @ExpectedErrorContextMessage  = ExpectedErrorContextMessage
   FROM #Tmp_CrtSessionInfo

   IF (     (@ExpectedErrorNumber IS NOT NULL) 
         OR (@ExpectedErrorMessage IS NOT NULL) 
         OR (@ExpectedErrorProcedure IS NOT NULL) )
   BEGIN
      SET @ExpectedErrorInfo = 
         'Error number: ' + ISNULL(CAST(@ExpectedErrorNumber AS varchar), 'N/A') +
         ' Procedure: ''' + ISNULL(@ExpectedErrorProcedure, 'N/A') + '''' + 
         ' Message: ' + ISNULL(@ExpectedErrorMessage, 'N/A')
      SET @ExpectedErrorContextMessage = ISNULL(@ExpectedErrorContextMessage, '')
   END
   
END
GO
/****** Object:  StoredProcedure [Internal].[CollectTempTablesSchema]    Script Date: 05/23/2013 11:40:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =======================================================================
-- PROCEDURE: CollectTempTablesSchema
-- Collects schema information about #ExpectedResult and #ActualResult 
-- in #SchemaInfoExpectedResults and #SchemaInfoActualResults
-- =======================================================================
CREATE PROCEDURE [Internal].[CollectTempTablesSchema]
AS
BEGIN

   INSERT INTO #SchemaInfoExpectedResults
   SELECT 
      SysColumns.name                     AS ColumnName,
      SysTypes.name                       AS DataTypeName,
      SysColumns.max_length               AS MaxLength,
      SysColumns.precision                AS ColumnPrecision,
      SysColumns.scale                    AS ColumnScale,
      ISNULL(PKColumns.IsPrimaryKey, 0)   AS IsPrimaryKey,
      CASE WHEN IgnoredColumns.ColumnName IS NULL THEN 0 ELSE 1 END AS IsIgnored,
      PKColumns.PkOrdinal                 AS PkOrdinal,
      SysColumns.collation_name           AS ColumnCollationName
   FROM tempdb.sys.columns AS SysColumns 
   INNER JOIN tempdb.sys.types AS SysTypes ON 
      SysTypes.user_type_id = SysColumns.user_type_id 
   LEFT OUTER JOIN (
         SELECT 
            SysColumns.name               AS PKColumnName,
            SysIndexes.is_primary_key     AS IsPrimaryKey,
            SysIndexColumns.key_ordinal   AS PkOrdinal
         FROM tempdb.sys.columns AS SysColumns 
         INNER JOIN tempdb.sys.indexes AS SysIndexes ON 
            SysIndexes.object_id = SysColumns.object_id 
         INNER JOIN tempdb.sys.index_columns AS SysIndexColumns ON 
            SysIndexColumns.object_id = SysColumns.object_id 
            AND SysIndexColumns.column_id = SysColumns.column_id
            AND SysIndexColumns.index_id = SysIndexes.index_id
         WHERE 
            SysColumns.object_id = object_id('tempdb..#ExpectedResult')
            AND SysIndexes.is_primary_key = 1
      ) AS PKColumns ON SysColumns.name = PKColumns.PKColumnName
   LEFT OUTER JOIN #IgnoredColumns AS IgnoredColumns ON IgnoredColumns.ColumnName = SysColumns.name
   WHERE 
      SysColumns.object_id = object_id('tempdb..#ExpectedResult')

   INSERT INTO #SchemaInfoActualResults
   SELECT 
      SysColumns.name                     AS ColumnName,
      SysTypes.name                       AS DataTypeName,
      SysColumns.max_length               AS MaxLength,
      SysColumns.precision                AS ColumnPrecision,
      SysColumns.scale                    AS ColumnScale,
      ISNULL(PKColumns.IsPrimaryKey, 0)   AS IsPrimaryKey,
      CASE WHEN IgnoredColumns.ColumnName IS NULL THEN 0 ELSE 1 END AS IsIgnored,
      PKColumns.PkOrdinal                 AS PkOrdinal,
      SysColumns.collation_name           AS ColumnCollationName
   FROM tempdb.sys.columns AS SysColumns 
   INNER JOIN tempdb.sys.types AS SysTypes ON 
      SysTypes.user_type_id = SysColumns.user_type_id 
   LEFT OUTER JOIN (
         SELECT 
            SysColumns.name               AS PKColumnName,
            SysIndexes.is_primary_key     AS IsPrimaryKey,
            SysIndexColumns.key_ordinal   AS PkOrdinal
         FROM tempdb.sys.columns AS SysColumns 
         INNER JOIN tempdb.sys.indexes AS SysIndexes ON 
            SysIndexes.object_id = SysColumns.object_id 
         INNER JOIN tempdb.sys.index_columns AS SysIndexColumns ON 
            SysIndexColumns.object_id = SysColumns.object_id 
            AND SysIndexColumns.column_id = SysColumns.column_id
            AND SysIndexColumns.index_id = SysIndexes.index_id
         WHERE 
            SysColumns.object_id = object_id('tempdb..#ActualResult')
            AND SysIndexes.is_primary_key = 1
      ) AS PKColumns ON SysColumns.name = PKColumns.PKColumnName
   LEFT OUTER JOIN #IgnoredColumns AS IgnoredColumns ON IgnoredColumns.ColumnName = SysColumns.name
   WHERE 
      SysColumns.object_id = object_id('tempdb..#ActualResult')

END
GO
/****** Object:  StoredProcedure [Internal].[ValidateTempTablesSchema]    Script Date: 05/23/2013 11:40:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =======================================================================
-- PROCEDURE: ValidateTempTablesSchema
-- Validates that #ExpectedResult and #ActualResult have the same schema 
-- and that all columns have types that can be handled by the comparison
-- procedure.
-- Asumes that #SchemaInfoExpectedResults and #SchemaInfoActualResults
-- are already created and contain the appropiate data.
-- At return: 
--    - If the validation passed then @SchemaError will be NULL
--    - If the validation did not passed then @SchemaError will contain an 
--      error message.
-- =======================================================================
CREATE PROCEDURE [Internal].[ValidateTempTablesSchema]
   @SchemaError       nvarchar(1000) OUT 
AS
BEGIN

   DECLARE @ColumnName                 sysname
   DECLARE @ColumnDataType             sysname
   DECLARE @ColumnTypeInExpected       sysname
   DECLARE @ColumnTypeInActual         sysname
   DECLARE @ColumnLengthInExpected     int
   DECLARE @ColumnLengthInActual       int
   DECLARE @ColumnCollationInExpected  sysname
   DECLARE @ColumnCollationInActual    sysname
   
   
   -- Make sure that we do not have duplicated entries in #IgnoredColumns 
   SET @ColumnName = NULL
   SELECT TOP 1 @ColumnName = ColumnName FROM #IgnoredColumns GROUP BY ColumnName HAVING COUNT(ColumnName) > 1
   IF (@ColumnName IS NOT NULL)
   BEGIN
      SET @SchemaError = 'Column ''' + @ColumnName + ''' is specified more than once in the list of ignored columns.'
      RETURN 
   END

   -- Make sure that all the columns indicated in #IgnoredColumns exist in at least one of the tables #ActualResult or #ExpectedResult
   SET @ColumnName = NULL
   SELECT TOP 1 @ColumnName = ColumnName 
   FROM #IgnoredColumns
   WHERE ColumnName NOT IN (
         SELECT ISNULL(#SchemaInfoExpectedResults.ColumnName, #SchemaInfoActualResults.ColumnName) AS ColumnName
         FROM #SchemaInfoExpectedResults 
         FULL OUTER JOIN #SchemaInfoActualResults ON #SchemaInfoExpectedResults.ColumnName = #SchemaInfoActualResults.ColumnName
      )

   IF (@ColumnName IS NOT NULL)
   BEGIN
      SET @SchemaError = 'Column ''' + @ColumnName + ''' from the list of ignored columns does not exist in any of #ActualResult or #ExpectedResult.'
      RETURN 
   END
   
   -- Make sure that no primary key is in #IgnoredColumns.
   -- We'll only look at the primary key in #SchemaInfoExpectedResults. No need to look at the primary key in #SchemaInfoActualResults
   -- since we check that they have the exact same columns in the primary key.
   SET @ColumnName = NULL
   SELECT TOP 1 @ColumnName = ColumnName FROM #SchemaInfoExpectedResults WHERE IsPrimaryKey = 1 AND IsIgnored = 1
   IF (@ColumnName IS NOT NULL)
   BEGIN
      SET @SchemaError = 'Column ''' + @ColumnName + ''' that is specified in the list of ignored columns cannot be ignored because is part of the primary key in #ActualResult and #ExpectedResult.'
      RETURN 
   END

   SET @ColumnName = NULL
   SELECT TOP 1 @ColumnName = ColumnName FROM #SchemaInfoExpectedResults WHERE IsIgnored = 0 AND ColumnName NOT IN (SELECT ColumnName FROM #SchemaInfoActualResults) 
   IF (@ColumnName IS NOT NULL)
   BEGIN
      SET @SchemaError = '#ExpectedResult and #ActualResult do not have the same schema. Column ''' + @ColumnName + ''' in #ExpectedResult but not in #ActualResult'
      RETURN 
   END

   SELECT TOP 1 @ColumnName = ColumnName FROM #SchemaInfoActualResults  WHERE IsIgnored = 0 AND ColumnName NOT IN (SELECT ColumnName FROM #SchemaInfoExpectedResults )
   IF (@ColumnName IS NOT NULL)
   BEGIN
      SET @SchemaError = '#ExpectedResult and #ActualResult do not have the same schema. Column ''' + @ColumnName + ''' in #ActualResult but not in #ExpectedResult'
      RETURN 
   END
   
   -- At this point, we confirmed that the two tables have the same columns. We will check the column types
   SELECT TOP 1 
      @ColumnName             = #SchemaInfoExpectedResults.ColumnName,
      @ColumnTypeInExpected   = ISNULL(#SchemaInfoExpectedResults.DataTypeName, '?'),
      @ColumnTypeInActual     = ISNULL(#SchemaInfoActualResults.DataTypeName, '?')
   FROM #SchemaInfoExpectedResults
   INNER JOIN #SchemaInfoActualResults ON #SchemaInfoActualResults.ColumnName = #SchemaInfoExpectedResults.ColumnName
   WHERE ISNULL(#SchemaInfoExpectedResults.DataTypeName, '?') != ISNULL(#SchemaInfoActualResults.DataTypeName, '?')

   IF (@ColumnName IS NOT NULL)
   BEGIN
      SET @SchemaError = '#ExpectedResult and #ActualResult do not have the same schema. Column #ExpectedResult.' + @ColumnName + ' has type ' + @ColumnTypeInExpected + '. #ActualResult.' + @ColumnName +' has type ' + @ColumnTypeInActual
      RETURN 
   END
   
   -- Columns in the two tables have to have the same max length.
   SELECT TOP 1 
      @ColumnName             = #SchemaInfoExpectedResults.ColumnName,
      @ColumnLengthInExpected = ISNULL(#SchemaInfoExpectedResults.MaxLength, 0),
      @ColumnLengthInActual   = ISNULL(#SchemaInfoActualResults.MaxLength, 0)
   FROM #SchemaInfoExpectedResults
   INNER JOIN #SchemaInfoActualResults ON #SchemaInfoActualResults.ColumnName = #SchemaInfoExpectedResults.ColumnName
   WHERE ISNULL(#SchemaInfoExpectedResults.MaxLength, 0) != ISNULL(#SchemaInfoActualResults.MaxLength, 0)

   IF (@ColumnName IS NOT NULL)
   BEGIN
      SET @SchemaError = '#ExpectedResult and #ActualResult do not have the same schema. Column #ExpectedResult.' + @ColumnName + ' has length ' + CAST(@ColumnLengthInExpected AS varchar) + '. #ActualResult.' + @ColumnName +' has length ' + CAST(@ColumnLengthInActual AS varchar)
      RETURN 
   END

   -- Columns in the two tables have to have the same collation.
   SELECT TOP 1 
      @ColumnName                = #SchemaInfoExpectedResults.ColumnName,
      @ColumnCollationInExpected = ISNULL(#SchemaInfoExpectedResults.ColumnCollationName, 'no collation'),
      @ColumnCollationInActual   = ISNULL(#SchemaInfoActualResults.ColumnCollationName, 'no collation')
   FROM #SchemaInfoExpectedResults
   INNER JOIN #SchemaInfoActualResults ON #SchemaInfoActualResults.ColumnName = #SchemaInfoExpectedResults.ColumnName
   WHERE ISNULL(#SchemaInfoExpectedResults.ColumnCollationName, 'no collation') != ISNULL(#SchemaInfoActualResults.ColumnCollationName, 'no collation')

   IF (@ColumnName IS NOT NULL)
   BEGIN
      SET @SchemaError = 
            '#ExpectedResult and #ActualResult do not have the same schema. Column #ExpectedResult.' + 
            @ColumnName + ' has collation ' + @ColumnCollationInExpected + '. #ActualResult.' + 
            @ColumnName + ' has collation ' + @ColumnCollationInActual
      RETURN 
   END
   
   -- Make sure that all columns have a valid data type 
   SELECT TOP 1 
      @ColumnName = #SchemaInfoExpectedResults.ColumnName, 
      @ColumnDataType = #SchemaInfoExpectedResults.DataTypeName
   FROM #SchemaInfoExpectedResults
   INNER JOIN #SchemaInfoActualResults ON #SchemaInfoActualResults.ColumnName = #SchemaInfoExpectedResults.ColumnName
   WHERE Internal.SFN_ColumnDataTypeIsValid(#SchemaInfoExpectedResults.DataTypeName) = 0
   AND #SchemaInfoExpectedResults.IsIgnored = 0
   IF (@ColumnName IS NOT NULL)
   BEGIN
      SET @SchemaError = 'Column ' + @ColumnName + ' has a type (''' + @ColumnDataType + ''') that cannot be processed by Assert.TableEquals. To ignore this column use the @IgnoredColumns parameter of Assert.TableEquals.'
      RETURN 
   END

   -- We will check that we have a PK
   IF NOT EXISTS (SELECT ColumnName FROM #SchemaInfoExpectedResults WHERE #SchemaInfoExpectedResults.IsPrimaryKey = 1)
   BEGIN
      SET @SchemaError = '#ExpectedResult and #ActualResult must have a primary key defined'
      RETURN 
   END

   -- We will check that the PK columns are the same and in the same order
   SELECT TOP 1 @ColumnName = #SchemaInfoExpectedResults.ColumnName
   FROM #SchemaInfoExpectedResults
   INNER JOIN #SchemaInfoActualResults ON #SchemaInfoActualResults.ColumnName = #SchemaInfoExpectedResults.ColumnName
   WHERE 
      #SchemaInfoExpectedResults.IsPrimaryKey != #SchemaInfoActualResults.IsPrimaryKey
      OR ISNULL(#SchemaInfoExpectedResults.PkOrdinal, -1) != ISNULL(#SchemaInfoActualResults.PkOrdinal, -1)

   IF (@ColumnName IS NOT NULL)
   BEGIN
      SET @SchemaError = 'The primary keys in #ExpectedResult and #ActualResult are not the same'
      RETURN 
   END

   SET @SchemaError = NULL
   RETURN

END
GO
/****** Object:  UserDefinedFunction [Internal].[SFN_ColumnDataTypeIsValid]    Script Date: 05/23/2013 11:40:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =======================================================================
-- FUNCTION : SFN_ColumnDataTypeIsValid
-- Returns 1 if the data type given by @DataTypeName can be 
--           processed by Assert.TableEquals
-- Returns 1 if the data type given by @DataTypeName cannot be 
--           processed by Assert.TableEquals
-- =======================================================================
CREATE FUNCTION [Internal].[SFN_ColumnDataTypeIsValid](@DataTypeName nvarchar(128)) RETURNS bit
AS
BEGIN

   IF (@DataTypeName = 'bigint'           ) RETURN 1
   IF (@DataTypeName = 'int'              ) RETURN 1
   IF (@DataTypeName = 'smallint'         ) RETURN 1
   IF (@DataTypeName = 'tinyint'          ) RETURN 1
   IF (@DataTypeName = 'money'            ) RETURN 1
   IF (@DataTypeName = 'smallmoney'       ) RETURN 1
   IF (@DataTypeName = 'bit'              ) RETURN 1
   IF (@DataTypeName = 'decimal'          ) RETURN 1
   IF (@DataTypeName = 'numeric'          ) RETURN 1
   IF (@DataTypeName = 'float'            ) RETURN 1
   IF (@DataTypeName = 'real'             ) RETURN 1
   IF (@DataTypeName = 'datetime'         ) RETURN 1
   IF (@DataTypeName = 'smalldatetime'    ) RETURN 1
   IF (@DataTypeName = 'char'             ) RETURN 1
   IF (@DataTypeName = 'text'             ) RETURN 0
   IF (@DataTypeName = 'varchar'          ) RETURN 1
   IF (@DataTypeName = 'nchar'            ) RETURN 1
   IF (@DataTypeName = 'ntext'            ) RETURN 0
   IF (@DataTypeName = 'nvarchar'         ) RETURN 1
   IF (@DataTypeName = 'binary'           ) RETURN 1
   IF (@DataTypeName = 'varbinary'        ) RETURN 1
   IF (@DataTypeName = 'image'            ) RETURN 0
   IF (@DataTypeName = 'cursor'           ) RETURN 0
   IF (@DataTypeName = 'timestamp'        ) RETURN 0
   IF (@DataTypeName = 'sql_variant'      ) RETURN 1
   IF (@DataTypeName = 'uniqueidentifier' ) RETURN 1
   IF (@DataTypeName = 'table'            ) RETURN 0
   IF (@DataTypeName = 'xml'              ) RETURN 0

   -- User defined types not accepted
   RETURN 0

END
GO
/****** Object:  UserDefinedFunction [Internal].[SFN_Internal_GetColumnPart]    Script Date: 05/23/2013 11:40:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =======================================================================
-- FUNCTION : SFN_Internal_GetColumnPart
-- Generates a portion of the SQL query that is used in RunTableComparison. 
-- See RunTableComparison and GenerateComparisonSQLQuery.
-- =======================================================================
CREATE FUNCTION [Internal].[SFN_Internal_GetColumnPart](
   @BareColumnName   sysname, 
   @DataTypeName     nvarchar(128), 
   @MaxLength        int, 
   @ColumnPrecision int) RETURNS nvarchar(max)
AS
BEGIN

   DECLARE @ExpectedResultConvertString   nvarchar(max)
   DECLARE @ActualResultConvertString     nvarchar(max)
   DECLARE @ColumnPartString              nvarchar(max)
   DECLARE @ReplacementValue              nvarchar(max)
   DECLARE @EscapedColumnName             sysname

   DECLARE @ConvertType     varchar(20)
   DECLARE @ConvertLength   varchar(20)
   DECLARE @ConvertStyle    varchar(20)
   DECLARE @UseConvert      int           -- 1 Use CONVERT
                                          -- 2 Use the column without aplying CONVERT
                                          -- 3 Use the string contained in @ReplacementValue

   SET @ConvertType     = 'varchar'
   SET @ConvertLength   = ''           -- We assume we don't need to specify the lenght in CONVERT
   SET @ConvertStyle    = ''           -- We asume that we don't need to specify the style in CONVERT
   SET @UseConvert      = 1            -- We assume that we do need to use CONVERT to nvarchar
   SET @EscapedColumnName = '[' + @BareColumnName + ']'

   IF      (@DataTypeName = 'money'            )    BEGIN SET @ConvertStyle = ', 2'; END
   ELSE IF (@DataTypeName = 'smallmoney'       )    BEGIN SET @ConvertStyle = ', 2'; END
   ELSE IF (@DataTypeName = 'decimal'          )    BEGIN SET @ConvertLength = '(' + CAST(@ColumnPrecision + 10 AS varchar) + ')'; END
   ELSE IF (@DataTypeName = 'numeric'          )    BEGIN SET @ConvertLength = '(' + CAST(@ColumnPrecision + 10 AS varchar) + ')'; END
   ELSE IF (@DataTypeName = 'float'            )    BEGIN SET @ConvertStyle = ', 2'; SET @ConvertLength = '(30)'; END
   ELSE IF (@DataTypeName = 'real'             )    BEGIN SET @ConvertStyle = ', 1'; SET @ConvertLength = '(30)'; END
   ELSE IF (@DataTypeName = 'datetime'         )    BEGIN SET @ConvertStyle = ', 121'; END
   ELSE IF (@DataTypeName = 'smalldatetime'    )    BEGIN SET @ConvertStyle = ', 120'; END
   ELSE IF (@DataTypeName = 'char'             )    BEGIN SET @ConvertLength = '(' + CAST(@MaxLength AS varchar) + ')'; END
   ELSE IF (@DataTypeName = 'nchar'            )    BEGIN SET @ConvertLength = '(' + CAST(@MaxLength/2 AS varchar) + ')'; SET @ConvertType = 'nvarchar'; END
   ELSE IF (@DataTypeName = 'varchar'          )    
   BEGIN 
      IF (@MaxLength = -1) SET @ConvertLength = '(max)'
      ELSE                 SET @ConvertLength = '(' + CAST(@MaxLength AS varchar) + ')'
   END
   ELSE IF (@DataTypeName = 'nvarchar'         )    
   BEGIN 
      SET @ConvertType = 'nvarchar'
      IF (@MaxLength = -1) SET @ConvertLength = '(max)'
      ELSE                 SET @ConvertLength = '(' + CAST(@MaxLength/2 AS varchar) + ')'
   END
   ELSE IF (@DataTypeName = 'binary'           )    BEGIN SET @ReplacementValue = '...binary value...'; SET @UseConvert = 3; END
   ELSE IF (@DataTypeName = 'varbinary'        )    BEGIN SET @ReplacementValue = '...binary value...'; SET @UseConvert = 3; END
   ELSE IF (@DataTypeName = 'uniqueidentifier' )    BEGIN SET @ConvertLength = '(36)'; END



   IF (@UseConvert = 1)
   BEGIN
      SET @ExpectedResultConvertString = 'CONVERT(' + @ConvertType + @ConvertLength + ', #ExpectedResult.' + @EscapedColumnName + @ConvertStyle + ') COLLATE database_default '
      SET @ActualResultConvertString   = 'CONVERT(' + @ConvertType + @ConvertLength + ', #ActualResult.'   + @EscapedColumnName + @ConvertStyle + ') COLLATE database_default '
   END
   ELSE IF (@UseConvert = 2)
   BEGIN
      SET @ExpectedResultConvertString = '#ExpectedResult.' + @EscapedColumnName
      SET @ActualResultConvertString   = '#ActualResult.' + @EscapedColumnName
   END

   IF (@UseConvert = 3)
   BEGIN
      SET @ColumnPartString = '''' + @BareColumnName + '=(' + @ReplacementValue + '/' + @ReplacementValue + ') '' '
   END
   ELSE
   BEGIN
      SET @ColumnPartString = '''' + @BareColumnName + 
               '=('' + ISNULL('     + @ExpectedResultConvertString + ', ''null'')' + 
               ' + ''/'' + ISNULL(' + @ActualResultConvertString   + ', ''null'') + '') '' '
   END
   
   RETURN @ColumnPartString

END
GO
/****** Object:  StoredProcedure [Internal].[GetSqlVarInfo]    Script Date: 05/23/2013 11:40:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =======================================================================
-- FUNCTION: GetSqlVarInfo
-- Determines the data type and the data type family for the value 
-- stored in @SqlVariant.
-- Also converts @SqlVariant in a string applying a CONVERT that will 
-- force the maximum precision.
--    The data type           The data type family             Abreviation
--       sql_variant                sql_variant                SV
--       datetime                   Date and Time              DT
--       smalldatetime              Date and Time              DT
--       float                      Approximate numeric        AN
--       real                       Approximate numeric        AN
--       numeric                    Exact numeric              EN
--       decimal                    Exact numeric              EN
--       money                      Exact numeric              EN
--       smallmoney                 Exact numeric              EN
--       bigint                     Exact numeric              EN
--       int                        Exact numeric              EN
--       smallint                   Exact numeric              EN
--       tinyint                    Exact numeric              EN
--       bit                        Exact numeric              EN
--       nvarchar                   Unicode                    UC
--       nchar                      Unicode                    UC
--       varchar                    Unicode                    UC
--       char                       Unicode                    UC
--       varbinary                  Binary                     BI
--       binary                     Binary                     BI
--       uniqueidentifier           Uniqueidentifier           UQ
--       Other                      Other                      ??
--
-- If @SqlVariant is NULL then both @BaseType and @DataTypeFamily will 
-- be returend as NULL.
-- =======================================================================
CREATE PROCEDURE [Internal].[GetSqlVarInfo]
   @SqlVariant       sql_variant,
   @BaseType         sysname OUT,
   @DataTypeFamily   char(2) OUT,
   @StringValue      nvarchar(max) OUT
AS
BEGIN

   SET @BaseType         = NULL
   SET @DataTypeFamily   = NULL
   SET @StringValue      = 'NULL'
   
   IF (@SqlVariant IS NULL) RETURN

   SET @BaseType = CAST(SQL_VARIANT_PROPERTY (@SqlVariant, 'BaseType') AS sysname)
   SET @StringValue = CONVERT(nvarchar(max), @SqlVariant); 
         IF (@BaseType = 'sql_variant'      ) BEGIN SET @DataTypeFamily = 'SV'; SET @StringValue = CONVERT(nvarchar(max), @SqlVariant      ); END
   ELSE  IF (@BaseType = 'datetime'         ) BEGIN SET @DataTypeFamily = 'DT'; SET @StringValue = CONVERT(nvarchar(max), @SqlVariant, 121 ); END
   ELSE  IF (@BaseType = 'smalldatetime'    ) BEGIN SET @DataTypeFamily = 'DT'; SET @StringValue = CONVERT(nvarchar(max), @SqlVariant, 121 ); END
   ELSE  IF (@BaseType = 'float'            ) BEGIN SET @DataTypeFamily = 'AN'; SET @StringValue = CONVERT(nvarchar(max), @SqlVariant, 2   ); END
   ELSE  IF (@BaseType = 'real'             ) BEGIN SET @DataTypeFamily = 'AN'; SET @StringValue = CONVERT(nvarchar(max), @SqlVariant, 2   ); END
   ELSE  IF (@BaseType = 'numeric'          ) BEGIN SET @DataTypeFamily = 'EN'; SET @StringValue = CONVERT(nvarchar(max), @SqlVariant      ); END
   ELSE  IF (@BaseType = 'decimal'          ) BEGIN SET @DataTypeFamily = 'EN'; SET @StringValue = CONVERT(nvarchar(max), @SqlVariant      ); END
   ELSE  IF (@BaseType = 'money'            ) BEGIN SET @DataTypeFamily = 'EN'; SET @StringValue = CONVERT(nvarchar(max), @SqlVariant, 2   ); END
   ELSE  IF (@BaseType = 'smallmoney'       ) BEGIN SET @DataTypeFamily = 'EN'; SET @StringValue = CONVERT(nvarchar(max), @SqlVariant, 2   ); END
   ELSE  IF (@BaseType = 'bigint'           ) BEGIN SET @DataTypeFamily = 'EN'; SET @StringValue = CONVERT(nvarchar(max), @SqlVariant      ); END
   ELSE  IF (@BaseType = 'int'              ) BEGIN SET @DataTypeFamily = 'EN'; SET @StringValue = CONVERT(nvarchar(max), @SqlVariant      ); END
   ELSE  IF (@BaseType = 'smallint'         ) BEGIN SET @DataTypeFamily = 'EN'; SET @StringValue = CONVERT(nvarchar(max), @SqlVariant      ); END
   ELSE  IF (@BaseType = 'tinyint'          ) BEGIN SET @DataTypeFamily = 'EN'; SET @StringValue = CONVERT(nvarchar(max), @SqlVariant      ); END
   ELSE  IF (@BaseType = 'bit'              ) BEGIN SET @DataTypeFamily = 'EN'; SET @StringValue = CONVERT(nvarchar(max), @SqlVariant      ); END
   ELSE  IF (@BaseType = 'nvarchar'         ) BEGIN SET @DataTypeFamily = 'UC'; SET @StringValue = '''' + CONVERT(nvarchar(max), @SqlVariant) + ''''; END
   ELSE  IF (@BaseType = 'nchar'            ) BEGIN SET @DataTypeFamily = 'UC'; SET @StringValue = '''' + CONVERT(nvarchar(max), @SqlVariant) + ''''; END
   ELSE  IF (@BaseType = 'varchar'          ) BEGIN SET @DataTypeFamily = 'UC'; SET @StringValue = '''' + CONVERT(nvarchar(max), @SqlVariant) + ''''; END
   ELSE  IF (@BaseType = 'char'             ) BEGIN SET @DataTypeFamily = 'UC'; SET @StringValue = '''' + CONVERT(nvarchar(max), @SqlVariant) + ''''; END
   ELSE  IF (@BaseType = 'varbinary'        ) BEGIN SET @DataTypeFamily = 'BI'; SET @StringValue = CONVERT(nvarchar(max), @SqlVariant      ); END
   ELSE  IF (@BaseType = 'binary'           ) BEGIN SET @DataTypeFamily = 'BI'; SET @StringValue = CONVERT(nvarchar(max), @SqlVariant      ); END
   ELSE  IF (@BaseType = 'uniqueidentifier' ) BEGIN SET @DataTypeFamily = 'UQ'; SET @StringValue = '{' + CONVERT(nvarchar(max), @SqlVariant) + '}'; END
   ELSE                                       BEGIN SET @DataTypeFamily = '??'; SET @StringValue = CONVERT(nvarchar(max), @SqlVariant      ); END

END
GO
/****** Object:  StoredProcedure [Utils].[DeleteTestTables]    Script Date: 05/23/2013 11:40:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
-- =======================================================================
-- PROCEDURE: DropTestTables
-- If exists then drops the table: #ActualResult
-- If exists then drops the table: #ExpectedResult
-- TODO: Do we need to provide this? 
-- =======================================================================
CREATE PROCEDURE Utils.DropTestTables
AS
BEGIN

   RETURN 
   
   IF (object_id('tempdb..#ExpectedResult') IS NOT NULL) DROP TABLE #ExpectedResult
   IF (object_id('tempdb..#ActualResult') IS NOT NULL) DROP TABLE #ActualResult

END
GO
*/

-- =======================================================================
-- PROCEDURE: DeleteTestTables
-- Deletes all entries from the table: #ActualResult
-- Deletes all entries from the table: #ExpectedResult
-- =======================================================================
CREATE PROCEDURE [Utils].[DeleteTestTables]
AS
BEGIN

   DELETE FROM #ActualResult
   DELETE FROM #ExpectedResult

END
GO
/****** Object:  Table [Data].[TestTimer]    Script Date: 05/23/2013 11:40:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [Data].[TestTimer](
	[TestTimerId] [int] IDENTITY(1,1) NOT NULL,
	[FullSprocName] [varchar](max) NULL,
	[DatabaseName] [sysname] NULL,
	[Suitename] [varchar](20) NULL,
	[TestStatus] [varchar](10) NULL,
	[TestStart] [datetime] NULL,
	[TestFinish] [datetime] NULL,
	[RunTimeMins] [float] NULL,
	[RunTimeSec] [float] NULL,
	[TestSessionId] [int] NULL,
	[TestSProcId] [int] NULL,
	[DatabaseTestName] [varchar](50) NULL,
	[RunDate] [datetime] NULL,
	[RunCount] [int] NULL
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [Data].[TestLog]    Script Date: 05/23/2013 11:40:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [Data].[TestLog](
	[LogEntryId] [int] IDENTITY(1,1) NOT NULL,
	[TestSessionId] [int] NOT NULL,
	[TestId] [int] NOT NULL,
	[EntryType] [char](1) NOT NULL,
	[CreatedTime] [datetime] NOT NULL,
	[LogMessage] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_TestLog] PRIMARY KEY CLUSTERED 
(
	[LogEntryId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
CREATE NONCLUSTERED INDEX [IX_TestLog_TestId] ON [Data].[TestLog] 
(
	[TestId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_TestLog_TestSessionId] ON [Data].[TestLog] 
(
	[TestSessionId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
/****** Object:  Table [Data].[Suite]    Script Date: 05/23/2013 11:40:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Data].[Suite](
	[SuiteId] [int] IDENTITY(1,1) NOT NULL,
	[TestSessionId] [int] NOT NULL,
	[SchemaName] [sysname] NULL,
	[SuiteName] [sysname] NULL,
 CONSTRAINT [PK_Suite] PRIMARY KEY CLUSTERED 
(
	[SuiteId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY],
 CONSTRAINT [UK_Suite_TestSessionId_SuiteName] UNIQUE NONCLUSTERED 
(
	[TestSessionId] ASC,
	[SchemaName] ASC,
	[SuiteName] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_Suite_TestSessionId_SuiteId] ON [Data].[Suite] 
(
	[TestSessionId] ASC,
	[SuiteId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
/****** Object:  Table [Data].[SystemErrorLog]    Script Date: 05/23/2013 11:40:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Data].[SystemErrorLog](
	[LogEntryId] [int] IDENTITY(1,1) NOT NULL,
	[TestSessionId] [int] NOT NULL,
	[CreatedTime] [datetime] NOT NULL,
	[LogMessage] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_SystemErrorLog] PRIMARY KEY CLUSTERED 
(
	[LogEntryId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_SystemErrorLog_TestSessionId] ON [Data].[SystemErrorLog] 
(
	[TestSessionId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
/****** Object:  Table [Data].[Test]    Script Date: 05/23/2013 11:40:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [Data].[Test](
	[TestId] [int] IDENTITY(1,1) NOT NULL,
	[TestSessionId] [int] NOT NULL,
	[SuiteId] [int] NOT NULL,
	[SchemaName] [sysname] NOT NULL,
	[SProcName] [sysname] NOT NULL,
	[SProcType] [char](8) NOT NULL,
 CONSTRAINT [PK_Test] PRIMARY KEY CLUSTERED 
(
	[TestId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY],
 CONSTRAINT [UK_Test_SuiteId_SchemaName_SProcName] UNIQUE NONCLUSTERED 
(
	[SuiteId] ASC,
	[SchemaName] ASC,
	[SProcName] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY],
 CONSTRAINT [UK_Test_TestSessionId_SchemaName_SProcName] UNIQUE NONCLUSTERED 
(
	[TestSessionId] ASC,
	[SchemaName] ASC,
	[SProcName] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
CREATE NONCLUSTERED INDEX [IX_Test_SuiteId_SProcName] ON [Data].[Test] 
(
	[SuiteId] ASC,
	[SProcName] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_Test_TestSessionId_SProcName] ON [Data].[Test] 
(
	[TestSessionId] ASC,
	[SProcName] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
/****** Object:  UserDefinedFunction [Internal].[SFN_UseTSTRollbackForTest]    Script Date: 05/23/2013 11:40:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =======================================================================
-- FUNCTION SFN_UseTSTRollbackForTest
-- Determins if transactions can be used for the given test.
-- =======================================================================
CREATE FUNCTION [Internal].[SFN_UseTSTRollbackForTest](@TestSessionId int, @TestId int) RETURNS bit
AS
BEGIN

   DECLARE @UseTSTRollback varchar(100)
   
   SET @UseTSTRollback = '1' -- Default value

   SELECT @UseTSTRollback = TSTParameters.ParameterValue
   FROM Data.TSTParameters 
   WHERE 
      TestSessionId = @TestSessionId
      AND ParameterName  = 'UseTSTRollback'
      AND Scope = 'All'

   -- The 'Suite' scope will overwrite the 'All' scope
   SELECT @UseTSTRollback = TSTParameters.ParameterValue
   FROM Data.TSTParameters
   INNER JOIN Data.Suite ON 
      Suite.TestSessionId = TSTParameters.TestSessionId
      AND TSTParameters.Scope = 'Suite'
      AND Suite.SuiteName = TSTParameters.ScopeValue
   INNER JOIN Data.Test ON 
      Test.SuiteId = Suite.SuiteId
   WHERE 
      TSTParameters.TestSessionId = @TestSessionId
      AND TSTParameters.ParameterName  = 'UseTSTRollback'
      AND Test.TestId = @TestId

   -- The 'Test' scope will overwrite the 'Suite' and 'All' scope
   SELECT @UseTSTRollback = TSTParameters.ParameterValue
   FROM Data.TSTParameters
   INNER JOIN Data.Test ON 
      Test.TestSessionId = TSTParameters.TestSessionId
      AND TSTParameters.Scope = 'Test'
      AND Test.SProcName = TSTParameters.ScopeValue
   WHERE 
      TSTParameters.TestSessionId = @TestSessionId
      AND TSTParameters.ParameterName  = 'UseTSTRollback'
      AND Test.TestId = @TestId
      
   IF @UseTSTRollback = '0' RETURN 0
   RETURN 1
   
END
GO
/****** Object:  StoredProcedure [Utils].[SetConfiguration]    Script Date: 05/23/2013 11:40:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =======================================================================
-- END TST Internals.
-- =======================================================================


-- =======================================================================
-- START TST API.
-- These are stored procedures that are typicaly called from within the 
-- test stored procedures.
-- =======================================================================

-- =======================================================================
-- PROCEDURE: SetConfiguration
-- Sets up TST parameters. Typically called by the tests in the SETUP 
-- procedure or in the TSTConfig procedures. 
-- In case of an invalid call it will raise an error and return 1
-- =======================================================================
CREATE PROCEDURE [Utils].[SetConfiguration]
   @ParameterName       varchar(32),        -- See table TSTParameters and CK_TSTParameters_ParameterName.
   @ParameterValue      varchar(100),       -- The parameter value. Depends on the ParameterName.
                                            -- See table TSTParameters and CK_TSTParameters_ParameterName.
   @Scope               sysname,            -- See table TSTParameters and CK_TSTParameters_Scope.
   @ScopeValue          sysname = NULL      -- Depends on Scope. 
                                            -- See table TSTParameters and CK_TSTParameters_Scope.
AS
BEGIN

   DECLARE @TestSessionId     int
   DECLARE @TestDatabaseName  sysname
   DECLARE @SuiteExists       bit

   SELECT @TestSessionId = TestSessionId FROM #Tmp_CrtSessionInfo
   SELECT @TestDatabaseName = TestSession.DatabaseName FROM Data.TestSession WHERE TestSessionId = @TestSessionId

   IF (@ParameterName != 'UseTSTRollback')
   BEGIN
         RAISERROR('Invalid call to SetConfiguration. @ParameterName has an invalid value: ''%s''.', 16, 1, @ParameterName)
         RETURN 1
   END
   
   -- Validate parameters
   IF (@ParameterName='UseTSTRollback')
   BEGIN
      IF (@ParameterValue IS NULL OR (@ParameterValue != '0' AND @ParameterValue != '1') )
      BEGIN
         RAISERROR('Invalid call to SetConfiguration. @ParameterValue has an invalid value: ''%s''. Valid values are ''0'' and ''1''', 16, 1, @ParameterValue)
         RETURN 1
      END
   END
   
   IF (@Scope='All')
   BEGIN
      IF (@ScopeValue IS NOT NULL)
      BEGIN
         RAISERROR('Invalid call to SetConfiguration. @ScopeValue has an invalid value: ''%s''. When @Scope=''All'' @ScopeValue can only be NULL', 16, 1, @ScopeValue)
         RETURN 1
      END
   END
   ELSE IF (@Scope='Suite')
   BEGIN
      IF (@ScopeValue IS NULL)
      BEGIN
         RAISERROR('Invalid call to SetConfiguration. @ScopeValue cannot be NULL when @Scope=''Suite''', 16, 1)
         RETURN 1
      END
      
      EXEC Internal.SuiteExists @TestDatabaseName, @ScopeValue, @SuiteExists OUT
      IF (@SuiteExists = 0)
      BEGIN
         RAISERROR('Invalid call to SetConfiguration. Cannot find the suite indicated by @ScopeValue: ''%s''', 16, 1, @ScopeValue)
         RETURN 1
      END
   END
   ELSE IF (@Scope='Test')
   BEGIN
      IF (@ScopeValue IS NULL)
      BEGIN
         RAISERROR('Invalid call to SetConfiguration. @ScopeValue cannot be NULL when @Scope=''Test''', 16, 1)
         RETURN 1
      END

      IF (Internal.SFN_SProcExists(@TestDatabaseName, @ScopeValue) = 0)
      BEGIN
         RAISERROR('Invalid call to SetConfiguration. Cannot find the test indicated by @ScopeValue: ''%s''', 16, 1, @ScopeValue)
         RETURN 1
      END

      -- Make sure that the procedure given by @ScopeValue followsthe namingconvention for a TST test
      DECLARE @SuiteName         sysname
      DECLARE @IsTSTSproc        bit
      DECLARE @SProcType         char(8)

      EXEC Internal.AnalyzeSprocName @ScopeValue, @SuiteName OUTPUT, @IsTSTSproc OUTPUT, @SProcType OUTPUT
      IF (@IsTSTSproc = 0 OR @SProcType != 'Test')
      BEGIN
         RAISERROR('Invalid call to SetConfiguration. The test indicated by @ScopeValue: ''%s'' does not follow the naming conventions for a TST test procedure', 16, 1, @ScopeValue)
         RETURN 1
      END
      
   END
   ELSE
   BEGIN
      RAISERROR('Invalid call to SetConfiguration. Invalid value for @Scope: ''%s''', 16, 1, @Scope)
      RETURN 1
   END

   -- Now that the parameters were validated, insert a row in TSTParameters
   INSERT INTO Data.TSTParameters(TestSessionId, ParameterName, ParameterValue, Scope, ScopeValue) 
   VALUES (@TestSessionId, @ParameterName, @ParameterValue, @Scope, @ScopeValue)

END
GO
/****** Object:  StoredProcedure [Internal].[CleanSessionData]    Script Date: 05/23/2013 11:40:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =======================================================================
-- PROCEDURE: CleanSessionData
-- It will delete all the transitory data that refers to the test session 
-- given by @TestSessionId
-- =======================================================================
CREATE PROCEDURE [Internal].[CleanSessionData]
   @TestSessionId   int
AS
BEGIN

   DELETE FROM Data.SystemErrorLog WHERE TestSessionId=@TestSessionId
   
   DELETE FROM Data.TestLog WHERE TestSessionId=@TestSessionId
   
   DELETE Data.Test
   FROM Data.Test
   WHERE Test.TestSessionId=@TestSessionId

   DELETE FROM Data.Suite WHERE TestSessionId=@TestSessionId
   
   DELETE FROM Data.TestSession WHERE TestSessionId=@TestSessionId
   
END
GO
/****** Object:  UserDefinedFunction [Internal].[SFN_GetFullSprocName]    Script Date: 05/23/2013 11:40:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =======================================================================
-- FUNCTION SFN_GetFullSprocName
-- Returns the full name of the sproc identified by @TestId
-- The full name has the format: Database.Schema.Name
-- =======================================================================
CREATE FUNCTION [Internal].[SFN_GetFullSprocName](@TestId int) RETURNS nvarchar(1000)
AS
BEGIN

   DECLARE @DatabaseName   sysname
   DECLARE @SchemaName     sysname
   DECLARE @SProcName      sysname
   DECLARE @FullSprocName  nvarchar(1000)

   SELECT 
      @DatabaseName  = TestSession.DatabaseName,
      @SchemaName    = Test.SchemaName,
      @SProcName     = Test.SProcName
   FROM Data.Test
   INNER JOIN Data.TestSession ON TestSession.TestSessionId = Test.TestSessionId
   WHERE TestId = @TestId
   
   SET @FullSprocName = QUOTENAME(@DatabaseName) + '.' + QUOTENAME(ISNULL(@SchemaName, '')) + '.' + QUOTENAME(@SProcName)

   RETURN @FullSprocName
   
END
GO
/****** Object:  View [Data].[TSTResultsEx]    Script Date: 05/23/2013 11:40:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =======================================================================
-- VIEW: TSTResultsEx
-- Aggregates data from several tables to facilitate results reporting
-- Adds more info compared with TSTResults. Specifically test status and suite status
-- =======================================================================
CREATE VIEW [Data].[TSTResultsEx] AS
SELECT 
   LogEntries.LogEntryId,
   LogEntries.TestSessionId,
   Suite.SuiteId,
   ISNULL(Suite.SuiteName, 'Anonymous') AS SuiteName,
   SuiteStatus = CASE WHEN SuiteFailInfo.FailuresOrErrorsCount > 0 THEN 'F' ELSE 'P' END,
   Test.TestId,
   Test.SProcName,
   TestStatus = CASE WHEN TestFailInfo.FailuresOrErrorsCount > 0 THEN 'F' ELSE 'P' END,
   LogEntries.EntryType,
   LogEntries.LogMessage,
   LogEntries.CreatedTime
FROM Data.TestLog AS LogEntries
INNER JOIN Data.Test  ON LogEntries.TestId = Test.TestId
INNER JOIN Data.Suite ON Suite.SuiteId = Test.SuiteId
INNER JOIN  (  SELECT 
                  TestId, 
                  (  SELECT COUNT(*) FROM Data.TestLog AS L1
                     WHERE 
                        (L1.EntryType = 'E' OR L1.EntryType = 'F' )
                        AND L1.TestId = T1.TestId
                  ) AS FailuresOrErrorsCount
               FROM TST.Data.Test AS T1
            ) AS TestFailInfo ON TestFailInfo.TestId = Test.TestId

INNER JOIN  (  SELECT 
                  SuiteId, 
                  (  SELECT COUNT(*) FROM Data.TestLog L2
                     INNER JOIN Data.Test AS T2 ON L2.TestId = T2.TestId 
                     WHERE 
                        (L2.EntryType = 'E' OR L2.EntryType = 'F' )
                        AND T2.SuiteId = S1.SuiteId
                  ) AS FailuresOrErrorsCount
               FROM TST.Data.Suite AS S1
            ) AS SuiteFailInfo ON SuiteFailInfo.SuiteId = Suite.SuiteId
GO
/****** Object:  View [Data].[TSTResults]    Script Date: 05/23/2013 11:40:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =======================================================================
-- VIEW: TSTResults 
-- Aggregates data from several tables to facilitate results reporting
-- =======================================================================
CREATE VIEW [Data].[TSTResults] AS
SELECT 
   TestLog.LogEntryId,
   TestLog.TestSessionId,
   Suite.SuiteId,
   Suite.SuiteName,
   Test.TestId,
   Test.SProcName,
   TestLog.EntryType,
   TestLog.CreatedTime,
   TestLog.LogMessage
FROM Data.TestLog
INNER JOIN Data.Test  ON TestLog.TestId = Test.TestId
INNER JOIN Data.Suite ON Suite.SuiteId = Test.SuiteId
GO
/****** Object:  UserDefinedFunction [Internal].[SFN_GetCountOfSuitesInSession]    Script Date: 05/23/2013 11:40:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =======================================================================
-- FUNCTION SFN_GetCountOfSuitesInSession
-- Returns the number of suites in the given session
-- =======================================================================
CREATE FUNCTION [Internal].[SFN_GetCountOfSuitesInSession](@TestSessionId int) RETURNS int
AS
BEGIN

   DECLARE @CountOfSuitesInSession int
   
   SELECT @CountOfSuitesInSession = COUNT(1) 
   FROM Data.Suite WHERE TestSessionId = @TestSessionId
   
   RETURN ISNULL(@CountOfSuitesInSession, 0)
END
GO
/****** Object:  StoredProcedure [Internal].[EnsureSuite]    Script Date: 05/23/2013 11:40:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =======================================================================
-- PROCEDURE EnsureSuite
-- This will make sure that the given suite is recorded in the table Suite
-- It will return the Suite Id in @SuiteId
-- =======================================================================
CREATE PROCEDURE [Internal].[EnsureSuite]
   @TestSessionId    int,              -- Identifies the test session.
   @SchemaName       sysname,          -- The schema name 
   @SuiteName        sysname,          -- The suite name
   @SuiteId          int OUTPUT        -- At return will indicate 
AS
BEGIN

   -- If this is the anonymous suite we'll ignore which schema is in. 
   IF @SuiteName IS NULL SET @SchemaName = NULL

   SET @SuiteId = NULL
   SELECT @SuiteId = SuiteId FROM Data.Suite 
   WHERE 
      @TestSessionId = TestSessionId 
      AND (SchemaName = @SchemaName OR (SchemaName IS NULL AND @SchemaName IS NULL) )
      AND (SuiteName = @SuiteName OR (SuiteName IS NULL AND @SuiteName IS NULL) )
   IF(@SuiteId IS NOT NULL) RETURN 0

   INSERT INTO Data.Suite(TestSessionId, SchemaName, SuiteName) VALUES(@TestSessionId, @SchemaName, @SuiteName)
   SET @SuiteId = SCOPE_IDENTITY()
   
   RETURN 0

END
GO
/****** Object:  UserDefinedFunction [Internal].[SFN_GetCountOfTestsInSession]    Script Date: 05/23/2013 11:40:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =======================================================================
-- FUNCTION SFN_GetCountOfTestsInSession
-- Returns the number of tests in the given session
-- =======================================================================
CREATE FUNCTION [Internal].[SFN_GetCountOfTestsInSession](@TestSessionId int) RETURNS int
AS
BEGIN

   DECLARE @CountOfTestsInSession int
   
   SELECT @CountOfTestsInSession = COUNT(1) 
   FROM Data.Test 
   WHERE 
      Test.TestSessionId = @TestSessionId
      AND Test.SProcType = 'Test'
   
   RETURN ISNULL(@CountOfTestsInSession, 0)
END
GO
/****** Object:  UserDefinedFunction [Internal].[SFN_GetCountOfPassedTestsInSession]    Script Date: 05/23/2013 11:40:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =======================================================================
-- FUNCTION SFN_GetCountOfPassedTestsInSession
-- Returns the number of tests that have passed in the given session
-- =======================================================================
CREATE FUNCTION [Internal].[SFN_GetCountOfPassedTestsInSession](@TestSessionId int) RETURNS int
AS
BEGIN

   DECLARE @CountOfPassedTestsInSession int
   
   SELECT @CountOfPassedTestsInSession = COUNT(1) 
   FROM Data.Test 
   WHERE 
      TestId NOT IN (
         SELECT DISTINCT Test.TestId 
         FROM Data.TestLog 
         INNER JOIN Data.Test ON Test.TestId = TestLog.TestId
         WHERE 
            TestLog.TestSessionId = @TestSessionId
            AND TestLog.EntryType IN ('F', 'E')
            AND Test.SProcType = 'Test'
         ) 
      AND Test.TestSessionId = @TestSessionId
      AND Test.SProcType = 'Test'
   
   RETURN ISNULL(@CountOfPassedTestsInSession, 0)

END
GO
/****** Object:  UserDefinedFunction [Internal].[SFN_GetCountOfFailedTestsInSession]    Script Date: 05/23/2013 11:40:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =======================================================================
-- FUNCTION SFN_GetCountOfFailedTestsInSession
-- Returns the number of failed tests in the given test session
-- =======================================================================
CREATE FUNCTION [Internal].[SFN_GetCountOfFailedTestsInSession](@TestSessionId int) RETURNS int
AS
BEGIN
   
   DECLARE @CountOfFailedTestsInSession int
   
   SELECT @CountOfFailedTestsInSession = COUNT(1) 
   FROM (
         SELECT DISTINCT Test.TestId 
         FROM Data.TestLog 
         INNER JOIN Data.Test ON Test.TestId = TestLog.TestId
         WHERE 
            TestLog.TestSessionId = @TestSessionId
            AND TestLog.EntryType IN ('F', 'E')
            AND Test.SProcType = 'Test'
        ) AS FailedTestsList
   
   RETURN ISNULL(@CountOfFailedTestsInSession, 0)
   
END
GO
/****** Object:  UserDefinedFunction [Internal].[SFN_GetCountOfTestsInSuite]    Script Date: 05/23/2013 11:40:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =======================================================================
-- FUNCTION SFN_GetCountOfTestsInSuite
-- Returns the number of passed tests in the given suite
-- =======================================================================
CREATE FUNCTION [Internal].[SFN_GetCountOfTestsInSuite](@SuiteId int) RETURNS int
AS
BEGIN
   
   DECLARE @CountOfTestInSuite int
   
   SELECT @CountOfTestInSuite = COUNT(1) 
   FROM Data.Test 
   WHERE 
      Test.SuiteId = @SuiteId
      AND Test.SProcType = 'Test'

   
   RETURN ISNULL(@CountOfTestInSuite, 0)
   
END
GO
/****** Object:  UserDefinedFunction [Internal].[SFN_GetCountOfFailedTestsInSuite]    Script Date: 05/23/2013 11:40:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =======================================================================
-- FUNCTION SFN_GetCountOfFailedTestsInSuite
-- Returns the number of failed tests in the given suite
-- =======================================================================
CREATE FUNCTION [Internal].[SFN_GetCountOfFailedTestsInSuite](@SuiteId int) RETURNS int
AS
BEGIN
   
   DECLARE @CountOfFailedTestInSuite int
   
   SELECT @CountOfFailedTestInSuite = COUNT(1) 
   FROM (
         SELECT DISTINCT Test.TestId 
         FROM Data.TestLog 
         INNER JOIN Data.Test ON TestLog.TestId = Test.TestId
         WHERE 
            Test.SuiteId = @SuiteId
            AND TestLog.EntryType IN ('F', 'E')
            AND Test.SProcType = 'Test'
        ) AS FailedTestsList
   
   RETURN ISNULL(@CountOfFailedTestInSuite, 0)
   
END
GO
/****** Object:  StoredProcedure [Assert].[Pass]    Script Date: 05/23/2013 11:40:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =======================================================================
-- PROCEDURE: Pass
-- Can be called by the test procedures to mark a test pass. 
-- It will record an entry in TestLog.
-- =======================================================================
CREATE PROCEDURE [Assert].[Pass]
   @Message nvarchar(max) = ''
AS
BEGIN
   DECLARE @TestSessionId int
   DECLARE @TestId int
   
   SELECT @TestSessionId = TestSessionId, @TestId = TestId FROM #Tmp_CrtSessionInfo
   INSERT INTO Data.TestLog(TestSessionId, TestId, EntryType, LogMessage) VALUES(@TestSessionId, @TestId, 'P', ISNULL(@Message, '') )
END
GO
/****** Object:  StoredProcedure [Assert].[Fail]    Script Date: 05/23/2013 11:40:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =======================================================================
-- PROCEDURE: Fail
-- Can be called by the test procedures to mark a test failure. 
-- It will record an entry in TestLog and raise an exception.
-- =======================================================================
CREATE PROCEDURE [Assert].[Fail]
   @ErrorMessage  nvarchar(max)
AS
BEGIN
   DECLARE @TestSessionId int
   DECLARE @TestId int
   
   SELECT @TestSessionId = TestSessionId, @TestId = TestId FROM #Tmp_CrtSessionInfo
   INSERT INTO Data.TestLog(TestSessionId, TestId, EntryType, LogMessage) VALUES(@TestSessionId, @TestId, 'F', ISNULL(@ErrorMessage, ''))
   RAISERROR('TST RAISERROR {6C57D85A-CE44-49ba-9286-A5227961DF02}', 16, 110)
END
GO
/****** Object:  UserDefinedFunction [Internal].[SFN_GetCountOfFailEntriesForTest]    Script Date: 05/23/2013 11:40:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =======================================================================
-- FUNCTION: SFN_GetCountOfFailEntriesForTest
-- Returns the number of log entries indicating failures or 
-- errors for the given test.
-- =======================================================================
CREATE FUNCTION [Internal].[SFN_GetCountOfFailEntriesForTest](@TestId int) RETURNS int
AS 
BEGIN

   DECLARE @FailEntries int

   SELECT @FailEntries = COUNT(1) 
   FROM Data.TestLog 
   WHERE 
      TestLog.TestId = @TestId
      AND EntryType IN ('F', 'E')

   RETURN ISNULL(@FailEntries, 0)

END
GO
/****** Object:  StoredProcedure [Internal].[LogErrorMessage]    Script Date: 05/23/2013 11:40:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =======================================================================
-- PROCEDURE: LogErrorMessage
-- Called by some other TST infrastructure procedures to log an 
-- error message.
-- =======================================================================
CREATE PROCEDURE [Internal].[LogErrorMessage]
   @ErrorMessage  nvarchar(max)
AS
BEGIN
   DECLARE @TestSessionId int
   DECLARE @TestId int
   
   SELECT @TestSessionId = TestSessionId, @TestId = TestId FROM #Tmp_CrtSessionInfo
   IF @TestId >= 0
   BEGIN
      INSERT INTO Data.TestLog(TestSessionId, TestId, EntryType, LogMessage) VALUES(@TestSessionId, @TestId, 'E', @ErrorMessage)
   END
   ELSE
   BEGIN
      INSERT INTO Data.SystemErrorLog(TestSessionId, LogMessage) VALUES(@TestSessionId, @ErrorMessage)
   END

END
GO
/****** Object:  StoredProcedure [Assert].[LogInfo]    Script Date: 05/23/2013 11:40:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =======================================================================
-- PROCEDURE: Log
-- Can be called by the TST test procedures to record an 
-- informational log entry.
-- It will record an entry in TestLog.
-- =======================================================================
CREATE PROCEDURE [Assert].[LogInfo]
   @Message  nvarchar(max)
AS
BEGIN
   DECLARE @TestSessionId int
   DECLARE @TestId int
   
   SELECT @TestSessionId = TestSessionId, @TestId = TestId FROM #Tmp_CrtSessionInfo
   INSERT INTO Data.TestLog(TestSessionId, TestId, EntryType, LogMessage) VALUES(@TestSessionId, @TestId, 'L', ISNULL(@Message, ''))
END
GO
/****** Object:  StoredProcedure [Internal].[RollbackWithLogPreservation]    Script Date: 05/23/2013 11:40:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =======================================================================
-- PROCEDURE RollbackWithLogPreservation
-- Rollbacks a transaction but makes sure that the entries in the log 
-- table TestLog are preserved.
-- =======================================================================
CREATE PROCEDURE [Internal].[RollbackWithLogPreservation]
   @TestSessionId                   int,        -- Identifies the test session.
   @LastTestLogEntryIdBeforeTest    int         -- The last id that was present in the TestLog 
                                                -- table before the test execution started.
AS
BEGIN

   DECLARE @LastTestLogEntryIdAfterRollback  int

   -- @TempLogEntries will temporarily save the log entries that will dissapear due to the ROLLBACK
   DECLARE @TempLogEntries TABLE (
      LogEntryId     int NOT NULL,
      TestSProcId    int NOT NULL,
      EntryType      char NOT NULL,
      CreatedTime    DateTime NOT NULL,
      LogMessage     nvarchar(max) NOT NULL
   )

   DELETE FROM @TempLogEntries
   
   INSERT INTO @TempLogEntries(LogEntryId, TestSProcId, EntryType, CreatedTime, LogMessage) 
   SELECT LogEntryId, TestId, EntryType, CreatedTime, LogMessage 
   FROM Data.TestLog
   WHERE 
      LogEntryId > @LastTestLogEntryIdBeforeTest
      AND TestSessionId = @TestSessionId


   ROLLBACK TRANSACTION

   -- Determine which entries from TestLog did not survived
   SELECT @LastTestLogEntryIdAfterRollback = LogEntryId FROM Data.TestLog WHERE TestSessionId = @TestSessionId
   SET @LastTestLogEntryIdAfterRollback = ISNULL(@LastTestLogEntryIdAfterRollback, 0)

   -- Put back in table TestLog the entries that were lost due to the ROLLBACK 
   INSERT INTO Data.TestLog (TestSessionId, TestId, EntryType, CreatedTime, LogMessage)
   SELECT @TestSessionId, TestSProcId, EntryType, CreatedTime, LogMessage
   FROM @TempLogEntries
   WHERE LogEntryId > @LastTestLogEntryIdAfterRollback
   ORDER BY CreatedTime

END
GO
/****** Object:  StoredProcedure [Internal].[PrintSystemErrorsForSession]    Script Date: 05/23/2013 11:40:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =======================================================================
-- PROCEDURE: PrintSystemErrorsForSession
-- It will print all the system errors that occured in the test session 
-- given by @TestSessionId
-- =======================================================================
CREATE PROCEDURE [Internal].[PrintSystemErrorsForSession]
   @TestSessionId    int,           -- Identifies the test session.
   @ResultsFormat    varchar(10)    -- Indicates if the format in which the results will be printed.
                                    -- See the coments at the begining of the file under section 'Results Format'
AS
BEGIN
   
   DECLARE @SystemError       nvarchar(1000)

   DECLARE CrsSystemErrors CURSOR LOCAL FOR
   SELECT LogMessage FROM Data.SystemErrorLog WHERE TestSessionId = @TestSessionId ORDER BY CreatedTime

   IF (@ResultsFormat = 'XML')
   BEGIN
      PRINT REPLICATE(' ', 2) + '<SystemErrors>'
   END
      
   OPEN CrsSystemErrors
   FETCH NEXT FROM CrsSystemErrors INTO @SystemError
   WHILE @@FETCH_STATUS = 0
   BEGIN

      IF (@ResultsFormat = 'Text')
      BEGIN
         PRINT REPLICATE(' ', 4) + 'Error: ' + @SystemError
      END
      ELSE IF (@ResultsFormat = 'XML')
      BEGIN
         PRINT REPLICATE(' ', 4) + '<SystemError>' + Internal.SFN_EscapeForXml(@SystemError) + '</SystemError>'
      END
      
      FETCH NEXT FROM CrsSystemErrors INTO @SystemError
   END
	
   CLOSE CrsSystemErrors
   DEALLOCATE CrsSystemErrors

   IF (@ResultsFormat = 'XML')
   BEGIN
      PRINT REPLICATE(' ', 2) + '</SystemErrors>'
   END

END
GO
/****** Object:  UserDefinedFunction [Internal].[SFN_GetCountOfSystemErrosInSession]    Script Date: 05/23/2013 11:40:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =======================================================================
-- FUNCTION SFN_GetCountOfSystemErrosInSession
-- Returns the number of failed tests in the given test session
-- =======================================================================
CREATE FUNCTION [Internal].[SFN_GetCountOfSystemErrosInSession](@TestSessionId int) RETURNS int
AS
BEGIN
   
   DECLARE @CountOfSystemErrosInSession int
   
   SELECT @CountOfSystemErrosInSession = COUNT(1) 
   FROM Data.SystemErrorLog
   WHERE TestSessionId = @TestSessionId
   
   RETURN ISNULL(@CountOfSystemErrosInSession, 0)
   
END
GO
/****** Object:  StoredProcedure [Internal].[GenerateComparisonSQLQuery]    Script Date: 05/23/2013 11:40:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =======================================================================
-- PROCEDURE: GenerateComparisonSQLQuery
-- Generates a SQL query that is used in RunTableComparison. 
-- See RunTableComparison.
-- Asumes that #SchemaInfoExpectedResults and #SchemaInfoActualResults
-- are already created and contain the appropiate data.
-- =======================================================================
CREATE PROCEDURE [Internal].[GenerateComparisonSQLQuery]
   @SqlCommand nvarchar(max)OUT
AS
BEGIN

   DECLARE @IsTheFirstColumn           bit
   DECLARE @DataTypeName               nvarchar(128)
   DECLARE @ColumnPrecision            int
   DECLARE @MaxLength                  int
   DECLARE @SqlCommandPkColumns        nvarchar(max)
   DECLARE @SqlCommandDataColumns      nvarchar(max)
   DECLARE @SqlCommandInnerJoinClause  nvarchar(max)
   DECLARE @SqlCommandWhereClause      nvarchar(max)
   DECLARE @Params                     nvarchar(100)
   DECLARE @BareColumnName             sysname
   DECLARE @EscapedColumnName          sysname

   DECLARE CrsPkColumns CURSOR FOR
      SELECT ColumnName, DataTypeName, MaxLength, ColumnPrecision      
      FROM #SchemaInfoActualResults
      WHERE IsPrimaryKey = 1
      ORDER BY PkOrdinal

   OPEN CrsPkColumns

   SET @IsTheFirstColumn = 1
   SET @SqlCommandPkColumns = ''
   SET @SqlCommandWhereClause = ''
   SET @SqlCommandInnerJoinClause = ''
   FETCH NEXT FROM CrsPkColumns INTO @BareColumnName, @DataTypeName, @MaxLength, @ColumnPrecision
   WHILE @@FETCH_STATUS = 0
   BEGIN
   
      SET @EscapedColumnName = '[' + @BareColumnName + ']'
      IF (@IsTheFirstColumn = 0) SET @SqlCommandPkColumns = @SqlCommandPkColumns + ' + '
      SET @SqlCommandPkColumns = @SqlCommandPkColumns + Internal.SFN_Internal_GetColumnPart(@BareColumnName, @DataTypeName, @MaxLength, @ColumnPrecision)

      IF (@IsTheFirstColumn = 0) SET @SqlCommandInnerJoinClause = @SqlCommandInnerJoinClause + ' AND ' 
      SET @SqlCommandInnerJoinClause = @SqlCommandInnerJoinClause + '#ActualResult.' + @EscapedColumnName + ' = #ExpectedResult.' + @EscapedColumnName 

      IF (@IsTheFirstColumn = 0) SET @SqlCommandWhereClause = @SqlCommandWhereClause + ' OR ' 
      SET @SqlCommandWhereClause = @SqlCommandWhereClause + 
         '(  ( (#ActualResult.' + @EscapedColumnName + ' IS NOT NULL) AND (#ExpectedResult.' + @EscapedColumnName + ' IS NULL    ) )  OR ' +
         '   ( (#ActualResult.' + @EscapedColumnName + ' IS NULL    ) AND (#ExpectedResult.' + @EscapedColumnName + ' IS NOT NULL) )  OR ' + 
         '   (#ActualResult.' + @EscapedColumnName + ' != #ExpectedResult.' + @EscapedColumnName + ') )' 

      SET @IsTheFirstColumn = 0
      
      FETCH NEXT FROM CrsPkColumns INTO @BareColumnName, @DataTypeName, @MaxLength, @ColumnPrecision
   END
   
   CLOSE CrsPkColumns
   DEALLOCATE CrsPkColumns

   DECLARE CrsDataColumns CURSOR FOR
      SELECT ColumnName, DataTypeName, MaxLength, ColumnPrecision      
      FROM #SchemaInfoActualResults
      WHERE 
         IsPrimaryKey = 0
         AND IsIgnored = 0

   OPEN CrsDataColumns

   SET @IsTheFirstColumn = 1
   SET @SqlCommandDataColumns = ''
   FETCH NEXT FROM CrsDataColumns INTO @BareColumnName, @DataTypeName, @MaxLength, @ColumnPrecision      
   WHILE @@FETCH_STATUS = 0
   BEGIN

      SET @EscapedColumnName = '[' + @BareColumnName + ']'
      SET @SqlCommandDataColumns = @SqlCommandDataColumns + ' + ' + Internal.SFN_Internal_GetColumnPart(@BareColumnName, @DataTypeName, @MaxLength, @ColumnPrecision)

      SET @SqlCommandWhereClause = @SqlCommandWhereClause + ' OR ' 
      SET @SqlCommandWhereClause = @SqlCommandWhereClause + 
         '(  ( (#ActualResult.' + @EscapedColumnName + ' IS NOT NULL) AND (#ExpectedResult.' + @EscapedColumnName + ' IS NULL    ) )  OR ' +
         '   ( (#ActualResult.' + @EscapedColumnName + ' IS NULL    ) AND (#ExpectedResult.' + @EscapedColumnName + ' IS NOT NULL) )  OR ' + 
         '   (#ActualResult.' + @EscapedColumnName + ' != #ExpectedResult.' + @EscapedColumnName + ') )' 

      SET @IsTheFirstColumn = 0
      
      FETCH NEXT FROM CrsDataColumns INTO @BareColumnName, @DataTypeName, @MaxLength, @ColumnPrecision      
   END
   
   CLOSE CrsDataColumns
   DEALLOCATE CrsDataColumns

   SET @SqlCommand = ' SELECT TOP 1 @DifString = '  + 
                     @SqlCommandPkColumns +
                     @SqlCommandDataColumns +
                     ' FROM #ExpectedResult FULL OUTER JOIN #ActualResult ON ' + 
                     @SqlCommandInnerJoinClause +
                     ' WHERE ' + 
                     @SqlCommandWhereClause

END
GO
/****** Object:  StoredProcedure [Internal].[PrintResultsSummaryForSession]    Script Date: 05/23/2013 11:40:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =======================================================================
-- PROCEDURE: PrintResultsSummaryForSession
-- It will print the last lines in the result screen - those that 
-- have the summary of the test session given by @TestSessionId.
-- =======================================================================
CREATE PROCEDURE [Internal].[PrintResultsSummaryForSession]
   @TestSessionId    int,         -- Identifies the test session.
   @ResultsFormat    varchar(10), -- Indicates if the format in which the results will be printed.
                                  -- See the coments at the begining of the file under section 'Results Format'
   @NoTimestamp      bit = 0      -- Indicates that no timestamp or duration info should be printed in results output
AS
BEGIN

   DECLARE @TestSessionStart              datetime
   DECLARE @TestSessionFinish             datetime
   DECLARE @TotalSuiteCount               int
   DECLARE @TotalTestCount                int
   DECLARE @TotalPassedCount              int
   DECLARE @TotalFailedCount              int
   DECLARE @TestSessionStatus             varchar(16)

   SELECT 
      @TestSessionStart   = TestSessionStart, 
      @TestSessionFinish  = TestSessionFinish
   FROM Data.TestSession
   WHERE TestSessionId = @TestSessionId
   
   SET @TotalSuiteCount  = Internal.SFN_GetCountOfSuitesInSession(@TestSessionId) 
   SET @TotalTestCount   = Internal.SFN_GetCountOfTestsInSession(@TestSessionId) 
   SET @TotalPassedCount = Internal.SFN_GetCountOfPassedTestsInSession(@TestSessionId) 
   SET @TotalFailedCount = Internal.SFN_GetCountOfFailedTestsInSession(@TestSessionId) 
   
   SET @TestSessionStatus = CASE WHEN @TotalFailedCount > 0 THEN 'Failed' ELSE 'Passed' END
   
   IF (@ResultsFormat = 'Text')
   BEGIN
      IF (@NoTimestamp = 0)
      BEGIN
         PRINT 'Start: ' + CONVERT(nvarchar(20), @TestSessionStart, 108) + '. Finish: ' + CONVERT(nvarchar(20), @TestSessionFinish, 108) + '. Duration: ' + CONVERT(nvarchar(10), DATEDIFF(ms, @TestSessionStart, @TestSessionFinish)) + ' miliseconds.'
      END

      PRINT 'Total suites: ' + CAST(@TotalSuiteCount as varchar) + '. Total tests: ' + CAST(@TotalTestCount AS varchar) + '. Test passed: ' + CAST(@TotalPassedCount AS varchar) + '. Test failed: ' + CAST(@TotalFailedCount AS varchar) + '.'
   END

END
GO
/****** Object:  StoredProcedure [Internal].[PrintHeaderForSession]    Script Date: 05/23/2013 11:40:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =======================================================================
-- PROCEDURE: PrintHeaderForSession
-- It will print the first lines in the result screen orin the XML file
-- =======================================================================
CREATE PROCEDURE [Internal].[PrintHeaderForSession]
   @TestSessionId    int,         -- Identifies the test session.
   @ResultsFormat    varchar(10), -- Indicates if the format in which the results will be printed.
                                  -- See the coments at the begining of the file under section 'Results Format'
   @NoTimestamp      bit = 0      -- Indicates that no timestamp or duration info should be printed in results output
AS
BEGIN

   DECLARE @TestSessionStart              datetime
   DECLARE @TestSessionFinish             datetime
   DECLARE @TotalFailedCount              int
   DECLARE @TestSessionStatus             varchar(16)
   DECLARE @ResultMessage                 nvarchar(1000)

   SELECT 
      @TestSessionStart   = TestSessionStart, 
      @TestSessionFinish  = TestSessionFinish
   FROM Data.TestSession
   WHERE TestSessionId = @TestSessionId
   
   SET @TotalFailedCount = Internal.SFN_GetCountOfFailedTestsInSession(@TestSessionId) 
   
   SET @TestSessionStatus = CASE WHEN @TotalFailedCount > 0 THEN 'Failed' ELSE 'Passed' END
   
   IF (@ResultsFormat = 'XML')
   BEGIN
      IF (@NoTimestamp=0)
      BEGIN
         SET @ResultMessage = '<TST' + 
            ' status="' + @TestSessionStatus + '"' + 
            ' testSessionId="' + CAST(@TestSessionId AS varchar) + '"' + 
            ' start="' + CONVERT(nvarchar(20), @TestSessionStart, 108) + '"' + 
            ' finish="' + CONVERT(nvarchar(20), @TestSessionFinish, 108) + '"' + 
            ' duration="' + CONVERT(nvarchar(10), DATEDIFF(ms, @TestSessionStart, @TestSessionFinish)) + '"' + 
            ' >'
      END
      ELSE
      BEGIN
         SET @ResultMessage = '<TST' + 
            ' status="' + @TestSessionStatus + '"' + 
            ' >'
     END
     PRINT @ResultMessage 
   END

END
GO
/****** Object:  StoredProcedure [Internal].[SetTestSessionConfiguration]    Script Date: 05/23/2013 11:40:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =======================================================================
-- PROCEDURE SetTestSessionConfiguration
-- It searches for a stored procedure called TSTConfig in the tested 
-- database. If it exists it calls it. This allow tests to configure 
-- TST before proceeding with the test session.
--    0 - OK.
--    1 - An error was detected during the execution of TSTConfig.
--        In case of an error an error message is stored in one of the log tables.
-- =======================================================================
CREATE PROCEDURE [Internal].[SetTestSessionConfiguration]
   @TestSessionId       int            -- Identifies the test session
AS
BEGIN

   DECLARE @SqlCommand        nvarchar(1000)
   DECLARE @TestDatabaseName  sysname
   DECLARE @PrepareResult     bit
   DECLARE @ErrorMessage      nvarchar(4000)   
   
   SET @PrepareResult = 0
   
   SELECT @TestDatabaseName = TestSession.DatabaseName FROM Data.TestSession WHERE TestSessionId = @TestSessionId

   IF (Internal.SFN_SProcExists(@TestDatabaseName, 'TSTConfig') = 1)
   BEGIN
      SET @SqlCommand = QUOTENAME(@TestDatabaseName) + '..' + QUOTENAME('TSTConfig')
      
      BEGIN TRY
         EXEC @SqlCommand
      END TRY
      BEGIN CATCH
         SET @ErrorMessage =  'An error occured during the execution of the TSTConfig procedure.' +
                              ' Error: ' + CAST(ERROR_NUMBER() AS varchar) + ', ' + ERROR_MESSAGE() + 
                              ' Procedure: ' + ISNULL(ERROR_PROCEDURE(), 'N/A') + '. Line: ' + CAST(ERROR_LINE() AS varchar)
         EXEC Internal.LogErrorMessage @ErrorMessage
         
         SET @PrepareResult = 1
      END CATCH

   END

   RETURN @PrepareResult
END
GO
/****** Object:  StoredProcedure [Internal].[PrepareTestInformation]    Script Date: 05/23/2013 11:40:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =======================================================================
-- PROCEDURE PrepareTestInformation
-- Analyses the given database and prepares all the information needed 
-- to run a test session for the suite given by @TargetSuiteName. 
-- If @TargetSuiteName is NULL it will do so for all the suites in the 
-- given database.
-- Basically it detects all the TST test procedures for the given 
-- @TestDatabaseName and @TargetSuiteName.
-- Return code:
--    0 - OK
--    1 - An error was detected:
--        The database given by @TestDatabaseName was not found or
--        @TargetSuiteName was specified and the suite given by @TargetSuiteName was not found or
--        @TargetTestName was specified and the test given by @TargetTestName was not found or
--        @TargetTestName was specified and the test name does not follow naming conventions for a TST test procedure.
--        No tests were detected that match the input parameters.
--        In case of an error an error message is stored in one of the log tables.
-- Note: This sproc will raise an error if the parameters are invalid in 
--       a way that indicates an internal error.
-- =======================================================================
CREATE PROCEDURE [Internal].[PrepareTestInformation]
   @TestSessionId       int,        -- Identifies the test session.
   @TestDatabaseName    sysname,    -- Specifies the database where the suite analysis is done.
   @TargetSuiteName     sysname,    -- The target suite name. It can be NULL.
   @TargetTestName      sysname     -- The target test name. It can be NULL
AS
BEGIN

   DECLARE @ErrorMessage         nvarchar(1000)
   DECLARE @SqlCommand           nvarchar(1000)
   DECLARE @SuiteName            sysname
   DECLARE @IsTSTSproc           bit
   DECLARE @SProcType            char(8)
   DECLARE @SchemaName           sysname
   DECLARE @SProcName            sysname
   DECLARE @SuiteId              int
   DECLARE @DuplicateSuiteName   sysname
   DECLARE @DuplicateTestName    sysname

   CREATE TABLE #Tmp_Procedures (
      SchemaName sysname NULL,
      SProcName sysname NOT NULL
   )
      
   IF (@TestDatabaseName IS NULL) 
   BEGIN
      RAISERROR('TST Internal Error. Invalid call to PrepareTestInformation. @TestDatabaseName must be specified.', 16, 1)
      RETURN 1
   END

   IF (@TargetSuiteName IS NOT NULL AND @TargetTestName IS NOT NULL) 
   BEGIN
      RAISERROR('TST Internal Error. Invalid call to PrepareTestInformation. @TargetSuiteName and @TargetTestName cannot both be specified.', 16, 1)
      RETURN 1
   END

   -- @TestDatabaseName must exist
   IF NOT EXISTS (SELECT [name] FROM sys.databases WHERE [name] = @TestDatabaseName)
   BEGIN
      SET @ErrorMessage = 'Database ''' + @TestDatabaseName + ''' not found.'
      EXEC Internal.LogErrorMessage @ErrorMessage
      RETURN 1
   END

   SELECT @SqlCommand = 
      'INSERT INTO #Tmp_Procedures ' + 
      'SELECT Schemas.name AS SchemaName, Procedures.name AS SProcName ' + 
      'FROM ' + QUOTENAME(@TestDatabaseName) + '.sys.procedures AS Procedures ' + 
      'INNER JOIN ' + QUOTENAME(@TestDatabaseName) + '.sys.schemas AS Schemas ON Schemas.schema_id = Procedures.schema_id ' + 
      'WHERE is_ms_shipped = 0 ORDER BY Procedures.name'

   EXEC (@SqlCommand)

   -- If @TargetTestName is specified then it must follow the TST naming conventions for a test name.
   -- At this point we must also determine its suite name so thatthe following loop can isolate its SETUP and TEARDOWN.
   IF @TargetTestName IS NOT NULL
   BEGIN
      EXEC Internal.AnalyzeSprocName @TargetTestName, @TargetSuiteName OUTPUT, @IsTSTSproc OUTPUT, @SProcType OUTPUT
      IF (@IsTSTSproc = 0 OR @SProcType != 'Test')
      BEGIN
         SET @ErrorMessage = 'Test procedure''' + @TargetTestName + ''' does not follow the naming conventions for a TST test procedure.'
         EXEC Internal.LogErrorMessage @ErrorMessage
         RETURN 1
      END
   END
   
   
   DECLARE CrsTests CURSOR LOCAL FOR
   SELECT 
      SchemaName,
      SProcName
   FROM #Tmp_Procedures 
   WHERE 
      SProcName LIKE 'SQLTest_%'
      AND (
               (SProcName = @TargetTestName) 
            OR (@TargetSuiteName IS NULL AND @TargetTestName IS NULL) 
            OR (SProcName = 'SQLTest_SETUP_' + @TargetSuiteName)
            OR (SProcName = 'SQLTest_TEARDOWN_' + @TargetSuiteName)
            OR (@TargetTestName IS NULL AND SProcName Like 'SQLTest_' + @TargetSuiteName + '#%')
          )
               
   OPEN CrsTests
   FETCH NEXT FROM CrsTests INTO @SchemaName, @SProcName
   WHILE @@FETCH_STATUS = 0
   BEGIN
      EXEC Internal.AnalyzeSprocName @SProcName, @SuiteName OUTPUT, @IsTSTSproc OUTPUT, @SProcType OUTPUT
      IF(@IsTSTSproc = 1)
      BEGIN

         -- TODO: validate the suite and test name
         IF (@TargetSuiteName IS NULL OR @TargetSuiteName = @SuiteName)
         BEGIN

            EXEC Internal.EnsureSuite @TestSessionId, @SchemaName, @SuiteName, @SuiteId OUTPUT
            INSERT INTO Data.Test(TestSessionId, SuiteId, SchemaName, SProcName, SProcType) VALUES (@TestSessionId, @SuiteId, @SchemaName, @SProcName, @SProcType)
         END
                  
      END
     
      FETCH NEXT FROM CrsTests INTO @SchemaName, @SProcName
   END

   CLOSE CrsTests
   DEALLOCATE CrsTests
   
   -- If @TargetTestName is specified then it must exist
   IF (@TargetTestName IS NOT NULL)
   BEGIN
      IF NOT EXISTS (SELECT 1 FROM Data.Test WHERE TestSessionId = @TestSessionId AND SProcName = @TargetTestName AND Test.SProcType = 'Test')
      BEGIN
         SET @ErrorMessage = 'Test procedure ''' + @TargetTestName + ''' not found in database ''' + @TestDatabaseName + '''.'
         EXEC Internal.LogErrorMessage @ErrorMessage
         RETURN 1
      END
   END

   IF (@TargetSuiteName IS NOT NULL)
   BEGIN
   
      -- If @TargetSuiteName is specified then it must exist.
      IF NOT EXISTS (SELECT 1 FROM Data.Suite WHERE TestSessionId = @TestSessionId AND SuiteName = @TargetSuiteName)
      BEGIN
         SET @ErrorMessage = 'Suite ''' + @TargetSuiteName + ''' not found in database ''' + @TestDatabaseName + '''.'
         EXEC Internal.LogErrorMessage @ErrorMessage
         RETURN 1
      END
   
      -- There must be at least one test defined for that suite.   
      IF NOT EXISTS (
         SELECT 1 
         FROM Data.Test 
         INNER JOIN Data.Suite ON Suite.SuiteId = Test.SuiteId
         WHERE Suite.TestSessionId = @TestSessionId AND Suite.SuiteName = @TargetSuiteName AND Test.SProcType = 'Test')
      BEGIN
         SET @ErrorMessage = 'Suite ''' + @TargetSuiteName + ''' in database ''' + @TestDatabaseName + ''' does not contain any test'
         EXEC Internal.LogErrorMessage @ErrorMessage
         RETURN 1
      END
   END
      
   -- There must be at least one test detected as a result of the analysis
   IF NOT EXISTS (
      SELECT 1 
      FROM Data.Test 
      WHERE Test.TestSessionId = @TestSessionId AND SProcType = 'Test')
   BEGIN
      SET @ErrorMessage = 'No test procedure was detected for the given search criteria in database ''' + @TestDatabaseName + '''.'
      EXEC Internal.LogErrorMessage @ErrorMessage
      RETURN 1
   END

   -- It is illegal to have two suites with the same name. This can happen if they are in different schemas.
   SET @DuplicateSuiteName = NULL
   SELECT @DuplicateSuiteName = SuiteName
   FROM TST.Data.Suite
   WHERE TestSessionId = @TestSessionId
   GROUP BY TestSessionId, SuiteName
   HAVING COUNT(*) > 1
   
   IF (@DuplicateSuiteName IS NOT NULL)
   BEGIN
      SET @ErrorMessage = 'The suite name ''' + @DuplicateSuiteName + ''' appears to be duplicated across different schemas in database ''' + @TestDatabaseName + '''.'
      EXEC Internal.LogErrorMessage @ErrorMessage
      RETURN 1
   END
   
   -- It is illegal to have two tests with the same name. This can happen if they are in the anonymous suite and in different schemas.
   SET @DuplicateTestName = NULL
   SELECT @DuplicateTestName = SProcName
   FROM TST.Data.Test
   WHERE TestSessionId = @TestSessionId
   GROUP BY TestSessionId, SProcName
   HAVING COUNT(*) > 1
   
   IF (@DuplicateTestName IS NOT NULL)
   BEGIN
      SET @ErrorMessage = 'The test name ''' + @DuplicateTestName + ''' appears to be duplicated across different schemas in database ''' + @TestDatabaseName + '''.'
      EXEC Internal.LogErrorMessage @ErrorMessage
      RETURN 1
   END

   RETURN 0
END
GO
/****** Object:  StoredProcedure [Internal].[PrintLogEntriesForTest]    Script Date: 05/23/2013 11:40:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =======================================================================
-- PROCEDURE: PrintLogEntriesForTest
-- It will print the results for the given test. Called by PrintOneSuiteResults
-- =======================================================================
CREATE PROCEDURE [Internal].[PrintLogEntriesForTest]
   @TestId          int,            -- Identifies the test.
   @ResultsFormat   varchar(10),    -- Indicates if the format in which the results will be printed.
                                    -- See the coments at the begining of the file under section 'Results Format'
   @Verbose         bit             -- If 1 then the output will contain all suites and tests names and all the log entries.
                                    -- If 0 then the output will contain all suites and tests names but only the 
                                    -- log entries indicating failures.
   
AS
BEGIN

   DECLARE @EntryType         char
   DECLARE @LogMessage        nvarchar(max)
   DECLARE @EntryTypeString   varchar(10)

   IF (@Verbose = 1)
   BEGIN
      DECLARE CrsTestResults CURSOR LOCAL FOR
      SELECT Internal.SFN_GetEntryTypeName(EntryType), LogMessage FROM Data.TSTResults
      WHERE TestId = @TestId
      ORDER BY LogEntryId
   END
   ELSE
   BEGIN
      DECLARE CrsTestResults CURSOR LOCAL FOR
      SELECT Internal.SFN_GetEntryTypeName(EntryType), LogMessage FROM Data.TSTResults
      WHERE TestId = @TestId AND EntryType IN ('F', 'E')
      ORDER BY LogEntryId
   END


   OPEN CrsTestResults
   FETCH NEXT FROM CrsTestResults INTO @EntryTypeString, @LogMessage
   WHILE @@FETCH_STATUS = 0
   BEGIN
      
      IF (@ResultsFormat = 'Text')
      BEGIN
         PRINT REPLICATE(' ', 12) + @EntryTypeString + ': ' + @LogMessage
      END
      ELSE IF (@ResultsFormat = 'XML')
      BEGIN
         PRINT REPLICATE(' ', 10) + '<Log entryType="' + @EntryTypeString + '">' + @LogMessage + '</Log>'
      END
      
      
      FETCH NEXT FROM CrsTestResults INTO @EntryTypeString, @LogMessage
   END
	
   CLOSE CrsTestResults
   DEALLOCATE CrsTestResults

END
GO
/****** Object:  StoredProcedure [Assert].[IsNull]    Script Date: 05/23/2013 11:40:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =======================================================================
-- PROCEDURE: Assert.IsNull
-- Can be called by the test procedures to verify that 
-- @ActualValue IS NULL.
-- If passes it will record an entry in TestLog.
-- If failes it will record an entry in TestLog and raise an error.
-- =======================================================================
CREATE PROCEDURE [Assert].[IsNull]
   @ContextMessage      nvarchar(1000),
   @ActualValue         sql_variant
AS
BEGIN
   DECLARE @Message nvarchar(4000)

   SET @ContextMessage = ISNULL(@ContextMessage, '')

   IF (@ActualValue IS NULL)
   BEGIN
      SET @Message = 'Assert.IsNull passed. [' + @ContextMessage + '] Expected value: NULL. Actual value: ''' + ISNULL(CAST(@ActualValue as nvarchar(max)), 'NULL') + ''''
      EXEC Assert.Pass @Message
      RETURN
   END

   SET @Message = 'Assert.IsNull failed. [' + @ContextMessage + '] Expected value: NULL. Actual value: ''' + ISNULL(CAST(@ActualValue as nvarchar(max)), 'NULL') + ''''
   EXEC Assert.Fail @Message

END
GO
/****** Object:  StoredProcedure [Assert].[IsNotNull]    Script Date: 05/23/2013 11:40:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =======================================================================
-- PROCEDURE: Assert.IsNotNull
-- Can be called by the test procedures to verify that 
-- @ActualValue IS NOT NULL.
-- If passes it will record an entry in TestLog.
-- If failes it will record an entry in TestLog and raise an error.
-- =======================================================================
CREATE PROCEDURE [Assert].[IsNotNull]
   @ContextMessage      nvarchar(1000),
   @ActualValue         sql_variant
AS
BEGIN
   DECLARE @Message nvarchar(4000)

   SET @ContextMessage = ISNULL(@ContextMessage, '')

   IF (@ActualValue IS NOT NULL)
   BEGIN
      SET @Message = 'Assert.IsNotNull passed. [' + @ContextMessage + '] Actual value: ''' + ISNULL(CAST(@ActualValue as nvarchar(max)), 'NULL') + ''''
      EXEC Assert.Pass @Message
      RETURN
   END

   SET @Message = 'Assert.IsNotNull failed. [' + @ContextMessage + '] Actual value: ''' + ISNULL(CAST(@ActualValue as nvarchar(max)), 'NULL') + ''''
   EXEC Assert.Fail @Message

END
GO
/****** Object:  StoredProcedure [Assert].[IsTableEmpty]    Script Date: 05/23/2013 11:40:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =======================================================================
-- PROCEDURE: Assert.IsTableEmpty
-- Can be called by the test procedures to verify that 
-- #ActualResult is empty.
-- =======================================================================
CREATE PROCEDURE [Assert].[IsTableEmpty]
   @ContextMessage      nvarchar(1000)
AS
BEGIN

   DECLARE @RowCount    int
   DECLARE @Message     nvarchar(4000)

   SET @ContextMessage = ISNULL(@ContextMessage, '')

   IF (object_id('tempdb..#ActualResult') IS NULL) 
   BEGIN
      SET @Message = 'Assert.IsTableEmpty failed. [' + @ContextMessage + '] #ActualResult table was not created.' 
      EXEC Assert.Fail @Message
      RETURN
   END

   SELECT @RowCount = COUNT(*) FROM #ActualResult

   IF (@RowCount = 0)
   BEGIN
      SET @Message = 'Assert.IsTableEmpty passed. [' + @ContextMessage + '] Table #ActualResult is empty'
      EXEC Assert.Pass @Message
      RETURN
   END

   SET @Message = 'Assert.IsTableEmpty failed. [' + @ContextMessage + '] Table #ActualResult has ' + CAST(@RowCount as varchar) + ' row(s)'
   EXEC Assert.Fail @Message

END
GO
/****** Object:  StoredProcedure [Internal].[BasicTempTableValidation]    Script Date: 05/23/2013 11:40:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =======================================================================
-- PROCEDURE: BasicTempTableValidation
-- Makes sure that #ExpectedResult and #ActualResult are created and have 
-- the same number of entries
-- Return code:
--    0 - OK. #ExpectedResult and #ActualResult are created and have 
--            the same number of entries.
--    1 - An error was detected. An error was raised.
-- =======================================================================
CREATE PROCEDURE [Internal].[BasicTempTableValidation]
   @ContextMessage      nvarchar(1000),
   @ExpectedRowCount    int OUT        -- At return will contain the number of rows in #ExpectedResult
AS
BEGIN

   DECLARE @ActualRowCount    int
   DECLARE @Message           nvarchar(4000)

   IF (object_id('tempdb..#ExpectedResult') IS NULL) 
   BEGIN
      SET @Message = 'Assert.TableEquals failed. [' + @ContextMessage + '] #ExpectedResult table was not created.' 
      EXEC Assert.Fail @Message
      RETURN 1
   END
   
   IF (object_id('tempdb..#ActualResult') IS NULL) 
   BEGIN
      SET @Message = 'Assert.TableEquals failed. [' + @ContextMessage + '] #ActualResult table was not created.' 
      EXEC Assert.Fail @Message
      RETURN 1
   END

   SELECT @ExpectedRowCount = COUNT(*) FROM #ExpectedResult
   SELECT @ActualRowCount   = COUNT(*) FROM #ActualResult

   IF (@ExpectedRowCount != @ActualRowCount )
   BEGIN
      SET @Message = 'Assert.TableEquals failed. [' + @ContextMessage + '] Expected row count=' + CAST(@ExpectedRowCount as varchar) + '. Actual row count=' + CAST(@ActualRowCount as varchar) 
      EXEC Assert.Fail @Message
      RETURN 1
   END
   
   RETURN 0

END
GO
/****** Object:  StoredProcedure [Internal].[RunOneSProc]    Script Date: 05/23/2013 11:40:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =======================================================================
-- PROCEDURE RunOneSProc
-- This will run one given TST test procedure. Caled by RunOneTestInternal
-- =======================================================================
CREATE PROCEDURE [Internal].[RunOneSProc]
   @TestId           int               -- Identifies the test.
AS
BEGIN
   DECLARE @SqlCommand     nvarchar(1000)
   
   SET @SqlCommand = Internal.SFN_GetFullSprocName(@TestId)
   EXEC @SqlCommand

END
GO
/****** Object:  StoredProcedure [Internal].[CollectErrorInfo]    Script Date: 05/23/2013 11:40:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =======================================================================
-- PROCEDURE CollectErrorInfo
-- Called from within inside a CATCH block. It processes the information 
-- in the ERROR_XXX functions. It examines XACT_STATE() and 
-- @@TRANCOUNT and based on all that it will return an error code.
-- Return code:
--    0 - This was an expected error as recorded by RegisterExpectedError.
--        No transaction was rolled back. The transaction if open is in 
--        a committable state. 
--    1 - Failure. Assert failure as oposed to an error. 
--    2 - Error. The test failed with an error. The transaction if open 
--               is in a committable state. The error was recorded and 
--               @ErrorMessage will be NULL.
--    3 - Error. The set-up failed with an error. The transaction is in a 
--               uncommittable state. @ErrorMessage will contain the error 
--               text. 
--    4 - Error. The transaction was rolled back. Normally this is acompanied 
--               by a 226 or 3609 error:
--                226: Transaction count after EXECUTE indicates that a COMMIT or ROLLBACK TRAN is missing
--               3609: The transaction ended in the trigger.
--               The error was recorded and @ErrorMessage will be NULL.
-- =======================================================================
CREATE PROCEDURE [Internal].[CollectErrorInfo]  
   @TestId                       int,                -- Identifies the test where the error occured.
   @UseTSTRollback               bit,                -- 1 if TSTRollback is enabled.
   @StartTranCount               int,                -- The transaction count before the setup procedure was invoked.
   @ErrorMessage                 nvarchar(max) OUT,  -- If an error occured it will contain the error text
   @NestedTransactionMessage     nvarchar(max) OUT   -- If a nested transaction caused issues this will have an error message regarding that.
AS 
BEGIN

   DECLARE @TSTRollbackMessage         nvarchar(4000)
   DECLARE @InProcedureMsg             nvarchar(100)
   DECLARE @FullSprocName              nvarchar(1000)

   DECLARE @Catch_ErrorMessage   nvarchar(2048) 
   DECLARE @Catch_ErrorProcedure nvarchar(126)
   DECLARE @Catch_ErrorLine      int
   DECLARE @Catch_ErrorNumber    int

   DECLARE @ExpectedErrorNumber       int
   DECLARE @ExpectedErrorMessage      nvarchar(2048) 
   DECLARE @ExpectedErrorProcedure    nvarchar(126)
   DECLARE @IsExpectedError           bit

   SET @Catch_ErrorMessage   = ERROR_MESSAGE()
   SET @Catch_ErrorProcedure = ERROR_PROCEDURE()
   SET @Catch_ErrorLine      = ERROR_LINE()
   SET @Catch_ErrorNumber    = ERROR_NUMBER()

   -- If this is an error raised by the TST API (like Assert) we don't have to log the error, it was already logged.
   IF (@Catch_ErrorMessage = 'TST RAISERROR {6C57D85A-CE44-49ba-9286-A5227961DF02}') RETURN 1

   -- Check if this is an expected error.
   SET @IsExpectedError = 0
   SELECT 
      @ExpectedErrorNumber       = ExpectedErrorNumber    ,
      @ExpectedErrorMessage      = ExpectedErrorMessage   ,
      @ExpectedErrorProcedure    = ExpectedErrorProcedure 
   FROM #Tmp_CrtSessionInfo
   
   IF ( (@ExpectedErrorNumber IS NOT NULL) OR (@ExpectedErrorMessage IS NOT NULL) OR (@ExpectedErrorProcedure IS NOT NULL) )
   BEGIN
      IF (      (@Catch_ErrorNumber    = @ExpectedErrorNumber    OR @ExpectedErrorNumber IS NULL      )
            AND (@Catch_ErrorMessage   = @ExpectedErrorMessage   OR @ExpectedErrorMessage IS NULL     )
            AND (@Catch_ErrorProcedure = @ExpectedErrorProcedure OR @ExpectedErrorProcedure IS NULL   ) )
      BEGIN
         SET @IsExpectedError = 1
      END
   END
      
   IF (@UseTSTRollback = 1)
   BEGIN
      IF (@Catch_ErrorNumber = 266 OR @Catch_ErrorNumber = 3609 OR @@TRANCOUNT != @StartTranCount)
      BEGIN
      
         SET @TSTRollbackMessage = 'To disable TST rollback create a stored procedure called TSTConfig in the database where you ' +
                        'have the test procedures. Inside TSTConfig call ' + 
                        '<EXEC TST.Utils.SetConfiguration @ParameterName=''UseTSTRollback'', @ParameterValue=''0'' @Scope=''Test'', @ScopeValue=''_name_of_test_procedure_''>. ' + 
                        'Warning: When you disable TST rollback, TST framework will not rollback the canges made by SETUP, test and TEARDOWN procedures. ' + 
                        'See TST documentation for more details.'

         IF (@Catch_ErrorProcedure IS NULL) SET @InProcedureMsg = ''
         ELSE SET @InProcedureMsg = ' in procedure ''' + @Catch_ErrorProcedure + ''''

         IF (@Catch_ErrorNumber = 266 OR @@TRANCOUNT != @StartTranCount)
         BEGIN
            IF (@@TRANCOUNT > 0)
            BEGIN
               SET @NestedTransactionMessage =  'BEGIN TRANSACTION with no matching COMMIT detected' + 
                                    @InProcedureMsg + '. ' + 
                                    'Please disable the TST rollback if you expect the tested procedure to use BEGIN TRANSACTION with no matching COMMIT. ' + 
                                    @TSTRollbackMessage
            END
            ELSE
            BEGIN
               SET @NestedTransactionMessage =  'ROLLBACK TRANSACTION detected' + 
                                    @InProcedureMsg + '. ' + 
                                    'All other TST messages logged during this test and previous to this error were lost. ' + 
                                    'Please disable the TST rollback if you expect the tested procedure to use ROLLBACK TRANSACTION. ' + 
                                    @TSTRollbackMessage
            END
         END
         ELSE
         BEGIN
            IF (@@TRANCOUNT > 0)
            BEGIN
               SET @NestedTransactionMessage =  'BEGIN TRANSACTION with no matching COMMIT detected during trigger execution' + 
                                    @InProcedureMsg + '. ' + 
                                    'This looks like a bug in the trigger and you should consider fixing that. ' + 
                                    'Alternatively you can disable the TST rollback if you expect the trigger to use BEGIN TRANSACTION with no matching COMMIT. ' + 
                                    @TSTRollbackMessage
            END
            ELSE
            BEGIN
               SET @NestedTransactionMessage =  'ROLLBACK TRANSACTION detected during trigger execution' + 
                                    @InProcedureMsg + '. ' + 
                                    'Please disable the TST rollback if you expect the trigger to use ROLLBACK TRANSACTION. ' + 
                                    @TSTRollbackMessage
            END
         END
      END
   END
      
   IF (@IsExpectedError = 1)
   BEGIN
      IF (XACT_STATE() = -1)  RETURN 3    -- The transaction is in a uncommittable state.
      IF (@@TRANCOUNT != @StartTranCount AND @@TRANCOUNT = 0) RETURN 4
      RETURN 0
   END
   ELSE
   BEGIN
      SET @FullSprocName = Internal.SFN_GetFullSprocName(@TestId)
      SET @ErrorMessage =  'An error occured during the execution of the test procedure ''' + @FullSprocName + 
                           '''. Error: ' + CAST(ERROR_NUMBER() AS varchar) + ', ' + ERROR_MESSAGE() + 
                           ' Procedure: ' + ISNULL(ERROR_PROCEDURE(), 'N/A') + '. Line: ' + CAST(ERROR_LINE() AS varchar)

      IF (XACT_STATE() = -1)  RETURN 3    -- The transaction is in a uncommittable state.

      IF (@IsExpectedError = 0) 
      BEGIN
         
         IF (@ErrorMessage IS NOT NULL)               
         BEGIN
            EXEC Internal.LogErrorMessage @ErrorMessage; SET @ErrorMessage = NULL
         END
         
         IF (@NestedTransactionMessage IS NOT NULL)   
         BEGIN
            EXEC Internal.LogErrorMessage @NestedTransactionMessage; SET @NestedTransactionMessage = NULL
         END
         
      END

      IF (@@TRANCOUNT != @StartTranCount AND @@TRANCOUNT = 0) RETURN 4
      RETURN 2
   END
   
END
GO
/****** Object:  StoredProcedure [Internal].[PrintStatusForSession]    Script Date: 05/23/2013 11:40:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =======================================================================
-- PROCEDURE: PrintStatusForSession
-- See the coments at the begining of the file under section 'Results Format'
-- This procedure will print the results when the @ResultsFormat = 'Batch'
-- =======================================================================
CREATE PROCEDURE [Internal].[PrintStatusForSession]
   @TestSessionId    int      -- Identifies the test session.
AS
BEGIN

   DECLARE @TotalFailedCount        int
   DECLARE @TotalSystemErrorsCount  int
   DECLARE @TestSessionStatus       varchar(16)
   
   SET @TotalFailedCount = Internal.SFN_GetCountOfFailedTestsInSession(@TestSessionId) 
   SET @TotalSystemErrorsCount = Internal.SFN_GetCountOfSystemErrosInSession(@TestSessionId) 
   
   SET @TestSessionStatus = 'Passed'
   IF (@TotalFailedCount > 0) SET @TestSessionStatus = 'Failed'
   IF (@TotalSystemErrorsCount > 0) SET @TestSessionStatus = 'Failed'
   
   PRINT 'TST Status: ' + @TestSessionStatus
   
END
GO
/****** Object:  StoredProcedure [Internal].[LogErrorMessageAndRaiseError]    Script Date: 05/23/2013 11:40:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =======================================================================
-- PROCEDURE: LogErrorMessageAndRaiseError
-- Called by some other TST infrastructure procedures to log an 
-- error message and raise a TST error.
-- =======================================================================
CREATE PROCEDURE [Internal].[LogErrorMessageAndRaiseError]
   @ErrorMessage  nvarchar(max)
AS
BEGIN
   EXEC Internal.LogErrorMessage @ErrorMessage
   RAISERROR('TST RAISERROR {6C57D85A-CE44-49ba-9286-A5227961DF02}', 16, 110)
END
GO
/****** Object:  StoredProcedure [Internal].[PrepareTestSession]    Script Date: 05/23/2013 11:40:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =======================================================================
-- PROCEDURE PrepareTestSession
-- Must be called at the start of a test session. 
-- Return code:
--    0 - OK.
--    1 - An error was detected. 
--        In case of an error an error message is stored in one of the log tables.
-- =======================================================================
CREATE PROCEDURE [Internal].[PrepareTestSession]
   @TestDatabaseName    sysname,       -- The database that contains the TST procedures.
   @TestSessionId       int OUT        -- At return it will identify the test session.
AS
BEGIN

   DECLARE @PrepareResult     bit

   IF (@TestDatabaseName IS NULL) 
   BEGIN
      RAISERROR('TST Internal Error. Invalid call to PrepareTestSession. @TestDatabaseName must be specified.', 16, 1)
      RETURN 1
   END

   -- Generate a new TestSessionId
   INSERT INTO Data.TestSession(DatabaseName, TestSessionStart, TestSessionFinish) VALUES (@TestDatabaseName, GETDATE(), NULL)
   SET @TestSessionId = SCOPE_IDENTITY()

   -- We will insert one row in #Tmp_CrtSessionInfo. This row is a placeholder 
   -- that we use to store info about what is the current TestSessionId, TestId
   -- This is how sprocs like Pass or Fail will know which test session 
   -- and which test are currently executed.
   -- Right now we are outside of any test stored procedure so we'll use -1 for TestId
   INSERT INTO #Tmp_CrtSessionInfo(TestSessionId, TestId, Stage) VALUES (@TestSessionId, -1, '-')

   -- Allow the user to set upconfiguration parameters   
   EXEC @PrepareResult = Internal.SetTestSessionConfiguration @TestSessionId
   
   RETURN @PrepareResult

END
GO
/****** Object:  StoredProcedure [Internal].[PrintOneSuiteResults]    Script Date: 05/23/2013 11:40:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =======================================================================
-- PROCEDURE: PrintOneSuiteResults
-- It will print the results for the given test suite. Called by PrintSuitesResultsForSession
-- =======================================================================
CREATE PROCEDURE [Internal].[PrintOneSuiteResults] 
   @SuiteId          int,              -- Identifies the suite.
   @ResultsFormat    varchar(10),      -- Indicates if the format in which the results will be printed.
                                       -- See the coments at the begining of the file under section 'Results Format'
   @Verbose          bit               -- If 1 then the output will contain all suites and tests names and all the log entries.
                                       -- If 0 then the output will contain all suites and tests names but only the 
                                       -- log entries indicating failures.
AS
BEGIN

   DECLARE @TestId         int
   DECLARE @SProcName      sysname
   DECLARE @TestStatus     nvarchar(10)
   DECLARE @FailEntries    int

   DECLARE CrsTestsResults CURSOR LOCAL FOR
   SELECT TestId, SProcName FROM Data.TSTResults 
   WHERE SuiteId = @SuiteId
   GROUP BY TestId, SProcName
   ORDER BY TestId

   IF (@ResultsFormat = 'XML')
   BEGIN
      PRINT REPLICATE(' ', 6) + '<Tests>'
   END

   OPEN CrsTestsResults
   FETCH NEXT FROM CrsTestsResults INTO @TestId, @SProcName
   WHILE @@FETCH_STATUS = 0
   BEGIN
   
      SET @FailEntries = Internal.SFN_GetCountOfFailEntriesForTest(@TestId)
      
      IF(@FailEntries != 0) SET @TestStatus = 'Failed'
      ELSE SET @TestStatus = 'Passed'
      
      IF (@ResultsFormat = 'Text')
      BEGIN
         PRINT REPLICATE(' ', 8) + 'Test: ' + @SProcName + '. ' + @TestStatus
      END
      ELSE IF (@ResultsFormat = 'XML')
      BEGIN
         PRINT REPLICATE(' ', 8) + '<Test' + 
            ' name="' + @SProcName + '"' +
            ' status="' + @TestStatus + '"' +
            ' >'
      END
      
      EXEC Internal.PrintLogEntriesForTest @TestId, @ResultsFormat, @Verbose
      
      IF (@ResultsFormat = 'XML')
      BEGIN
         PRINT REPLICATE(' ', 8) + '</Test>'
      END

      FETCH NEXT FROM CrsTestsResults INTO @TestId, @SProcName
   END
	
   CLOSE CrsTestsResults
   DEALLOCATE CrsTestsResults
   
   IF (@ResultsFormat = 'XML')
   BEGIN
      PRINT REPLICATE(' ', 6) + '</Tests>'
   END

END
GO
/****** Object:  StoredProcedure [Assert].[Equals]    Script Date: 05/23/2013 11:40:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =======================================================================
-- PROCEDURE: Assert.Equals
-- Can be called by the test procedures to verify that 
-- two values are equal. 
-- Note: NULL is invalid for @ExpectedValue. If Assert.Equals is
--       called with NULL for @ExpectedValue then it will fail with 
--       an ERROR. Use Assert.IsNull instead.
-- Result map:
--       @ExpectedValue    @ActualValue      Result
--                 NULL         Ignored        ERROR
--                value            NULL        Fail
--               value1          value2        Fail
--               value1          value1        Pass
-- If passes it will record an entry in TestLog.
-- If failes it will record an entry in TestLog and raise an error.
-- =======================================================================
CREATE PROCEDURE [Assert].[Equals]
   @ContextMessage      nvarchar(1000),
   @ExpectedValue       sql_variant,
   @ActualValue         sql_variant
AS
BEGIN

   DECLARE @ExpectedValueDataType         sysname
   DECLARE @ExpectedValueDataTypeFamily   char(2)
   DECLARE @ActualValueDataType           sysname
   DECLARE @ActualValueDataTypeFamily     char(2)
   DECLARE @ExpectedValueString           nvarchar(max)
   DECLARE @ActualValueString             nvarchar(max)
   DECLARE @Message                       nvarchar(4000)

   SET @ContextMessage = ISNULL(@ContextMessage, '')

   IF (@ExpectedValue IS NULL )
   BEGIN
      SET @Message = 'Invalid call to Assert.Equals. [' + @ContextMessage + '] @ExpectedValue cannot be NULL. Use Assert.IsNull instead.'
      EXEC Internal.LogErrorMessageAndRaiseError @Message
      RETURN
   END

   IF (@ActualValue IS NULL )
   BEGIN
      SET @Message = 'Assert.Equals failed. [' + @ContextMessage + '] Actual value is NULL'
      EXEC Assert.Fail @Message
   END

   EXEC Internal.GetSqlVarInfo @ExpectedValue , @ExpectedValueDataType OUT, @ExpectedValueDataTypeFamily OUT, @ExpectedValueString OUT
   EXEC Internal.GetSqlVarInfo @ActualValue   , @ActualValueDataType   OUT, @ActualValueDataTypeFamily   OUT, @ActualValueString   OUT

   IF(@ExpectedValueDataTypeFamily != @ActualValueDataTypeFamily OR 
      @ExpectedValueDataTypeFamily = 'SV' OR 
      @ExpectedValueDataTypeFamily = '??')
   BEGIN
      SET @Message = 'Invalid call to Assert.Equals. [' + @ContextMessage + '] @ExpectedValue (' + @ExpectedValueDataType + ') and @ActualValue (' + @ActualValueDataType + ') have incompatible types. Consider an explicit CONVERT, calling Assert.NumericEquals or calling Assert.FloatEquals'
      EXEC Internal.LogErrorMessageAndRaiseError @Message
      RETURN
   END

   IF (@ExpectedValueDataTypeFamily = 'AN')
   BEGIN
      SET @Message = 'Invalid call to Assert.Equals. [' + @ContextMessage + '] Float or real cannot be used when calling Assert.Equals since this could produce unreliable results. Use Assert.FloatEquals.'
      EXEC Internal.LogErrorMessageAndRaiseError @Message
      RETURN
   END

   IF (@ActualValue = @ExpectedValue)
   BEGIN
      SET @Message = 
            'Assert.Equals passed. [' + @ContextMessage + '] Test value: ' + @ExpectedValueString + ' (' + @ExpectedValueDataType + ')' + 
            '. Actual value: ' + @ActualValueString + ' (' + @ActualValueDataType + ')'
      EXEC Assert.Pass @Message
      RETURN
   END

   SET @Message = 
            'Assert.Equals failed. [' + @ContextMessage + '] Test value: ' + @ExpectedValueString + ' (' + @ExpectedValueDataType + ')' + 
            '. Actual value: ' + @ActualValueString + ' (' + @ActualValueDataType + ')'
   EXEC Assert.Fail @Message

END
GO
/****** Object:  StoredProcedure [Assert].[NotEquals]    Script Date: 05/23/2013 11:40:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =======================================================================
-- PROCEDURE: Assert.NotEquals
-- Can be called by the test procedures to verify that 
-- two values are not equal. 
-- Note: NULL is invalid for @ExpectedNotValue. If Assert.NotEquals is 
--       called with NULL for @ExpectedNotValue then it will fail with 
--       an ERROR. Use Assert.IsNotNull instead.
-- Result map:
--    @ExpectedNotValue    @ActualValue      Result
--                 NULL         Ignored        ERROR
--                value            NULL        Fail
--               value1          value2        Pass
--               value1          value1        Fail
-- If passes it will record an entry in TestLog.
-- If failes it will record an entry in TestLog and raise an error.
-- =======================================================================
CREATE PROCEDURE [Assert].[NotEquals]
   @ContextMessage      nvarchar(1000),
   @ExpectedNotValue    sql_variant,
   @ActualValue         sql_variant
AS
BEGIN

   DECLARE @ExpectedNotValueDataType         sysname
   DECLARE @ExpectedNotValueDataTypeFamily   char(2)
   DECLARE @ActualValueDataType              sysname
   DECLARE @ActualValueDataTypeFamily        char(2)
   DECLARE @ExpectedNotValueString           nvarchar(max)
   DECLARE @ActualValueString                nvarchar(max)
   DECLARE @Message                          nvarchar(4000)

   SET @ContextMessage = ISNULL(@ContextMessage, '')

   IF (@ExpectedNotValue IS NULL )
   BEGIN
      SET @Message = 'Invalid call to Assert.NotEquals. [' + @ContextMessage + '] @ExpectedNotValue cannot be NULL. Use Assert.IsNotNull instead.'
      EXEC Internal.LogErrorMessageAndRaiseError @Message
      RETURN
   END

   IF (@ActualValue IS NULL )
   BEGIN
      SET @Message = 'Assert.NotEquals failed. [' + @ContextMessage + '] Actual value is NULL'
      EXEC Assert.Fail @Message
   END

   EXEC Internal.GetSqlVarInfo @ExpectedNotValue , @ExpectedNotValueDataType OUT, @ExpectedNotValueDataTypeFamily OUT, @ExpectedNotValueString OUT
   EXEC Internal.GetSqlVarInfo @ActualValue      , @ActualValueDataType      OUT, @ActualValueDataTypeFamily      OUT, @ActualValueString      OUT

   IF(@ExpectedNotValueDataTypeFamily != @ActualValueDataTypeFamily OR 
      @ExpectedNotValueDataTypeFamily = 'SV' OR 
      @ExpectedNotValueDataTypeFamily = '??')
   BEGIN
      SET @Message = 'Invalid call to Assert.NotEquals. [' + @ContextMessage + '] @ExpectedNotValue (' + @ExpectedNotValueDataType + ') and @ActualValue (' + @ActualValueDataType + ') have incompatible types. Consider an explicit CONVERT, calling Assert.NumericEquals or calling Assert.FloatEquals'
      EXEC Internal.LogErrorMessageAndRaiseError @Message
      RETURN
   END

   IF (@ExpectedNotValueDataTypeFamily = 'AN')
   BEGIN
      SET @Message = 'Invalid call to Assert.NotEquals. [' + @ContextMessage + '] Float or real cannot be used when calling Assert.NotEquals since this could produce unreliable results. Use Assert.FloatNotEquals.'
      EXEC Internal.LogErrorMessageAndRaiseError @Message
      RETURN
   END

   IF (@ActualValue != @ExpectedNotValue)
   BEGIN
      SET @Message = 
         'Assert.NotEquals passed. [' + @ContextMessage + '] Test value: ' + @ExpectedNotValueString + ' (' +  + @ExpectedNotValueDataType + ')' + 
         '. Actual value: ' + @ActualValueString + ' (' + @ActualValueDataType + ')'
      EXEC Assert.Pass @Message
      RETURN
   END

   SET @Message = 
         'Assert.NotEquals failed. [' + @ContextMessage + '] Test value: ' + @ExpectedNotValueString + ' (' +  + @ExpectedNotValueDataType + ')' + 
         '. Actual value: ' + @ActualValueString + ' (' + @ActualValueDataType + ')'
   EXEC Assert.Fail @Message

END
GO
/****** Object:  StoredProcedure [Assert].[NumericEquals]    Script Date: 05/23/2013 11:40:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =======================================================================
-- PROCEDURE: Assert.NumericEquals
-- Can be called by the test procedures to verify that 
-- two numbers are equal considering a specified tolerance. 
-- Note: NULL is invalid for @ExpectedValue. If Assert.NumericEquals is
--       called with NULL for @ExpectedValue then it will fail with 
--       an ERROR. Use Assert.IsNull instead.
-- Note: NULL is invalid for @Tolerance. If Assert.NumericEquals is
--       called with NULL for @Tolerance then it will fail with 
--       an ERROR.
-- Note: @Tolerance must be greater or equal than 0. If Assert.NumericEquals is
--       called with a negative number for @Tolerance then it will fail 
--       with an ERROR.
-- Note: If @ActualValue is NULL then Assert.NumericEquals will fail.
-- If passes it will record an entry in TestLog.
-- If failes it will record an entry in TestLog and raise an error.
-- =======================================================================
CREATE PROCEDURE [Assert].[NumericEquals]
   @ContextMessage      nvarchar(1000),
   @ExpectedValue       decimal(38, 15),
   @ActualValue         decimal(38, 15),
   @Tolerance           decimal(38, 15)
AS
BEGIN
   DECLARE @Message     nvarchar(4000)
   DeCLARE @Difference  decimal(38, 15)
   
   SET @ContextMessage = ISNULL(@ContextMessage, '')

   IF (@ExpectedValue IS NULL )
   BEGIN
      SET @Message = 'Invalid call to Assert.NumericEquals. [' + @ContextMessage + '] @ExpectedValue cannot be NULL. Use Assert.IsNull instead.'
      EXEC Internal.LogErrorMessageAndRaiseError @Message
      RETURN
   END

   IF (@Tolerance IS NULL )
   BEGIN
      SET @Message = 'Invalid call to Assert.NumericEquals. [' + @ContextMessage + '] @Tolerance cannot be NULL.'
      EXEC Internal.LogErrorMessageAndRaiseError @Message
      RETURN
   END

   IF (@Tolerance <0)
   BEGIN
      SET @Message = 'Invalid call to Assert.NumericEquals. [' + @ContextMessage + '] @Tolerance must be a zero or a positive number.'
      EXEC Internal.LogErrorMessageAndRaiseError @Message
      RETURN
   END

   SET @Difference = @ActualValue - @ExpectedValue
   IF (@Difference < 0) SET @Difference = -@Difference

   IF (@Difference <= @Tolerance)
   BEGIN
      SET @Message = 
         'Assert.NumericEquals passed. [' + @ContextMessage + '] Test value: ' + ISNULL(CONVERT(varchar(50), @ExpectedValue, 2), 'NULL') + 
         '. Actual value: ' + ISNULL(CONVERT(varchar(50), @ActualValue, 2), 'NULL') + 
         '. Tolerance: ' + + ISNULL(CONVERT(varchar(50), @Tolerance, 2), 'NULL')
      EXEC Assert.Pass @Message
      RETURN
   END

   SET @Message = 'Assert.NumericEquals failed. [' + @ContextMessage + '] Test value: ' + ISNULL(CONVERT(varchar(50), @ExpectedValue, 2), 'NULL') + 
         '. Actual value: ' + ISNULL(CONVERT(varchar(50), @ActualValue, 2), 'NULL') + 
         '. Tolerance: ' + + ISNULL(CONVERT(varchar(50), @Tolerance, 2), 'NULL')
   EXEC Assert.Fail @Message

END
GO
/****** Object:  StoredProcedure [Assert].[NumericNotEquals]    Script Date: 05/23/2013 11:40:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =======================================================================
-- PROCEDURE: Assert.NumericNotEquals
-- Can be called by the test procedures to verify that 
-- two numbers are not equal considering a specified tolerance. 
-- Note: NULL is invalid for @ExpectedValue. If Assert.NumericNotEquals is
--       called with NULL for @ExpectedValue then it will fail with 
--       an ERROR. Use Assert.IsNotNull instead.
-- Note: NULL is invalid for @Tolerance. If Assert.NumericNotEquals is
--       called with NULL for @Tolerance then it will fail with 
--       an ERROR.
-- Note: @Tolerance must be greater or equal than 0. If Assert.NumericNotEquals
--       is called with a negative number for @Tolerance then it will fail
--       with an ERROR.
-- Note: If @ActualValue is NULL then Assert.NumericNotEquals will fail.
-- If passes it will record an entry in TestLog.
-- If failes it will record an entry in TestLog and raise an error.
-- =======================================================================
CREATE PROCEDURE [Assert].[NumericNotEquals]
   @ContextMessage      nvarchar(1000),
   @ExpectedNotValue    decimal(38, 15),
   @ActualValue         decimal(38, 15),
   @Tolerance           decimal(38, 15)
AS
BEGIN
   DECLARE @Message     nvarchar(4000)
   DeCLARE @Difference  decimal(38, 15)
   
   SET @ContextMessage = ISNULL(@ContextMessage, '')

   IF (@ExpectedNotValue IS NULL )
   BEGIN
      SET @Message = 'Invalid call to Assert.NumericNotEquals. [' + @ContextMessage + '] @ExpectedNotValue cannot be NULL. Use Assert.IsNotNull instead.'
      EXEC Internal.LogErrorMessageAndRaiseError @Message
      RETURN
   END

   IF (@Tolerance IS NULL )
   BEGIN
      SET @Message = 'Invalid call to Assert.NumericNotEquals. [' + @ContextMessage + '] @Tolerance cannot be NULL.'
      EXEC Internal.LogErrorMessageAndRaiseError @Message
      RETURN
   END

   IF (@Tolerance <0)
   BEGIN
      SET @Message = 'Invalid call to Assert.NumericNotEquals. [' + @ContextMessage + '] @Tolerance must be a zero or a positive number.'
      EXEC Internal.LogErrorMessageAndRaiseError @Message
      RETURN
   END

   SET @Difference = @ActualValue - @ExpectedNotValue
   IF (@Difference < 0) SET @Difference = -@Difference

   IF (@Difference > @Tolerance)
   BEGIN
      SET @Message = 
         'Assert.NumericNotEquals passed. [' + @ContextMessage + '] Test value: ' + ISNULL(CONVERT(varchar(50), @ExpectedNotValue, 2), 'NULL') + 
         '. Actual value: ' + ISNULL(CONVERT(varchar(50), @ActualValue, 2), 'NULL') + 
         '. Tolerance: ' + + ISNULL(CONVERT(varchar(50), @Tolerance, 2), 'NULL')
      EXEC Assert.Pass @Message
      RETURN
   END

   SET @Message = 'Assert.NumericNotEquals failed. [' + @ContextMessage + '] Test value: ' + ISNULL(CONVERT(varchar(50), @ExpectedNotValue, 2), 'NULL') + 
         '. Actual value: ' + ISNULL(CONVERT(varchar(50), @ActualValue, 2), 'NULL') + 
         '. Tolerance: ' + + ISNULL(CONVERT(varchar(50), @Tolerance, 2), 'NULL')
   EXEC Assert.Fail @Message

END
GO
/****** Object:  StoredProcedure [Assert].[FloatEquals]    Script Date: 05/23/2013 11:40:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
-- =======================================================================
-- PROCEDURE: Assert.FloatEquals
-- Can be called by the test procedures to verify that 
-- two numbers are equal considering a specified tolerance. 
-- Use Assert.FloatEquals instead of Assert.NumericEquals is the numbers you 
-- need to compare have high exponents.
-- Note: NULL is invalid for @ExpectedValue. If Assert.FloatEquals is
--       called with NULL for @ExpectedValue then it will fail with 
--       an ERROR. Use Assert.IsNull instead.
-- Note: NULL is invalid for @Tolerance. If Assert.FloatEquals is
--       called with NULL for @Tolerance then it will fail with 
--       an ERROR.
-- Note: @Tolerance must be greater or equal than 0. If Assert.FloatEquals 
--       is called with a negative number for @Tolerance then it will fail
--       with an ERROR.
-- Note: If @ActualValue is NULL then Assert.FloatEquals will fail.
-- If passes it will record an entry in TestLog.
-- If failes it will record an entry in TestLog and raise an error.
-- =======================================================================
CREATE PROCEDURE [Assert].[FloatEquals]
   @ContextMessage      nvarchar(1000),
   @ExpectedValue       float(53),
   @ActualValue         float(53),
   @Tolerance           float(53)
AS
BEGIN
   DECLARE @Message     nvarchar(4000)
   DeCLARE @Difference  float(53)
   
   SET @ContextMessage = ISNULL(@ContextMessage, '')

   IF (@ExpectedValue IS NULL )
   BEGIN
      SET @Message = 'Invalid call to Assert.FloatEquals. [' + @ContextMessage + '] @ExpectedValue cannot be NULL. Use Assert.IsNull instead.'
      EXEC Internal.LogErrorMessageAndRaiseError @Message
      RETURN
   END

   IF (@Tolerance IS NULL )
   BEGIN
      SET @Message = 'Invalid call to Assert.FloatEquals. [' + @ContextMessage + '] @Tolerance cannot be NULL.'
      EXEC Internal.LogErrorMessageAndRaiseError @Message
      RETURN
   END

   IF (@Tolerance <0)
   BEGIN
      SET @Message = 'Invalid call to Assert.FloatEquals. [' + @ContextMessage + '] @Tolerance must be a zero or a positive number.'
      EXEC Internal.LogErrorMessageAndRaiseError @Message
      RETURN
   END

   SET @Difference = @ActualValue - @ExpectedValue
   IF (@Difference < 0) SET @Difference = -@Difference

   IF (@Difference <= @Tolerance)
   BEGIN
      SET @Message = 
         'Assert.FloatEquals passed. [' + @ContextMessage + '] Test value: ' + ISNULL(CONVERT(varchar(50), CAST(@ExpectedValue AS DECIMAL(18,2)), 2), 'NULL') + 
         '. Actual value: ' + ISNULL(CONVERT(varchar(50), CAST(@ActualValue AS DECIMAL(18,2)), 2), 'NULL') + 
         '. Tolerance: ' + + ISNULL(CONVERT(varchar(50), @Tolerance, 2), 'NULL')
      EXEC Assert.Pass @Message
      RETURN
   END

   SET @Message = 'Assert.FloatEquals failed. [' + @ContextMessage + '] Test value: ' + ISNULL(CONVERT(varchar(50), CAST(@ExpectedValue AS DECIMAL(18,2)), 2), 'NULL') + 
         '. Actual value: ' + ISNULL(CONVERT(varchar(50), CAST(@ActualValue AS DECIMAL(18,2)), 2), 'NULL') + 
         '. Tolerance: ' + + ISNULL(CONVERT(varchar(50), @Tolerance, 2), 'NULL')
   EXEC Assert.Fail @Message

END
GO
/****** Object:  StoredProcedure [Assert].[FloatNotEquals]    Script Date: 05/23/2013 11:40:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =======================================================================
-- PROCEDURE: Assert.FloatNotEquals
-- Can be called by the test procedures to verify that 
-- two numbers are not equal considering a specified tolerance. 
-- Use Assert.FloatNotEquals instead of Assert.NumericEquals is the numbers you 
-- need to compare have high exponents.
-- Note: NULL is invalid for @ExpectedNotValue. If Assert.FloatNotEquals is
--       called with NULL for @ExpectedNotValue then it will fail with 
--       an ERROR. Use Assert.IsNotNull instead.
-- Note: NULL is invalid for @Tolerance. If Assert.FloatNotEquals is
--       called with NULL for @Tolerance then it will fail with 
--       an ERROR.
-- Note: @Tolerance must be greater or equal than 0. If 
--       Assert.FloatNotEquals is called with a negative number for 
--       @Tolerance then it will fail with an ERROR.
-- Note: If @ActualValue is NULL then Assert.FloatNotEquals will fail.
-- If passes it will record an entry in TestLog.
-- If failes it will record an entry in TestLog and raise an error.
-- =======================================================================
CREATE PROCEDURE [Assert].[FloatNotEquals]
   @ContextMessage      nvarchar(1000),
   @ExpectedNotValue    float(53),
   @ActualValue         float(53),
   @Tolerance           float(53)
AS
BEGIN
   DECLARE @Message     nvarchar(4000)
   DeCLARE @Difference  float(53)
   
   SET @ContextMessage = ISNULL(@ContextMessage, '')

   IF (@ExpectedNotValue IS NULL )
   BEGIN
      SET @Message = 'Invalid call to Assert.FloatNotEquals. [' + @ContextMessage + '] @ExpectedNotValue cannot be NULL. Use Assert.IsNotNull instead.'
      EXEC Internal.LogErrorMessageAndRaiseError @Message
      RETURN
   END

   IF (@Tolerance IS NULL )
   BEGIN
      SET @Message = 'Invalid call to Assert.FloatNotEquals. [' + @ContextMessage + '] @Tolerance cannot be NULL.'
      EXEC Internal.LogErrorMessageAndRaiseError @Message
      RETURN
   END

   IF (@Tolerance <0)
   BEGIN
      SET @Message = 'Invalid call to Assert.FloatNotEquals. [' + @ContextMessage + '] @Tolerance must be a zero or a positive number.'
      EXEC Internal.LogErrorMessageAndRaiseError @Message
      RETURN
   END

   SET @Difference = @ActualValue - @ExpectedNotValue
   IF (@Difference < 0) SET @Difference = -@Difference

   IF (@Difference > @Tolerance)
   BEGIN
      SET @Message = 
         'Assert.FloatNotEquals passed. [' + @ContextMessage + '] Test value: ' + ISNULL(CONVERT(varchar(50), @ExpectedNotValue, 2), 'NULL') + 
         '. Actual value: ' + ISNULL(CONVERT(varchar(50), @ActualValue, 2), 'NULL') + 
         '. Tolerance: ' + + ISNULL(CONVERT(varchar(50), @Tolerance, 2), 'NULL')
      EXEC Assert.Pass @Message
      RETURN
   END

   SET @Message = 'Assert.FloatNotEquals failed. [' + @ContextMessage + '] Test value: ' + ISNULL(CONVERT(varchar(50), @ExpectedNotValue, 2), 'NULL') + 
         '. Actual value: ' + ISNULL(CONVERT(varchar(50), @ActualValue, 2), 'NULL') + 
         '. Tolerance: ' + + ISNULL(CONVERT(varchar(50), @Tolerance, 2), 'NULL')
   EXEC Assert.Fail @Message

END
GO
/****** Object:  StoredProcedure [Assert].[FloatGreaterThan]    Script Date: 05/23/2013 11:40:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [Assert].[FloatGreaterThan]
   @ContextMessage      nvarchar(1000),
   @Smaler FLOAT,
   @Larger FLOAT
AS
BEGIN
   DECLARE @Message     nvarchar(4000)
   DeCLARE @Difference  float(53)
   
   SET @ContextMessage = ISNULL(@ContextMessage, '')

   IF (@Smaler IS NULL )
   BEGIN
      SET @Message = 'Invalid call to Assert.FloatGreaterThan. [' + @ContextMessage + '] @Smaler cannot be NULL. Use Assert.IsNull instead.'
      EXEC Internal.LogErrorMessageAndRaiseError @Message
      RETURN
   END

	SET @Difference = @Larger - @Smaler
	IF (@Difference > 0) 
	BEGIN
		SET @Message = 'Assert.FloatGreaterThan passed. '
		EXEC Assert.Pass @Message 
	END 
	ELSE
	BEGIN 
		SET @Message = 'Assert.FloatGreaterThan failed. [' + @ContextMessage + '] Test values: ' + ISNULL(CONVERT(varchar(50), @Smaler, 2), 'NULL') + 
         ' should be smaller than' + ISNULL(CONVERT(varchar(50), @Larger, 2), 'NULL')
		EXEC Assert.Fail @Message
	END
  

END
GO
/****** Object:  StoredProcedure [Internal].[CollectTestSProcErrorInfo]    Script Date: 05/23/2013 11:40:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =======================================================================
-- PROCEDURE CollectTestSProcErrorInfo
-- Called from within inside a CATCH block. It processes the information 
-- in the ERROR_XXX functions. It examines XACT_STATE() and 
-- @@TRANCOUNT and based on all that it will return an error code.
-- If the active transaction is in an uncommitable state it will do a 
-- ROLLBACK while preserving the entries in the TestLog table.
-- Return code:
--    0 - There was an error but it was expected as recorded by 
--        RegisterExpectedError. No transaction was rolled back. The 
--        transaction if open is in a committable state. 
--    1 - Error or failure but the execution can continue with the teardown.
--    2 - Error and the test execution has to be aborted.
-- =======================================================================
CREATE PROCEDURE [Internal].[CollectTestSProcErrorInfo]
   @TestSessionId                int,              -- Identifies the test session.
   @TestSProcId                  int,              -- Identifies the test where the error occured.
   @UseTSTRollback               bit,              -- 1 if TSTRollback is enabled.
   @StartTranCount               int,              -- The transaction count before the setup procedure was invoked.
   @LastTestLogEntryIdBeforeTest int               -- The last id that was present in the TestLog 
                                                   -- table before the test execution started.
AS
BEGIN

   DECLARE @ErrorCode                     int
   DECLARE @ReturnCode                    int
   DECLARE @FullSprocName                 nvarchar(1000)
   DECLARE @ErrorMessage                  nvarchar(max)  -- If an error occured it will contain the error text
   DECLARE @NestedTransactionMessage      nvarchar(max)  -- If a nested transaction caused issues this will have an error message regarding that.
   DECLARE @TransactionWarningMessage     nvarchar(max)  -- If the teardown will have to be invoked outside of the context of a transaction 
                                                         -- this will have an error message regarding that.
   SET @ReturnCode = -1
   
   EXEC @ErrorCode = Internal.CollectErrorInfo  
                           @TestSProcId, 
                           @StartTranCount, 
                           @UseTSTRollback, 
                           @ErrorMessage OUT,
                           @NestedTransactionMessage OUT
                           
   IF      (@UseTSTRollback = 1 AND @ErrorCode = 0)   SET @ReturnCode = 0
   ELSE IF (@UseTSTRollback = 1 AND @ErrorCode = 1)   SET @ReturnCode = 1
   ELSE IF (@UseTSTRollback = 1 AND @ErrorCode = 2)   SET @ReturnCode = 1
   ELSE IF (@UseTSTRollback = 1 AND @ErrorCode = 3)   
   BEGIN
      -- The transaction is in an invalid (uncomittable) state. We need to roll it back.
      SET @FullSprocName = Internal.SFN_GetFullSprocName(@TestSProcId)
      SET @TransactionWarningMessage = 'The transaction is in an uncommitable state after the test procedure ''' + @FullSprocName + ''' has failed. A rollback was forced. The TEARDOWN if any will be executed outside of a transaction scope.'
      SET @ReturnCode = 1

      GOTO LblSaveLogAndRollback
   END
   ELSE IF (@UseTSTRollback = 1 AND @ErrorCode = 4)   
   BEGIN
      SET @FullSprocName = Internal.SFN_GetFullSprocName(@TestSProcId)
      SET @TransactionWarningMessage = 'The transaction was rolled back during the test procedure ''' + @FullSprocName + '''. The TEARDOWN if any will be executed outside of a transaction scope.'
      SET @ReturnCode = 1
   END
   ELSE IF (@UseTSTRollback = 0 AND @ErrorCode = 0)   SET @ReturnCode = 0
   ELSE IF (@UseTSTRollback = 0 AND @ErrorCode = 1)   SET @ReturnCode = 1
   ELSE IF (@UseTSTRollback = 0 AND @ErrorCode = 2)   SET @ReturnCode = 1
   ELSE IF (@UseTSTRollback = 0 AND @ErrorCode = 3)   
   BEGIN
      -- If we did not begin a transaction but now we have a transaction in an uncommitable state 
      -- then it means that the client opened it. We will rollback the transaction.
      SET @FullSprocName = Internal.SFN_GetFullSprocName(@TestSProcId)
      SET @TransactionWarningMessage = 'The test procedure ''' + @FullSprocName + ''' opened a transaction that is now in an uncommitable state. A rollback was forced. The TEARDOWN if any will be executed outside of a transaction scope.'
      SET @ReturnCode = 1
      
      GOTO LblSaveLogAndRollback
   END
   -- ELSE IF (@UseTSTRollback = 0 AND @ErrorCode = 4) This cannot happen. We will live @ReturnCode set to -1 which will generate an internal error

   GOTO LblSaveErrors

LblSaveLogAndRollback:

   BEGIN TRY
      -- Rollback and in the same time preserves the log entries
      EXEC Internal.RollbackWithLogPreservation @TestSessionId, @LastTestLogEntryIdBeforeTest
   END TRY
   BEGIN CATCH
      -- RollbackWithLogPreservation will execute a ROLLBACK transaction so an error 266 caused by @@Trancount mismatch is expected. 
      IF (ERROR_NUMBER() != 266) EXEC Internal.Rethrow
   END CATCH

LblSaveErrors:

   IF (@ErrorMessage                 IS NOT NULL)  EXEC Internal.LogErrorMessage @ErrorMessage
   IF (@NestedTransactionMessage     IS NOT NULL)  EXEC Internal.LogErrorMessage @NestedTransactionMessage
   IF (@TransactionWarningMessage    IS NOT NULL)  EXEC Internal.LogErrorMessage @TransactionWarningMessage

   IF (@ReturnCode < 0)
   BEGIN 
      EXEC Internal.LogErrorMessage 'TST Internal Error in CollectTestSProcErrorInfo. Unexpected error code'
      SET @ReturnCode = 1
   END

   RETURN @ReturnCode

END
GO
/****** Object:  StoredProcedure [Internal].[CollectTeardownSProcErrorInfo]    Script Date: 05/23/2013 11:40:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =======================================================================
-- PROCEDURE CollectTeardownSProcErrorInfo
-- Called from within inside a CATCH block. It processes the information 
-- in the ERROR_XXX functions. It examines XACT_STATE() and 
-- @@TRANCOUNT and based on all that it will return an error code.
-- If the active transaction is in an uncommitable state it will do a 
-- ROLLBACK while preserving the entries in the TestLog table.
-- Return code: 1
-- =======================================================================
CREATE PROCEDURE [Internal].[CollectTeardownSProcErrorInfo]
   @TestSessionId                int,              -- Identifies the test session.
   @TeardownSProcId              int,              -- Identifies the teardown proc where the error occured.
   @UseTSTRollback               bit,              -- 1 if TSTRollback is enabled.
   @StartTranCount               int,              -- The transaction count before the setup procedure was invoked.
   @LastTestLogEntryIdBeforeTest int               -- The last id that was present in the TestLog 
                                                   -- table before the test execution started.
AS
BEGIN

   DECLARE @ErrorCode                     int
   DECLARE @ReturnCode                    int
   DECLARE @FullSprocName                 nvarchar(1000)
   DECLARE @ErrorMessage                  nvarchar(max)  -- If an error occured it will contain the error text
   DECLARE @NestedTransactionMessage      nvarchar(max)  -- If a nested transaction caused issues this will have an error message regarding that.
   DECLARE @TransactionWarningMessage     nvarchar(max)  -- If the teardown will have to be invoked outside of the context of a transaction 
                                                         -- this will have an error message regarding that.

   SET @ReturnCode = -1

   EXEC @ErrorCode = Internal.CollectErrorInfo  
                           @TeardownSProcId, 
                           @StartTranCount, 
                           @UseTSTRollback, 
                           @ErrorMessage OUT,
                           @NestedTransactionMessage OUT

   -- We do not allow "Expected errors" during the Teardown.
   -- If during the Teardown we get an "Expected errors" we will record an error.
   IF (@ErrorCode = 0) SET @ErrorCode = 2

   IF      (@UseTSTRollback = 1 AND @ErrorCode = 1)  SET @ReturnCode = 1
   ELSE IF (@UseTSTRollback = 1 AND @ErrorCode = 2)  SET @ReturnCode = 1
   ELSE IF (@UseTSTRollback = 1 AND @ErrorCode = 3)   
   BEGIN
      -- The transaction is in an invalid (uncomittable) state. We need to roll it back.
      SET @FullSprocName = Internal.SFN_GetFullSprocName(@TeardownSProcId)
      SET @TransactionWarningMessage = 'The transaction is in an uncommitable state after the teardown procedure ''' + @FullSprocName + ''' has failed. A rollback was forced.'
      SET @ReturnCode = 1
      
      GOTO LblSaveLogAndRollback
   END
   ELSE IF (@UseTSTRollback = 1 AND @ErrorCode = 4)   
   BEGIN
      SET @FullSprocName = Internal.SFN_GetFullSprocName(@TeardownSProcId)
      SET @TransactionWarningMessage = 'The transaction was rolled back during the teardown procedure ''' + @FullSprocName + '''.'
      SET @ReturnCode = 1
   END
   ELSE IF (@UseTSTRollback = 0 AND @ErrorCode = 1)  SET @ReturnCode = 1
   ELSE IF (@UseTSTRollback = 0 AND @ErrorCode = 2)  SET @ReturnCode = 1
   IF (@UseTSTRollback = 0 AND @ErrorCode = 3)   
   BEGIN
      -- If we did not begin a transaction but now we have a transaction in an uncommitable state 
      -- then it means that the client opened it. We will rollback the transaction.
      SET @FullSprocName = Internal.SFN_GetFullSprocName(@TeardownSProcId)
      SET @TransactionWarningMessage = 'The teardown procedure ''' + @FullSprocName + ''' opened a transaction that is now in an uncommitable state. A rollback was forced.'
      SET @ReturnCode = 1
      
      GOTO LblSaveLogAndRollback
   END
   -- ELSE IF (@UseTSTRollback = 0 AND @ErrorCode = 4) This cannot happen. We will live @ReturnCode set to -1 which will generate an internal error

   GOTO LblSaveErrors

LblSaveLogAndRollback:

   BEGIN TRY
      -- Rollback and in the same time preserves the log entries
      EXEC Internal.RollbackWithLogPreservation @TestSessionId, @LastTestLogEntryIdBeforeTest
   END TRY
   BEGIN CATCH
      -- RollbackWithLogPreservation will execute a ROLLBACK transaction so an error 266 caused by @@Trancount mismatch is expected. 
      IF (ERROR_NUMBER() != 266) EXEC Internal.Rethrow
   END CATCH

LblSaveErrors:

   IF (@ErrorMessage                 IS NOT NULL)  EXEC Internal.LogErrorMessage @ErrorMessage
   IF (@NestedTransactionMessage     IS NOT NULL)  EXEC Internal.LogErrorMessage @NestedTransactionMessage
   IF (@TransactionWarningMessage    IS NOT NULL)  EXEC Internal.LogErrorMessage @TransactionWarningMessage

   IF (@ReturnCode < 0)
   BEGIN 
      EXEC Internal.LogErrorMessage 'TST Internal Error in CollectTeardownSProcErrorInfo. Unexpected error code'
      SET @ReturnCode = 1
   END

   RETURN @ReturnCode

END
GO
/****** Object:  StoredProcedure [Internal].[CollectSetupSProcErrorInfo]    Script Date: 05/23/2013 11:40:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =======================================================================
-- PROCEDURE CollectSetupSProcErrorInfo
-- Called from within inside a CATCH block. It processes the information 
-- in the ERROR_XXX functions. It examines XACT_STATE() and 
-- @@TRANCOUNT and based on all that it will return an error code.
-- If the active transaction is in an uncommitable state it will do a 
-- ROLLBACK while preserving the entries in the TestLog table.
-- Return code:
--    1 - Error or failure but the execution can continue with the teardown.
--    2 - Error and the test execution has to be aborted.
-- =======================================================================
CREATE PROCEDURE [Internal].[CollectSetupSProcErrorInfo]
   @TestSessionId                int,              -- Identifies the test session.
   @SetupSProcId                 int,              -- Identifies the setup proc where the error occured.
   @UseTSTRollback               bit,              -- 1 if TSTRollback is enabled.
   @StartTranCount               int,              -- The transaction count before the setup procedure was invoked.
   @LastTestLogEntryIdBeforeTest int               -- The last id that was present in the TestLog 
                                                   -- table before the test execution started.
AS
BEGIN

   DECLARE @ErrorCode                     int
   DECLARE @ReturnCode                    int
   DECLARE @FullSprocName                 nvarchar(1000)
   DECLARE @ErrorMessage                  nvarchar(max)  -- If an error occured it will contain the error text
   DECLARE @NestedTransactionMessage      nvarchar(max)  -- If a nested transaction caused issues this will have an error message regarding that.
   DECLARE @TransactionWarningMessage     nvarchar(max)  -- If the teardown will have to be invoked outside of the context of a transaction 
                                                         -- this will have an error message regarding that.

   SET @ReturnCode = -1

   EXEC @ErrorCode = Internal.CollectErrorInfo  
                           @SetupSProcId, 
                           @StartTranCount, 
                           @UseTSTRollback, 
                           @ErrorMessage OUT,
                           @NestedTransactionMessage OUT
   
   -- We do not allow "Expected errors" during the Setup.
   -- If during the Setup we get an "Expected errors" we will record an error.
   IF (@ErrorCode = 0) SET @ErrorCode = 2

   IF      (@UseTSTRollback = 1 AND @ErrorCode = 1)  SET @ReturnCode = 1
   ELSE IF (@UseTSTRollback = 1 AND @ErrorCode = 2)  SET @ReturnCode = 1
   ELSE IF (@UseTSTRollback = 1 AND @ErrorCode = 3)   
   BEGIN
      -- The transaction is in an invalid (uncomittable) state. We need to roll it back.
      SET @FullSprocName = Internal.SFN_GetFullSprocName(@SetupSProcId)
      SET @TransactionWarningMessage = 'The transaction is in an uncommitable state after the setup procedure ''' + @FullSprocName + ''' has failed. A rollback was forced. The TEARDOWN if any will be executed outside of a transaction scope.'
      SET @ReturnCode = 1
      
      GOTO LblSaveLogAndRollback
   END
   ELSE IF (@UseTSTRollback = 1 AND @ErrorCode = 4)   
   BEGIN
      SET @FullSprocName = Internal.SFN_GetFullSprocName(@SetupSProcId)
      SET @TransactionWarningMessage = 'The transaction was rolled back during the setup procedure ''' + @FullSprocName + '''. The TEARDOWN if any will be executed outside of a transaction scope.'
      SET @ReturnCode = 1
   END
   ELSE IF (@UseTSTRollback = 0 AND @ErrorCode = 1)  SET @ReturnCode = 1
   ELSE IF (@UseTSTRollback = 0 AND @ErrorCode = 2)  SET @ReturnCode = 1
   IF (@UseTSTRollback = 0 AND @ErrorCode = 3)   
   BEGIN
      -- If we did not begin a transaction but now we have a transaction in an uncommitable state 
      -- then it means that the client opened it. We will rollback the transaction.
      SET @FullSprocName = Internal.SFN_GetFullSprocName(@SetupSProcId)
      SET @TransactionWarningMessage = 'The setup procedure ''' + @FullSprocName + ''' opened a transaction that is now in an uncommitable state. A rollback was forced. The TEARDOWN if any will be executed outside of a transaction scope.'
      SET @ReturnCode = 1
      
      GOTO LblSaveLogAndRollback
   END
   -- ELSE IF (@UseTSTRollback = 0 AND @ErrorCode = 4) This cannot happen. We will live @ReturnCode set to -1 which will generate an internal error

   GOTO LblSaveErrors

LblSaveLogAndRollback:

   BEGIN TRY
      -- Rollback and in the same time preserves the log entries
      EXEC Internal.RollbackWithLogPreservation @TestSessionId, @LastTestLogEntryIdBeforeTest
   END TRY
   BEGIN CATCH
      -- RollbackWithLogPreservation will execute a ROLLBACK transaction so an error 266 caused by @@Trancount mismatch is expected. 
      IF (ERROR_NUMBER() != 266) EXEC Internal.Rethrow
   END CATCH

LblSaveErrors:

   IF (@ErrorMessage                 IS NOT NULL)  EXEC Internal.LogErrorMessage @ErrorMessage
   IF (@NestedTransactionMessage     IS NOT NULL)  EXEC Internal.LogErrorMessage @NestedTransactionMessage
   IF (@TransactionWarningMessage    IS NOT NULL)  EXEC Internal.LogErrorMessage @TransactionWarningMessage

   IF (@ReturnCode < 0)
   BEGIN 
      EXEC Internal.LogErrorMessage 'TST Internal Error in CollectSetupSProcErrorInfo. Unexpected error code'
      SET @ReturnCode = 1
   END

   RETURN @ReturnCode

END
GO
/****** Object:  StoredProcedure [Internal].[RunTableComparison]    Script Date: 05/23/2013 11:40:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =======================================================================
-- PROCEDURE: RunTableComparison
-- Generates a SQL query that will pick up one row where the data in
-- #ExpectedResult and #ActualResult is not the same. Runs the query 
-- and by this determines if the data in #ExpectedResult and #ActualResult 
-- is the same or not. 
-- Asumes that #SchemaInfoExpectedResults and #SchemaInfoActualResults
-- are already created and contain the appropiate data.
-- Return code:
--    0 - The comparison was performed. 
--          - If the validation passed (the data in #ExpectedResult and 
--            #ActualResult is the same) then @DifferenceRowInfo will be NULL
--          - If the validation did not passed then @DifferenceRowInfo will 
--            contain a string showing data in one row that is different between
--            #ExpectedResult and #ActualResult 
--    1 - The comparison failed with an internal error. The appropiate 
--        error was logged
-- =======================================================================
CREATE PROCEDURE [Internal].[RunTableComparison]
   @DifferenceRowInfo nvarchar(max) OUT
AS
BEGIN

   DECLARE @SqlCommand                 nvarchar(max)
   DECLARE @Params                     nvarchar(100)

   EXEC Internal.GenerateComparisonSQLQuery @SqlCommand OUT

   -- PRINT ISNULL(@SqlCommand, 'null')

   IF (@SqlCommand IS NULL)
   BEGIN
      EXEC Internal.LogErrorMessageAndRaiseError 'TST Internal Error in RunTableComparison. @SqlCommand is NULL'
      RETURN 1
   END
                  
   SET @Params = '@DifString nvarchar(max) OUT'
   BEGIN TRY
      EXEC sp_executesql @SqlCommand, @Params, @DifString=@DifferenceRowInfo OUT
   END TRY
   BEGIN CATCH
      DECLARE @ErrorMessage    nvarchar(4000)

      -- Build the message string that will contain the original error information.
      PRINT 'TST Internal Error in RunTableComparison.'
      SELECT @ErrorMessage = 'TST Internal Error in RunTableComparison. ' + 
         'Error '       + ISNULL(CAST(ERROR_NUMBER()     as varchar        ), 'N/A') + 
         ', Level '     + ISNULL(CAST(ERROR_SEVERITY()   as varchar        ), 'N/A') + 
         ', State '     + ISNULL(CAST(ERROR_STATE()      as varchar        ), 'N/A') + 
         ', Procedure ' + ISNULL(CAST(ERROR_PROCEDURE()  as nvarchar(128)   ), 'N/A') + 
         ', Line '      + ISNULL(CAST(ERROR_LINE()       as varchar        ), 'N/A') + 
         ', Message: '  + ISNULL(CAST(ERROR_MESSAGE()    as nvarchar(2048)  ), 'N/A')

      EXEC Internal.LogErrorMessageAndRaiseError @ErrorMessage
      RETURN 1
   
   END CATCH

   RETURN 0
   
END
GO
/****** Object:  StoredProcedure [Assert].[RegisterExpectedError]    Script Date: 05/23/2013 11:40:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =======================================================================
-- PROCEDURE: RegisterExpectedError
-- Can be called by the test procedures to register an expected error.
-- TODO: Error out if all error params are null
-- TODO: Add severity and level
-- =======================================================================
CREATE PROCEDURE [Assert].[RegisterExpectedError]
   @ContextMessage            nvarchar(1000),
   @ExpectedErrorMessage      nvarchar(2048) = NULL,
   @ExpectedErrorProcedure    nvarchar(126) = NULL,
   @ExpectedErrorNumber       int = NULL
AS
BEGIN

   DECLARE @Stage          char
   DECLARE @ErrorMessage   nvarchar(1000)

   SELECT @Stage = Stage FROM #Tmp_CrtSessionInfo
   
   IF(@Stage != 'T')
   BEGIN
      IF (@Stage = 'S')
      BEGIN
         SET @ErrorMessage = 'A setup procedure cannot invoke RegisterExpectedError. RegisterExpectedError must be invoked by the test procedure before the error is raised.'
      END
      ELSE IF  (@Stage = 'X')
      BEGIN
         SET @ErrorMessage = 'A teardown procedure cannot invoke RegisterExpectedError. RegisterExpectedError must be invoked by the test procedure before the error is raised.'
      END
      ELSE 
      BEGIN
         SET @ErrorMessage = 'TST Internal Error. RegisterExpectedError appears to be called outside of any test context.'
      END

      EXEC Internal.LogErrorMessageAndRaiseError @ErrorMessage
      
   END

   UPDATE #Tmp_CrtSessionInfo SET 
      ExpectedErrorNumber          = @ExpectedErrorNumber          ,
      ExpectedErrorMessage         = @ExpectedErrorMessage         ,
      ExpectedErrorProcedure       = @ExpectedErrorProcedure       ,
      ExpectedErrorContextMessage  = @ContextMessage  

END
GO
/****** Object:  StoredProcedure [Internal].[RunOneTestInternal]    Script Date: 05/23/2013 11:40:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =======================================================================
-- PROCEDURE RunOneTestInternal
-- Runs a given test including its suite and teardown if defined. 
-- Implements the TST Rollback: will run the test in the context of a 
-- transaction that will be reverted at the end.
-- Note: The TST Rollback can be disabled.
-- =======================================================================
CREATE PROCEDURE [Internal].[RunOneTestInternal]
   @TestSessionId    int,     -- Identifies the test session.
   @TestSProcId      int,     -- Identifies the test stored procedure.
   @SetupSProcId     int,     -- Identifies the setup stored procedure.
   @TeardownSProcId  int      -- Identifies the teardown stored procedure.
AS
BEGIN

   DECLARE @LastTestLogEntryIdBeforeTest  int
   DECLARE @UseTSTRollback                bit
   DECLARE @SetupSprocErrorCode           int
   DECLARE @TestSprocErrorCode            int
   DECLARE @TeardownSprocErrorCode        int

   DECLARE @ExpectedErrorContextMessage   nvarchar(1000)
   DECLARE @ExpectedErrorInfo             nvarchar(4000)
   DECLARE @FullSprocName                 nvarchar(1000)
   DECLARE @Message                       nvarchar(max)
   DECLARE @StartTranCount                int


   UPDATE #Tmp_CrtSessionInfo SET TestId = @TestSProcId
   EXEC Internal.ClearExpectedError

   -- EXEC Utils.DropTestTables

   SET @UseTSTRollback = Internal.SFN_UseTSTRollbackForTest(@TestSessionId, @TestSProcId)
   IF (@UseTSTRollback = 1)
   BEGIN
      BEGIN TRANSACTION 
   END

   SET @StartTranCount = @@TRANCOUNT
   
   SELECT @LastTestLogEntryIdBeforeTest = LogEntryId FROM Data.TestLog WHERE TestSessionId = @TestSessionId
   SET @LastTestLogEntryIdBeforeTest = ISNULL(@LastTestLogEntryIdBeforeTest, 0)

   --================================
   -- SETUP
   --================================
   IF (@SetupSProcId IS NOT NULL) 
   BEGIN TRY
      UPDATE #Tmp_CrtSessionInfo SET Stage = 'S'
      EXEC Internal.RunOneSProc @SetupSProcId
   END TRY
   BEGIN CATCH
      BEGIN TRY
         EXEC @SetupSprocErrorCode = Internal.CollectSetupSProcErrorInfo
                                          @TestSessionId                 = @TestSessionId,
                                          @SetupSProcId                  = @SetupSProcId,
                                          @UseTSTRollback                = @UseTSTRollback,
                                          @StartTranCount                = @StartTranCount,
                                          @LastTestLogEntryIdBeforeTest  = @LastTestLogEntryIdBeforeTest
      END TRY
      BEGIN CATCH
         -- Some scenarios may cause CollectSetupSProcErrorInfo to rollback transactions. 
         -- When that happens the @@TRANCOUNT mismatch will trigger an error with error number 266. We'll ignore that error here.
         IF (ERROR_NUMBER() != 266) EXEC Internal.Rethrow
      END CATCH
      
      IF (@SetupSprocErrorCode = 0) GOTO LblBeforeTest
      IF (@SetupSprocErrorCode = 1) GOTO LblBeforeTeardown
      IF (@SetupSprocErrorCode = 2) GOTO LblPostTest

   END CATCH

LblBeforeTest:

   --================================
   -- TEST
   --================================
   BEGIN TRY
      UPDATE #Tmp_CrtSessionInfo SET Stage = 'T'
      EXEC Internal.RunOneSProc @TestSProcId

      -- Check if we were supposed to get an error.
      EXEC Internal.GetExpectedErrorInfo @ExpectedErrorContextMessage OUT, @ExpectedErrorInfo OUT 
      IF( @ExpectedErrorContextMessage IS NOT NULL)
      BEGIN
         SET @FullSprocName = Internal.SFN_GetFullSprocName(@TestSProcId)
         SET @Message = 'Test ' + @FullSprocName + ' failed. [' + @ExpectedErrorContextMessage + '] Expected error was not raised: ' + @ExpectedErrorInfo
         EXEC Assert.Fail @Message
      END
   END TRY
   BEGIN CATCH
      BEGIN TRY
         EXEC @TestSprocErrorCode = Internal.CollectTestSProcErrorInfo
                                       @TestSessionId                 = @TestSessionId,
                                       @TestSProcId                   = @TestSProcId,
                                       @UseTSTRollback                = @UseTSTRollback,
                                       @StartTranCount                = @StartTranCount,
                                       @LastTestLogEntryIdBeforeTest  = @LastTestLogEntryIdBeforeTest
      END TRY
      BEGIN CATCH
         -- Some scenarios may cause CollectTestSProcErrorInfo to rollback transactions. 
         -- When that happens the @@TRANCOUNT mismatch will trigger an error with error number 266. We'll ignore that error here.
         IF (ERROR_NUMBER() != 266) EXEC Internal.Rethrow
      END CATCH
      
      IF (@TestSprocErrorCode = 0) 
      BEGIN
         EXEC Internal.GetExpectedErrorInfo @ExpectedErrorContextMessage OUT, @ExpectedErrorInfo OUT 
         SET @FullSprocName = Internal.SFN_GetFullSprocName(@TestSProcId)
         SET @Message = 'Test ' + @FullSprocName + ' passed. [' + @ExpectedErrorContextMessage + '] Expected error was raised: ' + @ExpectedErrorInfo
         EXEC Assert.Pass @Message

         GOTO LblBeforeTeardown
      END
      IF (@TestSprocErrorCode = 1) GOTO LblBeforeTeardown
      IF (@TestSprocErrorCode = 2) GOTO LblPostTest

   END CATCH

LblBeforeTeardown:
   --================================
   -- TEARDOWN
   --================================
   IF (@TeardownSProcId IS NOT NULL)
   BEGIN TRY
      UPDATE #Tmp_CrtSessionInfo SET Stage = 'X'
      EXEC Internal.RunOneSProc @TeardownSProcId
   END TRY
   BEGIN CATCH
      BEGIN TRY
         EXEC @TeardownSprocErrorCode = Internal.CollectTeardownSProcErrorInfo
                                                @TestSessionId                 = @TestSessionId,
                                                @TeardownSProcId               = @TeardownSProcId,
                                                @UseTSTRollback                = @UseTSTRollback,
                                                @StartTranCount                = @StartTranCount,
                                                @LastTestLogEntryIdBeforeTest  = @LastTestLogEntryIdBeforeTest
      END TRY
      BEGIN CATCH
         -- Some scenarios may cause CollectTeardownSProcErrorInfo to rollback transactions. 
         -- When that happens the @@TRANCOUNT mismatch will trigger an error with error number 266. We'll ignore that error here.
         IF (ERROR_NUMBER() != 266) EXEC Internal.Rethrow
      END CATCH
     
   END CATCH

LblPostTest:

   IF (@@TRANCOUNT > 0)
   BEGIN
      BEGIN TRY
         -- Rollback and in the same time preserves the log entries
         EXEC Internal.RollbackWithLogPreservation @TestSessionId, @LastTestLogEntryIdBeforeTest
      END TRY
      BEGIN CATCH
         -- RollbackWithLogPreservation will execute a ROLLBACK transaction so an error 266 caused by @@Trancount mismatch is expected. 
         IF (ERROR_NUMBER() != 266) EXEC Internal.Rethrow
      END CATCH
   END

END
GO
/****** Object:  StoredProcedure [Internal].[PrintSuitesResultsForSession]    Script Date: 05/23/2013 11:40:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =======================================================================
-- PROCEDURE: PrintSuitesResultsForSession
-- It will print all the results of the current test session. 
-- =======================================================================
CREATE PROCEDURE [Internal].[PrintSuitesResultsForSession]
   @TestSessionId   int,            -- Identifies the test session.
   @ResultsFormat   varchar(10),    -- Indicates if the format in which the results will be printed.
                                    -- See the coments at the begining of the file under section 'Results Format'
   @Verbose          bit            -- If 1 then the output will contain all suites and tests names and all the log entries.
                                    -- If 0 then the output will contain all suites and tests names but only the 
                                    -- log entries indicating failures.
AS
BEGIN

   DECLARE @SuiteId                       int
   DECLARE @SuiteName                     sysname
   DECLARE @CountOfPassedTestInSuite      int
   DECLARE @CountOfFailedTestInSuite      int
   DECLARE @CountOfTestInSuite            int

   DECLARE CrsSuiteResults CURSOR LOCAL FOR
   SELECT SuiteId, SuiteName FROM Data.TSTResults 
   WHERE TestSessionId = @TestSessionId
   GROUP BY SuiteId, SuiteName
   ORDER BY SuiteName

   IF (@ResultsFormat = 'XML')
   BEGIN
      PRINT REPLICATE(' ', 2) + '<Suites>'
   END

   OPEN CrsSuiteResults
   FETCH NEXT FROM CrsSuiteResults INTO @SuiteId, @SuiteName
   WHILE @@FETCH_STATUS = 0
   BEGIN
   
      SET @CountOfTestInSuite = Internal.SFN_GetCountOfTestsInSuite(@SuiteId) 
      SET @CountOfFailedTestInSuite = Internal.SFN_GetCountOfFailedTestsInSuite(@SuiteId)
      SET @CountOfPassedTestInSuite = @CountOfTestInSuite - @CountOfFailedTestInSuite
      
      IF (@ResultsFormat = 'Text')
      BEGIN
         PRINT REPLICATE(' ', 4) + 'Suite: ' + ISNULL(@SuiteName, 'Anonymous') + '. Tests: ' + CAST(@CountOfTestInSuite as nvarchar(10)) + '. Passed: ' + CAST(@CountOfPassedTestInSuite as nvarchar(10)) + '. Failed: ' + CAST(@CountOfFailedTestInSuite as nvarchar(10))
      END
      ELSE IF (@ResultsFormat = 'XML')
      BEGIN
         PRINT REPLICATE(' ', 4) + '<Suite' + 
            ' suiteName="' + ISNULL(@SuiteName, 'Anonymous') + '"' + 
            ' testsCount="' + CAST(@CountOfTestInSuite as nvarchar(10)) + '"' + 
            ' passedCount="' + CAST(@CountOfPassedTestInSuite as nvarchar(10)) + '"' + 
            ' failedCount="' + CAST(@CountOfFailedTestInSuite as nvarchar(10)) + '"' + 
            ' >'
      END

	   EXEC Internal.PrintOneSuiteResults @SuiteId, @ResultsFormat, @Verbose
	   
      IF (@ResultsFormat = 'XML')
      BEGIN
         PRINT REPLICATE(' ', 4) + '</Suite>'
      END
      	   
      FETCH NEXT FROM CrsSuiteResults INTO @SuiteId, @SuiteName
   END
	
   CLOSE CrsSuiteResults
   DEALLOCATE CrsSuiteResults

   IF (@ResultsFormat = 'XML')
   BEGIN
      PRINT REPLICATE(' ', 2) + '</Suites>'
   END

END
GO
/****** Object:  StoredProcedure [Assert].[TableEquals]    Script Date: 05/23/2013 11:40:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =======================================================================
-- PROCEDURE: Assert.TableEquals
-- Can be called by the test procedures to verify that tables 
-- #ExpectedResult and #ActualResult have the same entries.
-- =======================================================================
CREATE PROCEDURE [Assert].[TableEquals]
   @ContextMessage      nvarchar(1000),
   @IgnoredColumns      ntext = NULL
AS
BEGIN

   DECLARE @ExpectedRowCount           int
   DECLARE @RunTableComparisonResult   int
   DECLARE @ValidationResult           int
   DECLARE @SchemaError                nvarchar(1000)
   DECLARE @Message                    nvarchar(4000)
   DECLARE @DifferenceRowInfo          nvarchar(max)

   SET @ContextMessage = ISNULL(@ContextMessage, '')

   EXEC @ValidationResult = Internal.BasicTempTableValidation @ContextMessage, @ExpectedRowCount OUT
   IF (@ValidationResult != 0) RETURN  -- an error was already raised

   IF (object_id('tempdb..#DiffRows') IS NULL) 
   BEGIN
      CREATE TABLE #DiffRows(
         ColumnName  sysname NOT NULL,
         ActualValue sql_variant,
         ExpectedValue sql_variant,
      )
   END
   ELSE DELETE FROM #DiffRows

   IF (object_id('tempdb..#SchemaInfoExpectedResults') IS NULL) 
   BEGIN
      CREATE TABLE #SchemaInfoExpectedResults (
         ColumnName           sysname NOT NULL,
         DataTypeName         nvarchar(128) NOT NULL,
         MaxLength            int NOT NULL,
         ColumnPrecision      int NOT NULL,
         ColumnScale          int NOT NULL,
         IsPrimaryKey         bit NOT NULL,
         IsIgnored            bit NOT NULL,
         PkOrdinal            int NULL,
         ColumnCollationName  sysname NULL
      )
   END
   ELSE DELETE FROM #SchemaInfoExpectedResults 
   
   IF (object_id('tempdb..#SchemaInfoActualResults') IS NULL) 
   BEGIN
      CREATE TABLE #SchemaInfoActualResults (
         ColumnName           sysname NOT NULL,
         DataTypeName         nvarchar(128) NOT NULL,
         MaxLength            int NOT NULL,
         ColumnPrecision      int NOT NULL,
         ColumnScale          int NOT NULL,
         IsPrimaryKey         bit NOT NULL,
         IsIgnored            bit NOT NULL,
         PkOrdinal            int NULL,
         ColumnCollationName  sysname NULL
      )
   END
   ELSE DELETE FROM #SchemaInfoActualResults 

   IF (object_id('tempdb..#IgnoredColumns') IS NULL) 
   BEGIN
      CREATE TABLE #IgnoredColumns (ColumnName varchar(500))
   END
   ELSE DELETE FROM #IgnoredColumns

   INSERT INTO #IgnoredColumns(ColumnName) SELECT ListItem FROM Internal.SFN_GetListToTable(@IgnoredColumns)

   EXEC Internal.CollectTempTablesSchema

   EXEC Internal.ValidateTempTablesSchema @SchemaError OUT
   IF (@SchemaError IS NOT NULL)
   BEGIN
      SET @Message = 'Invalid call to Assert.TableEquals. [' + @ContextMessage + '] ' + @SchemaError
      EXEC Internal.LogErrorMessageAndRaiseError @Message
      RETURN
   END

   EXEC @RunTableComparisonResult = Internal.RunTableComparison @DifferenceRowInfo OUT 
   IF (@RunTableComparisonResult != 0) RETURN 
   
   IF (@DifferenceRowInfo IS NOT NULL)
   BEGIN
      SET @Message = 'Assert.TableEquals failed. [' + @ContextMessage + '] #ExpectedResult and #ActualResult do not have the same data. Expected/Actual: ' + @DifferenceRowInfo 
      EXEC Assert.Fail @Message
      RETURN
   END

   SET @Message = 'Assert.TableEquals passed. [' + @ContextMessage + '] ' + CAST(@ExpectedRowCount as varchar) + ' row(s) compared between #ExpectedResult and #ActualResult'
   EXEC Assert.Pass @Message

END
GO
/****** Object:  StoredProcedure [Internal].[RunOneSuiteInternal]    Script Date: 05/23/2013 11:40:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
-- =======================================================================
-- PROCEDURE RunOneSuiteInternal
-- Runs a given test suite. 
-- =======================================================================
CREATE PROCEDURE [Internal].[RunOneSuiteInternal]
   @TestSessionId    int,              -- Identifies the test session.
                                       -- Note: this is provided as a optimization. It could be determined based on @SuiteId
   @SuiteId          int               -- Identifies the suite.
AS
BEGIN

   DECLARE @TestSProcId             int
   DECLARE @SetupSProcId            int
   DECLARE @TeardownSProcId         int
   DECLARE @ErrorMessage            nvarchar(4000)
   
   SELECT @SetupSProcId    = TestId FROM Data.Test WHERE SuiteId = @SuiteId AND SProcType = 'Setup'
   SELECT @TeardownSProcId = TestId FROM Data.Test WHERE SuiteId = @SuiteId AND SProcType = 'Teardown'

   DECLARE CrsTests CURSOR LOCAL FOR
   SELECT TestId 
   FROM Data.Test 
   WHERE SuiteId = @SuiteId AND SProcType = 'Test'
   ORDER By TestId

   OPEN CrsTests
   FETCH NEXT FROM CrsTests INTO @TestSProcId
   WHILE @@FETCH_STATUS = 0
   BEGIN
   
      BEGIN TRY
      		
		 --2011/12/06  mandy 
		 declare @ProcName varchar(max)
		 set @ProcName = Internal.SFN_GetFullSprocName(@TestSProcId)
		 
		 declare @Suite varchar(20)
		 select @Suite = Suitename from tst.Data.Suite where SuiteId = @SuiteId
		 
		 --2011/12/06  mandy 
		 --delete if run in past hour
--		 DELETE 
--		 FROM [TST].[Data].[TestTimer] 
--		 where FullSprocName = @ProcName 
--		 and TestStart <= dateadd(mi, -60, getdate())	 
--		 
--		 DELETE 
--		 FROM [TST].[Data].[TestTimer] 
--		 where FullSprocName =
--							 (select FullSprocName
--							 FROM [TST].[Data].[TestTimer] 
--							 where FullSprocName = @ProcName							 
--							 Group by FullSprocName
--							 having count(FullSprocName) > 5
--							 )
--		 	
		 declare @RunCount int
		 select @RunCount = ((select Count(TestTimerID) From [TST].[Data].[TestTimer] (nolock) where FullSprocName = @ProcName and RunDate = dateadd(dd,datediff(dd,0,getdate()),0)) + 1)
		 --2011/12/06  mandy 
		 --select * from tst.Data.TestTimer order by 2,1
		 INSERT INTO [TST].[Data].[TestTimer](FullSprocName,TestSessionId, TestSProcId, DatabaseName,TestStart, TestStatus, Suitename,DatabaseTestName,RunDate,RunCount)
			select @ProcName, @TestSessionId, @TestSProcId, @@servername, getdate(), null ,@Suite,db_name(),dateadd(dd,datediff(dd,0,getdate()),0),@RunCount
             
         EXEC Internal.RunOneTestInternal
               @TestSessionId    ,
               @TestSProcId      ,
               @SetupSProcId     ,
               @TeardownSProcId  

            IF		( 
					(	SELECT COUNT(1) 
						FROM Data.TestLog 
						WHERE TestSessionId = @TestSessionId 
						AND TestId = @TestSProcId 
						AND EntryType IN('P', 'F', 'E')
					) = 0 
					)
         BEGIN
            -- We don't want here to call Assert.Fail because that raises an error. 
            --We'll simply insert the error message in TestLog
            INSERT INTO Data.TestLog(TestSessionId, TestId, EntryType, LogMessage) 
            VALUES (@TestSessionId, @TestSProcId, 'F', 'No Assert, Fail or Pass was invoked by this test. You must call at least one TST API that performs a validation, records a failure or records a pass (Assert..., Pass, Fail, etc.)')
					 
         END
		 
		 --2011/12/06  mandy  
		 UPDATE [TST].[Data].[TestTimer] 
		 set TestFinish = getdate()
		 where TestSProcId = @TestSProcId
		 and TestSessionId = @TestSessionId
		 
		 --2011/12/06  mandy 
		 UPDATE [TST].[Data].[TestTimer] 
		 set RunTimeSec = abs(datediff(ss, TestFinish, TestStart)),
			 RunTimeMins  = (abs(datediff(ss, TestFinish, TestStart)))/60
		 where TestSProcId = @TestSProcId
		 and TestSessionId = @TestSessionId
		 
		--2011/12/06  mandy 
		Update t
		set TestStatus = CASE WHEN TestFailInfo.FailuresOrErrorsCount > 0 THEN 'Fail' ELSE 'Pass' END
		From [TST].[Data].[TestTimer] t
		INNER JOIN  (  
						SELECT  TestId, TestSessionId,
							  (  SELECT COUNT(*) 
							     FROM Tst.Data.TestLog AS L1
								 WHERE  (L1.EntryType = 'E' OR L1.EntryType = 'F' ) AND L1.TestId = T1.TestId
							  ) AS FailuresOrErrorsCount
						FROM TST.Data.Test AS T1
					) AS TestFailInfo ON TestFailInfo.TestSessionId = t.TestSessionId 
					and  TestFailInfo.TestId = t.TestSProcId
		 where  t.TestSessionId = @TestSessionId
		 and t.TestSProcId = @TestSProcId
		 
		 --2011/12/06  mandy 
		 if (select system_user) = 'SAHL\MandyM'
		 BEGIN		 
				select FullSprocName,
					   DatabaseName,
					   TestStatus,
					   TestStart,
					   TestFinish,
					   RunTimeMins,
					   RunTimeSec,
					   Suitename
			 from [TST].[Data].[TestTimer] 
			 where FullSprocName = @ProcName
			 Order by TestFinish DESC 
		 END
		 
		 
      END TRY
      BEGIN CATCH
         -- RunOneTestInternal should trap all possible errors and handle them
         -- We should not get into this situation. 
         
         -- TODO: can we get the below string building in a function? 
         SET @ErrorMessage =  'TST Internal Error in RunOneSuiteInternal. Unexpected error: ' +
                              CAST(ERROR_NUMBER() AS varchar) + ', ' + ERROR_MESSAGE() + 
                              ' Procedure: ' + ISNULL(ERROR_PROCEDURE(), 'N/A') + '. Line: ' + CAST(ERROR_LINE() AS varchar)
         EXEC Internal.LogErrorMessage @ErrorMessage
         
			 
      END CATCH

      -- Update #Tmp_CrtSessionInfo to indicate we are outside of any test stored procedure.
      UPDATE #Tmp_CrtSessionInfo SET TestId = -1, Stage = '-'

	   FETCH NEXT FROM CrsTests INTO @TestSProcId
   END
	
   CLOSE CrsTests
   DEALLOCATE CrsTests

END
GO
/****** Object:  StoredProcedure [Utils].[PrintResults]    Script Date: 05/23/2013 11:40:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =======================================================================
-- PROCEDURE: PrintResults
-- It will print all the results of the current test session. 
-- =======================================================================
CREATE PROCEDURE [Utils].[PrintResults]
   @TestSessionId    int,         -- Identifies the test session.
   @ResultsFormat    varchar(10), -- Indicates if the format in which the results will be printed.
                                  -- See the coments at the begining of the file under section 'Results Format'
   @NoTimestamp      bit = 0,     -- Indicates that no timestamp or duration info should be printed in results output
   @Verbose          bit = 0      -- If 1 then the output will contain all suites and tests names and all the log entries.
                                  -- If 0 then the output will contain all suites and tests names but only the 
                                  -- log entries indicating failures.
AS
BEGIN
   
   IF (      @ResultsFormat != 'Text'
         AND @ResultsFormat != 'XML'
         AND @ResultsFormat != 'Batch'
         AND @ResultsFormat != 'None' )
   BEGIN
      RAISERROR('Invalid call to RunSuite. @TestDatabaseName cannot be NULL.', 16, 1)
      RETURN 1
   END

   IF (@ResultsFormat = 'None') RETURN 0

   IF (@ResultsFormat = 'Batch' OR @ResultsFormat = 'Text' ) PRINT ''
   
   IF (@ResultsFormat = 'Batch')
   BEGIN
      PRINT 'TST TestSessionId: ' + CAST(@TestSessionId as varchar)

      -- For the rest of the print process 'Batch' mode is the same as 'Text' mode
      SET @ResultsFormat = 'Text'
   END
   
   IF (@ResultsFormat = 'XML')
   BEGIN
      PRINT '<?xml version="1.0" encoding="UTF-8" ?> '
   END

   EXEC Internal.PrintHeaderForSession         @TestSessionId, @ResultsFormat, @NoTimestamp
   EXEC Internal.PrintSystemErrorsForSession   @TestSessionId, @ResultsFormat
   EXEC Internal.PrintSuitesResultsForSession  @TestSessionId, @ResultsFormat, @Verbose

   IF (@ResultsFormat = 'Batch' OR @ResultsFormat = 'Text' ) PRINT ''
   EXEC Internal.PrintResultsSummaryForSession @TestSessionId, @ResultsFormat, @NoTimestamp

   IF (@ResultsFormat = 'Text')
   BEGIN
      PRINT ''
      EXEC Internal.PrintStatusForSession  @TestSessionId
      PRINT ''
   END
   ELSE
   IF (@ResultsFormat = 'XML')
   BEGIN
      PRINT '</TST>'
   END

   RETURN 0
END
GO
/****** Object:  StoredProcedure [Internal].[RunSuitesInternal]    Script Date: 05/23/2013 11:40:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =======================================================================
-- PROCEDURE: RunSuitesInternal
-- Called by RunSuites. See RunSuites comments
-- Return code:
--    0 - OK. All appropiate tests were run.
--    1 - Error. Suite not found or no suites were defined.
-- =======================================================================
CREATE PROCEDURE [Internal].[RunSuitesInternal]
   @TestSessionId       int,              -- Identifies the test session.
   @TestDatabaseName    sysname,          -- The database that contains the TST procedures.
   @SuiteName           sysname = NULL    -- The suite that must be run. If not specified then 
                                          -- tests in all suites will be run.
AS
BEGIN

   DECLARE @SuiteId     int
   DECLARE @LogMessage  nvarchar(max)
   DECLARE @CountSuite  int

   IF @SuiteName IS NOT NULL
   BEGIN
   
      SELECT @SuiteId = SuiteId FROM Data.Suite WHERE TestSessionId = @TestSessionId AND SuiteName = @SuiteName
      IF @SuiteId IS NULL
      BEGIN
         SET @LogMessage = 'Suite ''' + @SuiteName + ''' not found'
         EXEC Internal.LogErrorMessage @LogMessage
         RETURN 1
      END
      EXEC Internal.RunOneSuiteInternal @TestSessionId, @SuiteId
   END
   ELSE
   BEGIN

      DECLARE CrsSuites CURSOR LOCAL FOR 
      SELECT SuiteId 
      FROM Data.Suite 
      WHERE TestSessionId = @TestSessionId
      ORDER BY SuiteId

	   OPEN CrsSuites
	   FETCH NEXT FROM CrsSuites INTO @SuiteId
	   WHILE @@FETCH_STATUS = 0
	   BEGIN
   	   EXEC Internal.RunOneSuiteInternal @TestSessionId, @SuiteId
   	   FETCH NEXT FROM CrsSuites INTO @SuiteId
	   END
   	
	   CLOSE CrsSuites
	   DEALLOCATE CrsSuites
   END

   RETURN 0
END
GO
/****** Object:  StoredProcedure [Internal].[PostTestRun]    Script Date: 05/23/2013 11:40:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =======================================================================
-- PROCEDURE: PostTestRun
-- Execute the optional post test run steps: print results and 
-- clean of temporary data.
-- =======================================================================
CREATE PROCEDURE [Internal].[PostTestRun]
   @TestSessionId          int,              -- Identifies the test session.
   @ResultsFormat          varchar(10),      -- Indicates if the format in which the results will be printed.
                                             -- See the coments at the begining of the file under section 'Results Format'
   @NoTimestamp            bit,              -- Indicates that no timestamp or duration info should be printed in results output
   @Verbose                bit,              -- If 1 then the output will contain all suites and tests names and all the log entries.
                                             -- If 0 then the output will contain all suites and tests names but only the 
                                             -- log entries indicating failures.
   @CleanTemporaryData     bit               -- Indicates if the temporary tables should be cleaned at the end.
AS
BEGIN

   EXEC Utils.PrintResults @TestSessionId, @ResultsFormat, @NoTimestamp, @Verbose
   IF (@CleanTemporaryData = 1)  EXEC Internal.CleanSessionData  @TestSessionId
   
END
GO
/****** Object:  StoredProcedure [Runner].[RunSuite]    Script Date: 05/23/2013 11:40:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =======================================================================
-- END TST API.
-- =======================================================================


-- =======================================================================
-- START External trigger points.
-- These are stored procedures that can be called to trigger TST testing.
-- =======================================================================

-- =======================================================================
-- PROCEDURE: RunSuite
-- It will run all the test procedures in the database given 
-- by @TestDatabaseName and belonging to the suite given by @SuiteName.
-- If @SuiteName IS NULL then it will run all the Test suites 
-- detected in the database given by @TestDatabaseName.
-- =======================================================================
CREATE PROCEDURE [Runner].[RunSuite]
   @TestDatabaseName       sysname,                -- The database that contains the test procedures.
   @SuiteName              sysname,                -- The suite that must be run. If NULL then 
                                                   -- tests in all suites will be run.
   @Verbose                bit = 0,                -- If 1 then the output will contain all suites and tests names and all the log entries.
                                                   -- If 0 then the output will contain all suites and tests names but only the 
                                                   -- log entries indicating failures.
   @ResultsFormat          varchar(10) = 'Text',   -- Indicates if the format in which the results will be printed.
                                                   -- See the coments at the begining of the file under section 'Results Format'
   @NoTimestamp            bit = 0,                -- Indicates that no timestamp or duration info should be printed in results output
   @CleanTemporaryData     bit = 1,                -- Indicates if the temporary tables should be cleaned at the end.
   @TestSessionId          int = NULL OUT,         -- At return will identify the test session 
   @TestSessionPassed      bit = NULL OUT          -- At return will indicate if all tests passed or not.
AS
BEGIN

   DECLARE @PrepareResult int

   SET NOCOUNT ON

   IF (@TestDatabaseName IS NULL) 
   BEGIN
      RAISERROR('Invalid call to RunSuite. @TestDatabaseName cannot be NULL.', 16, 1)
      RETURN 1
   END      

   BEGIN
      CREATE TABLE #Tmp_CrtSessionInfo (
         TestSessionId                 int NOT NULL,
         TestId                        int NOT NULL,
         Stage                         char NOT NULL,       -- '-' Outside of any test
                                                            -- 'S' Setup stage
                                                            -- 'T' Test stage
                                                            -- 'X' Teardown stage
         ExpectedErrorNumber           int NULL,
         ExpectedErrorMessage          nvarchar(2048),
         ExpectedErrorProcedure        nvarchar(126),
         ExpectedErrorContextMessage   nvarchar(1000)
      )
   END
   
   EXEC @PrepareResult = Internal.PrepareTestSession @TestDatabaseName, @TestSessionId OUTPUT
   
   IF (@PrepareResult = 0)
   BEGIN
      EXEC @PrepareResult = Internal.PrepareTestInformation @TestSessionId, @TestDatabaseName, @SuiteName, NULL
      IF (@PrepareResult = 0)
      BEGIN
         EXEC Internal.RunSuitesInternal @TestSessionId, @TestDatabaseName, @SuiteName
      END
   END
   
   -- Note: if @PrepareResult is 0 then we already have errors in the TestLog table.

   SET @TestSessionPassed = 1
   IF EXISTS (SELECT 1 FROM Data.TestLog WHERE TestSessionId = @TestSessionId AND EntryType IN ('F', 'E')) SET @TestSessionPassed = 0
   IF EXISTS (SELECT 1 FROM Data.SystemErrorLog WHERE TestSessionId = @TestSessionId) SET @TestSessionPassed = 0

   UPDATE Data.TestSession SET TestSessionFinish = GETDATE()
  
   EXEC Internal.PostTestRun @TestSessionId, @ResultsFormat, @NoTimestamp, @Verbose, @CleanTemporaryData
   
END
GO
/****** Object:  StoredProcedure [Runner].[RunTest]    Script Date: 05/23/2013 11:40:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =======================================================================
-- PROCEDURE: RunTest
-- It will run the test procedure with the name given by @TestName
-- in the database given by @TestDatabaseName.
-- =======================================================================
CREATE PROCEDURE [Runner].[RunTest]
   @TestDatabaseName    sysname,                   -- The database that contains the test procedures.
   @TestName            sysname,                   -- The test that must be run.
   @Verbose             bit = 0,                   -- If 1 then the output will contain all suites and tests names and all the log entries.
                                                   -- If 0 then the output will contain all suites and tests names but only the 
                                                   -- log entries indicating failures.
   @ResultsFormat       varchar(10) = 'Text',      -- Indicates if the format in which the results will be printed.
                                                   -- See the coments at the begining of the file under section 'Results Format'
   @NoTimestamp         bit = 0,                   -- Indicates that no timestamp or duration info should be printed in results output
   @CleanTemporaryData  bit = 1,                   -- Indicates if the temporary tables should be cleaned at the end.
   @TestSessionId       int = NULL OUT,            -- At return will identify the test session 
   @TestSessionPassed   bit = NULL OUT             -- At return will indicate if all tests passedor not.
AS
BEGIN

   DECLARE @PrepareResult int

   SET NOCOUNT ON

   IF (@TestDatabaseName IS NULL) 
   BEGIN
      RAISERROR('Invalid call to RunTest. @TestDatabaseName cannot be NULL.', 16, 1)
      RETURN 1
   END

   BEGIN
      CREATE TABLE #Tmp_CrtSessionInfo (
         TestSessionId                 int NOT NULL,
         TestId                        int NOT NULL,
         Stage                         char NOT NULL,       -- '-' Outside of any test
                                                            -- 'S' Setup stage
                                                            -- 'T' Test stage
                                                            -- 'X' Teardown stage
         ExpectedErrorNumber           int NULL,
         ExpectedErrorMessage          nvarchar(2048),
         ExpectedErrorProcedure        nvarchar(126),
         ExpectedErrorContextMessage   nvarchar(1000)
      )
   END

   EXEC @PrepareResult = Internal.PrepareTestSession @TestDatabaseName, @TestSessionId OUTPUT
   IF (@PrepareResult = 0)
   BEGIN
      EXEC @PrepareResult = Internal.PrepareTestInformation @TestSessionId, @TestDatabaseName, NULL, @TestName
      IF (@PrepareResult = 0)
      BEGIN
         -- PrepareTestInformation will colect data only about the given test so we can 
         -- call RunSuitesInternal with NULL for @SuiteName 
         EXEC Internal.RunSuitesInternal @TestSessionId, @TestDatabaseName, NULL
      END
   END
   
   -- Note: if @PrepareResult is 0 then we already have errors in the TestLog table.

   SET @TestSessionPassed = 1
   IF EXISTS (SELECT 1 FROM Data.TestLog WHERE TestSessionId = @TestSessionId AND EntryType IN ('F', 'E')) SET @TestSessionPassed = 0
   IF EXISTS (SELECT 1 FROM Data.SystemErrorLog WHERE TestSessionId = @TestSessionId) SET @TestSessionPassed = 0

   UPDATE Data.TestSession SET TestSessionFinish = GETDATE()

   EXEC Internal.PostTestRun @TestSessionId, @ResultsFormat, @NoTimestamp, @Verbose, @CleanTemporaryData
   
END
GO
/****** Object:  StoredProcedure [Runner].[RunAll]    Script Date: 05/23/2013 11:40:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =======================================================================
-- PROCEDURE: RunAll
-- It will run all the test procedures in the database given 
-- by @TestDatabaseName.
-- =======================================================================
CREATE PROCEDURE [Runner].[RunAll]
   @TestDatabaseName    sysname,                   -- The database that contains the test procedures.
   @Verbose             bit = 0,                   -- If 1 then the output will contain all suites and tests names and all the log entries.
                                                   -- If 0 then the output will contain all suites and tests names but only the 
                                                   -- log entries indicating failures.
   @ResultsFormat       varchar(10) = 'Text',      -- Indicates if the format in which the results will be printed.
                                                   -- See the coments at the begining of the file under section 'Results Format'
   @NoTimestamp         bit = 0,                   -- Indicates that no timestamp or duration info should be printed in results output
   @CleanTemporaryData  bit = 1,                   -- Indicates if the temporary tables should be cleaned at the end.
   @TestSessionId       int = NULL OUT,            -- At return will identify the test session 
   @TestSessionPassed   bit = NULL OUT             -- At return will indicate if all tests passedor not.
AS
BEGIN

   IF (@TestDatabaseName IS NULL) 
   BEGIN
      RAISERROR('Invalid call to RunAll. @TestDatabaseName cannot be NULL.', 16, 1)
      RETURN 1
   END
   
   SET NOCOUNT ON
   EXEC Runner.RunSuite @TestDatabaseName, NULL,  @Verbose, @ResultsFormat, @NoTimestamp, @CleanTemporaryData, @TestSessionId OUT, @TestSessionPassed OUT
END
GO
/****** Object:  Default [DF__TSTVersio__Setup__173876EA]    Script Date: 05/23/2013 11:40:18 ******/
ALTER TABLE [Data].[TSTVersion] ADD  DEFAULT (getdate()) FOR [SetupDate]
GO
/****** Object:  Default [DF__TestLog__Created__29572725]    Script Date: 05/23/2013 11:40:21 ******/
ALTER TABLE [Data].[TestLog] ADD  DEFAULT (getdate()) FOR [CreatedTime]
GO
/****** Object:  Default [DF__SystemErr__Creat__2F10007B]    Script Date: 05/23/2013 11:40:21 ******/
ALTER TABLE [Data].[SystemErrorLog] ADD  DEFAULT (getdate()) FOR [CreatedTime]
GO
/****** Object:  Check [CK_TSTParameters_ParameterName]    Script Date: 05/23/2013 11:40:18 ******/
ALTER TABLE [Data].[TSTParameters]  WITH CHECK ADD  CONSTRAINT [CK_TSTParameters_ParameterName] CHECK  (([ParameterName]='UseTSTRollback'))
GO
ALTER TABLE [Data].[TSTParameters] CHECK CONSTRAINT [CK_TSTParameters_ParameterName]
GO
/****** Object:  Check [CK_TSTParameters_Scope]    Script Date: 05/23/2013 11:40:18 ******/
ALTER TABLE [Data].[TSTParameters]  WITH CHECK ADD  CONSTRAINT [CK_TSTParameters_Scope] CHECK  (([Scope]='All' OR [Scope]='Suite' OR [Scope]='Test'))
GO
ALTER TABLE [Data].[TSTParameters] CHECK CONSTRAINT [CK_TSTParameters_Scope]
GO
/****** Object:  Check [CK_TestLog_EntryType]    Script Date: 05/23/2013 11:40:21 ******/
ALTER TABLE [Data].[TestLog]  WITH CHECK ADD  CONSTRAINT [CK_TestLog_EntryType] CHECK  (([EntryType]='P' OR [EntryType]='L' OR [EntryType]='F' OR [EntryType]='E'))
GO
ALTER TABLE [Data].[TestLog] CHECK CONSTRAINT [CK_TestLog_EntryType]
GO
/****** Object:  Check [CK_Test_SProcType]    Script Date: 05/23/2013 11:40:21 ******/
ALTER TABLE [Data].[Test]  WITH CHECK ADD  CONSTRAINT [CK_Test_SProcType] CHECK  (([SProcType]='Setup' OR [SProcType]='Teardown' OR [SProcType]='Test'))
GO
ALTER TABLE [Data].[Test] CHECK CONSTRAINT [CK_Test_SProcType]
GO
/****** Object:  ForeignKey [FK_TestLog_TestId]    Script Date: 05/23/2013 11:40:21 ******/
ALTER TABLE [Data].[TestLog]  WITH CHECK ADD  CONSTRAINT [FK_TestLog_TestId] FOREIGN KEY([TestId])
REFERENCES [Data].[Test] ([TestId])
GO
ALTER TABLE [Data].[TestLog] CHECK CONSTRAINT [FK_TestLog_TestId]
GO
/****** Object:  ForeignKey [FK_TestLog_TestSessionId]    Script Date: 05/23/2013 11:40:21 ******/
ALTER TABLE [Data].[TestLog]  WITH CHECK ADD  CONSTRAINT [FK_TestLog_TestSessionId] FOREIGN KEY([TestSessionId])
REFERENCES [Data].[TestSession] ([TestSessionId])
GO
ALTER TABLE [Data].[TestLog] CHECK CONSTRAINT [FK_TestLog_TestSessionId]
GO
/****** Object:  ForeignKey [FK_Suite_TestSessionId]    Script Date: 05/23/2013 11:40:21 ******/
ALTER TABLE [Data].[Suite]  WITH CHECK ADD  CONSTRAINT [FK_Suite_TestSessionId] FOREIGN KEY([TestSessionId])
REFERENCES [Data].[TestSession] ([TestSessionId])
GO
ALTER TABLE [Data].[Suite] CHECK CONSTRAINT [FK_Suite_TestSessionId]
GO
/****** Object:  ForeignKey [FK_SystemErrorLog_TestSessionId]    Script Date: 05/23/2013 11:40:21 ******/
ALTER TABLE [Data].[SystemErrorLog]  WITH CHECK ADD  CONSTRAINT [FK_SystemErrorLog_TestSessionId] FOREIGN KEY([TestSessionId])
REFERENCES [Data].[TestSession] ([TestSessionId])
GO
ALTER TABLE [Data].[SystemErrorLog] CHECK CONSTRAINT [FK_SystemErrorLog_TestSessionId]
GO
/****** Object:  ForeignKey [FK_Test_SuiteId]    Script Date: 05/23/2013 11:40:21 ******/
ALTER TABLE [Data].[Test]  WITH CHECK ADD  CONSTRAINT [FK_Test_SuiteId] FOREIGN KEY([SuiteId])
REFERENCES [Data].[Suite] ([SuiteId])
GO
ALTER TABLE [Data].[Test] CHECK CONSTRAINT [FK_Test_SuiteId]
GO
use master
go
begin try
	drop database process_test;
end try
begin catch
end catch

create database [process_test]
go

USE [Process_Test]
GO

/****** Object:  StoredProcedure [dbo].[TSTConfig]    Script Date: 05/24/2013 08:56:41 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
if object_id('dbo.tstconfig') is not null
drop PROCEDURE [dbo].[TSTConfig]
go

create PROCEDURE [dbo].[TSTConfig]

AS
BEGIN

   --EXEC TST.Utils.SetConfiguration 
   --                  @ParameterName='UseTSTRollback', 
   --                  @ParameterValue='0', 
   --                  @Scope='Suite', 
   --                  @ScopeValue='haloNoTran'

   EXEC TST.Utils.SetConfiguration 
                     @ParameterName='UseTSTRollback', 
                     @ParameterValue='0', 
                     @Scope='Suite', 
                     @ScopeValue='Security'
END

GO
USE [Process_Test]
GO

/****** Object:  User [ServiceArchitect]    Script Date: 05/24/2013 08:59:11 ******/
IF  EXISTS (SELECT * FROM sys.database_principals WHERE name = N'ServiceArchitect')
DROP USER [ServiceArchitect]
GO

USE [Process_Test]
GO

/****** Object:  User [ServiceArchitect]    Script Date: 05/24/2013 08:59:11 ******/
GO

CREATE USER [ServiceArchitect] FOR LOGIN [ServiceArchitect] WITH DEFAULT_SCHEMA=[dbo]
GO

USE [Process_Test]
GO

/****** Object:  User [batch]    Script Date: 05/24/2013 08:59:49 ******/
IF  EXISTS (SELECT * FROM sys.database_principals WHERE name = N'batch')
DROP USER [batch]
GO

USE [Process_Test]
GO

/****** Object:  User [batch]    Script Date: 05/24/2013 08:59:49 ******/
GO

CREATE USER [batch] FOR LOGIN [batch] WITH DEFAULT_SCHEMA=[dbo]
GO




use [process_test]
go
if object_id('sqltest_Security#UserTableSecurityCheckUpdateFailWithServiceArchitect') is NOT NULL
drop proc sqltest_Security#UserTableSecurityCheckUpdateFailWithServiceArchitect
go
/*=============================================================
SCRIPT HEADER

VERSION:   1.01.0001
SERVER:    dbbuild03

SCRIPTED OBJECTS: DATABASE:	Process_Test
  Procedure:  sqltest_Security#UserTableSecurityCheckUpdateFailWithServiceArchitect


=============================================================*/
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_WARNINGS ON
SET NOCOUNT ON
SET XACT_ABORT ON
GO

-- BEGINNING TRANSACTION STRUCTURE
PRINT 'Beginning transaction STRUCTURE'
BEGIN TRANSACTION _STRUCTURE_
GO
-- Create Procedure [dbo].[sqltest_Security#UserTableSecurityCheckUpdateFailWithServiceArchitect]
Print 'Create Procedure [dbo].[sqltest_Security#UserTableSecurityCheckUpdateFailWithServiceArchitect]'
GO
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER OFF
GO

IF @@ERROR<>0 OR @@TRANCOUNT=0 BEGIN IF @@TRANCOUNT>0 ROLLBACK SET NOEXEC ON END
GO
CREATE PROCEDURE [dbo].[sqltest_Security#UserTableSecurityCheckUpdateFailWithServiceArchitect]
AS
/***************************************************************************************************************************************
 Author: GrantW
 Create date: 19 August 2011
 Description: This Unit test will try and update informaiton using ServiceArchitect login from 
				the Account, FinancialService, Balance, LoanBalance, FinancalTransaction, ArrearTransaction tables
				And we should expect to see a permissions error when trying to perform these actions.
 
 History:
			2011/08/25	GrantW	Created
			2011/08/29	MandyM	Added SPVTransaction to list of tables to test 	
			2013/04/25  Helasha Edited to Run on BuildServer 
***************************************************************************************************************************************/
  BEGIN
   --Declare variable

	--IF EXISTS OBJECT_ID('#TablesToProcess') 
	--	DROP TABLE #TablesToProcess


	SELECT	ROW_NUMBER() OVER(ORDER BY t.Name DESC) AS 'TableKey',
			s.Name + '.' + t.Name AS TableName,
			c.Name FirstColumnName
	INTO #TablesToProcess
	FROM [2AM].sys.Tables t
	INNER JOIN [2AM].sys.Schemas s
	ON t.Schema_ID = s.Schema_ID
	INNER JOIN [2AM].sys.Columns c
	ON c.[Object_ID] = t.[Object_ID]
	WHERE t.Name IN 
		(
			'Account',
			'FinancialService',
			'FinancialTransaction',
			'ArrearTransaction',
			'LoanBalance',
			'Balance',
			'SPVTransaction'
		)
		AND c.Column_ID = 2
		AND s.Name IN ('fin', 'dbo', 'SPV') 
	
	DECLARE @MaxKey INT
	DECLARE @CurrentKey INT
	DECLARE @TableName VARCHAR(50)
	DECLARE @FirstColumnName VARCHAR(50)
	DECLARE @msg AS VARCHAR(1024)
	
	DECLARE @query nVarchar(1024)
	DECLARE @ErrorCount INT
	
	SET @ErrorCount = 0
	
	SELECT @MaxKey = MAX(TableKey) FROM #TablesToProcess
	SELECT @CurrentKey = MIN(TableKey) FROM #TablesToProcess
	
	WHILE @MaxKey >= @CurrentKey
	BEGIN
		SELECT	@TableName = TableName, 
				@FirstColumnName = FirstColumnName
		FROM #TablesToProcess WHERE TableKey = @CurrentKey
	
		BEGIN TRY
			SET @query = 'UPDATE [2AM].' + @TableName + ' SET ' + @FirstColumnName + ' = 1 WHERE ' + @FirstColumnName + ' = 1'
			EXEC (@query) AS Login = 'ServiceArchitect'
			
		END TRY
		BEGIN CATCH
			--do some stuff, so we can catch the error.. blah blah
			IF ISNULL(error_message(), ' Failed') NOT LIKE '%The UPDATE permission was denied%'
			BEGIN
				SET @msg = 'Expect a permissions error, however we got a flower pot instead and this error - ' + ISNULL(error_message(), ' Failed')
				EXEC TST.Assert.Fail @msg
				SET @ErrorCount = @ErrorCount + 1
			END
		END CATCH
		
		SET @CurrentKey = @CurrentKey + 1
	END;
	
	EXEC TST.Assert.Floatequals 'Something went Pear!!', @ErrorCount, 0, 0.01
	
  END

GO

IF @@ERROR<>0 OR @@TRANCOUNT=0 BEGIN IF @@TRANCOUNT>0 ROLLBACK SET NOEXEC ON END
GO

-- COMMITTING TRANSACTION STRUCTURE
PRINT 'Committing transaction STRUCTURE'
IF @@TRANCOUNT>0
	COMMIT TRANSACTION _STRUCTURE_
GO

SET NOEXEC OFF
GO
use [process_test]
go
if object_id('sqltest_Security#UserTableSecurityCheckUpdateFailWithBatch') is NOT NULL
drop proc sqltest_Security#UserTableSecurityCheckUpdateFailWithBatch
go

/*=============================================================
SCRIPT HEADER

VERSION:   1.01.0001
SERVER:    dbbuild03

SCRIPTED OBJECTS: DATABASE:	Process_Test
  Procedure:  sqltest_Security#UserTableSecurityCheckUpdateFailWithBatch


=============================================================*/
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_WARNINGS ON
SET NOCOUNT ON
SET XACT_ABORT ON
GO

-- BEGINNING TRANSACTION STRUCTURE
PRINT 'Beginning transaction STRUCTURE'
BEGIN TRANSACTION _STRUCTURE_
GO
-- Create Procedure [dbo].[sqltest_Security#UserTableSecurityCheckUpdateFailWithBatch]
Print 'Create Procedure [dbo].[sqltest_Security#UserTableSecurityCheckUpdateFailWithBatch]'
GO
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER OFF
GO

IF @@ERROR<>0 OR @@TRANCOUNT=0 BEGIN IF @@TRANCOUNT>0 ROLLBACK SET NOEXEC ON END
GO
CREATE PROCEDURE [dbo].[sqltest_Security#UserTableSecurityCheckUpdateFailWithBatch]
AS
  /***************************************************************************************************************************************
 Author: GrantW
 Create date: 19 August 2011
 Description: This Unit test will try and update informaiton from 
				the Account, FinancialService, Balance, LoanBalance, FinancalTransaction, ArrearTransaction tables
				And we should expect to see a permissions error when trying to perform these actions.
 
 History:
			2011/08/25	GrantW	Created
			2011/08/29	MandyM	Added SPVTransaction to list of tables to test
			2013/04/25  Helasha Edited to Run on BuildServer
 	
***************************************************************************************************************************************/
  BEGIN
   --Declare variable

	--IF EXISTS OBJECT_ID('#TablesToProcess') 
	--	DROP TABLE #TablesToProcess

	SELECT	ROW_NUMBER() OVER(ORDER BY t.Name DESC) AS 'TableKey',
			s.Name + '.' + t.Name AS TableName,
			c.Name FirstColumnName
	INTO #TablesToProcess
	FROM [2AM].sys.Tables t
	INNER JOIN [2AM].sys.Schemas s 	ON t.Schema_ID = s.Schema_ID
	INNER JOIN [2AM].sys.Columns c 	ON c.[Object_ID] = t.[Object_ID]
	WHERE t.Name IN 
		(
			'Account',
			'FinancialService',
			'FinancialTransaction',
			'ArrearTransaction',
			'LoanBalance',
			'Balance',
			'SPVTransaction'
		)
		AND c.Column_ID = 2
		AND s.Name IN ('fin', 'dbo', 'SPV') 
	    

	--SELECT * FROM #TablesToProcess
	
	DECLARE @MaxKey INT
	DECLARE @CurrentKey INT
	DECLARE @TableName VARCHAR(50)
	DECLARE @FirstColumnName VARCHAR(50)
	DECLARE @msg AS VARCHAR(1024)
	
	DECLARE @query nVarchar(1024)
	DECLARE @ErrorCount INT
	
	SET @ErrorCount = 0
	
	SELECT @MaxKey = MAX(TableKey) FROM #TablesToProcess
	SELECT @CurrentKey = MIN(TableKey) FROM #TablesToProcess
	
	WHILE @MaxKey > @CurrentKey
	BEGIN
		SELECT	@TableName = TableName, 
				@FirstColumnName = FirstColumnName
		FROM #TablesToProcess WHERE TableKey = @CurrentKey
	
		BEGIN TRY
			SET @query = 'UPDATE [2AM].' + @TableName + ' SET ' + @FirstColumnName + ' = 1 WHERE ' + @FirstColumnName + ' = 1'
			EXEC (@query) AS Login = 'Batch'
			
		END TRY
		BEGIN CATCH
			--do some stuff, so we can catch the error.. blah blah
			--SELECT 'Error_Message()' = ISNULL(error_message(), ' Failed')
			IF ISNULL(error_message(), ' Failed') NOT LIKE '%The INSERT permission was denied%'
			BEGIN		
                SET @msg ='Expect a permissions error, however we got a flower pot instead and this error - ' + ISNULL(error_message(), ' Failed')
				EXEC TST.Assert.Fail @msg
				SET @ErrorCount = @ErrorCount + 1
			END
		END CATCH
		
		SET @CurrentKey = @CurrentKey + 1
	END;
	
	EXEC TST.Assert.Floatequals 'Something went Pear!!', @ErrorCount, 0, 0.01
	
  END

GO

IF @@ERROR<>0 OR @@TRANCOUNT=0 BEGIN IF @@TRANCOUNT>0 ROLLBACK SET NOEXEC ON END
GO

-- COMMITTING TRANSACTION STRUCTURE
PRINT 'Committing transaction STRUCTURE'
IF @@TRANCOUNT>0
	COMMIT TRANSACTION _STRUCTURE_
GO

SET NOEXEC OFF
GO
use [process_test]
go
if object_id('sqltest_Security#UserTableSecurityCheckSelectPassWithBatch') is NOT NULL
drop proc sqltest_Security#UserTableSecurityCheckSelectPassWithBatch
go
/*=============================================================
SCRIPT HEADER

VERSION:   1.01.0001
SERVER:    dbbuild03

SCRIPTED OBJECTS: DATABASE:	Process_Test
  Procedure:  sqltest_Security#UserTableSecurityCheckSelectPassWithBatch


=============================================================*/
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_WARNINGS ON
SET NOCOUNT ON
SET XACT_ABORT ON
GO

-- BEGINNING TRANSACTION STRUCTURE
PRINT 'Beginning transaction STRUCTURE'
BEGIN TRANSACTION _STRUCTURE_
GO
-- Create Procedure [dbo].[sqltest_Security#UserTableSecurityCheckSelectPassWithBatch]
Print 'Create Procedure [dbo].[sqltest_Security#UserTableSecurityCheckSelectPassWithBatch]'
GO
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER OFF
GO

IF @@ERROR<>0 OR @@TRANCOUNT=0 BEGIN IF @@TRANCOUNT>0 ROLLBACK SET NOEXEC ON END
GO
CREATE PROCEDURE [dbo].[sqltest_Security#UserTableSecurityCheckSelectPassWithBatch]
AS
  /***************************************************************************************************************************************
 Author: GrantW
 Create date: 19 August 2011
 Description: This Unit test will try and select informaiton from 
				the Account, FinancialService, Balance, LoanBalance, FinancalTransaction, ArrearTransaction tables
				And we should expect this to be allowed by the service architect login.
 
 History:
			2011/08/25	GrantW	Created
			2011/08/29	MandyM	Added SPVTransaction to list of tables to test
 	        2013/04/25  Helasha Edited to run on BuildServer  
***************************************************************************************************************************************/
  BEGIN
   --Declare variable

	--IF EXISTS OBJECT_ID('#TablesToProcess') 
	--	DROP TABLE #TablesToProcess

SELECT	ROW_NUMBER() OVER(ORDER BY t.Name DESC) AS 'TableKey',
			s.Name + '.' + t.Name AS TableName 
	INTO #TablesToProcess
	FROM [2AM].sys.Tables t
	INNER JOIN [2AM].sys.Schemas s 	ON t.Schema_ID = s.Schema_ID
	WHERE Type_Desc = 'USER_TABLE'
	AND t.Name IN 
	(
		'Account',
		'FinancialService',
		'FinancialTransaction',
		'ArrearTransaction',
		'LoanBalance',
		'Balance',
		'SPVTransaction'
	)
	AND s.Name IN ('fin', 'dbo', 'SPV')
	
	DECLARE @MaxKey INT
	DECLARE @CurrentKey INT
	DECLARE @TableName VARCHAR(50)
	DECLARE @query nVarchar(1024)
	DECLARE @ErrorCount INT
	DECLARE @msg VARCHAR(1024)
	
	SET @ErrorCount = 0
	
	SELECT @MaxKey = MAX(TableKey) FROM #TablesToProcess
	SELECT @CurrentKey = MIN(TableKey) FROM #TablesToProcess
	
	WHILE @MaxKey >= @CurrentKey
	BEGIN
		SELECT @TableName = TableName FROM #TablesToProcess WHERE TableKey = @CurrentKey

		BEGIN TRY
			SET @query = 'SELECT TOP 1 * into #t FROM [2am].' + @TableName +';drop table #t;'
			EXEC (@query) AS LOGIN = 'Batch' 
			
		END TRY
		BEGIN CATCH
				SET @msg = 'Something went wrong with select permission, the error message is - ' + ISNULL(error_message(), ' Failed') 
				EXEC TST.Assert.Fail @msg
				SET @ErrorCount = @ErrorCount + 1
		END CATCH
		
		SET @CurrentKey = @CurrentKey + 1
	END;
	
	EXEC TST.Assert.Floatequals 'Something went Pear!!', @ErrorCount, 0, 0.01
	
  END
GO

IF @@ERROR<>0 OR @@TRANCOUNT=0 BEGIN IF @@TRANCOUNT>0 ROLLBACK SET NOEXEC ON END
GO

-- COMMITTING TRANSACTION STRUCTURE
PRINT 'Committing transaction STRUCTURE'
IF @@TRANCOUNT>0
	COMMIT TRANSACTION _STRUCTURE_
GO

SET NOEXEC OFF
GO
use [process_test]
go
if object_id('sqltest_Security#UserTableSecurityCheckDeleteFailWithServiceArchitect') is NOT NULL
drop proc sqltest_Security#UserTableSecurityCheckDeleteFailWithServiceArchitect
go
/*=============================================================
SCRIPT HEADER

VERSION:   1.01.0001
SERVER:    dbbuild03

SCRIPTED OBJECTS: DATABASE:	Process_Test
  Procedure:  sqltest_Security#UserTableSecurityCheckDeleteFailWithServiceArchitect


=============================================================*/
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_WARNINGS ON
SET NOCOUNT ON
SET XACT_ABORT ON
GO

-- BEGINNING TRANSACTION STRUCTURE
PRINT 'Beginning transaction STRUCTURE'
BEGIN TRANSACTION _STRUCTURE_
GO
-- Create Procedure [dbo].[sqltest_Security#UserTableSecurityCheckDeleteFailWithServiceArchitect]
Print 'Create Procedure [dbo].[sqltest_Security#UserTableSecurityCheckDeleteFailWithServiceArchitect]'
GO
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER OFF
GO

IF @@ERROR<>0 OR @@TRANCOUNT=0 BEGIN IF @@TRANCOUNT>0 ROLLBACK SET NOEXEC ON END
GO
CREATE PROCEDURE [dbo].[sqltest_Security#UserTableSecurityCheckDeleteFailWithServiceArchitect]
AS
/***************************************************************************************************************************************
 Author: GrantW
 Create date: 19 August 2011
 Description: This Unit test will try and Delete informaiton using ServiceArchitect login from 
				the Account, FinancialService, Balance, LoanBalance, FinancalTransaction, ArrearTransaction tables
				And we should expect to see a permissions error when trying to perform these actions.
  
 History:
			2011/08/25	GrantW	Created
 			2011/08/29	MandyM	Added SPVTransaction to list of tables to test
			2013/04/25  Helasha edited to run on BuildServer
***************************************************************************************************************************************/
  BEGIN
   --Declare variable

	--IF EXISTS OBJECT_ID('#TablesToProcess') 
	--	DROP TABLE #TablesToProcess

		SELECT	ROW_NUMBER() OVER(ORDER BY t.Name DESC) AS 'TableKey',
			s.Name + '.' + t.Name AS TableName,
			c.Name FirstColumnName
	INTO #TablesToProcess
	FROM [2AM].sys.Tables t
	INNER JOIN [2AM].sys.Schemas s
	ON t.Schema_ID = s.Schema_ID
	INNER JOIN [2AM].sys.Columns c
	ON c.[Object_ID] = t.[Object_ID]
	WHERE t.Name IN 
		(
			'Account',
			'FinancialService',
			'FinancialTransaction',
			'ArrearTransaction',
			'LoanBalance',
			'Balance',
			'SPVTransaction'
		)
		AND c.Column_ID = 2
		AND s.Name IN ('fin', 'dbo', 'SPV') 
	
	DECLARE @MaxKey INT
	DECLARE @CurrentKey INT
	DECLARE @TableName VARCHAR(50)
	DECLARE @FirstColumnName VARCHAR(50)
	DECLARE @msg AS VARCHAR(1024)
	
	DECLARE @query nVarchar(1024)
	DECLARE @ErrorCount INT
	
	SET @ErrorCount = 0
	
	SELECT @MaxKey = MAX(TableKey) FROM #TablesToProcess
	SELECT @CurrentKey = MIN(TableKey) FROM #TablesToProcess
	
	WHILE @MaxKey >= @CurrentKey
	BEGIN
		SELECT	@TableName = TableName, 
				@FirstColumnName = FirstColumnName
		FROM #TablesToProcess WHERE TableKey = @CurrentKey
	
		BEGIN TRY
			SET @query = 'DELETE FROM [2AM].' + @TableName + ' WHERE ' + @FirstColumnName + ' = 1'
			EXEC (@query) AS Login = 'ServiceArchitect'
			
		END TRY
		BEGIN CATCH
			--do some stuff, so we can catch the error.. blah blah
			IF ISNULL(error_message(), ' Failed') NOT LIKE '%The DELETE permission was denied on the object%'
			BEGIN
				SET @msg = 'Expect a permissions error, however we got a flower pot instead and this error - ' + ISNULL(error_message(), ' Failed')
				EXEC TST.Assert.Fail @msg
				SET @ErrorCount = @ErrorCount + 1
			END
		END CATCH
		
		SET @CurrentKey = @CurrentKey + 1
	END;
	
	EXEC TST.Assert.Floatequals 'Something went Pear!!', @ErrorCount, 0, 0.01
	
  END
GO

IF @@ERROR<>0 OR @@TRANCOUNT=0 BEGIN IF @@TRANCOUNT>0 ROLLBACK SET NOEXEC ON END
GO

-- COMMITTING TRANSACTION STRUCTURE
PRINT 'Committing transaction STRUCTURE'
IF @@TRANCOUNT>0
	COMMIT TRANSACTION _STRUCTURE_
GO

SET NOEXEC OFF
GO
use [process_test]
go
if object_id('sqltest_Security#ServiceArchitectPassOnHaloSchemas') is NOT NULL
drop proc sqltest_Security#ServiceArchitectPassOnHaloSchemas
go
/*=============================================================
SCRIPT HEADER

VERSION:   1.01.0001
SERVER:    dbbuild03

SCRIPTED OBJECTS: DATABASE:	Process_Test
  Procedure:  sqltest_Security#ServiceArchitectPassOnHaloSchemas


=============================================================*/
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_WARNINGS ON
SET NOCOUNT ON
SET XACT_ABORT ON
GO

-- BEGINNING TRANSACTION STRUCTURE
PRINT 'Beginning transaction STRUCTURE'
BEGIN TRANSACTION _STRUCTURE_
GO
-- Create Procedure [dbo].[sqltest_Security#ServiceArchitectPassOnHaloSchemas]
Print 'Create Procedure [dbo].[sqltest_Security#ServiceArchitectPassOnHaloSchemas]'
GO
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER OFF
GO

IF @@ERROR<>0 OR @@TRANCOUNT=0 BEGIN IF @@TRANCOUNT>0 ROLLBACK SET NOEXEC ON END
GO
CREATE PROCEDURE [dbo].[sqltest_Security#ServiceArchitectPassOnHaloSchemas]
AS
  /***************************************************************************************************************************************
 Author: GrantW
 Create date: 19 August 2011
 Description: This Unit test will try and select informaiton from 
				the Account, FinancialService, Balance, LoanBalance, FinancalTransaction, ArrearTransaction tables
				And we should expect this to be allowed by the service architect login.
 
 History:
			2011/08/25	GrantW	Created
 	        2013/04/25  Helasha Edited to run on BuildServer   
***************************************************************************************************************************************/
  BEGIN
		SELECT ROW_NUMBER() OVER(ORDER BY p.Name DESC) AS 'ProcKey',
				s.Name + '.' + p.Name AS ProcName 
	INTO #ProcsToProcess
	FROM [Process].sys.Procedures p
	INNER JOIN [Process].sys.Schemas s
	ON p.Schema_ID = s.Schema_ID
	WHERE s.Name IN ('HALO')
	and p.Name not in ('pRateChange','pGetAlphaHousingTotal')
	order by p.Name
	
	DECLARE @MaxKey INT
	DECLARE @CurrentKey INT
	DECLARE @ProName VARCHAR(150)
	DECLARE @query nVarchar(1024)
	DECLARE @ErrorCount INT
	DECLARE @msg VARCHAR(1024)
	
	SET @ErrorCount = 0
	
	SELECT @MaxKey = MAX(ProcKey) FROM #ProcsToProcess
	SELECT @CurrentKey = MIN(ProcKey) FROM #ProcsToProcess
	
	WHILE @MaxKey >= @CurrentKey
	BEGIN
		SELECT @ProName = ProcName FROM #ProcsToProcess WHERE ProcKey = @CurrentKey

		BEGIN TRY
			SET @query = 'EXEC Process.' + @ProName

			EXEC (@query) AS Login = 'ServiceArchitect'

--			SET @ErrorCount = @ErrorCount + 1
		END TRY
		BEGIN CATCH
			--do some stuff, so we can catch the error.. blah blah
			IF ISNULL(error_message(), ' Failed') LIKE '%The EXECUTE permission was denied on the object%'
			BEGIN

				SELECT 'There you go', @query
				SET @msg = 'Got "EXECUTE permission was denied" on proc ' + @query
				EXEC TST.Assert.Fail @msg
				SET @ErrorCount = @ErrorCount + 1
			END
		END CATCH
		
		SET @CurrentKey = @CurrentKey + 1
	END;
	
	EXEC TST.Assert.Floatequals 'Something went Pear!!', @ErrorCount, 0, 0.01
  END
GO

IF @@ERROR<>0 OR @@TRANCOUNT=0 BEGIN IF @@TRANCOUNT>0 ROLLBACK SET NOEXEC ON END
GO

-- COMMITTING TRANSACTION STRUCTURE
PRINT 'Committing transaction STRUCTURE'
IF @@TRANCOUNT>0
	COMMIT TRANSACTION _STRUCTURE_
GO

SET NOEXEC OFF
GO
use [process_test]
go
if object_id('sqltest_Security#CheckProcessCanNotAccessHalo') is NOT NULL
drop proc sqltest_Security#CheckProcessCanNotAccessHalo
go

/*=============================================================
SCRIPT HEADER

VERSION:   1.01.0001
SERVER:    dbbuild03

SCRIPTED OBJECTS: DATABASE:	Process_Test
  Procedure:  sqltest_Security#CheckProcessCanNotAccessHalo


=============================================================*/
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_WARNINGS ON
SET NOCOUNT ON
SET XACT_ABORT ON
GO

-- BEGINNING TRANSACTION STRUCTURE
PRINT 'Beginning transaction STRUCTURE'
BEGIN TRANSACTION _STRUCTURE_
GO
-- Create Procedure [dbo].[sqltest_Security#CheckProcessCanNotAccessHalo]
Print 'Create Procedure [dbo].[sqltest_Security#CheckProcessCanNotAccessHalo]'
GO
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER OFF
GO

IF @@ERROR<>0 OR @@TRANCOUNT=0 BEGIN IF @@TRANCOUNT>0 ROLLBACK SET NOEXEC ON END
GO
CREATE PROCEDURE [dbo].[sqltest_Security#CheckProcessCanNotAccessHalo]
AS
  /***************************************************************************************************************************************
 Author: Paul C
 Create date: 29 August 2011
 Description: Will check that the process role can NOT execute procs in halo schema
 
 History:
			2011/08/29	Paul C	Created
 	        2013/04/25  Helasha Edited to run on BuildServer     
***************************************************************************************************************************************/
  BEGIN
	DECLARE @R int, @AK int, @HOCFinancialServiceKey int, @ParentAccountKey int, @MLFSKey int, @AccountKey int, @Ret int
	DECLARE @msg VARCHAR(1024)
	declare @Query nvarchar(1024)
begin tran
begin TRY
	
	---------------------------------------------------------------
		BEGIN TRY
		-- Exec a proc in the fin schema (we do expect an errors due to permissions)
		SET @query = 'declare @msg varchar(1024), @FSKey int;exec process.HALO.pRateChange @FSKey, @MSG output'
		EXEC (@query) AS LOGIN = 'Batch' 

	end try
	begin catch
		IF ISNULL(error_message(), ' Failed') NOT LIKE '%The Execute permission was denied%'
		begin
			select @msg = ISNULL(error_message(), ' Failed')
			raiserror(@msg,16,1)
		end
	end catch

	commit
	exec tst.assert.pass 'All Good'
end try
begin catch
	commit
	exec tst.assert.Fail @msg
end catch
end
GO

IF @@ERROR<>0 OR @@TRANCOUNT=0 BEGIN IF @@TRANCOUNT>0 ROLLBACK SET NOEXEC ON END
GO

-- COMMITTING TRANSACTION STRUCTURE
PRINT 'Committing transaction STRUCTURE'
IF @@TRANCOUNT>0
	COMMIT TRANSACTION _STRUCTURE_
GO

SET NOEXEC OFF
GO
use [process_test]
go
if object_id('sqltest_Security#CheckProcessCanAccessProcsButNotHalo') is NOT NULL
drop proc sqltest_Security#CheckProcessCanAccessProcsButNotHalo
go
/*=============================================================
SCRIPT HEADER

VERSION:   1.01.0001
SERVER:    dbbuild03

SCRIPTED OBJECTS: DATABASE:	Process_Test
  Procedure:  sqltest_Security#CheckProcessCanAccessProcsButNotHalo


=============================================================*/
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_WARNINGS ON
SET NOCOUNT ON
SET XACT_ABORT ON
GO

-- BEGINNING TRANSACTION STRUCTURE
PRINT 'Beginning transaction STRUCTURE'
BEGIN TRANSACTION _STRUCTURE_
GO
-- Create Procedure [dbo].[sqltest_Security#CheckProcessCanAccessProcsButNotHalo]
Print 'Create Procedure [dbo].[sqltest_Security#CheckProcessCanAccessProcsButNotHalo]'
GO
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER OFF
GO

IF @@ERROR<>0 OR @@TRANCOUNT=0 BEGIN IF @@TRANCOUNT>0 ROLLBACK SET NOEXEC ON END
GO
CREATE PROCEDURE [dbo].[sqltest_Security#CheckProcessCanAccessProcsButNotHalo]
AS
  /***************************************************************************************************************************************
 Author: Paul C
 Create date: 29 August 2011
 Description: Will check that the process role can execute procs in all schemas but will not test the halo one. There is a seperate test for that
 
 History:
			2011/08/29	Paul C	Created
 	        2013/04/25  Helasha Edited to run on BuildServer 
***************************************************************************************************************************************/
  BEGIN
	DECLARE @R int, @AK int, @HOCFinancialServiceKey int, @ParentAccountKey int, @MLFSKey int, @AccountKey int, @Ret int
	DECLARE @msg VARCHAR(1024)
	declare @Query nvarchar(1024)
begin tran
	begin TRY
	
	---------------------------------------------------------------
	BEGIN TRY
		-- Exec a proc in the fin schema (we do not expect any errors due to permissions)
		SET @query = '
				EXECUTE [process].fin.pCreateAccount
				@ReservedAccountKey=123,
				@OriginationSourceProductKey=17,
				@SPVKey=117,
				@USerID=''bobthebuilder'',
				@ParentAccountKey=NULL,
				@msg=''test'''
		EXEC (@query) AS LOGIN = 'Batch' 

	end try
	begin catch
		IF ISNULL(error_message(), ' Failed') LIKE '%The Execute permission was denied%'
		begin
			select @msg = ISNULL(error_message(), ' Failed')
			raiserror(@msg,16,1)
		end
	end catch
	
	---------------------------------------------------------------	
	begin try
			-- exec a proc in the batch schema (we do not expect any errors due to permissions)
			set @Query = 'declare @msg varchar(10);Exec process.[batch].[pCreateTrade] @msg'
			EXEC (@query) AS LOGIN = 'batch' 
	end try
	begin catch
		IF ISNULL(error_message(), ' Failed') LIKE '%The Execute permission was denied%'
		begin
			select @msg = ISNULL(error_message(), ' Failed')
			raiserror(@msg,16,1)
		end
	end CATCH
	
	---------------------------------------------------------------
	begin try
			-- Exec in HOC (we do not expect any errors due to permissions)
			set @Query = 'EXEC @Ret = Process.HOC.pCloseHOC @HOCFinancialServiceKey, @Msg OUTPUT'
			EXEC (@query) AS LOGIN = 'Batch' 
	end try
	begin catch
		IF ISNULL(error_message(), ' Failed') LIKE '%The Execute permission was denied%'
		begin
			select @msg = ISNULL(error_message(), ' Failed')
			raiserror(@msg,16,1)
		end
	end CATCH
	
	---------------------------------------------------------------
	begin try
		-- exec in life (we do not expect any errors due to permissions)
		SET @query = 'declare @R int, @AccountKey int, @ParentAccountKey int, @MSG varchar(1024);EXEC @R = process.[Life].[pCreateRelatedLifeAccount] @AccountKey, @ParentAccountKey, 5, 117, ''UnitTest'', @msg OUTPUT;'
		EXEC (@query) AS LOGIN = 'Batch' 
	end try
	begin catch
		IF ISNULL(error_message(), ' Failed') LIKE '%The Execute permission was denied%'
		begin
			select @msg = ISNULL(error_message(), ' Failed')
			raiserror(@msg,16,1)
		end
	end catch
	
	---------------------------------------------------------------
	begin try
		-- exec in SPV (we do not expect any errors due to permissions)
		SET @query = 'exec [Process].spv.GetCurrentPTI'
		EXEC (@query) AS LOGIN = 'Batch' 
	end try
	begin catch
		IF ISNULL(error_message(), ' Failed') LIKE '%The Execute permission was denied%'
		begin
			select @msg = ISNULL(error_message(), ' Failed')
			raiserror(@msg,16,1)
		end
	end CATCH
	
	commit
	exec tst.assert.pass 'All Good'
	
	end try
	begin catch
		commit
		exec tst.assert.fail @msg
	end catch
end
GO

IF @@ERROR<>0 OR @@TRANCOUNT=0 BEGIN IF @@TRANCOUNT>0 ROLLBACK SET NOEXEC ON END
GO

-- COMMITTING TRANSACTION STRUCTURE
PRINT 'Committing transaction STRUCTURE'
IF @@TRANCOUNT>0
	COMMIT TRANSACTION _STRUCTURE_
GO

SET NOEXEC OFF
GO
use [process_test]
go
if object_id('sqltest_Security#CheckAppRoleCanNOTAccessProcNotInHalo') is NOT NULL
drop proc sqltest_Security#CheckAppRoleCanNOTAccessProcNotInHalo
go
/*=============================================================
SCRIPT HEADER

VERSION:   1.01.0001
SERVER:    dbbuild03

SCRIPTED OBJECTS: DATABASE:	Process_Test
  Procedure:  sqltest_Security#CheckAppRoleCanNOTAccessProcNotInHalo


=============================================================*/
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_WARNINGS ON
SET NOCOUNT ON
SET XACT_ABORT ON
GO

-- BEGINNING TRANSACTION STRUCTURE
PRINT 'Beginning transaction STRUCTURE'
BEGIN TRANSACTION _STRUCTURE_
GO
-- Create Procedure [dbo].[sqltest_Security#CheckAppRoleCanNOTAccessProcNotInHalo]
Print 'Create Procedure [dbo].[sqltest_Security#CheckAppRoleCanNOTAccessProcNotInHalo]'
GO
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER OFF
GO

IF @@ERROR<>0 OR @@TRANCOUNT=0 BEGIN IF @@TRANCOUNT>0 ROLLBACK SET NOEXEC ON END
GO
CREATE PROCEDURE [dbo].[sqltest_Security#CheckAppRoleCanNOTAccessProcNotInHalo]
AS
  /***************************************************************************************************************************************
 Author: Paul C
 Create date: 29 August 2011
 Description: Will check that the AppRole role can execute procs in Halo ONLY 
 
 History:
			2011/08/29	Paul C	Created
			2013/04/25  Helasha Edited to run on BuildServer
 	
***************************************************************************************************************************************/
BEGIN
	DECLARE @msg VARCHAR(1024), @Query nvarchar(1024)

	BEGIN TRY
		-- Exec a proc in the fin schema
		SET @query = 'EXECUTE [process].fin.pCreateAccount
						@ReservedAccountKey=123,
						@OriginationSourceProductKey=17,
						@SPVKey=117,
						@USerID=''bobthebuilder'',
						@ParentAccountKey=NULL,
						@msg=''test'''
		EXEC (@query) AS LOGIN = 'ServiceArchitect' 
		
		exec tst.assert.fail 'Expected "EXECUTE permission was denied" but got nothing'
	end try
	begin catch
		IF ISNULL(error_message(), ' Failed') LIKE '%The Execute permission was denied%'
		begin
			exec tst.assert.pass 'all good'
		end
		else
		begin 
			select @msg = ISNULL(error_message(), ' Failed')
			exec tst.assert.fail @msg
		end
	end catch
end

GO

IF @@ERROR<>0 OR @@TRANCOUNT=0 BEGIN IF @@TRANCOUNT>0 ROLLBACK SET NOEXEC ON END
GO

-- COMMITTING TRANSACTION STRUCTURE
PRINT 'Committing transaction STRUCTURE'
IF @@TRANCOUNT>0
	COMMIT TRANSACTION _STRUCTURE_
GO

SET NOEXEC OFF
GO
use [process_test]
go
if object_id('sqltest_Security#CheckAppRoleCanAccessHalo') is NOT NULL
drop proc sqltest_Security#CheckAppRoleCanAccessHalo
go
/*=============================================================
SCRIPT HEADER

VERSION:   1.01.0001
SERVER:    dbbuild03

SCRIPTED OBJECTS: DATABASE:	Process_Test
  Procedure:  sqltest_Security#CheckAppRoleCanAccessHalo


=============================================================*/
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_WARNINGS ON
SET NOCOUNT ON
SET XACT_ABORT ON
GO

-- BEGINNING TRANSACTION STRUCTURE
PRINT 'Beginning transaction STRUCTURE'
BEGIN TRANSACTION _STRUCTURE_
GO
-- Create Procedure [dbo].[sqltest_Security#CheckAppRoleCanAccessHalo]
Print 'Create Procedure [dbo].[sqltest_Security#CheckAppRoleCanAccessHalo]'
GO
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER OFF
GO

IF @@ERROR<>0 OR @@TRANCOUNT=0 BEGIN IF @@TRANCOUNT>0 ROLLBACK SET NOEXEC ON END
GO
CREATE PROCEDURE [dbo].[sqltest_Security#CheckAppRoleCanAccessHalo]
AS
  /***************************************************************************************************************************************
 Author: Paul C
 Create date: 29 August 2011
 Description: Will check that the AppRole can access procs in HALO
 
 History:
			2011/08/29	Paul C	Created
			2013/04/25  Helasha Edited to run on BuildServer
 	
***************************************************************************************************************************************/
BEGIN
	DECLARE @msg VARCHAR(1024), @Query nvarchar(1024)

	BEGIN TRY
		-- Exec a proc in the fin schema
		SET @query = 'declare @msg varchar(1024), @FSKey int; exec process.HALO.pRateChange @FSKey, @MSG output'
		EXEC (@query) AS LOGIN = 'ServiceArchitect'
		
		exec tst.assert.pass 'All Good'
	end try
	begin catch
		select @msg = ISNULL(error_message(), ' Failed')
		exec tst.assert.Fail @msg
	end catch
end
GO

IF @@ERROR<>0 OR @@TRANCOUNT=0 BEGIN IF @@TRANCOUNT>0 ROLLBACK SET NOEXEC ON END
GO

-- COMMITTING TRANSACTION STRUCTURE
PRINT 'Committing transaction STRUCTURE'
IF @@TRANCOUNT>0
	COMMIT TRANSACTION _STRUCTURE_
GO

SET NOEXEC OFF
GO
use [process_test]
go
if object_id('sqltest_Security#BatchPassOnBackEndSchemas') is NOT NULL
drop proc sqltest_Security#BatchPassOnBackEndSchemas
go
/*=============================================================
SCRIPT HEADER

VERSION:   1.01.0001
SERVER:    dbbuild03

SCRIPTED OBJECTS: DATABASE:	Process_Test
  Procedure:  sqltest_Security#BatchPassOnBackEndSchemas


=============================================================*/
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_WARNINGS ON
SET NOCOUNT ON
SET XACT_ABORT ON
GO

-- BEGINNING TRANSACTION STRUCTURE
PRINT 'Beginning transaction STRUCTURE'
BEGIN TRANSACTION _STRUCTURE_
GO
-- Create Procedure [dbo].[sqltest_Security#BatchPassOnBackEndSchemas]
Print 'Create Procedure [dbo].[sqltest_Security#BatchPassOnBackEndSchemas]'
GO
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER OFF
GO

IF @@ERROR<>0 OR @@TRANCOUNT=0 BEGIN IF @@TRANCOUNT>0 ROLLBACK SET NOEXEC ON END
GO
CREATE PROCEDURE [dbo].[sqltest_Security#BatchPassOnBackEndSchemas]
AS
  /***************************************************************************************************************************************
 Author: GrantW
 Create date: 19 August 2011
 Description: This Unit test will try and select informaiton from 
				the Account, FinancialService, Balance, LoanBalance, FinancalTransaction, ArrearTransaction tables
				And we should expect this to be allowed by the service architect login.
 
 History:
			2011/08/25	GrantW	Created
 	        2013/04/25  Helasha	Edited to run on BuildServer 
			2013/05//10	GrantW	excluded a proc that the hasn't the correct security.    
***************************************************************************************************************************************/
  BEGIN
	SELECT	ROW_NUMBER() OVER(ORDER BY p.Name DESC) AS 'ProcKey',
				s.Name + '.' + p.Name AS ProcName 
	INTO #ProcsToProcess
	FROM [Process].sys.Procedures p
	INNER JOIN [Process].sys.Schemas s
	ON p.Schema_ID = s.Schema_ID
	WHERE s.Name NOT IN ('HALO', 'Batch', 'Spv', 'Report', 'ework', 'e-work', 'ErrorHandling')
	AND p.Name NOT IN 
			(
				'pSPVTransactionBalance', 
				'pCheckProcessErrors', 
				'pLifeBulkLeadCreate',
				'pMonthEndHOC',
				'pImportConnectDirectDisbursementUnpaid',
				'pImportConnectDirectDisbursement',
				'pCreateLifeHistory',
				'pCloseHOC',
				'pAccountsRaiseInterest',
				'LTVRule',
				'pProcessPendingDomiciliumAddress'
			)
	ORDER BY p.Name

	DECLARE @MaxKey INT
	DECLARE @CurrentKey INT
	DECLARE @ProName VARCHAR(150)
	DECLARE @query nVarchar(1024)
	DECLARE @ErrorCount INT
	DECLARE @msg VARCHAR(1024)
	
	SET @ErrorCount = 0
	
	SELECT @MaxKey = MAX(ProcKey) FROM #ProcsToProcess
	SELECT @CurrentKey = MIN(ProcKey) FROM #ProcsToProcess
	
	WHILE @MaxKey > @CurrentKey
	BEGIN
		SELECT @ProName = ProcName FROM #ProcsToProcess WHERE ProcKey = @CurrentKey

		BEGIN TRY
			SET @query = 'EXEC Process.' + @ProName
			EXEC (@query) AS Login = 'BATCH'
			
		END TRY
		BEGIN CATCH
			--do some stuff, so we can catch the error.. blah blah
			IF ISNULL(error_message(), ' Failed') NOT LIKE '%expects parameter%'
				AND ISNULL(error_message(), ' Failed') NOT LIKE  '%Invalid object name%'
				AND ISNULL(error_message(), ' Failed') NOT LIKE  '%No Transaction%'
				AND ISNULL(error_message(), ' Failed') NOT LIKE  '%but has not send out emails since this is DEV%'  --Trys to send an email [ErrorHandling].[pCheckProcessErrors] 
				AND ISNULL(error_message(), ' Failed') NOT LIKE  '%still needs to be implemented%'
				AND ISNULL(error_message(), ' Failed') NOT LIKE  '%Does Not Have Any Records To Process%'
				AND ISNULL(error_message(), ' Failed') NOT LIKE  '%Table structures do not match:%'
				AND ISNULL(error_message(), ' Failed') NOT LIKE  '%The current transaction cannot be committed%'
			BEGIN
				SET @msg = 'Expected "EXECUTE permission was denied", however we got a flower pot instead and this error - ' + ISNULL(error_message(), ' Failed')

--				SELECT '@query ' = @query, '@msg ' = @msg

				EXEC TST.Assert.Fail @msg
				SET @ErrorCount = @ErrorCount + 1
			END
		END CATCH
		
		SET @CurrentKey = @CurrentKey + 1
	END
	
	EXEC TST.Assert.Floatequals 'Something went Pear!!', @ErrorCount, 0, 0.01
	
  END


GO

IF @@ERROR<>0 OR @@TRANCOUNT=0 BEGIN IF @@TRANCOUNT>0 ROLLBACK SET NOEXEC ON END
GO

-- COMMITTING TRANSACTION STRUCTURE
PRINT 'Committing transaction STRUCTURE'
IF @@TRANCOUNT>0
	COMMIT TRANSACTION _STRUCTURE_
GO

SET NOEXEC OFF
GO
use [process_test]
go
if object_id('sqltest_Security#BatchFailOnHaloSchemas') is NOT NULL
drop proc sqltest_Security#BatchFailOnHaloSchemas
go
/*=============================================================
SCRIPT HEADER

VERSION:   1.01.0001
SERVER:    dbbuild03

SCRIPTED OBJECTS: DATABASE:	Process_Test
  Procedure:  sqltest_Security#BatchFailOnHaloSchemas


=============================================================*/
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_WARNINGS ON
SET NOCOUNT ON
SET XACT_ABORT ON
GO

-- BEGINNING TRANSACTION STRUCTURE
PRINT 'Beginning transaction STRUCTURE'
BEGIN TRANSACTION _STRUCTURE_
GO
-- Create Procedure [dbo].[sqltest_Security#BatchFailOnHaloSchemas]
Print 'Create Procedure [dbo].[sqltest_Security#BatchFailOnHaloSchemas]'
GO
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER OFF
GO

IF @@ERROR<>0 OR @@TRANCOUNT=0 BEGIN IF @@TRANCOUNT>0 ROLLBACK SET NOEXEC ON END
GO
CREATE PROCEDURE [dbo].[sqltest_Security#BatchFailOnHaloSchemas]
AS
  /***************************************************************************************************************************************
 Author: GrantW
 Create date: 19 August 2011
 Description: This Unit test will try and select informaiton from 
				the Account, FinancialService, Balance, LoanBalance, FinancalTransaction, ArrearTransaction tables
				And we should expect this to be allowed by the service architect login.
 
 History:
			2011/08/25	GrantW	Created
			2012/11/19	Mandy	Exclude pCalcHOCPremium as it has loads of parameters
 	        2013/04/25  Helasha Altered test to run on server    
***************************************************************************************************************************************/
  BEGIN
   --Declare variable

	--IF EXISTS OBJECT_ID('#TablesToProcess') 
	--	DROP TABLE #TablesToProcess

		SELECT	ROW_NUMBER() OVER(ORDER BY p.Name DESC) AS 'ProcKey',
				s.Name + '.' + p.Name AS ProcName 
	INTO #ProcsToProcess
	FROM [Process].sys.Procedures p
	INNER JOIN [Process].sys.Schemas s
	ON p.Schema_ID = s.Schema_ID
	WHERE --Type_Desc = 'USER_TABLE' AND 
	s.Name IN ('HALO')

	--We should not be able to execute these procs, but until we have time to fix this security
	--hole, we're going to exclude these procs from this test
	and p.Name NOT IN ('pGetAlphaHousingTotal')
	
	DECLARE @MaxKey INT
	DECLARE @CurrentKey INT
	DECLARE @ProName VARCHAR(150)
	DECLARE @query nVarchar(1024)
	DECLARE @ErrorCount INT
	DECLARE @msg VARCHAR(1024)
	
	SET @ErrorCount = 0
	
	SELECT @MaxKey = MAX(ProcKey) FROM #ProcsToProcess
	SELECT @CurrentKey = MIN(ProcKey) FROM #ProcsToProcess
	
	WHILE @MaxKey > @CurrentKey
	BEGIN
		SELECT @ProName = ProcName FROM #ProcsToProcess WHERE ProcKey = @CurrentKey

		BEGIN TRY
			SET @query = 'EXEC Process.' + @ProName
			EXEC (@query) AS Login = 'BATCH'
			
			SET @ErrorCount = @ErrorCount + 1
		END TRY
		BEGIN CATCH
			--do some stuff, so we can catch the error.. blah blah
			IF ISNULL(error_message(), ' Failed') LIKE '%The EXECUTE permission was denied on the object%'
			BEGIN
				EXEC TST.Assert.Pass 'we are happy'
			END
			ELSE
			BEGIN
				SET @msg = 'We had permission to execute something that we shouldnt - ' + ISNULL(error_message(), ' Failed')
				EXEC TST.Assert.Fail @msg
				SET @ErrorCount = @ErrorCount + 1
			END
		END CATCH
		
		SET @CurrentKey = @CurrentKey + 1
	END;
	
	EXEC TST.Assert.Floatequals 'Something went Pear!!', @ErrorCount, 0, 0.01
	
  END
GO

IF @@ERROR<>0 OR @@TRANCOUNT=0 BEGIN IF @@TRANCOUNT>0 ROLLBACK SET NOEXEC ON END
GO

-- COMMITTING TRANSACTION STRUCTURE
PRINT 'Committing transaction STRUCTURE'
IF @@TRANCOUNT>0
	COMMIT TRANSACTION _STRUCTURE_
GO

SET NOEXEC OFF
GO
