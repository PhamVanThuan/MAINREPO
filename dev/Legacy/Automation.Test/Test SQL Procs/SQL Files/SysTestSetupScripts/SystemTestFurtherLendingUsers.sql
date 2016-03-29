USE [2AM]
GO

UPDATE AdUser
SET GeneralStatusKey=2
WHERE AdUserKey in
(
SELECT DISTINCT(a.AdUserKey) FROM AdUser a
JOIN UserOrganisationStructure uos on a.AdUserKey=uos.AdUserKey
JOIN OrganisationStructure os on uos.OrganisationStructureKey=os.OrganisationStructureKey
where os.ParentKey=172
)
GO

UPDATE AdUser
SET GeneralStatusKey=1
WHERE AdUserKey in
(
	SELECT AdUserKey FROM AdUser 
	WHERE AdUserName in 
	(
	'SAHL\FLmUser',
	'SAHL\FLSUser',
	'SAHL\FLAppProcUser',
	'SAHL\FLAppProcUser1',
	'SAHL\FLAppProcUser2',
	'SAHL\FLAppProcUser3',
	'SAHL\FLAppProcUser4',
	'SAHL\FLAppProcUser5',
	'SAHL\FLAppProcUser6',
	'SAHL\FLAppProcUser7',
	'SAHL\FLAppProcUser8',
	'SAHL\FLAppProcUser9',
	'SAHL\FLAppProcUser10'
	) 
)

--we need to reset the round robin pointers
update [2am].dbo.RoundRobinPointer
set roundrobinpointerindexid = 1
