USE [2AM]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[test].[pTestHOCDailyExtract]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [test].[pTestHOCDailyExtract]

GO

CREATE PROC [test].[pTestHOCDailyExtract]
	@RunProc bit
AS
BEGIN

/*
----------------------------------------------------------------
This script test the running of the DTS Package which is schedule via a JOB 
----------------------------------------------------------------
This will test the following :
 - Creation of test data
 - Testing of the Audit SP that creates the HOCTransactions records
 - Running of the SP that does both the Innovations & IIX Extract
 - Since we are only interested in the IIX Extract we can verifiy 
	that the staging table has the test cases we have created
----------------------------------------------------------------
-- Test data we created to test the following :
-- 1) Update to Legal Entity Linked to SAHL Insurer Policy
-- 2) HOC Reinstatement
-- 3) SAHL HOC Insurer Update
-- 4) HOC Addition
-- 5) Closed - External Insurer
-- 6) Closed - Closed Policy
----------------------------------------------------------------
*/
SET NOCOUNT ON

-- Get Current Date and do conversions
declare @LastExtract datetime
declare @ControlText varchar(19)

print '1'
select  @ControlText = convert(varchar(19),getdate(),120)
select @LastExtract = convert(varchar(19),getdate(),120)

if object_id('tempdb..#TestAccounts','U') is not null
	drop table #TestAccounts

if object_id('tempdb..#enum_job','U') is not null
	drop table #enum_job

create table #TestAccounts(AccountKey int, HOCKey int, LegalEntityKey int, Reason varchar(max), Test varchar(max))

create table #enum_job 
( Job_ID uniqueidentifier,
Last_Run_Date int,
Last_Run_Time int,
Next_Run_Date int,
Next_Run_Time int,
Next_Run_Schedule_ID int,
Requested_To_Run int,
Request_Source int,
Request_Source_ID varchar(100),
Running int,
Current_Step int,
Current_Retry_Attempt int, 
State int)

print '2'
--select @ControlText
select @LastExtract

-- Update Control Table
--select ControlText from Control (nolock) where ControlDescription = 'Innovations HOC Extract Text'
update Control set ControlText = @ControlText where ControlDescription = 'Innovations HOC Extract Text'

print '3'
-- Clean HOCTransactions Table for the Current Date
delete from HOCTransactions 
where insertDate >= @LastExtract

print '4'
-- Create test Data
exec [test].[pTestGenerateHOCIIXData] 

select 'Test data that created'
select ta.* from #TestAccounts ta

print '6'
DECLARE @ExtractServer VARCHAR(max)
SET @ExtractServer = (select controltext from [2am]..Control (nolock) where ControlDescription = 'HOC Extract Server')
if (@ExtractServer = 'sahls103a')
begin
	exec xp_cmdshell 'XCOPY /Q \\sahls103a\sahoc$\*.* \\sahls103a\datastaging$\History\*.*'
	exec xp_cmdshell 'DEL /Q \\sahls103a\DataStaging$\IIX\*.*'
	exec xp_cmdshell 'DEL /Q \\sahls103a\sahoc$\*.*'
end

if (@ExtractServer = 'sahls203a')
begin
	exec xp_cmdshell 'XCOPY /Q \\sahls203a\sahoc$\*.* \\sahls203a\datastaging$\History\*.*'
	exec xp_cmdshell 'DEL /Q \\sahls203a\DataStaging$\IIX\*.*'
	exec xp_cmdshell 'DEL /Q \\sahls203a\sahoc$\*.*'
end

-- Here we start the job then monitor when it has completed

IF (@RunProc = 1)
BEGIN
	SELECT 'RUNNING PROC pHOCDailyExtract'
	EXEC dbo.pHOCDailyExtract
END
ELSE
BEGIN

	-- Clean HOCTransactions Table for the Current Date
	delete from HOCTransactions 
	where insertDate >= @LastExtract

	DECLARE @job_id uniqueidentifier
	DECLARE @State int

	EXEC msdb.dbo.sp_start_job N'HOC Daily Extract';

	SELECT @job_id=job_id FROM msdb.dbo.sysjobs WHERE name=N'HOC Daily Extract' 

	select @State = 1
	SELECT 'JOB RUNNING'

		while (@State = 1)
		begin
			waitfor delay '00:00:10'

			truncate table #enum_job
			insert into #enum_job 
			EXEC master.dbo.xp_sqlagent_enum_jobs 1, sa, @job_id

			select @State = State from #enum_job 
		end

	SELECT 'JOB COMPLETE'
END

print '7'

select 'Test Loan Numbers not appearing in the extract'
select *
from #TestAccounts ta
left join Staging.dbo.HOCDailyExtract sh
	on ta.AccountKey = sh.MortgageLoanAccountNumber
where sh.MortgageLoanAccountNumber is null

print '8'

select 'HOC Tran vs Extract Data'
select *
from #TestAccounts ta
join Staging.dbo.HOCDailyExtract sh
	on ta.AccountKey = sh.MortgageLoanAccountNumber

END
