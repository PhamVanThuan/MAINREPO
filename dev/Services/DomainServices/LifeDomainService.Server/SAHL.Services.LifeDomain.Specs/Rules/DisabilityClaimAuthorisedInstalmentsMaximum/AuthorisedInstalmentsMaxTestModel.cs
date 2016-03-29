using SAHL.Services.Interfaces.LifeDomain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Services.LifeDomain.Specs.RuleSpecs.DisabilityClaimAuthorisedInstalmentsMaximum
{
    public class AuthorisedInstalmentsMaxTestModel : IDisabilityClaimApproveModel
    {
        public AuthorisedInstalmentsMaxTestModel(int numberOfInstalmentsAuthorised)
        {
            this.NumberOfInstalmentsAuthorised = numberOfInstalmentsAuthorised;
        }

        public int LoanNumber { get; protected set; }

        public int NumberOfInstalmentsAuthorised { get; protected set; }

        public DateTime PaymentEndDate { get; protected set; }

        public DateTime PaymentStartDate { get; protected set; }

    }
}
