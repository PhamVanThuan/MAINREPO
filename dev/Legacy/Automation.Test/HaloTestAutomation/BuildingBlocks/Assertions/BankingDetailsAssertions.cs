using BuildingBlocks.Services.Contracts;
using Common.Enums;
using NUnit.Framework;

namespace BuildingBlocks.Assertions
{
    /// <summary>
    /// Assertions related to Adding, Updating and Viewing bank details
    /// </summary>
    public static class BankingDetailsAssertions
    {
        private static readonly IBankingDetailsService bankingDetailsService;

        static BankingDetailsAssertions()
        {
            bankingDetailsService = ServiceLocator.Instance.GetService<IBankingDetailsService>();
        }

        /// <summary>
        /// Ensures that a bank account record exists
        /// </summary>
        /// <param name="bankAccount">bankAccount</param>
        public static int AssertBankAccountExists(Automation.DataModels.BankAccount bankAccount)
        {
            int bankAccountKey = 0;
            var r = bankingDetailsService.GetBankAccount(bankAccount.ACBBankDescription, bankAccount.ACBBranchCode, bankAccount.ACBTypeDescription, bankAccount.AccountNumber);
            if (r.HasResults)
                bankAccountKey = int.Parse(r.Rows(0).Column(0).Value);
            Assert.True(bankAccountKey > 0, "Bank Account was not found.");
            return bankAccountKey;
        }

        /// <summary>
        /// This assertion will check that the legal entity bank account relationship exists
        /// </summary>
        /// <param name="_legalEntityKey"></param>
        /// <param name="bankAccountKey"></param>
        /// <param name="status">status of bank account record</param>
        public static void AssertLegalEntityBankAccountByGeneralStatus(int _legalEntityKey, int bankAccountKey, GeneralStatusEnum status)
        {
            var r = bankingDetailsService.GetLegalEntityBankAccountByStatus(_legalEntityKey, bankAccountKey, status);
            Assert.True(r.HasResults, "No Legal Entity Bank Account for LEKey={0}, BankAccountKey={1} with a status of {2} was found",
                                        _legalEntityKey, bankAccountKey, (int)status);
        }
    }
}