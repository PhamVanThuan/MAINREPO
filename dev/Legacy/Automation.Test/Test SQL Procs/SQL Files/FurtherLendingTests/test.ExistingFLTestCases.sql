USE [2AM]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF EXISTS (select * from dbo.sysobjects where id = object_id(N'test.ExistingFLTestCases') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
Begin
	Drop Procedure test.ExistingFLTestCases
	Print 'Dropped Proc test.ExistingFLTestCases'
End
Go

Create Procedure test.ExistingFLTestCases

AS

  if object_id('tempdb..#furtherAdvanceOffers','U') is not null
	begin
		drop table #furtherAdvanceOffers
	end

--lets remove the existing entries
delete from test.AutomationFLTestCases
where TestIdentifier in ('LAAExceeded')
/*
This inserts a test case for the _016_PostFAdvDisbursementLAAExceeded() test method. This test
is part of the Readvance Payments test suite.
*/
select LoanAgreementAmount as fAdvAmount, o.offerKey, o.reservedAccountKey, s.name
into #furtherAdvanceOffers
from x2.x2data.readvance_payments rp
join x2.x2.instance i on rp.instanceid=i.id
join x2.x2.state s on i.stateid=s.id
join [2am].dbo.Offer o on rp.applicationKey=o.offerKey
join [2am].dbo.Account a on o.accountKey=a.accountKey
join [2am].dbo.offerInformation oi (nolock) on oi.offerinformationkey = (select
 max(offerinformationkey) from [2am].dbo.offerInformation where offerkey=o.offerkey)
join [2am].dbo.offerInformationVariableLoan oivl on oi.offerInformationKey=oivl.offerInformationKey
where s.name in ('setup payment','disburse funds','awaiting schedule','send schedule')
and o.offerTypeKey=3 and rrr_productKey <> 2

insert into test.AutomationFLTestCases (
TestIdentifier, TestGroup, AccountKey, FAdvOfferKey, CurrentState, AssignedFLSupervisor, Password, InstanceID, ProcessViaWorkflowAutomation)
select top 1 'LAAExceeded', 'Existing - Further Advance', #furtherAdvanceOffers.reservedAccountKey,
#furtherAdvanceOffers.offerKey, #furtherAdvanceOffers.name, 'SAHL\FLSUser','Natal1', 0, 0
from #furtherAdvanceOffers 
join [2am].dbo.account a on #furtherAdvanceOffers.reservedAccountKey=a.accountKey
join [2am].dbo.financialService fs on a.accountKey=fs.accountKey and fs.parentfinancialServiceKey is null
join [2am].dbo.financialServiceType fst on fs.financialServiceTypeKey=fst.financialServiceTypeKey
	and fst.financialServiceGroupKey = 1
join [2am].fin.mortgageLoan ml on fs.financialServiceKey=ml.financialServiceKey
join [2am].fin.Balance b on fs.financialServiceKey = b.financialServiceKey and b.balanceTypeKey = 1
join [2am].fin.LoanBalance lb on fs.financialServiceKey = lb.financialServiceKey 
join [2am].dbo.bondMortgageLoan bml on fs.financialServiceKey=bml.financialServiceKey
join [2am].dbo.bond bd on bml.bondkey=bd.bondKey
group by #furtherAdvanceOffers.reservedAccountKey,#furtherAdvanceOffers.offerKey,#furtherAdvanceOffers.name, #furtherAdvanceOffers.fAdvAmount+b.Amount 
having sum(bd.bondLoanAgreementAmount)*102.5/100 < (#furtherAdvanceOffers.fAdvAmount+b.Amount)




