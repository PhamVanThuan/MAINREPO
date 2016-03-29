using SAHL.Core.Data;
using SAHL.Core.Data.Models._2AM;
using System;
using System.Linq;

namespace SAHL.Services.FrontEndTest.Managers.Statements
{
    public class SetClientAddressToInactiveStatement : ISqlStatement<LegalEntityAddressDataModel>
    {
        public int ClientAddressKey { get; protected set; }

        public SetClientAddressToInactiveStatement(int clientAddressKey)
        {
            this.ClientAddressKey = clientAddressKey;
        }

        public string GetStatement()
        {
            string query = @"update [2am].dbo.legalEntityAddress
                            set GeneralStatusKey = 2
                            where legalEntityAddressKey = @ClientAddressKey";
            return query;
        }
    }
}