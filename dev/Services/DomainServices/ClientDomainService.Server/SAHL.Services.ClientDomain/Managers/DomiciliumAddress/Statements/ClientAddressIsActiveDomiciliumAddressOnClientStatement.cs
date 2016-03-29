﻿using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data;

namespace SAHL.Services.ClientDomain.Managers.DomiciliumAddress.Statements
{
    public class ClientAddressIsActiveDomiciliumAddressOnClientStatement : ISqlStatement<int>
    {
        public int ClientAddressKey { get; protected set; }
        public int ClientKey { get; protected set; }
        public int ActiveStatusKey { get; protected set; }

        public ClientAddressIsActiveDomiciliumAddressOnClientStatement(int clientAddressKey, int clientKey)
        {
            this.ClientAddressKey = clientAddressKey;
            this.ClientKey = clientKey;
            this.ActiveStatusKey = (int)GeneralStatus.Active;
        }

        public string GetStatement()
        {
            var query = @"SELECT 
                            COUNT(*) AS Total 
                        FROM
                            [2AM].[dbo].[LegalEntityDomicilium] led
                        JOIN
                            [2AM].[dbo].[LegalEntityAddress] lea ON led.LegalEntityAddressKey = lea.LegalEntityAddressKey
                        WHERE
                            lea.[LegalEntityKey] = @ClientKey
                        AND
                            led.[LegalEntityAddressKey] = @ClientAddressKey
                        AND
                            led.[GeneralStatusKey] = @ActiveStatusKey";
            return query;
        }
    }
}