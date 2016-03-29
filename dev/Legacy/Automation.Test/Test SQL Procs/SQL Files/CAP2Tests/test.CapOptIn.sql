USE [2AM]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF EXISTS (select * from dbo.sysobjects where id = object_id(N'test.CapOptIn') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
Begin
	Drop Procedure test.CapOptIn
	Print 'Dropped procedure test.CapOptIn'
End
Go

CREATE PROCEDURE test.CapOptIn

AS

BEGIN

	BEGIN TRAN ProcessTran
	DECLARE @Msg varchar(1024)
	DECLARE @RC int

		EXECUTE @RC = [Process].[product].[pCAPOptIn] @Msg OUTPUT

	IF @RC <> 0 OR ISNULL(@Msg, '') <> ''
		BEGIN
			ROLLBACK TRAN ProcessTran
		END
			ELSE
		BEGIN
			COMMIT TRAN ProcessTran
		END
END