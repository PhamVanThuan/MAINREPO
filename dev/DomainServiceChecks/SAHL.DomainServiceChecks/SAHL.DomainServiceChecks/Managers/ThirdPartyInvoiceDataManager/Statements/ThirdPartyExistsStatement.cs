using SAHL.Core.Data;
using SAHL.Core.Data.Models._2AM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.DomainServiceChecks.Managers.ThirdPartyInvoiceDataManager.Statements
{
    public class ThirdPartyExistsStatement : ISqlStatement<ThirdPartyDataModel>
    {
        public Guid ThirdPartyId { get; protected set; }

        public ThirdPartyExistsStatement(Guid thirdPartyId)
        {
            this.ThirdPartyId = thirdPartyId;
        }

        public string GetStatement()
        {
            var sql = @"SELECT [ThirdPartyKey],[Id],[LegalEntityKey],[IsPanel],[GeneralStatusKey],[GenericKey],[ThirdPartyTypeKey], [GenericKeyTypeKey]
                        FROM [2AM].[dbo].[ThirdParty]
                        WHERE [Id] = @thirdPartyId";
            return sql;
        }
    }
}
