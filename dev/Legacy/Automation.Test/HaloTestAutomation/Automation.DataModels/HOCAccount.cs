using System;

namespace Automation.DataModels
{
    public class HOCAccount
    {
        public int AccountKey { get; set; }

        public int FinancialServiceKey { get; set; }

        public double Payment { get; set; }

        public int GeneralStatus { get; set; }

        public int HOCInsurerKey { get; set; }

        public string InsurerDescription { get; set; }

        public DateTime CommencementDate { get; set; }

        public DateTime AnniversaryDate { get; set; }

        public int HOCStatusKey { get; set; }

        public string SAHLPolicyNumber { get; set; }

        public double HOCTotalSumInsured { get; set; }

        public string HOCRoof { get; set; }

        public string HOCConstruction { get; set; }

        public string HOCSubsidence { get; set; }

        public double HOCThatchAmount { get; set; }

        public double HOCConventionalAmount { get; set; }

        public double HOCShingleAmount { get; set; }

        public bool Ceded { get; set; }

        public DateTime ValuationDate { get; set; }

        public int PropertyKey { get; set; }

        public Automation.DataModels.Property PropertyDetails { get; set; }
    }
}