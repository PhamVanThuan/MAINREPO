﻿using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data;
using SAHL.Core.Data.Models._2AM;

namespace SAHL.Services.ClientDomain.Managers.DomiciliumAddress.Statements
{
    public class FindExistingClientPendingDomiciliumStatement : ISqlStatement<LegalEntityDomiciliumDataModel>
    {
        public int ClientKey { get; protected set; }
        public int PendingStatusKey { get; protected set; }

        public FindExistingClientPendingDomiciliumStatement(int clientKey)
        {
            this.ClientKey = clientKey;
            this.PendingStatusKey = (int)GeneralStatus.Pending;
        }

        public string GetStatement()
        {
            var query = @"SELECT 
                            led.* 
                        FROM
                            [2AM].[dbo].[LegalEntityDomicilium] led
                        JOIN
                            [2AM].[dbo].[LegalEntityAddress] lea ON led.LegalEntityAddressKey = lea.LegalEntityAddressKey
                        WHERE
                            lea.[LegalEntityKey] = @ClientKey
                        AND
                            led.[GeneralStatusKey] = @PendingStatusKey";
            return query;
        }
    }
}