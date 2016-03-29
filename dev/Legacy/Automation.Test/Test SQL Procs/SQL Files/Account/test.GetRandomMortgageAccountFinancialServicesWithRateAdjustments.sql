
USE [2AM]

if object_id('test.GetRandomMortgageAccountFinancialServicesWithRateAdjustments') is not null
begin
	drop procedure test.GetRandomMortgageAccountFinancialServicesWithRateAdjustments
end

go
create procedure test.GetRandomMortgageAccountFinancialServicesWithRateAdjustments
as
begin
	select distinct 
			a.AccountKey,
			a.FixedPayment,
			a.AccountStatusKey,
			a.InsertedDate,
			a.OriginationSourceProductKey,
			a.OpenDate,
			a.CloseDate,
			a.RRR_ProductKey as ProductKey,
			a.RRR_OriginationSourceKey as OriginationSourceKey,
			a.UserID,
			a.ChangeDate,
			a.SPVKey,
			a.ParentAccountKey,
			fs.FinancialServiceKey,
			fs.Payment,
			fs.FinancialServiceTypeKey,
			fs.TradeKey,
			fs.CategoryKey,
			fs.AccountStatusKey as FinancialServiceStatusKey,
			fs.NextResetDate,
			fs.ParentFinancialServiceKey,
			lb.Term, 
			lb.InitialBalance, 
			lb.RemainingInstalments, 
			lb.InterestRate, 
			lb.ActiveMarketRate, 
			lb.RateAdjustment, 
			lb.MTDInterest,
			ml.CreditMatrixKey, 
			m.Value as LinkRate, 
			lb.ActiveMarketRate as MarketRate, 
			ml.mortgageLoanPurposeKey as MortgageLoanPurpose, 
			case when lb.RateAdjustment > 0 then convert(bit,1) else convert(bit,0) end as IsSurchargeRateAdjustment,
			fs.FinancialServiceKey                     
	from [2am].dbo.Account a
		join [2am].dbo.Role r on a.AccountKey = r.AccountKey and r.RoleTypeKey in (2,3)
		join financialservice fs on a.accountkey = fs.accountkey 
		join [2am].fin.loanBalance lb on fs.financialServiceKey = lb.financialServiceKey
		left join [2am].fin.mortgageLoan ml on fs.financialServiceKey = ml.financialServiceKey
		join [2am].dbo.financialServiceType ft  on fs.financialServiceTypeKey = ft.financialServiceTypeKey and ft.financialServiceGroupKey = 1
		join [2am].dbo.rateConfiguration rc  on lb.rateConfigurationKey = rc.rateConfigurationKey
		join [2am].dbo.margin m on rc.marginkey = m.marginkey
	where lb.RateAdjustment > 0 and a.accountkey not in (select accountkey from debtcounselling.debtcounselling where debtcounsellingstatuskey = 1)
	union
	select distinct 
			a.AccountKey,
			a.FixedPayment,
			a.AccountStatusKey,
			a.InsertedDate,
			a.OriginationSourceProductKey,
			a.OpenDate,
			a.CloseDate,
			a.RRR_ProductKey as ProductKey,
			a.RRR_OriginationSourceKey as OriginationSourceKey,
			a.UserID,
			a.ChangeDate,
			a.SPVKey,
			a.ParentAccountKey,
			fs.FinancialServiceKey,
			fs.Payment,
			fs.FinancialServiceTypeKey,
			fs.TradeKey,
			fs.CategoryKey,
			fs.AccountStatusKey as FinancialServiceStatusKey,
			fs.NextResetDate,
			fs.ParentFinancialServiceKey,
			lb.Term, 
			lb.InitialBalance, 
			lb.RemainingInstalments, 
			lb.InterestRate, 
			lb.ActiveMarketRate, 
			lb.RateAdjustment, 
			lb.MTDInterest,
			ml.CreditMatrixKey, 
			m.Value as LinkRate, 
			lb.ActiveMarketRate as MarketRate, 
			ml.mortgageLoanPurposeKey as MortgageLoanPurpose,
			case when lb.RateAdjustment > 0 then convert(bit,1) else convert(bit,0) end as IsSurchargeRateAdjustment,
			fs.FinancialServiceKey
	from [2am].dbo.Account a
		join [2am].dbo.Role r on a.AccountKey = r.AccountKey and r.RoleTypeKey in (2,3)
		join financialservice fs on a.accountkey = fs.accountkey 
		join [2am].fin.loanBalance lb on fs.financialServiceKey = lb.financialServiceKey
		left join [2am].fin.mortgageLoan ml on fs.financialServiceKey = ml.financialServiceKey
		join [2am].dbo.financialServiceType ft  on fs.financialServiceTypeKey = ft.financialServiceTypeKey and ft.financialServiceGroupKey = 1
		join [2am].dbo.rateConfiguration rc  on lb.rateConfigurationKey = rc.rateConfigurationKey
		join [2am].dbo.margin m on rc.marginkey = m.marginkey                        
	where lb.RateAdjustment < 0 and a.accountkey not in (select accountkey from debtcounselling.debtcounselling where debtcounsellingstatuskey = 1)
end
