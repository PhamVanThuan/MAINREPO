Use [2am]
go
UPDATE ad 
SET ad.generalStatusKey = 2
FROM dbo.UserOrganisationStructure uos
JOIN dbo.ADUser ad ON uos.ADUserKey = ad.ADUserKey
WHERE uos.GenericKey IN (30,31,32,33,34)
AND uos.GenericKeyTypeKey = 34

UPDATE ad 
SET ad.generalStatusKey = 2
FROM dbo.UserOrganisationStructure uos
JOIN dbo.ADUser ad ON uos.ADUserKey = ad.ADUserKey
WHERE adusername like 'SAHL\MasonG'

UPDATE stat
SET GeneralStatusKey = 2
FROM dbo.UserOrganisationStructure uos
JOIN dbo.UserOrganisationStructureRoundRobinStatus stat
ON uos.UserOrganisationStructureKey = stat.UserOrganisationStructureKey
WHERE uos.GenericKey IN (30,31,32,33,34)
AND uos.GenericKeyTypeKey = 34


UPDATE uos
SET GeneralStatusKey = 2
FROM dbo.UserOrganisationStructure uos
JOIN dbo.UserOrganisationStructureRoundRobinStatus stat
ON uos.UserOrganisationStructureKey = stat.UserOrganisationStructureKey
WHERE uos.GenericKey IN (30,31,32,33,34)
AND uos.GenericKeyTypeKey = 34


UPDATE ad 
SET ad.generalStatusKey = 1
FROM dbo.UserOrganisationStructure uos
JOIN dbo.ADUser ad ON uos.ADUserKey = ad.ADUserKey
WHERE uos.GenericKey IN (30,31,32,33,34)
AND uos.GenericKeyTypeKey = 34
AND ad.ADUserName IN 
(
'SAHL\PLMUser',
'SAHL\PLSUser1',
'SAHL\PLSUser2',
'SAHL\PLCUser1',
'SAHL\PLCUser3',
'SAHL\PLCAUser1',
'SAHL\PLCAUser2',
'SAHL\PLCAUser3',
'SAHL\PLCUser2',
'SAHL\PLAdmin1',
'SAHL\PLAdmin2',
'SAHL\PLAdmin3',
'SAHL\PLCUser4',
'SAHL\GIAdmin',
'SAHL\CBAdmin',
'SAHL\JMAdmin'
)


UPDATE stat
SET GeneralStatusKey = 1
FROM dbo.UserOrganisationStructure uos
JOIN dbo.UserOrganisationStructureRoundRobinStatus stat
ON uos.UserOrganisationStructureKey = stat.UserOrganisationStructureKey
JOIN dbo.ADUser ad ON uos.ADUserKey = ad.ADUserKey
WHERE uos.GenericKey IN (30,31,32,33,34)
AND uos.GenericKeyTypeKey = 34
AND ad.ADUserName IN 
(
'SAHL\PLMUser',
'SAHL\PLSUser1',
'SAHL\PLSUser2',
'SAHL\PLCUser1',
'SAHL\PLCUser3',
'SAHL\PLCAUser1',
'SAHL\PLCAUser2',
'SAHL\PLCAUser3',
'SAHL\PLCUser2',
'SAHL\PLAdmin1',
'SAHL\PLAdmin2',
'SAHL\PLAdmin3',
'SAHL\PLCUser4',
'SAHL\GIAdmin',
'SAHL\CBAdmin',
'SAHL\JMAdmin'
)


UPDATE uos
SET GeneralStatusKey = 1
FROM dbo.UserOrganisationStructure uos
JOIN dbo.UserOrganisationStructureRoundRobinStatus stat
ON uos.UserOrganisationStructureKey = stat.UserOrganisationStructureKey
JOIN dbo.ADUser ad ON uos.ADUserKey = ad.ADUserKey
WHERE uos.GenericKey IN (30,31,32,33,34)
AND uos.GenericKeyTypeKey = 34
AND ad.ADUserName IN 
(
'SAHL\PLMUser',
'SAHL\PLSUser1',
'SAHL\PLSUser2',
'SAHL\PLCUser1',
'SAHL\PLCUser3',
'SAHL\PLCAUser1',
'SAHL\PLCAUser2',
'SAHL\PLCAUser3',
'SAHL\PLCUser2',
'SAHL\PLAdmin1',
'SAHL\PLAdmin2',
'SAHL\PLAdmin3',
'SAHL\PLCUser4',
'SAHL\GIAdmin',
'SAHL\CBAdmin',
'SAHL\JMAdmin'
)
