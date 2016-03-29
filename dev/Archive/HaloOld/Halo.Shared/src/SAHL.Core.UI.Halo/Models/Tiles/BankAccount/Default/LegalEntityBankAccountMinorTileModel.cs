using SAHL.Core.UI.Models;

namespace SAHL.Core.UI.Halo.Tiles.BankAccount.Default
{
    public class LegalEntityBankAccountMinorTileModel : ITileModel
    {
        public string AccountName { get; set; }
        public string AccountNumber { get; set; }
        public string Bank { get; set; }
        public string BranchCode { get; set; }
        public string AccountType { get; set; }
    }
}