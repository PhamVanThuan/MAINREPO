using System;
namespace SAHL.Services.BankAccountDomain.Utils
{
    public interface ICdvValidationManager
    {
        bool ValidateAccountNumber(string branchCode, int accountType, string accountNumber);
    }
}
