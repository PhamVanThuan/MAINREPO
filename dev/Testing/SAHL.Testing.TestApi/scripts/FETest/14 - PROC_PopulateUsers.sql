USE [FETest]
GO


IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'PopulateUsers')
	DROP PROCEDURE dbo.PopulateUsers
GO

CREATE PROCEDURE dbo.PopulateUsers

AS

BEGIN

IF(EXISTS(SELECT 1 FROM FETest.dbo.HaloUsers))
	BEGIN
		truncate table FETest.dbo.HaloUsers
	END

select uosc.UserOrganisationStructureKey, c.Description 
into #capabilities
from [2am].OrgStruct.UserOrganisationStructureCapability uosc
join [2am].OrgStruct.Capability c on uosc.CapabilityKey=c.CapabilityKey

insert into FETest.dbo.HaloUsers
(ADUserKey, ADUserName, LegalEntityKey, UserOrganisationStructureKey, OrgStructureDescription, Capabilities)
select a.ADUserKey, REPLACE(a.adusername, 'SAHL\', '') as ADUserName,  le.LegalEntityKey, uos.UserOrganisationStructureKey, os.pathStr, isnull(capabilities.Capability_List, '')
from [2am].dbo.UserOrganisationStructure uos
join [2am].dbo.AdUser a on uos.ADUserKey=a.ADUserKey
join [2am].dbo.LegalEntity le on a.LegalEntityKey=le.LegalEntityKey
join [2am].dbo.vOrganisationStructure os on uos.OrganisationStructureKey=os.OrganisationStructureKey
outer apply (
SELECT DISTINCT
    UserOrganisationStructureKey,
    STUFF((SELECT ',', Description as [text()]
           FROM #capabilities i 
           WHERE i.UserOrganisationStructureKey = t.UserOrganisationStructureKey
           FOR XML PATH ('')), 1, 1, '') as Capability_List
FROM
    #capabilities t
where uos.UserOrganisationStructureKey=t.UserOrganisationStructureKey
) as capabilities
where a.GeneralStatusKey=1 
and OrganisationStructure <> 'RCS'
and ADUserName not like '%Auditor%'
order by a.ADUserName

drop table #capabilities



END






