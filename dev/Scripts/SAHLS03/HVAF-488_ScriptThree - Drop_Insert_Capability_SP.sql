use [2am]

go

IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'InsertCapability')

	BEGIN

		DROP PROCEDURE dbo.InsertCapability

	END

GO