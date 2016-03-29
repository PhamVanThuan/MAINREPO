
USE [FeTest]
GO

IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'PopulateWorkflowSearch')
	DROP PROCEDURE dbo.PopulateWorkflowSearch
GO

CREATE PROCEDURE dbo.PopulateWorkflowSearch

AS

IF OBJECT_ID (N'dbo.WorkflowSearch', N'U') IS NOT NULL
BEGIN

	TRUNCATE TABLE dbo.WorkflowSearch

	DECLARE @STATE NVARCHAR(MAX)
	DECLARE @WORKFLOW NVARCHAR(MAX)
	DECLARE @DATATABLE NVARCHAR(MAX)
	DECLARE @WORKFLOWID INT
	DECLARE @STATEID INT
	DECLARE @QUERY NVARCHAR(MAX)
	DECLARE @cnt INT = 0
	DECLARE @cnt2 INT = 0
	DECLARE @ALLWORKFLOWS TABLE(
		ID INT,
		Name VARCHAR(max),
		Used bit
	)
	DECLARE @OUTPUT TABLE(
		InstanceID INT,
		Name VARCHAR(max),
		State VARCHAR(max)
	)
	DECLARE @ALLSTATES TABLE(
		ID INT,
		Name VARCHAR(max),
		Used bit
	)

	INSERT INTO @ALLWORKFLOWS
	SELECT DISTINCT w.id, w.name, 0 FROM x2.x2.WorkFlow w 
	WHERE w.id in (
		SELECT max(id) FROM x2.x2.WorkFlow w2
		WHERE w.name = w2.name
	) AND w.name not in ('Delete Debit Order','InterestOnlySMS','Quick Cash','RCS','Release And Variations','IT')

	WHILE @cnt <  (SELECT COUNT(ID) FROM @ALLWORKFLOWS)
	BEGIN
		SET @WORKFLOWID = (SELECT TOP 01 ID FROM @ALLWORKFLOWS WHERE Used = 0)
		SELECT  @DATATABLE = StorageTable FROM x2.x2.WorkFlow WHERE Id = @WORKFLOWID
		SELECT  @WORKFLOW = Name FROM x2.x2.WorkFlow WHERE Id = @WORKFLOWID
		PRINT 'LOOKING AT WORKFLOW ' +  @WORKFLOW 

		INSERT INTO @ALLSTATES
		SELECT DISTINCT s.id, s.name, 0 FROM x2.x2.State s
		WHERE s.WorkFlowID = @WORKFLOWID
		and s.Name in ('Application Capture', 'Manage Application', 'Credit', 'Loss Control Invoice Received', 'Invoice Captured', 'Contact Client', 'Open CAP2 Offer',
		'Manage Proposal','Process Client Request', 'Term Change Request', 'Accepted for Processing', 'Archive Disputes' --we need an archive state
		)

		WHILE @cnt2 <  (SELECT COUNT(ID) FROM @ALLSTATES)
		BEGIN
			SET @STATEID = (SELECT TOP 01 ID FROM @ALLSTATES WHERE Used = 0)
			SELECT @STATE = Name FROM @ALLSTATES WHERE Id = @STATEID
			SET @QUERY = N'INSERT INTO FETest.dbo.WorkflowSearch '
			SET @QUERY = @QUERY + N'SELECT TOP 1 i.ID as InstanceID, i.Subject, dataTable.GenericKey ,s.Name as State, w.Name as Workflow FROM X2.X2DATA.' + @DATATABLE + ' as dataTable'
			SET @QUERY = @QUERY + N' JOIN X2.X2.Instance i ON i.ID = dataTable.InstanceID and len(i.Subject) > 0'
			SET @QUERY = @QUERY + N' JOIN X2.X2.State s ON s.ID = i.StateID'
			SET @QUERY = @QUERY + N' JOIN X2.X2.Workflow w ON w.ID = i.WOrkflowID'
			SET @QUERY = @QUERY + N' WHERE s.Name = ''' + @STATE + '''AND i.Subject IS NOT NULL AND w.Name = ''' + @WORKFLOW + ''''
			SET @QUERY = @QUERY + N' ORDER BY dataTable.GenericKey DESC'
			SET @cnt2 = @cnt2 + 1
			EXEC sp_executesql @QUERY
			UPDATE @ALLSTATES SET Used = 1 WHERE Id = @STATEID
		END
		SET @cnt = @cnt + 1
		UPDATE @ALLWORKFLOWS SET Used = 1 WHERE Id = @WORKFLOWID
	END
END