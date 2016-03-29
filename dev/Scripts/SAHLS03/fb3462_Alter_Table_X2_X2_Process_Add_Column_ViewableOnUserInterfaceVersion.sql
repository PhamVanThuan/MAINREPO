USE [X2]
GO

IF NOT EXISTS
(
	SELECT 1
	FROM sys.columns
	WHERE [name] = N'ViewableOnUserInterfaceVersion'
	AND [object_id] = OBJECT_ID(N'[X2].[X2].[Process]')
)
BEGIN

	ALTER TABLE [X2].[X2].[Process]
	ADD [ViewableOnUserInterfaceVersion] VARCHAR(50) NULL;

END