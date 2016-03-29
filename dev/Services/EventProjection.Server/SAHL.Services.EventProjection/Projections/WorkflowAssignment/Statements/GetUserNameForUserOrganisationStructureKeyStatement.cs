using SAHL.Core.Data;

namespace SAHL.Services.EventProjection.Projections.WorkflowAssignment.Statements
{
    public class GetUserNameForUserOrganisationStructureKeyStatement : ISqlStatement<string>
    {
        public int UserOrganisationStructureKey { get; private set; }

        public GetUserNameForUserOrganisationStructureKeyStatement(int userOrganisationStructureKey)
        {
            this.UserOrganisationStructureKey = userOrganisationStructureKey;
        }

        public string GetStatement()
        {
            return @"
SELECT adu.ADUserName
FROM [2AM].dbo.UserOrganisationStructure uos
INNER JOIN [2AM].dbo.ADUser adu ON uos.ADUserKey = adu.ADUserKey
WHERE uos.UserOrganisationStructureKey = @UserOrganisationStructureKey
";
        }
    }
}