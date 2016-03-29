USE [2AM]

if object_id('test.GetApplicationsInSPVAndCategory') is not null
begin
	drop procedure test.GetApplicationsInSPVAndCategory
end

go
create procedure test.GetApplicationsInSPVAndCategory
	@productkey int,
	@parentSpvDescription varchar(max),
	@minHouseholdIncome float,
	@maxHouseholdIncome float = 30000000,
	@isAcceptedOffer bit,
	@returnTodaysCases bit,
	@minltv float,
	@maxltv float,
	@mincategory int,
	@maxcategory int
as
begin
		
	if object_id('tempdb..#OfferInfo') is not null
	begin
		drop table #OfferInfo
	end
	
	select
		max(offerinformationkey) as offerinformationkey,
		max(OfferInsertDate) as OfferInsertDate,
		max(offerinformationtypekey) as offerinformationtype
	into #OfferInfo
	from dbo.offerinformation
	group by offerkey 
	
	if (@returnTodaysCases = 1)
	begin
		delete from #OfferInfo
		where OfferInsertDate < (getdate() -1)
	end
		
	if (@isAcceptedOffer = 1)
	begin
		delete from #OfferInfo
		where offerinformationtype != 3
	end
	if (@isAcceptedOffer = 0)
	begin
		delete from #OfferInfo
		where offerinformationtype = 3
	end
	
	if object_id('tempdb..#Cases') is not null
	begin
		drop table #Cases
	end
	
	select top 5
		o.offerkey,
		oit.description as OfferInformationType,
		oivl.HouseholdIncome,
		@parentSpvDescription as SpvDescription,
		round(ltv,0) as LTV,
		p.description as Product,
		cat.categorykey,
		oat.description as OfferAttribute
	into #Cases
	from dbo.offer as o
		join dbo.offerinformation as oi
			on o.offerkey=oi.offerkey
		join dbo.offerinformationtype as oit
			on oi.offerinformationtypekey=oit.offerinformationtypekey
		join dbo.offerinformationvariableloan as oivl
			on oi.offerinformationkey=oivl.offerinformationkey
		join dbo.product as p
			on oi.productkey=p.productkey
		join dbo.category as cat
			on oivl.categorykey=cat.categorykey
		left join dbo.offerattribute as oa 
			on o.offerkey=oa.offerkey
		left join dbo.offerattributetype as oat
			on oa.offerattributetypekey=oat.offerattributetypekey
	where
		o.offerstatuskey = 1 
		and o.originationsourcekey  = 1
		and oi.offerinformationkey in (select 
											offerinformationkey 
								       from #OfferInfo 
									   where offerinformationkey = oi.offerinformationkey)
		and p.productkey = @productkey
		and cat.categorykey between @mincategory and @maxcategory
		and oivl.spvkey in 
		(
			select 
				spvkey 
			from spv.spv
			where spv.description  = @parentSpvDescription
		)
		and ltv between @minltv and @maxltv
		and oivl.householdincome between @minHouseholdIncome and @maxHouseholdIncome
	order by 1 desc
	
	if (select count(*) from #Cases) > 0
		begin
			select * from #Cases
		end
	else
		begin
			select 'No cases in ' + @parentSpvDescription
		end
end


