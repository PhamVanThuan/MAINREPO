using SAHL.UI.Halo.Shared.Configuration;

namespace SAHL.UI.Halo.Models.Common.BankAccount
{
    public class BankAccountChildModel : IHaloTileModel
    {
        public string AccountName { get; set; }

        public string AccountNumber { get; set; }

        public string AccountType { get; set; }

        public string Bank { get; set; }

        public string BankID { get; set; }

        public string Branch { get; set; }

        public string BranchCode { get; set; }
    }
}