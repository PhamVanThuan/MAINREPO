using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Services.Interfaces.LifeDomain.Models
{
    public interface IDisabilityClaimApproveModel
    {
        DateTime PaymentStartDate { get; }

        int NumberOfInstalmentsAuthorised { get; }

        DateTime PaymentEndDate { get; }

        int LoanNumber { get; }
    }
}
