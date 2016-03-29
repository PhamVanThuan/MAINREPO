using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data;
using SAHL.Core.Data.Models._2AM;

namespace SAHL.Services.ClientDomain.Managers.DomiciliumAddress.Statements
{

    public class GetActiveClientAddressStatement : ISqlStatement<LegalEntityAddressDataModel>
    {
        public int ClientAddressKey { get; protected set; }

        public int ActiveStatusKey { get; protected set; }

        public GetActiveClientAddressStatement(int clientAddressKey)
        {
            this.ClientAddressKey = clientAddressKey;
            this.ActiveStatusKey = (int)GeneralStatus.Active;
        }

        public string GetStatement()
        {
            var sql = @"SELECT * FROM 
                            [2AM].[dbo].[LegalEntityAddress]
                        WHERE
                            [LegalEntityAddressKey] = @ClientAddressKey
                        AND
                            [GeneralStatusKey] = @ActiveStatusKey";
            return sql;
        }
    }
}