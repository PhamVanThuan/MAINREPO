use [2AM]
go
if (object_id('test.AutomationDebtCounsellingTestCases') is not null)
	begin
		drop TABLE test.AutomationDebtCounsellingTestCases
		print 'Dropped TABLE: test.AutomationDebtCounsellingTestCases'
	end

go


 
create table test.AutomationDebtCounsellingTestCases
(LegalEntityKey INT,
IDNumber varchar(13),
AccountKey int,
eStageName varchar(250),
eFolderId varchar(250),
eFolderName varchar(250),
userToDo varchar(250),
arrearTransactionNewBalance float,
productKey INT,
isInterestOnly int,
DebtCounsellingKey int,
ExpectedEndState varchar(250),
ScriptFile varchar(250),
ScriptToRun varchar(250),
LegalEntityCountOfAccounts int
)

if object_id('tempdb..#Counts') IS NOT NULL
	begin
		drop table #Counts
	end
	
		select r.legalEntityKey, count(r.accountKey) as LoanCount
		into #Counts
		from [2am].dbo.role r
		join [2am].dbo.account a on r.accountkey=a.accountkey
			and a.accountstatuskey=1
			and a.rrr_productkey in (1,2,5,6,9,11)
		where roleTypeKey = 2
		group by r.legalEntityKey
		order by 2 desc

		insert into test.AutomationDebtCounsellingTestCases
		(
		LegalEntityKey, 
		IDNumber, 
		AccountKey, 
		eStageName, 
		eFolderID, 
		efolderName, 
		userToDo, 
		arrearTransactionNewBalance, 
		productKey, 
		isInterestOnly,
		DebtCounsellingKey, 
		ExpectedEndState, 
		ScriptFile, 
		ScriptToRun, 
		LegalEntityCountOfAccounts
		)
		select 
		le.legalentitykey,
		le.idnumber,
		r.accountkey, 
		loss.eStageName,
		loss.eFolderId,
		loss.eFolderName,
		loss.UserToDo, 
		sum(isnull(arr.ArrearBalance,0)) as arrearTransactionNewBalance,
		rrr_productkey, 
		max(case isnull(fa.financialAdjustmentKey,0) when 0 then 0 else 1 end) as InterestOnly,
		0,
		'Review Notification',
		'DebtCounselling.xaml',
		'CaseCreate', 
		isnull(max(c.LoanCount),0) 
		from 
		[2am].dbo.legalentity le with (nolock)
		join [2am].dbo.role r with (nolock) on le.legalentitykey=r.legalentitykey  
			and r.generalstatuskey=1 and r.roletypekey = 2  
		join [2am].dbo.account a with (nolock) on r.accountkey=a.accountkey 
			and rrr_productkey in (1,2,5,6,9,11) 
			and a.accountStatusKey=1
		join [2am].dbo.financialService fs with (nolock) on a.AccountKey=fs.AccountKey 
			and fs.accountStatusKey=1
			and fs.parentFinancialServiceKey is null
		--check for interest only
		left join [2am].fin.financialAdjustment fa with (nolock) on fs.financialServiceKey=fa.financialServiceKey 
		and 
		(	
			fa.financialAdjustmentTypeKey = 6  
			and fa.financialAdjustmentSourceKey = 6
			and fa.financialAdjustmentStatusKey = 1
		)
		--get the latest arrears balance
		join [2am].dbo.vMortgageLoanArrearBalance arr on a.AccountKey=arr.AccountKey
		left join [2am].test.losscontroleworkcases loss with (nolock) on a.accountkey=loss.accountKey  
		left join [2am].debtcounselling.debtcounselling dc with (nolock) on a.accountkey=dc.accountkey 
			and dc.DebtCounsellingStatusKey=1
		left join #Counts c on le.legalEntityKey = c.LegalEntityKey  
		where 
		a.accountStatusKey=1 
		and a.accountkey not in (select accountkey from test.DebtCounsellingAccounts)  
		and dc.accountkey is null 
		and len(le.idnumber) = 13 
		and le.idnumber is not null
		--we want to ensure that at least one LE has an active bank account
		and exists (
		select leba.legalentitybankaccountkey from 
		[2am].dbo.role roles 
		join [2am].dbo.legalentity on roles.legalentitykey=legalentity.legalentitykey
		join [2am].dbo.legalentitybankaccount leba on legalentity.legalentitykey=leba.legalentitykey
		where roles.accountkey=r.accountkey and leba.generalstatuskey=1
		and  roles.generalstatuskey=1 and roles.roletypekey = 2
		)
		--exclude defending cancellations
		and a.accountKey not in ( 
				select distinct a.accountKey 
				from [2am].dbo.account a 
				join [2am].dbo.financialservice fs on a.accountkey=fs.accountkey
				join [2am].fin.financialAdjustment fadj on fs.financialservicekey=fadj.financialservicekey
					and fadj.financialAdjustmentSourceKey = 9 
					and fadj.financialAdjustmentTypeKey = 2
					and fadj.financialAdjustmentStatusKey = 1
				where a.accountstatuskey = 1
		)
		group by fs.accountKey,le.legalentitykey,le.idnumber, r.accountkey, loss.eStageName, loss.eFolderId, loss.eFolderName, loss.UserToDo, rrr_productkey, arr.arrearBalance 
		
		--insert personal loan test cases
		delete from #Counts
		
		insert into #Counts
		select r.legalEntityKey, count(r.accountKey) as LoanCount
		from [2am].dbo.role r
		join [2am].dbo.account a on r.accountkey=a.accountkey
			and a.accountstatuskey=1
			and a.rrr_productkey in (12)
		where roleTypeKey = 2
		group by r.legalEntityKey
		order by 2 desc
		
		insert into test.AutomationDebtCounsellingTestCases
		(
		LegalEntityKey, 
		IDNumber, 
		AccountKey, 
		eStageName, 
		eFolderID, 
		efolderName, 
		userToDo, 
		arrearTransactionNewBalance, 
		productKey, 
		isInterestOnly,
		DebtCounsellingKey, 
		ExpectedEndState, 
		ScriptFile, 
		ScriptToRun, 
		LegalEntityCountOfAccounts
		)
		select 
		le.legalentitykey,
		le.idnumber,
		fs.accountkey, 
		null,
		null,
		null,
		null, 
		0 as arrearTransactionNewBalance,
		12, 
		0 as InterestOnly,
		0,
		'',
		'',
		'', 
		isnull(max(c.LoanCount),0) 
		from 
		[2am].dbo.legalentity le with (nolock)
		join [2am].dbo.role r with (nolock) on le.legalentitykey=r.legalentitykey  
			and r.generalstatuskey=1 
			and r.roletypekey = 2  
		join [2am].dbo.account a with (nolock) on r.accountkey=a.accountkey 
			and rrr_productkey in (12) 
			and a.accountStatusKey=1
		join [2am].dbo.financialService fs with (nolock) on a.AccountKey=fs.AccountKey 
			and fs.accountStatusKey=1
			and fs.parentFinancialServiceKey is null
		left join [2am].debtcounselling.debtcounselling dc with (nolock) on a.accountkey=dc.accountkey 
			and dc.DebtCounsellingStatusKey=1
		left join #Counts c on le.legalEntityKey = c.LegalEntityKey  
		where 
		a.accountStatusKey=1 
		and dc.accountkey is null 
		and len(le.idnumber) = 13 
		and le.idnumber is not null
		group by le.legalEntityKey, le.idnumber, fs.accountKey
		

		