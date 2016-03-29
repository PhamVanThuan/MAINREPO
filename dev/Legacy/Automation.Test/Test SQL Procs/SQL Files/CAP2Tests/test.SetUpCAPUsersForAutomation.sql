USE [2AM]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF EXISTS (select * from dbo.sysobjects where id = object_id(N'test.SetUpCAPUsersForAutomation') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
Begin
	Drop Procedure test.SetUpCAPUsersForAutomation  
	Print 'Dropped Proc test.SetUpCAPUsersForAutomation'
End
Go

CREATE PROCEDURE test.SetUpCAPUsersForAutomation

@BrokerAdUserName varchar(50),
@CreditAdUserName varchar(50)

AS

exec test.InsertCAPBrokerOrAdmin 'consultant',@BrokerAdUserName

exec test.InsertCAPBrokerOrAdmin 'cap admin',@CreditAdUserName

delete from capcreditbrokertoken
where brokerkey in (
select brokerkey from broker b
where adusername <> @CreditAdUserName
)