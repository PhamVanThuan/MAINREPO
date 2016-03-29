using Common.Enums;
using System;

namespace Automation.DataModels
{
    public class ValuationImprovement : IComparable<ValuationImprovement>
    {
        public int ValuationImprovementValuationKey { get; set; }

        public int ValuationImprovementKey { get; set; }

        public int ValuationKey { get; set; }

        public ValuationImprovementTypeEnum ValuationImprovementTypeKey { get; set; }

        public string ValuationImprovementTypeDescription { get; set; }

        public DateTime? ImprovementDate { get; set; }

        public double ImprovementValue { get; set; }

        public int CompareTo(ValuationImprovement other)
        {
            throw new NotImplementedException();
        }
    }
}