using System;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using SAHL.Core.Services;
using SAHL.Core.SystemMessages;

namespace SAHL.Config.Services.Tests
{
    public class TestQueryHandler : IServiceQueryHandler<TestQuery>
    {
        public ISystemMessageCollection HandleQuery(TestQuery query)
        {
            return SystemMessageCollection.Empty();
        }
    }
}
