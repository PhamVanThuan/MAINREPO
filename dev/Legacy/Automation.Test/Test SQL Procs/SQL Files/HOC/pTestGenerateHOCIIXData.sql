USE [2AM]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[test].[pTestGenerateHOCIIXData]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [test].[pTestGenerateHOCIIXData]

GO

CREATE PROC [test].[pTestGenerateHOCIIXData]
AS
BEGIN

/*--------------------------------------------------------------------------------------
This script creates test data for the various--  records that
need to be in the HOC IIX Daily Extract.
--------------------------------------------------------------------------------------*/

/*--------------------------------------------------------------------------------------
Clean up and create vars
--------------------------------------------------------------------------------------*/
if object_id('tempdb..#HOCIIXTestCases','U') is not null
	drop table #HOCIIXTestCases

if object_id('tempdb..#HOCIIXTestAccounts','U') is not null
	drop table #HOCIIXTestAccounts

if object_id('tempdb..#HOCIIXSetUpAuditData','U') is not null
	drop table #HOCIIXSetUpAuditData

declare @TestAccountKey01 int
declare @HOCKey int
declare @LegalEntityKey int
declare @testDate datetime
declare @Message varchar(max)
declare @Reason varchar(max)

create table #HOCIIXSetUpAuditData(ID int IDENTITY(1,1), AccKey int, HOCKey int, status varchar(10))
create table #HOCIIXTestCases(AccountKey int, HOCKey int, LegalEntityKey int, Reason varchar(max), Test varchar(max))
create table #HOCIIXTestAccounts(AccountKey int)

/*------------------------------------------------------------------------------
Setup some test data
------------------------------------------------------------------------------*/
EXEC [test].[pTestSetUpHOCAuditData]

/*--------------------------------------------------------------------------------------
Set Date to today to check for any transactions that will be written for AccountKeys
--------------------------------------------------------------------------------------*/
set @TestAccountKey01 = -1
set @HOCKey = -1
set @LegalEntityKey = -1
set @Message = ''
set @Reason = ''
set @testDate = (convert(varchar(10),getdate(),101)) 

/*--------------------------------------------------------------------------------------
Test 1
SAHL HOC Insurer Update
--------------------------------------------------------------------------------------*/

-- Find an Open HOC Policy 
-- Test update to HOC
select top 1 
@TestAccountKey01 = AccKey ,@HOCKey = HOCKey 
from #HOCIIXSetUpAuditData
where AccKey not in (select accountKey from #HOCIIXTestAccounts) and status = 'U'

-- Set Up Data
update h
		set 
			h.HOCMonthlyPremium = (h.HOCMonthlyPremium  + ((h.HOCMonthlyPremium + 100.00)  * 0.1)),
			h.HOCProrataPremium = (h.HOCProrataPremium + ((h.HOCProrataPremium  +100.00) * 0.1))
FROM hoc h
WHERE h.FinancialServiceKey = @HOCKey

-- Clean Trans
exec test.pCleanHOCAuditData @HOCKey

-- Test Update
update h
		set 
			h.HOCMonthlyPremium = (h.HOCMonthlyPremium  + ((h.HOCMonthlyPremium + 100.00)  * 0.1)),
			h.HOCProrataPremium = (h.HOCProrataPremium + ((h.HOCProrataPremium  +100.00) * 0.1))
FROM hoc h
WHERE h.FinancialServiceKey = @HOCKey

-- Run the job to create the HOCTransactions
exec [dbo].[pHOCAuditExtract]

SELECT @Message = 'Test: SAHL HOC Insurer Update -> The HOC Update wrote a HOCTransaction successfully'
SELECT @Reason = 'SAHL HOC Insurer Update'

-- write accountkey so we dont use in the next test
insert into #HOCIIXTestAccounts
select @TestAccountKey01

-- write tran to log
insert into #HOCIIXTestCases (AccountKey,HOCKey,LegalEntityKey,Reason,Test)
values(@TestAccountKey01,@HOCKey,@LegalEntityKey,@Reason,@Message)

--Reset Vars
set @TestAccountKey01 = -1
set @HOCKey = -1
set @LegalEntityKey = -1
set @Message = ''
set @Reason = ''

/*--------------------------------------------------------------------------------------
Test 2
Update to Legal Entity Linked to SAHL Insurer Policy
--------------------------------------------------------------------------------------*/

-- Create a Test Legal Entity
delete r
from  [2am].[dbo].[role] r (nolock)
inner join [2am].[dbo].LegalEntity le (nolock)
	on r.LegalEntityKey = le.LegalEntityKey
where le.FirstNames = 'TestIIXHOCExtractUser' and le.Surname = 'TestIIXHOCExtractUser'

delete r
from  [2am].[dbo].[role] r (nolock)
inner join [2am].[dbo].LegalEntity le (nolock)
	on r.LegalEntityKey = le.LegalEntityKey
where le.FirstNames = 'TestIIXHOCExtractUser01' and le.Surname = 'TestIIXHOCExtractUser01'

delete le
from [2am].[dbo].LegalEntity le (nolock)
where le.FirstNames = 'TestIIXHOCExtractUser' and le.Surname = 'TestIIXHOCExtractUser'

delete le
from [2am].[dbo].LegalEntity le (nolock)
where le.FirstNames = 'TestIIXHOCExtractUser01' and le.Surname = 'TestIIXHOCExtractUser01'

insert into dbo.LegalEntity (LegalEntityTypeKey, IntroductionDate, FirstNames, Surname, CitizenTypeKey, EducationKey, HomeLanguageKey, DocumentLanguageKey, EmailAddress, LegalEntityStatusKey, SalutationKey, CellPhoneNumber) 
values (2, getdate(), 'TestIIXHOCExtractUser', 'TestIIXHOCExtractUser', 1, 1, 1, 2, 'Test@IIXHOC.User', 1, 1, '00077771122')

select @LegalEntityKey = LegalEntityKey from dbo.LegalEntity 
where FirstNames = 'TestIIXHOCExtractUser' and Surname = 'TestIIXHOCExtractUser'

-- Find an Open HOC Policy 
-- Test update to Legal Entity Linked to HOC Policy
select distinct top 1 
@TestAccountKey01 = AccKey ,@HOCKey = HOCKey 
from #HOCIIXSetUpAuditData
where AccKey not in (select accountKey from #HOCIIXTestAccounts) and status = 'U'

-- Set Up Data
update h
		set 
			h.HOCMonthlyPremium = (h.HOCMonthlyPremium  + ((h.HOCMonthlyPremium + 100.00)  * 0.1)),
			h.HOCProrataPremium = (h.HOCProrataPremium + ((h.HOCProrataPremium  +100.00) * 0.1))
FROM hoc h
WHERE h.FinancialServiceKey = @HOCKey

-- Clean Trans
exec test.pCleanHOCAuditData @HOCKey

-- Add the Legal Entity to the Loan Account
if not exists(select * from role where AccountKey = @TestAccountKey01 and LegalEntityKey = @LegalEntityKey and roleTypeKey = 2)
begin
	INSERT INTO [2AM].[dbo].[Role]
			   ([LegalEntityKey]
			   ,[AccountKey]
			   ,[RoleTypeKey]
			   ,[GeneralStatusKey]
			   ,[StatusChangeDate])
	VALUES (@LegalEntityKey,@TestAccountKey01,2,1,getdate())
end

-- Test the LE Update that exists against the Loan for the HOC Policy
update le
set 
	le.FirstNames = 
	case 
		when le.FirstNames = 'TestIIXHOCExtractUser'
			then 'TestIIXHOCExtractUser01'
			else 'TestIIXHOCExtractUser'
	end,
	le.Surname = 
	case 
		when le.Surname = 'TestIIXHOCExtractUser'
			then 'TestIIXHOCExtractUser01'
			else 'TestIIXHOCExtractUser'
	end
from  [2am].[dbo].[role] r (nolock)
inner join [2am].[dbo].LegalEntity le (nolock)
	on r.LegalEntityKey = le.LegalEntityKey
where 
r.LegalEntityKey = @LegalEntityKey 
	and 
r.AccountKey = @TestAccountKey01 
	and 
r.roleTypeKey = 2

-- Run the job to create the HOCTransactions
EXEC [dbo].[pHOC_LegalEntityAuditExtract]

SELECT @Message = 'Test: Update to Legal Entity Linked to SAHL Insurer Policy -> The HOC Update wrote a HOCTransaction successfully'
SELECT @Reason = 'Update to Legal Entity'

-- write accountkey so we dont use in the next test
insert into #HOCIIXTestAccounts
select @TestAccountKey01

-- write tran to log
insert into #HOCIIXTestCases (AccountKey,HOCKey,LegalEntityKey,Reason,Test)
values(@TestAccountKey01,@HOCKey,@LegalEntityKey,@Reason,@Message)

--Reset Vars
set @TestAccountKey01 = -1
set @HOCKey = -1
set @LegalEntityKey = -1
set @Message = ''
set @Reason = ''

/*--------------------------------------------------------------------------------------
Test 3
HOC Reinstatement
--------------------------------------------------------------------------------------*/
-- Find an Open HOC Policy 
-- Test update to HOC
select distinct top 1 
@TestAccountKey01 = AccKey ,@HOCKey = HOCKey 
from #HOCIIXSetUpAuditData
where AccKey not in (select accountKey from #HOCIIXTestAccounts) and status = 'R'

-- Set Up Data
update h
		set 
			h.HOCMonthlyPremium = (h.HOCMonthlyPremium  + ((h.HOCMonthlyPremium + 100.00)  * 0.1)),
			h.HOCProrataPremium = (h.HOCProrataPremium + ((h.HOCProrataPremium  +100.00) * 0.1)),
			h.HOCInsurerKey = 5
FROM hoc h
WHERE h.FinancialServiceKey = @HOCKey

-- Clean Trans
exec test.pCleanHOCAuditData @HOCKey

-- Test Update
update h
		set 
			h.HOCMonthlyPremium = (h.HOCMonthlyPremium  + ((h.HOCMonthlyPremium + 100.00)  * 0.1)),
			h.HOCProrataPremium = (h.HOCProrataPremium + ((h.HOCProrataPremium  +100.00) * 0.1)),
			h.HOCInsurerKey = 2
FROM hoc h
WHERE h.FinancialServiceKey = @HOCKey

-- Run the job to create the HOCTransactions
exec [dbo].[pHOCAuditExtract]

SELECT @Message = 'Test: HOC Reinstatement -> The HOC Update wrote a HOCTransaction successfully'
SELECT @Reason = 'HOC Reinstatement'

-- write accountkey so we dont use in the next test
insert into #HOCIIXTestAccounts
select @TestAccountKey01

-- write tran to log
insert into #HOCIIXTestCases (AccountKey,HOCKey,LegalEntityKey,Reason,Test)
values(@TestAccountKey01,@HOCKey,@LegalEntityKey,@Reason,@Message)

--Reset Vars
set @TestAccountKey01 = -1
set @HOCKey = -1
set @LegalEntityKey = -1
set @Message = ''
set @Reason = ''

/*--------------------------------------------------------------------------------------
Test 4
HOC Addition
--------------------------------------------------------------------------------------*/
-- Find an Open HOC Policy 
-- Test update to HOC
select distinct top 1 
@TestAccountKey01 = AccKey ,@HOCKey = HOCKey 
from #HOCIIXSetUpAuditData
where AccKey not in (select accountKey from #HOCIIXTestAccounts) and status = 'A'

-- Clean Trans
exec test.pCleanHOCAuditData @HOCKey

-- Test Update
update h
set 
	h.HOCMonthlyPremium = (h.HOCMonthlyPremium  + ((h.HOCMonthlyPremium + 100.00)  * 0.1)),
	h.HOCProrataPremium = (h.HOCProrataPremium + ((h.HOCProrataPremium  +100.00) * 0.1))
FROM hoc h
WHERE h.FinancialServiceKey = @HOCKey

-- Run the job to create the HOCTransactions
exec [dbo].[pHOCAuditExtract]

-- Check if we have some transactions
SELECT @Message = 'Test: HOC Addition -> The HOC Update wrote a HOCTransaction successfully'

SELECT @Reason = 'HOC Addition'

-- write accountkey so we dont use in the next test
insert into #HOCIIXTestAccounts
select @TestAccountKey01

-- write tran to log
insert into #HOCIIXTestCases (AccountKey,HOCKey,LegalEntityKey,Reason,Test)
values(@TestAccountKey01,@HOCKey,@LegalEntityKey,@Reason,@Message)

--Reset Vars
set @TestAccountKey01 = -1
set @HOCKey = -1
set @LegalEntityKey = -1
set @Message = ''
set @Reason = ''

/*--------------------------------------------------------------------------------------
Test 5
Closed - External Insurer
--------------------------------------------------------------------------------------*/
-- Find an Open HOC Policy 
-- Test update to HOC
select distinct top 1 
@TestAccountKey01 = AccKey ,@HOCKey = HOCKey 
from #HOCIIXSetUpAuditData
where AccKey not in (select accountKey from #HOCIIXTestAccounts) and status = 'U'

-- Set Up Data
update h
		set 
			h.HOCMonthlyPremium = (h.HOCMonthlyPremium  + ((h.HOCMonthlyPremium + 100.00)  * 0.1)),
			h.HOCProrataPremium = (h.HOCProrataPremium + ((h.HOCProrataPremium  +100.00) * 0.1))
FROM hoc h
WHERE h.FinancialServiceKey = @HOCKey

-- Clean Trans
exec test.pCleanHOCAuditData @HOCKey

-- Test Update
-- Test Update
update h
set 
	h.HOCMonthlyPremium = (h.HOCMonthlyPremium  + ((h.HOCMonthlyPremium + 100.00)  * 0.1)),
	h.HOCProrataPremium = (h.HOCProrataPremium + ((h.HOCProrataPremium  +100.00) * 0.1)),
	h.HOCInsurerKey = 1
FROM hoc h
WHERE h.FinancialServiceKey = @HOCKey

-- Run the job to create the HOCTransactions
exec [dbo].[pHOCAuditExtract]

SELECT @Message =  'Test: Closed - External Insurer -> The HOC Update wrote a HOCTransaction successfully'
SELECT @Reason = 'Closed - External Insurer'

-- write accountkey so we dont use in the next test
insert into #HOCIIXTestAccounts
select @TestAccountKey01

-- write tran to log
insert into #HOCIIXTestCases (AccountKey,HOCKey,LegalEntityKey,Reason,Test)
values(@TestAccountKey01,@HOCKey,@LegalEntityKey,@Reason,@Message)

--Reset Vars
set @TestAccountKey01 = -1
set @HOCKey = -1
set @LegalEntityKey = -1
set @Message = ''
set @Reason = ''


/*--------------------------------------------------------------------------------------
Test 6
Closed - Closed Policy
--------------------------------------------------------------------------------------*/
-- Find an Open HOC Policy 
-- Test update to HOC
select distinct top 1 
@TestAccountKey01 = AccKey ,@HOCKey = HOCKey 
from #HOCIIXSetUpAuditData
where AccKey not in (select accountKey from #HOCIIXTestAccounts) and status = 'U'

-- Set Up Data
update h
		set 
			h.HOCMonthlyPremium = (h.HOCMonthlyPremium  + ((h.HOCMonthlyPremium + 100.00)  * 0.1)),
			h.HOCProrataPremium = (h.HOCProrataPremium + ((h.HOCProrataPremium  +100.00) * 0.1))
FROM hoc h
WHERE h.FinancialServiceKey = @HOCKey

-- Clean Trans
exec test.pCleanHOCAuditData @HOCKey

-- Test Update
update h
set 
	h.HOCMonthlyPremium = (h.HOCMonthlyPremium  + ((h.HOCMonthlyPremium + 100.00)  * 0.1)),
	h.HOCProrataPremium = (h.HOCProrataPremium + ((h.HOCProrataPremium  +100.00) * 0.1)),
	h.HOCInsurerKey = 23
FROM hoc h
WHERE h.FinancialServiceKey = @HOCKey

-- Run the job to create the HOCTransactions
exec [dbo].[pHOCAuditExtract]

SELECT @Message = 'Test: Closed - Closed Policy -> The HOC Update wrote a HOCTransaction successfully'
SELECT @Reason = 'Closed - Closed Policy'

-- write accountkey so we dont use in the next test
insert into #HOCIIXTestAccounts
select @TestAccountKey01

-- write tran to log
insert into #HOCIIXTestCases (AccountKey,HOCKey,LegalEntityKey,Reason,Test)
values(@TestAccountKey01,@HOCKey,@LegalEntityKey,@Reason,@Message)

--Reset Vars
set @TestAccountKey01 = -1
set @HOCKey = -1
set @LegalEntityKey = -1
set @Message = ''
set @Reason = ''

/*--------------------------------------------------------------------------------------
That concludes our test cases
--------------------------------------------------------------------------------------*/

INSERT INTO #TestAccounts
SELECT * FROM #HOCIIXTestCases

END







