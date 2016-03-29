using SAHL.Core.Data;
using SAHL.Core.Data.Models._2AM;

namespace SAHL.Services.WorkflowAssignmentDomain.Managers.Statements
{
    public class GetADUserByOrganisationStructureKeyStatement : ISqlStatement<ADUserDataModel>
    {
        public int UserOrganisationStructureKey { get; private set; }

        public GetADUserByOrganisationStructureKeyStatement(int userOrganisationStructureKey)
        {
            this.UserOrganisationStructureKey = userOrganisationStructureKey;
        }

        public string GetStatement()
        {
            return @"SELECT ad.[ADUserKey]
                          ,ad.[ADUserName]
                          ,ad.[GeneralStatusKey]
                          ,ad.[Password]
                          ,ad.[PasswordQuestion]
                          ,ad.[PasswordAnswer]
                          ,ad.[LegalEntityKey]
                      FROM [2AM].[dbo].UserOrganisationStructure uos
                      join [2AM].[dbo].AdUser ad on uos.adUserKey=ad.adUserKey
                      WHERE uos.UserOrganisationStructureKey = @UserOrganisationStructureKey";
        }
    }
}