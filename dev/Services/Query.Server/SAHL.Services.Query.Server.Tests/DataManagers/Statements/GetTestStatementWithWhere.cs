using SAHL.Core.Data;
using SAHL.Services.Interfaces.Query.Models;
using SAHL.Services.Query.Server.Tests.Models;

namespace SAHL.Services.Query.Server.Tests.DataManagers.Statements
{
    public class GetTestStatementWithWhere : ISqlStatement<TestDataModel>
    {

        public string GetStatement()
        {
            return @"Select T.Id as Id, T.Description as Description, T.IsActive as IsActive From Test T Where T.IsActive = 1";
        }

    }

}