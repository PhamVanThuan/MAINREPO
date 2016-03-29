USE [2AM]
GO

UPDATE AdUser
SET GeneralStatusKey=2
WHERE AdUserKey in
(
SELECT DISTINCT(a.AdUserKey) FROM AdUser a
JOIN UserOrganisationStructure uos on a.AdUserKey=uos.AdUserKey
JOIN OrganisationStructure os on uos.OrganisationStructureKey=os.OrganisationStructureKey
where os.ParentKey=1005
)
GO

UPDATE AdUser
SET GeneralStatusKey=1
WHERE AdUserKey in
(
	SELECT AdUserKey FROM AdUser 
	WHERE AdUserName in 
	(
	'SAHL\CUUser',
	'SAHL\CUUser2',
	'SAHL\CUUser3',
	'SAHL\CUUser4',
	'SAHL\CUUser5',
	'SAHL\CSUser',
	'SAHL\CSUser2',
	'SAHL\CSUser3',
	'SAHL\CMUser',
	'SAHL\CMUser1',
	'SAHL\CEUser'
	) 
)
GO

DELETE FROM AllocationMandateSetUserOrganisationStructure
WHERE UserOrganisationStructureKey in (SELECT uos.UserOrganisationStructureKey 
FROM AdUser a JOIN UserOrganisationStructure uos on a.AdUserKey=uos.AdUserKey
WHERE a.AdUserName in ('SAHL\CUUser','SAHL\CUUser2','SAHL\CUUser3',	'SAHL\CUUser4',
'SAHL\CUUser5',	'SAHL\CSUser','SAHL\CSUser2','SAHL\CSUser3')
AND uos.OrganisationStructureKey IN (1008,1007))

GO

INSERT INTO AllocationMandateSetUserOrganisationStructure 
(
AllocationMandateSetKey,
UserOrganisationStructureKey
)
		SELECT 
		CASE uos.OrganisationStructureKey 
		WHEN 1008 THEN 1 
		WHEN 1007 THEN 2 
		ELSE 0 END AS AllocationMandateSetKey,
		uos.UserOrganisationStructureKey 
		FROM AdUser a
		JOIN UserOrganisationStructure uos on a.AdUserKey=uos.AdUserKey
		WHERE a.AdUserName in 
		(
		'SAHL\CUUser',
		'SAHL\CUUser2',
		'SAHL\CUUser3',
		'SAHL\CUUser4',
		'SAHL\CUUser5',
		'SAHL\CSUser',
		'SAHL\CSUser2',
		'SAHL\CSUser3'
		)
		AND uos.OrganisationStructureKey IN (1008,1007)
