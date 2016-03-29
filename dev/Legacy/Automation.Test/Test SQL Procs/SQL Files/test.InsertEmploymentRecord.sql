USE [2AM]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
/********************************************************************************************************************************
Description:
	
	
*********************************************************************************************************************************/

IF EXISTS (select * from dbo.sysobjects where id = object_id(N'[test].[InsertEmploymentRecord]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
Begin
	Drop Procedure [test].[InsertEmploymentRecord]
	Print 'Dropped Proc [test].[InsertEmploymentRecord]'
End
Else
begin
	print 'Proc [test].[InsertEmploymentRecord] not found'
End
Go

CREATE PROCEDURE [test].[InsertEmploymentRecord](
	@LegalEntityKey int,
	@EmploymentTypeKey int,
	@RemunerationTypeKey int,
	@ADUserName varchar(50),
	@BasicIncome float,
	@ConfirmedBasicIncome float,
	@EmploymentStartDate datetime = null,
	@Commission float = null,
	@ConfirmedCommission float = null)

AS

/*
begin tran
--rollback
Declare @LegalEntityKey int,
	@EmploymentTypeKey int,
	@RemunerationTypeKey int,
	@ADUserName varchar(50),
	@BasicIncome float,
	@ConfirmedBasicIncome float,
	@EmploymentStartDate datetime,
	@Commission float,
	@ConfirmedCommission float

Set @LegalEntityKey = 94929
Set @EmploymentTypeKey = 1
Set @RemunerationTypeKey = 1
Set @ADUserName = 'SAHL\AndrewK'
Set @BasicIncome = 10000
Set @ConfirmedBasicIncome = 10000
Set @EmploymentStartDate = getdate()
Set @Commission = null
Set @ConfirmedCommission  = null
*/
	
Declare @RandomRowNum int,
@EmployerKey int,
@EmploymentConfirmationSourceKey int

if @EmploymentStartDate is null
	begin 
		Set @EmploymentStartDate = getdate()
	end

if exists(select * from Employment where LegalEntityKey = @LegalEntitykey)
	begin
		Update Employment Set EmploymentStatusKey = 2 where LegalEntityKey = @LegalEntityKey
	End

select @RandomRowNum = (1 + round(rand()*count(*),0)) from employer;

With EmployerExtract as (select EmployerKey, Name, Row_Number() Over(Order By EmployerKey) as 'RowNum'
							from Employer (nolock))
							
                        Select @EmployerKey = EmployerKey 
							from EmployerExtract
								where RowNum = @RandomRowNum
                                
Set @EmploymentConfirmationSourceKey = 1 + round(rand()*4,0)
                                
INSERT INTO [2AM].[dbo].[Employment]
           ([EmployerKey]
           ,[EmploymentTypeKey]
           ,[RemunerationTypeKey]
           ,[EmploymentStatusKey]
           ,[LegalEntityKey]
           ,[EmploymentStartDate]
           ,[ContactPerson]
           ,[ContactPhoneNumber]
           ,[ContactPhoneCode]
           ,[ConfirmedBy]
           ,[ConfirmedDate]
           ,[UserID]
           ,[ChangeDate]
           ,[Department]
           ,[BasicIncome]
           ,[Commission]
           ,[ConfirmedBasicIncome]
           ,[ConfirmedCommission]
           ,[ConfirmedEmploymentFlag]
           ,[ConfirmedIncomeFlag]
           ,[EmploymentConfirmationSourceKey])
     VALUES
           (@EmployerKey
           ,@EmploymentTypeKey
           ,@RemunerationTypeKey
           ,1
           ,@LegalEntityKey
           ,@EmploymentStartDate
           ,'Test'
           ,'1234567'
           ,'012'
           ,@ADUserName
           ,getdate()
           ,@ADUserName
           ,getdate()
           ,'Test'
           ,@BasicIncome
           ,@Commission
           ,@ConfirmedBasicIncome
           ,@ConfirmedCommission
           ,1
           ,1
           ,@EmploymentConfirmationSourceKey)
GO

--select * from employment (nolock) where legalentitykey = @LegalEntityKey
SET ANSI_NULLS Off
GO

SET QUOTED_IDENTIFIER Off
GO
