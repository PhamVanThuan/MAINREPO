using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Services.Capitec.Tests.TestModels
{
    public class FeeRecord
    {
        public int FeeRange { get; set; }
        public decimal FeeNaturalTransferDuty { get; set; }
        public decimal FeeNaturalConveyancing { get; set; }
        public decimal FeeNaturalVAT { get; set; }
        public decimal FeeLegalTransferDuty { get; set; }
        public decimal FeeLegalConveyancing { get; set; }
        public decimal FeeLegalVAT { get; set; }
        public decimal FeeBondStamps { get; set; }
        public decimal FeeBondConveyancing { get; set; }
        public decimal FeeBondVAT { get; set; }
        public decimal FeeAdmin { get; set; }
        public decimal FeeValuation { get; set; }
        public decimal FeeCancelDuty { get; set; }
        public decimal FeeCancelConveyancing { get; set; }
        public decimal FeeCancelVAT { get; set; }
        public decimal FeeFlexiSwitch { get; set; }
        public decimal FeeRCSBondConveyancing { get; set; }
        public decimal FeeRCSBondVAT { get; set; }
        public decimal FeeDeedsOffice { get; set; }
        public decimal FeeRCSBondPreparation { get; set; }
        public decimal FeeBondConveyancing80Pct { get; set; }
        public decimal FeeBondVAT80Pct { get; set; }
        public decimal FeeBondConveyancingNoFICA { get; set; }
        public decimal FeeBondNoFICAVAT { get; set; }

        public override string ToString()
        {
            return String.Format("Fee Range: {0}",this.FeeRange);
        }
    }
}
