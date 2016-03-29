USE [2AM]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF EXISTS (select * from dbo.sysobjects where id = object_id(N'test.CancelFinancialAdjustments') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
Begin
	Drop Procedure test.CancelFinancialAdjustments
	Print 'Dropped procedure test.CancelFinancialAdjustments'
End
Go

CREATE PROCEDURE test.CancelFinancialAdjustments

@accountKey int

AS

begin

	--set all financial adjustment records for the account to cancelled
	update fadj
	set financialAdjustmentStatusKey = 3
	from [2am].dbo.financialService fs 
	join [2am].fin.financialAdjustment fadj on fadj.financialservicekey = fs.financialservicekey
		and fadj.financialAdjustmentStatusKey in (1,2)
	where fs.accountkey=@accountKey
	--we need close to any child financial services
	update child
	set accountstatuskey=2
	from [2am].dbo.Account a
	join [2am].dbo.financialService fs on a.accountKey=fs.accountKey
	join [2am].dbo.financialService child on fs.financialServiceKey = child.parentFinancialServiceKey
		and child.financialServiceTypeKey in (6,7)
		and child.accountStatusKey = 1
	where a.accountKey =  @accountKey

end

