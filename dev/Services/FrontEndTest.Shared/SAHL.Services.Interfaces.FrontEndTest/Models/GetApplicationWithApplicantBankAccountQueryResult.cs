using SAHL.Core.BusinessModel.Enums;

namespace SAHL.Services.Interfaces.FrontEndTest.Models
{
    public class GetApplicationWithApplicantBankAccountQueryResult
    {
        public int ApplicationNumber { get; set; }

        public int ClientBankAccountKey { get; set; }

        public int DebitOrderDay { get; set; }

        public FinancialServicePaymentType PaymentType { get; set; }
    }
}