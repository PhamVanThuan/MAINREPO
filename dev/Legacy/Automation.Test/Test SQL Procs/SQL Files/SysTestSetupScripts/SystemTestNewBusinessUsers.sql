USE [2AM]
GO

UPDATE AdUser
SET GeneralStatusKey=2
WHERE AdUserKey in
(
SELECT DISTINCT(a.AdUserKey) FROM AdUser a
JOIN UserOrganisationStructure uos on a.AdUserKey=uos.AdUserKey
JOIN OrganisationStructure os on uos.OrganisationStructureKey=os.OrganisationStructureKey
where os.ParentKey=105
)
GO

UPDATE AdUser
SET GeneralStatusKey=1
WHERE AdUserKey in
(
	SELECT AdUserKey FROM AdUser 
	WHERE AdUserName in 
	(
	'SAHL\ResubUser',
	'SAHL\NBMUser',
	'SAHL\NBSUser',
	'SAHL\NBPUser',
	'SAHL\NBPUser1',
	'SAHL\NBPUser2',
	'SAHL\NBPUser3',
	'SAHL\NBPUser4',
	'SAHL\NBPUser5',
	'SAHL\NBPUser6',
	'SAHL\NBPUser7',
	'SAHL\NBPUser8',
	'SAHL\NBPUser9',
	'SAHL\NBPuser10'
	) 
)