using SAHL.Core.Data;
using SAHL.Core.Services;
using SAHL.Core.Services.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Core.Testing.Specs
{
    public class GetApplicationPurposeQueryStatement : IServiceQuerySqlStatement<GetApplicationPurposeQuery, GetApplicationPurposeQueryResult>
    {
        public GetApplicationPurposeQueryStatement(int test)
        {

        }
        public string GetStatement()
        {
            return @"select Id, Name from [Capitec].dbo.ApplicationPurposeEnum where IsActive = 1";
        }
    }
    public class GetApplicationPurposeQueryResult
    {
        public Guid Id { get; set; }

        public string Name { get; set; }
    }
    [AuthorisedCommand(Roles = "User")]
    public class GetApplicationPurposeQuery : ServiceQuery<GetApplicationPurposeQueryResult>, ISqlServiceQuery<GetApplicationPurposeQueryResult>
    {
    }
}
