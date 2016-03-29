USE [2AM]
GO

--Set all adusers in Loss Control department node to Inactive
Update aduser set generalstatuskey = 2 from organisationstructure os
JOIN organisationstructure pos on os.parentkey = pos.organisationstructurekey and pos.description = 'loss control'
JOIN UserOrganisationStructure uos on uos.OrganisationStructureKey=os.OrganisationStructureKey
JOIN AdUser a on a.AdUserKey=uos.AdUserKey

Update userorganisationstructure set generalstatuskey = 2 from organisationstructure os
JOIN organisationstructure pos on os.parentkey = pos.organisationstructurekey and pos.description = 'loss control'
JOIN UserOrganisationStructure uos on uos.OrganisationStructureKey=os.OrganisationStructureKey
JOIN AdUser a on a.AdUserKey=uos.AdUserKey

--Set all adusers in Recoveries branch to Inactive
Update aduser set generalstatuskey = 2 from organisationstructure os
JOIN organisationstructure pos on os.parentkey = pos.organisationstructurekey and pos.description = 'recoveries'
JOIN UserOrganisationStructure uos on uos.OrganisationStructureKey=os.OrganisationStructureKey
JOIN AdUser a on a.AdUserKey=uos.AdUserKey

Update userorganisationstructure set generalstatuskey = 2 from organisationstructure os
JOIN organisationstructure pos on os.parentkey = pos.organisationstructurekey and pos.description = 'recoveries'
JOIN UserOrganisationStructure uos on uos.OrganisationStructureKey=os.OrganisationStructureKey
JOIN AdUser a on a.AdUserKey=uos.AdUserKey

--Set all test users in Loss Control department node to Active
Update aduser set generalstatuskey = 1 from organisationstructure os
JOIN organisationstructure pos on os.parentkey = pos.organisationstructurekey and pos.description = 'loss control'
JOIN UserOrganisationStructure uos on uos.OrganisationStructureKey=os.OrganisationStructureKey
JOIN AdUser a on a.AdUserKey=uos.AdUserKey
where a.adusername in ('SAHL\LCDUser')

Update userorganisationstructure set generalstatuskey = 1 from organisationstructure os
JOIN organisationstructure pos on os.parentkey = pos.organisationstructurekey and pos.description = 'loss control'
JOIN UserOrganisationStructure uos on uos.OrganisationStructureKey=os.OrganisationStructureKey
JOIN AdUser a on a.AdUserKey=uos.AdUserKey
where a.adusername in ('SAHL\LCDUser')

--Set all test users in Recoveries branch to Active
Update aduser set generalstatuskey = 1 from organisationstructure os
JOIN organisationstructure pos on os.parentkey = pos.organisationstructurekey and pos.description = 'recoveries'
JOIN UserOrganisationStructure uos on uos.OrganisationStructureKey=os.OrganisationStructureKey
JOIN AdUser a on a.AdUserKey=uos.AdUserKey
where a.adusername in ('SAHL\DCMUser',
'SAHL\DCMUser1',
'SAHL\DCAUser',
'SAHL\DCAUser1',
'SAHL\DCAUser2',
'SAHL\DCAUser3',
'SAHL\DCAUser4',
'SAHL\DCSUser',
'SAHL\DCSUser1',
'SAHL\DCCUser',
'SAHL\DCCUser1',
'SAHL\DCCUser2',
'SAHL\DCCUser3',
'SAHL\DCCUser4',
'SAHL\DCCCUser',
'SAHL\DCCCUser1',
'SAHL\DCCCUser2',
'SAHL\DCCCUser3',
'SAHL\DCCCUser4')

Update userorganisationstructure set generalstatuskey = 1 from organisationstructure os
JOIN organisationstructure pos on os.parentkey = pos.organisationstructurekey and pos.description = 'recoveries'
JOIN UserOrganisationStructure uos on uos.OrganisationStructureKey=os.OrganisationStructureKey
JOIN AdUser a on a.AdUserKey=uos.AdUserKey
where a.adusername in ('SAHL\DCMUser',
'SAHL\DCMUser1',
'SAHL\DCAUser',
'SAHL\DCAUser1',
'SAHL\DCAUser2',
'SAHL\DCAUser3',
'SAHL\DCAUser4',
'SAHL\DCSUser',
'SAHL\DCSUser1',
'SAHL\DCCUser',
'SAHL\DCCUser1',
'SAHL\DCCUser2',
'SAHL\DCCUser3',
'SAHL\DCCUser4',
'SAHL\DCCCUser',
'SAHL\DCCCUser1',
'SAHL\DCCCUser2',
'SAHL\DCCCUser3',
'SAHL\DCCCUser4')

--we need to reset the round robin pointers
update [2am].dbo.RoundRobinPointer
set roundrobinpointerindexid = 1
