USE [2AM]
GO

declare @OrgStructureKey int

set @OrgStructureKey = 15

update ss
set GeneralStatusKey = 2, CapitecGeneralStatusKey = 2
from organisationStructure os
join OrganisationStructure os_child on os.OrganisationStructureKey = os_child.ParentKey
join userOrganisationStructure uos on os_child.OrganisationStructureKey = uos.OrganisationStructureKey
join UserOrganisationStructureRoundRobinStatus ss on uos.UserOrganisationStructureKey = ss.UserOrganisationStructureKey
join adUser ad on uos.ADUserKey = ad.ADUserKey
where os.organisationStructureKey = @OrgStructureKey
and adusername not in (
		'SAHL\TELAUser',
		'SAHL\TELCUser',
		'SAHL\TELCUser1',
		'SAHL\TELMUser',
		'SAHL\TELSUser'
	)

UPDATE ad
set GeneralStatusKey = 2
from organisationStructure os
join OrganisationStructure os_child on os.OrganisationStructureKey = os_child.ParentKey
join userOrganisationStructure uos on os_child.OrganisationStructureKey = uos.OrganisationStructureKey
join UserOrganisationStructureRoundRobinStatus ss on uos.UserOrganisationStructureKey = ss.UserOrganisationStructureKey
join adUser ad on uos.ADUserKey = ad.ADUserKey
where os.organisationStructureKey = @OrgStructureKey
and adusername not in (
		'SAHL\TELAUser',
		'SAHL\TELCUser',
		'SAHL\TELCUser1',
		'SAHL\TELMUser',
		'SAHL\TELSUser'
)

update ss
set GeneralStatusKey = 1, CapitecGeneralStatusKey = 1
from organisationStructure os
join OrganisationStructure os_child on os.OrganisationStructureKey = os_child.ParentKey
join userOrganisationStructure uos on os_child.OrganisationStructureKey = uos.OrganisationStructureKey
join UserOrganisationStructureRoundRobinStatus ss on uos.UserOrganisationStructureKey = ss.UserOrganisationStructureKey
join adUser ad on uos.ADUserKey = ad.ADUserKey
where os.organisationStructureKey = @OrgStructureKey
and adusername in (
		'SAHL\TELAUser',
		'SAHL\TELCUser',
		'SAHL\TELCUser1',
		'SAHL\TELMUser',
		'SAHL\TELSUser'
	)
