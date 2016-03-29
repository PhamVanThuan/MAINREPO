use [2am]
go

/**************************************************
 Statement  : COMMON\GetFees
 Change Date: 2015/04/28 09:59:44 AM - Version 57
 Change User: SAHL\ClintS
 **************************************************/
if( not exists(select StatementName from [2am].dbo.uiStatement where StatementName = 'GetFees' and ApplicationName = 'COMMON' and Version = 57) ) 
begin
   declare @higherversion int;
   -- check version numbers to determine if we can do insert
   select @higherversion = count(*) from [2am].dbo.uiStatement where StatementName = 'GetFees' and ApplicationName = 'COMMON' and Version > 57
   -- if there are no higher versions on file then we can go ahead and insert
   if (@higherversion = 0)
   begin
       -- if we are not on dev environment then delete all versions
       if (cast(serverproperty('MachineName') as varchar(128)) not like '%DEV%')
       begin
           if( exists(select statementname from [2am].dbo.uiStatement where statementname = 'GetFees' and ApplicationName = 'COMMON' ) )
	            delete from [2am].dbo.uiStatement where statementname = 'GetFees' and ApplicationName = 'COMMON'; 
       end

       -- insert new version of uistatement
       insert into [2am].dbo.uiStatement 
           (ApplicationName, StatementName, ModifyDate, Version, ModifyUser, Statement, Type, LastAccessedDate)
       values ('COMMON', 'GetFees', GetDate(), 57, 'SAHL\craigf',
                'EXECUTE [dbo].[GetFees]  @LoanAmount, @BondRequired, @LoanType, @CashOut, @OverRideCancelFee, @CapitaliseFees, @NCACompliant, @IsBondExceptionAction, @QuickPay, @HouseholdIncome, @EmploymentTypeKey, @LTV, @ApplicationParentAccountKey, @IsStaffLoan, @IsDiscountedInitiationFee, @OfferStartDate, @CapitaliseInitiationFee, @isGEPF',
                1, GetDate());
   end
end
