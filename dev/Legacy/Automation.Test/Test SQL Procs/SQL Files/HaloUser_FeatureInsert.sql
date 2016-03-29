use [2am]
go
	-- give ITStaff access to all the features
	insert into FeatureGroup 
	select 'ITStaff', f.FeatureKey from Feature f
	where not exists (select FeatureKey from [2am]..FeatureGroup fg where fg.FeatureKey = f.FeatureKey and fg.ADUserGroup = 'ITStaff')

	-- remove featuregroups for 'dummy feature' 
	delete from FeatureGroup where ADUserGroup = 'ITStaff' and FeatureKey = 999999
	-- remove featuregroups for 'no create' calculators
	delete from FeatureGroup where ADUserGroup = 'ITStaff' and FeatureKey in (
	select FeatureKey from [2am]..feature f where f.ShortName in ('Application Calculator No Create','Calculators No Create'))
	
	
--featuregroups
if (
select count(*)
from dbo.featuregroup
where adusergroup ='itstaff'
and featurekey not in (
select featurekey from dbo.featuregroup 
where adusergroup = 'halouser'
)) > 0

begin

	insert into dbo.featureGroup
	select 'HaloUser', featurekey from dbo.featuregroup
	where adusergroup ='itstaff'
	and featurekey not in (
	select featurekey from dbo.featuregroup 
	where adusergroup = 'halouser'
	)
end

--transaction type access
if (select count(*)
from dbo.TransactionTypeDataAccess
where adcredentials = 'itstaff'
and transactiontypeKey not in (
select transactiontypeKey 
from dbo.TransactionTypeDataAccess
where adcredentials = 'halouser'
)) > 0

begin
	insert into dbo.TransactionTypeDataAccess
	select 'HaloUser',transactiontypeKey 
	from dbo.TransactionTypeDataAccess
	where adcredentials = 'itstaff'
	and transactiontypeKey not in (
	select transactiontypeKey 
	from dbo.TransactionTypeDataAccess
	where adcredentials = 'halouser'
	union all
	select 300
	union all
	select 240
	)
end