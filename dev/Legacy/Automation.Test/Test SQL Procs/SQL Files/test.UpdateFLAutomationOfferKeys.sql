USE [2AM]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF EXISTS (select * from dbo.sysobjects where id = object_id(N'test.UpdateFLAutomationOfferKeys ') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
Begin
	Drop Procedure test.UpdateFLAutomationOfferKeys 
	Print 'Dropped Proc test.UpdateFLAutomationOfferKeys '
End
Go

CREATE PROCEDURE test.UpdateFLAutomationOfferKeys 

@AccountKey int 

AS

declare @ReadvanceOfferKey int
declare @FurtherAdvanceOfferKey int
declare @FurtherLoanOfferKey int

select @ReadvanceOfferKey = ISNULL(OfferKey,0) from [2am].dbo.Offer where AccountKey=@AccountKey and OfferTypeKey=2 and OfferStatusKey=1
select @FurtherAdvanceOfferKey = ISNULL(OfferKey,0) from [2am].dbo.Offer where AccountKey=@AccountKey and OfferTypeKey=3 and OfferStatusKey=1
select @FurtherLoanOfferKey = ISNULL(OfferKey,0) from [2am].dbo.Offer where AccountKey=@AccountKey and OfferTypeKey=4 and OfferStatusKey=1

update [2am].test.AutomationFLTestCases
set 
ReadvOfferKey = ISNULL(@ReadvanceOfferKey,0),
FAdvOfferKey = ISNULL(@FurtherAdvanceOfferKey,0),
FLOfferKey = ISNULL(@FurtherLoanOfferKey,0)
where AccountKey=@AccountKey




