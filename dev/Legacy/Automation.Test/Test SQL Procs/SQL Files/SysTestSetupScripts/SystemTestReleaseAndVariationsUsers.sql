USE [2AM]
GO

UPDATE AdUser
SET GeneralStatusKey=2
WHERE AdUserKey in
(
SELECT DISTINCT(a.AdUserKey) FROM AdUser a
JOIN UserOrganisationStructure uos on a.AdUserKey=uos.AdUserKey
JOIN OrganisationStructure os on uos.OrganisationStructureKey=os.OrganisationStructureKey
where os.ParentKey=586
)
GO

UPDATE AdUser
SET GeneralStatusKey=1
WHERE AdUserKey in
(
	SELECT AdUserKey FROM AdUser 
	WHERE AdUserName in 
	(
	'SAHL\RVMUser',
	'SAHL\RVAUser',
	'SAHL\RVAUser1'
	) 
)