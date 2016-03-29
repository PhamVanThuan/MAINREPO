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
    public class GetLinkedKeyFromGuidStatement : ISqlStatement<int>
    {
        public string GetStatement()
        {
            var query = @"SELECT
                               [LinkedKey]
                             , [GuidKey]
                        FROM
                            [2AM].[dbo].[LinkedKeys]
                        WHERE
                            [GuidKey] = @GuidKey";

            return query;
        }
    }
}
