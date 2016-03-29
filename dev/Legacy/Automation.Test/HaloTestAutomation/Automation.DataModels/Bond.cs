using System;

namespace Automation.DataModels
{
    public class Bond
    {
        public int AccountKey { get; set; }

        public int BondKey { get; set; }

        public string DeedsOffice { get; set; }

        public string Attorney { get; set; }

        public string BondRegistrationNumber { get; set; }

        public DateTime BondRegistrationDate { get; set; }

        public double BondRegistrationAmount { get; set; }

        public double BondLoanAgreementAmount { get; set; }

        public double Diff { get; set; }
    }
}