USE [2AM]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF EXISTS (select * from dbo.sysobjects where id = object_id(N'test.CAP2AutomationSetup') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
Begin
	Drop Procedure test.[CAP2AutomationSetup]  
	Print 'Dropped Proc test.[CAP2AutomationSetup]'
End
Go

CREATE PROCEDURE [test].[CAP2AutomationSetup]

	@BrokerAdUserName VARCHAR(50), 
	@CreditAdUserName VARCHAR(50)
	
AS
BEGIN

	--SETUP USERS
	EXEC test.SetUpCAPUsersForAutomation @BrokerAdUserName, @CreditAdUserName
	--INSERT CAP CONFIG
	EXEC test.InsertCAPTypeConfiguration
	--INSERT CAP TEST CASES
	EXEC [test].[InsertCAPTestCases]
	--CREATE AUTOMATION CASES
	EXEC [test].[CreateCAP2TestCases]

END