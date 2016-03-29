use [2am]
go
IF OBJECT_ID('tempdb..#leoskeys') IS NOT NULL
BEGIN
drop table #leoskeys
END
ELSE
BEGIN
create table #leoskeys (
legalentitykey int,
leoskey int,
orgstructkey int
)
END

insert into #leoskeys
select leos.legalentitykey, leos.legalentityorganisationstructureKey, os.organisationstructurekey 
from [2am].dbo.legalentityorganisationstructure leos
join [2am].dbo.organisationstructure os on leos.organisationstructurekey=os.organisationstructurekey
--not a parent of another LE
left join [2am].dbo.organisationstructure os_parent on os.organisationstructurekey=os_parent.parentkey
--not playing the role of DC
left join [2am].dbo.externalrole er on leos.legalentitykey=er.legalentitykey
where os.parentkey=4729 and os_parent.parentkey is null and er.legalentitykey is null
order by os.description desc

--remove debt counsellor details
delete from debtcounselling.debtcounsellordetail
where legalentitykey in (
select legalentitykey from #leoskeys
)
--remove leos records
delete from legalentityorganisationstructure
where legalentityorganisationstructureKey in (
select leoskey from #leoskeys
)
--org structure records
delete from organisationstructure
where organisationstructureKey in (
select orgstructkey from #leoskeys
)