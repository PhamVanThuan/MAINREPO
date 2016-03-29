USE [2AM]
GO

UPDATE AdUser
SET GeneralStatusKey=2
WHERE AdUserKey in
(
SELECT DISTINCT(a.AdUserKey) FROM AdUser a
JOIN UserOrganisationStructure uos on a.AdUserKey=uos.AdUserKey
JOIN OrganisationStructure os on uos.OrganisationStructureKey=os.OrganisationStructureKey
where os.ParentKey=165
)
GO

UPDATE AdUser
SET GeneralStatusKey=1
WHERE AdUserKey in
(
	SELECT AdUserKey FROM AdUser 
	WHERE AdUserName in 
	(
	'SAHL\VMUser',
	'SAHL\VPUser',
	'SAHL\VPUser1',
	'SAHL\VPUser2',
	'SAHL\VPUser3',
	'SAHL\VPUser4'
	) 
)