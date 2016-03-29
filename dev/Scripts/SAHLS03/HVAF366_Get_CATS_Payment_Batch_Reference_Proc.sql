USE [2AM]
Go

IF EXISTS ( SELECT * FROM sys.objects WHERE name = N'GetNextCATSPaymentBatchReference' AND type IN ( N'P', N'PC' ) )
BEGIN
	DROP PROCEDURE [dbo].[GetNextCATSPaymentBatchReference] 
END 
Go
-- =============================================
-- Author:		Vincent Majavu
-- Create date: 2015-07-06
-- =============================================
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE GetNextCATSPaymentBatchReference @batchTypeKey INT, @batchKey INT OUT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	DECLARE @batchStatusKey int
	Set @batchKey = 0

	select @batchStatusKey = 1
	
	Insert into CATSPaymentBatch ([CreatedDate], [ProcessedDate], [CATSPaymentBatchStatusKey], [CATSPaymentBatchTypeKey])
	values (GetDate(),NULL, @batchStatusKey, @batchTypeKey);

	select @batchKey = SCOPE_IDENTITY()

END
GO

GRANT EXECUTE ON dbo.GetNextCATSPaymentBatchReference to AppRole 