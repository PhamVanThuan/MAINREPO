using SAHL.Core.Attributes;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data;
using SAHL.Services.Interfaces.LifeDomain.Models;
using System;

namespace SAHL.Services.LifeDomain.Managers.Statements
{
    [InsertConventionExclude]
    public class LodgeDisabilityClaimStatement :  ISqlStatement<int>
    {
        public int AccountKey { get; protected set; }
        public int LegalEntityKey { get; protected set; }
        public int DisabilityClaimStatusKey { get; protected set; }

        public LodgeDisabilityClaimStatement(int lifeAccountKey, int claimantLegalEntityKey)
        {
            AccountKey = lifeAccountKey;
            LegalEntityKey = claimantLegalEntityKey;
            DisabilityClaimStatusKey = (int)DisabilityClaimStatus.Pending;
        }

        public string GetStatement()
        {
            return @"insert into [2am].[dbo].DisabilityClaim ([AccountKey],[LegalEntityKey],[DisabilityClaimStatusKey],[DateClaimReceived]) 
                        values (@AccountKey, @LegalEntityKey, @DisabilityClaimStatusKey,getdate());
                     select cast(SCOPE_IDENTITY() as int)";            
        }
    }
}