namespace SAHL.Services.Interfaces.FinancialDomain.Models
{
    public class FeeApplicationAttributesModel
    {
        public FeeApplicationAttributesModel(bool CapitaliseFees, bool StaffHomeLoan, bool QuickPayLoan, bool DiscountedInitiationFeeReturningClient, bool GEPF, bool CapitaliseInitiationFee)
        {
            this.CapitaliseFees = CapitaliseFees;
            this.StaffHomeLoan = StaffHomeLoan;
            this.QuickPayLoan = QuickPayLoan;
            this.DiscountedInitiationFeeReturningClient = DiscountedInitiationFeeReturningClient;
            this.GEPF = GEPF;
            this.CapitaliseInitiationFee = CapitaliseInitiationFee;
        }

        public bool CapitaliseFees;
        public bool StaffHomeLoan;
        public bool QuickPayLoan;
        public bool DiscountedInitiationFeeReturningClient;
        public bool GEPF;
        public bool CapitaliseInitiationFee;
    }
}