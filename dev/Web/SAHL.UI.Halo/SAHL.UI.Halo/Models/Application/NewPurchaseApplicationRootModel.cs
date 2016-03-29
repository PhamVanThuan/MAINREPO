using SAHL.UI.Halo.Shared.Configuration;

namespace SAHL.UI.Halo.Models.Application
{
    public class NewPurchaseApplicationRootModel : IHaloTileModel
    {
        public int ApplicationNumber { get; set; }
        public int AccountKey { get; set; }
        public string Product { get; set; }
        public string ApplicantType { get; set; }
        public string EmploymentType { get; set; }
        public decimal HouseholdIncome { get; set; }
        public decimal PurchasePrice { get; set; }
        public decimal CashDeposit { get; set; }
        public decimal TotalFees { get; set; }
        public decimal LoanAgreementAmount { get; set; }
        public decimal PropertyValuation { get; set; }
        public decimal MarketRate { get; set; }
        public decimal EffectiveLinkRate { get; set; }
        public decimal EffectiveInterestRate { get; set; }
        public int Term { get; set; }
        public decimal MonthlyInstalment { get; set; }
        public decimal LTV { get; set; }
        public decimal PTI { get; set; }
        public string SPV { get; set; }
        public string OccupancyType { get; set; }
    }
}
