USE [Process]
GO

IF OBJECT_ID('halo.pThrowApiError', 'P') IS NOT NULL
BEGIN
	DROP PROC [halo].[pThrowApiError]
END
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER OFF
GO

/********************************************************************************************************************************
Author: ClintS 
Create date: 20/08/2015
Description: This is only used for the x2 scenario map testing and should never get to PROD

*********************************************************************************************************************************/ 
create procedure [halo].[pThrowApiError]
@Msg varchar(1024) OUTPUT
AS
set nocount on;
declare @RetVal INT

set @RetVal = 0

BEGIN TRY

	SET @Msg = ''
	RAISERROR(@Msg, 16, 1)

END TRY
BEGIN CATCH
	SET @Msg = '[halo].[pThrowApiError] : ' + isnull(error_message(), 'Failed!')
	return 1
END CATCH
GO

GRANT EXECUTE ON [halo].[pThrowApiError] TO [AppRole] AS [dbo]
GO
