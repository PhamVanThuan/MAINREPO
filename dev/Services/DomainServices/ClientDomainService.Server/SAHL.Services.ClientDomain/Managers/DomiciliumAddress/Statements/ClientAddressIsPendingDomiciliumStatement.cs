﻿using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data;

namespace SAHL.Services.ClientDomain.Managers.DomiciliumAddress.Statements
{
    public class ClientAddressIsPendingDomiciliumStatement : ISqlStatement<int>
    {
        public int ClientAddressKey { get; protected set; }
        public int PendingStatusKey { get; protected set; }

        public ClientAddressIsPendingDomiciliumStatement(int clientAddressKey)
        {
            this.ClientAddressKey = clientAddressKey;
            this.PendingStatusKey = (int)GeneralStatus.Pending;
        }

        public string GetStatement()
        {
            var query = @"SELECT 
                            COUNT(*) AS Total 
                        FROM
                            [2AM].[dbo].[LegalEntityDomicilium]
                        WHERE
                            [LegalEntityAddressKey] = @ClientAddressKey
                        AND
                            [GeneralStatusKey] = @PendingStatusKey";
            return query;
        }
    }
}