USE [2AM]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF EXISTS (select * from dbo.sysobjects where id = object_id(N'test.InsertLifePolicyClaim') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
Begin
	Drop Procedure test.InsertLifePolicyClaim 
	Print 'Dropped Proc test.InsertLifePolicyClaim'
End
Go

CREATE PROCEDURE test.InsertLifePolicyClaim
	@lifeFinancialServiceKey int,
	@lifePolicyClaimKey int out,
	@claimStatusKey int,
	@claimTypeKey int,
	@claimDate datetime
AS

insert into [2am].dbo.LifePolicyClaim (FinancialServiceKey,ClaimStatusKey,ClaimTypeKey,ClaimDate)
values (@lifeFinancialServiceKey,@claimStatusKey,@claimTypeKey,@claimDate)

set @lifePolicyClaimKey = (select SCOPE_IDENTITY())




