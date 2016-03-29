declare @FKeys table(FeatureKey int, ADUserGroup varchar(255));
declare @CurrADUser varchar(255);

declare  ADUserCursor cursor for
	select fg.ADUserGroup
from 
	[2am]..featuregroup fg
where 
	featurekey in 
	(
		select cm.featurekey from [2am]..corebusinessobjectmenu cm
			where
		cm.parentkey is null
	) 
	group by adusergroup order by count(featureKey) desc

open ADUserCursor

fetch next from ADUserCursor into
	@CurrADUser

while @@FETCH_STATUS = 0
begin
		insert into @FKeys
		select 
			FG.FeatureKey, @CurrADUser
		from
			FeatureGroup FG
		left outer join
			@FKeys FK
		on
			FK.FeatureKey = FG.FeatureKey
		where 
			FG.ADUserGroup = @CurrADUser
		and
			FK.FeatureKey is null
		and
			FG.featurekey in 
			(
				select cm.featurekey from [2am]..corebusinessobjectmenu cm
					where
				cm.parentkey is null
			)
			
	fetch next from ADUserCursor into
	@CurrADUser
end

close ADUserCursor
deallocate ADUserCursor

select ADUserGroup from @FKeys group by ADUserGroup; --, [2AM].dbo.CommaDelimit(FeatureKey) 

-- here we are selecting the count of all top level menus from the available adusergroups
select count(fk.featurekey) from @FKeys fk
inner join
	[2am]..corebusinessobjectmenu cm
on
	cm.featurekey = fk.featurekey
where
	cm.parentkey is null