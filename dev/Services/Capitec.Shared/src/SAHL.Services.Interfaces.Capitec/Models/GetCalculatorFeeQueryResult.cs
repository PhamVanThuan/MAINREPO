namespace SAHL.Services.Interfaces.Capitec.Models
{
    public class GetCalculatorFeeQueryResult
    {
        public decimal CancellationFee { get; set; }

        public decimal InitiationFee { get; set; }

        public decimal RegistrationFee { get; set; }

        public decimal InterimInterest { get; set; }

        public decimal BondToRegister { get; set; }
    }
}