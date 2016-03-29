using SAHL.Core.Data;
using SAHL.Core.Services;
using SAHL.Core.Services.Attributes;
using SAHL.Services.Interfaces.Capitec.Models;

namespace SAHL.Services.Interfaces.Capitec.Queries
{
    [AuthorisedCommand(Roles = "User")]
    public class GetCalculatorFeeQuery : ServiceQuery<GetCalculatorFeeQueryResult>, ISqlServiceQuery<GetCalculatorFeeQueryResult>
    {
        public GetCalculatorFeeQuery(int offerType, decimal loanAmount, decimal cashOut)
        {
            this.OfferType = offerType;
            this.LoanAmount = loanAmount;
            this.CashOut = cashOut;
        }

        public int OfferType { get; protected set; }

        public decimal LoanAmount { get; protected set; }

        public decimal CashOut { get; protected set; }
    }
}