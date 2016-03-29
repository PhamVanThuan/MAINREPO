USE [2AM]
IF EXISTS(SELECT * FROM SYS.OBJECTS WHERE name = N'GetNextInvoiceReference' AND TYPE IN (N'P', N'PC'))
   DROP PROCEDURE [dbo].GetNextInvoiceReference
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Alex Mhungu
-- Create date: 2015-03-27
-- =============================================
CREATE PROCEDURE GetNextInvoiceReference 
	@referenceNumber VARCHAR(100) OUT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	DECLARE @uniqueYearMonthPart VARCHAR(100)
	SELECT @uniqueYearMonthPart = 'SAHL'+ '-'+ CAST(DATEPART(yyyy, GETDATE()) AS VARCHAR(4))
									+ '/' 
									+ CASE WHEN DATEPART(mm, GETDATE()) <=9 THEN '0' 
									+ CAST(DATEPART(mm, GETDATE()) AS VARCHAR(4)) ELSE CAST(DATEPART(mm, GETDATE()) AS VARCHAR(4)) END 
									+ '/'

	IF NOT EXISTS(SELECT TOP 1 *
			FROM   
				[2AM].[dbo].ThirdPartyInvoice t
			WHERE  
				SUBSTRING(t.SahlReference,0, (LEN(t.SahlReference)+2 - CHARINDEX('/',REVERSE(t.SahlReference)))) =  @uniqueYearMonthPart) 
	BEGIN
		ALTER SEQUENCE dbo.Invoice_Seq RESTART
	END

	DECLARE @nextSequence int
	SELECT @nextSequence = NEXT VALUE FOR dbo.Invoice_Seq

	SELECT @referenceNumber = @uniqueYearMonthPart+ CAST(@nextSequence as VARCHAR(6))

END
GO

GRANT EXECUTE ON dbo.GetNextInvoiceReference to AppRole 