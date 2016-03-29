using SAHL.Core.Data;
using SAHL.Services.Interfaces.LifeDomain.Models;

namespace SAHL.Services.LifeDomain.Managers.Statements
{
    public class GetDisabilityClaimsByAccountStatement : ISqlStatement<DisabilityClaimModel>
    {
        public int AccountNumber { get; protected set; }

        public GetDisabilityClaimsByAccountStatement(int accountNumber)
        {
            AccountNumber = accountNumber;
        }

        public string GetStatement()
        {
            return @"select [DisabilityClaimKey]
      ,[AccountKey]
      ,[LegalEntityKey]
      ,[DateClaimReceived]
      ,[LastDateWorked]
      ,[DateOfDiagnosis]
      ,[ClaimantOccupation]
      ,[DisabilityTypeKey]
      ,[OtherDisabilityComments]
      ,[ExpectedReturnToWorkDate]
      ,[DisabilityClaimStatusKey]
      ,[PaymentStartDate]
      ,[NumberOfInstalmentsAuthorised]
      ,[PaymentEndDate]            
from [2am].[dbo].[DisabilityClaim]
where [AccountKey] = @AccountNumber";
        }
    }
}