USE [2AM]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF EXISTS (select * from dbo.sysobjects where id = object_id(N'test.SetupDCTestUsers') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
Begin
	Drop Procedure test.SetupDCTestUsers
	Print 'Dropped procedure test.SetupDCTestUsers'
End
Go

CREATE PROCEDURE test.SetupDCTestUsers

AS

--Set all adusers in Loss Control department node to Inactive
Update a set generalstatuskey = 2 from [2AM].DBO.organisationstructure os
JOIN [2AM].DBO.organisationstructure pos on os.parentkey = pos.organisationstructurekey and pos.description = 'loss control'
JOIN [2AM].DBO.UserOrganisationStructure uos on uos.OrganisationStructureKey=os.OrganisationStructureKey
JOIN [2AM].DBO.AdUser a on a.AdUserKey=uos.AdUserKey

Update uos set generalstatuskey = 2 from [2AM].DBO.organisationstructure os
JOIN [2AM].DBO.organisationstructure pos on os.parentkey = pos.organisationstructurekey and pos.description = 'loss control'
JOIN [2AM].DBO.UserOrganisationStructure uos on uos.OrganisationStructureKey=os.OrganisationStructureKey
JOIN [2AM].DBO.AdUser a on a.AdUserKey=uos.AdUserKey

--Set all adusers in Recoveries branch to Inactive
Update a set generalstatuskey = 2 from [2AM].DBO.organisationstructure os
JOIN [2AM].DBO.organisationstructure pos on os.parentkey = pos.organisationstructurekey and pos.description = 'recoveries'
JOIN [2AM].DBO.UserOrganisationStructure uos on uos.OrganisationStructureKey=os.OrganisationStructureKey
JOIN [2AM].DBO.AdUser a on a.AdUserKey=uos.AdUserKey

Update uos set generalstatuskey = 2 from [2AM].DBO.organisationstructure os
JOIN [2AM].DBO.organisationstructure pos on os.parentkey = pos.organisationstructurekey and pos.description = 'recoveries'
JOIN [2AM].DBO.UserOrganisationStructure uos on uos.OrganisationStructureKey=os.OrganisationStructureKey
JOIN [2AM].DBO.AdUser a on a.AdUserKey=uos.AdUserKey

--Set all test users in Loss Control department node to Active
Update a set generalstatuskey = 1 from [2AM].DBO.organisationstructure os
JOIN [2AM].DBO.organisationstructure pos on os.parentkey = pos.organisationstructurekey and pos.description = 'loss control'
JOIN [2AM].DBO.UserOrganisationStructure uos on uos.OrganisationStructureKey=os.OrganisationStructureKey
JOIN [2AM].DBO.AdUser a on a.AdUserKey=uos.AdUserKey
where a.adusername in ('SAHL\LCDUser')

Update uos set generalstatuskey = 1 from [2AM].DBO.organisationstructure os
JOIN [2AM].DBO.organisationstructure pos on os.parentkey = pos.organisationstructurekey and pos.description = 'loss control'
JOIN [2AM].DBO.UserOrganisationStructure uos on uos.OrganisationStructureKey=os.OrganisationStructureKey
JOIN [2AM].DBO.AdUser a on a.AdUserKey=uos.AdUserKey
where a.adusername in ('SAHL\LCDUser')

--Set all test users in Recoveries branch to Active
Update a set generalstatuskey = 1 from [2AM].DBO.organisationstructure os
JOIN [2AM].DBO.organisationstructure pos on os.parentkey = pos.organisationstructurekey and pos.description = 'recoveries'
JOIN [2AM].DBO.UserOrganisationStructure uos on uos.OrganisationStructureKey=os.OrganisationStructureKey
JOIN [2AM].DBO.AdUser a on a.AdUserKey=uos.AdUserKey
where a.adusername in ('SAHL\DCMUser','SAHL\DCMUser1',
'SAHL\DCAUser','SAHL\DCAUser1','SAHL\DCAUser2','SAHL\DCAUser3','SAHL\DCAUser4',
'SAHL\DCSUser','SAHL\DCSUser1','SAHL\DCCUser','SAHL\DCCUser1','SAHL\DCCUser2',
'SAHL\DCCUser3','SAHL\DCCUser4','SAHL\DCCCUser','SAHL\DCCCUser1','SAHL\DCCCUser2','SAHL\DCCCUser3',
'SAHL\DCCCUser4')

Update uos set generalstatuskey = 1 from [2AM].DBO.organisationstructure os
JOIN [2AM].DBO.organisationstructure pos on os.parentkey = pos.organisationstructurekey and pos.description = 'recoveries'
JOIN [2AM].DBO.UserOrganisationStructure uos on uos.OrganisationStructureKey=os.OrganisationStructureKey
JOIN [2AM].DBO.AdUser a on a.AdUserKey=uos.AdUserKey
where a.adusername in ('SAHL\DCMUser','SAHL\DCMUser1',
'SAHL\DCAUser','SAHL\DCAUser1','SAHL\DCAUser2','SAHL\DCAUser3','SAHL\DCAUser4',
'SAHL\DCSUser','SAHL\DCSUser1','SAHL\DCCUser','SAHL\DCCUser1','SAHL\DCCUser2','SAHL\DCCUser3',
'SAHL\DCCUser4','SAHL\DCCCUser','SAHL\DCCCUser1','SAHL\DCCCUser2','SAHL\DCCCUser3','SAHL\DCCCUser4')
