USE [2AM]
go
set ansi_nulls on
go
set quoted_identifier on

go
if OBJECT_ID('[2AM].test.ValuationTestCases') is not null
begin
	drop table test.ValuationTestCases
end

create table test.ValuationTestCases
(
	TestIdentifier varchar(100),
	Priority int not null,
	TestGroup varchar(100),
	ApplicationKey int default 0
)

-- ValuationReview
insert into test.ValuationTestCases(TestIdentifier,Priority,TestGroup) values('ReviewValuationRequired',1,'ValuationReview')
insert into test.ValuationTestCases(TestIdentifier,Priority,TestGroup) values('RequestValuationReview',2,'ValuationReview')

-- FurtherValuation
insert into test.ValuationTestCases(TestIdentifier,Priority,TestGroup) values('FurtherValuationRequired',1,'FurtherValuation')
insert into test.ValuationTestCases(TestIdentifier,Priority,TestGroup) values('ReInstructValuer',2,'FurtherValuation')

-- AdcheckValuationWithdraw
insert into test.ValuationTestCases(TestIdentifier,Priority,TestGroup) values('InstructAdcheckValuation',1,'AdcheckValuationWithdraw')
insert into test.ValuationTestCases(TestIdentifier,Priority,TestGroup) values('CancelWithdrawnAssessment',2,'AdcheckValuationWithdraw')
insert into test.ValuationTestCases(TestIdentifier,Priority,TestGroup) values('WithdrawAssessment',3,'AdcheckValuationWithdraw')
insert into test.ValuationTestCases(TestIdentifier,Priority,TestGroup) values('InstructValuerToWithdraw',4,'AdcheckValuationWithdraw')

-- AdcheckValuationComplete
insert into test.ValuationTestCases(TestIdentifier,Priority,TestGroup) values('InstructAdcheckValuation',1,'AdcheckValuationComplete')
insert into test.ValuationTestCases(TestIdentifier,Priority,TestGroup) values('ValuationInOrder',2,'AdcheckValuationComplete')

-- ValuationsManager
insert into test.ValuationTestCases(TestIdentifier,Priority,TestGroup) values('Escalate',1,'ValuationsManager')
insert into test.ValuationTestCases(TestIdentifier,Priority,TestGroup) values('Archive',2,'ValuationsManager')
