using SAHL.Core.Data;
using SAHL.Core.Data.Models._2AM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Services.DomainQuery.Managers.Client.Statements
{
    public class ClientExistsStatement : ISqlStatement<int>
    {
        public int ClientKey { get; protected set; }
        public ClientExistsStatement(int clientKey)
        {
            ClientKey = clientKey;
        }

        public string GetStatement()
        {
            var query = @"SELECT count(*) FROM [2AM].[dbo].[LegalEntity] WHERE [LegalEntityKey] = @ClientKey";
            return query;
        }
    }
}
