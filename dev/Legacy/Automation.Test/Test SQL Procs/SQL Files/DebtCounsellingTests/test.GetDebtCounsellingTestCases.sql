USE [2AM]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF EXISTS (select * from dbo.sysobjects where id = object_id(N'[test].[GetDebtCounsellingTestCases]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
Begin
	Drop Procedure [test].[GetDebtCounsellingTestCases] 
	Print 'Dropped procedure [test].[GetDebtCounsellingTestCases]'
End
Go

CREATE PROCEDURE [test].[GetDebtCounsellingTestCases] 
	@TestIdentifier varchar(100) = NULL,
	@SellectAll bit = 0
AS
BEGIN

IF (@SellectAll = 1)
	SELECT * FROM test.DebtCounsellingTestCases
ELSE
	SELECT * FROM test.DebtCounsellingTestCases WHERE DebtCounsellingTestCases.TestIdentifier = @TestIdentifier
END