using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data;
using SAHL.Core.Data.Models._2AM;
using System;
using System.Linq;

namespace SAHL.Services.AddressDomain.Managers.Statements
{
    public class GetExistingActiveClientAddressStatement : ISqlStatement<LegalEntityAddressDataModel>
    {
        public int ClientKey { get; protected set; }

        public int AddressKey { get; protected set; }

        public int AddressType { get; protected set; }

        public int GeneralStatusKey { get; private set; }

        public GetExistingActiveClientAddressStatement(int clientKey, int addressKey, int addressType)
        {
            this.ClientKey = clientKey;
            this.AddressKey = addressKey;
            this.AddressType = addressType;
            this.GeneralStatusKey = (int)GeneralStatus.Active;
        }

        public string GetStatement()
        {
            string query = @"select top 1 * from [2am].dbo.LegalEntityAddress 
                            where legalEntityKey = @ClientKey 
                            and AddressKey = @AddressKey 
                            and AddressTypeKey = @AddressType 
                            and GeneralStatusKey = @GeneralStatusKey";
            return query;
        }
    }
}