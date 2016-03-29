USE [2AM]

if object_id('test.GetAccountInSPV') is not null
begin
	drop procedure test.GetAccountInSPV
end

go
create procedure test.GetAccountInSPV
	@productkey int,
	@parentSpvDescription varchar(max),
	@minHouseholdIncome float,
	@maxHouseholdIncome float = 30000000,
	@minltv float,
	@maxltv float,
	@mincategory int,
	@maxcategory int
as
begin
	select distinct
		acc.*,
		leEmp.confirmedincome
	from dbo.account as acc
		join dbo.financialservice as fs
			on  acc.accountkey=fs.accountkey
		join dbo.role as r
			on acc.accountkey =r.accountkey
		join dbo.legalentity as le
			on r.legalentitykey=le.legalentitykey
		join  (select 
					max(r.accountkey) as accountkey,
					sum(confirmedincome) as confirmedincome
			   from dbo.role as r
					join dbo.employment as em
						on r.legalentitykey =em.legalentitykey
			   where 
					ConfirmedIncomeFlag = 1 
					and ConfirmedEmploymentFlag = 1
					and r.roletypekey in (2,3)
					and employmentstatuskey = 1
			   group by
					accountkey
			   ) as leEmp
					on r.accountkey = leEmp.accountkey
	where 
		acc.accountstatuskey = 1 
		and rrr_originationsourcekey = 1 
		and rrr_productkey = 1
		and spvkey in (select spvkey from spv.spv where description = @parentSpvDescription)
	
end


