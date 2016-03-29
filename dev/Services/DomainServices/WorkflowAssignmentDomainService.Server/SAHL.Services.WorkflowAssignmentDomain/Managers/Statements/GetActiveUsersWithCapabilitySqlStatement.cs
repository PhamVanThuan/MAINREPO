using SAHL.Core.Data;
using SAHL.Services.Interfaces.WorkflowAssignmentDomain.Model;

namespace SAHL.Services.WorkflowAssignmentDomain.Statements
{
    public class GetActiveUsersWithCapabilitySqlStatement : ISqlStatement<UserModel>
    {
        public int CapabilityKey { get; private set; }

        public GetActiveUsersWithCapabilitySqlStatement(int capabilityKey)
        {
            this.CapabilityKey = capabilityKey;
        }

        public string GetStatement()
        {
            var query = @"select
	                        ad.ADUserName [UserName],
	                        uosc.UserOrganisationStructureKey,
                            lg.FirstNames + ' ' + lg.Surname [FullName]
                        from [2AM].OrgStruct.UserOrganisationStructureCapability uosc
		                join [2am].dbo.UserOrganisationStructure uos on uosc.UserOrganisationStructureKey = uos.UserOrganisationStructureKey
	                    join [2am].dbo.AdUser ad on uos.ADUserKey = ad.ADUserKey
	                    join [2am].dbo.LegalEntity lg on lg.LegalEntityKey = ad.LegalEntityKey
                        where uosc.CapabilityKey = @CapabilityKey and ad.GeneralStatusKey = 1";

            return query;
        }
    }
}