USE [2AM]
GO
/****** Object:  StoredProcedure test.InsertEmployment******/
IF  EXISTS (SELECT * FROM sys.objects 
	WHERE object_id = OBJECT_ID(N'test.InsertEmployment') 
		AND type in (N'P', N'PC'))
	begin
		DROP PROCEDURE test.InsertEmployment
	end
go
USE [2AM]
GO

/****** Object:  StoredProcedure test.InsertEmployment******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE test.InsertEmployment
		@EmployerKey int
	   ,@EmploymentTypeKey int
	   ,@RemunerationTypeKey int
	   ,@EmploymentStatusKey int
	   ,@LegalEntityKey int
	   ,@EmploymentStartDate datetime
	   ,@EmploymentEndDate datetime
	   ,@ContactPerson varchar(50)
	   ,@ContactPhoneNumber varchar(50)
	   ,@ContactPhoneCode varchar(50)
	   ,@ConfirmedBy varchar(50)
	   ,@ConfirmedDate datetime
	   ,@UserID varchar(25)
	   ,@ChangeDate datetime
	   ,@Department varchar(50)
	   ,@BasicIncome float
	   ,@Commission float
	   ,@Overtime float
	   ,@Shift float
	   ,@Performance float
	   ,@Allowances float
	   ,@PAYE float
	   ,@UIF float
	   ,@PensionProvident float
	   ,@MedicalAid float
	   ,@ConfirmedBasicIncome float
	   ,@ConfirmedCommission float
	   ,@ConfirmedOvertime float
	   ,@ConfirmedShift float
	   ,@ConfirmedPerformance float
	   ,@ConfirmedAllowances float
	   ,@ConfirmedPAYE float
	   ,@ConfirmedUIF float
	   ,@ConfirmedPensionProvident float
	   ,@ConfirmedMedicalAid float
	   ,@JobTitle varchar(255)
	   ,@ConfirmedEmploymentFlag bit
	   ,@ConfirmedIncomeFlag bit
	   ,@EmploymentConfirmationSourceKey int
	   ,@SalaryPaymentDay int
AS
BEGIN
	INSERT INTO [2am].[dbo].[Employment]
			   ([EmployerKey]
			   ,[EmploymentTypeKey]
			   ,[RemunerationTypeKey]
			   ,[EmploymentStatusKey]
			   ,[LegalEntityKey]
			   ,[EmploymentStartDate]
			   ,[EmploymentEndDate]
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
			   ,[Overtime]
			   ,[Shift]
			   ,[Performance]
			   ,[Allowances]
			   ,[PAYE]
			   ,[UIF]
			   ,[PensionProvident]
			   ,[MedicalAid]
			   ,[ConfirmedBasicIncome]
			   ,[ConfirmedCommission]
			   ,[ConfirmedOvertime]
			   ,[ConfirmedShift]
			   ,[ConfirmedPerformance]
			   ,[ConfirmedAllowances]
			   ,[ConfirmedPAYE]
			   ,[ConfirmedUIF]
			   ,[ConfirmedPensionProvident]
			   ,[ConfirmedMedicalAid]
			   ,[JobTitle]
			   ,[ConfirmedEmploymentFlag]
			   ,[ConfirmedIncomeFlag]
			   ,[EmploymentConfirmationSourceKey]
			   ,[SalaryPaymentDay])
		 VALUES
			   (	
					@EmployerKey
				   ,@EmploymentTypeKey 
				   ,@RemunerationTypeKey 
				   ,@EmploymentStatusKey 
				   ,@LegalEntityKey 
				   ,@EmploymentStartDate 
				   ,@EmploymentEndDate 
				   ,@ContactPerson
				   ,@ContactPhoneNumber 
				   ,@ContactPhoneCode 
				   ,@ConfirmedBy 
				   ,@ConfirmedDate
				   ,@UserID
				   ,@ChangeDate
				   ,@Department
				   ,@BasicIncome 
				   ,@Commission 
				   ,@Overtime 
				   ,@Shift 
				   ,@Performance 
				   ,@Allowances 
				   ,@PAYE 
				   ,@UIF 
				   ,@PensionProvident 
				   ,@MedicalAid 
				   ,@ConfirmedBasicIncome 
				   ,@ConfirmedCommission 
				   ,@ConfirmedOvertime 
				   ,@ConfirmedShift 
				   ,@ConfirmedPerformance 
				   ,@ConfirmedAllowances 
				   ,@ConfirmedPAYE 
				   ,@ConfirmedUIF 
				   ,@ConfirmedPensionProvident 
				   ,@ConfirmedMedicalAid
				   ,@JobTitle 
				   ,@ConfirmedEmploymentFlag 
				   ,@ConfirmedIncomeFlag 
				   ,@EmploymentConfirmationSourceKey 
				   ,@SalaryPaymentDay 
			   )
		declare @employmentkey int
		set @employmentkey = SCOPE_IDENTITY()
		
		select * from dbo.employment
		where employmentkey = @employmentkey
END