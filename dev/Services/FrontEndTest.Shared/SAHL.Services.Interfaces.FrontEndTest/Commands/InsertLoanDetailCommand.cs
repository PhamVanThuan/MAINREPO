using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Services;
using System;

namespace SAHL.Services.Interfaces.FrontEndTest.Commands
{
    public class InsertLoanDetailCommand : ServiceCommand, IFrontEndTestCommand
    {
        public DetailDataModel LoanDetail { get; protected set; }

        public Guid LoanDetailId { get; protected set; }

        public InsertLoanDetailCommand(DetailDataModel loanDetail, Guid loanDetailId)
        {
            this.LoanDetail = loanDetail;
            this.LoanDetailId = loanDetailId;
        }
    }
}