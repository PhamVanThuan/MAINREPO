/* 	******************************************************************************************** */
/* 	This will script the security on the existing loan servcing CBO for new new NBP user groups  */
/* 	******************************************************************************************** */
use [2am]
go

disable trigger ALL ON FeatureGroup
go
disable trigger ALL ON Feature
go

use [2am]
go

print 'Scripting ITStaff featuregroups'
-- give ITStaff access to all the features
insert into FeatureGroup 
select 'ITStaff', f.FeatureKey from Feature f
where not exists (select FeatureKey from [2am]..FeatureGroup fg where fg.FeatureKey = f.FeatureKey and fg.ADUserGroup = 'ITStaff')

-- remove featuregroups for 'dummy feature' 
delete from FeatureGroup where ADUserGroup = 'ITStaff' and FeatureKey = 999999
-- remove featuregroups for 'no create' calculators
delete from FeatureGroup where ADUserGroup = 'ITStaff' and FeatureKey in (
select FeatureKey from [2am]..feature f where f.ShortName in ('Application Calculator No Create','Calculators No Create'))
GO

enable trigger ALL ON FeatureGroup
go
enable trigger ALL ON Feature
go
