using SAHL.Core.Data;
using SAHL.Services.Capitec.Tests.TestModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Services.Capitec.Tests.TestUIStatements
{
    public class GetFees : ISqlStatement<FeeRecord>
    {
        public string GetStatement()
        {
            return @"select * from [Capitec].[dbo].[CapitecFees]
                    order by FeeRange";
        }
    }
}
