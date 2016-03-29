using System;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using SAHL.Core.Data;
using SAHL.Core.Data.Models._2AM;

namespace SAHL.Services.DomainQuery.Managers.Client.Statements
{
    public class FindClientByIdNumberStatement : ISqlStatement<LegalEntityDataModel>
    {
        public FindClientByIdNumberStatement(string idNumber)
        {
            this.IdNumber = idNumber;
        }

        public string IdNumber { get; protected set; }

        public string GetStatement()
        {
            var sqlQuery = @"SELECT * FROM [2AM].[dbo].[LegalEntity] WHERE [IDNumber] = @IdNumber";
            return sqlQuery;
        }
    }
}
