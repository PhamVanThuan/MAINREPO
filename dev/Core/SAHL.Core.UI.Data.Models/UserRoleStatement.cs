using SAHL.Core.Data;

namespace SAHL.Core.UI.Data.Models
{
    public class UserRoleStatement : ISqlStatement<UserRole>
    {
        public UserRoleStatement(string adUserName)
        {
            this.ADUserName = adUserName;
        }

        public string ADUserName { get; protected set; }

        public string GetStatement()
        {
            return @"SELECT os1.Description as OrganisationArea, os.Description as RoleName
                  FROM [2AM].[dbo].[UserOrganisationStructure] uos
                  inner join [2am].dbo.aduser ad on ad.aduserkey = uos.aduserkey
                  inner join [2am].dbo.[organisationstructure] os on os.organisationstructurekey = uos.organisationstructurekey
                  inner join [2am].dbo.[organisationstructure] os1 on os1.organisationstructurekey = os.parentkey
                 where
                    os.organisationtypekey = 7
                 and
                    ad.adusername = @ADUserName";
        }
    }
}