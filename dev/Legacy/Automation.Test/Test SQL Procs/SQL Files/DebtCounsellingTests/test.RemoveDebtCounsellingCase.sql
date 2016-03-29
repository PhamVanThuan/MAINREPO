USE [2AM]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF EXISTS (select * from dbo.sysobjects where id = object_id(N'[test].[RemoveDebtCounsellingCase]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
Begin
	Drop Procedure [test].RemoveDebtCounsellingCase
	Print 'Dropped procedure [test].[RemoveDebtCounsellingCase]'
End
Go

CREATE PROCEDURE test.RemoveDebtCounsellingCase 

@DebtCounsellingKey int,
@DebtCounsellingGroupKey int

AS

BEGIN

	DELETE FROM [2AM].dbo.ExternalRole WHERE GenericKey=@DebtCounsellingKey
	AND GenericKeyTypeKey=27
	
	DELETE FROM [2AM].DebtCounselling.DebtCounselling WHERE DebtCounsellingKey=@DebtCounsellingKey
	
	DELETE FROM [2AM].DebtCounselling.DebtCounsellingGroup WHERE DebtCounsellingGroupKey=@DebtCounsellingGroupKey
	
	SELECT 1
	
END
