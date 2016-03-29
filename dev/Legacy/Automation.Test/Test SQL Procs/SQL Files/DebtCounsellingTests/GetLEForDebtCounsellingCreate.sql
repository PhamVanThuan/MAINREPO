USE [2AM]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF EXISTS (select * from dbo.sysobjects where id = object_id(N'test.GetLEForDebtCounsellingCreate') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
Begin
	Drop Procedure test.GetLEForDebtCounsellingCreate
	Print 'Dropped Proc test.GetLEForDebtCounsellingCreate'
End
Go

CREATE PROCEDURE test.GetLEForDebtCounsellingCreate


@lossControlCase int,
@CountofAccounts int,
@HasArrearBalance int,
@ProductKey int,
@InterestOnly int

AS

if (@CountofAccounts > 1 and @lossControlCase != 0)
	begin
		RAISERROR (N'The loss control parameter should be 0 when looking for a count of accounts > than 1', -- Message text.
				   16, -- Severity,
				   1); -- State.
				   return
	end

--remove any legal entities linked to open dc cases
delete from test.AutomationDebtCounsellingTestCases where LegalEntityKey in (
select legalEntityKey from debtcounselling.debtcounselling dc 
join [2am].dbo.externalRole er on dc.debtcounsellingkey=er.generickey
	and externalRoleTypeKey = 1
where dc.debtCounsellingStatusKey = 1
)

if @CountOfAccounts = 1
begin
	if @HasArrearBalance = 0
	begin 
		if @lossControlCase = 1
			begin
				--A SINGLE CASE WITH NO ARREAR BALANCE CHECK AND A LOSS CONTROL CASE
				select top 1 test.legalentitykey,test.idnumber  
				from [2am].test.AutomationDebtCounsellingTestCases test
				join [e-work]..efolder e on test.efolderid = e.efolderid
					and e.emapname = 'LossControl'
				where test.accountkey not in (select accountkey from test.DebtCounsellingAccounts)  
				and e.eStageName in 
				( select distinct eStageName from [e-work]..eAction
						where eactionname='x2 debt counselling'
							and emapname ='losscontrol'
							)
				and len(test.userToDo) > 0
				and test.efolderid is not null
				and productKey = @ProductKey 
				and isInterestOnly = @InterestOnly 
				and test.LegalEntityCountOfAccounts = @CountofAccounts
				group by test.legalentitykey,test.idnumber
				order by newid()
			end
		else
			begin
				select top 1 test.legalentitykey,test.idnumber  
				from [2am].test.AutomationDebtCounsellingTestCases test
				where test.accountkey not in (select accountkey from test.DebtCounsellingAccounts)  
				and efolderid is null
				and productKey = @ProductKey 
				and isInterestOnly = @InterestOnly
				and test.LegalEntityCountOfAccounts = @CountofAccounts
				group by test.legalentitykey,test.idnumber 
				order by newid()
			end
	end
	else
		if @lossControlCase = 1
			begin
				select top 1 test.legalentitykey,test.idnumber  
				from [2am].test.AutomationDebtCounsellingTestCases test
				join [e-work]..efolder e on test.efolderid = e.efolderid
					and e.emapname = 'LossControl'
				where test.accountkey not in (select accountkey from test.DebtCounsellingAccounts)  
				and e.eStageName in 
				( select distinct eStageName from [e-work]..eAction
						where eactionname='x2 debt counselling'
							and emapname ='losscontrol'
							)
				and test.ArrearTransactionNewBalance > 500 
				and len(test.userToDo) > 0
				and test.efolderid is not null
				and productKey = @ProductKey and isInterestOnly = @InterestOnly and test.LegalEntityCountOfAccounts = @CountofAccounts
				group by test.legalentitykey,test.idnumber
				order by newid()
			end
		else
			begin
				select top 1 test.legalentitykey,test.idnumber  
				from [2am].test.AutomationDebtCounsellingTestCases test
				where test.accountkey not in (select accountkey from test.DebtCounsellingAccounts)  
				and test.ArrearTransactionNewBalance > 500
				and efolderid is null
				and productKey = @ProductKey and isInterestOnly = @InterestOnly and test.LegalEntityCountOfAccounts = @CountofAccounts
				group by test.legalentitykey,test.idnumber 
				order by newid()
			end
end
	else
begin
			select top 1 test.legalentitykey,test.idnumber  
			from [2am].test.AutomationDebtCounsellingTestCases test
			where test.accountkey not in (select accountkey from test.DebtCounsellingAccounts)  
			and efolderid is null
			and test.LegalEntityCountOfAccounts = @CountofAccounts
			group by test.legalentitykey,test.idnumber 
			order by newid()
end
	
